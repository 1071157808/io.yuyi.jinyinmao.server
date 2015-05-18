zim.ns('zim.page.payment.bankPage', {
    create:function(d){
        var def_bank = $('#jqm-select-bank').attr('data-bank');
        var tpl = $('#jqm-tpl-banks').text();
        var html = zim.tpl(tpl, {banks:d, def_bank:def_bank});
        this.__page = zim.ui.SinglePage({
            title:"选择支付银行卡",
            html:html
        }).show().inject_page_back();
    },
    destroy:function(){
        this.__page && this.__page.hide().destroy();
    }
});
zim.ns('zim.page.payment.create', {
    init:function(){
        this.setPageData();
        this.Callbacks = $.Callbacks({memory:true});
        this.bind();
        this.render();
        this.getData();
    },
    setPageData:function(){
        var source = zim.urlQuery('source');
        if(source){zim.storage.snap("partnerSource", source);}
        var uuid = zim.urlQuery('uuid');
        if(uuid){zim.storage.snap("partnerUuid", uuid);}
        var mobile = zim.urlQuery('mobile');
        if(zim.isMobile(mobile)){zim.storage.snap("partnerMobile", mobile);}
        var count = zim.urlQuery('count');
        var product_no = zim.urlQuery('id');
        var type = zim.urlQuery('type') || "amp";
        zim.storage.snap("partnerCount", count);
        zim.storage.snap("partnerId", product_no);
        zim.storage.snap("partnerType", type);
    },
    bind:function(){
        var that = this;
        zim.action.extend({
            "select-bank":function(){
                zim.page.payment.bankPage.create(that._banks);
            },
            "selected-bank":function(){
                that.selectedBank(this);
            },
            "payment-submit":function(){
                that.submit()
            }
        });
    },
    cardSlice:function(card){
        return card.substr(card.length - 4, 4);
    },
    checkCount:function(count, minCount, maxCount){
        count = parseFloat(count) || 1;
        minCount = parseFloat(minCount) || 0;
        maxCount = parseFloat(maxCount) || 1;
        if(count > maxCount){
            return maxCount;
        } else if(count < minCount){
            return minCount;
        } else {
            return count;
        }
    },
    getBankInfo:function(name){
        var obj = {}, b = zimBanks;
        $.each(b, function(i, v){
            if(name == v.name){
                obj.limit = v.limit;
                return false;
            }
        });
        return obj;
    },
    getDefBank:function(d){
        this._banks = d;
        var defBank = null, name, obj;
        for(var i = 0, len = d.length; i < len; i++){
            name = d[i].CardBankName;
            obj = this.getBankInfo(name);
            $.extend(d[i], obj);
            if(d[i].IsDefault){
                defBank = d[i];
            }
        }
        return defBank;
    },
    selectedBank:function(o){
        var card = o.getAttribute('data-bank');
        var obj = $('#jqm-select-bank');
        var banks = this._banks;
        $.each(banks, function(i, v){
            if(v.BankCardNo == card){
                var html = '<div class="'+zim.page.getBankClass(v.CardBankName)+'">';
                html += '<span>'+v.CardBankName+'(尾号'+zim.page.payment.create.cardSlice(v.BankCardNo)+')</span>';
                html += '<em>限额单笔'+v.limit[0]+'万，单日'+v.limit[1]+'万</em></div>';
                obj.attr('data-bank', card).html(html);
                return false;
            }
        });
        zim.page.payment.bankPage.destroy();
    },
    getData:function(){
        var that = this;
        var source = zim.urlQuery('source');
        var mobile = zim.urlQuery('mobile');
        var count = zim.urlQuery('count');
        var product_no = zim.urlQuery('id');
        var type = zim.urlQuery('type') || "amp";
        var product_api = type == "amp" ? zim.api.amp.detail : zim.api.cpl.detail;
        zim.batchSync([
            {"url":zim.api.user.info},
            {"url":zim.api.user.bankcards},
            {"url":zim.params(product_api, {"productNo":product_no})}
        ], function(d, d1, d2){
            if(d.code == 401){
                zim.location("/partner/login?mobile=" + mobile);
            } else if(d.data.HasDefaultBankCard){
                var iCount = that.checkCount(count, d2.data.MinShareCount, d2.data.MaxShareCount);
                var bank = that.getDefBank(d1.data);
                that._data = {
                    Count:iCount,
                    ProductNo:product_no,
                    ProductType:type,
                    ProductUnit:d2.data.UnitPrice,
                    mobile:d.data.Cellphone
                };
                that.Callbacks.fire({card:bank, item:d2.data, count:iCount, type:type});
            } else {
                if(d.data.HasSetPaymentPassword){
                    zim.location("/partner/yilian/identity");
                } else {
                    zim.location("/partner/yilian/password");
                }
            }
        });
    },
    render:function(){
        this.Callbacks.add(function(d){
            zim.tplRender('#jqm-order-create', '#jqm-tpl-create', d);
            var btn = $('#btnPayment');
            zim.bindCallback('#isAgreement', function(obj){
                if(obj.hasClass('ui-selected')){
                    btn.removeClass('ui-btn2').addClass('ui-btn1');
                } else {
                    btn.removeClass('ui-btn1').addClass('ui-btn2');
                }
            });
        });
    },
    canSubmit:1,
    send:function(d){
        if(this._data){
            var that = this;
            d = $.extend(d, {Count:this._data.Count, ProductNo:this._data.ProductNo});
            this.canSubmit = 0;
            zim.sync({
                type:'post',
                url:zim.api.payment.Investing,
                data:d,
                success:function(d){
                    var url = zim.params("/partner/payment/success?id={ProductNo}&type={ProductType}", that._data);
                    url += url + '&code=' + that.o_code(d.OrderNo);
                    zim.location(url);
                },
                error:function(d){
                    zim.ui.alert.show(d.responseJSON.Message);
                    that.canSubmit = 1;
                }
            });
        }
    },
    o_code:function(orderNo){
        var d = this._data;
        var _ozsku = [d.ProductNo, d.ProductUnit, d.Count, d.ProductType];
        var sumPrice = d.ProductUnit * d.Count;
        var str = ["orderid=" + orderNo];
        str.push("ordertotal=" + sumPrice);
        str.push("phone=" + d.mobile);
        str.push("skulist=" + _ozsku.join(","));
        return encodeURIComponent(str.join("&"));
    },
    submit:function(){
        var isAgree = $('#isAgreement').hasClass('ui-selected');
        if(!isAgree || !this.canSubmit){ return false; }
        var pwd = $('#jqm-payment-password').val();
        if(zim.trimAll(pwd) == ""){
            zim.ui.alert.show('支付密码不能为空')
            return false;
        }
        var card = $('#jqm-select-bank').attr('data-bank');
        this.send({
            BankCardNo:card,
            PaymentPassword:pwd
        });
    }
});
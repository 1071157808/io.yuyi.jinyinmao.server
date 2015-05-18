zim.ns('zim.page.jym1118', {
    __getData:function(callback){
        var api = '/api/v1/Orders/GetActivityStatus1000';
        zim.sync({
            url:api,
            success:function(d){
                if(d && d.Status < 40){
                    zim.fire(callback, d);
                }
            },
            error:function(){}
        });
    },
    __status20:function(){
        var obj = $('#jym1118');
        var s = '<div class="jqm-wrapper">';
        s += '<a href="#" id="setPaySy" class="ui-checkbox ui-selected" style="color:#ecaf09;" data-action="checkbox"><i></i>立即使用1000元理财金</a></div>';
        obj.html(s);
        var d = zim.page.payment.create._data;
        var orderSy = zim.sy_price(d.ProductUnit * d.Count, d.Yield, d.Period);
        var otherSy = zim.sy_price(1000, d.Yield, d.Period);
        var sy = $('#jqm-payment-sy').html('(理财金收益+' + otherSy + '<samp>元</samp>)');
        zim.page.payment.create._productInfo.ActivityNo = 1000;
        zim.bindCallback('#setPaySy', function(obj){
            if(obj.hasClass('ui-selected')){
                sy.html('(理财金收益+' + otherSy + '<samp>元</samp>)');
                zim.page.payment.create._productInfo.ActivityNo = 1000;
            } else {
                sy.html('');
                delete zim.page.payment.create._productInfo.ActivityNo;
            }
        });
    },
    payment:function(){
        var that = this;
        this.__getData(function(d){
            zim.fire(that['__status'+d.Status]);
        });
    }
});
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
        this.Callbacks = $.Callbacks({memory:true});
        this.bind();
        this.render();
        this.getData();
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
        var count = zim.urlQuery('count');
        var product_no = zim.urlQuery('id');
        var type = zim.urlQuery('type') || "amp";
        var product_api = type == "amp" ? zim.api.amp.detail : zim.api.cpl.detail;
        zim.batchSync([
            {"url":zim.api.user.info},
            {"url":zim.api.user.bankcards},
            {"url":zim.params(product_api, {"productNo":product_no})}
        ], function(d, d1, d2){
            if(d.data.HasDefaultBankCard){
                var iCount = that.checkCount(count, d2.data.MinShareCount, d2.data.MaxShareCount);
                var bank = that.getDefBank(d1.data);
                that._data = {
                    Count:iCount,
                    ProductNo:product_no,
                    ProductType:type,
                    ProductUnit:d2.data.UnitPrice,
                    mobile:d.data.Cellphone,
                    Yield:d2.data.Yield,
                    Period:d2.data.Period
                };
                that._productInfo = {
                    Count:iCount,
                    ProductNo:product_no
                };
                that.Callbacks.fire({card:bank, item:d2.data, count:iCount, type:type});
            } else {
                zim.location("/payment/nopay");
            }
        });
    },
    render:function(){
        this.Callbacks.add(function(d){
            zim.tplRender('#jqm-order-create', '#jqm-tpl-create', d);
            zim.page.jym1118.payment();
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
            d = $.extend(d, this._productInfo);
            this.canSubmit = 0;
            zim.sync({
                type:'post',
                url:zim.api.payment.Investing,
                data:d,
                success:function(d){
                    var url = zim.params("/payment/success?id={ProductNo}&type={ProductType}", that._data);
                    url += url + '&code=' + that.o_code(d.OrderNo) + '&uid=' + jsMd5.hexUpper(that._data.mobile);
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
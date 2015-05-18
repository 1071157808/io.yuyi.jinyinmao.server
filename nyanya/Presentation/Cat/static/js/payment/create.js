define(['jym', 'dialog'], function(jym, dialog){
    var object = {
        init:function(){
            this.bankcards.init();
            this.limit.init();
            this.create.init();
        }
    };
    object.bankcards = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var list = $('.ctt-payment-list');
            var more = $('.payment-card-more').click(function(){
                this.style.display = 'none';
                list.find('li:hidden').show();
            });
            jym.action.extend({
                "select-bank":function(){
                    list.find('li a.current').removeClass('current');
                    var obj = $(this).addClass('current');
                    setTimeout(function(){ obj.find('input:radio').prop('checked',true); }, 10);
                }
            });
        }
    };
    object.limit = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var d = jymBanks;
            var find_limit = function(){
                var el = this;
                var name = this.getAttribute('data-bank');
                $.each(d, function(i, v){
                    if(v.name == name){
                        el.innerHTML = object.limit.tpl(v);
                        return false;
                    }
                });
            };
            $('.payment-card-limit').each(find_limit);
        },
        tpl:function(d){
            var tpl = '单笔限额'+d.limit[0]+'万，日限额'+d.limit[1]+'万';
            if(d.desc){ tpl += '（'+d.desc+'）'; }
            return tpl;
        }
    };
    object.button = {
        disabled:function(btn){
            btn.removeClass('ui-btn').addClass('ui-btn1');
        },
        enable:function(btn){
            btn.removeClass('ui-btn1').addClass('ui-btn');
        },
        status:function(btn){
            return !btn.hasClass('ui-btn1');
        }
    };
    object.create = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var btn = $('#btnPayment');
            $('#isAgreement').click(function(){
                if($(this).prop('checked')){
                    object.button.enable(btn);
                } else {
                    object.button.disabled(btn);
                }
            });
            $('#pwdPayment').focus(function(){
                jym.msgError(this, null, !1);
            });
            jym.action.extend({
                "submitPayment":function(){
                    if(object.button.status(btn) == false){ return false; }
                    object.create.submit()
                }
            });
        },
        send:function(d){
            if(typeof _productInfo == 'undefined'){ throw new Error('_productInfo is undefined.'); }
            d = $.extend(d, _productInfo);
            var btn = $('#btnPayment');
            object.button.disabled(btn);
            jym.sync({
                type:"post",
                url:jym.api.payment.Investing,
                data:d,
                success:function(data){
                    var d = {
                        id:data.OrderNo,
                        name:_productName,
                        count:_productInfo.Count,
                        unit:_productPrice,
                        price:_orderPrice,
                        time:_orderTime,
                        prdNo:_productInfo.ProductNo,
                        type:_productType
                    };
                    var param = '';
                    if(_bankName){
                        d['bank'] = _bankName;
                        param = '&bank={bank}';
                    }
                    var str = "/payment/verify?id={id}&name={name}&count={count}&unit={unit}&price={price}&time={time}&prdNo={prdNo}&type={type}" + param;
                    var url = jym.params(str, d);
                    location.href = url;
                },
                error:function(d){
                    dialog.alert.show(d.responseJSON.Message);
                    object.button.enable(btn);
                }
            });
        },
        submit:function(){
            var isAgree = $('#isAgreement').prop('checked');
            var isNotSubmit = object.button.status($('#btnPayment')) == false;
            if(!isAgree || isNotSubmit){ return false; }
            var pwd = $('#pwdPayment').val();
            if(!jym.isPayPassword(pwd)){
                jym.msgError('#pwdPayment','密码为8~18位，支持数字，字母，区分大小写');
                return false;
            }
            var cardNo = $('input[name=payment-card]:checked').attr("data-bank");
            this.send({
                BankCardNo:cardNo,
                PaymentPassword:pwd
            });
        }
    };
    object.init();
    return object;
});
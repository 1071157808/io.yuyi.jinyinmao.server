define(['jym', 'dialog'], function(jym, dialog){
    var CanSubmitStatus = true;
    var T = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "ctt-selected-bank":function(){
                    var i = $('.pay-bank-list .selected').removeClass('selected').find('i');
                    $(this).addClass('selected').append(i);
                },
                "refreshShareCount":function(){
                    T.lastCount.refresh();
                },
                "ctt-payment-submit":function(){
                    if(CanSubmitStatus){ T.submit() }
                },
                "tempChangeMobile":function(){
                    jym.load.logout(function(){
                        var url = location.href;
                        location.href = url.replace(/\&firstbuy=1/i, '');
                    });
                }
            });
            $('.MsgContext').focus(function(){
                jym.msgError(this, null, false);
                jym.msgInfo(null, false);
            });
            var checkCount = function(v){
                if(v == ""){
                    T.paymentCount.calc(0);
                } else {
                    var count = parseInt(v);
                    if(isNaN(count)) { count = T.paymentCount.safeCount(); }
                    T.paymentCount.calc(count);
                }
            };
            var n = $('#ctt-number').on('input propertychange', function(){
                checkCount(this.value);
            }).blur(function(){
                T.paymentCount.validate()
            });
            checkCount(n.val());
        },
        send:function(data){
            jym.sync({
                type:"post",
                url: jym.api.payment.Investing,
                data:data,
                success:function(){
                    T.dialog.show();
                },
                error:function(d){
                    CanSubmitStatus = true;
                    if(d.status == 401){
                        dialog.alert.show({
                            msg:'登录超时，请重新登录。',
                            callback:function(){ location.reload(); }
                        });
                    } else {
                        var str = d.responseJSON.Message;
                        str = str.replace('重置交易密码','<a style="color:#00F;" href="/user/manage/reset/payment-password-1">重置交易密码</a>');
                        jym.msgInfo(str);
                    }
                }
            });
        },
        submit:function(){
            var count = T.validate.count();
            if(count == false) return false;
            var pwd = T.validate.password();
            if(pwd == false) return false;
            if(count && pwd){
                CanSubmitStatus = false;
                var card = $('.pay-bank-list .selected').attr('data-bank');
                this.send({
                    BankCardNo:card,
                    Count:count,
                    PaymentPassword:pwd,
                    ProductNo:_productNo
                });
            }
        }
    };
    T.dialog = {
        create:jym.once(function(){
            var obj = new dialog();
            $.extend(obj, {
                init_once:jym.once(function(){
                    this.init_dialog('payment');
                })
            });
            this.obj = obj;
        }),
        tpl:function(t){
            var tpl = '<div class="dialog-payment"><div class="dialog-payment-h1">';
            tpl += '<h2>正在处理您的支付请求</h2>';
            tpl += '<p>我们会在5分钟内反馈您支付结果，您可进入<br><a href="/user">我的兴业票</a> &gt; ';
            tpl += '<a href="/user/order">我的订单</a> 查看该订单的支付状态。</p>';
            tpl += '</div><div class="dialog-payment-line"></div>';
            if(t){ tpl += '<div class="dialog-payment-password">建议您<a href="/passport/reset-password-1">设置登录密码</a></div>'; }
            tpl += '<div class="dialog-payment-btn">';
            tpl += '<button class="ui-action btn-common" data-href="/user/order">立即查看支付状态</button>';
            tpl += '</div></div>';
            return tpl;
        },
        hasPassword:function(callback){
            jym.sync({
                url:jym.api.passport.hasLoginPassword,
                data:{cellphone:_mobile},
                success:callback
            });
        },
        show:function(){
            var that = this;
            this.create();
            this.hasPassword(function(d){
                var tpl = that.tpl(!d.Result);
                that.obj.show({
                    msg:tpl, wrap:'', callback:function(){
                        location.href = "/";
                    }
                });
                $('#dialog-close-payment').hide();
                $('#dialog-payment').css({
                    width:'560px',
                    height:'320px',
                    marginLeft:'-280px',
                    marginTop:'-160px'
                });
            });
        }
    };
    T.validate = {
        count:function(){
            var count = $('#ctt-number').val();
            if(!count){
                jym.msgError('#ctt-number','请输入购买份数');
                return false;
            }
            if(parseInt(count) == 0){
                jym.msgError('#ctt-number','购买份额不能为零');
                return false;
            }
            return count;
        },
        password:function(){
            var pwd = $.trim($('#payment-password').val());
            if(!pwd){
                jym.msgError('#payment-password','交易密码不能为空');
                return false;
            }
            return pwd;
        }
    };
    T.lastCount = {
        refresh:function(){
            jym.sync({
                url:jym.api.amp.SaleProcess,
                data:{"productIdentifier":_identifier},
                success:function(d){
                    _available = d.Available;
                    T.lastCount.render(d);
                }
            });
        },
        render:function(d){
            $('#ctt-last-count').text(d.Available);
            $('#ctt-paying-count').text(d.Paying);
            T.paymentCount.validate();
        }
    };
    T.paymentCount = {
        validate:function(){
            var obj = $('#ctt-number');
            var v = $.trim(obj.val());
            if(v){
                var count = parseInt(v) || _minCount;
                count = T.paymentCount.chkCount(count);
                if(count > 0){
                    obj.val(count);
                    T.paymentCount.calc(count);
                } else {
                    obj.val('');
                    T.paymentCount.calc(0);
                }
            }
        },
        chkCount:function(count){
            if(_available > _minCount){
                var max = Math.min(_available, _maxCount);
                return count >= max ? max : count <= _minCount ? _minCount : count;
            } else {
                return _available;
            }
        },
        allCount:function(){
            var count = _available;
            if(count > _maxCount){ count = _maxCount; }
            return count;
        },
        safeCount:function(){
            var count = _available;
            if(count > _minCount){ count = _minCount; }
            return count;
        },
        calc:function(count){
            var p_pay = _detail.unit * count;
            var p_inp = jym.sy_price(p_pay, _detail.yield, _detail.period);
            $('#p-pay').text(p_pay.toFixed(2) + '元');
            $('#p-inp').text(p_inp.toFixed(2) + '元');
        }
    };
    T.init();
    return T;
});
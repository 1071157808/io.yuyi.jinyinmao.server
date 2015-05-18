zim.ns('zim.sms', {
    codeType:10,
    Cellphone:"",
    sendCode:function(){
        if(this.canSend){
            var that = this;
            var callback = function(t){
                if(t){
                    zim.ui.alert('发送成功！');
                    that.countSend();
                }
            };
            this.canSend = false;
            if(this.codeType == 10){
                this.RegSend(this.Cellphone, 10, callback)
            } else {
                this.send(this.Cellphone, this.codeType, callback);
            }
        }
    },
    _SendCode:function(mobile, type, callback){
        zim.sms.getCode({
            data:{Cellphone:mobile, Type:type},
            success:function(d){
                if(d.Successful){
                    callback(true);
                } else {
                    if(d.RemainCount <= 0){
                        zim.ui.alert('已超过今日发送次数！');
                    } else {
                        zim.ui.alert('发送失败，剩余发送次数'+d.RemainCount+'！');
                    }
                }
            }
        });
    },
    send:function(mobile, type, callback){
        var that = this;
        zim.page.checkMobile(mobile, function(data){
            if(data.Result){
                that._SendCode(mobile, type, callback);
            } else { callback(false); }
        });
    },
    RegSend:function(mobile, callback){
        var that = this;
        zim.page.checkMobile(mobile, function(data){
            if(!data.Result){
                that._SendCode(mobile, 10, callback);
            } else { callback(false); }
        });
    },
    canSend:false,
    countSend:function(){
        var that = this, el = $('a.jqm-count-back');
        var opt = {
            callback:function(count){
                el.text(count);
                if(count == 0){
                    that.canSend = true;
                    that.changeSend(el);
                    el.text('');
                }
            }
        };
        zim.countdown(opt);
    },
    changeSend:function(obj){
        if(obj.hasClass('jqm-count-send')){
            obj.removeClass('jqm-count-send');
            this.sendCode();
        } else {
            obj.addClass('jqm-count-send');
        }
    },
    getCode:function(opt){
        opt = $.extend({type:"POST", url:zim.api.sms.sendCode}, opt);
        zim.sync(opt);
    },
    verifyCode:function(opt){
        opt = $.extend({type:"POST", url:zim.api.sms.verifyCode}, opt);
        zim.sync(opt);
    }
});
zim.ns('zim.passport', {
    login:function(opt){
        opt = $.extend({type:"POST", url:zim.api.passport.SignIn}, opt);
        zim.sync(opt);
    },
    register:function(opt){
        opt = $.extend({type:"POST", url:zim.api.passport.SignUp}, opt);
        zim.sync(opt);
    },
    setPassword:function(opt){
        opt = $.extend({type:"POST", url:zim.api.passport.ResetPwd}, opt);
        zim.sync(opt);
    },
    setPaymentPassword:function(opt){
        opt = $.extend({type:"POST", url:zim.api.payment.ResetPaymentPassword}, opt);
        zim.sync(opt);
    },
    PaymentRender:function(){
        zim.user.validate(function(d){
            zim.tplRender('#jqm-payment-mobile', '#jqm-tpl-mobile', d);
        });
    },
    PaymentCodeRender:function(){
        zim.user.validate(function(d){
            zim.tplRender('#jqm-payment-code', '#jqm-tpl-code', d);
            zim.sms.countSend();
        });
    },
    CodeType:{
        "caiff":"90301"
    }
});
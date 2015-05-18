define(['jym'], function(jym){
    var resetAjax = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.ResetPaymentPassword,
            data:data,
            success:callback,
            error:function(d){
                require(['dialog'], function(dialog){
                    dialog.alert.show(d.responseJSON.Message);
                });
            }
        });
    }
    var status = {
        code:0,
        token:0,
        bankCard:0,
        idCard:0,
        password:0
    };
    var obj = {
        init:function(){ this.bind() },
        bind:function(){
            jym.action.extend({
                "reset-payment1":function(){ obj.resetPayment1(this) },
                "reset-payment2":function(){ obj.resetPayment2() },
                "reset-payment3":function(){ obj.resetPayment3() }
            });
            $('#password').focus(function(){
                jym.msgError(this,null,!1);
            }).blur(function(){
                obj.validate.password1();
            });
            $('#vpassword').focus(function(){
                jym.msgError(this,null,!1);
            }).blur(function(){
                obj.validate.password2();
            });
            $('#code').focus(function(){
                jym.msgError(this, null, false);
            }).blur(function(){
                obj.validate.code(function(){
                    $('#validBtn').removeClass('ui-btn1').addClass('ui-btn');
                });
            });
            $('#getCode').click(function(){
                var that = this, self = $(this), un = 'btn-code-disabled';
                if(self.hasClass(un)){ return false; }
                jym.sendValidateCode(_Cellphone, '30', function(d, num){
                    if(d == -1){
                        jym.msgError(that, '发送次数超过5次，请明天再试！');
                    } else if(!d){
                        jym.msgError(that, '短信发送失败！');
                    } else {
                        if(num > 0){
                            self.addClass(un).val(num + '秒后重新获取！');
                        } else {
                            self.removeClass(un).val('获取验证码');
                        }
                    }
                }, 59);
                jym.msgError(that, '如未收到验证码，请查看短信是否被拦截。');
            });
            $('#bankCard').focus(function(){ 
                jym.msgError(this,null,!1);
            }).blur(function(){
                obj.validate.bankCard();
            });
            $('#idCard').focus(function(){
                jym.msgError(this,null,!1);
            }).blur(function(){
                obj.validate.idCard();
            });
        },
        resetPayment1:function(o){
            if($(o).hasClass('ui-btn1')){ return false; }
            obj.validate.code(function(){
                location.href = '/user/reset/payment/validCard?token=' + status.token;
            });
        },
        resetPayment2:function(){
            obj.validate.bankCard();
            obj.validate.idCard();
            if(status.bankCard && status.idCard){
                var url = "/user/reset/payment/password?token={token}&bankcard={bankcard}&idcard={idcard}";
                var d = {token:_token, bankcard:status.bankCard, idcard:status.idCard};
                location.href = jym.params(url, d);
            }
        },
        resetPayment3:function(){
            obj.validate.password1();
            obj.validate.password2();
            if(status.password){
                var d = {
                    BankCardNo:_BankCard,
                    CredentialNo:_IdCard,
                    Token:_token,
                    Password:status.password
                };
                resetAjax(d, function(){
                    location.href = '/user/reset/payment/success';
                });
            }
        }
    };
    obj.validate = {
        password1:function(){
            var $pwd = $('#password'), pwd = $pwd.val();
            if($.trim(pwd) == ''){
                jym.msgError($pwd, '密码不能为空！');
                return false;
            }
            if(!jym.isPayPassword(pwd)){
                jym.msgError($pwd, '密码为8-18位字母、符号和数字组合，区分大小写！');
                return false;
            }
            jym.msgError($pwd, null, false);
        },
        password2:function(){
            status.password = 0;
            var $pwd1 = $('#password'), pwd1 = $pwd1.val();
            var $pwd2 = $('#vpassword'), pwd2 = $pwd2.val();
            if($.trim(pwd2) == ''){
                jym.msgError($pwd2, '密码不能为空！');
                return false;
            }
            if(pwd1 !== pwd2){
                jym.msgError($pwd2, '两次密码不一致！');
                return false;
            }
            jym.msgError($pwd2, null, false);
            status.password = pwd1;
        },
        bankCard:function(){
            var $bCard = $('#bankCard'), bCard = $bCard.val();
            if($.trim(bCard) == ''){
                jym.msgError($bCard, '卡号不能为空！');
                return false;
            }
            if(!jym.isBankCard(bCard)){
                jym.msgError($bCard, '卡号为15~19位银行借记卡！');
                return false;
            }
            jym.msgError($bCard, null, false);
            status.bankCard = bCard;
        },
        idCard:function(){
            var $id = $('#idCard'), id = $id.val();
            if($.trim(id) == ''){
                jym.msgError($id, '证件号不能为空！');
                return false;
            }
            jym.msgError($id, null, false);
            status.idCard = id;
        },
        code:function(codeCallback){
            status.code = 0;
            status.token = 0;
            var chkCode = function(mobile, callback){
                if(status.code && status.token){
                    callback(!0, status.token);
                } else {
                    jym.verifyCode(mobile, code, '30', callback);
                }
            };
            var $code = $('#code'), code = $.trim($code.val());
            if(code == ''){
                jym.msgError($code, '验证码不能为空！');
                return false;
            }
            chkCode(_Cellphone, function(d, token){
                if(d == -1){
                    jym.msgError($code, '验证码错误或已失效，请重新获取！');
                } else if(d){
                    jym.msgError($code, null, false);
                    status.code = code;
                    status.token = token;
                    jym.fire(codeCallback);
                } else {
                    jym.msgError($code, '验证码错误！');
                }
            });
        }
    };
    obj.init();
    return obj;
});
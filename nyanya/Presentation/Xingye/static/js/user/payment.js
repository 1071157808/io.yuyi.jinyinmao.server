define(['jym'], function(jym){
    var resetAjax = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.ResetPaymentPassword,
            data:data,
            success:callback,
            error:function(d){
                jym.msgInfo(d.responseJSON.Message);
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
                "reset-payment1":function(){ obj.resetPayment1() },
                "reset-payment2":function(){ obj.resetPayment2() }
            });
            $('#password').focus(function(){
                jym.msgError(this,null,!1);
                jym.msgInfo(null, false);
            }).blur(function(){
                obj.validate.password1();
            });
            $('#vpassword').focus(function(){
                jym.msgError(this,null,!1);
                jym.msgInfo(null, false);
            }).blur(function(){
                obj.validate.password2();
            });
            $('#code').focus(function(){
                jym.msgError(this, null, false);
                jym.msgInfo(null, false);
            }).blur(function(){
                obj.validate.ValidCode();
            });
            $('#getCode').click(function(){
                var that = this, self = $(this), un = 'btn-code-disabled';
                if(self.hasClass(un)){ return false; }
                jym.sendValidateCode(_Cellphone, '30', function(d, num){
                    if(d == -1){
                        jym.msgError('#code', '发送次数超过5次，请明天再试。');
                    } else if(!d){
                        jym.msgError('#code', '短信发送失败');
                    } else {
                        if(num > 0){
                            self.addClass(un).val(num + '秒后重新获取');
                        } else {
                            self.removeClass(un).val('获取校验码');
                        }
                    }
                }, 59);
            });
            $('#bankCard').focus(function(){ 
                jym.msgError(this,null,!1);
                jym.msgInfo(null, false);
            }).blur(function(){
                obj.validate.bankCard();
            });
            $('#idCard').focus(function(){
                jym.msgError(this,null,!1);
                jym.msgInfo(null, false);
            }).blur(function(){
                obj.validate.idCard();
            });
        },
        checkPaymentCard:function(callback){
            obj.validate.bankCard();
            obj.validate.idCard();
            if(status.bankCard && status.idCard){
                jym.sync({
                    type:'post',
                    url:jym.api.passport.validPayment,
                    data:{
                        BankCardNo:status.bankCard,
                        CredentialNo:status.idCard
                    },
                    success:function(){
                        jym.fire(callback, [status.bankCard, status.idCard]);
                    },
                    error:function(d){
                        jym.msgInfo(d.responseJSON.Message);
                    }
                });
            }
        },
        resetPayment1:function(){
            obj.checkPaymentCard(function(bankCard, idCard){
                obj.validate.code(function(token){
                    $('#validBtn').removeClass('ui-btn1').addClass('ui-btn');
                    var url = "/user/manage/reset/payment-password-2?token={token}&bankcard={bankcard}&idcard={idcard}";
                    var d = {token:token, bankcard:bankCard, idcard:idCard};
                    location.href = jym.params(url, d);
                });
            });
        },
        resetPayment2:function(){
            if(obj.validate.password1() && obj.validate.password2()){
                var d = {
                    BankCardNo:_BankCard,
                    CredentialNo:_IdCard,
                    Token:_token,
                    Password:status.password
                };
                resetAjax(d, function(){
                    location.href = '/user/manage/reset/payment-success';
                });
            }
        }
    };
    obj.validate = {
        password1:function(){
            var $pwd = $('#password'), pwd = $pwd.val();
            if($.trim(pwd) == ''){
                jym.msgError($pwd, '密码不能为空');
                return false;
            }
            if(!jym.isPayPassword(pwd)){
//                jym.msgError($pwd, '密码为6-18位字母、数字和符号(包括~!@#$%^&*_)组合，区分大小写。');
                jym.msgError($pwd, '8-18位字母、数字和符号(~!@#$%^&*_)组合，区分大小写。');
                return false;
            }
            jym.msgError($pwd, null, false);
            return true;
        },
        password2:function(){
            status.password = 0;
            var $pwd1 = $('#password'), pwd1 = $pwd1.val();
            var $pwd2 = $('#vpassword'), pwd2 = $pwd2.val();
            if($.trim(pwd2) == ''){
                jym.msgError($pwd2, '密码不能为空');
                return false;
            }
            if(pwd1 !== pwd2){
                jym.msgError($pwd2, '两次密码不一致');
                return false;
            }
            jym.msgError($pwd2, null, false);
            status.password = pwd1;
            return true;
        },
        bankCard:function(){
            var $bCard = $('#bankCard'), bCard = jym.trimAll($bCard.val());
            if(bCard == ''){
                jym.msgError($bCard, '卡号不能为空');
                return false;
            }
            if(!jym.isBankCard(bCard)){
                jym.msgError($bCard, '卡号为15~19位银行储蓄卡');
                return false;
            }
            jym.msgError($bCard, null, false);
            status.bankCard = bCard;
        },
        idCard:function(){
            var $id = $('#idCard'), id = jym.trimAll($id.val());
            if(id == ''){
                jym.msgError($id, '证件号不能为空');
                return false;
            }
            jym.msgError($id, null, false);
            status.idCard = id;
        },
        ValidCode:function(){
            status.code = 0;
            var $code = $('#code'), code = $.trim($code.val());
            if(code == ''){
                jym.msgError($code, '校验码不能为空');
                return false;
            }
            if(!jym.isVerifyCode(code)){
                jym.msgError($code, '校验码为6位数字');
                return false;
            }
            status.code = code;
        },
        code:function(codeCallback){
            if(this.ValidCode() == false){ return false; }
            status.token = 0;
            var chkCode = function(mobile, callback){
                if(status.code && status.token){
                    callback(!0, status.token);
                } else {
                    jym.verifyCode(mobile, status.code, '30', callback);
                }
            };
            chkCode(_Cellphone, function(d, token){
                if(d == -1){
                    jym.msgError('#code', '校验码错误或已失效，请重新获取。');
                } else if(d){
                    jym.msgError('#code', null, false);
                    status.token = token;
                    jym.fire(codeCallback, [token]);
                } else {
                    jym.msgError('#code', '校验码错误');
                }
            });
        }
    };
    obj.init();
    return obj;
});
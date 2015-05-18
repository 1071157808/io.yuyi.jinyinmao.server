zim.action.extend({
    "login-submit":function(){
        var name = $.trim($('#jqm-login-name').val());
        if(!name){
            zim.ui.alert('手机号码不能为空！');
            return false;
        }
        if(!zim.isMobile(name)){
            zim.ui.alert('手机号码格式错误！');
            return false;
        }
        var pwd = $.trim($('#jqm-login-pass').val());
        if(!pwd){
            zim.ui.alert('密码不能为空！');
            return false;
        }
        if(!zim.isPassword(pwd)){
            zim.ui.alert('密码至少6位，不能包含空格！');
            return false;
        }
        zim.passport.login({
            data:{Name:name,Password:pwd},
            success:function(d){
                if(d.Successful){
                    zim.OCode.login(jsMd5.hexUpper(name));
                    var url = zim.urlQuery('backUrl');
                    setTimeout(function(){
                        if(zim.page.isSafeUrl(url)){
                            zim.location(url);
                        } else {
                            zim.location('/user/index');
                        }
                    }, 200);
                } else if(!d.UserExist){
                    zim.ui.alert('该用户不存在！');
                } else if(d.Lock){
                    zim.ui.alert('账号已锁定，请明日再试！');
                } else {
                    zim.ui.alert('用户名或者密码错误，还可重试'+d.RemainCount+'次！');
                }
            }
        });
    }
});
zim.action.extend({
    "user-register-sent":function(){
        if(zim.sms.canSend){
            zim.sms.codeType = 10;
            zim.sms.Cellphone = zim.urlQuery('mobile');
            zim.sms.changeSend($(this));
        }
    },
    "user-register1":function(){
        var mobile = $.trim($('#jqm-register-mobile').val());
        if(!mobile){
            zim.ui.alert('手机号码不能为空！');
            return false;
        }
        if(!zim.isMobile(mobile)){
            zim.ui.alert('手机号码格式错误！');
            return false;
        }
        zim.sms.RegSend(mobile, function(t){
            if(t){
                zim.ui.alert('短信验证码已发送至您的手机！', function(){
                    zim.location("/passport/register/code?mobile="+mobile);
                });
            } else {
                zim.ui.alert('该手机号码已存在！');
            }
        });
    },
    "user-register2":function(){
        var code = $.trim($('#jqm-register-code').val());
        if(!code){
            zim.ui.alert('验证码不能为空！');
            return false;
        }
        if(!zim.isVerifyCode(code)){
            zim.ui.alert('验证码格式错误！');
            return false;
        }
        var mobile = zim.urlQuery('mobile');
        zim.sms.verifyCode({
            data:{Cellphone:mobile, Code:code, Type:10},
            success:function(d){
                if(d.Successful){
                    zim.location("/passport/register/password?token="+d.Token+"&mobile="+mobile);
                } else {
                    zim.ui.alert('验证码错误！');
                }
            }
        });
    },
    "user-register3":function(){
        var pwd1 = $('#jqm-register-password1').val();
        var pwd2 = $('#jqm-register-password2').val();
        if(!pwd1){
            zim.ui.alert('密码不能为空！');
            return false;
        }
        if(pwd1 != zim.trimAll(pwd1)){
            zim.ui.alert('密码不能包含空格！');
            return false;
        }
        if(!zim.isPassword(pwd1)){
            zim.ui.alert('密码为6~18位字符、数字或字母，区分大小写！');
            return false;
        }
        if(pwd2 !== pwd1){
            zim.ui.alert('两次密码不一致！');
            return false;
        }
        var token = zim.urlQuery('token');
        var mobile = zim.urlQuery('mobile');
        zim.passport.register({
            data:{Password:pwd1, Token:token, Code:'903'},
            success:function(){
                zim.OCode.register(jsMd5.hexUpper(mobile));
                zim.ui.confirm({
                    msg:'注册成功！<br>是否进入个人中心？',
                    btnCancel:'返回主页',
                    callback:function(t){
                        zim.location(t?'/user/index':'/');
                    }
                });
            },
            error:function(d){
                zim.ui.message(d.responseJSON.Message);
            }
        });
    }
});
zim.action.extend({
    "reset-user-sent":function(){
        if(zim.sms.canSend){
            zim.sms.codeType = 20;
            zim.sms.Cellphone = zim.urlQuery('mobile');
            zim.sms.changeSend($(this));
        }
    },
    "reset-user-pwd1":function(){
        var mobile = $.trim($('#jqm-forgot-mobile').val());
        if(!mobile){
            zim.ui.alert('手机号码不能为空！');
            return false;
        }
        if(!zim.isMobile(mobile)){
            zim.ui.alert('手机号码格式错误！');
            return false;
        }
        zim.sms.send(mobile, 20, function(t){
            if(t){
                zim.ui.alert('短信验证码已发送至您的手机！', function(){
                    zim.location("/passport/reset/code?mobile="+mobile);
                });
            } else {
                zim.ui.alert('该手机号码不存在！');
            }
        });
    },
    "reset-user-pwd2":function(){
        var code = $.trim($('#jqm-forgot-code').val());
        if(!code){
            zim.ui.alert('验证码不能为空！');
            return false;
        }
        if(!zim.isVerifyCode(code)){
            zim.ui.alert('验证码格式错误！');
            return false;
        }
        var mobile = zim.urlQuery('mobile');
        zim.sms.verifyCode({
            data:{Cellphone:mobile, Code:code, Type:20},
            success:function(d){
                if(d.Successful){
                    zim.location("/passport/reset/password?token="+d.Token);
                } else {
                    zim.ui.alert('验证码错误！');
                }
            }
        });
    },
    "reset-user-pwd3":function(){
        var pwd1 = $('#jqm-forgot-password1').val();
        var pwd2 = $('#jqm-forgot-password2').val();
        if(!pwd1){
            zim.ui.alert('密码不能为空！');
            return false;
        }
        if(pwd1 != zim.trimAll(pwd1)){
            zim.ui.alert('密码不能包含空格！');
            return false;
        }
        if(!zim.isPassword(pwd1)){
            zim.ui.alert('密码为6~18位字符、数字或字母，区分大小写！');
            return false;
        }
        if(pwd2 !== pwd1){
            zim.ui.alert('两次密码不一致！');
            return false;
        }
        var token = zim.urlQuery('token');
        zim.passport.setPassword({
            data:{Password:pwd1, Token:token},
            success:function(){
                zim.ui.confirm({
                    msg:'修改密码成功！<br>是否立即登录？',
                    btnCancel:'返回主页',
                    callback:function(t){
                        zim.location(t?'/passport/login':'/');
                    }
                });
            },
            error:function(d){
                zim.ui.message(d.responseJSON.Message);
            }
        });
    }
});
zim.action.extend({
    "user-payment-sent":function(){
        if(zim.sms.canSend){
            var obj = $(this);
            zim.user.validate(function(d){
                zim.sms.codeType = 30;
                zim.sms.Cellphone = d.Cellphone;
                zim.sms.changeSend(obj);
            });
        }
    },
    "reset-payment-pwd1":function(){
        zim.user.validate(function(d){
            zim.sms.send(d.Cellphone, 30, function(t){
                if(t){
                    zim.ui.alert('短信验证码已发送至您的手机！', function(){
                        zim.location("/passport/payment/code");
                    });
                } else {
                    zim.ui.alert('该手机号码不存在！');
                }
            });
        });
    },
    "reset-payment-pwd2":function(){
        var card = $.trim($('#jqm-bank-card').val());
        if(!card){
            zim.ui.alert('卡号不能为空！');
            return false;
        }
        if(!zim.isBankCard(card)){
            zim.ui.alert('卡号为15-19位银行卡！');
            return false;
        }
        var id = $.trim($('#jqm-id-card').val());
        if(!id){
            zim.ui.alert('证件号不能为空！');
            return false;
        }
        var code = $.trim($('#jqm-valid-code').val());
        if(!code){
            zim.ui.alert('验证码不能为空！');
            return false;
        }
        if(!zim.isVerifyCode(code)){
            zim.ui.alert('验证码格式错误！');
            return false;
        }
        var mobile = zim.urlQuery('mobile');
        zim.user.validate(function(data){
            zim.sms.verifyCode({
                data:{Cellphone:data.Cellphone, Code:code, Type:30},
                success:function(d){
                    if(d.Successful){
                        var p = zim.params("?token={Token}&id={idCard}&card={BankCard}", {
                            Token:d.Token,
                            idCard:id,
                            BankCard:card
                        });
                        zim.location("/passport/payment/password"+p);
                    } else {
                        zim.ui.alert('验证码错误！');
                    }
                }
            });
        });
    },
    "reset-payment-pwd3":function(){
        var pwd1 = $('#jqm-payment-password1').val();
        var pwd2 = $('#jqm-payment-password2').val();
        if(!pwd1){
            zim.ui.alert('密码不能为空！');
            return false;
        }
        if(pwd1 != zim.trimAll(pwd1)){
            zim.ui.alert('密码不能包含空格！');
            return false;
        }
        if(!zim.isPayPassword(pwd1)){
            zim.ui.alert('密码为8~18位字符、数字和字母，区分大小写！');
            return false;
        }
        if(pwd1 != pwd2){
            zim.ui.alert('两次密码不一致！');
            return false;
        }
        var token = zim.urlQuery('token');
        var ident = zim.urlQuery('id');
        var card = zim.urlQuery('card');
        var callback = function(){
            zim.location('/user/index');
        };
        zim.passport.setPaymentPassword({
            data:{Password:pwd1, Token:token, BankCardNo:card, CredentialNo:ident},
            success:function(){
                zim.ui.message('密码重置成功<br>请保护好您的新密码哦！', callback);
            },
            error:function(e){
                zim.ui.message(e.responseJSON.Message);
            }
        });
    }
});
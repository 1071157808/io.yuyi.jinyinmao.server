define(['jym', 'dialog'], function(jym, dialog){
    jym.action.extend({
        "auth-login-checked":function(){
            T.switchLogin()
        }
    });
    var AuthMode = 0;//0:验证短信登录,1:支付密码登录,2:密码登录
    var MobileData = 0;
    var T = {
        init:function(){
            this.bind();
            this.authCookie();
        },
        switchLogin:function(){
            switch(AuthMode){
                case 0: T.AuthGuest.login();break;
                case 1: T.AuthPayment.login();break;
                case 2: T.AuthPassword.login();break;
                default: break;
            }
        },
        XYMobile:function(m){
            if(m === undefined){
                return jym.cookie('xy_auth_mobile');
            } else {
                jym.cookie('xy_auth_mobile', m, {expires:3650});
            }
        },
        authCookie:function(){
            var mobile = this.XYMobile();
            if(mobile && jym.isMobile(mobile)){
                $('#auth-mobile').val(mobile);
                T.authMobile(mobile);
            } else {
                mobile = $('#auth-mobile').val();
                if(jym.isMobile(mobile)){
                    T.authMobile(mobile);
                }
            }
        },
        bind:function(){
            var keyCallback = function(e){
                if(e.keyCode == 13){ T.switchLogin() }
            };
            $('#auth-password').on('keydown', keyCallback);
            $('#auth-code').on('keydown', keyCallback);
            $('#auth-mobile').on('input propertychange', function(){
                var mobile = this.value;
                MobileData = 0;
                if(mobile.length == 11 && jym.isMobile(mobile)){
                    T.authMobile(mobile);
                }
            });
            $('.MsgContext').focus(function(){
                jym.msgError(this, null, false);
                jym.msgInfo(null, false);
            });
            var isCanResendCode = true;
            jym.action.extend({
                "send-auth-code":function(){
                    if(!isCanResendCode){ return; }
                    var obj = this;
                    var mobile = T.AuthGuest.Valid.mobile();
                    if(mobile){
                        T.AuthGuest.SendCode(mobile, function(t){
                            if(t){
                                isCanResendCode = false;
                                obj.setAttribute('style','background:#CCC;');
                                jym.countdown({
                                    count:60,
                                    callback:function(i){
                                        if(!i){
                                            isCanResendCode = true;
                                            obj.setAttribute('style','');
                                            obj.value = '重新获取';
                                        } else {
                                            obj.value = '重新获取('+i+')';
                                        }
                                    }
                                });
                            }
                        });
                    }
                },
                "reset-password":function(){
                    var mobile = $('#auth-mobile').val();
                    var url = this.getAttribute('href');
                    if(jym.isMobile(mobile)){
                        url += (url.indexOf('?') > -1 ? "&" : "?") + "mobile=" + mobile;
                    }
                    location.href = url;
                }
            });
        },
        authMobile:function(m){
            jym.sync({
                url:jym.api.passport.CheckCellphone,
                data:{cellphone:m},
                success:function(d){
                    MobileData = m;
                    var obj1 = $('#password-wrap'), obj2 = $('#verifyCode-wrap');
                    var forgot = $('#forgot-password');
                    if(d.Result){
                        obj1.show();
                        obj2.hide();
                        T.AuthPassword.hasPassword(m, function(dd){
                            if(dd.Result){
                                AuthMode = 2;
                                obj1.find('.lab').text('登录密码：');
                                forgot.text('忘记密码？').attr('href','/passport/reset-password-1');
                                if(ISCATVALID && !jym.isXYUser(d.UserType)){
                                    var txt = "请使用金银猫账户的密码登录";
                                    $('#jymMsg').html(txt).show();
                                } else {
                                    $('#jymMsg').html("").hide();
                                }
                            } else {
                                AuthMode = 1;
                                obj1.find('.lab').text('交易密码：');
                                forgot.text('忘记交易密码？').attr('href','/passport/reset-payment-1');
                            }
                        });
                    } else {
                        AuthMode = 0;
                        obj2.show();
                        obj1.hide();
                    }
                },
                error:function(){
                    MobileData = 0;
                    jym.msgError('#auth-mobile', '手机号无效');
                }
            });
        }
    };
    T.AuthGuest = {
        SendCode:function(m, callback){
            jym.sync({
                type:"post",
                url:jym.api.sms.sendValidateCode,
                data:{Cellphone:m, Type:40},
                success:function(d){
                    if(d.RemainCount > 0){
                        jym.fire(callback, [true]);
                    } else {
                        jym.msgError('#auth-code','发送次数超过5次，请明天再试。');
                        jym.fire(callback, [false]);
                    }
                },
                error:function(){
                    jym.msgError('#auth-code','短信发送失败');
                    jym.fire(callback, [false]);
                }
            });
        },
        ValidCode:function(data, callback){
            jym.sync({
                type:"post",
                url:jym.api.sms.checkValidateCode,
                data:data,
                success:function(d){
                    jym.fire(callback, [d]);
                },
                error:function(){
                    jym.fire(callback, [{Successful:false}]);
                }
            });
        },
        login:function(){
            var mob = this.Valid.mobile();
            if(mob == false){ return; }
            var code = this.Valid.code();
            if(code == false){ return; }
            if(mob && code){
                var data = {Cellphone:mob, Code:code, Type:40};
                this.ValidCode(data, function(d1){
                    if(d1.Successful){
                        jym.sync({
                            type:"post",
                            url:jym.api.passport.tempSignUp,
                            data:{Token:d1.Token},
                            success:function(){
                                T.XYMobile(mob);
                                location.reload();
                            },
                            error:function(d2){
                                jym.msgInfo(d2.responseJSON.Message);
                            }
                        });
                    } else {
                        jym.msgError('#auth-code','校验码已失效');

                    }
                });
            }
        },
        Valid:{
            mobile:function(){
                var mob = $.trim($('#auth-mobile').val());
                if(!mob){
                    jym.msgError('#auth-mobile','手机号不能为空');
                    return false;
                }
                if(!jym.isMobile(mob)){
                    jym.msgError('#auth-mobile','手机号格式错误');
                    return false;
                }
                if(mob !== MobileData){
                    jym.msgError('#auth-mobile','手机号无效');
                    return false;
                }
                return mob;
            },
            code:function(){
                var $code = $('#auth-code'), code = $.trim($code.val());
                if(!code){
                    jym.msgError($code,'校验码不能为空');
                    return false;
                }
                if(!jym.isVerifyCode(code)){
                    jym.msgError($code,'校验码为6位数字');
                    return false;
                }
                return code;
            }
        }
    };
    var AuthLogin = function(callback){
        return function(){
            var mob = this.Valid.mobile();
            if(mob == false){ return; }
            var pwd = this.Valid.password();
            if(pwd == false){ return; }
            if(mob && pwd){
                jym.sync({
                    type:"post",
                    url:jym.api.passport.SignIn,
                    data:{Name:mob, Password:pwd},
                    success:function(d){
                        if(d.Successful){
                            T.XYMobile(mob);
                            location.reload();
                        } else if(d.Lock){
                            //jym.msgInfo('账户名和密码错误超过5次，您的账户已锁定，请找回您的登录密码或明天再试。');
                            jym.fire(callback, [mob, d]);
                        } else {
                            jym.msgInfo('账户名或密码错误，请重新输入。');
                        }
                    },
                    error:function(d){
                        jym.msgInfo(d.responseJSON.Message);
                    }
                });
            }
        };
    };
    T.AuthPayment = {
        login:AuthLogin(function(mob){
            var url = '/passport/reset-payment-1?mobile=' + mob;
            jym.msgInfo('账户名和密码错误超过5次，您的账户已锁定，请<a href="'+url+'" style="color:#00F;">找回您的交易密码</a>或明天再试。');
        }),
        Valid:{
            mobile:T.AuthGuest.Valid.mobile,
            password:function(){
                var pwd = $.trim($('#auth-password').val());
                if(!pwd){
                    jym.msgError('#auth-password', '登录密码不能为空');
                    return false;
                }
                if(!jym.isPayPassword(pwd)){
                    jym.msgInfo('账户名或密码错误，请重新输入。');
                    return false;
                }
                return pwd;
            }
        }
    };
    T.AuthPassword = {
        hasPassword:function(m, callback){
            jym.sync({
                url:jym.api.passport.hasLoginPassword,
                data:{cellphone:m},
                success:callback
            });
        },
        login:AuthLogin(function(mob){
            var url = '/passport/reset-password-1?mobile=' + mob;
            jym.msgInfo('账户名和密码错误超过5次，您的账户已锁定，请<a href="'+url+'" style="color:#00F;">找回您的登录密码</a>或明天再试。');
        }),
        Valid:{
            mobile:T.AuthGuest.Valid.mobile,
            password:function(){
                var pwd = $.trim($('#auth-password').val());
                if(!pwd){
                    jym.msgError('#auth-password','登录密码不能为空');
                    return false;
                }
                if(!jym.isPassword(pwd)){
                    jym.msgInfo('账户名或密码错误，请重新输入。');
                    return false;
                }
                return pwd;
            }
        }
    };
    T.init();
    return T;
});
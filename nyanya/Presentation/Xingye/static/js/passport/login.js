define(['jym'], function(jym){
    var status = {
        mobile : 0,
        password : 0
    }
    var loginAjax = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.SignIn,
            data:data,
            success:callback,
            error:function(d){
                msgWarn(d.responseJSON.Message);
            }
        });
    };
    var msgError = function(jq, msg, method){
        method = method || 'show';
        jq.siblings('.login-error').removeClass('login-ok')[method]();
        jq.siblings('.login-info').html(msg)[method]();
    };
    var msgCorrect = function(jq){
        jq.siblings('.login-error').addClass('login-ok').show();
        jq.siblings('.login-info').html('').hide();
    };
    var msgWarn = function(msg, visible){
        var jq = $('#loginInfo');
        if(visible != false){
            jymMsg(null);
            jq.html('<div class="ui-warning"><div class="ui-error">' + msg + '</div></div>').show();
        } else {
            jq.hide();
        }
    }
    var jymMsg = function(msg){
        var jq = $('#jymMsg');
        if(!msg){
            jq.html('');
        } else {
            msgWarn(null, false);
            jq.html('<div class="ui-warning"><div class="ui-tips">'+msg+'</div></div>');
        }
    };
    var obj = {
        init : function(){
            this.bind();
            this.AuthRemember();
        },
        XYMobile:function(m){
            if(m === undefined){
                return jym.cookie('xy_auth_mobile');
            } else {
                jym.cookie('xy_auth_mobile', m, {expires:3650});
            }
        },
        AuthRemember:function(){
            var mobile = obj.XYMobile();
            if(mobile && jym.isMobile(mobile)){
                $('#username').val(mobile);
                obj.validate.mobile();
            } else {
                mobile = $('#username').val();
                if(jym.isMobile(mobile)){
                    obj.validate.mobile();
                }
            }
        },
        bind : function(){
            jym.action.extend({
                "login-submit":function(){
                    obj.login();
                },
                "forgotPwd":function(){
                    obj.find(this);
                }
            });
            $('#username').focus(function(){
                jym.msgError($(this), '', 0);
                msgWarn('', 0);
            }).blur(function(){
                obj.validate.mobile();
            }).keydown(function(e){
                if(e.keyCode == 13){
                    e.preventDefault();
                    obj.login();
                }
            });
            $('#password').focus(function(){
                jym.msgError($(this), '', 0);
                msgWarn('', 0);
            }).blur(function(){
                obj.validate.password();
            }).keydown(function(e){
                if(e.keyCode == 13){
                    e.preventDefault();
                    obj.login();
                }
            });
        },
        login : function(){
            obj.validate.status(function(){
                var pwd = obj.validate.password();
                if(pwd == false){ return; }
                var data = {
                    Name:status.mobile,
                    Password:pwd
                };
                loginAjax(data, function(d){
                    if(d.Successful){
                        obj.XYMobile(status.mobile);
                        if(jym.isMyUrl(_backUrl)){
                            location.href = _backUrl;
                        } else {
                            location.href = "/user/index";
                        }
                    } else if(d.Lock){
                        msgWarn('账户信息及密码错误超过5次，您的账户已锁定。请找回您的登录密码或明天再试。');
                    } else {
                        msgWarn('账户名或密码错误，请重新输入。');
                    }
                });
            });
        },
        find : function(obj){
            var $m = $('#username'), m = $.trim($m.val());
            var url = obj.getAttribute('href');
            if(jym.isMobile(m)){
                url += '?mobile=' + m;
            }
            location.href = url;
        }
    }
    obj.validate = {
        setPaymentStatus:function(isPay){
            if(isPay){
                $('#loginPwd').html('交易密码：').parent().addClass('payment-type');
                $('#password').attr('placeholder', '请使用交易密码登录');
                $('#forgotPwd').text('忘记交易密码？').attr('href','/passport/reset-payment-1');
            } else {
                $('#loginPwd').html('登录密码：').parent().removeClass('payment-type');
                $('#password').attr('placeholder', '');
                $('#forgotPwd').text('忘记密码？').attr('href','/passport/reset-password-1');
            }
        },
        mobile : function(callback){
            var $m = $('#username'), m = $.trim($m.val());
            if(m == ''){
                msgWarn('手机号不能为空');
                return false;
            }
            if(!jym.isMobile(m)){
                msgWarn('请正确输入您的注册手机号');
                return false;
            }
            jym.checkMobile(m, function(result, dd){
                if(!result){
                    msgWarn('该手机号未注册，您可以购买兴业票产品进行注册认证。<a href="/" target="_blank">立即购买</a>');
                    return false;
                } else {
                    status.mobile = m;
                    jym.hasPassword(m, function(d){
                        if(!d.Result){
                            obj.validate.setPaymentStatus(true);
                        } else {
                            obj.validate.setPaymentStatus(false);
                            if(ISCATVALID && !jym.isXYUser(dd.UserType)){
                                jymMsg('请使用金银猫账户的密码登录');
                            } else {
                                jymMsg(null, false)
                            }
                        }
                    });
                    jym.fire(callback, [m]);
                }
            });
        },
        password : function(){
            var pwd = $('#password'), pwd_val = pwd.val();
            if(!pwd_val){
                msgWarn('密码不能为空');
                return false;
            }
            if(!jym.isPassword(pwd_val)){
                msgWarn('账户名或密码错误，请重新输入。');
                return false;
            }
            // status.password = pwd_val;
            return pwd_val;
        },
        status : function(callback){
            obj.validate.mobile(function(mobile){
                if(mobile){
                    callback();
                }
            });
        }
    }
    obj.init();
    return obj;
});
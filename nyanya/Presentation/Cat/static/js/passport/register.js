define(['jym', 'jsMd5'], function(jym, md5){
    var status = {
        mobile:0,
        token:0,
        code:0,
        vipCode:0,
        password:0
    };
    var ajaxRegister = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.SignUp,
            data:data,
            success:callback,
            error:function(err){
                require(['dialog'], function(d){
                    d.alert.show(err.responseJSON.Message);
                });
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
        jq.siblings('.login-info').html('').css('display', 'none');
    };
    var obj = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "login-submit":function(){
                    obj.register()
                },
                "partner-submit":function(){
                    obj.partnerReg();
                }
            });
            $('#username').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.mobile();
            });
            $('#password').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.password1();
            });
            $('#vpassword').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.password2();
            });
            $('#code').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.code();
            });
            $('#getCode').click(function(){
                var self = $(this), un = 'btn-code-disabled';
                if(self.hasClass(un)){ return false; }

                obj.validate.mobile(function(mobile){
                    jym.sendValidateCode(mobile, '10', function(d, num){
                        if(d == -1){
                            msgError(self, '发送次数超过5次，请明天再试！');
                        } else if(!d){
                            msgError(self, '短信发送失败！');
                        } else {
                            if(num > 0){
                                self.addClass(un).val(num + '秒后重新获取！');
                            } else {
                                self.removeClass(un).val('获取验证码');
                            }
                        }
                    }, 59);
                    msgError(self, '如未收到验证码，请查看短信是否被拦截。');
                });
            });
            var btn = $('#registerBtn');
            var txt='请勾选“我同意 金银猫版权声明 和 隐私保护 条款”';
            var info = $('#agreeInfo');
            $('#iAgree').click(function(){
                if($(this).prop('checked')){
                    btn.attr('disabled', false);
                    info.text('');
                } else {
                    btn.attr('disabled', true);
                    info.text(txt);
                }
            });
        },
        register:function(){
            obj.validate.status(function(){
                var d = {
                    Password:status.password,
                    Token:status.token,
                    Code:0
                };
                ajaxRegister(d, function(){
                    var uid = md5.hexUpper(status.mobile);
                    jym.OCode.register(uid);
                    if(_backUrl && jym.isSafeUrl(_backUrl)){
                        location.href = _backUrl;
                    } else {
                        location.href = '/passport/register-success';
                    }
                });
            });
        },
        partnerReg:function(){
            obj.validate.status(function(){
                obj.validate.vipCode();
                if(status.vipCode){
                    var d = {
                        Password:status.password,
                        Token:status.token,
                        Code:status.vipCode
                    };
                    ajaxRegister(d, function(){
                        var uid = md5.hexUpper(status.mobile);
                        jym.OCode.register(uid);
                        if(_backUrl && jym.isSafeUrl(_backUrl)){
                            location.href = _backUrl;
                        } else {
                            location.href = '/passport/register-success';
                        }
                    });
                }
            });
        }
    };
    obj.validate = {
        vipCode:function(){
            var $code = $('#vip_code'), code = $.trim($code.val());
            if(code == ''){
                msgError($code, '区域注册码不能为空！');
                return false;
            }
            msgCorrect($code);
            status.vipCode = code;
        },
        status:function(callback){
            var that = this;
            this.code(function(){
                that.password1();
                that.password2();
                if(status.mobile && status.code && status.token && status.password){
                    callback();
                }
            });
        },
        mobile:function(callback){
            status.mobile = 0;
            var $m = $('#username'), m = $.trim($m.val());
            if(m == ''){
                msgError($m, '手机号不能为空！');
                return false;
            }
            if(!jym.isMobile(m)){
                msgError($m, '请正确输入您的注册手机号！');
                return false;
            }
            jym.checkMobile(m, function(result, dd){
                if(result){
                    if(ISXYVALID && !jym.isCatUser(dd.UserType)){
                        var s = 'text-decoration:underline;color:#db5629;';
                        var str = '您是兴业票用户，请直接<a style="'+s+'" href="/passport/login?action=xy&mobile='+m+'">升级账户</a>为金银猫用户。';
                        msgError($m, str);
                    } else {
                        msgError($m, '该手机号已注册！');
                    }
                    return false;
                } else {
                    msgCorrect($m);
                    status.mobile = m;
                    jym.fire(callback, [m]);
                }
            });
        },
        password1:function(){
            var $pwd = $('#password'), pwd = $pwd.val();
            if($.trim(pwd) == ''){
                msgError($pwd, '密码不能为空！');
                return false;
            }
            if(!jym.isPassword(pwd)){
                msgError($pwd, '密码为6-18位字母、符号和数字组合，区分大小写！');
                return false;
            }
            msgCorrect($pwd);
        },
        password2:function(){
            status.password = 0;
            var $pwd1 = $('#password'), pwd1 = $pwd1.val();
            var $pwd2 = $('#vpassword'), pwd2 = $pwd2.val();
            if($.trim(pwd2) == ''){
                msgError($pwd2, '密码不能为空！');
                return false;
            }
            if(pwd1 !== pwd2){
                msgError($pwd2, '两次密码不一致！');
                return false;
            }
            msgCorrect($pwd2);
            status.password = pwd1;
        },
        code:function(codeCallback){
            status.code = 0;
            status.token = 0;
            var chkMobile = function(callback){
                if(status.mobile){
                    callback(status.mobile);
                } else {
                    obj.validate.mobile(callback);
                }
            };
            chkMobile(function(mobile){
                var $code = $('#code'), code = $.trim($code.val());
                if(code == ''){
                    msgError($code, '验证码不能为空！');
                    return false;
                }
                var chkCode = function(mobile, callback){
                    if(status.code && status.token){
                        callback(!0, status.token);
                    } else {
                        jym.verifyCode(mobile, code, '10', callback);
                    }
                };
                chkCode(mobile, function(d, token){
                    if(d == -1){
                        msgError($code, '验证码错误或已失效<br/>请重新获取！');
                    } else if(d){
                        msgCorrect($code);
                        status.code = code;
                        status.token = token;
                        jym.fire(codeCallback);
                    } else {
                        msgError($code, '验证码错误！');
                    }
                });
            });
        }
    };
    obj.init();
    return obj;
});
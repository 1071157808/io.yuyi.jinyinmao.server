define(['jym'], function (jym) {
    var resetAjax = function (data, callback) {
        jym.sync({
            type: "post",
            url: jym.api.passport.ResetPwd,
            data: data,
            success: callback,
            error: function (msg) {
                require(['dialog'], function (d) {
                    d.alert.show(msg.responseJSON.Message);
                });
            }
        });
    }
    var msgError = function (jq, msg, method) {
        method = method || 'show';
        var dis = msg.length == 0 ? 'none' : 'block';
        jq.siblings('.required-error').removeClass('required-ok')[method]();
        jq.siblings('.required-info').css('display', dis).find('b').html(msg);
    };
    var msgCorrect = function (jq) {
        jq.siblings('.required-error').addClass('required-ok').show();
        jq.siblings('.required-info').css('display', 'none').find('b').html('');
    };
    var status = {
        mobile:0,
        code:0,
        token:0,
        password:0
    };
    var obj = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "reset-step1":function(){ obj.resetStep1(this); },
                "reset-step2":function(){ obj.resetStep2(); }
            });
            $('#username').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.mobile();
            });
            $('#code').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                obj.validate.code(function(){
                    $('#resetBtn').removeClass('disable-btn');
                });
            });
            $('#getCode').click(function(){
                var self = $(this), un = 'btn-code-disabled';
                if(self.hasClass(un)){ return false; }
                obj.validate.mobile(function(mobile){
                    jym.sendValidateCode(mobile, 20, function (d, num) {
                        if (d == -1) {
                            msgError(self, '发送次数超过5次，请明天再试！');
                        } else if (!d) {
                            msgError(self, '短信发送失败！');
                        } else {
                            if (num > 0) {
                                self.addClass(un).val(num + '秒后重新获取');
                            } else {
                                self.removeClass(un).val('获取验证码');
                            }
                        }
                    }, 59);
                    msgError(self, '如未收到验证码，请查看短信是否被拦截。', 'hide');
                });

            });
            $('#password').focus(function () {
                msgError($(this), '', 'hide');
            }).blur(function () {
                obj.validate.password1();
            });
            $('#vpassword').focus(function () {
                msgError($(this), '', 'hide');
            }).blur(function () {
                obj.validate.password2();
            });
        },
        resetStep1:function(o){
            if($(o).hasClass('disable-btn')){ return false; }
            obj.validate.code(function(){
                location.href = '/passport/reset/password2?token=' + status.token;
            });
        },
        resetStep2:function(){
            obj.validate.password1();
            obj.validate.password2();
            if(status.password){
                resetAjax({
                    Password:status.password,
                    Token:_token
                }, function(){
                    location.href = '/passport/reset/password3';
                });
            }
        }
    };
    obj.validate = {
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
                if(!result){
                    msgError($m, '该手机号未注册！');
                    return false;
                } else {
                    if(ISXYVALID && !jym.isCatUser(dd.UserType)){
                        var s = 'text-decoration:underline;';
                        var str = '您是兴业票用户，请返回兴业票<a style="'+s+'" href="'+XY_HOST+'/passport/reset-password-1">找回密码</a>';
                        msgError($m, str);
                    } else {
                        msgCorrect($m);
                        status.mobile = m;
                        jym.fire(callback, [m]);
                    }
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
                        jym.verifyCode(mobile, code, '20', callback);
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
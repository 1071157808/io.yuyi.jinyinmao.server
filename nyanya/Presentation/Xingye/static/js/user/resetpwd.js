define(['jym'], function (jym) {
    var resetAjax = function (data, callback) {
        jym.sync({
            type: "post",
            url: jym.api.passport.ResetPwd,
            data: data,
            success: callback,
            error: function (msg) {
                jym.msgInfo(msg.responseJSON.Message);
            }
        });
    }
    var status = {
        mobile:0, // user\manage\reset\login-password-1 12行
        code:0,
        token:0,
        password:0
    };
    if(typeof mobile != 'undefined'){
        status.mobile = mobile;
    }
    var obj = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var mobile = status.mobile;
            jym.action.extend({
                "reset-step1":function(){ obj.resetStep1(this); },
                "reset-step2":function(){ obj.resetStep2(this); }
            });
            $('#username').focus(function(){
                jym.msgError(this,null,!1);
                jym.msgInfo('',!1);
            }).blur(function(){
                obj.validate.mobile();
            });
            $('#code').focus(function(){
                jym.msgError(this,null,!1);
                jym.msgInfo('',!1);
            }).blur(function(){
                obj.validate.ValidCode();
            });
            $('#getCode').click(function(){
                var self = $(this), un = 'btn-code-disabled';
                if(self.hasClass(un)){ return false; }
                jym.sendValidateCode(mobile, 20, function (d, num) {
                    if (d == -1) {
                        jym.msgError('#code','发送次数超过5次，请明天再试。');
                    } else if (!d) {
                        jym.msgError('#code','短信发送失败');
                    } else {
                        if (num > 0) {
                            self.addClass(un).val(num + '秒后重新获取');
                        } else {
                            self.removeClass(un).val('获取校验码');
                        }
                    }
                }, 59);

            });
            $('#password').focus(function () {
                jym.msgError($(this),null,!1);
                jym.msgInfo('',!1);
            }).blur(function () {
                obj.validate.password1();
            });
            $('#vpassword').focus(function () {
                jym.msgError($(this),null,!1);
                jym.msgInfo('',!1);
            }).blur(function () {
                obj.validate.password2();
            });
        },
        resetStep1:function(o){
            if($(o).hasClass('disable-btn')){ return false; }
            obj.validate.code(function(){
                location.href = "/user/manage/reset/login-password-2?token=" + status.token;
            });
        },
        resetStep2:function(){
            var pwd1 = obj.validate.password1();
            if(!pwd1){ return; }
            var pwd2 = obj.validate.password2();
            if(!pwd2){ return; }
            if(status.password && token){
                resetAjax({
                    CellPhone:mobile,
                    Password:status.password,
                    Token:token
                }, function(){
                    location.href = '/user/manage/reset/login-success';
                });
            }
        }
    };
    obj.validate = {
        mobile:function(callback){
            status.mobile = 0;
            var $m = $('#username'), m = $.trim($m.val());
            if(m == ''){
                jym.msgError($m,'手机号不能为空');
                return false;
            }
            if(!jym.isMobile(m)){
                jym.msgError($m,'请正确输入您的注册手机号');
                return false;
            }
            jym.checkMobile(m, function(result){
                if(!result){
                    jym.msgError($m,'该手机号未注册');
                    return false;
                } else {
                    jym.msgError($m,null,!1);
                    status.mobile = m;
                    jym.fire(callback, [m]);
                }
            });
        },
        password1:function(){
            var $pwd = $('#password'), pwd = $pwd.val();
            if($.trim(pwd) == ''){
                jym.msgError($pwd,'密码不能为空');
                return false;
            }
            if(!jym.isPassword(pwd)){
                jym.msgError($pwd,'6-18位字母、数字或符号(~!@#$%^&*_)组合，区分大小写。');
                return false;
            }
            return true;
        },
        password2:function(){
            status.password = 0;
            var $pwd1 = $('#password'), pwd1 = $pwd1.val();
            var $pwd2 = $('#vpassword'), pwd2 = $pwd2.val();
            if($.trim(pwd2) == ''){
                jym.msgError($pwd2,'密码不能为空');
                return false;
            }
            if(pwd1 !== pwd2){
                jym.msgError($pwd2, '两次密码不一致');
                return false;
            }
            status.password = pwd1;
            return true;
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
            var mobile = status.mobile;
            var chkCode = function(mobile, callback){
                if(status.code && status.token){
                    callback(!0, status.token);
                } else {
                    jym.verifyCode(mobile, status.code, '20', callback);
                }
            };
            chkCode(mobile, function(d, token){
                if(d == -1){
                    jym.msgError('#code', '校验码错误或已失效，请重新获取。');
                } else if(d){
                    jym.msgError('#code',null,!1);
                    status.token = token;
                    jym.fire(codeCallback);
                } else {
                    jym.msgError('#code', '校验码错误');
                }
            });
        }
    };
    obj.init();
    return obj;
});
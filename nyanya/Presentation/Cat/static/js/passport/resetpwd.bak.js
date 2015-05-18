define(['jym'], function (jym) {
    var resetAjax = function (data, callback) {
        jym.sync({
            type: "post",
            url: jym.api.passport.ResetPwd,
            data: data,
            success: callback,
            error: function () {
                require(['dialog'], function (d) {
                    d.alert.show(d.responseJSON.Message);
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
    var name_val = '';
    var loginChkName = function () {
        var name = $('#username');
        if (typeof name_val == 'boolean') {
            return name_val;
        }
        name_val = $.trim(name.val());
        if (!name_val) {
            msgError(name, '手机号不能为空！');
            return false;
        }
        if (!jym.isMobile(name_val)) {
            msgError(name, '请正确输入您的注册手机号！');
            return false;
        }
        // 检查是否注册
        jym.checkMobile(name_val, function (d) {
            if (!d) {
                msgError(name, '该手机号未注册！');
                name_val = false;
            }
        });
        msgCorrect(name);
        return name_val;
    };
    var token = '';
    var loginChkCode = function () {
        var code = $('#code'),
            code_val = $.trim(code.val());
        if (token.length > 0) {
            return token;
        }
        var name = loginChkName();
        if (name === false) { return null; }
        if (!code_val) {
            msgError(code, '验证码不能为空！');
            return false;
        }
        if (code_val.length < 6) {
            msgError(code, '验证码最少6位！');
            return false;
        }
        // name, code, type,callback 10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        jym.verifyCode(name, code_val, 20, function (d, t) {
            if (d == -1) {
                msgError(code, '验证码错误或已失效，请重新获取！', 'hide');
            } else if (d) {
                token = t;
            } else {
                msgError(code, '重置失败！');
            }
        });
        msgCorrect(code);
        return token;
    }
    var loginChkData = function () {
        var name = loginChkName();
        if (name === false) { return null; }
        var token = loginChkCode();
        if (token === '') { return null; }
        return {
            Token: token
        };
    };
    var resetChkPwd = function () {
        var pwd = $('#password'), pwd_val = $.trim(pwd.val());
        if (!pwd_val) {
            msgError(pwd, '密码不能为空！');
            return false;
        }
        if (pwd_val.length < 6 || pwd_val.length > 18) {
            msgError(pwd, '密码必须为6-18位！');
            return false;
        }
        if (!jym.isPassword(pwd_val)) {
            msgError(pwd, '密码必须为数字和字母组合，区分大小写！');
            return false;
        }
        msgCorrect(pwd);
        return pwd_val;
    };
    var resetChkVPwd = function () {
        var pwd = $('#password'), pwd_val = $.trim(pwd.val()),
            vpwd = $('#vpassword'), vpwd_val = $.trim(vpwd.val());
        if (!vpwd_val) {
            msgError(vpwd, '密码不能为空！');
            return false;
        } else {
            if (pwd_val == vpwd_val) {
                msgCorrect(vpwd);
                return true;
            } else {
                msgError(vpwd, '2次密码必须相同');
                return false;
            }
        }
        return true;
    }
    var resetChkData = function () {
        var pwd = resetChkPwd();
        if (pwd === false) { return null; }
        var vpwd = resetChkVPwd();
        if (vpwd === false) { return null; }
        return {
            Password: pwd,
            Token: token // 此值在password2.cshtml 13行
        };
    }
    // 重置登录绑定
    var resetBind = function () {
        // firset step
        var username = $('#username');
        username.focus(function () {
            msgError($(this), '', 'hide');
        }).blur(function () {
            loginChkName();
        });
        $('#getCode').click(function () {
            var v = loginChkName(),
                self = $(this),
                un = 'btn-code-disabled';

            if (!v) { return; }

            jym.sendValidateCode(v, 20, function (d, num) {
                if (d == -1) {
                    msgError(self, '发送次数超过5次，请明天再试！');
                } else if (!d) {
                    msgError(self, '短信发送失败！');
                } else {
                    if (num > 0) {
                        self.addClass(un).val(num + '秒后重新获取');
                    } else {
                        self.removeClass(un).val('获取验证码');
                        //msgError(username, '手机号不能为空！');
                    }
                }
            });
        });
        $('#code').focus(function () {
            msgError($(this), '', 'hide');
        }).blur(function () {
            loginChkCode();
        });
        // 第一步跳转
        $('#resetBtn').click(function () {
            var data = loginChkData();
            if (data) {
                location.href = '/passport/reset/password2?token=' + data.Token;
            }
        });
        // second step
        $('#password').focus(function () {
            msgError($(this), '', 'hide');
        }).blur(function () {
            resetChkPwd();
        });
        $('#vpassword').focus(function () {
            msgError($(this), '', 'hide');
        }).blur(function () {
            resetChkVPwd();
        });
    };
    jym.action.extend({
        "reset-submit-2": function () {
            var data = resetChkData();
            if (data) { resetAjax(data, resetChkData); }
            location.href = '/passport/reset/password3';
            return false;
        }
    });
    $(resetBind);
});
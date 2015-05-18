define(['jym'], function(jym){
    var loginAjax = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.SignUp,
            data:data,
            success:callback
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
    var loginChkName = function(){
        var name = $('#username'), name_val = $.trim(name.val());
        if(!name_val){
            msgError(name, '手机号不能为空！');
            return false;
        }
        if(!jym.isMobile(name_val)){
            msgError(name, '请正确输入您的注册手机号！');
            return false;
        }
        // 检查是否注册
        jym.checkMobile(name_val, function(d){
            if(d){
                msgError(name, '该手机号已注册！');
                return false;
            }
        });
        msgCorrect(name);
        return name_val;
    };
    var loginChkPwd = function(){
        var pwd = $('#password'), pwd_val = $.trim(pwd.val());
        if(!pwd_val){
            msgError(pwd, '密码不能为空！');
            return false;
        }
        if(pwd_val.length < 6 || pwd_val.length > 18){
            msgError(pwd, '密码必须为6-18位！');
            return false;
        }
        if(!jym.isPassword(pwd_val)){
            msgError(pwd, '密码必须为数字和字母组合，区分大小写！');
            return false;
        }
        msgCorrect(pwd);
        return pwd_val;
    };
    // 合作商注册码检查
    var inviteChkCode = function(){
        var vip = $('#vip_code'),
            vip_val = $.trim(vip.val()),
            token = '';

        if(!vip_val){
            msgError(vip, '验证码不能为空！');
            return false;
        }
        if(vip_val.length < 6){
            msgError(vip, '验证码不能少于6位！');
            return false;
        }
        // name, code, type, callback
        jym.verifyCode(name, vip_val, 10, function(d, t){
            if(d == -1){
                msgError(vip, '验证码错误或已失效<br/>请重新获取！');
            } else if(d){
                token = t;
            } else {
                msgError(vip, '注册失败！');
            }
        });
        msgCorrect(vip);
        return vip_val;
    }
    var loginChkVPwd = function(){
        var pwd = $('#password'), pwd_val = $.trim(pwd.val()),
            vpwd = $('#vpassword'), vpwd_val = $.trim(vpwd.val());
        if(!vpwd_val){
            msgError(vpwd, '密码不能为空！');
            return false;
        } else {
            if(pwd_val == vpwd_val){
                msgCorrect(vpwd);
                return true;
            } else {
                msgError(vpwd, '2次密码必须相同');
                return false;
            }
        }
        return true;
    }
    var token = '';
    var loginChkCode = function(){
        var code = $('#code'),
            code_val = $.trim(code.val()),
            name = loginChkName();
        if(token.length > 0){
            return token;
        }
        if(!code_val){
            msgError(code, '验证码不能为空！');
            return false;
        }
        // name, code, type, callback 10 => 注册，20 => 重置登录密码
        jym.verifyCode(name, code_val, '10', function(d, t){
            if(d < 0){
                msgError(code, '验证码错误或已失效<br/>请重新获取！');
            } else if(d){
                token = t;
            } else {
                msgError(code, '注册失败！');
            }
        });
        msgCorrect(code);
        return token;
    }
    var loginChkVip = function(){
        var vip = $('#vip_code'),
            vip_val = $.trim(vip.val());
        if(!vip_val){
            msgError(vip, '区域注册码不能为空！');
            return false;
        }
        return vip_val;
    }
    var loginChkData = function(){
        var name = loginChkName();
        if(name === false){ return null; }
        var token = loginChkCode();
        if(token === ''){ return null; }
        var pwd = loginChkPwd();
        if(pwd === false){ return null; }
        return {
            Code : 0, // 为区分金银e家，普通用户为0，金银e家需要正常取值
            Password: pwd,
            Token : token
        };
    };
    var partnerChkData = function(){
        var name = loginChkName();
        if(name === false){ return null; }
        var token = loginChkCode();
        if(token === ''){ return null; }
        var pwd = loginChkPwd();
        if(pwd === false){ return null; }
        var code = inviteChkCode();
        if(code === false){ return null; }
        return {
            Code : code, // 为区分金银e家，普通用户为0，金银e家需要正常取值
            Password: pwd,
            Token : token
        };
    }
    var registerCallback = function(d){
        location.href = '/passport/register-success';
    };
    var registerBind = function(){
        var username = $('#username');
        username.focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkName();
        });
        $('#password').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkPwd();
        });
        $('#vpassword').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkVPwd();
        });
        $('#getCode').click(function(){
            var v = loginChkName(),
                self = $(this),
                username = $('#username'),
                un = 'btn-code-disabled';

            if(!v) { return; }
            jym.sendValidateCode(v, '10', function(d, num){
                if(d == -1){
                    msgError(self, '发送次数超过5次，请明天再试！');
                } else if(!d){
                    msgError(self, '短信发送失败！');
                } else {
                    if(num > 0){
                        self.addClass(un).val(num + '秒后重新获取！');
                    } else {
                        self.removeClass(un).val('获取验证码');
                        //msgError(username, '手机号不能为空！');
                    }
                }
            }, 59);

        });
        $('#code').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkCode();
        });
        $('#vip_code').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkVip();
        });
        var btn = $('#registerBtn');
        $('#iAgree').click(function(){
            var txt='请勾选“我同意 金银猫版权声明 和 隐私保护 条款”' || txt;
            var info = $('#agreeInfo');
            if ($(this).prop('checked')) {
                btn.attr('disabled', false);
                info.text('');
            } else {
                btn.attr('disabled', true);
                info.text(txt);
            }
        });
    };
    jym.action.extend({
        "login-submit":function(){
            var data = loginChkData();
            if(data){ loginAjax(data, registerCallback); }
        },
        // 合作商注册
        "partner-submit":function(){
            var data = partnerChkData();
            if(data){ loginAjax(data, registerCallback); }
        }
    });
    $(registerBind);
});
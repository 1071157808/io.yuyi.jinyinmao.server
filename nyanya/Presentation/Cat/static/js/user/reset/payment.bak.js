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
    var token = '';
    var resetChkCode = function(callback){
        var code = $('#code'),
            code_val = code.val(),
            name = cellphone; // validCode.cshtml 12行
        if(token.length > 0){
            jym.fire(callback,[token]);
            return false;
        }
        if(!code_val){
            jym.msgError(code,'验证码不能为空！');
            return false;
        }
        // name, code, type, callback 10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        jym.verifyCode(name, code_val, 30, function(d, t){
            if(d == -1){
                jym.msgError(code,'验证码错误或已失效<br/>请重新获取！');
            } else if(d){
                token = t;
                jym.fire(callback, [t]);
            } else {
                jym.msgError(code,'获取失败！');
            }
        });
    }
    var bankCardChk = function(){
        var card = $('#bankCard'), card_val = $.trim(card.val());
        if(!card_val){
            jym.msgError(card,'储蓄卡号不能为空！');
            return false;
        }
        if(!jym.isBankCard(card_val)){
            jym.msgError(card,'请填写正确的储蓄卡号！');
            return false;
        }
        jym.msgError(card, null, !1);
        return card_val;
    }
    var idCardChk = function(){
        var card = $('#idCard'), card_val = $.trim(card.val());
        if(!card_val){
            jym.msgError(card,'证件号不能为空！');
            return false;
        }
        var isCard = jym.isIDCard(card_val);
        if(!isCard.pass){
            jym.msgError(card,isCard.msg);
            return false;
        }
        jym.msgError(card,null,!1);
        return card_val;
    }
    var resetBind = function(){
        $('#getCode').click(function(){
            var v = cellphone, // validCode.cshtml 10行
                self = $(this),
                un = 'btn-code-disabled';

            // 10 => 注册，20 => 重置登录密码，30 => 重置支付密码
            jym.sendValidateCode(v, 30, function(d, num){
                if(d == -1){
                    jym.msgError(self,'发送次数超过5次，请明天再试！');
                } else if(!d){
                    jym.msgError(self,'短信发送失败！');
                } else {
                    if(num > 0){
                        self.addClass(un).text(num + '秒后重新获取');
                    } else {
                        self.removeClass(un).text('获取验证码');
                        //msgError(username, '手机号不能为空！');
                    }
                }
            }, 59);

        });
        $('#code').focus(function(){
            jym.msgError(this,null,!1);
        }).blur(function(){
            resetChkCode(function(t){
                token = t;
            });
        });
        // 银行卡
        $('#bankCard').focus(function(){
            jym.msgError(this,null,!1);
        }).blur(function(){
            bankCardChk();
        });
        // 身份证
        $('#idCard').focus(function(){
            jym.msgError(this,null,!1);
        }).blur(function(){
            idCardChk();
        });
        $('#password').focus(function(){
            jym.msgError(this,null,!1);
        }).blur(function(){
            resetChkPwd();
        });
        $('#vpassword').focus(function(){
            jym.msgError(this,null,!1);
        }).blur(function(){
            resetChkVPwd();
        });
    }
    var cardChkData = function(){
        var bankNo = bankCardChk();
        if(bankNo === false){ return null; }
        var idCard = idCardChk();
        if(idCard === false){ return null; }
        return {
            CredentialNo : idCard,
            BankCardNo : bankNo
        };
    }
    var resetChkPwd = function(){
        var pwd = $('#password'), pwd_val = $.trim(pwd.val());
        if(!pwd_val){
            jym.msgError(pwd,'密码不能为空！');
            return false;
        }
        if(pwd_val.length < 8 || pwd_val.length > 18){
            jym.msgError(pwd,'密码必须为8-18位！');
            return false;
        }
        if(!jym.isPayPassword(pwd_val)){
            jym.msgError(pwd,'密码必须为数字和字母组合，区分大小写！');
            return false;
        }
        jym.msgError(pwd, null, !1);
        return pwd_val;
    };
    var resetChkVPwd = function(){
        var pwd = $('#password'), pwd_val = $.trim(pwd.val()),
            vpwd = $('#vpassword'), vpwd_val = $.trim(vpwd.val());
        if(!vpwd_val){
            jym.msgError(vpwd,'密码不能为空！');
            return false;
        } else {
            if(pwd_val == vpwd_val){
                jym.msgError(vpwd,null,!1);
                return true;
            } else {
                jym.msgError(vpwd,'2次密码必须相同');
                return false;
            }
        }
        return true;
    }
    var resetChkCard = function(){
        var pwd = resetChkPwd();
        if(pwd === false){ return null; }
        var vpwd = resetChkVPwd();
        if(vpwd === false){ return null; }
        return {
            Password: pwd,
            BankCardNo: BankCardNo,
            CredentialNo: CredentialNo,
            Token : _token
        };
    }
    var resetCallback = function(){
        location.href = '/user/reset/payment/success';
    }
    jym.action.extend({
        "validCode-submit":function(){
            resetChkCode(function(token){
                if(token){
                    location.href = '/user/reset/payment/validCard?token=' + token;
                }
            });
            return false;
        },
        "validCard-submit":function(){
            var data = cardChkData();
            if(data){
                var p = '?token='+_token +'&BankCardNo=' + data.BankCardNo + '&CredentialNo=' + data.CredentialNo;
                location.href = '/user/reset/payment/password' + p;
            }
            return false;
        },
        "validPwd-submit":function(){
            var data = resetChkCard();
            if(data){ resetAjax(data, resetCallback); }
            return false;
        }
    });
    $(resetBind);
});
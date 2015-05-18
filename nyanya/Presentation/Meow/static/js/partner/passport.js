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
                    if(zim.page.isSafeUrl(url)){
                        zim.location(url);
                    } else {
                        zim.user.validate(function(g){
                            if(g.HasDefaultBankCard){
                                var count = zim.storage.snap('partnerCount');
                                var product_no = zim.storage.snap('partnerId');
                                var type = zim.storage.snap('partnerType');
                                zim.location('/partner/payment/create?id='+product_no+'&count='+count+'&type='+type);
                            } else if(g.HasSetPaymentPassword){
                                zim.location('/partner/yilian/identity');
                            } else {
                                zim.location('/partner/yilian/password');
                            }
                        });
                    }
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
                    zim.location("/partner/register/code?mobile="+mobile);
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
                    zim.location("/partner/register/password?token="+d.Token+"&mobile="+mobile);
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
        var source = zim.storage.snap('partnerSource');
        var code = zim.passport.CodeType[source] || "903";
        zim.passport.register({
            data:{Password:pwd1, Token:token, Code:code},
            success:function(){
                zim.OCode.register(jsMd5.hexUpper(mobile));
                zim.ui.confirm({
                    msg:'注册成功！<br>开通快捷支付后才能进行购买！',
                    btnCancel:'开通快捷支付',
                    callback:function(t){
                        zim.location(t?'/':'/partner/yilian/password');
                    }
                });
            },
            error:function(d){
                zim.ui.message(d.responseJSON.Message);
            }
        });
    }
});
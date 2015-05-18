define(['jym','jsMd5'], function(jym, md5){
    var loginAjax = function(data, callback){
        jym.sync({
            type:"post",
            url:jym.api.passport.SignIn,
            data:data,
            success:callback
        });
    };
    var status = {
        mobile:0
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
    var loginChkName = function(callback){
        var name = $('#username'), name_val = $.trim(name.val());
        status.mobile = 0;
        if(!name_val){
            msgError(name, '手机号不能为空！');
            return false;
        }
        if(!jym.isMobile(name_val)){
            msgError(name, '请正确输入您的注册手机号！');
            return false;
        }
        if(ISXYVALID){
            jym.checkMobile(name_val, function(result, dd){
                if(result && !jym.isCatUser(dd.UserType)){
                    var s = 'text-decoration:underline;color:#db5629;';
                    var str = '您是兴业票用户，请直接<a style="'+s+'" href="#" class="ui-action" data-action="cat-to-xy">升级为金银猫用户</a>。';
                    msgError(name, str);
                } else {
                    msgCorrect(name);
                    status.mobile = name_val;
                    jym.fire(callback, [name_val]);
                }
            });
        } else {
            msgCorrect(name);
            status.mobile = name_val;
            jym.fire(callback, [name_val]);
        }
    };
    var loginChkPwd = function(){
        var pwd = $('#password'), pwd_val = pwd.val();
        if(!pwd_val){
            msgError(pwd, '密码不能为空！');
            return false;
        }
        if(!jym.isPassword(pwd_val)){
            msgError(pwd, '登录密码为6-18位字母、数字和符号组合！');
            return false;
        }
        msgCorrect(pwd);
        return pwd_val;
    };

    var loginChkData = function(callback){
        loginChkName(function(name){
            var pwd = loginChkPwd();
            if(pwd === false){ return null; }
            jym.fire(callback, [{
                Name: name,
                Password: pwd
            }]);
        });
    };
    var login_submit_callback = function(data, isXY){
        if(data){
            var str_pwd = isXY ? "#xy_password" : "#password";
            loginAjax(data, function(d){
                if(d.Successful){
                    var uid = md5.hexUpper(data.Name);
                    jym.OCode.login(uid);
                    var login_callback = function(){
                        if(jym.isSafeUrl(_backUrl)){
                            location.href = _backUrl;
                        } else {
                            location.href = "/user/index";
                        }
                    };
                    if(ISXYVALID){
                        xy_login.add(login_callback);
                        xy_login.submit(d.RCode);
                    } else {
                        login_callback();
                    }
                } else if(d.Lock){
                    msgError($(str_pwd), '重试超过5次，账户已被锁定！<br>请找回密码或明天再试。');
                } else {
                    msgError($(str_pwd), '账户或密码错误！<br>请检查正确后再登录。');
                }
            }); 
        }
    };
    var loginSubmit = function(){
        loginChkData(function(d){
            login_submit_callback(d, false);
        });
    };
    var loginBind = function(){
        var u = $('#username').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkName();
        }).keydown(function(e){
            if(e.keyCode == 13){
                e.preventDefault();
                loginSubmit();
            }
        });
        var m = u.val();
        if(m && jym.isMobile(m)){
            loginChkName();
        }
        $('#password').focus(function(){
            msgError($(this), '', 'hide');
        }).blur(function(){
            loginChkPwd();
        }).keydown(function(e){
            if(e.keyCode == 13){
                e.preventDefault();
                loginSubmit();
            }
        });
    };
    jym.action.extend({
        "login-submit":function(){
            loginSubmit();
        },
        "upgrade-submit":function(){
            xy_login.loginSubmit();
        },
        "cat-to-xy":function(){
            $('#cat-login-span').hide();
            $('#xy-login-span').show();
            var m = $('#username').val();
            var xyu = $('#xy_username').val(m);
            jym.placeholder.trigger(xyu);
            xy_login.loginChkName();
        },
        "xy-to-cat":function(){
            $('#cat-login-span').show();
            $('#xy-login-span').hide();
            var u = $('#username').val('');
            jym.placeholder.show(u);
            msgError(u, '', 'hide');
            var p = $('#password').val('');
            msgError(p, '', 'hide');
        }
    });
    var xy_login = {
        init:function(){
            this.bind();
        },
        readyCallbacks:[],
        bind:function(){
            var u = $('#xy_username').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                xy_login.loginChkName();
            }).keydown(function(e){
                if(e.keyCode == 13){
                    e.preventDefault();
                    xy_login.loginSubmit();
                }
            });
            var m = u.val();
            if(m && jym.isMobile(m)){
                xy_login.loginChkName();
            }
            $('#xy_password').focus(function(){
                msgError($(this), '', 'hide');
            }).blur(function(){
                xy_login.loginChkPwd();
            }).keydown(function(e){
                if(e.keyCode == 13){
                    e.preventDefault();
                    xy_login.loginSubmit();
                }
            });
        },
        add:function(fn){
            this.readyCallbacks.push(fn);
        },
        fireCallback:function(d){
            var fn = null;
            var list = this.readyCallbacks;
            while(fn = list.shift()){fn(d)}
        },
        submit:function(code){
            setTimeout(function(){ xy_login.fireCallback({}); }, 2000);
            var url = XY_HOST + '/api/v1/User/SignInByCode';
            $.ajax({
                url:url,
                data:{rCode:code},
                dataType:'jsonp',
                success:function(d){
                    xy_login.fireCallback(d);
                }
            });
        },
        checkHasPassword:function(callback){
            var u = $('#xy_username');
            var m = u.val();
            var url = XY_HOST + '/api/v1/User/HasLoginPassword';
            if(m && jym.isMobile(m)){
                $.ajax({
                    url:url,
                    data:{cellphone: m},
                    dataType:'jsonp',
                    success:function(d){
                        jym.fire(callback, [d]);
                        if(!d.Result){
                            var s = 'text-decoration:underline;color:#db5629;';
                            var str = '您还没有<a style="'+s+'" href="'+XY_HOST+'/user">设置兴业票登录密码</a>';
                            var pwd = $('#xy_password').prop('disabled', true);
                            msgError(pwd, str);
                        }
                    }
                });
            }
        },
        loginChkName:function(callback){
            var name = $('#xy_username'), name_val = $.trim(name.val());
            status.mobile = 0;
            if(!name_val){
                msgError(name, '手机号不能为空！');
                return false;
            }
            if(!jym.isMobile(name_val)){
                msgError(name, '请正确输入您的注册手机号！');
                return false;
            }
            this.checkHasPassword(function(){
                msgCorrect(name);
                status.mobile = name_val;
                jym.fire(callback, [name_val]);
            });
        },
        loginChkPwd:function(){
            var pwd = $('#xy_password'), pwd_val = pwd.val();
            if(!pwd_val){
                msgError(pwd, '密码不能为空！');
                return false;
            }
            if(!jym.isPassword(pwd_val)){
                msgError(pwd, '登录密码为6-18位字母、数字和符号组合！');
                return false;
            }
            msgCorrect(pwd);
            return pwd_val;
        },
        loginChkData:function(callback){
            var that = this;
            this.loginChkName(function(name){
                var pwd = that.loginChkPwd();
                if(pwd === false){ return null; }
                jym.fire(callback, [{
                    Name: name,
                    Password: pwd
                }]);
            });
        },
        loginSubmit:function(){
            var args = $('#rd_register').prop('checked');
            if(args){
                this.loginChkData(function(d){
                    login_submit_callback(d, true)
                });
            }
        }
    };
    $(function(){
        loginBind();
        xy_login.init();
    });
});
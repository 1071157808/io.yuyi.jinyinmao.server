define(['jym', 'dialog'], function(jym, dialog){
    var T = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "setPassword":function(){
                    T.submit()
                },
                "tempChangeMobile":function(){
                    jym.load.logout(function(){
                        location.reload();
                    });
                }
            });
        },
        submit:function(){
            var pwd1 = this.Valid.password1();
            if(pwd1 == false) return false;
            var pwd2 = this.Valid.password2();
            if(pwd2 == false) return false;
            jym.sync({
                type:"post",
                url:jym.api.passport.setLoginPassword,
                data:{Password: pwd1},
                success:function(){
                    location.reload();
                },
                error:function(d){
                    jym.msgError('#ctt-password1',d.responseJSON.Message);
                }
            });
        },
        Valid:{
            password1:function(){
                var pwd = $.trim($('#ctt-password1').val());
                if(!pwd){
                    jym.msgError('#ctt-password1','登录密码不能为空');
                    return false;
                }
                if(pwd != jym.trimAll(pwd)){
                    jym.msgError('#ctt-password1','登录密码不能包含空格');
                    return false;
                }
                if(!jym.isPassword(pwd)){
                    jym.msgError('#ctt-password1','登录密码为6-18位字符、数字或字母的组合。');
                    return false;
                }
                return pwd;
            },
            password2:function(){
                var pwd1 = $.trim($('#ctt-password1').val());
                var pwd2 = $.trim($('#ctt-password2').val());
                if(pwd1 !== pwd2){
                    jym.msgError('#ctt-password2','两次密码不一致');
                    return false;
                }
                return true;
            }
        }
    };
    T.init();
    return T;
});
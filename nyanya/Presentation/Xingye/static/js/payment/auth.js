define(['jym', 'dialog'], function(jym, dialog){
    var SelectBank = {
        bind:function(selector){
            var target = this.$target = $(selector);
            this.$text = target.find('.ctt-xSelect-text').click(function(){
                SelectBank.toggle();
            });
            target.find('.ctt-xSelect-drop').click(function(){
                SelectBank.toggle();
            });
            this.$span = target.find('.ctt-xSelect-span');
            this.initData(jymBanks);
            this.data = {
                BankName:"兴业银行",
                CityName:"上海|上海"
            };
            jym.action.extend({
                "select-bank":function(){
                    SelectBank.select(this);
                }
            });
        },
        initData:function(banks){
            var tpl = '<ul class="clearfix ctt-xSelect-banklist">';
            var city = '上海|上海';
            $.each(banks, function(i, v){
                tpl += '<li><a href="#" class="ui-action" data-action="select-bank" data-bank="'+v.name+'" data-city="'+(v.defaults||city)+'">';
                tpl += '<em class="ctt-xSelect-bank '+v.code+'">'+v.name+'</em></a></li>';
            });
            tpl += '</ul>';
            this.$span.html(tpl);
        },
        select:function(o){
            this.data.BankName = o.getAttribute('data-bank');
            this.data.CityName = o.getAttribute('data-city');
            this.$text.html(o.innerHTML);
            this.toggle();
        },
        toggle:function(){
            this.$span.toggle();
            jym.msgError('#bankSelect',null,!1);
        }
    };
    var CanSyncStatus = true;
    var getFirstBuyUrl = function(){
        if(typeof _addBackUrl === 'undefined'){
            var url = location.href, c = '?', key = 'firstbuy';
            var regx = /(\?|\&)([\w]+)=([^\?\&]+)/g;
            if(url.indexOf(c) == -1){
                url = url + c + key + "=1";
            } else {
                var p = [], o = {};
                url = url.replace(regx, function(a,b,c,d){
                    if(c != key && !o[c]){ o[c] = d; p.push(c+'='+d); }
                    return "";
                });
                p.push(key+'=1');
                url = url + c + p.join('&');
            }
        } else { url = _addBackUrl; }
        return url;
    };
    var BankDialog = {
        status:{
            countdown:function(){
                var delay = 1000 * 60 * 10;
                setTimeout(function(){
                    BankDialog.status.get();
                }, delay);
            },
            get:function(){
                var card = jym.trimAll($('#ctt-bankcard').val());
                jym.sync({
                    type:"post",
                    url:jym.api.bankcard.AddBankCardStatus,
                    data:{BankCardNo:card},
                    success:BankDialog.status.callback,
                    error:$.noop
                });
            },
            callback:function(d){
                if(d.Status == 1){
                    location.href = getFirstBuyUrl();
                } else if(d.Status == 2){
                    var s = '<p class="ui-ico-error">对不起，您的银行卡仍在认证中，请耐心等候。</p>';
                    $('#ctt-bank-dialog-msg').html(s).show();
                } else {
                    var s = '<p class="ui-ico-error">'+d.Message+'</p>';
                    $('#ctt-bank-dialog-msg').html(s).show();
                }
            }
        },
        create:jym.once(function(){
            var style = 'style="position:absolute;margin-top:12px;margin-left:20px;"';
            var p = '<p class="ui-ico-error">对不起，您的银行卡仍在认证中，请耐心等候。</p>';
            var html = '<div class="ctt-bank-dialog">';
            html += '<div id="ctt-bank-dialog-msg" class="ui-msg" style="display:none;">'+p+'</div>';
            html += '<div class="ctt-bank-dialog-wrap">';
            html += '<button class="ui-action btn-common" data-action="ctt-bank-dialog-btn">电话认证成功</button>';
            html += '<a href="#" '+style+' class="ui-action" data-action="ctt-bank-dialog-fail">认证失败？</a>';
            html += '<div class="ctt-bank-dialog-span">如有问题，请联系4008-556-333</div>';
            html += '</div></div>';
            this.$obj = $(html).appendTo('body');
            if(jym.browser.ie6){
                this.$obj.css({ top : $(document).scrollTop() + ($(window).height() - this.$obj.height()) / 2 + 'px', marginTop : 0})
            }
            jym.action.extend({
                "ctt-bank-dialog-btn":function(){
                    BankDialog.status.get();
                },
                "ctt-bank-dialog-fail":function(){
                    CanSyncStatus = true;
                    BankDialog.$obj.hide();
                    dialog.mask.hide();
                    $('#ctt-bank-dialog-msg').hide();
                }
            });
        }),
        show:function(){
            this.create();
            this.status.countdown();
            this.$obj.show();
            dialog.mask.show(990);
        }
    };
    var T = {
        init:function(){
            this.bind();
        },
        bind:function(){
            SelectBank.bind('#bankSelect');
            jym.action.extend({
                "bind-bank-card":function(){
                    if(CanSyncStatus){ T.submit(); }
                },
                "tempChangeMobile":function(){
                    jym.load.logout(function(){
                        location.reload();
                    });
                }
            });
            $('.MsgContext').focus(function(){
                jym.msgError(this, null, false);
                jym.msgInfo(null, false);
            });
        },
        submit:function(){
            jym.msgInfo(null, false);
            var BankName = SelectBank.data.BankName;
            var CityName = SelectBank.data.CityName;
            var BankCardNo = this.Valid.bankcard();
            if(!BankCardNo){return false;}
            var RealName = this.Valid.name();
            if(!RealName){return false;}
            var Credential = $('#ctt-idtype').val();
            var CredentialNo = this.Valid.idCard();
            if(!CredentialNo){return false;}
            var isSetPassword = this.Valid.isSetPassword();
            if(BankCardNo && RealName && CredentialNo){
                var data = {
                    BankCardNo:BankCardNo,
                    BankName:BankName,
                    CityName:CityName,
                    Credential:Credential,
                    CredentialNo:CredentialNo,
                    RealName:RealName
                };
                if(isSetPassword){
                    this.setPaymentPassword(function(){
                        T.addBankCard(data);
                    });
                } else {
                    this.addBankCard(data);
                }
            }
        },
        setPaymentPassword:function(callbck){
            var pwd1 = this.Valid.password1();
            if(pwd1 == false) return false;
            var pwd2 = this.Valid.password2();
            if(pwd2 == false) return false;
            if(pwd1 && pwd2){
                jym.sync({
                    type:"post",
                    url:jym.api.bankcard.setPassword,
                    data:{Password: pwd1},
                    beforeSend:$.noop,
                    complete:$.noop,
                    success:callbck,
                    error:function(d){
                        CanSyncStatus = true;
                        if(d.status == 401){
                            dialog.alert.show({
                                msg:'登录超时，请重新登录。',
                                callback:function(){ location.reload(); }
                            });
                        } else if('已设置交易密码' == d.responseJSON.Message){
                            location.href = getFirstBuyUrl();
                        } else {
                            jym.msgInfo(d.responseJSON.Message);
                        }
                    }
                });
            } else {
                CanSyncStatus = true;
            }
        },
        addBankCard:function(data){
            if(data){
                CanSyncStatus = false;
                jym.sync({
                    type:"post",
                    url:jym.api.bankcard.SignUpPayment,
                    data:data,
                    success:function(){
                        BankDialog.show();
                    },
                    error:function(d){
                        CanSyncStatus = true;
                        if(d.status == 401){
                            dialog.alert.show({
                                msg:'登录超时，请重新登录。',
                                callback:function(){ location.reload(); }
                            });
                        } else if('已经开通快捷支付功能' == d.responseJSON.Message){
                            location.href = getFirstBuyUrl();
                        } else {
                            jym.msgInfo(d.responseJSON.Message);
                        }
                    }
                });
            } else {
                CanSyncStatus = true;
            }
        },
        Valid:{
            bankcard:function(){
                var card = jym.trimAll($('#ctt-bankcard').val());
                if(!card){
                    jym.msgError('#ctt-bankcard','银行储蓄卡卡号不能为空');
                    return false;
                }
                if(!jym.isBankCard(card)){
                    jym.msgError('#ctt-bankcard','银行储蓄卡卡号为15-19位数字');
                    return false;
                }
                return card;
            },
            name:function(){
                var name = $.trim($('#ctt-name').val());
                if(!name){
                    jym.msgError('#ctt-name','姓名不能为空');
                    return false;
                }
                if(!jym.isRealName(name)){
                    jym.msgError('#ctt-name','请输入真实姓名');
                    return false;
                }
                return name;
            },
            idCard:function(){
                var card = jym.trimAll($('#ctt-idcard').val());
                if(!card){
                    jym.msgError('#ctt-idcard','证件号码不能为空');
                    return false;
                }
                return card;
            },
            isSetPassword:function(){
                return !!document.getElementById("ctt-password1");
            },
            password1:function(){
                var pwd = $('#ctt-password1').val();
                if(!pwd){
                    jym.msgError('#ctt-password1','兴业票交易密码不能为空');
                    return false;
                }
                if(pwd !== jym.trimAll(pwd)){
                    jym.msgError('#ctt-password1','交易密码不能包含空格');
                    return false;
                }
                if(!jym.isPayPassword(pwd)){
                    jym.msgError('#ctt-password1','交易密码为8-18位字符、数字和字母的组合。');
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
    define(['jym', 'dialog'], function(jym, dialog){
    var BK = {};
    BK.setPassword = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "setPaymentPassword":function(){
                    BK.setPassword.submit()
                }
            });
            $("input.pwd-input").focus(function(){
                jym.msgError(this, '', false);
            });
        },
        send:function(pwd){
            jym.sync({
                type:"post",
                url:jym.api.bankcard.setPassword,
                data:{Password:pwd},
                success:function(){
                    dialog.alert.show('支付密码设置成功！', function(){
                        location.href = "/bankcard/identity";
                    });
                },
                error:function(d){
                    dialog.alert.show(d.responseJSON.Message);
                }
            });
        },
        submit:function(){
            var pwd1 = $('#payment-pwd1').val();
            var pwd2 = $('#payment-pwd2').val();
            if(!jym.isPassword(pwd1)){
                jym.msgError('#payment-pwd1', '密码为8~18位，支持数字，字母，区分大小写。');
                return false;
            } else if(pwd1 != pwd2){
                jym.msgError('#payment-pwd2', '两次密码不一致。');
                return false;
            }
            this.send(pwd1);
        }
    };
    BK.identity = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "submitIdentity":function(){
                    BK.identity.submit()
                }
            });
            $("input.text-input").focus(function(){
                jym.msgError(this, '', false);
            });
        },
        submit:function(){
            var realName = jym.trimAll($('#realName').val());
            var idCard = jym.trimAll($('#idCard').val());
            var idCardType = $('#idCardType').val();
            if(!jym.isRealName(realName)){
                jym.msgError('#realName', '请输入您的真实姓名。');
                return false;
            }
            if(idCardType == 0){
                var o = jym.isIDCard(idCard);
                if(!o.pass){
                    jym.msgError('#idCard', '身份证号码格式错误。');
                    return false;
                }
            } else if(!idCard){
                jym.msgError('#idCard', '证件号码不能为空。');
                return false;
            }
            var p = [
                "r=" + encodeURIComponent(realName),
                "d=" + idCard,
                "t=" + idCardType
            ].join('&');
            dialog.confirm.show('确认您的信息准确？', function(t){
                if(t){ location.href = "/bankcard/addCard?" + p; }
            });
        }
    };
    var SelectBank = {
        bind:function(selector){
            var target = this.$target = $(selector);
            this.$text = target.find('.ctt-bSelect-text').click(function(){
                SelectBank.toggle();
            });
            this.$span = target.find('.ctt-bSelect-span');
            this.initData(jymBanks);
            jym.action.extend({
                "select-bank":function(){
                    SelectBank.select(this);
                }
            });
        },
        initData:function(banks){
            var tpl = '<ul class="clearfix ctt-bSelect-banklist">';
            var city = '上海|上海';
            $.each(banks, function(i, v){
                tpl += '<li><a href="#" class="ui-action" data-action="select-bank" data-bank="'+v.name+'" data-city="'+(v.defaults||city)+'">';
                tpl += '<i class="ctt-bSelect-bank '+v.code+'"></i></a></li>';
            });
            tpl += '</ul>';
            this.$span.html(tpl);
        },
        select:function(o){
            this.data = {};
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
    BK.addCard = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "addCardSubmit":function(){
                    BK.addCard.submit();
                }
            });
            SelectBank.bind("#bankSelect");
            $('#bankcard').focus(function(){
                jym.msgError(this, null, !1);
            });
            $('#isAgreement').click(function(){
                if($(this).prop('checked')){
                    $('#btnAddCard').removeClass('ui-btn1').addClass('ui-btn');
                } else {
                    $('#btnAddCard').removeClass('ui-btn').addClass('ui-btn1');
                }
            });
        },
        send:function(d){
            var api = jym.api.bankcard.AddBankCard;
            if(typeof _user_info !== 'undefined'){
                api = jym.api.bankcard.SignUpPayment;
                d = $.extend(d, _user_info);
            }
            jym.sync({
                type:'post',
                url:api,
                data:d,
                success:function(){
                    location.href = "/bankcard/verify";
                },
                error:function(d){
                    dialog.alert.show(d.responseJSON.Message);
                }
            });
        },
        submit:function(){
            var isAgree = $('#isAgreement').prop('checked');
            if(!isAgree){ return false; }
            if(!SelectBank.data){
                jym.msgError('#bankSelect','请选择开户银行。');
                return false;
            }
            var card = $('#bankcard').val();
            if(!jym.isBankCard(card)){
                jym.msgError('#bankcard','请正确输入银行卡号。');
                return false;
            }
            var data = $.extend({BankCardNo:card}, SelectBank.data);
            this.send(data);
        }
    };
    BK.verify = {
        init:function(){
            jym.action.extend({
                "gotoLook":function(){
                    location.href = "/amp/list";
                }
            });
        }
    };
    return BK;
});
zim.ns('zim.page.yilian.valid', {
    NoPassword:function(d){
        if(!d.HasSetPaymentPassword){
            zim.location('/user/yilian/password');
        }
    },
    hasCard:function(d){
        if(d.HasDefaultBankCard){
            zim.location('/user/yilian/addCard');
        }
    }
});
zim.ns('zim.page.yilian.bankPage', {
    create:function(){
        var def_bank = $('#jqm-bank').attr('data-bank');
        var tpl = $('#jqm-tpl-banks').text();
        var html = zim.tpl(tpl, {banks:zimBanks, def_bank:def_bank});
        this.__page = zim.ui.SinglePage({
            title:"选择开户银行",
            html:html
        }).show().inject_page_back();
    },
    destroy:function(){
        this.__page && this.__page.hide().destroy();
    }
});
zim.ns('zim.page.yilian.IdPage', {
    create:function(){
        var idType = $('#jqm-idType').attr('data-idType');
        var tpl = $('#jqm-tpl-idtype').text();
        var html = zim.tpl(tpl, {types:zim.eHash.IdType, def_type:idType});
        this.__page = zim.ui.SinglePage({
            title:"选择证件类型",
            html:html
        }).show().inject_page_back();
    },
    destroy:function(){
        this.__page && this.__page.hide().destroy();
    }
});
zim.ns('zim.page.yilian.password', {
    init:function(){
        this.bind();
        this.render();
    },
    bind:function(){
        var that = this;
        zim.action.extend({
            "set-password":function(){
                that.submit()
            }
        })
    },
    render:function(){
        zim.user.validate(function(d){
            if(d.HasDefaultBankCard){
                zim.location('/user/yilian/addCard');
            } else if(d.HasSetPaymentPassword){
                zim.location('/user/yilian/identity');
            }
        });
    },
    submit:function(){
        var pwd1 = $.trim($('#frm-payment-pwd1').val());
        var pwd2 = $('#frm-payment-pwd2').val();
        if(!pwd1){
            zim.ui.alert('支付密码不能为空！');
            return false;
        }
        if(!zim.isPayPassword(pwd1)){
            zim.ui.alert('支付密码必须8-18位字母、数字和符号的两组组合！');
            return false;
        }
        if(pwd1 !== pwd2){
            zim.ui.alert('两次支付密码不一致！');
            return false;
        }
        zim.sync({
            type:'post',
            url:zim.api.bankcard.setPassword,
            data:{Password:pwd1},
            success:function(){
                zim.location('/user/yilian/identity');
            },
            error:function(d){
                zim.ui.alert(d.responseJSON.Message);
            }
        });
    }
});
zim.ns('zim.page.yilian.identity', {
    init:function(){
        this.bind();
        this.render();
    },
    bind:function(){
        var that = this;
        zim.action.extend({
            "select-idtype":function(){
                zim.page.yilian.IdPage.create();
            },
            "selected-idtype":function(){
                that.IdTypeSelected(this);
            },
            "next-addCard":function(){
                that.nextAddCard();
            }
        });
    },
    render:function(){
        zim.user.validate(function(d){
            zim.page.yilian.valid.NoPassword(d);
            zim.page.yilian.valid.hasCard(d);
            zim.tplRender('#jqm-identity-valid', '#jqm-tpl-identity', d);
        });
    },
    IdTypeSelected:function(o){
        var idType = o.getAttribute('data-idType');
        var idTypeName = o.getAttribute('data-text');
        $('#jqm-idType').attr('data-idType', idType).html('<span>'+idTypeName+'</span>');
        zim.page.yilian.IdPage.destroy();
    },
    nextAddCard:function(){
        var name = $.trim($('#jqm-realName').val());
        if(!name){
            zim.ui.alert('姓名不能为空！');
            return false;
        }
        if(!zim.isRealName(name)){
            zim.ui.alert('请正确输入您的姓名！');
            return false;
        }
        var idCard = $.trim($('#jqm-idCard').val());
        if(!idCard){
            zim.ui.alert('证件号码不能为空！');
            return false;
        }
        var p = zim.params('?name={realName}&type={IdType}&card={IdCard}', {
            realName:name,
            IdType:$('#jqm-idType').attr('data-idType'),
            IdCard:idCard
        });
        zim.location('/user/yilian/addCard' + p);
    }
});
zim.ns('zim.page.yilian.addCard', {
    init:function(){
        this.bind();
        this.render();
    },
    bind:function(){
        var that = this;
        zim.action.extend({
            "select-bank":function(){
                zim.page.yilian.bankPage.create();
            },
            "selected-bank":function(){
                that.BankSelected(this);
            },
            "validate-card":function(){
                that.validateCard();
            }
        });
    },
    render:function(){
        zim.user.validate(function(d){
            zim.page.yilian.valid.NoPassword(d);
            if(!d.HasDefaultBankCard){
                if(d.HasYSBInfo){
                    d.RealName = d.YSBRealName;
                    d.Credential = 0;
                    d.CredentialNo = d.YSBIdCard;
                } else {
                    var idType = zim.urlQuery('type');
                    var idCard = zim.urlQuery('card');
                    var name = zim.urlQuery('name');
                    if(idType && idCard && name){
                        d.RealName = name;
                        d.Credential = idType;
                        d.CredentialNo = idCard;
                    } else {
                        zim.location('/user/yilian/identity');
                    }
                }
            }
            zim.tplRender('#jqm-addCard-wrap', '#jqm-tpl-addCard', d);
        });
    },
    BankSelected:function(o){
        var bank = o.getAttribute('data-bank');
        var city = o.getAttribute('data-city');
        $('#jqm-bank').attr('data-bank', bank).attr('data-city',city).html('<span>'+bank+'</span>');
        zim.page.yilian.bankPage.destroy();
    },
    submit:function(d, api){
        zim.sync({
            type:"post",
            url:api,
            data:d,
            success:function(){
                zim.location('/user/yilian/verify');
            },
            error:function(d){
                zim.ui.alert(d.responseJSON.Message);
            }
        });
    },
    validateCard:function(){
        var $bank = $('#jqm-bank');
        var bank = $bank.attr('data-bank');
        var city = $bank.attr('data-city');
        if(!bank){
            zim.ui.alert('请选择开户银行！');
            return false;
        }
        var card = $('#bankcard').val();
        if(!zim.isBankCard(card)){
            zim.ui.alert('请输入15-19位银行卡号！');
            return false;
        }
        var that = this, data = {
            BankCardNo:card,
            BankName:bank,
            CityName:city
        };
        zim.user.validate(function(d){
            var api = zim.api.bankcard.AddBankCard;
            if(!d.HasDefaultBankCard){
                api = zim.api.bankcard.SignUpPayment;
                if(d.HasYSBInfo){
                    data = $.extend(data, {
                        Credential:0,
                        CredentialNo:d.YSBIdCard,
                        RealName:d.YSBRealName
                    });
                } else {
                    data = $.extend(data, {
                        Credential:zim.urlQuery('type'),
                        CredentialNo:zim.urlQuery('card'),
                        RealName:zim.urlQuery('name')
                    });
                }
            }
            that.submit(data, api);
        });
    }
});
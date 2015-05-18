zim.ns('zim.page.user.card.list', {
    init:function(){
        this.Callbacks = $.Callbacks({memory:true});
        this.bind();
        this.render();
        this.getData();
    },
    getSlice:function(c){
        return c.substr(c.length-4,4);
    },
    bind:function(){
        var that = this;
        this.route = zim.routes({
            "":function(){
                zim.page.user.card.detail.destroy();
            },
            "card/:id":function(id){
                that.getBankInfo(id);
            }
        });
        zim.action.extend({
            "add-new-card":function(){
                zim.user.validate(function(d){
                    if(!d.HasSetPaymentPassword){
                        zim.location("/user/yilian/password");
                    } else if(!d.HasDefaultBankCard){
                        zim.location("/user/yilian/identity");
                    } else {
                        zim.location("/user/yilian/addCard");
                    }
                });
            },
            "set-default":function(){
                zim.page.user.card.detail.setDefault(this);
            }
        });
    },
    getBankInfo:function(cardNo){
        var BankInfo = function(d){
            for(var i=0,l=d.length;i<l;i++){
                if(d[i].BankCardNo == cardNo){
                    zim.page.user.card.detail.create(d[i]);
                    break;
                }
            }
        };
        if(this.data){
            BankInfo(this.data)
        } else {
            this.Callbacks.add(BankInfo);
        }
    },
    getData:function(){
        var that = this;
        zim.sync({
            url:zim.api.user.bankcards,
            success:function(d){
                that.Callbacks.fire(d);
                that.data = d;
            }
        });
    },
    render:function(){
        var that = this;
        this.Callbacks.add(function(d){
            zim.tplRender('#jqm-bank-cards', '#jqm-tpl-cards', {items:d});
        });
    }
});
zim.ns('zim.page.user.card.detail', {
    CheckDefault:function(item){
        if(!this._def_card && item.IsDefault){
            this._def_card = item.BankCardNo;
        }
    },
    create:function(card){
        if(!card){return false;}
        this.CheckDefault(card);
        card.IsDefault = card.BankCardNo == this._def_card;
        card.CardSlice = zim.page.user.card.list.getSlice(card.BankCardNo);
        var tpl = $('#jqm-tpl-detail').text();
        var html = zim.tpl(tpl, card);
        this.__page = zim.ui.SinglePage({
            title:card.CardBankName,
            html:html
        }).show();
    },
    destroy:function(){
        this.__page && this.__page.hide().destroy();
    },
    setAjaxDefault:function(card, callback){
        zim.sync({
            url:zim.api.bankcard.SetDefault,
            data:{bankCardNo:card},
            success:callback,
            error:function(d){
                zim.ui.alert(d.responseJSON.Message);
            }
        });
    },
    setDefault:function(o){
        var that = this;
        var card = o.getAttribute('data-card');
        this.setAjaxDefault(card, function(){
            that._def_card = card;
            $('.bankcard-default', '#jqm-bank-cards').remove();
            $('#card-'+card).append('<samp class="bankcard-default">默认</samp>');
            $(o).attr('data-action','').removeClass('ui-btn1').addClass('ui-btn2');
        })
    }
});
define(['jym', 'dialog'], function(jym, dialog){
    var setDefaultBank = function(obj){
        $('.my-card-link .card-yet').replaceWith('<span class="card-set">设为默认卡</span>');
        obj.find('.card-set').replaceWith('<span class="card-yet">默认卡</span>');
    };
    var bankDefaultSend = function(o){
        var obj = $(o);
        if(obj.find('.card-yet').length){
            return false;
        }
        var card = o.getAttribute('data-card');
        jym.sync({
            url : jym.api.bankcard.SetDefault,
            data : {bankCardNo : card},
            success : function(){
                setDefaultBank(obj);
            },
            error : function(d){
                jym.msgInfo(d.responseJSON.Message);
            }
        });
    };

    var obj = {
        init : function(){
            this.bind();
        },
        add : function (){
            var that = this, tpl = $('#addBankList-tpl').html();
            if(!this.$bank){
                var bank = this.$bank = new dialog();
                $.extend(bank, {
                    init_once:jym.once(function(){
                        this.init_dialog('bank');
                    })
                });
            }
            this.$bank.show(tpl, '添加银行卡', function(t){
                if(t == 1){ setTimeout(function(){ BankDialog.show() }, 500); }
            });
            SelectBank.bind("#bankSelect");
            that.bind();
        },
        bind : function(){
            var that = this;
            jym.action.extend({
                "addCardSubmit":function(){
                    that.submit();
                },
                "setDefault" : function(){
                    bankDefaultSend(this);
                },
                "add-new-bank":function(){
                    that.add();
                }
            });
            $('#ctt-bankcard').focus(function(){
                jym.msgError(this, null, !1);
                jym.msgInfo(null, !1);
            });
            if(jym.browser.ie6){
                $('.my-card-link').hover(function(){
                    $(this).find('.card-set').show();
                }, function(){
                    $(this).find('.card-set').hide();
                });
            }
        },
        send:function(d){
            var api = jym.api.bankcard.AddBankCard;
            jym.sync({
                type:'post',
                url:api,
                data:d,
                success:function(){
                    obj.$bank.hide(1);
                },
                error:function(d){
                    jym.msgInfo(d.responseJSON.Message);
                }
            });
        },
        submit:function(){
            if(!SelectBank.data || $('.select-blank').length < 1){
                jym.msgInfo('请选择开户银行');
                return false;
            }
            var card = jym.trimAll($('#ctt-bankcard').val());
            if(!jym.isBankCard(card)){
                jym.msgError('#ctt-bankcard','请正确输入银行卡号');
                return false;
            }
            var data = $.extend({BankCardNo:card}, SelectBank.data);
            this.send(data);
        }
    }

    var CanSyncStatus = true;
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
                    BankDialog.$obj.hide();
                    dialog.mask.hide();
                    $('#ctt-bank-dialog-msg').hide();
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
            html += '<a href="#" class="ui-action ctt-bank-dialog-close" data-action="ctt-bank-dialog-close">&#215;</a>';
            html += '<div id="ctt-bank-dialog-msg" class="ui-msg" style="display:none;">'+p+'</div>';
            html += '<div class="ctt-bank-dialog-wrap">';
            html += '<button class="ui-action btn-common" data-action="ctt-bank-dialog-btn">电话认证成功</button>';
            html += '<a href="#" '+style+' class="ui-action" data-action="ctt-bank-dialog-fail">认证失败？</a>';
            html += '<div class="ctt-bank-dialog-span">如有问题，请联系4008-556-333</div>';
            html += '</div></div>';
            this.$obj = $(html).appendTo('body');
            jym.action.extend({
                "ctt-bank-dialog-btn":function(){
                    BankDialog.status.get();
                },
                "ctt-bank-dialog-close":function(){
                    CanSyncStatus = true;
                    BankDialog.$obj.hide();
                    dialog.mask.hide();
                    $('#ctt-bank-dialog-msg').hide();
                },
                "ctt-bank-dialog-fail":function(){
                    CanSyncStatus = true;
                    BankDialog.$obj.hide();
                    dialog.mask.hide();
                    $('#ctt-bank-dialog-msg').hide();
                    obj.add();
                }
            });
        }),
        show:function(){
            this.create();
            this.$obj.show();
            dialog.mask.show(990);
        }
    };

    var SelectBank = {
        bind:function(selector){
            var target = this.$target = $(selector);
            this.initData(jymBanks);
            jym.action.extend({
                "select-bank":function(){
                    var o = $(this).closest('ul').find('.handle-blank');
                    o.removeClass('select-blank');
                    o.find('i').remove();
                    $(this).addClass('select-blank').append('<i></i>');
                    SelectBank.select(this);
                    jym.msgInfo('', 0);
                }
            });
        },
        initData:function(banks){
            var tpl = '<ul class="blank-list clearfix">';
            var city = '上海|上海';
            $.each(banks, function(i, v){
                tpl += '<li><a href="#" class="ui-action handle-blank" data-action="select-bank" data-bank="'+v.name+'" data-city="'+(v.defaults||city)+'">';
                tpl += '<em class="ctt-xSelect-bank '+v.code+'">'+v.name+'</em></a></li>';
            });
            tpl += '</ul>';
            this.$target.html(tpl);
        },
        select:function(o){
            this.data = {};
            this.data.BankName = o.getAttribute('data-bank');
            this.data.CityName = o.getAttribute('data-city');
            jym.msgError('#bankSelect','');
        }
    };

    obj.init();
    return obj;
});
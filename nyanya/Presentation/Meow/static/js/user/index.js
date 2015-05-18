zim.ns('zim.page.jym1118', {
    __getData:function(callback){
        var api = '/api/v1/Orders/GetActivityStatus1000';
        zim.batchSync([
            {url:'/api/v1/Orders/GetActivityStatus1000'},
            {url:'/api/v1/Luckhub/GetUserCanPalyStatu'}
        ], function(d, d1){
            zim.fire(callback, [d, d1])
        });
    },
    __status20:function(){
        var obj = $('#jym1118');
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="#" data-action="set-1ksy">'
        s += '<span style="color:#F60;">立即使用1000元理财金</span></a></li></ul>';
        obj.html(s);
        zim.action.extend({
            "set-1ksy":function(){
                zim.ui.alert({
                    msg:'将为您选购的产品增加1000元理财本金，本金不显示在投资金额中，不可提现，产品到期可获得相对应的收益！',
                    btnOk:'知道了',
                    callback:function(){
                        zim.location('/amp/list')
                    }
                });
            }
        });
    },
    __status30:function(d){
        var obj = $('#jym1118');
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="#" data-action="set-1ksy">';
        s += '<span style="color:#F60;">已获得1000元理财金收益'+d.ExtraInterest+'元</span></a></li></ul>';
        obj.html(s);
        zim.action.extend({
            "set-1ksy":function(){
                zim.ui.alert({
                    msg:'本金不显示在投资金额中，不可提现，产品到期可获得相对应的收益！',
                    btnOk:'知道了'
                });
            }
        });
    },
    __lottery10:function(){
        var obj = $('#jymlottery');
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="/amp/list">';
        s += '<span style="color:#F60;">投资1元即可获得抽奖机会，GO！</span></a></li></ul>';
        obj.html(s);
    },
    __lottery20:function(){
        var obj = $('#jymlottery');
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="/page/topic/1118/lottery">';
        s += '<span style="color:#F60;">您获得1次抽奖机会，立即抽奖！</span></a></li></ul>';
        obj.html(s);
    },
    __lottery30:function(d){
        var obj = $('#jymlottery');
        var g = {
            "10":"恭喜您，获得IPhone6一台",
            "20":"恭喜您，获得银钞一张",
            "30":"恭喜您，获得U盘一个",
            "40":"恭喜您，获得5元现金红包一个",
            "50":"恭喜您，获得2元现金红包一个",
            "60":"未中奖，感谢您的参与"
        };
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="/page/topic/1118/lottery">';
        s += '<span style="color:#F60;">'+g[d.AwardLevel]+'</span></a></li></ul>';
        obj.html(s);
    },
    __lottery50:function(){
        var obj = $('#jymlottery');
        var s = '<ul class="jqm-menu-nav jqm-index-nav">';
        s += '<li><a class="ui-link" href="/page/topic/1118/lottery">';
        s += '<span style="color:#F60;">抽奖无效，请查看活动规则！</span></a></li></ul>';
        obj.html(s);
    },
    home:function(){
        var that = this;
        this.__getData(function(d, d1){
            if(d.data.Status < 40){
                zim.fire(that['__status'+d.data.Status], d.data);
            }
            if(d1.data){
                zim.fire(that['__lottery'+d1.data.Status], d1.data);
            }
        });
    }
});
zim.ns('zim.page.user.index', {
    init:function(){
        this.bind();
        this.render();
    },
    fixPrice:function(p){
        var s = Math.floor(p * 1000000000) / 1000000000;
        return s.toString().replace(/[0]{2,}1$/, '');
    },
    logout_location:function(){
        if(zim.urlQuery("app") == "android"){
            zim.location('/passport/login');
        } else {
            zim.location('/');
        }
    },
    bind:function(){
        var that = this;
        zim.action.extend({
            "jqm-funds-share":function(){
                var txt = $('#jqm-index-view .jqm-span-funds').text();
                if(that.hasOrder){
                    txt = txt + $('#jqm-index-view .income-block p').text();
                }
                var url = zim.sns.weibo({title:txt, url:window.location.href});
                zim.location(url);
            },
            "index-logout":function(){
                zim.user.logout({
                    success:that.logout_location,
                    error:that.logout_location
                });
            }
        });
    },
    getData:function(fn){
        zim.sync({
            url:zim.api.user.index,
            success:fn,
            error:zim.user.callback_error
        });
    },
    render:function(){
        var that = this;
        this.getData(function(d){
            zim.tplRender('#jqm-index-view', '#jqm-tpl-invsummary', d);
            zim.page.jym1118.home();
        });
    }
});
zim.ns('zim.page.user.funds', {
    init:function(){
        this.render();
    },
    getData:function(fn){
        zim.sync({
            url:zim.api.user.info,
            success:fn,
            error:zim.user.callback_error
        });
    },
    render:function(){
        var that = this;
        this.getData(function(d){
            if(d.HasDefaultBankCard){
                that.IndexDataRender(d);
            } else {
                zim.tplRender('#jqm-funds-view', '#jqm-tpl-noIdenty', {User:d});
            }
        });
    },
    IndexDataRender:function(UserData){
        zim.page.user.index.getData(function(d){
            zim.tplRender('#jqm-funds-view', '#jqm-tpl-view', {User:UserData, Account:d});
        });
    }
});
zim.ns('zim.page.user.settings', {
    init:function(){
        this.bind();
    },
    bind:function(){
        zim.action.extend({
            "reset-password-user":function(){
                zim.location("/passport/reset");
            },
            "reset-password-payment":function(){
                zim.user.validate(function(d){
                    if(!d.HasDefaultBankCard){
                        zim.ui.confirm.show({
                            msg:'您还未开通快捷支付，暂时无法使用此功能。',
                            btnOk:'立即开通',
                            callback:function(t){
                                if(!d.HasSetPaymentPassword){
                                    zim.location('/user/yilian/password');
                                } else {
                                    zim.location('/user/yilian/identity');
                                }
                            }
                        })
                    } else {
                        zim.location('/passport/payment');
                    }
                });
            }
        });
    }
});
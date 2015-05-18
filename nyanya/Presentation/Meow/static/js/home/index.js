zim.ns('zim.ui.slider', {
    init:function(wrap_id, nav_id){
        var that = this;
        var wrap = this.$wrap = $(wrap_id);
        this.wrap_id = wrap_id.slice(1);
        this.$cache = wrap.html();
        this.$nav = $(nav_id);
        this._getElem();
        this._bindScroll();
        zim.orichange.add(function(e){
            zim.ga.event('index-orientation', e.orientation);
            setTimeout(function(){
                that._reInit();
            }, 500);
        });
    },
    _reInit:function(){
        this.objScroll && this.objScroll.destroy();
        this.objScroll = null;
        this.$wrap.html(this.$cache).removeAttr('style');
        this._getElem();
        this._bindScroll();
    },
    _bindScroll:function(){
        var that = this;
        this.objScroll = new iScroll(this.wrap_id, {
            snap: true,
            momentum: false,
            hScrollbar: false,
            hScroll:true,
            onBeforeScrollStart: function ( e ) {
                if(zim.ua.isUC || this.absDistX > (this.absDistY + 5 ) ) {
                    e.preventDefault();
                }
            },
            onTouchEnd: function () {
                var self = this;
                if (self.touchEndTimeId) { clearTimeout(self.touchEndTimeId); }
                self.touchEndTimeId = setTimeout(function () {
                    self.absDistX = 0;
                    self.absDistY = 0;
                }, 600);
            },
            onScrollEnd: function () {
                that._change_nav(this.currPageX+1);
            }
        });
    },
    _getElem:function(wrap_id, nav_id){
        var ul = this.$ul = this.$wrap.find('ul');
        var items = this.$items = this.$wrap.find('li');
        this.length = items.length;
        this._init_width();
        this._init_nav();
    },
    _init_width:function(){
        var w = this._el_width = document.body.clientWidth;
        var h = this._el_height = w * 0.421875;
        var len = this.$items.length;
        var p = {"position":"absolute","left":"0","top":"0"};
        this.$wrap.width(w).height(h);
        this.$items.width(w).height(h).css({'float':'left','display':'block'});
        this.$ul.width(w * len).css(p);
    },
    _init_nav:function(){
        var len = this.length;
        if(len < 2){ return; }
        var i = 0, html = [], cls = 'currIndex';
        for(;i<len;i++){
            html.push('<li class="'+cls+'"></li>');
            cls = '';
        }
        this.$nav.html(html.join(''));
        var ml = this.$nav.width()/2;
        this.$nav.css('margin-left',-ml);
    },
    _change_nav:function(index){
        this.$nav.find('li.currIndex').removeClass('currIndex');
        this.$nav.find('li:nth-child('+index+')').addClass('currIndex');
    }
});
/*
 * page index render
 */
zim.ns('zim.page.index', {
    init:function(){
        this.banner.render();
        this.recommend.render();
        this.safe.render();
    },
    recommend:{
        getData:function(fn){
            zim.sync({
                url:zim.api.indexTop, 
                success:fn
            });
        },
        render:function(){
            this.getData(function(d){
                zim.tplRender('#jqm-index-top', '#jqm-tpl-top', d);
            });
        }
    },
    banner:{
        getData:function(fn){
            if(typeof cms_ad_19 != 'undefined' && cms_ad_19){
                fn({Banners:cms_ad_19.ADImage});
            }
        },
        render:function(){
            this.getData(function(d){
                zim.tplRender('#jqm-slide-banner', '#jqm-tpl-banner', d);
                zim.ui.slider.init('#jqm-slide-gallery', '#jqm-slide-nav');
            });
        }
    },
    safe:{
        render:function(){
            zim.tplRender('#jqm-index-safe', '#jqm-tpl-safe');
        }
    }
});
zim.ns('zim.page.feedback', {
    init:function(){
        this.bind();
    },
    bind:function(){
        var that = this;
        $('#jqm-feedback-btn').on('click', function(){
            that.send()
        });
    },
    send:function(){
        var Content = $.trim($('#jqm-feedback-content').val());
        if(!Content){
            zim.ui.message('请输入您的宝贵意见.');
            return false;
        } else if(Content.length > 200){
            Content = Content.substr(0, 200);
        }
        var data = {
            Content:Content
        };
        zim.sync({
            type:"POST",
            url:zim.api.feedbacks,
            data:data,
            success:function(){
                zim.ui.message('您的意见发送成功.', function(){
                    zim.page.hackBack();
                });
            },
            error:function(){
                zim.ui.message('发送失败，请稍后再试.');
            }
        });
    }
});
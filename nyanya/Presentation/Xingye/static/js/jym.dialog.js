define(['jquery', 'jym'], function($, jym){
    var fixIframe = function(selector, s){
        if(jym.browser.ie6){
            var obj = $(selector);
            s = $.extend({
                    top     : 'auto', // auto == .currentStyle.borderTopWidth
                    left    : 'auto', // auto == .currentStyle.borderLeftWidth
                    width   : 'auto', // auto == offsetWidth
                    height  : 'auto', // auto == offsetHeight
                    opacity : true,
                    src     : 'javascript:false;'
            }, s || {});
            var prop = function(n){return n&&n.constructor==Number?n+'px':n;},
                html = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="'+s.src+'"'+
                           'style="display:block;position:absolute;z-index:-1;'+
                                   (s.opacity !== false?'filter:Alpha(Opacity=\'0\');':'')+
                                           'top:'+(s.top=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')':prop(s.top))+';'+
                                           'left:'+(s.left=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')':prop(s.left))+';'+
                                           'width:'+(s.width=='auto'?'expression(this.parentNode.offsetWidth+\'px\')':prop(s.width))+';'+
                                           'height:'+(s.height=='auto'?'expression(this.parentNode.offsetHeight+\'px\')':prop(s.height))+';'+
                                    '"/>';
            return obj.each(function() {
                    if ( $('> iframe.bgiframe', this).length == 0 )
                            this.insertBefore( document.createElement(html), this.firstChild );
            });
        }
    };
    var mask = {
        _init_once:jym.once(function(){
            var mask = document.createElement('div');
            mask.className = 'ui-mask';
            mask.style.display = 'none';
            document.body.appendChild(mask);
            this.$mask = $(mask);
            if(jym.browser.ie6){
                var that = this;
                fixIframe(mask);
                $(window).resize(function(){
                    that._setSize();
                });
            }
        }),
        _setSize:function(){
            if(jym.browser.ie6){
                var body = document.documentElement;
                var width = body.clientWidth;
                var height = $(document).height();
                this.$mask.css({
                    "width":width,
                    "height":height
                });
            }
        },
        isShow:!1,
        show:function(zIndex){
            this._init_once();
            if(!this.isShow){
                this.isShow = !0;
                this.$mask.show().css("zIndex", zIndex || "99991");
                this._setSize();
            }
        },
        hide:function(fx){
            if(this.isShow){
                this.isShow = !1;
                this.$mask[fx||'hide']();
            }
        }
    };
    var dialog = function(){};
    $.extend(dialog.prototype, {
        _init_tpl:function(type){
            var tpl = '<div class="ui-dlg-header clearfix"><h1>{{title}}</h1><a href="#" id="dialog-close-'+type+'" class="ui-action ui-dlg-close">&#215;</a></div>';
            tpl += '<div class="ui-dlg-main">{{content}}</div>';
            var tpl_alert = tpl + '<div class="ui-dlg-btn"><button class="ui-action ui-btn" data-action="btn-alert-ok">{{btnOk}}</button></div>';
            var tpl_confirm = tpl + '<div class="ui-dlg-btn"><button class="ui-action ui-btn1" data-action="btn-confirm-cancel">{{btnCancel}}</button>';
                tpl_confirm += '<button class="ui-action ui-btn" data-action="btn-confirm-ok">{{btnOk}}</button></div>';
            var obj = {'alert':tpl_alert, 'confirm':tpl_confirm};
            return function(opts){
                var html = obj[type] || tpl;
                opts = $.extend({title:'',content:'', btnOk:'确定', btnCancel:'取消'}, opts);
                return html.replace(/\{\{([\w]+)\}\}/g, function(a,b,c){ return opts[b] || ""; });
            };
        },
        init_dialog:function(type){
            var that = this;
            var dialog = document.createElement('div');
            var id = 'dialog-' + type;
            dialog.id = id;
            dialog.className = 'ui-dialog ui-' + type;
            var $d = $('#' + id);
            if($d.length < 1){
                document.body.appendChild(dialog);
            } else {
                $d.fadeOut();
            }
            this.$dialog = $d = $('#' + id);
            this.tpl = this._init_tpl(type);
            $(document).on('click', '#dialog-close-'+type,function(){
                that.hide(!1);
            });
        },
        init_show:function(msg, title, callback){
            var opt = $.extend({
                wrap:'<div class="ui-dlg-msg">{{html}}</div>',
                mask:!0
            }, typeof msg === 'string' ? {msg:msg, callback:callback} : msg);
            opt.title = title;
            opt.content = opt.wrap ? opt.wrap.replace("{{html}}", opt.msg) : opt.msg;
            var html = this.tpl(opt);
            this.callback = $.isFunction(opt.callback) ? jym.once(opt.callback) : $.noop;
            this.$dialog.html(html).show();
            this.hasMask = opt.mask;
            if(opt.mask){ mask.show(); }
        },
        show:function(msg, title, callback){
            this.init_once();
            this.init_show(msg, title, callback);
            jym.fire(this.auto_hide, null, this);
        },
        hide:function(){
            this.$dialog.fadeOut();
            if(this.hasMask){ mask.hide('fadeOut') }
            jym.fire(this.callback, arguments);
        }
    });
    var obj_message = new dialog();
    $.extend(obj_message, {
        init_once:jym.once(function(){
            this.init_dialog('message');
        }),
        auto_hide:function(){
            var that = this;
            setTimeout(function(){ that.hide() }, 2000);
        }
    });
    var obj_alert = new dialog();
    $.extend(obj_alert, {
        init_once:jym.once(function(){
            var that = this;
            this.init_dialog('alert');
            jym.action.extend({
                "btn-alert-ok":function(){ that.hide(); }
            });
        })
    });
    var obj_confirm = new dialog();
    $.extend(obj_confirm, {
        init_once:jym.once(function(){
            var that = this;
            this.init_dialog('confirm');
            jym.action.extend({
                "btn-confirm-ok":function(){
                    jym.fire(that.callback, true);
                    that.hide();
                },
                "btn-confirm-cancel":function(){
                    jym.fire(that.callback, false);
                    that.hide();
                }
            });
        })
    });
    dialog.fixIframe = fixIframe;
    dialog.mask = mask;
    dialog.message = obj_message;
    dialog.alert = obj_alert;
    dialog.confirm = obj_confirm;
    jym.dialog = dialog;
    return dialog;
});
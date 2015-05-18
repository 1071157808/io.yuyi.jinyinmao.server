(function(root, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['zepto', 'exports'], function($, exports) {
            root.zim = factory(root, exports, $);
        });
    } else if (typeof exports !== 'undefined') {
        var $ = require('zepto');
        factory(root, exports, $);
    } else {
        root.zim = factory(root, {}, (root.Zepto || root.$));
    }
}(this, function(root, zim, $) {
    var window = root;
    var core_slice = Array.prototype.slice;
    var rvalidchars = /^[\],:{}\s]*$/,
        rvalidbraces = /(?:^|:|,)(?:\s*\[)+/g,
        rvalidescape = /\\(?:["\\\/bfnrt]|u[\da-fA-F]{4})/g,
        rvalidtokens = /"[^"\\\r\n]*"|true|false|null|-?(?:\d+\.|)\d+(?:[eE][+-]?\d+|)/g;
    var mix = function(){
        var copy, name, options;
        var target = arguments[0] || {}, i = 1, length = arguments.length;
        if(length === i){
            target = this;
            --i;
        }
        if(typeof target !== "object" && !$.isFunction(target)){ target = {} }
        for(;i<length;i++){
            if((options = arguments[i]) != null){
                for(name in options){
                    if(target[name] !== undefined){
                        throw new Error(name + ' in target was defined property.');
                    }
                    copy = options[name];
                    if(target === copy){ continue; }
                    if(copy !== undefined){ target[ name ] = copy; }
                }
            }
        }
        return target;
    };
    var arr_escape = ['&', '<', '>', '"', "'"];
    var arr_unescape = ['&amp;', '&lt;', '&gt;', '&quot;', '&#x27;'];
    var entity = {
        escape: new RegExp('[' + arr_escape.join('') + ']', 'g'),
        unescape: new RegExp('(' + arr_unescape.join('|') + ')', 'g'),
        map:{}
    };
    $.each(arr_escape, function(i, v){
        entity.map[v] = arr_unescape[i];
    });
    $.each(arr_unescape, function(i, v){
        entity.map[v] = arr_escape[i];
    });
    $.each(['escape', 'unescape'], function(i, v){
        zim[v] = function(string){
            if (string == null) return '';
            return ('' + string).replace(entity[v], function(match) {
                return entity.map[match];
            });
        };
    });
    var noMatch = /(.)^/;
    var escapes = {"'":"'", '\\':'\\', '\r':'r', '\n':'n', '\t':'t', '\u2028':'u2028', '\u2029':'u2029'};
    var escaper = /\\|'|\r|\n|\t|\u2028|\u2029/g;
    var fn_once = function(func){
        var ran = false, memo;
        return function() {
            if (ran) return memo;
            ran = true;
            memo = func.apply(this, arguments);
            func = null;
            return memo;
        };
    };
    mix(zim, {
        mix: mix,
        once:fn_once,
        noop:function(){},
        tplSettings: {
            evaluate    : /<%([\s\S]+?)%>/g,
            interpolate : /<%=([\s\S]+?)%>/g,
            escape      : /<%-([\s\S]+?)%>/g
        },
        tpl:function(text, data, settings) {
            var render;
            settings = $.extend({}, settings, zim.tplSettings);
            var matcher = new RegExp([
                (settings.escape || noMatch).source,
                (settings.interpolate || noMatch).source,
                (settings.evaluate || noMatch).source
            ].join('|') + '|$', 'g');
            var index = 0;
            var source = "__p+='";
            text.replace(matcher, function(match, escape, interpolate, evaluate, offset) {
                source += text.slice(index, offset)
                .replace(escaper, function(match) { return '\\' + escapes[match]; });
                if (escape) {
                    source += "'+\n((__t=(" + escape + "))==null?'':zim.escape(__t))+\n'";
                }
                if (interpolate) {
                    source += "'+\n((__t=(" + interpolate + "))==null?'':__t)+\n'";
                }
                if (evaluate) {
                    source += "';\n" + evaluate + "\n__p+='";
                }
                index = offset + match.length;
                return match;
            });
            source += "';\n";
            if (!settings.variable) source = 'with(obj||{}){\n' + source + '}\n';
            source = "var __t,__p='',__j=Array.prototype.join," +
            "print=function(){__p+=__j.call(arguments,'');};\n" +
            source + "return __p;\n";
            try {
                render = new Function(settings.variable || 'obj', 'zim', source);
            } catch (e) {
                e.source = source;
                throw e;
            }
            if (data) return render(data, zim);
            var template = function(data) {
                return render.call(this, data, zim);
            };
            template.source = 'function(' + (settings.variable || 'obj') + '){\n' + source + '}';
            return template;
        },
        tplRender:function(el, el_tpl, data, isAppend){
            var tpl = $(el_tpl).text();
            var tpl_render = zim.tpl(tpl, data);
            $(el)[isAppend ? 'append' : 'html'](tpl_render);
        },
        ns:function(strNS, property){
            if (typeof strNS === 'string') {
                var nss = strNS.split('.'), parent = window;
                if (strNS.charAt(0) === '.') {
                    nss.shift();
                }
                while (strNS = nss.shift()) {
                    parent[strNS] = parent[strNS] || {};
                    parent = parent[strNS];
                }
                if ($.isFunction(property)) {
                    property.call(parent);
                } else if ($.isPlainObject(property)) {
                    mix(parent, property);
                }
                return parent;
            }
            return strNS;
        },
        fire:function(fn, args, target){
            if($.isFunction(fn)){
                args = this.isLikeArray(args) ? args : [args];
                return fn.apply(target || window, args);
            }
        },
        newDate:function(d, offset){
            d = new Date(d);
            offset = d.getTimezoneOffset() + (offset || 8) * 60;
            return offset ? new Date(d.getTime() - offset * 60000) : d;
        },
        strDataObject:function(s){
            if(/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/.test(s)){
                return {
                    year:RegExp.$1-0, 
                    month:RegExp.$2-1, 
                    day:RegExp.$3-0, 
                    hour:RegExp.$4-0, 
                    minute:RegExp.$5-0, 
                    seconds:RegExp.$6-0
                };
            }
            return null;
        },
        strToDate:function(s){
            var o = this.strDataObject(s);
            if(o){
                var d = new Date(o.year, o.month, o.day, o.hour, o.minute, o.seconds);
                return this.newDate(d);
            }
            return null;
        },
        getGuid:function(){ 
            return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15); 
        },
        oneObject:function(s, defVal){
            var obj = {};
            defVal = defVal == undefined ? 1 : defVal;
            s.replace(/[^\s]+/g, function(k){ obj[k] = defVal; });
            return obj;
        },
        isBankCard:function(card){
            return /^[0-9]{15,19}$/.test(card);
        },
        isEmail: function(val){
            return /^[a-zA-Z0-9\._-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/.test(val);
        },
        isMobile:function(v){
            return /^1(3|4|5|7|8)\d{9}$/.test(v);
        },
        isPassword:function(s){
            return s === this.trimAll(s) && /^[a-zA-Z\d~!@#$%^&*_]{6,18}$/.test(s);
        },
        isPayPassword:function(s){
            return s === this.trimAll(s) && /^(?![^a-zA-Z~!@#$%^&*_]+$)(?!\D+$).{8,18}$/.test(s);
        },
        isVerifyCode:function(code){
            return /^[0-9]{6}$/.test(code);
        },
        isLikeArray:function(obj){
            if(obj && typeof obj === 'object'){
                var length = obj.length;
                if(+length === length && length >= 0 && obj.hasOwnProperty(length-1)){
                    return true;
                }
            }
            return false;
        },
        isRealName:function(d){
            var v = this.trimAll(d);
            if(v.length < 2 || !/^[\u4e00-\u9fa5]{2,10}$/.test(v)){
                return false;
            }
            return true;
        },
        isIDCard:function(code){
            var tip = "", pass = true;
            var regx = /^[1-9][0-9]{5}(19[0-9]{2}|200[0-9]|2010)(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])[0-9]{3}[0-9xX]$/i;
            if (!code || !regx.test(code)) {
                tip = "身份证号码格式错误";
                pass = false;
            } else if(!city[code.substr(0,2)]){
                tip = "地址编码错误";
                pass = false;
            } else {
                if(code.length == 18){
                    code = code.split('');
                    var factor = [ 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 ];
                    var parity = [ 1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2 ];
                    var sum = 0, ai = 0, wi = 0;
                    for (var i = 0; i < 17; i++){
                        ai = code[i];
                        wi = factor[i];
                        sum += ai * wi;
                    }
                    var last = parity[sum % 11];
                    if(parity[sum % 11] != code[17]){
                        tip = "校验位错误";
                        pass =false;
                    }
                }
            }
            return {msg:tip, pass:pass};
        },
        sy_price:function(price, expected_yield, period, year_day){
            price = Math.floor(price * expected_yield * period / (year_day || 360)) / 100;
            return this.autoFixed(price);
        },
        autoFixed:function(s){
            return parseFloat(s).toFixed(2).replace(/\.?[0]+$/,'');
        },
        trimAll:function(v){
            if(v == null){return "";}
            return v.replace(/\s/g,"");
        },
        urlQuery:function(name){
            var s = window.location.search.slice(1);
            if(!s) return null;
            if(name){
                return new RegExp(name + '=([^\\&]*)').test(s) ? decodeURIComponent(RegExp.$1) : null;
            } else {
                var p = {};
                s.replace(/([^\?\&]+)=([^\&]*)/g, function(a,b,c){
                    p[b] = decodeURIComponent(c);
                });
                return p;
            }
        },
        parseJSON:function(data){
            if(window.JSON && window.JSON.parse){ return window.JSON.parse( data ); }
            if(data === null){ return data; }
            if(typeof data === "string"){
                data = $.trim(data);
                if(data){
                    data = data.replace(rvalidescape, "@")
                    .replace(rvalidtokens, "]")
                    .replace(rvalidbraces, "");
                    if(rvalidchars.test(data)){
                        return (new Function("return " + data))();
                    }
                }
            }
            return null;
        },
        stringify:function(obj) {
            var m = {'\b':'\\b','\t':'\\t','\n':'\\n','\f':'\\f','\r':'\\r','"' :'\\"','\\':'\\\\'},
                s = {
                    'array': function(x){var a=['['],b,f,i,l=x.length,v;for(i=0;i<l;i+=1){v=x[i];f=s[typeof v];if(f){v=f(v);if(typeof v=='string'){if(b){a[a.length]=','}a[a.length]=v;b=true}}}a[a.length]=']';return a.join('')},
                    'boolean': function(x){return String(x)},
                    'null': function(x){return 'null'},
                    'number': function(x){return isFinite(x)?String(x):'null'},
                    'object': function(x){if(x){if(x instanceof Array){return s.array(x)}var a=['{'],b,f,i,v;for(i in x){v=x[i];f=s[typeof v];if(f){v=f(v);if(typeof v=='string'){if(b){a[a.length]=','}a.push(s.string(i),':',v);b=true}}}a[a.length]='}';return a.join('')}return'null'},
                    'string': function(x){if(/["\\\x00-\x1f]/.test(x)){x=x.replace(/([\x00-\x1f\\"])/g,function(a,b){var c=m[b];if(c){return c}c=b.charCodeAt();return'\\u00'+Math.floor(c/16).toString(16)+(c%16).toString(16)})}return'\"'+x+'\"'}
                };
            return s.object(obj);
        },
        delayCallback:function(callback, delay){
            if($.isFunction(callback)){
                delay = delay || 2000;
                return setTimeout(callback, delay);
            }
        },
        countdown:function(opt){
            if($.isFunction(opt)){ opt = {callback:opt}; }
            if($.isFunction(opt.callback)){
                opt = $.extend({count:60, delay:1000}, opt);
                opt.callback(opt.count);
                var djs = function(){
                    opt.count--;
                    opt.callback(opt.count);
                    if(opt.count <= 0){
                        clearInterval(IntervalID);
                        IntervalID = null;
                    }
                };
                var IntervalID = setInterval(djs, opt.delay);
            }
        },
        bindCallback:function(selector, callback){
            var obj = $(selector);
            var guid = this.getGuid();
            window[guid] = function(){ zim.fire(callback, [obj, selector]) };
            obj.attr('data-callback', guid);
        },
        routes:fn_once(function(routes){
            if(typeof mRoute === 'undefined'){ throw new Error('framework mRoute must be loaded.'); }
            var Router = mRoute.Router.extend({routes:routes});
            var route = new Router, obj = {
                navigate:function(name){
                    route.navigate(name, true);
                    return this;
                },
                invoke:route
            };
            mRoute.history.start();
            return obj;
        }),
        location:function(url){
            if(zim.urlQuery("app") == "android"){
                if(!/(\?|\&)app=android/.test(url)){
                    url += (url.indexOf('?')>-1 ? '&' : '?') + 'app=android';
                }
            }
            window.location.href = url;
        },
        appCall:function(method, d){
            d = this.stringify(d);
            var url = 'jym://'+method+'?'+encodeURIComponent(d);
            var iframe = document.createElement("IFRAME");
            iframe.setAttribute("src", url);
            document.documentElement.appendChild(iframe);
            iframe.parentNode.removeChild(iframe);
            iframe = null;
        },
        params:function(txt, obj, unescape){
            var encode = unescape ? function(s){return s} : function(s){return encodeURIComponent(s)};
            return txt.replace(/\{(\w+)\}/g, function(a, b){
                return obj[b]==undefined?"":encode(obj[b]);
            });
        },
        sync:function(setting){
            if(!$.isFunction(setting.success)){
                throw new Error('call:' + setting.url + ', no success callback');
            }
            var ajax_api_url = setting.url;
            if(ajax_api_queue[ajax_api_url]){return false;}
            if(!setting.type || /^(get|delete)$/i.test(setting.type.toLowerCase())){
                if(setting.data){
                    setting.url = zim.params(setting.url, setting.data);
                    delete setting.data;
                }
                var f = setting.url.indexOf('?')>0 ? "&t=" : "?t=";
                setting.url = setting.url + f + zim.getGuid();
            }
            var loadOptions = {
                beforeSend: function(data, status, xhr){ zim.ui.loading.show() },
                complete: function(xhr, ex){ zim.ui.loading.hide() }
            };
            if(setting.loading == false){
                loadOptions = {};
                delete setting.loading;
            }
            var ajaxOptions = $.extend(loadOptions, ajax_opt_defaults, setting);
            var _complete = ajaxOptions.complete;
            var _error = ajaxOptions.error;
            ajaxOptions.complete = function(){
                ajax_api_queue[ajax_api_url] = 0;
                return zim.fire(_complete, arguments, this);
            };
            ajaxOptions.error = function(xhr, status, ex){
                xhr.responseJSON = zim.parseJSON(xhr.responseText || "{}");
                return zim.fire(_error, [xhr, status, ex], this);
            };
            ajax_api_queue[ajax_api_url] = 1;
            return $.ajax(ajaxOptions);
        },
        batchSync:function(requests, callback){
            var reqs = [], url = '';
            $.each(requests, function(i, r){
                if(r.data){
                    url = zim.params(r.url, r.data);
                    delete r.data;
                } else { url = r.url; }
                reqs.push({Method:'get',RelativeUrl:url.slice(1)});
            });
            $.ajax({
                type:'POST',
                dataType: 'json',
                timeout: 20000,
                url:zim.api.batch,
                data:{batch:'{requests:'+zim.stringify(reqs)+'}'},
                success:function(d){
                    $.each(d, function(i, v){
                        try{
                            d[i].data = zim.parseJSON(v.Body);
                        }catch(e){ d[i].data = {}; }
                    });
                    zim.fire(callback, d);
                }
            });
        },
        log:function(o){
            return function(){ if(o && o.log) { o.log.apply(o, arguments); } };
        }(window.console)
    });
    var ajax_api_queue = $['@ajax_api_queue'] = {};
    var ajax_opt_defaults = {
        type: 'get',
        dataType: 'json',
        timeout: 20000,
        error: function(xhr, status, ex){
            zim.ui.message('<p style="text-align:center;line-height:4em;">网络不给力，请稍后再试！</p>');
        }
    };
    return zim;
}));
zim.ns('zim.env', function(){
    var host = window.location.hostname;
    var protocol = window.location.protocol;
    var regx_prd = /^m\.jinyinmao\.com\.cn$/i;
    var regx_test = /^mtest\.jinyinmao\.com\.cn$/i;
    if(regx_prd.test(host)){
        this.isPrd = !0;
    } else if(regx_test.test(host)){
        this.isTest = !0;
    } else {
        this.isDev = !0;
    }
    this.server_url = protocol + '//' + host + '/';
});
zim.ns('zim.ua', function(){
    var ua = window.navigator.userAgent;
    this.userAgent = ua;
    this.isUC = ua.indexOf('UCBrowser') > -1;
    if(/\sMicroMessenger\/([\d\.]+)/i.test(ua)){
        this.weixin = RegExp.$1;
    }else if(/\sWeibo\s\((.*)__weibo__([\d\.]+)__(.*)\)/i.test(ua)){
        this.weibo = RegExp.$2;
    }
    if(/;\sAndroid\s([\d\.]+);/.test(ua)){
        this.android = RegExp.$1;
    } else if(/(iPad|iPod).*OS\s([\d_]+)/.test(ua)){
        this[RegExp.$1] = this['ios'] = RegExp.$2;
    } else if(/(iPhone\sOS)\s([\d_]+)/.test(ua)){
        this['iPhone'] = this['ios'] = RegExp.$2;
    }
    this.Callback = function(fn){
        return zim.fire(fn, this);
    };
});
zim.ns('zim.support', function(){
    var __Callbacks = $.Callbacks({once:true, memory:true});
    var __position = "";
    (function(div){
        div.style.cssText = 'display:none;position:fixed;z-index:100;';
        document.body.appendChild(div);
        __position = window.getComputedStyle(div).position;
        div.parentNode.removeChild(div);
    }(document.createElement('div')));
    this.position = __position;
    this.addCallback = function(){
        __Callbacks.add.apply(this.__Callbacks, arguments);
    };
    __Callbacks.fire(this);
});
zim.ns('zim.cookie', {
    set:function(name, value, options){
        if (typeof value != 'undefined') {
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '', date;
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else { date = options.expires; }
                expires = '; expires=' + date.toUTCString();
            }
            var path = '; path=' + (options.path ? options.path : '/');
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        }
    },
    get:function(name){
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = $.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    },
    remove:function(name){
        this.set(name, null);
    }
});
zim.ns('zim.storage', function(){
    var win = window, hostname = win.location.hostname;
    var sStorage = win.sessionStorage;
    var lStorage = win.localStorage || (win.globalStorage && win.globalStorage.namedItem(hostname));
    var supportStorage = function(target, object){
        try{
            target.setItem('hostname', hostname);
            target.removeItem('hostname');
            return target;
        }catch(e){
            zim.log(e);
            return object;
        }
    };
    var localStorage = supportStorage(lStorage, {
        _mode:'cookie',
        setItem:function(key, value){ zim.cookie.set(key, value, {expires:3650}); },
        getItem:function(key){ return zim.cookie.get(key); },
        removeItem:function(key){ zim.cookie.remove(key); }
    });
    var sessionStorage = supportStorage(sStorage, {
        _mode:'cookie',
        setItem:function(key, value){ zim.cookie.set(key, value); },
        getItem:function(key){ return zim.cookie.get(key); },
        removeItem:function(key){ zim.cookie.remove(key); }
    });
    this.localMode = localStorage._mode || 'localStorage';
    this.snapMode = sessionStorage._mode || 'sessionStorage';
    this.setItem = localStorage.setItem;
    this.getItem = localStorage.getItem;
    this.removeItem = localStorage.removeItem;
    this.snap = function(key, value){
        if(typeof value === 'undefined'){
            return sessionStorage.getItem(key);
        } else if(value == null){
            sessionStorage.removeItem(key);
        } else {
            sessionStorage.setItem(key, value);
        }
    };
});
zim.ns('zim.OCode', {
    ozfac:function(tprm, hash){
        if(window['__ozfac2'] && window['__ozfac2'].call){
            window['__ozfac2'](tprm, hash);
        }
    },
    login:function(id){
        this.ozfac("userid="+id, "#loginok");
    },
    register:function(id){
        this.ozfac("userid="+id, "#regok");
    }
});
zim.ns('zim.ga', {
    add:function(){
        var ga = window['_ga'];
        ga && ga.apply && ga.apply(window, arguments);
        return this;
    },
    pv:function(data){
        return this.add('send', 'pageview', data);
    },
    event:function(Category, Action, Label, Value){
        return this.add('send', 'event', Category, Action, Label, Value);
    },
    trackEvent:function(track){
        if(track){
            track = track.split('|');
            if(track.length > 1){
                var label = track[2] || undefined;
                var value = track[3] ? parseFloat(track[3]) : undefined;
                this.event(track[0], track[1], label, value);
            }
        }
    }
});
zim.ns('zim.action', {
    _fn_list:{},
    _handle:function(e){
        var el = $(this), name = el.attr('data-action');
        var track_event = el.attr('data-track-event');
        if(track_event){ zim.ga.trackEvent(track_event); }
        if (name) {
            var callback = zim.action._fn_list[name];
            zim.fire(callback, [e, name], this);
        } else {
            var src = el.attr('href') || el.attr('data-href');
            if(src && src.replace(/#/g,'') !== ''){ zim.location(src); }
        }
        e.preventDefault();
        return false;
    },
    bind:function(selector){
        $(document).on('click', selector, this._handle);
    },
    extend:function(){
        zim.mix.apply(this._fn_list, arguments);
    }
});
zim.ns('zim.sns', {
    weibo:function(opt){
        var url = "http://service.weibo.com/share/share.php?title={title}&url={url}&pic={pic}&ralateUid=3822187413&type=link&language=zh_cn&searchPic=false&style=simple";
        return zim.params(url, opt);
    }
});
zim.ns('zim.ui', {
    RadioGroup:function(opt){
        return this.RadioGroup.init(opt);
    },
    SinglePage:function(opt){
        return this.SinglePage.init(opt);
    },
    message:function(){
        this.message.show.apply(this.message, arguments);
    },
    alert:function(){
        this.alert.show.apply(this.alert, arguments);
    },
    confirm:function(){
        this.confirm.show.apply(this.confirm, arguments);
    },
    pieSlider:function(opts){
        if(!opts.id) throw new Error("must be canvas id.");
        var canvas = document.getElementById(opts.id), ctx;
        if(canvas && (ctx = canvas.getContext("2d"))){
            canvas.width = canvas.height = "200";
            var noop = function(){};
            var before = opts.onBefore || noop;
            var after = opts.onAfter || noop;
            before(ctx);
            ctx.fillStyle = opts.color || '#e74c3c';
            var step = opts.step || 1;
            var delay = opts.delay || 10;
            var i = (opts.start || 0), rage = 360 * (opts.percent || 0);
            var sRage = -Math.PI * 0.5;
            var djs = function(){
                i = i + step;
                if(i <= rage){
                    ctx.beginPath();
                    ctx.moveTo(100, 100);   
                    ctx.arc(100, 100, 100, sRage, Math.PI * 2 * (i/360)+sRage);
                    ctx.fill();
                    setTimeout(djs, delay);
                } else { after(ctx); }
            };
            djs();
        }
    }
});
zim.ns('zim.ui.loading', {
    "@useCount":1,
    show:function(){
        if(!this["@useCount"]){ $('#zim-loading').show(); }
        this["@useCount"]++;
    },
    hide:function(){
        this["@useCount"]--;
        if(this["@useCount"]<=0){
            this["@useCount"] = 0;
            $('#zim-loading').hide();
        }
        this.removeMask();
    },
    removeMask:zim.once(function(){
        $('#zim-loading-mask').remove();
    })
});
zim.ns('zim.ui.Dialog', {
    _init_tpl:function(type){
        var content = '<div class="ui-dialog-header"><h5><span><%=title%></span></h5></div>';
            content += '<div class="ui-dialog-content"><%=content%></div>';
        var btn1 = '<div class="ui-dialog-footer">';
            btn1 += '<button class="ui-action ui-btn1" data-action="btn-alert-ok"><%=btnOk%></button></div>';
        var btn2 = '<div class="ui-dialog-footer">';
            btn2 += '<button class="ui-action ui-btn2" data-action="btn-confirm-cancel"><%=btnCancel%></button>';
            btn2 += '<button class="ui-action ui-btn1" data-action="btn-confirm-ok"><%=btnOk%></button></div>';
        return function(opts){
            opts = $.extend({title:'喵喵提示', content:'', btnOk:'确定', btnCancel:'取消'}, opts);
            var html = content;
            if(type === 'alert'){ html += btn1; } else if(type === 'confirm'){ html += btn2; }
            return zim.tpl(html, opts);
        };
    },
    init_dialog:function(type){
        var dialog = document.createElement('div');
        dialog.id = 'zim-' + type;
        dialog.className = 'ui-dialog ui-' + type;
        document.body.appendChild(dialog);
        this.$dialog = $(dialog);
        this.tpl = this._init_tpl(type);
    },
    init_show:function(msg, callback){
        var opt = $.extend({
            wrap:'<p class="ui-dialog-wrap">{html}</p>'
        }, typeof msg === 'string' ? {msg:msg, callback:callback} : msg);
        opt.content = opt.wrap ? opt.wrap.replace("{html}", opt.msg) : opt.msg;
        var html = this.tpl(opt);
        this.callback = $.isFunction(opt.callback) ? zim.once(opt.callback) : zim.noop;
        this.$dialog.html(html);
        this._show();
    },
    show:function(msg, callback){
        this.init_once();
        this.init_show(msg, callback);
        zim.fire(this.auto_hide, null, this);
    },
    _show:function(){
        this.$dialog.css({display:'block', opacity:.95});
        var width = this.$dialog.width()/2;
        this.$dialog.css('margin-left', -width);
    },
    hide:function(){
        this.$dialog.fadeOut();
        zim.fire(this.callback);
    }
});
zim.ns('zim.ui.message', function(){
    $.extend(this, zim.ui.Dialog, {
        init_once:zim.once(function(){
            this.init_dialog('message');
        }),
        auto_hide:function(){
            var that = this;
            setTimeout(function(){ that.hide() }, 2000);
        }
    });
});
zim.ns('zim.ui.alert', function(){
    $.extend(this, zim.ui.Dialog, {
        init_once:zim.once(function(){
            var that = this;
            this.init_dialog('alert');
            zim.action.extend({
                "btn-alert-ok":function(){ that.hide(); }
            });
        })
    });
});
zim.ns('zim.ui.confirm', function(){
    $.extend(this, zim.ui.Dialog, {
        init_once:zim.once(function(){
            var that = this;
            this.init_dialog('confirm');
            zim.action.extend({
                "btn-confirm-ok":function(){
                    zim.fire(that.callback, true);
                    that.hide();
                },
                "btn-confirm-cancel":function(){
                    zim.fire(that.callback, false);
                    that.hide();
                }
            });
        })
    });
});
zim.ns('zim.ui.RadioGroup', function(){
    var RadioGroup = function(opt){
        opt = $.extend({
            parent:null,
            items:null,
            className:'ui-selected'
        }, opt);
        var parent = $(opt.parent);
        $(opt.items, parent).on('tap.selected', function(e){
            $('.'+opt.className, parent).removeClass(opt.className);
            $(this).addClass(opt.className);
            zim.fire(opt.callback, e, this);
        });
    };
    this.init = RadioGroup;
});
zim.ns('zim.ui.SinglePage', function(){
    var createPageContent = function(id, callback){
        var div = document.createElement('div');
        div.id = id;
        div.className = 'ui-single-page';
        div.style.display = 'none';
        zim.fire(callback, div);
        return div;
    };
    var getUniquePage = function(id){
        var div = document.getElementById(id);
        return div || createPageContent(id);
    };
    var SinglePageContent = function(title, html){
        var tpl = '<div class="ui-header">';
            tpl += '<div class="ui-wrapper">';
            tpl += '<div class="ui-left">';
            tpl += '<a class="jqm-page-back" data-action="back" href="#">返回</a>';
            tpl += '</div>';
            tpl += '<h1 class="jqm-title">'+title+'</h1>';
            tpl += '</div>';
            tpl += '</div>';
            tpl += '<div class="ui-content">'+html+'</div>';
        return tpl;
    };
    var SinglePage = function(opt){
        opt = $.extend({
            selector:'.ui-page',
            parent:'.viewport',
            html:''
        }, opt);
        var id = opt.id || ('single-page-' + zim.getGuid());
        var div = getUniquePage(id);
        var remove_div = function(){
            if(div){
                div.innerHTML = '';
                div.parentNode.removeChild(div);
                div = null;
            }
        };
        div.innerHTML = SinglePageContent(opt.title, opt.html);
        $(opt.parent).append(div);
        var obj = {
            id:id,
            status:0,
            _hash:{},
            inject_page_back:function(fn){
                if(!$.isFunction(fn)){
                    fn = function(){ this.hide(); };
                }
                var old_back = this._page_back = zim.page.back;
                zim.page.back = function(){
                    if(obj.status){
                        fn.apply(obj);
                    } else {
                        old_back.apply(this);
                    }
                };
                return this;
            },
            activePage:null,
            show:function(callback){
                if(this.status){ return this; }
                if(!this.activePage) {
                    this.activePage = $(opt.selector);
                }
                this.activePage.css('display','none');
                div.style.display = '';
                this.status = 1;
                this._hash.title = document.title;
                document.title = opt.title;
                zim.fire(callback, null, this);
                return this;
            },
            hide:function(callback){
                if(!this.status){ return this; }
                if(this.activePage) {
                    this.activePage.css('display','');
                }
                div.style.display = 'none';
                this.status = 0;
                document.title = this._hash.title;
                zim.fire(callback, null, this);
                return this;
            },
            html:function(s){
                $('.ui-content', div).html(s);
                return this;
            },
            destroy:function(callback){
                zim.fire(callback, div, this);
                this.activePage = null;
                if(this._page_back){
                    zim.page.back = this._page_back;
                    this._page_back = null;
                }
                remove_div();
            }
        };
        zim.fire(opt.callback, null, obj);
        return obj;
    };
    this.init = SinglePage;
});
zim.ns('zim.user', {
    init:function(){
        if(typeof _CheckedLoginStatus != 'undefined' && _CheckedLoginStatus){
            if(_CheckedLoginStatus == 2){
                this.__checkLogin();
            }
            this.__pageTopLoad();
            this.__getData();
        }
    },
    addCallback:function(){
        this.__Callbacks.add.apply(this.__Callbacks, arguments);
    },
    callback_error:function(xhr, type, error){
        if(xhr.status === 401){
            zim.location(zim.user.loginUrl());
        } else {
            zim.ui.message('<p style="text-align:center;line-height:4em;">网络不给力，请稍后再试！</p>');
        }
    },
    loginUrl:function(url){
        return '/passport/login?backUrl=' + encodeURIComponent(url || location.href);
    },
    validate:function(){
        this.__OnceValidData();
        this.__ValidCallbacks.add.apply(this.__ValidCallbacks, arguments);
    },
    __OnceValidData:function(){
        var that = this;
        this.__ValidCallbacks = $.Callbacks({memory:true});
        zim.sync({
            url:zim.api.user.info,
            success:function(d){
                that.__ValidCallbacks.fire(d);
            },
            error:function(d){
                that.__ValidCallbacks.fire(d);
            }
        });
    },
    __checkLogin:function(){
        var that = this;
        this.addCallback(function(status){
            if(!status){ zim.location(that.loginUrl()); }
        });
    },
    __pageTopLoad:function(){
        var that = this;
        this.addCallback(function(status){
            if(!status){
                var user = $('#topNav-user');
                user.attr('href', that.loginUrl(user.attr('href')));
            }
        });
    },
    __Callbacks:$.Callbacks({memory:true}),
    __getData:function(){
        var that = this;
        zim.sync({
            url:zim.api.user.status,
            success:function(d){
                that.__Callbacks.fire(d.Valid, d.LoginName);
            }
        });
    },
    logout:function(opt){
        opt = $.extend({url:zim.api.passport.SignOut}, opt);
        zim.sync(opt);
    }
});
zim.ns('zim.orichange', {
    _change_callbacks:$.Callbacks({memory:true}),
    _chnage_bind_once:zim.once(function(){
        var that = this;
        $(window).on("orientationchange", function(e){
            that._change_callbacks.fire(e);
        });
    }),
    add:function(){
        this._chnage_bind_once();
        this._change_callbacks.add.apply(this._change_callbacks, arguments);
        return this;
    }
});
zim.ns('zim.pager', {
    _scrollEnd_callbacks:$.Callbacks({memory:true}),
    _scrollEnd_bind_once:zim.once(function(){
        var that = this;
        var bdy = document.body;
        var win = $(window).on('scroll', function(e){
            var sh = bdy.scrollHeight;
            var ch = bdy.clientHeight;
            var st = win.scrollTop();
            if(sh - ch - st < 100){
                that._scrollEnd_callbacks.fire(e);
            }
        });
    }),
    onScrollEnd:function(){
        this._scrollEnd_bind_once();
        this._scrollEnd_callbacks.add.apply(this._scrollEnd_callbacks, arguments);
        return this;
    }
});
zim.ns('zim.fixIOS', {
    __Callbacks:$.Callbacks({memory:true}),
    __bind:zim.once(function(){
        var that = this;
        window.addEventListener('pageshow', function(event){
            if (event.persisted) { that.__Callbacks.fire(event); }
        }, false);
    }),
    onPageShow:function(){
        this.__bind();
        this.__Callbacks.add.apply(this.__Callbacks, arguments);
        return this;
    },
    fixedPosition:function(opt){
        var fixed = $(opt.fixed);
        var fixBlur = function(){
            setTimeout(function(){
                window.scrollTo(0, Math.random());
            }, 10);
        };
        if(zim.ua.ios && fixed.css('position')==='fixed'){
            $(opt.selector).on('focus', function(){
                fixed.css({'position':'absolute'});
            }).on('blur', function(){
                fixed.css({'position':'fixed'});
                if(opt.fixBlur){
                    fixBlur();
                } else if($.isFunction(opt.callback)){
                    zim.fire(opt.callback);
                }
            });
        }
    },
    fixedHeader:function(selector, fixBlur){
        this.fixedPosition({
            selector:selector, 
            fixed:'.ui-header .ui-wrapper',
            fixBlur:!!fixBlur
        });
    }
});
/*
 * page api intterface
 */
zim.ns('zim.api', {
    batch:'/Api/V1/$batch',
    feedbacks:'/Api/V1/Meow/Feedbacks',
    banner:'https://cmsadmin.jinyinmao.com.cn/caches/poster_js/19.js',
    indexTop:'/api/v1/ProductInfo/BA/Top',
    SaleProcess:'/api/v1/ProductInfo/SaleProcess?productIdentifier={productIdentifier}'
});
zim.ns('zim.api.agreement', {
    Consignment:'/api/v1/ProductInfo/ConsignmentAgreement?productIdentifier={id}',
    Pledge:'/api/v1/ProductInfo/PledgeAgreement?productIdentifier={id}',
    OrderConsignment:'/api/v1/Orders/ConsignmentAgreement?orderIdentifier={id}',
    OrderPledge:'/api/v1/Orders/PledgeAgreement?orderIdentifier={id}'
});
zim.ns('zim.api.amp', {
    list:'/api/v1/ProductInfo/BA/Page?number={number}',
    detail:'/api/v1/ProductInfo/BA?productNo={productNo}'
});
zim.ns('zim.api.cpl', {
    list:'/api/v1/ProductInfo/TA/Page?number={number}',
    detail:'/api/v1/ProductInfo/TA?productNo={productNo}'
});
zim.ns('zim.api.order', {
    amp_list:'/api/v1/Orders/BA?pageIndex={pageIndex}&sortMode={sortMode}',
    amp_fail:'/api/v1/Orders/BA/Failed',
    cpl_list:'/api/v1/Orders/TA?pageIndex={pageIndex}&sortMode={sortMode}',
    cpl_fail:'/api/v1/Orders/TA/Failed',
    detail:'/api/v1/Orders?orderIdentifier={orderIdentifier}'
});
zim.ns('zim.api.passport', {
    SignIn:"/api/v1/User/SignIn",
    SignUp:"/api/v1/User/SignUp",
    SignOut:"/api/v1/User/SignOut",
    CheckCellphone:"/api/v1/User/CheckCellphone?cellphone={cellphone}",
    ResetPwd:"/api/v1/User/ResetLoginPassword"
});
zim.ns('zim.api.user', {
    info:"/api/v1/UserInfo",
    index:"/api/v1/UserInfo/Index",
    status:"/api/v1/UserInfo/Login",
    summary:"/api/v1/UserInfo/Index/Settlings",
    bankcards:'/api/v1/UserInfo/BankCards'
});
zim.ns('zim.api.bankcard', {
    setPassword:'/api/v1/User/SetPaymentPassword',
    SignUpPayment:'/api/v1/User/SignUpPayment',
    AddBankCard:'/api/v1/User/AddBankCard',
    SetDefault:'/api/v1/User/SetDefaultBankCard?bankCardNo={bankCardNo}',
    AddBankCardStatus:'/api/v1/User/AddBankCardStatus'
});
zim.ns('zim.api.payment', {
    Investing:'/api/v1/Investing',
    ResetPaymentPassword:"/api/v1/User/ResetPaymentPassword"
});
zim.ns('zim.api.sms', {
    sendCode:'/api/v1/VeriCodes/Send',
    verifyCode:'/api/v1/VeriCodes/Verify'
});

zim.ns('zim.eHash', {
    IdType:{
        "0":"身份证",
        "1":"护照",
        "2":"台湾同胞来往内地通行证",
        "3":"军人证"
    },
    SellingStatus:{
        "10":"待售",
        "20":"预售",
        "30":"抢购",
        "40":"已售罄",
        "50":"已结束"
    },
    GuaranteeMode:{
        "10":"银行保兑",
        "20":"央企担保",
        "30":"国企担保",
        "40":"国有担保公司担保",
        "50":"担保公司担保",
        "60":"上市集团担保",
        "70":"集团担保",
        "80":"国资参股担保公司担保"
    }
});
/*
 * page action callback
 */
zim.action.extend({
    "back":function(){ 
        zim.page.back();
    },
    "checkbox":function(){
        $(this).toggleClass("ui-selected");
        var callback = this.getAttribute('data-callback');
        if(callback && window[callback]){
            zim.fire(window[callback], [this]);
        }
    }
});
/*
 * page api interface
 */
zim.ns('zim.page', {
    getBankClass:function(name){
        var b = zimBanks, cls = '';
        $.each(b, function(i, v){
            if(name == v.name){
                cls = v.code;
                return false;
            }
        });
        return cls;
    },
    fmtDate:function(date){
        return date.split('T')[0];
    },
    fmtDateTime:function(date){
        return date.split('T').join(' ');
    },
    fmtValueDate:function(data){
        if(data.ValueDateMode < 30){
            return data.ValueDateString;
        } else if(data.ValueDateMode == 30){
            return this.fmtDate(data.ValueDate);
        }
        return "";
    },
    toLocalTime:function(t){
        t = t.split('T')[0];
        return t.replace(/-/g,'/');
    },
    scrollY:function(y){
        setTimeout(function(){ window.scrollTo(0, y||0); }, 0);
    },
    resetScroll:function(){
        this.scrollY(0);
    },
    isMyUrl:function(url){
        return url && /^http(s)?\:\/\/([\w\.]+\.)?jinyinmao\.com\.cn/i.test(url);
    },
    isSafeUrl:function(url){
        if(!url){ return false; }
        if(/^http(s)?\:\/\//i.test(url)){
            return this.isMyUrl(url);
        }
        return true;
    },
    checkMobile:function(mobile, callback){
        zim.sync({
            url:zim.api.passport.CheckCellphone,
            data:{cellphone:mobile},
            success:callback
        });
    },
    checkQuery:function(name, callback){
        var value = zim.urlQuery(name);
        if(!value){
            if(!$.isFunction(callback)){
                callback = function(){ zim.page.back() };
            }
            zim.fire(callback, value);
        } else {
            return value;
        }
    },
    back:function(){
        var df = document.referrer;
        if(!df || this.isMyUrl(df)){
            if(window.history.length > 1){
                window.history.back(-1);
            } else {
                zim.location('/');
            }
        } else {
            zim.location('/');
        }
    },
    hackBack:function(){
        var df = document.referrer;
        if(!df || this.isMyUrl(df)){
            zim.location(df);
        } else {
            zim.location('/');
        }
    },
    getProductName:function(name, number){
        if (number == -1){ return name; }
        return name + '第' + number + '期';
    },
    getProductStatus:function(status){
        if(status < 30){
            return "<em>" + zim.eHash.SellingStatus[status] + "</em>";
        }else if(status == 30){
            return "<strong>" + zim.eHash.SellingStatus[status] + "</strong>";
        } else {
            return zim.eHash.SellingStatus[status];
        }
    },
    getOrderStatus:function(status){
        switch(status){
            case 10: return "付款中";
            case 20: return "待起息";
            case 30: return "已起息";
            case 40: return "已结息";
            case 50: return "支付失败";
        }
        return "异常订单";
    },
    getExtraYield:function(yield){
        return yield>0?'+'+yield+'%':'';
    },
    getOverDay:function(due){
        var now = zim.newDate(zim.page.toLocalTime(_timespan) + ' 00:00:00');
        due = zim.newDate(zim.page.toLocalTime(due) + ' 00:00:00');
        if(now >= due){ return "0"; } else {
            return (due - now) / (24 * 60 * 60 * 1000);
        }
    },
    setPieSlider:function(id, percent, start){
        zim.ui.pieSlider({
            id:id,
            start:start,
            percent:percent/100
        });
    },
    setPieOverDay:function(id, d){
        var vday = zim.newDate(zim.page.toLocalTime(d.SoldOutTime) + ' 00:00:00');
        var now = zim.newDate(zim.page.toLocalTime(_timespan) + ' 00:00:00');
        var due = zim.newDate(zim.page.toLocalTime(d.RepaymentDeadline) + ' 00:00:00');
        var due_day = now >= due ? 0 : (due - now) / (24 * 60 * 60 * 1000);
        var all_day = (due - vday) / (24 * 60 * 60 * 1000);
        var percent = 1 - due_day / all_day;
        zim.ui.pieSlider({
            id:id,
            percent:percent,
            color:'#f4f6f6',
            onBefore:function(ctx){
                ctx.fillStyle = '#47b28b';
                ctx.beginPath();
                ctx.moveTo(100, 100);   
                ctx.arc(100, 100, 100, 0, Math.PI * 2);
                ctx.fill();
            }
        });
    }
});
/*
 * page init callback
 */
Zepto(function(){
    var page_source = zim.urlQuery();
    if(page_source && page_source.from){
        zim.ga.event('page-from', page_source.from, 'app-installed-' + page_source.isappinstalled);
    }
    zim.support.addCallback(function(ns){
        if(ns.position != 'fixed'){
            document.documentElement.className = 'not-fixed';
        }
    });
    zim.action.bind('a');
    zim.action.bind('.ui-action');
    zim.ui.loading.hide();
    zim.user.init();
});
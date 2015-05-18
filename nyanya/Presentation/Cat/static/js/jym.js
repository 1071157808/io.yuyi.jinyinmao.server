(function(root, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['jquery', 'exports'], function($, exports) {
            root.jym = factory(root, exports, $);
        });
    } else if (typeof exports !== 'undefined') {
        var $ = require('jquery');
        factory(root, exports, $);
    } else {
        root.jym = factory(root, {}, (root.jQuery || root.$));
    }
}(this, function(root, jym, $) {
    var hostname = location.hostname.toLowerCase();
    var env_name = "Dev";
    if(/(www\.|^)jinyinmao\.com\.cn/.test(hostname)){
        env_name = "Prd";
    } else if(/www\.test\.ad\.jinyinmao\.com\.cn/.test(hostname)){
        env_name = "Test";
    }
    jym['is'+env_name] = true;
    $.extend(jym, {
        fire:function(fn, args, target){
            if($.isFunction(fn)){
                args = this.isLikeArray(args) ? args : [args];
                return fn.apply(target || window, args);
            }
        },
        once:function(func){
            var ran = false, memo;
            return function() {
                if (ran) return memo;
                ran = true;
                memo = func.apply(this, arguments);
                func = null;
                return memo;
            };
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
        isMyUrl:function(url){
            var regx_cat = /^http(s)?\:\/\/([\w\.]+\.)?jinyinmao\.com\.cn/i;
            var regx_xy = /^http(s)?\:\/\/([\w\.]+\.)?cib\.com\.cn/i;
            if(!jym.isPrd){
                regx_xy = /^http(s)?\:\/\/([\w\.]+\.)?jymtest\.com/i;
            }
            return url && (regx_cat.test(url) || regx_xy.test(url));
        },
        isSafeUrl:function(url){
            if(!url){ return false; }
            if(/^http(s)?\:\/\//i.test(url)){
                return this.isMyUrl(url);
            }
            return true;
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
        isRealName:function(d){
            var v = jym.trimAll(d);
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
        isCatUser:function(UserType){ return UserType >> 1 & 1; },
        isXYUser:function(UserType){ return UserType & 1; },
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
            if(ajax_api_queue[ajax_api_url]){
                return false;
            }
            if(!setting.type || /^(get|delete)$/i.test(setting.type.toLowerCase())){
                if(setting.data){
                    setting.url = jym.params(setting.url, setting.data);
                    delete setting.data;
                }
                var f = setting.url.indexOf('?')>0 ? "&t=" : "?t=";
                setting.url = setting.url + f + jym.getGuid();
            }
            var ajaxOptions = $.extend({}, ajax_opt_defaults, setting);
            var _complete = ajaxOptions.complete;
            ajaxOptions.complete = function(){
                ajax_api_queue[ajax_api_url] = 0;
                return _complete.apply(this, arguments);
            };
            ajax_api_queue[ajax_api_url] = 1;
            return $.ajax(ajaxOptions);
        },
        sy_price:function(price, expected_yield, period, year_day){
            return Math.floor(price * expected_yield * period / (year_day || 360)) / 100;
        },
        formPost:function(url, data, target){
            var tget = target ? target !== true ? 'target="'+target+'"' : 'target="_blank"' : '';
            var tpl = '<form action="'+url+'" method="post" '+tget+' style="display:none;">';
            $.each(data, function(k, v){
                tpl += '<input type="hidden" name="'+k+'" value="'+v+'" />';
            });
            tpl += '<\/form>';
            $(tpl).appendTo('body').submit();
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
        intTObject:function(total){
            var day=Math.floor(total/86400000),
                hour=Math.floor(total/3600000%24),
                min=Math.floor(total/60000%60),
                sec=Math.floor(total/1000%60),
                to2=function(v){ return (v<10?"0":"") + v; };
            return {d:to2(day), h:to2(hour), m:to2(min), s:to2(sec)};
        },
        msgError:function(selector, msg, visable){
            var jq = $(selector), tip = jq.siblings('.ui-tip-msg');
            if(visable != false){
                if(!tip.length){
                    tip = $('<div class="ui-tip-msg"><p class="ui-ico-error"></p><b class="ui-tip-left"></b></div>');
                    jq.after(tip);
                }
                tip.show().find('p').html(msg);
            } else if(tip.length) {
                tip.hide();
            }
        },
        login_backUrl:function(url){
            var loginUrl = "/passport/login";
            if(url !== ""){ loginUrl += "?backUrl=" + encodeURIComponent(url || location.href); }
            location.href = loginUrl;
        },
        loadProgress:function(percent, callback){
            var count = 0,
                plus = function () {
                    callback(count);
                    if (count == percent || count == 100) {
                        clearInterval(interval);
                        interval = null;
                    };
                    count++;
                }, interval = setInterval(plus,10);
        },
        // 发送手机验证码
        sendValidateCode:function(mobile, type, callback, time){
            time = time || 120;
            this.sync({
                type:"POST",
                url : jym.api.sms.sendValidateCode,
                data : {Cellphone : mobile, Type : type},
                success : function (d) {
                    if(d.RemainCount > 0){
                        jym.countdown({
                            count:time,
                            callback:function(i){ callback(!0, i) }
                        });
                    } else {
                        callback(-1);
                    }
                },
                error : function (d) { callback(!1) }
            })
        },
        // 验证发送的验证码
        verifyCode:function(mobile, code, type, callback){
            this.sync({
                type:"POST",
                url : jym.api.sms.checkValidateCode,
                data : {Cellphone : mobile, Code : code, Type : type},
                success : function (d) {
                    if(d.RemainCount == -1){ // 失效
                        callback(-1);
                    } else if(d.Successful){
                        callback(!0, d.Token);
                    } else {
                        callback(!1);
                    }
                },
                error : function (d) {
                    callback(!1);
                }
            });
        },
        //检测手机号是否唯一
        checkMobile:function(v, callback){
            v = $.trim(v);
            this.sync({
                type:"GET",
                url : jym.api.passport.CheckCellphone + '?cellphone=' + v,
                success : function (d) {
                    callback(d.Result, d);
                },
                error : function (d) {
                    callback(!1, d);
                }
            });
        }
    });
    var city = jym.oneObject("11 12 13 14 15 21 22 23 31 32 33 34 35 36 37 41 42 43 44 45 46 50 51 52 53 54 61 62 63 64 65 71 81 82 91");
    var ajax_api_queue = $['@ajax_api_queue'] = {};
    var ajax_opt_defaults = {
        type: 'get',
        dataType: 'json',
        timeout: 20000,
        beforeSend: function(data, status, xhr){jym.loading()},
        complete: function(xhr, ex){jym.loading.hide()},
        error: function(xhr, status, ex){
            require(['dialog'], function(d){
                d.message.show('网络不给力，请稍后再试！')
            })
        }
    };
    var fixPlaceholder = function(){
        var _this = $(this);
        if(_this.attr('data-fixholder') == 'false'){ return; }
        var newObj = $('<input type="text" data-type="hidden" value="'+_this.attr('placeholder')+'" autocomplete="off" class="'+_this[0].className+'" />');
        _this.hide().before(newObj);
        newObj.css('color','#CCC').focus(function(){
            newObj.hide();
            _this.show().focus();
        });
        _this.blur(function(){
            if(_this.val() == '') {
                newObj.show();
                _this.hide();
            }
        });
    };
    if ("placeholder" in document.createElement("input")) {
        jym.placeholder = function(selector){ return $(selector) };
        jym.placeholder.trigger = $.noop;
        jym.placeholder.show = $.noop;
    } else {
        jym.placeholder = function(selector){ return $(selector).each(fixPlaceholder) };
        jym.placeholder.trigger = function(selector){
            $(selector).prev('[data-type=hidden]').trigger('focus');
        };
        jym.placeholder.show = function(selector){
            $(selector).hide().prev('[data-type=hidden]').show();
        };
        $(function(){
            jym.placeholder('input[placeholder]').each(function(){
                if(this.value){jym.placeholder.trigger(this)}
            });
        });
    }
    jym.cookie = function(name, value, options){
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
        } else {
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
        }
    };
    var dateTimeTast = $.Callbacks('memory');
    var dateTimeTast_init = jym.once(function(){
        setInterval(function(){ dateTimeTast.fire() }, 1000);
    });
    jym.timeClock = {
        add:function(){
            dateTimeTast.add.apply(dateTimeTast, arguments);
            dateTimeTast_init();
        },
        remove:function(){
            dateTimeTast.remove.apply(dateTimeTast, arguments);
        },
        clear:function(){
            dateTimeTast.empty();
        }
    };
    jym.loading = function(){jym.loading.show()};
    jym.loading["@useCount"] = 0;
    jym.loading._create = jym.once(function(){
        var tpl = '<div id="jym-loading" class="ui-loading">';
        tpl += '<span class="ui-loading-icon"></span>';
        tpl += '<h3>正在加载....</h3></div>';
        $("body").append(tpl);
    });
    jym.loading.show = function(){
        this._create();
        if(!this["@useCount"]){ $('#jym-loading').show(); }
        this["@useCount"]++;
    };
    jym.loading.hide = function(){
        this["@useCount"]--;
        if(this["@useCount"]<=0){
            this["@useCount"] = 0;
            $('#jym-loading').hide();
        }
    };
    (function(UA, browser){
        browser = jym.browser = {};
        if(/msie 6\./.test(UA)){
            browser.ie6 = !0;
        }
    }(window.navigator.userAgent.toLowerCase()));
    jym.OCode = {
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
    };
    jym.ga = {
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
    };
    jym.action = {
        _fn_list:{},
        _init_once:jym.once(function(){
            this.bind('.ui-action');
        }),
        _handle:function(e){
            var el = $(this), name = el.attr('data-action');
            var track_event = el.attr('data-track-event');
            if(track_event){ jym.ga.trackEvent(track_event); }
            if (name) {
                var callback = jym.action._fn_list[name];
                jym.fire(callback, [e, name], this);
            } else {
                var src = el.attr('href') || el.attr('data-href');
                if(src && src.replace(/#/g,'') !== ''){ location.href = src; }
            }
            e.preventDefault();
            return false;
        },
        bind:function(selector){
            $(document).on('click', selector, this._handle);
        },
        extend:function(){
            $.extend.apply(this._fn_list, arguments);
            this._init_once();
        }
    };
    jym.notice = {
        init:function(){
            var el = document.getElementById('top-marquee');
            if(el){this.render(el);}
        },
        trimHtml:function(s){
            return s.replace(/^(<[^>]+>)\1*/g,'').replace(/(<[^>]+>)\1*$/g,'').replace(/[\r\n]+/g,'');
        },
        render:function(mq){
            require(['https://source.jinyinmao.com.cn/api/notice.php'], function(d){
                if(d){
                    var html = '<span>'+jym.notice.trimHtml(d.content)+'</span>';
                    $(mq).html(html);
                    jym.notice.setMarquee(mq);
                }
            });
        },
        setMarquee:function(obj){
            var width = obj.offsetWidth, i = 0, delay = 30;
            var html = obj.innerHTML;
            obj.innerHTML = html + html;
            var scroll = function(){
                if(i >= width){
                    obj.style.left = '0px';
                    i = 0;
                } else {
                    obj.style.left = '-'+i+'px';
                    i++;
                }
            };
            var time = setInterval(scroll, delay);
            $(obj).hover(function(){
                clearInterval(time);
                time = null;
            }, function(){
                time = setInterval(scroll, delay);
            });
        }
    };
    jym.eHash = {};
    jym.eHash.IdType = {
        "0":"身份证",
        "1":"护照",
        "2":"台湾同胞来往内地通行证",
        "3":"军人证"
    };
    jym.eHash.ampStatus = {
        "10":"待售",
        "20":"预售",
        "30":"抢购",
        "40":"售罄",
        "50":"结束"
    };
    jym.api = {};
    jym.api.amp = {
        SaleProcess:'/api/v1/ProductInfo/SaleProcess?productIdentifier={productIdentifier}'
    };
    jym.api.user = {
        info:"/api/v1/UserInfo"
    };
    jym.api.passport = {
        SignIn:"/api/v1/user/SignIn",
        SignUp:"/api/v1/user/SignUp",
        SignOut:"/api/v1//User/SignOut",
        loginStatus:"/api/v1/UserInfo/Login",
        CheckCellphone:"/api/v1/User/CheckCellphone",
        ResetPwd:"/api/v1/User/ResetLoginPassword",
        ResetPaymentPassword:"/api/v1/User/ResetPaymentPassword"
    };
    jym.api.sms = {
        sendValidateCode : '/api/v1/VeriCodes/Send',
        checkValidateCode : '/api/v1/VeriCodes/Verify'
    };
    jym.api.bankcard = {
        setPassword:'/api/v1/User/SetPaymentPassword',
        SignUpPayment:'/api/v1/User/SignUpPayment',
        AddBankCard:'/api/v1/User/AddBankCard',
        SetDefault:'/api/v1/User/SetDefaultBankCard?bankCardNo={bankCardNo}'
    };
    jym.api.payment = {
        Investing:'/api/v1/Investing'
    };
    var loginStatusCallbacks = $.Callbacks("once memory");
    jym.load = {
        login_tpl:function(){
            var tpl = '<a class="ui-nav-login" href="/user/index">我的金银猫</a>';
            tpl += '<a class="ui-nav-register" href="#" onclick="jym.load.logout();return false;">安全退出</a>';
            return tpl;
        },
        loginStatus:function(){ loginStatusCallbacks.add.apply(loginStatusCallbacks, arguments); },
        login:function(){
            jym.sync({
                url:jym.api.passport.loginStatus,
                beforeSend:$.noop,
                complete:$.noop,
                error:$.noop,
                success:function(d){ loginStatusCallbacks.fire(d); }
            });
        },
        logout:function(){
            jym.sync({
                url:jym.api.passport.SignOut,
                success:function(){ location.href = "/"; }
            });
        }
    };
    jym.load.loginStatus(function(d){
        if(d.Valid){ $('#nav-passport-status').html(jym.load.login_tpl()); }
    });
    $(function(){ jym.notice.init() });
    root.XY_HOST = "https://xy.dev.jymtest.com";
    var xy_location = location.hostname.toLowerCase();
    if(/(www\.|^)jinyinmao\.com\.cn/.test(xy_location)){
        XY_HOST = "https://piao.cib.com.cn";
    } else if(/test\.ad\.jinyinmao\.com\.cn/.test(xy_location)){
        XY_HOST = "https://xy.test.jymtest.com";
    }
    //================================
    if(false && !jym.cookie('ysbNotice')){
        require(['dialog'], function(dialog){
            var notice = new dialog;
            $.extend(notice, {
                init_once:jym.once(function(){
                    this.init_dialog('notice');
                })
            });
            var tpl = '<div style="width:400px;padding:0 10px;text-align:left;font-size:12px;">';
            tpl += '<p style="text-indent:2em;">尊敬的用户，您好！</p>';
            tpl += '<p style="text-indent:2em;">金银猫全面改版，功能升级，服务更升级！由于银生宝相关模块还在升级中，目前暂不支持购买（您可使用最新的快捷支付）。</p>';
            tpl += '<p style="text-indent:2em;">为方便使用银生宝的老用户管理资金账户，我们在旧版保留了相关功能，您可返回管理查询。</p>';
            tpl += '<div style="text-align:center;"><img src="/static/images/user/all-notice.png" /></div>';
            tpl += '<p style="text-indent:2em;">感谢您的理解，为您带来的不便我们深表歉意，如有问题，请拨打客服电话<em style="color:#da5626">4008-556-333</em></p>';
            tpl += '</div>'
            notice.show(tpl, function(){
                jym.cookie('ysbNotice', '1', {expires:7, domain:'.jinyinmao.com.cn'});
            });
        });
    }
    //================================
    return jym;
}));
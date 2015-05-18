define(['jym'], function(jym){
    var jym1118 = {};
    var SetUserFun = function(d, selector){
        $(selector).show();
    };
    var SetPaymentFun = function(d, selector){
        var obj = $(selector);
        if(d.Status == 20){
            var s = '<div style="border-top:solid 1px #e5e5e5;padding-top:8px;">';
            s += '<label for="chk_jym1000">使用1000元理财金</label>';
            s += '&nbsp;<input type="checkbox" checked="checked" id="chk_jym1000" /></div>';
            obj.html(s).css('padding-top','80px');
            SetPaymentSy(true);
            $('#chk_jym1000').click(function(){
                SetPaymentSy(this.checked)
            });
        }
    };
    var SetPaymentSy = function(addSy){
        if(addSy){
            _productInfo.ActivityNo = 1000;
            $('#ctt-payment-sy').html('&nbsp;&nbsp;&nbsp;(理财金收益+<strong>'+_otherSy+'</strong>元)');
        } else {
            delete _productInfo.ActivityNo;
            $('#ctt-payment-sy').html('');
        }
    };
    var SetDetailFun = function(d, selector){
        var obj = $(selector).css({
            "position":"relative",
            "z-index":"99"
        });
        if(d.Status == 20 || d.Status == 30){
            var style = 'position:absolute;padding:1px 5px;background-color:#FFF;';
            style += 'border-radius:5em;margin-top:-3px;margin-left:30px;';
            if(d.Status == 20){ style += 'color:#db5629;border:1px solid #db5629;'; }
            if(d.Status == 30){ style += 'color:#CCC;border:1px solid #CCC;'; }
            var s = '<span style="'+style+'">可使用1000元理财金</span>';
            s += '<a href="#" class="qa-help" style="position:absolute;margin-left:160px;"><i></i>';
            s += '<div class="ui-tip-msg" style="width:260px;text-align:left;left:-217px;">';
            s += '<p>理财金仅限在1118发财季期间注册的新用户使用。<br>产品到期即可获得相对应的收益！';
            s += '<br>新用户注册时间：2014年11月12日-12月12日。</p>';
            s += '<b class="ui-tip-top" style="margin-left:74px;"></b>';
            s += '</div></a>';
            obj.append(s);
        }
    };
    jym1118.gift1000 = {
        __getData:function(callback, errCallback){
            var api = '/api/v1/Orders/GetActivityStatus1000';
            jym.sync({
                url:api,
                success:function(d){
                    if(d && d.Status < 40){
                        jym.fire(callback, d);
                    }
                },
                error:function(d){
                    jym.fire(errCallback, d);
                }
            });
        },
        setUserStatus:function(selector){
            this.__getData(function(d){
                SetUserFun(d, selector);
            });
        },
        setPaymentStatus:function(selector){
            this.__getData(function(d){
                SetPaymentFun(d, selector);
            });
        },
        setDetailStatus:function(selector){
            this.__getData(function(d){
                SetDetailFun(d, selector);
            }, function(d){
                d.Status = d.status == 401 ? 20 : 0;
                SetDetailFun(d, selector);
            });
        }
    };
    return jym1118;
});
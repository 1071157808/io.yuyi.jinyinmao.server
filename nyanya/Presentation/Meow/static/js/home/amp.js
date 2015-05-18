zim.ns('zim.page.jym1118', {
    __getData:function(callback, errCallback){
        var api = '/api/v1/Orders/GetActivityStatus1000';
        zim.sync({
            url:api,
            success:function(d){
                if(d && d.Status < 40){
                    zim.fire(callback, d);
                }
            },
            error:function(d){
                zim.fire(errCallback, d);
            }
        });
    },
    __status20:function(color){
        color = color || 'F60';
        var style = 'border-style:solid;border-width:1px 0;border-color:#efede8;';
        style += 'background-color:#FFF;margin-top:.3em;padding:0 1em;';
        var s = '<div style="'+style+'">';
        s += '<a class="ui-link" href="#" data-action="jym-topic-1118" style="color:#'+color+';">可使用1000元理财金</a></div>';
        $('#jym1118').html(s);
        zim.action.extend({
            "jym-topic-1118":function(){
                zim.ui.alert({
                    msg:'理财金仅限在1118发财季期间注册的新用户使用。产品到期即可获得对应的收益！新用户注册时间：2014年11月12日-12月12日。',
                    btnOk:'知道了'
                })
            }
        });
    },
    __status30:function(){
        zim.page.jym1118.__status20('CCC');
    },
    getStatus:function(){
        var that = this;
        this.__getData(function(d){
            zim.fire(that['__status'+d.Status]);
        }, function(d){
            d.Status = d.status == 401 ? 20 : 0;
            zim.fire(that['__status'+d.Status]);
        });
    }
});
zim.ns('zim.page.list', {
    init:function(type){
        this.type = type || 'amp';
        zim.page.resetScroll();
        this.products.render();
        this.products.bind();
    },
    getProductStatus:function(status){
        if(status >= 60){
            return "";
        } else {
            return "<b>" + zim.eHash.SellingStatus[status] + "</b>";
        }
    },
    products:{
        PageIndex:1,
        HasNextPage:true,
        getData:function(fn, PageIndex){
            var that = this;
            var url = zim.api[zim.page.list.type].list;
            zim.sync({
                url:url, 
                data:{number:PageIndex},
                success:function(d){
                    that.HasNextPage = d.HasNextPage;
                    if(d.HasNextPage){ that.PageIndex++; }
                    zim.fire(fn, d);
                }
            });
        },
        render:function(PageIndex){
            if(!this.HasNextPage) return false;
            PageIndex = PageIndex || this.PageIndex;
            this.getData(function(d){
                zim.tplRender('#jqm-list-products', '#jqm-tpl-products', {items:d.Products}, true);
            }, PageIndex);
        },
        bind:function(){
            var that = this;
            zim.pager.onScrollEnd(function(){
                that.render();
            });
        }
    }
});
zim.ns('zim.page.detail', {
    init:function(type){
        this.product.type = type || 'amp';
        this.product.render();
        this.product.bind();
    },
    getDefNumber:function(item){
        var max = item.UnitPrice == 1 ? 10000 : 10;
        return Math.min(item.AvailableShareCount, item.MaxShareCount, max);
    },
    canRefresh:true,
    countdown:function(){
        var that = this;
        this.canRefresh = false;
        zim.countdown({
            count:5,
            callback:function(s){
                that.canRefresh = !s;
            }
        });
    },
    cachePercent:0,
    refresh:function(){
        if(!this.canRefresh){ 
            zim.page.setPieSlider("jqm-round-sector", this.cachePercent);
            return false;
        }
        this.countdown();
        var that = this;
        var id = this.product.identifier
        zim.sync({
            url:zim.api.SaleProcess,
            data:{"productIdentifier": id},
            loading:false,
            success:function(d){
                that.AvailableCount = d.Available;
                $('#jqm-available').text(d.Available);
                var p = that.cachePercent = Math.floor(d.Paid * 100 / d.Sum);
                $('.jqm-round-circle').html('<strong>已售出</strong>'+p+'<samp>%</samp>');
                zim.page.setPieSlider("jqm-round-sector", p);
            }
        });
    }
});
zim.ns('zim.page.detail.product', {
    id:0,
    yield:0,
    duration:0,
    AvailableCount:0,
    identifier:0,
    bind:function(){
        zim.action.extend({
            "num-reload":function(){
                zim.page.detail.refresh();
            },
            "buy-product":function(){
                zim.page.detail.buy.submit();
            }
        });
    },
    getData:function(fn){
        var id = zim.urlQuery('id');
        if(!id){
            zim.ui.message('参数错误！', function(){
                zim.location('/');
            });
            return false;
        }
        this.id = id;
        var that = this;
        var url = zim.api[zim.page.detail.product.type].detail;
        zim.sync({
            url:url,
            data:{productNo:id},
            success:fn
        });
    },
    render:function(){
        var that = this;
        this.getData(function(d){
            that.yield = d.Yield;
            that.duration = d.Period;
            that.unitPrice = d.UnitPrice;
            that.identifier = d.ProductIdentifier;
            that.AvailableCount = d.AvailableShareCount;
            that.minCount = d.MinShareCount;
            that.maxCount = d.MaxShareCount;
            var name = zim.page.getProductName(d.ProductName, d.ProductNumber);
            $('.jqm-title').text(name);
            zim.tplRender('#jqm-product-detail', '#jqm-tpl-detail', {item:d});
            zim.tplRender('#jqm-product-info', '#jqm-tpl-info', {item:d});
            if(d.ShowingStatus == 30){
                zim.page.setPieSlider("jqm-round-sector", d.PaidPercent);
            } else if(d.ShowingStatus == 40) {
                zim.page.setPieOverDay("jqm-round-sector", d);
            } else if(d.ShowingStatus < 30){
                zim.page.detail.buy.countdown();
            }
            zim.page.detail.calc.showResult($('#jqm-sy-num').val());
            zim.page.detail.calc.bind();
            zim.page.jym1118.getStatus();
        });
    }
});
zim.ns('zim.page.detail.calc', {
    bind:function(){
        var that = this, obj = $('#jqm-sy-num');
        zim.fixIOS.fixedHeader('#jqm-sy-num');
        obj.on('input', function(){
            var count = parseFloat(this.value);
            if(isNaN(count)) { count = zim.page.detail.buy.safeCount(); }
            that.showResult(count);
        }).on('blur', function(){
            var count = parseInt(this.value) || zim.page.detail.product.minCount;
            count = zim.page.detail.buy.chkCount(count);
            if(count > 0){
                this.value = count;
                that.showResult(count);
            } else {
                this.value = "";
                that.showResult(0);
            }
        });
    },
    showResult:function(value){
        var num = parseFloat(value) || 0;
        var price = num * zim.page.detail.product.unitPrice;
        var yield = parseFloat(zim.page.detail.product.yield) || 0;
        var duration = parseFloat(zim.page.detail.product.duration) || 0;
        var sy = zim.sy_price(price, yield, duration);
        $('#jqm-sum-result').text(price);
        $('#jqm-sy-result').text(sy);
    }
});
zim.ns('zim.page.detail.buy', {
    countdown:function(){
        var btn = $('#btnBuy'), time = btn.attr('data-time');
        if(time){
            var startTime = zim.strToDate(_timespan);
            var endTime = zim.strToDate(time);
            if(startTime && endTime && endTime.getTime() > startTime.getTime()){
                var count = endTime.getTime() - startTime.getTime();
                count = Math.ceil(count / 1000);
                zim.countdown({
                    count:count,
                    callback:function(s){
                        if(s <= 0){
                            btn.removeClass('ui-btn1').addClass('ui-btn3').attr('data-action','buy-product').text('立即抢购');
                            var txt = '<strong>已售出</strong>0<samp>%</samp>';
                            $('.jqm-round-circle').html(txt);
                            $('#jqm-detail-date1').remove();
                            $('#jqm-detail-date2').show();
                        }else if(s < 100){
                            btn.text('待售('+s+'秒)')
                        } else {
                            btn.attr('data-s', s);
                        }
                    }
                });
            }
        }
    },
    chkCount:function(count){
        var _available = zim.page.detail.product.AvailableCount;
        var _minCount = zim.page.detail.product.minCount;
        var _maxCount = zim.page.detail.product.maxCount;
        if(_available > _minCount){
            var max = Math.min(_available, _maxCount);
            return count >= max ? max : count <= _minCount ? _minCount : count;
        } else {
            return _available;
        }
    },
    allCount:function(){
        var _available = zim.page.detail.product.AvailableCount;
        var _maxCount = zim.page.detail.product.maxCount;
        var count = _available;
        if(count > _maxCount){ count = _maxCount; }
        return count;
    },
    safeCount:function(){
        var count = zim.page.detail.product.AvailableCount;
        var _minCount = zim.page.detail.product.minCount;
        if(count > _minCount){ count = _minCount; }
        return count;
    },
    submit:function(){
        var that = this;
        zim.user.addCallback(function(isLogin){
            if(!isLogin){
                zim.ui.confirm('请先登录！', function(t){
                    if(t){ zim.location(zim.user.loginUrl()); }
                });
            } else {
                zim.user.validate(function(d){
                    if(!d.HasDefaultBankCard){
                        zim.ui.confirm('您还没有开通快捷支付功能！<br>是否立即开通？', function(t){
                            if(t){
                                if(!d.HasSetPaymentPassword){
                                    zim.location("/user/yilian/password");
                                } else {
                                    zim.location("/user/yilian/identity");
                                }
                            }
                        });
                        return false;
                    } else {
                        var count = $('#jqm-sy-num').val();
                        if(zim.trimAll(count) == ""){
                            zim.ui.alert("请输入投资份额。");
                            return false;
                        }
                        if(parseFloat(count) == 0){
                            zim.ui.alert("购买份额不能为零。");
                            return false;
                        }
                        var url = zim.params("/payment/create?id={ProductNo}&count={Count}&type={Type}", {
                            ProductNo:zim.page.detail.product.id,
                            Count:parseFloat(count),
                            Type:zim.page.detail.product.type
                        });
                        that.submit = zim.noop;
                        zim.location(url);
                    }
                });
            }
        });
    }
});
define(['jym', 'dialog'], function(jym, dialog){
    var detail = {
        init:function(){
            this.validCount();
            this.countdown.init();
            this.progress.init();
            this.lastCount.init();
            this.validate.init();
            this.payment.init();
            this.tabs.init();
        },
        validCount:function(){
            var obj = $('#paymentCount');
            var count = obj.val();
            if(count != "" && count != 0){
                count = detail.validate.chkCount(count);
                obj.val(count);
                detail.validate.calc(count);
            }
        }
    };
    detail.countdown = {
        init:function(){
            if(document.getElementById('saleOver')){
                this.bind();
            }
        },
        bind:function(){
            var btn = $('#saleOver');
            var time = btn.attr('data-time');
            var serverTime = jym.strToDate(_server_time);
            var endTime = jym.newDate(time);
            if(serverTime && endTime && endTime.getTime() > serverTime.getTime()){
                var count = endTime.getTime() - serverTime.getTime();
                count = Math.ceil(count / 1000);
                jym.countdown({
                    count:count,
                    callback:function(s){
                        if(s <= 0){
                            location.reload();
                        }else if(s <= 99){
                            btn.text('即将开售('+s+'秒)');
                        }else{
                            btn.attr('data-s', s);
                        }
                    }
                });
            }
        }
    };
    detail.progress = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var o = $('.amp-reduce-progress .percent em');
            var t = $('.amp-reduce-progress .text b');
            var p = o.attr('data-value');
            jym.loadProgress(parseInt(p), function(s){
                o.width(s+'%');
                t.text(s);
            });
        }
    };
    detail.lastCount = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "refreshShareCount":function(){
                    detail.lastCount.refresh();
                }
            });
        },
        refresh:function(){
            jym.sync({
                url:jym.api.amp.SaleProcess,
                data:{"productIdentifier":_identifier},
                success:function(d){
                    _available = d.Available;
                    _paid = d.Paid;
                    _sum = d.Sum;
                    detail.lastCount.render();
                }
            });
        },
        render:function(){
            $('.amp-reduce-part b').text(_available);
            $('#paymentCount').trigger('blur');
            var progress = $('.amp-reduce-progress');
            var cent = parseInt((_paid/_sum) * 100);
            var bar = progress.find('em');
            var num = progress.find('b');
            bar.data('value', cent);
            jym.loadProgress(cent, function(s){
                bar.width(s+'%');
                num.text(s);
            });
        }
    };
    detail.validate = {
        init:function(){
            this.bind();
        },
        bind:function(){
            var btnAll = $('#pay-all').click(function(){
                if(btnAll.prop('checked')){
                    var count = detail.validate.allCount();
                    input.val(count);
                    jym.placeholder.trigger('#paymentCount');
                    detail.validate.calc(count);
                }
            });
            var input = $('#paymentCount').on('input propertychange', function(){
                var count = parseInt(this.value);
                if(isNaN(count)){ count = detail.validate.safeCount(); }
                detail.validate.calc(count);
            }).on('blur', function(){
                var count = parseInt(this.value) || _minCount;
                count = detail.validate.chkCount(count);
                if(count > 0){
                    this.value = count;
                    detail.validate.calc(count);
                } else {
                    this.value = "";
                    detail.validate.calc(0);
                }
            });
        },
        chkCount:function(count){
            if(_available > _minCount){
                var max = Math.min(_available, _maxCount);
                return count >= max ? max : count <= _minCount ? _minCount : count;
            } else {
                return _available;
            }
        },
        allCount:function(){
            var count = _available;
            if(count > _maxCount){ count = _maxCount; }
            return count;
        },
        safeCount:function(){
            var count = _available;
            if(count > _minCount){ count = _minCount; }
            return count;
        },
        calc:function(count){
            var p_pay = _detail.unit * count;
            var p_inp = jym.sy_price(p_pay, _detail.yield, _detail.period);
            $('#p-pay').text(p_pay.toFixed(2));
            $('#p-inp').text(p_inp.toFixed(2));
        }
    };
    detail.payment = {
        init:function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "GotoPayment":function(){
                    jym.load.loginStatus(function(d){
                        if(d.Valid){
                            detail.payment.chkPayment(function(){
                                detail.payment.submit();
                            });
                        } else {
                            dialog.confirm.show('请先登录！', function(t){
                                if(t){ jym.login_backUrl(); }
                            });
                        }
                    });
                }
            });
        },
        chkPayment:function(callback){
            jym.sync({
                url:jym.api.user.info,
                success:function(d){
                    if(d.HasDefaultBankCard){
                        jym.fire(callback);
                    } else {
                        dialog.confirm.show('您还没有开通快捷支付功能！<br>是否立即开通？', function(t){
                            if(t){ location.href = "/bankcard/setPassword"; }
                        });
                    }
                }
            });
        },
        submit:function(){
            var input = $('#paymentCount'), count = input.val();
            if(jym.trimAll(count) == ""){
                dialog.alert.show('请输入投资份额。');
                return false;
            }
            if(parseInt(count) == 0){
                dialog.alert.show('购买份额不能为零。');
                return false;
            }
            jym.formPost('/payment/create', {
                ProductNo:_productNo,
                ProductType:'cpl',
                Count:count
            });
        }
    };
    detail.tabs = {
        init:function(){
            jym.action.extend({
                "change-tabs":function(){
                    detail.tabs.change(this);
                }
            });
        },
        change:function(tab){
            $('.amp-tab-list li.on').removeClass('on');
            $('.amp-tab-items:visible').hide();
            var tab_id = tab.getAttribute('data-tab');
            if(!document.getElementById(tab_id)){
                var txt = $('#tpl-'+tab_id).val();
                $('.amp-tab-content').append(txt);
            } else {
                $('#'+tab_id).show();
            }
            tab.parentNode.className = "on";
        }
    };
    detail.init();
    return detail;
});
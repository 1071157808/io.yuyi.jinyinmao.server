define(['jquery', 'jym'], function($, jym){
    (function($){
        $.fn.fullBanner = function(options){
            var that = this;
            return this.each(function(){
                var defaults = {
                        prev      : '.prev',
                        next      : '.next',
                        links     : '.items',
                        speed     : 5000, // 速度
                        isShowBtn : true, // 是否显示上下按钮
                        isAutoDot : true,
                        nobg      : true,
                        bar       : null
                    },
                    config       = $.extend(defaults, options),
                    links        = that.find(config.links),
                    bar          = that.find(config.bar),
                    prev         = that.find(config.prev),
                    next         = that.find(config.next),
                    loading      = that.find('.img-loading'),
                    isShowBtn    = config.isShowBtn,
                    isAutoDot    = config.isAutoDot,
                    len          = links.length,
                    on           = 'dq',
                    no           = 'no',
                    timer        = null,
                    resizeTimer  = null,
                    currentIndex = 0;

                links.each(function(){
                    var self = $(this),
                        img = self.find('img');
                    if(config.nobg){
                        self.css({"background-image" : "url(" + img.attr("src") + ")", 'background-color' : links.eq(0).attr('name')});
                        img.hide();
                    }
                    bar.append("<div/>");
                });
                isAutoDot && barCenter();
                var bar_item = bar.find('div');
                loading.hide();

                $(window).resize(function(){
                    if(!isAutoDot) { return; }
                    if(resizeTimer){ clearTimeout(resizeTimer); }
                    resizeTimer = setTimeout(function(){
                        barCenter();
                    }, 200);
                });

                function change(i){
                    currentIndex = i;
                    for(var j = 0; j < len; j++){
                        if(j == i){
                            links.eq(j).animate({opacity: 1, zIndex : 1}, 'slow');
                            bar_item.eq(j).removeClass().addClass(on);
                            links.eq(j).css('background-color', links.eq(j).attr("name"));
                        } else {
                            links.eq(j).animate({opacity: 0, zIndex : 0}, 'slow');
                            bar_item.eq(j).removeClass().addClass(no);
                        }
                    }
                }

                function startAm() {
                    timer = setInterval(timer_tick, config.speed);
                }
                function stopAm() {
                    clearInterval(timer);
                }
                function timer_tick() {
                    currentIndex = currentIndex >= (len - 1) ? 0 : currentIndex + 1;
                    change(currentIndex);
                }
                function barCenter(){
                    bar.css({ left : (that.width() - 25 * len) / 2});
                }
                that.on('click', config.bar + ' div', function(){
                    change($(this).index());
                });
                next.click(function(){
                    currentIndex = currentIndex >= (len - 1) ? 0 : currentIndex + 1;
                    change(currentIndex);
                });
                prev.click(function(){
                    currentIndex = currentIndex <= 0 ? (len - 1) : currentIndex - 1;
                    change(currentIndex);
                });
                that.mouseenter(function(){
                    stopAm();
                    if(isShowBtn){
                        prev.fadeIn();
                        next.fadeIn();
                    }
                }).mouseleave(function(){
                    startAm();
                    if(isShowBtn){
                        prev.fadeOut();
                        next.fadeOut();
                    }
                });
                startAm();
            });
        }
    }($));

});
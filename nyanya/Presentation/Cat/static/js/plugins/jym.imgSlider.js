define(['jquery', 'jym'], function($, jym){
    (function($){
        $.fn.imgSlider = function(options){
            var defaults = {
                    auto : true,
                    num  : 3,
                    speed: 4 // speed
                },
                that     = $(this),
                size     = that.find('li').length,
                timer    = 0,
                opts     = $.extend({}, defaults, options),
                n        = opts.num;

            if(size > n){
                var page = size + 1, // pages
                    li_w = that.find('li').outerWidth(true),
                    ul_w = li_w * size,
                    ul   = that.find('ul');
                ul.append(ul.html()).css({'width': 2 * ul_w, 'marginLeft': - ul_w}); // in order to move forward with pictures

                that.find('.btnRight').bind('click', function(){
                    if(!ul.is(':animated')){
                        if(page < 2 * size - n){ //
                            ul.animate({'marginLeft': '-=' + li_w}, 'slow', 'linear');
                            page++;
                        } else {
                            ul.animate({'marginLeft': '-=' + li_w}, 'slow', 'linear', function(){
                                ul.css('marginLeft', (n - size) * li_w);
                                page = (size - n) + 1;
                            });
                        }
                    }
                });
                that.find('.btnLeft').bind('click', function(){
                    if(!ul.is(':animated')){
                        if(page > 2){
                            ul.animate({'marginLeft': '+=' + li_w}, 'slow', 'linear');
                            page--;
                        } else {
                            ul.animate({'marginLeft': '+=' + li_w}, 'slow', 'linear', function(){
                                ul.css('marginLeft', - ul_w);
                                page = size + 1;
                            });
                        }
                    }
                });
                that.hover(function(){
                    clearInterval(timer);
                }, function(){
                    if(opts.auto){
                        timer = setInterval(function(){
                            that.find('.btnRight').click();
                        }, opts.speed * 1000);
                    }
                }).trigger('mouseleave');
            }
        };
    }($));

});
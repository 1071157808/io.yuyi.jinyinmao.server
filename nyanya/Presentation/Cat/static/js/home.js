define(['jym', 'fullbanner', 'imgSlider'], function(jym){
    var HOST_CMS = 'https://cmsadmin.jinyinmao.com.cn/';
    var HOST_NEWS = 'https://source.jinyinmao.com.cn/';
    var poster = HOST_CMS+'caches/poster_js/';
    var getPoster = function(id){
        return poster + id + '.js?v=' + ~(-new Date()/6e6);
    };
    function renderBanner(callback){
        require([getPoster(13)],function(d){
            var flashBg = $('#flashBg .flash'),
                flashSub = '#flash_sub';

            if(flashBg.find('.items').length > 0){
                flashBg.find('a').remove('.items');
            }
            if(typeof d !== 'undefined'){
                appendBanner(flashBg, d.ADImage);
            } else {
                renderFlash(flashBg, eval('(' + cmsAD_16.ADContent + ')'));
            }
            flashBg.fullBanner({
                bar : flashSub
            });
        });
        require([getPoster(15)],function(d){
            var flash = $('#flash_mid_sub'),
                sub = '#flash_sub_2';

            if(typeof d !== 'undefined'){
                appendBanner(flash, d.ADImage);
            } else {
                renderFlash(flash, eval('(' + cmsAD_22.ADContent + ')'));
            }
            flash.fullBanner({
                bar : sub
            });
        });
    }
    function renderFlash(flash, data){
        if(data.ADImage.length > 0){
            appendBanner(flash, data.ADImage);
        }
    }
    function renderNews(){
        require([HOST_NEWS+'api/meit.php'],function(data){
            if(typeof meitinfo !== 'undefined'){
                appendHtml('#homeMsg', meitinfo, '资讯');
            }
        });
        require([HOST_NEWS+'api/miao.php'],function(data){
            if(typeof miaoinfo !== 'undefined'){
                appendHtml('#homeState', miaoinfo);
            }
        });
    }

    function appendBanner(obj, data){
        var frag = '', first = '';
        $.each(data, function(i){
            if(this.ImgPath.length > 0){
                first = i === 0 ? 'first' : '';
                frag += '<a name="#'+ this.imgID +'" class="items ' + first + '" target="_blank" href="' + decodeURIComponent(this.imgADLinkUrl) + '"><img alt="'+this.imgADAlt+'" src="' + this.ImgPath.replace(/^http:/, 'https:') + '"></a>';
            }
        });
        return $(obj).prepend(frag);
    }
    function appendHtml(obj, data, type){
        var frag = ''; type = type || '动态';
        $.each(data, function(){
            frag += '<li class="ellipsis"><a target="_blank" title="' + this.title + '" href="' + this.url + '">' + this.title + '</a></li>';
        });
        return $(frag).appendTo(obj);
    }
    function payProgress(){
        var o = $('.percent em', this), t = $('.text b', this);
        var p = o.attr('data-value');
        if(p){
            jym.loadProgress(parseInt(p), function(s){
                o.width(s+'%');
                t.text(s);
            });
        }
    }
    function renderSlide(){
        var banner = $('.slideBanner');
        var arr = [getPoster(22), getPoster(23), getPoster(24)];
        require(arr, function(amp,cpl,bank){
            appendBanner($('#bankBanner'), bank.ADImage);
            appendBanner($('#ampBanner'), amp.ADImage);
            appendBanner($('#cplBanner'), cpl.ADImage);
            banner.each(function(i){
                var self = $(this);
                self.fullBanner({
                    bar : '.slide-mid-bar',
                    isAutoDot : 0,
                    nobg : 0
                });
            });
        });


    }
    function homeBind(){
        renderNews();
        renderBanner();
        renderSlide();

        // amp轮换
        $('#homeAmpSlide').imgSlider({auto : false});
        //require(['http://tjs.sjs.sinajs.cn/open/api/js/wb.js']);

        $('.bank-process').each(payProgress);
    }
    $(homeBind);
});
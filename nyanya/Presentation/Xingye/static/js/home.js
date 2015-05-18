define(['jym'], function(){
    var home = {
        init : function(){
            this.changeSort();
            this.sns();
        },
        changeSort : function(){
            var url = location.href;
            var getSortParams = function(name, value){
                if(url.indexOf("?") > -1){
                    url = url.split('?')[0];
                }
                url += "?" + name + "=" + value;
                location.href = url;
            };
            var $yield = $('#xy-sort-yield');
            var $period = $('#xy-sort-period');
            $yield.click(function(){
                var y = $yield.attr('data-yield') == 1 ? 2 : 1;
                getSortParams("yield", y);
            });
            $period.click(function(){
                var p = $period.attr('data-period') == 1 ? 2 : 1;
                getSortParams("period", p);
            });
        },
        sns : function(){
            var title = encodeURIComponent(document.title),
                url = encodeURIComponent(location.href),
                txt = '<a href="http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url=' + url + '&desc=' + title + '" title="分享到QQ空间" class="qzon" target="_blank"></a>' +
                '<a href="http://v.t.sina.com.cn/share/share.php?url=' + url + '&title=' + title + '" title="分享到新浪微博" class="sina" target="_blank"></a>' +
                '<a href="http://share.v.t.qq.com/index.php?c=share&a=index&url=' + url + '&title=' + title + '" title="分享到腾讯微博" class="tx-wb" target="_blank"></a>' +
                '<a href="#" class="tx-win" style="display: none;" target="_blank"></a>';

            $('#ticketShare').html(txt);
        }
    }

    home.init();
    return home;
});
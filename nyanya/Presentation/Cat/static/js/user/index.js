define(['jym'], function (jym) {
    var weibo = function (opt) {
        var url = "http://service.weibo.com/share/share.php?title={title}&url={url}&pic={pic}&ralateUid=3822187413&type=link&language=zh_cn&searchPic=false&style=simple";
        return jym.params(url, opt);
    };
    var index = {
        init: function () {
            jym.action.extend({
                "sns-share-weibo": function () {
                    var url = weibo({
                        title: index.getTitle(),
                        url: 'https://www.jinyinmao.com.cn/',
                        pic: 'https://www.jinyinmao.com.cn/static/images/user/logo.png'
                    });
                    window.open(url);
                }
            });
        },
        getTitle: function () {
            var t = '我在金银猫的投资每小时能为我带来' + incomeMinute + '元的收益。。。';
            if (incomeMinute < 0.06) {
                t += '属于理财实习生，我还要加油啊！';
            } else if (incomeMinute > 0.6) {
                t += '是招财猫级的理财人啦！';
            } else {
                t += '已经是理财熟练工啦！';
            }
            t += '大金融、心服务，聪明理财就在金银猫 https://www.jinyinmao.com.cn';
            return t;
        }
    };
    index.init();
    return index;
});
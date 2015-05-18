define(['jym'], function(jym){
    var payProgress = function(){
        var o = $('.percent em', this), t = $('.text b', this);
        var p = o.attr('data-value');
        if(p){
            jym.loadProgress(parseInt(p), function(s){
                o.width(s+'%');
                t.text(s);
            });
        }
    };
    var fmtDateTime = function(s){
        var o = jym.intTObject(s);
        return jym.params('{d}日{h}时{m}分{s}秒', o, true);
    };
    var time_start = jym.strToDate(_server_time);
    var timeProgress = function(){
        var o = $(this);
        var time_end = jym.strToDate(o.attr('data-time'));
        var time_delay = time_end.getTime() - time_start.getTime();
        var time_func = function(){
            var s = fmtDateTime(time_delay);
            o.html(s);
            time_delay = time_delay - 1000;
            if(time_delay < 1){
                jym.timeClock.remove(time_func);
                status_payment(o);
            }
        };
        if(time_delay > 0){
            jym.timeClock.add(time_func);
        }
    };
    var status_payment = function(o){
        var li = o.parents('li');
        li.find('i.amp-02').removeClass('amp-02').addClass('amp-01');
        li.find('.pro-start-wrap').hide();
        li.find('.pro-finish-wrap').show();
    };
    var list = {};
    list.progress = {
        init:function(){
            $('.total-num').each(payProgress);
        }
    };
    list.timeDelay = {
        init:function(){
            $(".timeProgress").each(timeProgress);
        }
    };
    list.progress.init();
    list.timeDelay.init();
    return list;
});
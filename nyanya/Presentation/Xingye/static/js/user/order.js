define(['jym', 'WdatePicker'], function(jym){
    var T = {
        init : function(){
            this.bind();
        },
        bind:function(){
            jym.action.extend({
                "sort-datetime":function(){
                    var start = this.getAttribute('data-value');
                    var timeType = this.getAttribute('data-type');
                    $('.first-date').val(start);
                    $('.end-date').val(_today);
                    T.search(timeType);
                },
                "sort-datatime-before-year":function(){
                    $('.first-date').val("2008-01-01");
                    $('.end-date').val(_year);
                    T.search('by');
                },
                "sort-type":function(){
                    $('.sort-type .sel').removeClass('sel');
                    $(this).addClass('sel');
                    T.search();
                },
                "sort-status":function(){
                    $('.sort-status .sel').removeClass('sel');
                    $(this).addClass('sel');
                    T.search();
                }
            });
            $('.time').find('.first-date, .end-date').click(function(){
                WdatePicker({dateFmt:'yyyy-MM-dd', onpicked:function(){
                    T.search();
                }});
            });
        },
        search:function(timeType){
            var startDate = $('.first-date').val();
            var endDate = $('.end-date').val();
            var sortType = $('.sort-type .sel').attr('data-value');
            var sortStatus = $('.sort-status .sel').attr('data-value');
            var url = '/user/order/?start={start}&end={end}&type={type}&status={status}&time={timeType}';
            location.href = jym.params(url, {
                start:startDate,
                end:endDate,
                type:sortType,
                status:sortStatus,
                timeType:timeType
            });
        }
    };
    T.init();
    return T;
});
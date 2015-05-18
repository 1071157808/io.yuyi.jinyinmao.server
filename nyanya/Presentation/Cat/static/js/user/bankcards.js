define(['jym'], function (jym) {
    var index = {
        init: function () {
            this.fix6();
        },
        fix6 : function(){
            $('.payment-limit').hover(function(){
                $(this).parent().addClass('ctt-fixed-z');
            }, function(){
                $(this).parent().removeClass('ctt-fixed-z');
            });
        }
    };
    index.init();
    return index;
});
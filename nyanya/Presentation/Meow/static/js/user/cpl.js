zim.ns('zim.page.user.cpl.list', {
    init:function(){
        this.PageIndex = 1;
        this.HasNextPage = true;
        this.render();
        this.bind();
    },
    bind:function(){
        var that = this;
        zim.pager.onScrollEnd(function(){
            that.render();
        });
    },
    getData:function(fn, PageIndex){
        var that = this;
        zim.sync({
            url:zim.api.order.cpl_list, 
            data:{pageIndex:PageIndex, sortMode:1},
            success:function(d){
                that.HasNextPage = d.HasNextPage;
                if(d.HasNextPage){ that.PageIndex++; }
                zim.fire(fn, d);
            },
            error:zim.user.callback_error
        });
    },
    render:function(PageIndex){
        if(!this.HasNextPage) return false;
        PageIndex = PageIndex || this.PageIndex;
        this.getData(function(d){
            zim.tplRender('#jqm-user-order', '#jqm-tpl-orders', d, true);
        }, PageIndex);
    }
});
zim.ns('zim.page.user.cpl.detail', {
    init:function(){
        this.bind();
        this.render();
    },
    bind:function(){},
    getData:function(fn){
        var that = this;
        var id = zim.urlQuery('id');
        zim.sync({
            url:zim.api.order.detail, 
            data:{orderIdentifier:id},
            success:function(d){
                zim.fire(fn, d);
            },
            error:zim.user.callback_error
        });
    },
    render:function(){
        this.getData(function(d){
            zim.tplRender('#jqm-product-detail','#jqm-tpl-detail', {item:d})
        });
    }
});
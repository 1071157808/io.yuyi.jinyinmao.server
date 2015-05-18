/*
 *  1.安装Node.js环境
 *  2.安装fis命令：npm install -g fis
 *  3.在项目根目录下配置fis-conf.js
 *  4.编译产出命令：fis release -Dmod ./output
 */
fis.config.merge({
    project : {
        charset : 'gbk',
        md5Connector : '-',
        md5Length : 7
    },
    settings : {
        optimizer : {
            //fis-optimizer-uglify-js插件的配置数据
            'uglify-js' : {
                output : {
                    ascii_only : true
                }
            },
            //fis-optimizer-clean-css插件的配置数据
            'clean-css' : {
                keepBreaks : true
            }
        }
    },
    roadmap : {
        path : [
            {
                reg : '/static/js/**.min.js',
                release : '/$&',
                useHash : false,
                useCompile : false
            },{
                reg : '/static/js/code/**.js',
                release : '/$&',
                useHash : false,
                useCompile : false
            },{
                reg : '/static/js/**.js',
                release : '/$&'
            },{
                reg : '/static/css/**.css',
                release : '/$&'
            },{
                reg : /^\/static\/images\/(.*\.(?:png|gif|jpg))/i,
                release : '/static/images/$1'
            },{
                reg : /.*/,
                release : '/$&'
            }
        ]
    }
});
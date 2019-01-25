var MixinBase = {
    props: ['id'],
    computed: {
        elementId: function () {
            // if (typeof this.id === "undefined") this.id = this.vid;
            if (typeof this.id === "undefined")
                return GetId();
            else
                return this.id;
        },
        preLoaderTarget: function () {
            var $me = $("#" + this.elementId);
            if ($me.closest(".card").length > 0) return $me.closest(".card");
            return $me;
        }
    },
    methods: {
        execute: function (inject) {
            // console.log("execute", inject);
            var me = this;
            if (typeof inject === "function") inject(me);
        },
        loadingShow: function () {
            this.preLoaderTarget.addClass("preLoader");
        },
        loadingHide: function () {
            this.preLoaderTarget.removeClass("preLoader");
        },
        loadingError: function () {
            this.preLoaderTarget.removeClass("preLoader");
            this.preLoaderTarget.addClass("preLoaderError");
        },
        downloadFile: function (base64,type,fileName){
            if (base64.match("base64,") !== null) base64 = base64.split('base64,')[1];
            var blob = b64toBlob(base64, type);
            var newBlob = new Blob([blob], { type: type });

            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                window.navigator.msSaveOrOpenBlob(newBlob);
                return;
            } 

            const data = window.URL.createObjectURL(newBlob);
            var link = document.createElement('a');
            link.href = data;
            link.download= fileName;
            link.click();

            setTimeout(function () {
                window.URL.revokeObjectURL(data);
            }, 100);
        }
    },
    filters: {
        Display: function (value, source) {
            console.log("filters Display", value);
            console.log("filters Display", source);
            if (typeof bus.$data.A === "undefined") return "DISPLAY=>" + value;

            return bus.$data.A;
        }
    }
};


function v_format_directive(e1, binding, vnode) {
    if (binding.value.format === null) return;

    if (typeof binding.value.format === "string") {
        try {
            binding.value.format = JSON.parse(binding.value.format);
        } catch { }
    }
    //console.log("FORMAT", binding.value.format);
    if (binding.value.format.fct === 'Display') {
        var display = "";
        var list = bus.getList(binding.value.format.source);
        // console.log("FORMAT 2 ",e1, "list", list);
        for (var i in list) {
            var e = list[i];
            //console.log("FORMAT e.Value", e.Value);
            // console.log("FORMAT ", e.Value, " === ", binding.value.value, e.Value == binding.value.value);
            if (e.Value == binding.value.value) {
                display = e.Display;
                break;
            }
        }
        //console.log("FORMAT display", display);

        $(e1).html(display === "" ? binding.value.value : display);
    }
}
Vue.directive("format", {
    bind: v_format_directive,
    update: v_format_directive
});

Vue.directive("include", {
    bind(e1, binding, vnode) {
        // console.log("include", binding);
        var url = binding.value.url;
        var $element = $(e1);
        var $preloaderElement = $element.parent(".card").length > 0 ? $element.parent(".card") : $element;
        $preloaderElement.addClass("preLoader");
        $element.load(url, function (response, status, xhr) {
            $preloaderElement.removeClass("preLoader");
            if (status === "success") {

                updateDom();
            } else {
                $preloaderElement.addClass("preLoaderError");
            }
        });
    }
});

var bus = new Vue({
    data: {
        lists: {},
        listsConfig: {},
        listPil: {},
        scope: {},
    },
    methods: {
        init: function () {
            this.lists = {};
            this.listsConfig = {};
            this.listPil = {};
            this.scope = {};
        },
        getKeyFromSource: function (source) {
            if (typeof source === "string") {
                try {
                    source = JSON.parse(source);
                } catch (e) {
                    return source;
                }
            }
            return source.source + "|" + source.value + "|" + source.display + "|" + source.filter;
        },
        getList: function (source) {
            key = this.getKeyFromSource(source);

            // LOAD LIST IF NOT LOADED
            if (typeof this.lists[key] === "undefined") {
                this.loadList(source, source, (a) => {  }, false);
            }
            return this.lists[key];
        },
        loadList: function (key, datasource, done, async) {
            async = async === "undefined" ? true : async;

            var me = this;
            if (typeof datasource !== "undefined" && datasource !== null && datasource !== "") {
                var jsonSource = null;
                if (typeof datasource === "object")
                    jsonSource = clone(datasource);
                else {
                    try {
                        jsonSource = JSON.parse(datasource);
                    } catch{
                        if (typeof datasource === "string")
                            jsonSource = { url: datasource, method: "GET" };
                    }
                }

                //Fill from bus lists by key
                if (jsonSource !== null && typeof jsonSource.key !== "undefined") {
                    done(this.lists[jsonSource.key]);
                    return;
                }

                // Manual filling
                if (jsonSource !== null && typeof jsonSource.source === "undefined" & typeof jsonSource.url === "undefined") {
                    done(jsonSource);
                    if (typeof key === "undefined") key = GetId();
                    this.lists[key] = jsonSource;
                    return;
                }

                var url = "metabo/SelectSource";
                var method = "POST";
                if (typeof jsonSource.url !== "undefined") url = jsonSource.url;
                if (typeof jsonSource.method !== "undefined") method = jsonSource.method;

                // SET KEY
                if (typeof jsonSource.source !== "undefined")
                    key = this.getKeyFromSource(jsonSource);
                if (typeof key === "undefined")
                    key = url;

                // IF LIST IS ALREADY LOADED or being loaded
                if (typeof this.lists[key] !== "undefined") {
                    if (this.lists[key] === "loading") {
                        if (typeof this.listPil[key] === "undefined") this.listPil[key] = [];
                        this.listPil[key].push(done);
                    }
                    else {
                        done(this.lists[key]);
                    }
                    return;
                }

                this.lists[key] = "loading";
                var data = EV.getComponent("data");
                data.ExecuteSource({
                    url: url,
                    method: method,
                    data: datasource,
                    async: async,
                    loadComplete: function (obj, response) {
                        me.lists[key] = response;
                        me.listsConfig[key] = { key: key, datasource: datasource, done: done };
                        done(response);

                        // list pil
                        if (typeof me.listPil[key] !== "undefined") {
                            while (me.listPil[key].length > 0) {
                                me.listPil[key].pop()(me.lists[key]);
                            }
                        }
                    },
                    fail: function () {
                        delete me.lists[key];
                        me.listPil = [];
                    }
                });
            }
        },
        reLoadList: function (key) {
            if (typeof this.listsConfig[key] === "undefined") return;
            delete this.lists[key];
            this.loadList(this.listsConfig[key].key, this.listsConfig[key].datasource, this.listsConfig[key].done);
        },
        setList: function (key, data) {
            console.log("setList", key, data);
            this.lists[key] = data;
        },
        setMeta: function (id, value) {
            // console.log("SET DATA " + id, value);
            this.$data[id] = value;

            // GET not yet loaded LISTS
            if (typeof this.$data[id].META_FIELD !== "undefined") {
                typeof this.$data[id].META_FIELD.forEach((e) => {
                    // console.log("call from mixing");
                    this.loadList(e.DB_NAME, e.FORM_SOURCE, () => { });
                });
            }
        },
        setScope: function (id, value) {
            this.$set(this.scope, id, value);
            //this.$data[id] = value;
        }
    }
});

var SideBarVue;
function SideBarVueInit(menu) {
    SideBarVue = new Vue({
        el: '#sidebar',
        data: {
            menu: menu
        },
        methods: {
            closeAll: function () {
                for (var i in this.menu) {
                    this.menu[i].open = false;
                }
            }
        }
    });
}
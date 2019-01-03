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

Vue.directive("format", {
    bind(e1, binding, vnode) {
        if (binding.value.format === null) return;

        if (binding.value.format.fct === 'Display') {
            var display = "";
            for (var i in bus.lists[binding.value.format.source]) {
                var e = bus.lists[binding.value.format.source][i];
                // console.log("e.Value", e.Value);
                if (e.Value === binding.value.value) {
                    display = e.Display;
                    break;
                }
            }

            $(e1).html(display === "" ? binding.value.value : display);
        }

    }
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
        loadList: function (key, datasource, done) {
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
                            jsonSource = {url:datasource, method: "GET"};
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
                    key = JSON.stringify(jsonSource);
                if (typeof key === "undefined")
                    key = url;

                // IF LIST IS ALREADY LOADED or being loaded
                if (typeof this.lists[key] !== "undefined") {
                    if (this.lists[key] === "loading") {
                        if(typeof this.listPil[key] === "undefined") this.listPil[key] = [];
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
                    loadComplete: function (obj, response) {
                        me.lists[key] = response;
                        me.listsConfig[key] = {key: key, datasource: datasource, done: done};
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
        setList: function (key, data){
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
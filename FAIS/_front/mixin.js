var MixinBase = {
    props: ['id'],
    computed: {
        elementId: function () {
            // if (typeof this.id === "undefined") this.id = this.vid;
            if (typeof this.id === "undefined")
                return GetId();
            else
                return this.id;
        }
    },
    methods: {
        execute: function (inject) {
            var me = this;
            if (typeof inject === "function") inject(me);
        }
    }
};

var bus = new Vue({});
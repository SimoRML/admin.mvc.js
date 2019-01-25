var PagerMixin = {
    props: {
        paging: {
            type: Boolean,
            default: true
        },
    },
    data: function () {
        return {
            pager: {
                size: 10,
                current: 0,
                count: 1,
                pages: [0],
                sliceFrom: 0,
                sliceTo: 0,
            }
        };
    },
    computed: {
        pagerCurrent: function () { 
            return this.pager.current;
        },
        pagerSize: function () { 
            return this.pager.size;
        }
    },
    watch: {
        pagerCurrent() { 
            this.setSlices();
        },
        pagerSize() { 
            this.setPages();
        },
    },
    methods: {
        initPager: function (list) {
            this.pager.countEntries = list.length;
            this.setPages();
        },
        setSlices: function () {            
            this.pager.sliceFrom = this.pager.current * this.pager.size;
            this.pager.sliceTo = (this.pager.current+1) * this.pager.size;
        },
        setPages: function () { 
            this.pager.count = this.pager.countEntries / this.pager.size;
            if (this.pager.count > parseInt(this.pager.count)) this.pager.count = parseInt(this.pager.count) + 1;
            this.pager.pages = [];
            for (var i = 0; i < this.pager.count; i++) {
                this.pager.pages.push(i);
            }
            this.setSlices();
        }
    }
};
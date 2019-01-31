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
            this.setPages();
        },
        pagerSize() {
            this.resetPages();
        },
    },
    methods: {
        initPager: function (list) {
            this.pager.countEntries = list.length;
            this.resetPages();
        },
        setSlices: function () {
            this.pager.sliceFrom = this.pager.current * this.pager.size;
            this.pager.sliceTo = (this.pager.current + 1) * this.pager.size;
        },
        resetPages: function () {
            this.pager.count = this.pager.countEntries / this.pager.size;
            if (this.pager.count > parseInt(this.pager.count)) this.pager.count = parseInt(this.pager.count) + 1;
            if (this.pager.current > this.pager.count) this.pager.current = this.pager.count-1;
            this.setSlices();
            this.setPages();
        },
        setPages: function () {
            this.pager.pages = [];
            var start = this.pager.current - 2;
            var end = this.pager.current + 3;
            if (start < 0) {
                start = 0;
                end = 5;
            }
            if (end > this.pager.count) {
                end = this.pager.count;
                if (end - 5 > 0) start = end - 5;
                else start = 0;
            }
            for (var i = start; i < end; i++) {
                this.pager.pages.push(i);
            }
        }
    }
};

/* PAGE SIZE
 * 
<div class="row">
    <div class="col-sm-6">
        <div class="dataTables_length">
            Affiche 
            <select name="datatables_length" v-model="pager.size">
                <option value="10">10</option>
                <option value="25">25</option>
                <option value="50">50</option>
            </select> entrées
        </div>
    </div>
    <div class="col-sm-6">
    </div>
</div>

*/
/* PAGINATION
 
 <div class="row">
    <div class="col-md-5" style="padding-top: 25px;">
        <div class="dataTables_info">
            Affiche {{ (pager.current*pager.size)+1 }}
            à
            <template v-if="pager.current+1 == pager.count">
                {{ pager.countEntries }}
            </template>
            <template v-else>
                {{ ((pager.current+1)*pager.size) }}
            </template>
            de {{ pager.countEntries }} entrées
        </div>
    </div>
    <div class="col-md-7">
        <div class="paging_full_numbers pull-right">
            <ul class="pagination">
                <li v-on:click="pager.current = 0"
                    :class="'paginate_button ' + (pager.current==0 ?  'disabled':'')">
                    <a href="javascript:;" tabindex="0">Début</a>
                </li>
                <li v-on:click="pager.current += pager.current>0 ? -1 : 0"
                    :class="'paginate_button ' + (pager.current==0 ?  'disabled':'')">
                    <a href="javascript:;" tabindex="0">Précédent</a>
                </li>
                <template>
                    <li v-for="index in pager.pages"
                        v-on:click="pager.current=index"
                        :class="'paginate_button ' + (pager.current==index ? 'active' : '')">
                        <a href="javascript:;" :tabindex="index">{{ index+1 }}</a>
                    </li>
                </template>
                <li v-on:click="pager.current += pager.current<(pager.count-1) ? 1 : 0"
                    :class="'paginate_button ' + (pager.current+1 == pager.count ?  'disabled':'')">
                    <a href="javascript:;" tabindex="0">Suivant</a>
                </li>
                <li v-on:click="pager.current = pager.count-1"
                    :class="'paginate_button ' + (pager.current+1 == pager.count ?  'disabled':'')">
                    <a href="javascript:;" tabindex="0">Dernier</a>
                </li>
            </ul>
        </div>
    </div>
</div>
 
 */
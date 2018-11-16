const store = new Vuex.Store({
    state: {
        //from: null
    },
    getters: {
        // test: state => state.test,
        // form: state => state.form.a,
        // form: state => x => typeof state.from[x] === "undefined" ? null : state.from[x],
        // formValue: state => x => typeof state.from[x.id][x.key] === "undefined" ? null : state.from[x.id][x.key],
        // formValue: state => x => typeof state.from[x.id],
    },
    mutations: {
        fromBody(state, { formId, body }) {

            if (typeof state.form === "undefined") state.form = {};
            state.form[formId] = body;
            // Vue.set(state.form, formId, body);
            console.log("mutations:fromBody", state.form);
        },
        fromValue(state, { formId, key, value }) {
            state.form[formId][key] = value;
        }
    }
});
const store = new Vuex.Store({
    state: { lists: { } },
    getters: {
        get: (state) => (key) => {
            if (typeof state.lists[key] === "undefined") return null;
            return state.lists[key]["data"] || null;
        },
        getFilter: (state) => (payload) => {
            if (typeof state.lists[payload.key] === "undefined") return null;
            return state.lists[payload.key]["data"].filter(payload.filter) || null;
        },
    },
    mutations: {
        set(state, payload) {
            Vue.set(state.lists, payload.key, {
                key: payload.key,
                source: typeof payload.source === "undefined" ? null : payload.source,
                data: payload.data,
                expires: payload.frequence === "day" ? new Date(new Date().setDate(new Date().getDate() + 1)) : 
                        payload.frequence === "week" ? new Date(new Date().setDate(new Date().getDate() + 7)) :
                        payload.frequence === "month" ? new Date(new Date().setDate(new Date().getDate() + 30)) : payload.frequence
            });
        }
    },
    actions: {
        async load(context, payload) {
            var api = EV.getComponent("data");
            payload.data = await api.SendAsync({url:payload.source});
            if (typeof payload.data === "string") {
                try {
                    payload.data = JSON.parse(payload.data);
                } catch {}
            }
            context.commit('set', payload); //{ key: payload.key, data: payload.data, frequence: payload.frequence });
        },
        async set(context, payload) {
            // console.log("actions", payload);
            context.commit('set', payload); //{ key: payload.key, data: payload.data, frequence: payload.frequence });
        }
    }
});
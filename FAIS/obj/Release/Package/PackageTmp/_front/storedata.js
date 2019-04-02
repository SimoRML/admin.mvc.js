function LoadStoreData() {
    
    store.dispatch("load", { key: "access", source: "/profile/access", frequence: "day" });
    /*store.dispatch("load", { key: "sources", source: "api/shared/GetSources", frequence: "month" });
    store.dispatch("load", { key: "ItemType", source: "api/ItemLists/GetItemListType", frequence: "" });
    store.dispatch("load", { key: "ItemList", source: "api/ItemLists", frequence: "week" });
    store.dispatch("load", { key: "person", source:"api/shared/Person", frequence: "day" });
    */
}
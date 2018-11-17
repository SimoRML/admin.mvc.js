$(function () {
    // LOAD VUE COMPONENTS
    $("#v-components").load("router/vcomponents", function (response, status, xhr) {
        if (status === "success") LOAD();
    });
});

function LOAD() {
    EV = new Environement("Accueil");
    var data = new DataSource("data", {
        baseUrl: "/api/",
        sources: [
            {
                url: "Profile/Menu",
                target: "#menu",
                loadComplete: function (obj) {
                    obj.TriggerFor("Preloader", "hide");
                    obj.TriggerFor("Router", "LoadHomePage");
                }
            },
            {
                url: "Account/Logout",
                on: {
                    el: "#logout",
                    event: "click",
                },
                method: "POST",
                loadComplete: function (obj) {
                    localStorage.clear();
                    location.reload();
                }
            }
        ]
    });
    EV.CreateObject(data);

    EV.CreateObject(new Preloader());
    EV.CreateObject(new Preloader("PagePreloader"));

    EV.CreateObject(new Router("Router", "#viewcontainer",
        {
            home: "#bon.home",
        }
    ));
}
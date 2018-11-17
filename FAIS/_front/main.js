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
                //template: function(key, value){return '<li><a href="#page.'+value.href+'">'+value.text+'</a></li>';},
                loadComplete: function (obj, response) {
                    obj.TriggerFor("Preloader", "hide");
                    obj.TriggerFor("Router", "LoadHomePage");
                    var vmenu = new Vue({
                        el: '#sidebar',
                        data: {
                            list: response
                        }
                    });

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
var indexedProducts = {};
function init($scope, $timeout) {
    $scope.P = new INGOMA.classes.permanentObject("P", {});
    $scope.DV = new INGOMA.classes.permanentObject("DV", []);
    $scope.currentVente = [];
    $scope.currentVenteId = 0;
    $scope.FilteredProducts = {};
    $scope.FrequentProducts = [];
    $scope.synchronizing = false;

    api.Post({
        async: true,
        url: "metabo/filter",
        data: '{"MetaBoID":3,"Items":[]}', // {"Logic":" and ","Field":"nom","Condition":" like ","Value":"%doli%"}
        done: function (response) {
            var produits = {};
            response.forEach((e) => {
                e.id = e.BO_ID;
                e.name = e.nom;
                e.prix = parseFloat(e.PPV);
                e.deleted = false;
                e.temp1 = 0;
                e.temp2 = 0;
                e.temp3 = 0;
                e.icone = e.FORME.replaceAll(" ", "_").toLowerCase();
                produits[e.id] = e;

                // var key = e.nom.substr(0, 2);
            });
            // console.log("produits",produits);
            $scope.P.data = produits;
            $scope.P.save();
            $scope.$apply();
        }
    });


    // LISTS
    $scope.icones = ["photo", "coffee", "beer", "glass", "cutlery", "hdd-o", "apple", "lemon-o", "leaf", "birthday-cake", "cloud", "cube", "fire", "hourglass-o", "inbox", "magic", "moon-o", "money", "newspaper-o", "plug", "sun-o", "video-camera", "volume-control-phone"];
    $scope.colors = ["black", "red", "pink", "purple", "deep-purple", "indigo", "blue", "light-blue", "cyan", "teal", "green", "light-green", "lime", "yellow", "amber", "orange", "deep-orange", "brown", "grey", "blue-grey"];
    $scope.dropDown = { 'icone': 0, 'color': 0 };
    $scope.setIcone = function (icone) {
        $scope.dIcone = icone;
        $scope.dropDown['icone'] = 0;
    };
    $scope.setColor = function (color) {
        $scope.dColor = color;
        $scope.dropDown['color'] = 0;
    };

    // fields 	
    $scope.txtName = "";
    $scope.txtPrix = "";
    $scope.txtCalc = 0;
    $scope.dIcone = "photo";
    $scope.dColor = "black";

    // VARS
    $scope.recette = 1;
    $scope.venteTab = 1;
    $scope.detailVente = [];
    $scope.newlyAdded = false;

    // LOAD EXEC FUNCTIONS
    $timeout(() => {
        $scope.V = new INGOMA.classes.permanentObject("V", []);
        calculerRecette($scope);
        $("#detailVentes").trigger("click");
        $scope.setVenteTab(2);
    }, 1000);


    // filter
    $scope.filter = {
        fields: {
            BO_ID: null
            , CODE: ""
            , DCI1: ""
            , DOSAGE1: ""
            , FORME: ""
            , PH: ""
            , PPV: ""
            , PRESENTATION: ""
            , PRINCEPS_GENERIQUE: ""
            , PRIX_BR: ""
            , TAUX_REMBOURSEMENT: ""
            , UNITE_DOSAGE1: ""
            , couleur: null
            , deleted: null
            , icone: ""
            , id: null
            , name: ""
            , prix: null
            , temp1: 0
            , temp2: 0
            , temp3: 0
        },
        filtering: false,
        filter: function (event) {
            // if (event.which != 13) return;
            if ($scope.filter.filtering) return;

            $scope.filter.filtering = true;
            EV.Event(() => { 
                var where = "1==1";
                for (var field in $scope.filter.fields) {
                    var value = $scope.filter.fields[field];
                    if (value !== null && value !== 0 && value.trim() !== "") {
                        where += " && []." + field + ".toLowerCase().match('" + value.toLowerCase() + "')";
                    }
                }
                // console.log(event.which, where);
                if (where === "1==1") {
                    $scope.FilteredProducts = {};
                } else {
                    $scope.FilteredProducts = $scope.P.Select("*", where);
                }
                $scope.filter.filtering = false;
                $scope.$apply();
            },2);            
        }
    };

    // SYNCHRONIZER
    synchronizer($scope);
}


app.controller('MainController', ['$scope', '$window', '$timeout', function ($scope, $window, $timeout) {
    init($scope, $timeout);

    // AJOUTER PRODUIT
    $scope.addProduct = function () {
        if ($scope.txtName.trim() == "") return;
        var id = $scope.P.nextId();
        $scope.P.data[id] = {
            id: id,
            name: $scope.txtName,
            prix: $scope.txtPrix,
            couleur: $scope.dColor,
            icone: $scope.dIcone,
            deleted: false,
            temp1: 0,
            temp2: 0,
            temp3: 0
        };
        $scope.txtName = "";
        $scope.txtPrix = "";
        $scope.dIcone = "photo";
        $scope.dColor = "black";
        $scope.P.save();
    };
    $scope.deleteProduct = function (pId) {
        if (confirm("Sûr ?")) {
            $scope.P.data[pId].deleted = true;
            $scope.P.save();
        }
    };

    // CURRENT VENTE
    $scope.addCurrentVente = function (id) {
        //console.log(typeof id);
        $scope.currentVente.push(Object.assign({}, $scope.P.data[id]));
        $scope.currentVente[$scope.currentVenteId].temp1 = $scope.currentVenteId;
        $scope.currentVenteId++;
        calculerCurrentVente($scope);
    };
    $scope.deleteCurrentVente = function (id) {
        $scope.currentVente[id].deleted = true;
        calculerCurrentVente($scope);
    };
    $scope.clearCurrentVente = function (id) {
        $scope.currentVente = [];
        calculerCurrentVente($scope);
        $scope.paye = 0;
    };

    // VENTE
    $scope.addVente = function () {
        if ($scope.txtCalc == 0) return;
        var idV = $scope.V.nextId();
        $scope.V.data[idV] = {
            id: idV,
            dateC: new Date(),
            dateM: new Date(),
            etat: 0, // 0 : new, 1 : regle
            deleted: false,
            synchronized: false,
            total: $scope.txtCalc,
            paye: $scope.paye,
            reste: $scope.paye - $scope.txtCalc,
            temp1: 0,
            temp2: 0,
            temp3: 0
        };
        $scope.V.save();
        // DETAIL VENTE
        for (var i in $scope.currentVente) {
            if (!$scope.currentVente[i].deleted) {
                var idDV = $scope.DV.nextId();
                $scope.DV.data[idDV] = {
                    id: idDV,
                    idv: idV,
                    idP: $scope.currentVente[i].id,
                    dateC: new Date(),
                    deleted: false,
                    synchronized: false,
                    temp1: 0,
                    temp2: 0,
                    temp3: 0
                };
            }
        }
        $scope.DV.save();
        $scope.clearCurrentVente();
        $scope.currentVenteId = 0;

        $scope.V.orderBy('id', 'desc');
        $scope.V.save();
        calculerRecette($scope);
        $scope.newlyAdded = true;
        $timeout(function () {
            $scope.newlyAdded = false;
            $timeout(function () {
                $scope.newlyAdded = true;
                $timeout(function () {
                    $scope.newlyAdded = false;
                }, 600);
            }, 200);
        }, 300);

        synchronizer($scope);
    };
    $scope.setVenteTab = function (tab) {
        $scope.venteTab = tab;
        switch (tab) {
            case 1:
                break;
            case 2:
                break;
            case 3:
                $scope.detailVente = getDetailVentes($scope);
                break;
        }
    };
    $scope.regleVente = function () {
        for (var i in $scope.V.data) {
            var v = $scope.V.data[i];
            if (v.etat == 0) {
                $scope.V.data[i].etat = 1;
                $scope.V.data[i].dateM = new Date();
            }
        }
        $scope.V.save();
        calculerRecette($scope);
    };
}]);

function calculerCurrentVente($scope) {
    $scope.txtCalc = 0;
    for (var i in $scope.currentVente) {
        if (!$scope.currentVente[i].deleted) $scope.txtCalc += $scope.currentVente[i].prix;
    }
    $scope.txtCalc = Math.round(($scope.txtCalc) * 100) / 100;
}
function getPrductNextId($scope) {
    var maxId = -1;
    for (var i in $scope.P.data) {
        if (parseInt($scope.P.data[i].id) > maxId) maxId = parseInt($scope.P.data[i].id);
    }

    return maxId == -1 ? 0 : maxId + 1;
}
function getVenteNextId($scope) {
    var maxId = -1;
    for (var i in $scope.V.data) {
        if (parseInt($scope.V.data[i].id) > maxId) maxId = parseInt($scope.V.data[i].id);
    }

    return maxId == -1 ? 0 : maxId + 1;
}
function getDetailVenteNextId($scope) {
    var maxId = -1;
    for (var i in $scope.DV.data) {
        if (parseInt($scope.DV.data[i].id) > maxId) maxId = parseInt($scope.DV.data[i].id);
    }

    return maxId == -1 ? 0 : maxId + 1;
}
function getDetailVentes($scope) {
    var a = {};
    for (var i in $scope.DV.data) {
        var dv = $scope.DV.data[i];
        var p = $scope.P.data[dv.idP]; //$scope.P.Select(["name", "FORME", "icone", "prix"], "[].id==" + dv.idP)[0];
        // console.log("P", p);
        if (typeof a[dv.idP] == "undefined") {
            a[dv.idP] = {
                name: p.name,
                FORME: p.FORME,
                icone: p.icone,
                title: p.name + "\nPrix : " + p.name + "\nForme : " + p.FORME,
                cnt: 0,
                prix: p.prix                
            };
        }
        a[dv.idP].cnt++;
        a[dv.idP].total = Math.round((p.prix * a[dv.idP].cnt) * 100) / 100;
    }
    return a;
}
function calculerRecette($scope) {
    var t = $scope.V.Select(["total"], "[].etat==0");
    $scope.recette = 0;
    for (var k in t) {
        $scope.recette += t[k].total;
    }
    $scope.recette = Math.round(($scope.recette) * 100) / 100;
}

function synchronizer($scope) {
    EV.Event(function () {
        var ventes = [], ventesMeta = [];
        for (var i in $scope.V.data) {
            var current = $scope.V.data[i];
            if (!current.synchronized) {
                ventesMeta.push({ index: i });
                ventes.push({ total: current.total, paye: current.paye, reste: current.reste });
            }
        }
        if (ventes.length > 0) {            
            $scope.synchronizing = true;

            api.Post({ // SEND VENTES
                data: JSON.stringify(ventes),
                url: "metabo/CrudMultiple/4",
                done: function (response) {
                    for (var i in response) {
                        if (response[i].inserted) {
                            $scope.V.data[ventesMeta[i].index].synchronized = true;
                            $scope.V.data[ventesMeta[i].index].BO_ID = response[i].BO_ID;
                        }
                    }
                    $scope.V.save();

                    var detailVentes = [], detailVentesMeta = [];
                    for (var i in $scope.DV.data) {
                        var current = $scope.DV.data[i];
                        if (!current.synchronized) {
                            detailVentesMeta.push({ index: i });
                            detailVentes.push({ Produit: current.idP, parentId: $scope.V.Select(["BO_ID"], "[].id==" + current.idv)[0].BO_ID });
                        }
                    }
                    if (detailVentes.length > 0) {
                        api.Post({ // SEND DETAIL VENTES
                            data: JSON.stringify(detailVentes),
                            url: "metabo/CrudMultipleChilds/5",
                            done: function (response) {
                                for (var i in response) {
                                    if (response[i].inserted) {
                                        $scope.DV.data[detailVentesMeta[i].index].synchronized = true;
                                        $scope.DV.data[detailVentesMeta[i].index].BO_ID = response[i].BO_ID;
                                    }
                                }
                                $scope.DV.save();
                                  
                                $scope.synchronizing = false;
                                $scope.$apply();
                            }
                        });
                    }
                }
            });
        }
        frequentProduct($scope);
    }, 5);
}

function frequentProduct($scope) {
    $scope.FrequentProducts = [];
    var cnt = alasql("select idP, count(idP) as cnt from ? GROUP BY idP order by cnt desc", [$scope.DV.data]);
    var max = 0;
    for (var i in cnt) {
        if (cnt[i].cnt > max) max = cnt[i].cnt;
        // console.log("max", max, "diff", (cnt.cnt > max-10), "cnt.cnt", cnt.cnt, typeof cnt.cnt);
        //if(cnt[i].cnt > max-10) 
        $scope.FrequentProducts.push(cnt[i]);

        if (i > 10) break;
    }
    console.log("frequentProduct", $scope.FrequentProducts);
    $scope.$apply();
    //$scope.FrequentProducts = alasql("select idP, count(idP) as data from ? GROUP BY idP", [dv.data]);
}
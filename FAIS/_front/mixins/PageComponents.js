var EDITION_WIDGET = {
    icone: "print",
    class: "",
    props: {
        source: {
            label: "Source de données",
            type: "v-select",
            datasource: JSON.stringify({ source: "meta_bo", value: "META_BO_ID", display: "BO_NAME", filter: "[STATUS]<>'-1'" }),
            changed: function (payload) {
                // log.greenTitle("changed", value);
                bus.loadList(payload.value, JSON.stringify({ source: "meta_field", value: "DB_NAME", display: "FORM_NAME", filter: "[META_BO_ID]=" + payload.value }),
                    function (data) {
                        // log.green("data", data);
                        vapp.$refs.num[0].populateData(data);
                        vapp.$refs.ref[0].populateData(data);
                        vapp.$refs.aSource[0].populateData(data);

                        var subForm = data.filter(x => x.Attributes.FORM_TYPE.indexOf("subform") != -1);
                        if (subForm.length > 0) {
                            vapp.api.Get({
                                url: "metabo/byname/" + subForm[0].Attributes.FORM_TYPE.replaceAll("subform-", ""),
                                done: function (response) {
                                    bus.loadList(response, JSON.stringify({ source: "meta_field", value: "DB_NAME", display: "FORM_NAME", filter: "[META_BO_ID]=" + response }),
                                        function (subData) {
                                            vapp.$refs.productName[0].populateData(subData);
                                            vapp.$refs.unite[0].populateData(subData);
                                            vapp.$refs.qte[0].populateData(subData);
                                            vapp.$refs.prix[0].populateData(subData);
                                        }
                                    );
                                }
                            });
                        }
                    }
                );
            }
        },
        filter: { label: "Filtre", type: "v-text", detail: "ex: id=[uri.id]" },

        logo: { devider: "En-tête", label: "Logo", type: "v-file" },
        title: { label: "Titre", type: "v-text" },
        societe: { label: "Société", type: "v-textarea" },
        //societeFacture: { label: "Société facturée", type: "v-textarea" },


        aSource: {
            devider: "Société facturée", label: "Champ source", type: "v-select",
            changed: function (payload) {
                try {
                    var source = JSON.parse(payload.object.Attributes.FORM_SOURCE).source;
                    // log.greenTitle("aSource", source);
                    vapp.api.Get({
                        url: "metabo/byname/" + source,
                        done: function (response) {
                            vapp.selectedComponent.props.aDataSource = response;
                            bus.loadList(response, JSON.stringify({ source: "meta_field", value: "DB_NAME", display: "FORM_NAME", filter: "[META_BO_ID]=" + response }),
                                function (data) {
                                    vapp.$refs.aRS[0].populateData(data);
                                    vapp.$refs.aADR[0].populateData(data);
                                    vapp.$refs.aTEL[0].populateData(data);
                                    vapp.$refs.aMAIL[0].populateData(data);
                                }
                            );
                        }
                    });
                } catch (e) { }
            }
        },
        aDataSource: { type: "v-hidden" },
        aRS: { label: "Raison social", type: "v-select" },
        aADR: { label: "Adresse", type: "v-select" },
        aTEL: { label: "Tél", type: "v-select" },
        aMAIL: { label: "Courriel", type: "v-select" },

        num: { devider: "Références", label: "Champ N°", type: "v-select" },
        ref: { label: "Champ Référence", type: "v-select" },

        productName: {
            devider: "Détail: Sub-form", label: "Champ designation", type: "v-select",
            changed: function (payload) {
                vapp.selectedComponent.props.productNameFormat = payload.object.Attributes.GRID_FORMAT;
            }
        },
        productNameFormat: { type: "v-hidden" },

        unite: {
            label: "Champ unité", type: "v-select",
            changed: function (payload) {
                vapp.selectedComponent.props.uniteFormat = payload.object.Attributes.GRID_FORMAT;
            }
        },
        uniteFormat: { type: "v-hidden" },

        qte: {
            label: "Champ quantité", type: "v-select",
            changed: function (payload) {
                vapp.selectedComponent.props.qteFormat = payload.object.Attributes.GRID_FORMAT;
            }
        },
        qteFormat: { type: "v-hidden" },

        prix: {
            label: "Champ prix unitaire", type: "v-select",
            changed: function (payload) {
                vapp.selectedComponent.props.prixFormat = payload.object.Attributes.GRID_FORMAT;
            }
        },
        prixFormat: { type: "v-hidden" },

        footer: { devider: "Pied", label: "Pied de page", type: "v-textarea" },
    },
    apply: function ($compDiv, props, compKey) {
        //log.green("props", props);
        INCLUDE($compDiv, "router/widgets/"+ this.widgetName +"?compKey=" + compKey, function () {
            window["vappEditionWidget" + compKey].val(props);
        });
    }
}
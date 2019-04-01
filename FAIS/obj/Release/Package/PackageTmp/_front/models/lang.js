/*
 <th>Champ</th>
                                        <th>Libéllé</th>
                                        <th>Largeur</th>
 */
var language = {
    computed: {
        L: function () { 
            var words = {
                key: "clé",
                optional: "optionnel",
                text_to_display: "texte à afficher",
                condition: "condition",
                field: "champ",
                label: "libéllé",
                width: "largeur",
                list: "Liste",
                add: "ajouter",
                edit: "modifier",
                name: "nom",
                status: "statut",
                type: "type",
                title: "titre",
                save: "enregistrer",
                cancel: "annuler",
                data_source: "source de données",
                conditional: "conditionelle",
                parent: "parent",
                filter: "filtre",
                version: "version",
                enter_one_element_per_ligne: "entrez un élément par ligne",
            };

            // Plural
            for (var key in words) {
                if(key.match("_") === null && typeof words[key + "s"] === "undefined")
                    words[key + "s"] = words[key] + "s";
            }

            for (var key in words) {
                // First capital
                if(typeof words[capitalizeFirstLetter(key)] === "undefined")
                    words[capitalizeFirstLetter(key)] = capitalizeFirstLetter(words[key]);

                // Upper case
                if(typeof words[key.toUpperCase()] === "undefined")
                    words[key.toUpperCase()] = words[key].toUpperCase();
            }

            return words;
        }
    }
};
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
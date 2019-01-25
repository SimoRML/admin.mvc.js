/**
 * Created by SimoRML on 15/06/15.
 */
function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); };
String.prototype.ltrim = function () { return this.replace(/^\s+/, ''); };
String.prototype.rtrim = function () { return this.replace(/\s+$/, ''); };
String.prototype.fulltrim = function () { return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' '); };
if (!String.prototype.contains) {
    String.prototype.contains = function () {
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}

var INGOMA = INGOMA || {};

INGOMA.vars = {
    soundOn: false,
    snap: null,
    server_url: "http://www.ingoma.net/scrapper/ajax/galenus.php",
    pass: "secureLine"
};

INGOMA.methods = {
    playSound: function (sound) {
        if (INGOMA.vars.soundOn && sound) {
            var audioElement = document.createElement('audio');
            audioElement.setAttribute('src', sound);
            audioElement.setAttribute('autoplay', 'autoplay');
            audioElement.play();
        }
    }
};

INGOMA.util = {
    LinkedList: function () {
        this.index = -1;
        this.items = new Array();

        this.myIndex = function () {
            return this.index;
        };
        this.current = function () {
            if (this.index != -1) return this.items[this.index];
            else return false;
        };
        this.next = function (toThisIndex) {
            if (this.index + 1 < this.count()) {
                if (toThisIndex) this.index = toThisIndex + 1;
                else this.index++;

                if (this.items[this.index].active) return this.items[this.index];
                return this.next();
            }
            else return false;
        };
        this.previous = function () {
            if (this.index - 1 >= 0) {
                this.index--;
                if (this.items[this.index].active) return this.items[this.index];
                else return this.previous();
            }
            else return false;
        };
        this.first = function () {
            if (this.count() > 0) {
                this.index = 0;
                if (this.items[this.index].active) return this.items[this.index];
                return this.next();
            }
            else return false;
        };

        this.last = function () {
            if (this.count() > 0) {
                this.index = this.count() - 1;
                if (this.items[this.index].active) return this.items[this.index];
                else return this.previous();
            }
            else return false;
        };

        this.hasItems = function () {
            return (this.count() > 0);
        };
        this.hasActiveItems = function () {
            return (this.countActive() > 0);
        };

        this.count = function () {
            return this.items.length;
        };
        this.countActive = function () {
            var cnt = 0;
            for (var i = 0; i < this.items.length; i++) {
                if (this.items[i].active) cnt++;
            }
            return cnt;
        };

        this.add = function (item) {
            this.items[this.count()] = { element: item, active: true, index: this.count() };
        };
        this.remove = function () {
            if (this.index - 1 >= 0) this.items[this.index].active = false;
        };
        this.removeAt = function (index) {
            if (this.items[index]) this.items[index].active = false;
        };

        this.toString = function () {
            var rst = "\ncount : " + this.count();
            rst += "\nindex : " + this.index;
            rst += "\ncurrent : " + this.current() + "\nitems : ";
            for (var i = 0; i < this.count(); i++) {
                if (i > 0) rst += ",";
                rst += "{index: " + this.items[i].index + ", active: " + this.items[i].active + ", element: " + this.items[i].element + "}";
            }
            return rst + "\n";
        };
    }
};

INGOMA.classes = {
    Objet: function (sound, width, height) {
        this.sound = null;
        if (sound) this.sound = sound;

        this.imgs = new INGOMA.util.LinkedList();
        this.snap = null;
        this.x = null;
        this.y = null;
        this.W = width;
        this.H = height;

        this.playSound = function () {
            INGOMA.methods.playSound(this.sound);
        };
        this.render = function (attrs) {
            var me = this;

            Snap.load(me.imgs.current().element, function (f) {
                me.snap = f.select("g");
                INGOMA.vars.snap.append(me.snap);
                me.snap.select("#obj").attr(attrs);
            });
        };
    },
    Game: function (width, height) {
        this.map = null;
        this.nbrPlayers = 0;
        this.admin = null;
        this.players = new INGOMA.util.LinkedList();
        this.W = width;
        this.H = height;
        this.snap = Snap(this.W, this.H);
        INGOMA.vars.snap = this.snap;

        this.loadMap = function (mapPath) {
            this.map = new INGOMA.classes.Map(this.W, this.H);
            this.map.loadFile(mapPath);
        };
    },
    Map: function (width, height) {
        INGOMA.classes.Objet.call(this, 'sound/ZdMinaret.wav', width, height);


        this.tiles = new INGOMA.util.LinkedList();
        this.echels = new INGOMA.util.LinkedList();
        this.serpents = new INGOMA.util.LinkedList();
        this.bombes = new INGOMA.util.LinkedList();
        this.des = new INGOMA.util.LinkedList();

        this.loadFile = function (file) {
            var me = this;
            //console.log("file "+file+" is loading  ...");
            $.getScript(file, function () {
                //console.log(mapData);

                var x = parseInt(mapData.tiles.split("x")[0]);
                var y = parseInt(mapData.tiles.split("x")[1]);

                //tiles
                for (var j = 0; j < y; j++) {
                    for (var i = 0; i < x; i++) {
                        me.tiles.add(new INGOMA.classes.tile(i, j, (me.W / x), (me.H / y)));
                    }
                }

                me.playSound();
            });
        };
    },
    tile: function (i, j, width, height) {
        INGOMA.classes.Objet.call(this, '', width, height);

        this.i = i;
        this.j = j;
        this.index = i + j;
        this.items = new INGOMA.util.LinkedList();

        this.imgs.add("svg/tile.svg");
        this.imgs.first();
        this.x = this.i * this.W;
        this.y = this.j * this.H;
        this.render({
            width: this.W,
            height: this.Y,
            x: this.x,
            y: this.y
        });
    },
    client: function (lang, server_url, no_persistance) {
        //Declare
        this.server_url = INGOMA.vars.server_url;
        this.lang = lang;
        this.lang_checkbox_prefix = "#c_";
        this.templates = [];
        this.args = {
            site: -1,
            cat: -1
        };

        //INIT
        this.INIT = function () {
            //initiate from constructor
            if (lang) this.lang = lang;
            if (server_url) this.server_url = server_url;
            //Load persisted settings
            if (!no_persistance) this.persist_settings();
            //populate page
            this.populate_settings();
        };

        //METHODES
        this.setLang = function () {
            var lang = "";
            if ($(this.lang_checkbox_prefix + "ar_AR").prop("checked") == true) lang += "ar_AR";
            if ($(this.lang_checkbox_prefix + "fr_FR").prop("checked") == true) {
                if (lang != "") lang += ",";
                lang += "fr_FR";
            }
            if ($(this.lang_checkbox_prefix + "en_EN").prop("checked") == true) {
                if (lang != "") lang += ",";
                lang += "en_EN";
            }
            if (lang == "") return false;
            if (lang == this.lang) return false;

            this.lang = lang;
            if (!no_persistance) localStorage.setItem("lang", this.lang);
            return true;
        };

        this.persist_settings = function () {
            if (localStorage.getItem("lang")) c.lang = localStorage.getItem("lang");
            else localStorage.setItem("lang", c.lang);

            for (var key in this.args) {
                if (localStorage.getItem(key)) this.args[key] = localStorage.getItem(key);
                else localStorage.setItem(key, this.args[key]);
            }
        };

        this.setArg = function (key, value) {
            if (value == this.args[key]) return false;

            this.args[key] = value;
            if (!no_persistance) localStorage.setItem(key, this.args[key]);
            return true;
        };

        this.populate_settings = function () {
            var langs = c.lang.split(",");
            for (var i = 0; i < langs.length; i++) {
                $(this.lang_checkbox_prefix + langs[i]).prop("checked", true);
            }
        };

        this.call_remote_function = function (method, params, success, fail) {
            var args = $.extend(false, {}, {
                m: method,
                pass: INGOMA.vars.pass
            }, params);
            //console.log(args);
            $.post(this.server_url, args).done(success).fail(fail);
        };

        this.addTemplate = function (key, html) {
            this.templates[key] = html;
        };

        this.populate = function (template_key, target, data, append) {
            if (!append) $(target).html("");

            var html = "";
            for (var i = 0; i < data.length; i++) {
                html = this.templates[template_key];
                ////console.log(i,template_key);
                for (var key in data[i]) {
                    ////console.log(key + " : ",data[i][key]);
                    if (data[i][key]) html = replaceAll(html, "[@" + key + "]", data[i][key]);
                    else html = replaceAll(html, "[@" + key + "]", "");
                }
                $(target).append($(html));
            }
            $(target).trigger('create');
        };


    },

    permanentObject: function (id, data, type) {
        this.type = typeof (type) == "undefined" ? "json" : type;
        this.data = data;
        this.id = id;

        this.init = function () {
            if (localStorage.getItem("permanent_" + this.id)) {
                try {
                    switch (this.type) {
                        case "json":
                            this.data = JSON.parse(localStorage.getItem("permanent_" + this.id));
                            break;
                        default:
                            this.data = localStorage.getItem("permanent_" + this.id);
                            break;
                    }
                }
                catch (err) {
                    this.save();
                }

            }
            else this.save();
        };
        this.save = function () {
            switch (this.type) {
                case "json":
                    //console.log("save 1 ", this.data);
                    for (var i in this.data) {
                        delete this.data[i]["$$hashKey"];
                    }
                    //console.log("save 2 ", JSON.stringify(this.data));
                    localStorage.setItem("permanent_" + this.id, JSON.stringify(this.data));
                    break;
                default:
                    localStorage.setItem("permanent_" + this.id, this.data);
                    break;
            }
        };
        this.orderBy = function (field, dir) {
            if (dir == "asc") {
                this.data.sort(function (a, b) {
                    if (a[field] instanceof Date) {
                        if (a[field].getTime() > b[field].getTime())
                            return 1;
                        if (a[field].getTime() < b[field].getTime())
                            return -1;
                        return 0;
                    } else {
                        if (a[field] > b[field])
                            return 1;
                        if (a[field] < b[field])
                            return -1;
                        return 0;
                    }
                });
            } else if (dir == "desc") {
                this.data.sort(function (a, b) {
                    if (a[field] instanceof Date) {
                        if (a[field].getTime() > b[field].getTime())
                            return -1;
                        if (a[field].getTime() < b[field].getTime())
                            return 1;
                        return 0;
                    } else {
                        if (a[field] > b[field])
                            return -1;
                        if (a[field] < b[field])
                            return 1;
                        return 0;
                    }
                });
            }
        };
        this.nextId = function () {
            var maxId = -1;
            for (var i in this.data) {
                if (parseInt(this.data[i].id) > maxId) maxId = parseInt(this.data[i].id);
            }

            return maxId == -1 ? 0 : maxId + 1;
        };

        /*
         * fields : * | array of fields names
         * where : [].field == 'qlq chose'
         *          [].field == 'qlq chose' && [].field2 < 5 || [].field3.toLowerCase().match('valeur')
         */ 
        this.Select = function (fields, where) {
            where = where.replaceAll("[]", "this.data[i]");
            var t = [];
            for (var i in this.data) {
                if (eval(where.trim())) {
                    var row = {};

                    if (fields === "*") { // select * 
                        t.push(this.data[i]);
                    } else { // select some fields
                        for (var j in fields) {
                            row[fields[j]] = this.data[i][fields[j]];
                        }
                        t.push(row);
                    }
                }
            }
            return t;
        };
        this.init();
    }
};

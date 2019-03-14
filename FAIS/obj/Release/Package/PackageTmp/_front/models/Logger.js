const log = {
    ACTIVE: true,
    on: function () { this.ACTIVE = true; },
    off: function () { console.warn("LOGGER is turned OFF"); this.ACTIVE = false; },
    init: function () {
        console.time('looper');
        let i = 0;
        while(i<1000000){i++}
        console.timeEnd('looper');
    },
    log: function (...args) {
        if (!this.ACTIVE) return;
        console.log(...args);
    },
    trace: function (...args) {
        if (!this.ACTIVE) return;
        console.trace(...args);
    },
    group: function (...args) {
        if (!this.ACTIVE) return;
        console.group(...args);
    },
    groupEnd: function (...args) {
        args[0] = "--- // " + args[0];
        this.green(...args);
        console.groupEnd();
    },
    /* STYLING */
    title: function (color, ...args) {
        this.log("%c" + args[0], "padding:2px 5px; font-weight:bold; font-size:120%; color:"+color
            +";border: 1px solid "+color
            +";" + (color==="white" ? "background:#000":""),...args.slice(1));
    },
    blueTitle: function (...args) {
        this.title("blue",...args);
    },
    redTitle: function (...args) {
        this.title("red",...args);
    },
    orangeTitle: function (...args) {
        this.title("orange",...args);
    },
    greenTitle: function (...args) {
        this.title("green",...args);
    },
    whiteTitle: function (...args) {
        this.title("white",...args);
    },
    blue: function (...args) {
        this.log("%c" + args[0], "color:blue; font-weight:bold;",...args.slice(1));
    },
    red: function (...args) {
        this.log("%c" + args[0], "color:red; font-weight:bold;",...args.slice(1));
    },
    green: function (...args) {
        this.log("%c" + args[0], "color:green; font-weight:bold;",...args.slice(1));
    },
    orange: function (...args) {
        this.log("%c" + args[0], "color:orange; font-weight:bold;",...args.slice(1));
    },
    table: function (...args) {
        if (!this.ACTIVE) return;
        console.table(JSON.parse(JSON.stringify(args[0])),...args.slice(1));
    },
};
log.init();
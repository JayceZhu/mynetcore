var AssetAdapter = function () {
    function e() { }
    var t = (__define, e),
        i = t.prototype;
    return i.getAsset = function (e, t, i) {
        function n(n) {
            t.call(i, n, e)
        }
        if (RES.hasRes(e)) {
            var a = RES.getRes(e);
            a ? n(a) : RES.getResAsync(e, n, this)
        } else RES.getResByUrl(e, n, this, RES.ResourceItem.TYPE_IMAGE)
    },
        e
}();
egret.registerClass(AssetAdapter, "AssetAdapter", ["eui.IAssetAdapter"]);
var GamePlaying = function (e) {
    function t() {
        e.call(this),
            this.PrizeNum = 12,
            this.LoadPicNum = 0,
            this.WillLoadPicNum = 0,
            t.GameContent = this,
            window.ZlanLuck || (window.ZlanLuck = {}),
            window.ZlanLuck.LuckStart = this.LuckStart,
            window.ZlanLuck.LoadPrize = this.LoadPrize,
            window.ZlanLuck.giftCode = null,
            window.ZlanLuck.SetPrize = this.SetPrize,
            window.ZlanLuck.LuckButton && (window.ZlanLuck.LuckButton.width && (t.DIY_LOTTERY_WIDTH = window.ZlanLuck.LuckButton.width), window.ZlanLuck.LuckButton.height && (t.DIY_LOTTERY_HEIGHT = window.ZlanLuck.LuckButton.height), window.ZlanLuck.LuckButton.x && (t.DIY_LOTTERY_X = window.ZlanLuck.LuckButton.x), window.ZlanLuck.LuckButton.y && (t.DIY_LOTTERY_Y = window.ZlanLuck.LuckButton.y)),
            window.ZlanLuck.Award && (window.ZlanLuck.Award.width && (t.DIY_AWARD_WIDTH = window.ZlanLuck.Award.width), window.ZlanLuck.Award.minspeed && (t.MARK_CHANGE_TIMES_PER_SECOND_MIN = window.ZlanLuck.Award.minspeed), window.ZlanLuck.Award.maxspeed && (t.MARK_CHANGE_TIMES_PER_SECOND_MAX = window.ZlanLuck.Award.maxspeed), window.ZlanLuck.Award.lastpxtime && (t.LASTPXTIME = window.ZlanLuck.Award.lastpxtime)),
            this.panelList || (this.panelList = ["resource/assets/panel0.png", "resource/assets/panel1.png", "resource/assets/panel2.png"]),
            this.skinName = "GamePlayingSkin",
            this.init();
        var i = {
            height: 900,
            top: 0,
            left: 0,
            width: 900
        };
        window.ZlanLuck.Background && (window.ZlanLuck.Background.panelList && (this.panelList = window.ZlanLuck.Background.panelList), window.ZlanLuck.Background.x && (i.left = window.ZlanLuck.Background.x), window.ZlanLuck.Background.y && (i.top = window.ZlanLuck.Background.y), window.ZlanLuck.Background.width && (i.width = window.ZlanLuck.Background.width), window.ZlanLuck.Background.height && (i.height = window.ZlanLuck.Background.height)),
            this.panel.height = i.height,
            this.panel.top = i.top,
            this.panel.left = i.left,
            this.panel.width = i.width,
            window.ZlanLuck.PrizeNum && (this.PrizeNum = window.ZlanLuck.PrizeNum, this.SetPrizeSite(this.PrizeNum)),
            window.ZlanLuck.OnLoad && window.ZlanLuck.OnLoad()
    }
    __extends(t, e);
    var i = (__define, t),
        n = i.prototype;
    return n.SetPrizeSite = function (e) {
        var i = [[], [], []];
        if (12 == e ? i.push([]) : (t.ROW_NUM = 3, t.COL_NUM = 3, t.AWARD_WIDTH = 240, t.AWARD_COUNT = 8, t.BTN_LOTTERY_WIDTH = 240, t.AWARD_RDS = t.AWARD_WIDTH / 5, window.ZlanLuck.Award && window.ZlanLuck.Award.radius && (t.AWARD_RDS = Window.ZlanLuck.Award.radius)), t.DIY_AWARD_WIDTH > 0 && (t.AWARD_WIDTH = t.DIY_AWARD_WIDTH), t.MARGIN = (t.AWARD_PANEL_WIDTH - t.AWARD_WIDTH * t.ROW_NUM) / (t.ROW_NUM + 1), 12 == e) {
            for (var n = 0; 4 > n; n++) i[0].push({});
            for (var n = 4; 6 > n; n++) i[1].push({});
            for (var n = 6; 8 > n; n++) i[2].push({});
            for (var n = 8; 12 > n; n++) i[3].push({})
        } else {
            for (var n = 0; 3 > n; n++) i[0].push({});
            for (var n = 3; 5 > n; n++) i[1].push({});
            for (var n = 5; 8 > n; n++) i[2].push({})
        }
        for (var n = 0,
            a = i.length; a > n; n++) for (var r = 0,
                o = i[n].length; o > r; r++) {
                var s = i[n][r],
                    d = new egret.Shape,
                    l = d.graphics;
                l.lineStyle(2, 16711680, 0),
                    l.beginFill(16777215, .8),
                    l.drawRoundRect(0, 0, t.AWARD_WIDTH, t.AWARD_WIDTH, t.AWARD_RDS, t.AWARD_RDS),
                    l.endFill();
                var h = new egret.TextField;
                h.text = "Load...",
                    h.height = t.AWARD_WIDTH,
                    h.width = t.AWARD_WIDTH,
                    h.textAlign = "center",
                    h.verticalAlign = "middle";
                var u = {
                    x: (1 == n || 2 == n && 4 == t.ROW_NUM) && 1 == r ? t.AWARD_PANEL_X + t.MARGIN * t.ROW_NUM + t.AWARD_WIDTH * (t.ROW_NUM - 1) : t.AWARD_PANEL_X + t.MARGIN * (r + 1) + t.AWARD_WIDTH * r,
                    y: t.AWARD_PANEL_Y + t.MARGIN * (n + 1) + t.AWARD_WIDTH * n
                };
                d.blendMode = egret.BlendMode.ADD,
                    d.x = u.x,
                    d.y = u.y,
                    h.x = u.x,
                    h.y = u.y,
                    h.textColor = 0,
                    h.size = 30,
                    t.GameContent.addChild(d),
                    t.GameContent.addChild(h),
                    s.gvd = d,
                    s.gtext = h
            }
        t.LoadArr = i,
            t.GameContent.Pushluckbutton()
    },
        n.LoadPrize = function (e) {
            if (e || (e = RES.getRes("awards")), e.length && (12 == e.length || 8 == e.length)) {
                if (window.ZlanLuck.PrizeNum) {
                    if (t.GameContent.PrizeNum != e.length) return void console.log("奖品数量和设置数量不符")
                } else t.GameContent.SetPrizeSite(e.length);
                var i = [[], [], []];
                if (12 == e.length && i.push([]), 12 == e.length) {
                    for (var n = 0; 4 > n; n++) i[0].push(e[n]);
                    for (var n = 4; 6 > n; n++) i[1].push(e[n]);
                    for (var n = 6; 8 > n; n++) i[2].push(e[n]);
                    for (var n = 8; 12 > n; n++) i[3].push(e[n])
                } else {
                    for (var n = 0; 3 > n; n++) i[0].push(e[n]);
                    for (var n = 3; 5 > n; n++) i[1].push(e[n]);
                    for (var n = 5; 8 > n; n++) i[2].push(e[n])
                }
                var a = t.GameContent;
                a.awardArr = i;
                var r = a.awardArr,
                    o = new Array;
                12 == e.length ? a.lotteryLine = [r[0][0], r[0][1], r[0][2], r[0][3], r[1][1], r[2][1], r[3][3], r[3][2], r[3][1], r[3][0], r[2][0], r[1][0]] : a.lotteryLine = [r[0][0], r[0][1], r[0][2], r[1][1], r[2][2], r[2][1], r[2][0], r[1][0]];
                for (var n = 0,
                    s = r.length; s > n; n++) for (var d = 0,
                        l = r[n].length; l > d; d++) {
                        var h = r[n][d],
                            u = {
                                view: t.GameContent,
                                width: t.AWARD_WIDTH,
                                height: t.AWARD_WIDTH,
                                x: (1 == n || 2 == n && 4 == t.ROW_NUM) && 1 == d ? t.AWARD_PANEL_X + t.MARGIN * t.ROW_NUM + t.AWARD_WIDTH * (t.ROW_NUM - 1) : t.AWARD_PANEL_X + t.MARGIN * (d + 1) + t.AWARD_WIDTH * d,
                                y: t.AWARD_PANEL_Y + t.MARGIN * (n + 1) + t.AWARD_WIDTH * n,
                                award: h,
                                awardArr: r,
                                uri: h.img,
                                gview: t.LoadArr[n][d].gvd,
                                gtext: t.LoadArr[n][d].gtext,
                                com: function (e) {
                                    this.view.removeChild(this.gview),
                                        this.view.removeChild(this.gtext);
                                    var i = e,
                                        n = new egret.Bitmap(i);
                                    if (n.width = this.width, n.height = this.height, n.x = this.x, n.y = this.y, this.view.addChild(n), this.award.x = n.x, this.award.y = n.y, this.view.LoadPicNum++ , this.view.LoadPicNum == t.GameContent.WillLoadPicNum) {
                                        this.view.awardMark = new egret.Shape;
                                        var a = this.view.awardMark.graphics;
                                        a.lineStyle(2, 16711680, 0);
                                        var r = {
                                            color: "0xffffff",
                                            opacity: .3,
                                            rds: t.AWARD_WIDTH / 5,
                                            height: t.AWARD_WIDTH,
                                            width: t.AWARD_WIDTH
                                        };
                                        window.ZlanLuck.Fill && (window.ZlanLuck.Fill.color && (r.color = window.ZlanLuck.Fill.color), window.ZlanLuck.Fill.opacity && (r.opacity = window.ZlanLuck.Fill.opacity), window.ZlanLuck.Fill.radius && (r.rds = window.ZlanLuck.Fill.radius), window.ZlanLuck.Fill.width && (r.width = window.ZlanLuck.Fill.width), window.ZlanLuck.Fill.height && (r.height = window.ZlanLuck.Fill.height), window.ZlanLuck.Fill.fx && (t.fX = window.ZlanLuck.Fill.fx), window.ZlanLuck.Fill.fy && (t.fY = window.ZlanLuck.Fill.fy)),
                                            a.beginFill(r.color, r.opacity),
                                            a.drawRoundRect(0, 0, r.width, r.height, r.rds, r.rds),
                                            a.endFill(),
                                            this.view.awardMark.x = -999,
                                            this.view.awardMark.blendMode = egret.BlendMode.ADD,
                                            this.view.addChild(this.view.awardMark),
                                            t.GameContent.btnLottery.touchEnabled = !0
                                    }
                                }
                            };
                        o.push(u)
                    }
                a.WillLoadPicNum = o.length,
                    o.forEach(function (e) {
                        RES.getResByUrl(e.uri, e.com, e, RES.ResourceItem.TYPE_IMAGE)
                    })
            }
        },
        n.Pushluckbutton = function () {
            var e = t.GameContent,
                i = 1,
                n = 1,
                a = (1 == i || 2 == i && 4 == t.ROW_NUM) && 1 == n ? t.AWARD_PANEL_X + t.MARGIN * t.ROW_NUM + t.AWARD_WIDTH * (t.ROW_NUM - 1) : t.AWARD_PANEL_X + t.MARGIN * (n + 1) + t.AWARD_WIDTH * n;
            n = 0;
            var r = (1 == i || 2 == i && 4 == t.ROW_NUM) && 1 == n ? t.AWARD_PANEL_X + t.MARGIN * t.ROW_NUM + t.AWARD_WIDTH * (t.ROW_NUM - 1) : t.AWARD_PANEL_X + t.MARGIN * (n + 1) + t.AWARD_WIDTH * n,
                o = t.AWARD_PANEL_Y + t.MARGIN * (i + 1) + t.AWARD_WIDTH * i,
                s = (a - r - t.AWARD_WIDTH - t.BTN_LOTTERY_WIDTH) / 2;
            e.btnLottery = new egret.Bitmap(RES.getRes("btnLottery")),
                e.btnLottery.width = t.BTN_LOTTERY_WIDTH,
                e.btnLottery.height = t.BTN_LOTTERY_WIDTH,
                t.DIY_LOTTERY_HEIGHT > 0 && (e.btnLottery.height = t.DIY_LOTTERY_HEIGHT),
                t.DIY_LOTTERY_WIDTH > 0 && (e.btnLottery.width = t.DIY_LOTTERY_WIDTH),
                e.btnLottery.x = r + t.AWARD_WIDTH + s,
                e.btnLottery.y = o - t.MARGIN + s,
                0 != t.DIY_LOTTERY_X && (e.btnLottery.x = t.DIY_LOTTERY_X),
                0 != t.DIY_LOTTERY_Y && (e.btnLottery.y = t.DIY_LOTTERY_Y),
                e.addChild(e.btnLottery),
                e.btnLottery.touchEnabled = !1
        },
        n.init = function () {
            this.isStart = !1,
                this.panelIndex = 0,
                this.panelLastTime = egret.getTimer(),
                this.addEventListener(egret.TouchEvent.TOUCH_TAP, this.onTouchTap, this)
        },
        n.childrenCreated = function () {
            e.prototype.childrenCreated.call(this),
                this.addEventListener(egret.Event.ENTER_FRAME, this.update, this)
        },
        n.onTouchLuck = function () {
            window.ZlanLuck && window.ZlanLuck.onTouchLuck && window.ZlanLuck.onTouchLuck()
        },
        n.onTouchTap = function (e) {
            var t = e.target;
            t === this.btnLottery && this.onTouchLuck()
        },
        n.SetPrize = function (e) {
            window.ZlanLuck.giftCode = e
        },
        n.LuckStart = function () {
            var e = t.GameContent;
            if (!e.isStart) {
                e.isStart = !0,
                    e.btnLottery.touchEnabled = !1,
                    window.ZlanLuck.giftCode = null,
                    e.awardCode = null,
                    e.markIndex || (e.markIndex = 0),
                    e.lastTime = 0,
                    e.awardCode = "",
                    e.reduceOffset = 0;
                var i = e.awardArr[0][0];
                e.awardMark.x = i.x - 2,
                    e.awardMark.y = i.y - 2
            }
        },
        n.update = function () {
            this.isStart && this.markMove();
            var e = egret.getTimer();
            e - this.panelLastTime >= 500 && (this.panelIndex = (this.panelIndex + 1) % this.panelList.length, this.panel.source = this.panelList[this.panelIndex], this.panelLastTime = e)
        },
        n.markMove = function () {
            var e = Math.floor(1e3 / (t.MARK_CHANGE_TIMES_PER_SECOND_MAX - this.reduceOffset)),
                i = egret.getTimer();
            if (!(i - this.lastTime < e)) {
                this.awardCode = window.ZlanLuck.giftCode,
                    this.awardCode && ("NULL" == this.awardCode && (this.isStart = !1, this.awardMark.visible = !1), t.MARK_CHANGE_TIMES_PER_SECOND_MAX - this.reduceOffset > t.MARK_CHANGE_TIMES_PER_SECOND_MIN && (this.reduceOffset += Math.ceil(t.MARK_CHANGE_TIMES_PER_SECOND_MAX * (i - this.lastTime) / 2e3), t.MARK_CHANGE_TIMES_PER_SECOND_MAX - this.reduceOffset < t.MARK_CHANGE_TIMES_PER_SECOND_MIN && (this.reduceOffset = t.MARK_CHANGE_TIMES_PER_SECOND_MAX - t.MARK_CHANGE_TIMES_PER_SECOND_MIN))),
                    0 == this.lastTime ? (this.lastTime = i, this.markIndex = ++this.markIndex % this.PrizeNum) : (this.markIndex = (this.markIndex + Math.floor((i - this.lastTime) / e)) % t.AWARD_COUNT, this.lastTime = i);
                var n = this.lotteryLine[this.markIndex];
                this.awardMark.x = n.x + t.fX,
                    this.awardMark.y = n.y + t.fY,
                    t.MARK_CHANGE_TIMES_PER_SECOND_MAX - this.reduceOffset === t.MARK_CHANGE_TIMES_PER_SECOND_MIN && n.name === this.awardCode && (this.isStart = !1, egret.Tween.get(this.awardMark).wait(t.LASTPXTIME).set({
                        visible: !1
                    },
                        this.awardMark).wait(t.LASTPXTIME).set({
                            visible: !0
                        },
                            this.awardMark).wait(t.LASTPXTIME).set({
                                visible: !1
                            },
                                this.awardMark).wait(t.LASTPXTIME).set({
                                    visible: !0
                                },
                                    this.awardMark).wait(t.LASTPXTIME).set({
                                        visible: !1
                                    },
                                        this.awardMark).wait(t.LASTPXTIME).set({
                                            visible: !0
                                        },
                                            this.awardMark).call(function () {
                                                window.ZlanLuck && window.ZlanLuck.LuckEnd(),
                                                    this.btnLottery.touchEnabled = !0
                                            },
                                                this))
            }
        },
        t.AWARD_COUNT = 12,
        t.AWARD_PANEL_X = 50,
        t.AWARD_PANEL_Y = 50,
        t.AWARD_PANEL_WIDTH = 800,
        t.AWARD_PANEL_HEIGHT = 800,
        t.ROW_NUM = 4,
        t.COL_NUM = 4,
        t.AWARD_WIDTH = 180,
        t.AWARD_RDS = t.AWARD_WIDTH / 5,
        t.BTN_LOTTERY_WIDTH = 340,
        t.DIY_LOTTERY_HEIGHT = 0,
        t.DIY_LOTTERY_WIDTH = 0,
        t.DIY_LOTTERY_X = 0,
        t.DIY_LOTTERY_Y = 0,
        t.DIY_PIC_NUM = 3,
        t.DIY_AWARD_WIDTH = 0,
        t.DIY_AWARD_RDS = 0,
        t.LASTPXTIME = 50,
        t.MARGIN = (t.AWARD_PANEL_WIDTH - t.AWARD_WIDTH * t.ROW_NUM) / (t.ROW_NUM + 1),
        t.MARK_CHANGE_TIMES_PER_SECOND_MAX = 100,
        t.MARK_CHANGE_TIMES_PER_SECOND_MIN = 100,
        t.fX = 0,
        t.fY = 0,
        t
}(eui.Component);
egret.registerClass(GamePlaying, "GamePlaying");
var LoadingUI = function (e) {
    function t() {
        e.call(this),
            this.createView()
    }
    __extends(t, e);
    var i = (__define, t),
        n = i.prototype;
    return n.createView = function () {
        this.textField = new egret.TextField,
            this.addChild(this.textField),
            this.textField.y = 900,
            this.textField.width = 900,
            this.textField.height = 100,
            this.textField.textAlign = "center"
    },
        n.setProgress = function (e, t) {
            this.textField.text = "Loading..." + Math.floor(e / t * 100) + "%"
        },
        t
}(egret.Sprite);
egret.registerClass(LoadingUI, "LoadingUI");
var Main = function (e) {
    function t() {
        e.apply(this, arguments),
            this.isThemeLoadEnd = !1,
            this.isResourceLoadEnd = !1
    }
    __extends(t, e);
    var i = (__define, t),
        n = i.prototype;
    return n.createChildren = function () {
        e.prototype.createChildren.call(this);
        var t = new AssetAdapter;
        this.stage.registerImplementation("eui.IAssetAdapter", t),
            this.stage.registerImplementation("eui.IThemeAdapter", new ThemeAdapter),
            RES.addEventListener(RES.ResourceEvent.CONFIG_COMPLETE, this.onConfigComplete, this),
            RES.loadConfig("resource/default.res.json", "resource/")
    },
        n.onConfigComplete = function (e) {
            RES.removeEventListener(RES.ResourceEvent.CONFIG_COMPLETE, this.onConfigComplete, this);
            var t = new eui.Theme("/resource/default.thm.json", this.stage);
            t.addEventListener(eui.UIEvent.COMPLETE, this.onThemeLoadComplete, this),
                RES.addEventListener(RES.ResourceEvent.GROUP_COMPLETE, this.onResourceLoadComplete, this),
                RES.addEventListener(RES.ResourceEvent.GROUP_LOAD_ERROR, this.onResourceLoadError, this),
                RES.addEventListener(RES.ResourceEvent.GROUP_PROGRESS, this.onResourceProgress, this),
                RES.addEventListener(RES.ResourceEvent.ITEM_LOAD_ERROR, this.onItemLoadError, this),
                RES.loadGroup("loading")
        },
        n.onThemeLoadComplete = function () {
            this.isThemeLoadEnd = !0,
                this.createScene()
        },
        n.onResourceLoadComplete = function (e) {
            "loading" == e.groupName ? (this.loadingView = new LoadingUI, this.stage.addChild(this.loadingView), RES.loadGroup("preload")) : "preload" == e.groupName && (this.stage.removeChild(this.loadingView), RES.removeEventListener(RES.ResourceEvent.GROUP_COMPLETE, this.onResourceLoadComplete, this), RES.removeEventListener(RES.ResourceEvent.GROUP_LOAD_ERROR, this.onResourceLoadError, this), RES.removeEventListener(RES.ResourceEvent.GROUP_PROGRESS, this.onResourceProgress, this), RES.removeEventListener(RES.ResourceEvent.ITEM_LOAD_ERROR, this.onItemLoadError, this), this.isResourceLoadEnd = !0, this.createScene())
        },
        n.createScene = function () {
            this.isThemeLoadEnd && this.isResourceLoadEnd && this.startCreateScene()
        },
        n.onItemLoadError = function (e) {
            console.warn("Url:" + e.resItem.url + " has failed to load")
        },
        n.onResourceLoadError = function (e) {
            console.warn("Group:" + e.groupName + " has failed to load"),
                this.onResourceLoadComplete(e)
        },
        n.onResourceProgress = function (e) {
            "preload" == e.groupName && this.loadingView.setProgress(e.itemsLoaded, e.itemsTotal)
        },
        n.startCreateScene = function () {
            var e = new GamePlaying;
            this.addChild(e)
        },
        n.onButtonClick = function (e) {
            var t = new eui.Panel;
            t.title = "Title",
                t.horizontalCenter = 0,
                t.verticalCenter = 0,
                this.addChild(t)
        },
        t
}(eui.UILayer);
egret.registerClass(Main, "Main");
var ThemeAdapter = function () {
    function e() { }
    var t = (__define, e),
        i = t.prototype;
    return i.getTheme = function (e, t, i, n) {
        function a(e) {
            t.call(n, e)
        }
        function r(t) {
            t.resItem.url == e && (RES.removeEventListener(RES.ResourceEvent.ITEM_LOAD_ERROR, r, null), i.call(n))
        }
        RES.addEventListener(RES.ResourceEvent.ITEM_LOAD_ERROR, r, null),
            RES.getResByUrl(e, a, this, RES.ResourceItem.TYPE_TEXT)
    },
        e
}();
egret.registerClass(ThemeAdapter, "ThemeAdapter", ["eui.IThemeAdapter"]);
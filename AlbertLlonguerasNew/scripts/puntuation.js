
function PrepareObject(json) {
    for (var i = 0; i < json.length; i++) {
        puntuations.push([json[i].Information.OldInformation.GlobalPuntuation, json[i].PlayerName, '#7D252B']);
    }
    $('#puntuationGraph').jqBarGraph({ data: puntuations });
}

function PreparePuntuationOfMonth(json) {
    for (var i = 0; i < json.length; i++) {
        puntuations.push([json[i].Information.OldInformation.MonthPuntuation, json[i].PlayerName, '#7D252B']);
    }
}

function PreparePuntuationOfGlobalMonth(json) {
    for (var i = 0; i < json.length; i++) {
        var values = new Array();
        for (var j = 0; j < json[i].Puntuation.length; j++) {
            values.push([json[i].Puntuation[j]]);
        }
        puntuations.push([json[i].Puntuation, json[i].Name]);
    }
}

function PreparePuntuationOfLastGame(json) {
    for (var i = 0; i < json.length; i++) {
        puntuations.push([json[i].Information.OldInformation.LastScore, json[i].PlayerName, '#7D252B']);
    }
}

function InitializeHtml() {
    $('#puntuationGraph').html('');
}

function InitializeGraph(puntuations) {
    $('#puntuationGraph').jqBarGraph({
        data: puntuations,
        colors: ['#d2b48c', '#cd853f', '#a0522d']
    });
}

function LoadInitialGraph(json) {
    InitializeHtml();
    PrepareObject(json);
    InitializeGraph();
}

function SortByPuntuationAsc(puntuations) {
    return puntuations.sort(function (a, b) {
        if (a[0] < b[0])
            return -1;
        if (a[0] > b[0])
            return 1;
        return 0;
    });
}

function SortByPuntuationDesc(puntuations) {
    return puntuations.sort(function (a, b) {
        if (a[0] > b[0])
            return -1;
        if (a[0] < b[0])
            return 1;
        return 0;
    });
}

function SortByPuntuationMonthDesc(puntuations) {
    return puntuations.sort(function (a, b) {
        var sumaA = 0;
        var sumaB = 0;
        for (var i = 0; i < a.length; i++) {
            sumaA = sumaA + a[0][i];
            sumaB = sumaB + b[0][i];
        }
        if (sumaA > sumaB)
            return -1;
        if (sumaA < sumaB)
            return 1;
        return 0;
    });
}

function InitializeArray() {
    puntuations = new Array();
}

function Initialize() {
    InitializeHtml();
    InitializeArray();
}

function GlobalWithPorrero() {
    Initialize();
    PreparePuntuationOfGlobalMonth(GlobalMonthPorreroPuntuation);
    var jsonOrdered = SortByPuntuationMonthDesc(puntuations);
    InitializeGraph(jsonOrdered);
}

function GlobalWithoutPorrero() {
    Initialize();
    PreparePuntuationOfGlobalMonth(GlobalMonthPuntuation);
    var jsonOrdered = SortByPuntuationMonthDesc(puntuations);
    InitializeGraph(jsonOrdered);
}

function CurrentMonth() {
    Initialize();
    PreparePuntuationOfMonth(GlobalPuntuation);
    var jsonOrdered = SortByPuntuationDesc(puntuations);
    InitializeGraph(jsonOrdered);
}

function LastPuntuation() {
    Initialize();
    PreparePuntuationOfLastGame(GlobalPuntuation);
    var jsonOrdered = SortByPuntuationDesc(puntuations);
    InitializeGraph(jsonOrdered);
}
$(document).ready(function () {
    $('.sortDesc').click(function () {
        GlobalWithPorrero();
    });
    $('.globalMonth').click(function () {
        $(".puntuationSelect .active").removeClass("active");
        GlobalWithoutPorrero();
    });
    $('.porrero').click(function () {
        $(".puntuationSelect .active").removeClass("active");
        CurrentMonth();
    });
    $('.lastPuntuation').click(function () {
        $(".puntuationSelect .active").removeClass("active");
        LastPuntuation();
    });
});
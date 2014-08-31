
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

function InitializeHtml() {
    $('#puntuationGraph').html('');
}

function InitializeGraph(puntuations) {
    $('#puntuationGraph').jqBarGraph({ data: puntuations });
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

function InitializeArray() {
    puntuations = new Array();
}

$(document).ready(function () {
    $('.boto3d.sortAsc').click(function () {
        InitializeArray();
        PrepareObject(GlobalPuntuation);
        var jsonOrdered = SortByPuntuationAsc(puntuations);
        InitializeHtml();
        InitializeGraph(jsonOrdered);
    });
    $('.boto3d.sortDesc').click(function () {
        InitializeArray();
        PrepareObject(GlobalPuntuation);
        var jsonOrdered = SortByPuntuationDesc(puntuations);
        InitializeHtml();
        InitializeGraph(jsonOrdered);
    });
    $('.boto3d.porrero').click(function () {
        InitializeHtml();
        InitializeArray();
        PreparePuntuationOfMonth(GlobalPuntuation);
        var jsonOrdered = SortByPuntuationDesc(puntuations);
        InitializeGraph(jsonOrdered);
    });
});
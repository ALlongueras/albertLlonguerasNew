
function PrepareObject(json) {
    for (var i = 0; i < json.length; i++) {
        puntuations.push([json[i].Information.OldInformation.GlobalPuntuation, json[i].PlayerName, '#7D252B']);
    }
    $('#puntuationGraph').jqBarGraph({ data: puntuations });
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

function SortByPuntuationAsc() {
    return puntuations.sort(function (a, b) {
        if (a[0] < b[0])
            return -1;
        if (a[0] > b[0])
            return 1;
        return 0;
    });
}

function SortByPuntuationDesc() {
    return puntuations.sort(function (a, b) {
        if (a[0] > b[0])
            return -1;
        if (a[0] < b[0])
            return 1;
        return 0;
    });
}

$(document).ready(function () {
    $('.boto3d.sortAsc').click(function () {
        var jsonOrdered = SortByPuntuationAsc();
        InitializeHtml();
        InitializeGraph(jsonOrdered);
    });
    $('.boto3d.sortDesc').click(function () {
        var jsonOrdered = SortByPuntuationDesc();
        InitializeHtml();
        InitializeGraph(jsonOrdered);
    });
});
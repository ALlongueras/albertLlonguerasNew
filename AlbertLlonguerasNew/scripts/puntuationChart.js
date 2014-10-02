function InitializeChart(json) {
    var jsonChart = JSON.parse(json);
    var dataset = new Array();
    for (var i = 0; i < jsonChart.Datasets.length; i++) {
        var datasetJson = jsonChart.Datasets[i];
        var values = datasetJson.Data.split(',');
        var dataValues = new Array();
        for (var j = 0; j < values.length; j++) {
            dataValues[j] = parseInt(values[j]);
        }
        datasets: dataset.push({
            label: datasetJson.Label,
            fillColor: datasetJson.FillColor,
            strokeColor: datasetJson.StrokeColor,
            pointColor: datasetJson.PointColor,
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: datasetJson.HighlightStroke,
            data: dataValues
        });
    }
    var labelsArray = new Array();
    var labelsSplit = jsonChart.Labels.split(',');
    for (var k = 0; k < labelsSplit.length; k++) {
        labelsArray[k] = labelsSplit[k];
    }
    var data = {
        labels: labelsArray,
        datasets:dataset
    };
    return data;
}

function InitializeForChart() {
    $("#puntuationGraph").addClass("hidden");
    $("#chart_block").removeClass("hidden");
}

globalChart = function GlobalWithChart() {
    InitializeForChart();
    var data = InitializeChart(GlobalPuntuationChartJson);
    //alert(paco);
    var ctx = document.getElementById("myChart").getContext("2d");
    //var options;
    //GlobalPuntuationChartJson = GlobalPuntuationChartJson.toLowerCase();

    var options = {

        responsive: true,

        ///Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - Whether the line is curved between points
        bezierCurve: true,

        //Number - Tension of the bezier curve between points
        bezierCurveTension: 0.4,

        //Boolean - Whether to show a dot for each point
        pointDot: true,

        //Number - Radius of each point dot in pixels
        pointDotRadius: 4,

        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,

        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,

        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,

        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,

        //Boolean - Whether to fill the dataset with a colour
        datasetFill: true,

        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %>",

        //String - A legend template
        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

    };
    var myLineChart = new Chart(ctx).Line(data, options);
    legend(document.getElementById("chart_block"), data);
    //var legend = myLineChart.generateLegend();
    //$('#mychart').append(legend);
}

function legend(parent, data) {
    //parent.className = 'legend';
    var datas = data.hasOwnProperty('datasets') ? data.datasets : data;

    // remove possible children of the parent
    //while (parent.hasChildNodes()) {
    //    parent.removeChild(parent.lastChild);
    //}

    var container = document.createElement('div');
    container.className = 'legendChart col-sm-1';
    parent.appendChild(container);
    datas.forEach(function (d) {
        var title = document.createElement('span');
        title.className = 'title';
        title.style.backgroundColor = d.hasOwnProperty('strokeColor') ? d.strokeColor : d.color;
        title.style.textAlign = 'center';
        //title.style.borderStyle = 'solid';
        parent.children[1].appendChild(title);

        var text = document.createTextNode(d.label);
        title.appendChild(text);
    });
}
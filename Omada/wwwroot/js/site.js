// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
Date.prototype.getWeekNumber = function () {
    var d = new Date(Date.UTC(this.getFullYear(), this.getMonth(), this.getDate() + 1));
    var dayNum = d.getUTCDay() || 7;
    d.setUTCDate(d.getUTCDate() + 4 - dayNum);
    var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
    return Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
};        

var colors = function () {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
};

var today = new Date();
var weeks = [];
var startWeek = today.getWeekNumber() - 5;

var omadaChart = document.getElementById("chart").getContext("2d");
var myDatasets = [];

function createChart() {
    weeks.splice(0, weeks.length);
    myDatasets.splice(0, myDatasets.length);
    for (var i = 1; i <= today.getWeekNumber(); i++) {
        weeks.push(i);
    }
    addData();
    new Chart(omadaChart, {
        type: 'line',
        data: {
            labels: weeks,
            datasets: myDatasets
        },
        options: {
            scales: {
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Week'
                    },
                    ticks: {
                        min: startWeek,
                        max: today.getWeekNumber()
                    }
                }],
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Rate'
                    }
                }]
            },
            plugins: {
                zoom: {
                    pan: {
                        enabled: true,
                        mode: 'xy',
                        rangeMin: {
                            x: 1,
                            y: 1
                        },
                        rangeMax: {
                            x: today.getWeekNumber(),
                            y: 5
                        },
                    },
                    zoom: {
                        enabled: true,
                        mode: 'xy',
                        rangeMin: {
                            x: 1,
                            y: 1
                        },
                        rangeMax: {
                            x: today.getWeekNumber(),
                            y: 5
                        },
                        speed: 0.1,
                        sensitivity: 1
                    },
                    responsive: true
                }
            }
        }
    });
}
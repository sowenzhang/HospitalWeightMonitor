(function () {
    'use strict';

    angular
        .module('app.widgets')
        .directive('lineChart', lineChart);

    function lineChart($parse, $window) {
        return {
            restrict: 'EA',
            template: "<svg width='850' height='200'></svg>",
            link: function (scope, elem, attrs) {
                var exp = $parse(attrs.chartData);

                var dataToPlot = exp(scope);
                var padding = 20;
                var pathClass = "path";
                var xScale, yScale, xAxisGen, yAxisGen, lineFun;

                var d3 = $window.d3;
                var rawSvg = elem.find('svg');
                var svg = d3.select(rawSvg[0]);

                scope.$watchCollection(exp, function (newVal, oldVal) {
                    dataToPlot = newVal;
                    redrawLineChart();
                });

                function setChartParameters() {
                    if (dataToPlot.length <= 0) return;

                    xScale = d3.scale.linear()
                        .domain([dataToPlot[0].x, dataToPlot[dataToPlot.length - 1].x])
                        .range([padding + 5, rawSvg.attr("width") - padding]);

                    yScale = d3.scale.linear()
                        .domain([
                            0, d3.max(dataToPlot, function (d) {
                                return d.y;
                            })
                        ])
                        .range([rawSvg.attr("height") - padding, 0]);

                    xAxisGen = d3.svg.axis()
                        .scale(xScale)
                        .orient("bottom")
                        .ticks(dataToPlot.length - 1);

                    yAxisGen = d3.svg.axis()
                        .scale(yScale)
                        .orient("left")
                        .ticks(5);

                    lineFun = d3.svg.line()
                        .x(function (d) {
                            return xScale(d.x);
                        })
                        .y(function (d) {
                            return yScale(d.y);
                        })
                        .interpolate("basis");
                }

                function drawLineChart() {
                    if (dataToPlot.length <= 0) return;

                    setChartParameters();

                    svg.append("svg:g")
                        .attr("class", "x axis")
                        .attr("transform", "translate(0,180)")
                        .call(xAxisGen);

                    svg.append("svg:g")
                        .attr("class", "y axis")
                        .attr("transform", "translate(20,0)")
                        .call(yAxisGen);

                    svg.append("svg:path")
                        .attr({
                            d: lineFun(dataToPlot),
                            "stroke": "blue",
                            "stroke-width": 2,
                            "fill": "none",
                            "class": pathClass
                        });
                }

                function redrawLineChart() {
                    if(dataToPlot.length <= 0) return;

                    setChartParameters();

                    svg.selectAll("g.y.axis").call(yAxisGen);

                    svg.selectAll("g.x.axis").call(xAxisGen);

                    svg.selectAll("." + pathClass)
                        .attr({
                            d: lineFun(dataToPlot)
                        });
                }

                drawLineChart();
            }
        };
    }

})();
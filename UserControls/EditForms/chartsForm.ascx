<%@ Control Language="C#" AutoEventWireup="true" CodeFile="chartsForm.ascx.cs" Inherits="UserControls_EditForms_chartsForm" %>
<script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.js"></script>
    <script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/themes/fusioncharts.theme.fusion.js"></script>
    <script type="text/javascript">
        FusionCharts.ready(function () {
            var revenueChart = new FusionCharts({
                type: 'pareto3d',
                renderAt: 'chart-container',
                width: '700',
                height: '400',
                dataFormat: 'json',
                dataSource: {
                    "chart": {
                        "theme": "fusion",
                        "caption": "Employee late arrivals by reported cause",
                        "subCaption": "Last month",
                        "xAxisName": "Reported Cause",
                        "pYAxisName": "No. of Occurrence",
                        "sYAxisname": "Cumulative Percentage",
                        "showHoverEffect": "1",
                        "divlineColor": "#999999"
                    },
                    "data": [{
                        "label": "Traffic",
                        "value": "5680"
                    },
                    {
                        "label": "Family Engagement",
                        "value": "1036"
                    },
                    {
                        "label": "Public Transport",
                        "value": "950"
                    },
                    {
                        "label": "Weather",
                        "value": "500"
                    },
                    {
                        "label": "Emergency",
                        "value": "140"
                    },
                    {
                        "label": "Others",
                        "value": "68"
                    }
                    ]
                }
            }).render();
        });

    </script>
<div id="chart-container">FusionCharts will render here</div>
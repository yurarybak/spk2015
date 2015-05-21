using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel; 

namespace Labworks.ExcelAddIn
{
    public partial class EvolutionaryAlgorithmsRibbon
    {
        private void EvolutionaryAlgorithmsRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void TSPCharts_Click(object sender, RibbonControlEventArgs e)
        {
            var wb = Globals.ThisAddIn.Application.ActiveWorkbook;
            var ws = wb.ActiveSheet as Worksheet;
            var rowCount = ws.UsedRange.Rows.Count;
            object misValue = System.Reflection.Missing.Value;

            int rowHeight = 15;

            //ws.UsedRange.Rows.RowHeight = 20;

            int currentRow = 1;
            while (currentRow <= rowCount)
            {
                var value = ws.Range["A" + currentRow].Value;
                if (value == null) break;

                var nameValue = ws.Range["B" + currentRow].Value;

                char entry = ((string)value)[0];
                string name = ((string)nameValue);
                if (entry == 'c' || entry == 'g' || entry == 's')
                {
                    double l = 240;
                    double t = currentRow * rowHeight;
                    double h = 16 * rowHeight;
                    double w = h * 1.5;

                    Excel.ChartObjects charts = (Excel.ChartObjects)ws.ChartObjects(Type.Missing);
                    Excel.ChartObject chart = (Excel.ChartObject)charts.Add(l, t, w, h);
                    Excel.Chart chartPage = chart.Chart;

                    chartPage.ChartType = Excel.XlChartType.xlXYScatterLines;
                    chartPage.ChartStyle = 205;

                    Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
                    Excel.Series series1 = seriesCollection.NewSeries();
                    series1.XValues = ws.Range["C" + (currentRow + 1) + ":C" + (currentRow + 16)];
                    series1.Values = ws.Range["D" + (currentRow + 1) + ":D" + (currentRow + 16)];
                    series1.Name = name + " ";

                    series1.Name += "Distance: " + ((double)ws.Range["C" + currentRow].Value).ToString();
                    series1.Name += ", Fitness: " + ((double)ws.Range["D" + currentRow].Value).ToString();

                    currentRow += 17;
                }

                currentRow++;
            }





        }
    }
}

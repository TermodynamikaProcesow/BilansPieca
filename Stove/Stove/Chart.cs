using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stove
{
    internal class Chart
    {
        public static void DrawVChart(IEnumerable<OutputData> outputDatas, string outputFileName)
        {
            //populate dataset with outputDatas..
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Vg", typeof(int));
            dt.Columns.Add("Vc", typeof(int));
            dt.Columns.Add("Vs", typeof(int));

            foreach (var data in outputDatas)
            {
                DataRow r = dt.NewRow();
                r[0] = data.Vg;
                r[1] = data.VcRz;
                r[2] = data.VsRz;
                dt.Rows.Add(r);
            }

            dataSet.Tables.Add(dt);

            List<Series> series = new List<Series>
            {
                new Series
                {
                    Name = "Vc",
                    Color = Color.FromArgb(100, 0, 100),
                    BorderColor = Color.FromArgb(164, 164, 164),
                    ChartType = SeriesChartType.FastLine,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BorderWidth = 3,
                    ShadowColor = Color.FromArgb(128, 128, 128),
                    ShadowOffset = 1,
                    IsValueShownAsLabel = true,
                    XValueMember = "Vg",
                    YValueMembers = "Vc",
                    Font = new Font("Tahoma", 8.0f),
                    BackSecondaryColor = Color.FromArgb(0, 102, 153),
                    LabelForeColor = Color.FromArgb(100, 100, 100)
                },
                new Series
                {
                    Name = "Vs",
                    Color = Color.FromArgb(0, 100, 0),
                    BorderColor = Color.FromArgb(164, 164, 164),
                    ChartType = SeriesChartType.FastLine,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BorderWidth = 3,
                    ShadowColor = Color.FromArgb(128, 128, 128),
                    ShadowOffset = 1,
                    IsValueShownAsLabel = true,
                    XValueMember = "Vg",
                    YValueMembers = "Vs",
                    Font = new Font("Tahoma", 8.0f),
                    BackSecondaryColor = Color.FromArgb(0, 102, 153),
                    LabelForeColor = Color.FromArgb(100, 100, 100)
                }
            };

            CreateChart(outputFileName, dataSet, series);
        }

        public static void DrawQChart(IEnumerable<OutputData> outputDatas, string outputFileName)
        {
            //populate dataset with outputDatas..
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Vg", typeof(int));
            dt.Columns.Add("Qw", typeof(int));
            dt.Columns.Add("Qs", typeof(int));
            dt.Columns.Add("Qstr", typeof(int));

            foreach (var data in outputDatas)
            {
                DataRow r = dt.NewRow();
                r[0] = data.Vg;
                r[1] = data.Qw;
                r[2] = data.Qs;
                r[3] = data.Qstr;
                dt.Rows.Add(r);
            }

            dataSet.Tables.Add(dt);

            List<Series> series = new List<Series>
            {
                new Series
                {
                    Name = "Qw",
                    Color = Color.FromArgb(100, 0, 100),
                    BorderColor = Color.FromArgb(164, 164, 164),
                    ChartType = SeriesChartType.FastLine,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BorderWidth = 3,
                    ShadowColor = Color.FromArgb(128, 128, 128),
                    ShadowOffset = 1,
                    IsValueShownAsLabel = true,
                    XValueMember = "Vg",
                    YValueMembers = "Qw",
                    Font = new Font("Tahoma", 8.0f),
                    BackSecondaryColor = Color.FromArgb(0, 102, 153),
                    LabelForeColor = Color.FromArgb(100, 100, 100)
                },
                new Series
                {
                    Name = "Qs",
                    Color = Color.FromArgb(0, 100, 0),
                    BorderColor = Color.FromArgb(164, 164, 164),
                    ChartType = SeriesChartType.FastLine,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BorderWidth = 3,
                    ShadowColor = Color.FromArgb(128, 128, 128),
                    ShadowOffset = 1,
                    IsValueShownAsLabel = true,
                    XValueMember = "Vg",
                    YValueMembers = "Qs",
                    Font = new Font("Tahoma", 8.0f),
                    BackSecondaryColor = Color.FromArgb(0, 102, 153),
                    LabelForeColor = Color.FromArgb(100, 100, 100)
                },
                new Series
                {
                    Name = "Qstr",
                    Color = Color.FromArgb(0, 150, 150),
                    BorderColor = Color.FromArgb(164, 164, 164),
                    ChartType = SeriesChartType.FastLine,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BorderWidth = 3,
                    ShadowColor = Color.FromArgb(128, 128, 128),
                    ShadowOffset = 1,
                    IsValueShownAsLabel = true,
                    XValueMember = "Vg",
                    YValueMembers = "Qstr",
                    Font = new Font("Tahoma", 8.0f),
                    BackSecondaryColor = Color.FromArgb(0, 102, 153),
                    LabelForeColor = Color.FromArgb(100, 100, 100)
                }
            };

            CreateChart(outputFileName, dataSet, series);
        }
        
        private static void CreateChart(string outputFileName, DataSet dataSet, List<Series> series)
        {
            //prepare chart control...
            var chart = new System.Windows.Forms.DataVisualization.Charting.Chart
            {
                DataSource = dataSet.Tables[0],
                Width = 1280,
                Height = 720
            };

            foreach (var serie in series)
                chart.Series.Add(serie);

            //create chartareas...
            chart.ChartAreas.Add(new ChartArea
            {
                Name = "ChartArea1",
                BackColor = Color.White,
                BorderColor = Color.FromArgb(26, 59, 105),
                BorderWidth = 0,
                BorderDashStyle = ChartDashStyle.Solid,
                AxisX = new Axis
                {
                    Title = "Vg [Nm3/h]",
                    TitleFont = new Font("Tahoma", 12.0f),
                    Minimum = 3,
                    Maximum = 9
                },
                AxisY = new Axis
                {
                    Title = "V [m3/h]",
                    TitleFont = new Font("Tahoma", 12.0f),
                }
            });

            chart.Legends.Add(new Legend("Legend") { Font = new Font("Tahoma", 12.0f) });

            //databind...
            chart.DataBind();

            //save result...
            chart.SaveImage(outputFileName, ChartImageFormat.Png);

            //open result...
            ProcessStartInfo psi = new ProcessStartInfo(outputFileName) { UseShellExecute = true };
            Process.Start(psi);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stove
{
    internal class Program
    {
        public static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;

        private static void Main()
        {
            try
            {
                InputData inputData1 = FileStream.Read("input.dat");
                inputData1.DisplayData();

                List<OutputData> outputDatas = new List<OutputData>();
//
//                Console.WriteLine("\nPress any key to estimate output1.dat...");
//                Console.ReadKey();

                OutputData outputData1 = Balance.Estimate(inputData1);
                FileStream.Write(outputData1, "output1.dat");
                outputDatas.Add(outputData1);
//                outputData1.DisplayData();
//                
//                Console.WriteLine("\nPress any key to estimate output2.dat...");
//                Console.ReadKey();

                OutputData outputData2 = Balance.Estimate(new InputData(inputData1, 2));
                FileStream.Write(outputData2, "output2.dat");
                outputDatas.Add(outputData2);
//                outputData2.DisplayData();
//                
//                Console.WriteLine("\nPress any key to estimate output3.dat...");
//                Console.ReadKey();

                OutputData outputData3 = Balance.Estimate(new InputData(inputData1, 3));
                FileStream.Write(outputData3, "output3.dat");
                outputDatas.Add(outputData3);
//                outputData3.DisplayData();
//
//                Console.WriteLine("\nPress any key to draw chart...");
//                Console.ReadKey();

                DrawChart(outputDatas);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void DrawChart(IEnumerable<OutputData> outputDatas)
        {
            //populate dataset with outputDatas..
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Vg", typeof(string));
            dt.Columns.Add("Vc", typeof(int));
            dt.Columns.Add("Vs", typeof(int));

            foreach (var data in outputDatas)
            {
                DataRow r = dt.NewRow();
                r[0] = data.Vg.ToString(CultureInfo);
                r[1] = data.Vc;
                r[2] = data.Vs;
                dt.Rows.Add(r);
            }

            dataSet.Tables.Add(dt);

            //prepare chart control...
            Chart chart = new Chart
            {
                DataSource = dataSet.Tables[0],
                Width = 1280,
                Height = 720
            };

            //create serie Vc...
            chart.Series.Add(new Series
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
            });

            //create serie Vs...
            chart.Series.Add(new Series
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
            });

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
                    TitleFont = new Font("Tahoma", 12.0f)
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
            chart.SaveImage("chart.png", ChartImageFormat.Png);

            //open result...
            ProcessStartInfo psi = new ProcessStartInfo("chart.png") { UseShellExecute = true };
            Process.Start(psi);
        }
    }
}
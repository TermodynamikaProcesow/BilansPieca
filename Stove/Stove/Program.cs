using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stove
{
    internal class Program
    {
        public static readonly CultureInfo CultureInfo = new CultureInfo("en");

        static void Main()
        {
            try
            {
                InputData inputData = FileStream.Read("input.dat");
                OutputData outputData = Balance.Estimate(inputData);
                FileStream.Write(outputData, "output.dat");

                DrawChart();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void DrawChart()
        {
            //populate dataset with some demo data..
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Counter", typeof(int));
            DataRow r1 = dt.NewRow();
            r1[0] = "Demo";
            r1[1] = 8;
            dt.Rows.Add(r1);
            DataRow r2 = dt.NewRow();
            r2[0] = "Second";
            r2[1] = 15;
            dt.Rows.Add(r2);
            dataSet.Tables.Add(dt);


            //prepare chart control...
            Chart chart = new Chart();
            chart.DataSource = dataSet.Tables[0];
            chart.Width = 600;
            chart.Height = 350;
            //create serie...
            Series serie1 = new Series();
            serie1.Name = "Serie1";
            serie1.Color = Color.FromArgb(112, 255, 200);
            serie1.BorderColor = Color.FromArgb(164, 164, 164);
            serie1.ChartType = SeriesChartType.Column;
            serie1.BorderDashStyle = ChartDashStyle.Solid;
            serie1.BorderWidth = 1;
            serie1.ShadowColor = Color.FromArgb(128, 128, 128);
            serie1.ShadowOffset = 1;
            serie1.IsValueShownAsLabel = true;
            serie1.XValueMember = "Name";
            serie1.YValueMembers = "Counter";
            serie1.Font = new Font("Tahoma", 8.0f);
            serie1.BackSecondaryColor = Color.FromArgb(0, 102, 153);
            serie1.LabelForeColor = Color.FromArgb(100, 100, 100);
            chart.Series.Add(serie1);
            //create chartareas...
            ChartArea ca = new ChartArea();
            ca.Name = "ChartArea1";
            ca.BackColor = Color.White;
            ca.BorderColor = Color.FromArgb(26, 59, 105);
            ca.BorderWidth = 0;
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.AxisX = new Axis();
            ca.AxisY = new Axis();
            chart.ChartAreas.Add(ca);
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
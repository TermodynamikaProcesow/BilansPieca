using System;
using System.Collections.Generic;
using System.Globalization;

namespace Stove
{
    internal class Program
    {
        public static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;

        private static void Main()
        {
            try
            {
                Console.WriteLine("input.dat");
                InputData inputData = FileStream.Read("input.dat");
                inputData.DisplayData();

                List<OutputData> outputDatas = new List<OutputData>();

                for (int i = 3; i < 10; i++)
                {
                    OutputData outputData = Balance.Estimate(new InputData(inputData, i));
                    outputDatas.Add(outputData);

                    if (i != 3 && i != 9) continue;
                    FileStream.Write(outputData, $"output{i / 3}.dat");
                    Console.WriteLine($"\noutput{i / 3}.dat");
                    outputData.DisplayData();
                }

                Chart.DrawVChart(outputDatas, "chartV.png");
                Chart.DrawQChart(outputDatas, "chartQ.png");
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
    }
}
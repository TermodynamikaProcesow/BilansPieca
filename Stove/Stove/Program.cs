using System;
using System.Globalization;

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
    }
}
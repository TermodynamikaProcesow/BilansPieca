using System;

namespace Stove
{
    internal class Balance
    {
        private const double Qco = 12610.576;
        private const double Qch4 = 35856.88;
        private const double Qh2 = 10752.88;
        private const double TemperatureReference = 273.15;
        private const double PressureNormal = 101325;

        public static OutputData Estimate(InputData input)
        {
            var ci = Program.CultureInfo;
            input.DisplayData();

            double theoreticalOxygen = CountTheoreticalOxygen(input.CO, input.CH4, input.H2, input.O2);
            Console.WriteLine(string.Format(ci, "{0} tlen teoretyczny", theoreticalOxygen));

            double totalOxygen = CountTotalOxygen(theoreticalOxygen, input.lambda);
            Console.WriteLine(string.Format(ci, "{0} tlen całkowity", totalOxygen));

            double theoreticalAir = CountTheoreticalAir(theoreticalOxygen);
            Console.WriteLine(string.Format(ci, "{0} powietrze teoretyczne", theoreticalAir));

            return new OutputData();
        }

        private static double CountTheoreticalOxygen(double CO, double CH4, double H2, double O2)
        {
            return 0.5 * CO / 100 + 2 * CH4 / 100 + 0.5 * H2 / 100 - O2 / 100;
        }

        private static double CountTotalOxygen(double theoreticalOxygen, double lambda)
        {
            return lambda * theoreticalOxygen;
        }

        private static double CountTheoreticalAir(double theoreticalOxygen)
        {
            return theoreticalOxygen * 100 / 21;
        }

        private static double CountTotalAir()
        {
            return 1;
        }


        private static double CountVPrims()
        {
            return 1;
        }

        private static double CountVgrz()
        {
            return 1;
        }

        private static double CountVc()
        {
            return 1;
        }

        private static double CountVcrz()
        {
            return 1;
        }

        private static double CountVs()
        {
            return 1;
        }

        private static double CountVsrz()
        {
            return 1;
        }

        private static double CountQ_q()
        {
            return 1;
        }

        //funkcje z pliku z wytycznymi
        private static double hs_H2Ol(double T)
        {
            return 1.0 / 18.015 * (2.348e3 * (T - TemperatureReference) - 4.548 * 0.5 * (T * T - TemperatureReference * TemperatureReference) + 3.437e-3 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference));
        }

        private static double hs_H2O(double T)
        {
            return 1.0 / 22.42 * (4.184 * (8.22 * (T - TemperatureReference) + 0.00015 * 0.5 * (T * T - TemperatureReference * TemperatureReference) - 0.00000134 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference)));
        }

        private static double hs_O2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (8.27 * (T - TemperatureReference) + 0.000258 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        private static double hs_CO2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (10.34 * (T - TemperatureReference) + 0.00274 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        private static double hs_CO(double T)
        {
            return 1.0 / 22.42 * (4.184 * (6.6 * (T - TemperatureReference) + 0.0012 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        private static double hs_H2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (6.62 * (T - TemperatureReference) + 0.00081 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        private static double hs_N2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        private static double hs_CH4(double T)
        {
            return 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }
    }
}
using System;

namespace Stove
{
    class Balance
    {

        public static double Qco = 12610.576;
        public static double Qch4 = 35856.88;
        public static double Qh2 = 10752.88;
        public static double TemperatureReference = 273.15;
        public static double PressureNormal = 101325;

        public static OutputData Estimate(InputData input)
        {
            DisplayData(input);

            double TheoreticalOxygen = CountTheoreticalOxygen(input.CO, input.CH4, input.H2, input.O2);
            Console.WriteLine(TheoreticalOxygen + " tlen teoretyczny");

            double TotalOxygen = CountTotalOxygen(TheoreticalOxygen, input.lambda);
            Console.WriteLine(TotalOxygen + " tlen całkowity");

            double TheoreticalAir = CountTheoreticalAir(TheoreticalOxygen);
            Console.WriteLine(TheoreticalAir + " powietrze teoretyczne");
            Console.ReadKey();

            return new OutputData();
        }

        public static void DisplayData(InputData input)
        {
            Console.WriteLine(input.CO + " % -> zawartość CO w paliwie");
            Console.WriteLine(input.CO2 + " % -> zawartość CO2 w paliwie");
            Console.WriteLine(input.CH4 + " % -> zawartość CH4 w paliwie");
            Console.WriteLine(input.O2 + " % -> zawartość O2 w paliwie");
            Console.WriteLine(input.N2 + " % -> zawartość N2 w paliwie");
            Console.WriteLine(input.H2 + " % -> zawartość H2 w paliwie");
            Console.WriteLine(input.lambda + " -> współczynnik nadmiaru powietrza");
            Console.WriteLine(input.V + " Nm/h -> objętościowy strumień paliwa");
            Console.WriteLine(input.tg + " st. C -> temperatura paliwa");
            Console.WriteLine(input.tp + " st. C -> temperatura powietrza");
            Console.WriteLine(input.twIn + " st. C -> temperatura wody przed piecem");
            Console.WriteLine(input.mw + " kg/h -> strumień masowy wody w wymienniku pieca");
            Console.WriteLine(input.A + " m2 -> powierzchnia wymiennika");
            Console.WriteLine(input.alfa + " -> współczynnik przejmowania ciepła wymiennika");
            Console.WriteLine(input.beta + " -> współczynnik podziału ciepła do spalin i otoczenia");
        }

        public static double CountTheoreticalOxygen(double CO, double CH4, double H2, double O2)
        {
            return 0.5 * CO / 100 + 2 * CH4 / 100 + 0.5 * H2 / 100 - O2 / 100;
        }

        public static double CountTotalOxygen(double TheoreticalOxygen, double lambda)
        {
            return lambda * TheoreticalOxygen;
        }

        public static double CountTheoreticalAir(double TheoreticalOxygen)
        {
            return TheoreticalOxygen * 100 / 21;
        }

        public static double CountTotalAir()
        {
            return 1;
        }


        public static double CountVPrims()
        {
            return 1;
        }

        public static double CountVgrz()
        {
            return 1;
        }

        public static double CountVc()
        {
            return 1;
        }

        public static double CountVcrz()
        {
            return 1;
        }

        public static double CountVs()
        {
            return 1;
        }

        public static double CountVsrz()
        {
            return 1;
        }

        public static double CountQ_q()
        {
            return 1;
        }

        //funkcje z pliku z wytycznymi
        public static double hs_H2Ol(double T)
        {
            return 1.0 / 18.015 * (2.348e3 * (T - TemperatureReference) - 4.548 * 0.5 * (T * T - TemperatureReference * TemperatureReference) + 3.437e-3 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference));
        }

        public static double hs_H2O(double T)
        {
            return 1.0 / 22.42 * (4.184 * (8.22 * (T - TemperatureReference) + 0.00015 * 0.5 * (T * T - TemperatureReference * TemperatureReference) - 0.00000134 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference)));
        }

        public static double hs_O2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (8.27 * (T - TemperatureReference) + 0.000258 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        public static double hs_CO2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (10.34 * (T - TemperatureReference) + 0.00274 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        public static double hs_CO(double T)
        {
            return 1.0 / 22.42 * (4.184 * (6.6 * (T - TemperatureReference) + 0.0012 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        public static double hs_H2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (6.62 * (T - TemperatureReference) + 0.00081 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        public static double hs_N2(double T)
        {
            return 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }

        public static double hs_CH4(double T)
        {
            return 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        }
    }
}

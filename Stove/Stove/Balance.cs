using System;

namespace Stove
{
    internal class Balance
    {
        //zmienne stałe
        private const double Qco = 12610.576;
        private const double Qch4 = 35856.88;
        private const double Qh2 = 10752.88;
        private const double TemperatureReference = 273.15;
        private const double PressureNormal = 101325;
        private const double ksi = 0;

        public static OutputData Estimate(InputData input)
        {
            var ci = Program.CultureInfo;
            var ic = System.Globalization.CultureInfo.InvariantCulture;
            input.DisplayData();

            // double theoreticalOxygen = CountTheoreticalOxygen(input.CO, input.CH4, input.H2, input.O2);
            // Console.WriteLine(string.Format(ci, "{0} tlen teoretyczny", theoreticalOxygen));

            // double totalOxygen = CountTotalOxygen(theoreticalOxygen, input.lambda);
            //  Console.WriteLine(string.Format(ci, "{0} tlen całkowity", totalOxygen));

            //   double theoreticalAir = CountTheoreticalAir(theoreticalOxygen);
            // Console.WriteLine(string.Format(ci, "{0} powietrze teoretyczne", theoreticalAir));
            Console.WriteLine("\n Wyniki obliczeń: \n");
            //tlen teoretyczny
            double Ot = countOt(input.CO, input.CH4, input.H2, input.O2);
            Console.WriteLine("Tlen teoretyczny: {0} [m3 O2 / m3 paliwa]", Ot.ToString("F5", ic));
            //tlen całkowity
            double Oc = countOc(Ot, input.lambda);
            Console.WriteLine("Tlen całkowity: {0} [m3 O2 / m3 paliwa]", Oc.ToString("F5", ic));
            //powietrze teoretyczne
            double Vo = countVo(Ot);
            Console.WriteLine("Powietrze teoretyczne: {0} [m3 powietrza / m3 paliwa]", Vo.ToString("F5", ic));
            //powietrze całkowite
            double Vc = countVc(input.lambda, Vo);
            Console.WriteLine("Powietrze całkowite: {0} [m3 powietrza / m3 paliwa]\n", Vc.ToString("F5", ic));
            //strumien objętości gazu w warunkach rzeczywistych
            double Vg_wrz = countVg_wrz(input.tg, input.V);
            Console.WriteLine("Objętościowy strumień paliwa: ");
            Console.WriteLine("Warunki normalne: {0} Nm3/h", input.V.ToString("F5", ic));
            Console.WriteLine("Warunki rzeczywiste: {0} m3/h\n", Vg_wrz.ToString("F5", ic));

            //ilość i skład spalin
            double V_CO2 = countV_CO2(input.CO, input.CO2, input.CH4);
            double V_H2O = countV_H2O(input.H2, input.CH4, ksi, Vg_wrz);
            double V_N2 = countV_N2(input.N2, Vc);
            double V_O2 = countV_O2(Oc, Ot);
            double VSprim = countVSprim(V_CO2, V_H2O, V_N2, V_O2);
            Console.WriteLine("Spaliny wilgotne Vs': {0} [m3 spalin / m3 paliwa]", VSprim.ToString("F5", ic));
            double V_CO2sp = countV_PercentSp(V_CO2, VSprim);
            double V_H2Osp = countV_PercentSp(V_H2O, VSprim);
            double V_N2sp = countV_PercentSp(V_N2, VSprim);
            double V_O2sp = countV_PercentSp(V_O2, VSprim);
            Console.WriteLine("Zawartość CO2 w spalinach: {0} %", V_CO2sp.ToString("F5", ic));
            Console.WriteLine("Zawartość H2O w spalinach: {0} %", V_H2Osp.ToString("F5", ic));
            Console.WriteLine("Zawartość N2 w spalinach: {0} %", V_N2sp.ToString("F5", ic));
            Console.WriteLine("Zawartość O2 w spalinach: {0} %\n", V_O2sp.ToString("F5", ic));

            //strumień objętości powietrza
            double Vc_wn = countVc_wn(Vc, input.V);
            double Vc_wrz = countVc_wrz(input.tp, Vc_wn);
            Console.WriteLine("Objętościowy strumień powietrza: ");
            Console.WriteLine("Warunki normalne: {0} Nm3/h", Vc_wn.ToString("F5", ic));
            Console.WriteLine("Warunki rzeczywiste: {0} m3/h", Vc_wrz.ToString("F5", ic));

            //strumień objętości spalin
            //double VolumeS = CountVs(input.V, Vc_wn);



            //zapis do pliku
            OutputData output = new OutputData();
            output.CO2 = Math.Round(V_CO2sp, 2);
            output.H2O = Math.Round(V_H2Osp, 2);
            output.N2 = Math.Round(V_N2sp, 2);
            output.O2 = Math.Round(V_O2sp, 2);
            output.Vg = Math.Round(input.V, 2);
            output.Vc = Math.Round(Vc_wn, 2);
            output.VcRz = Math.Round(Vc_wrz, 2);
            output.Vs = Math.Round(0.0, 2);
            output.VsRz = Math.Round(0.0, 2);
            output.tw = Math.Round(0.0, 2);
            output.ta = Math.Round(0.0, 2);
            output.ts = Math.Round(0.0, 2);
            output.Qq = Math.Round(0.0, 2);
            output.Qp = Math.Round(0.0, 2);
            output.Qw = Math.Round(0.0, 2);
            output.Qstr = Math.Round(0.0, 2);
            output.eta = Math.Round(0.0, 2);
            Console.ReadKey();

            return output;
        }

        //obliczenie ilości tlenu teoretycznego
        private static double countOt(double CO, double CH4, double H2, double O2)
        {
            return 0.5 * CO / 100 + 2 * CH4 / 100 + 0.5 * H2 / 100 - O2 / 100;
        }

        //obliczenie ilości tlenu całkowitego
        private static double countOc(double TheoreticalOxygen, double lambda)
        {
            return lambda * TheoreticalOxygen;
        }

        //obliczenie ilości powietrza teoretycznego
        private static double countVo(double Ot)
        {
            return Ot * 100 / 21;
        }
        //obliczenie ilości powietrza całkowitego
        private static double countVc(double lambda, double Vo)
        {
            return lambda * Vo;
        }
        //obliczenie ilości gazu w warunkach rzeczywistych
        private static double countVg_wrz(double TemperatureGas, double VolumeGas)
        {
            return VolumeGas * (TemperatureGas + TemperatureReference) / TemperatureReference;
        }

        //funkcje do obliczenia ilości i składu spalin
        private static double countV_CO2(double procentCO, double procentCO2, double procentCH4)
        {
            return ((procentCO / 100) + (procentCO2 / 100) + (procentCH4 / 100));
        }
        private static double countV_H2O(double procentH2, double procentCH4, double ksi, double VolumeGasReal)
        {
            return ((procentH2 / 100) + (2 * procentCH4 / 100) + (0.0016 * ksi * VolumeGasReal));
        }

        private static double countV_N2(double procentN2, double V)
        {
            return ((procentN2 / 100) + (V * 79 / 100));
        }

        private static double countV_O2(double Oc, double Ot)
        {
            return Oc - Ot;
        }
        private static double countVSprim(double V_CO2, double V_H2O, double V_N2, double V_O2)
        {
            return V_CO2 + V_H2O + V_N2 + V_O2;
        }

        private static double countV_PercentSp(double V_Component, double VSprim)
        {
            return V_Component / VSprim * 100;
        }

        //strumień objętości powietrza w warunkach normalnych
        private static double countVc_wn(double Vc, double Vg)
        {
            return Vc * Vg;
        }
        //strumień objętości powietrza w warunkach rzeczywistych
        private static double countVc_wrz(double tp, double Vc_wn)
        {
            return Vc_wn * (tp + TemperatureReference) / TemperatureReference;
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
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
            
//            Console.WriteLine("\nWyniki obliczeń: \n");
            //tlen teoretyczny
            double Ot = countOt(input.CO, input.CH4, input.H2, input.O2);
//            Console.WriteLine("Tlen teoretyczny: {0} [m3 O2 / m3 paliwa]", Ot.ToString("F5", ci));
            //tlen całkowity
            double Oc = countOc(Ot, input.lambda);
//            Console.WriteLine("Tlen całkowity: {0} [m3 O2 / m3 paliwa]", Oc.ToString("F5", ci));
            //powietrze teoretyczne
            double Vo = countVo(Ot);
//            Console.WriteLine("Powietrze teoretyczne: {0} [m3 powietrza / m3 paliwa]", Vo.ToString("F5", ci));
            //powietrze całkowite
            double Vc = countVc(input.lambda, Vo);
//            Console.WriteLine("Powietrze całkowite: {0} [m3 powietrza / m3 paliwa]\n", Vc.ToString("F5", ci));
            //strumien objętości gazu w warunkach rzeczywistych
            double Vg_wrz = countVg_wrz(input.tg, input.Vg);
//            Console.WriteLine("Objętościowy strumień paliwa: ");
//            Console.WriteLine("Warunki normalne: {0} Nm3/h", input.Vg.ToString("F5", ci));
//            Console.WriteLine("Warunki rzeczywiste: {0} m3/h\n", Vg_wrz.ToString("F5", ci));

            //ilość i skład spalin
            double V_CO2 = countV_CO2(input.CO, input.CO2, input.CH4);
            double V_H2O = countV_H2O(input.H2, input.CH4, ksi, Vg_wrz);
            double V_N2 = countV_N2(input.N2, Vc);
            double V_O2 = countV_O2(Oc, Ot);
            double VSprim = countVSprim(V_CO2, V_H2O, V_N2, V_O2);
//            Console.WriteLine("Spaliny wilgotne Vs': {0} [m3 spalin / m3 paliwa]", VSprim.ToString("F5", ci));
            double V_CO2sp = countV_PercentSp(V_CO2, VSprim);
            double V_H2Osp = countV_PercentSp(V_H2O, VSprim);
            double V_N2sp = countV_PercentSp(V_N2, VSprim);
            double V_O2sp = countV_PercentSp(V_O2, VSprim);
//            Console.WriteLine("Zawartość CO2 w spalinach: {0} %", V_CO2sp.ToString("F5", ci));
//            Console.WriteLine("Zawartość H2O w spalinach: {0} %", V_H2Osp.ToString("F5", ci));
//            Console.WriteLine("Zawartość N2 w spalinach: {0} %", V_N2sp.ToString("F5", ci));
//            Console.WriteLine("Zawartość O2 w spalinach: {0} %\n", V_O2sp.ToString("F5", ci));

            //strumień objętości powietrza
            double Vc_wn = countVc_wn(Vc, input.Vg);
            double Vc_wrz = countVc_wrz(input.tp, Vc_wn);
//            Console.WriteLine("Objętościowy strumień powietrza: ");
//            Console.WriteLine("Warunki normalne: {0} Nm3/h", Vc_wn.ToString("F5", ci));
//            Console.WriteLine("Warunki rzeczywiste: {0} m3/h\n", Vc_wrz.ToString("F5", ci));

            //strumień objętości spalin
            double Tsp = 514.932;
            double Vs_wn = countVs(input.Vg, VSprim);
            double Vs_wrz = countVs_wrz(Tsp, Vs_wn);
//            Console.WriteLine("Objętościowy strumień spalin: ");
//            Console.WriteLine("Warunki normalne: {0} Nm3/h", Vs_wn.ToString("F5", ci));
//            Console.WriteLine("Warunki rzeczywiste: {0} m3/h\n", Vs_wrz.ToString("F5", ci));

            //wartość opałowa gazu
            double Qi = countQi(input.CO, input.CH4, input.H2);
//            Console.WriteLine("Wartość opałowa gazu Qi: {0} kJ/m3", Qi.ToString("F5", ci));
            //entalpia gazu
            double Hs_gas = countHsGas(input.CO, input.CH4, input.H2, input.O2, input.CO2, input.N2, input.tg);
//            Console.WriteLine("Entalpia gazu: {0} kJ/m3", Hs_gas.ToString("F5", ci));
            //strumień ciepła z gazu
            double Qg = countQg(input.Vg, Qi, Hs_gas);
//            Console.WriteLine("Strumień ciepła gazu (Qg): {0} kW\n", (Qg/3600).ToString("F5", ci));
            //strumień ciepła powietrza
            double Qp = countQp(Vc_wn, input.tp);
//            Console.WriteLine("Strumień ciepła powietrza (Qp): {0} kW", (Qp/3600).ToString("F5", ci));
            //temperatura adiabatyczna
            double ta = 1948.272;//countAdiabaticTemperatureOfFlame(Qi, VSprim, V_CO2sp, V_H2Osp, V_N2sp, V_O2sp, Qg, Qp);
//            Console.WriteLine("Adiabatyczna temperatura płomienia: {0} st.C", ta.ToString("F5", ci));
            //strumień ciepła wody
            double Qw = countQw(input.alfa, input.Vg, Vc_wn, ta, input.A, input.twIn);
//            Console.WriteLine("Strumień ciepła wody (Qw): {0} kW", (Qw/3600).ToString("F5", ci));
            //temperatura za piecem
            double tw = countTw(input.mw, Qw, input.twIn, input.alfa, input.Vg, Vc_wn, input.A, ta);
//            Console.WriteLine("Temperatura wody za piecem: {0} st.C", tw.ToString("F5", ci));
            //strumień ciepła spalin
            double Qs = countQs(input.beta, Qg, Qp, Qw);
            //strumień ciepła strat
            double Qstr = countQstr(input.beta, Qg, Qp, Qw);
            //sprawność pieca
            double eta = countEta(Qg, Qw, Qp);
//            Console.WriteLine("Strumień ciepła spalin (Qs): {0} kW", Qs.ToString("F5", ci));
//            Console.WriteLine("Strumień ciepła strat (Qstr): {0} kW", Qstr.ToString("F5", ci));
//            Console.WriteLine("Sprawność pieca: {0}", eta.ToString("F5", ci));
            return new OutputData
            {
                CO2 = Math.Round(V_CO2sp, 3),
                H2O = Math.Round(V_H2Osp, 3),
                N2 = Math.Round(V_N2sp, 3),
                O2 = Math.Round(V_O2sp, 3),
                Vg = Math.Round(input.Vg, 3),
                Vc = Math.Round(Vc_wn, 3),
                VcRz = Math.Round(Vc_wrz, 3),
                Vs = Math.Round(Vs_wn, 3),
                VsRz = Math.Round(Vs_wrz, 3),
                tw = Math.Round(tw, 3),
                ta = Math.Round(ta, 3),
                ts = Math.Round(Tsp, 3),
                Qq = Math.Round((Qg/3600), 3),
                Qp = Math.Round((Qp/3600), 3),
                Qw = Math.Round((Qw/3600), 3),
                Qs = Math.Round((Qs), 3),
                Qstr = Math.Round((Qstr), 3),
                eta = Math.Round(eta, 3)
            };
        }

        /// <summary>
        /// obliczenie ilości tlenu teoretycznego
        /// </summary>
        private static double countOt(double CO, double CH4, double H2, double O2) => 0.5 * CO / 100 + 2 * CH4 / 100 + 0.5 * H2 / 100 - O2 / 100;

        /// <summary>
        /// obliczenie ilości tlenu całkowitego
        /// </summary>
        private static double countOc(double Ot, double lambda) => lambda * Ot;

        /// <summary>
        /// obliczenie ilości powietrza teoretycznego
        /// </summary>
        private static double countVo(double Ot) => Ot * 100 / 21;

        /// <summary>
        /// obliczenie ilości powietrza całkowitego
        /// </summary>
        private static double countVc(double lambda, double Vo) => lambda * Vo;

        /// <summary>
        /// obliczenie ilości gazu w warunkach rzeczywistych
        /// </summary>
        private static double countVg_wrz(double TemperatureGas, double VolumeGas) => VolumeGas * (TemperatureGas + TemperatureReference) / TemperatureReference;

        //funkcje do obliczenia ilości i składu spalin
        private static double countV_CO2(double procentCO, double procentCO2, double procentCH4) => ((procentCO / 100) + (procentCO2 / 100) + (procentCH4 / 100));

        private static double countV_H2O(double procentH2, double procentCH4, double ksi, double VolumeGasReal) => ((procentH2 / 100) + (2 * procentCH4 / 100) + (0.0016 * ksi * VolumeGasReal));

        private static double countV_N2(double procentN2, double V) => ((procentN2 / 100) + (V * 0.79));

        private static double countV_O2(double Oc, double Ot) => Oc - Ot;

        private static double countVSprim(double V_CO2, double V_H2O, double V_N2, double V_O2) => V_CO2 + V_H2O + V_N2 + V_O2;

        private static double countV_PercentSp(double V_Component, double VSprim) => V_Component / VSprim * 100;

        /// <summary>
        /// strumień objętości powietrza w warunkach normalnych
        /// </summary>
        private static double countVc_wn(double Vc, double Vg) => Vc * Vg;

        /// <summary>
        /// strumień objętości powietrza w warunkach rzeczywistych
        /// </summary>
        private static double countVc_wrz(double tp, double Vc_wn) => Vc_wn * (tp + TemperatureReference) / TemperatureReference;

        /// <summary>
        /// strumień objetości spalin w warunkach normalnych
        /// </summary>
        private static double countVs(double Vg, double Vc) => Vg * Vc;

        private static double countVs_wrz(double Tsp, double Vsp) => Vsp * (Tsp + TemperatureReference) / TemperatureReference;

        /// <summary>
        /// wartość opałowa gazu
        /// </summary>
        private static double countQi(double CO_fuel, double CH4_fuel, double H2_fuel) => Qco * (CO_fuel / 100) + Qch4 * (CH4_fuel / 100) + Qh2 * (H2_fuel / 100);

        /// <summary>
        /// entalpia gazu
        /// </summary>
        private static double countHsGas(double CO_fuel, double CH4_fuel, double H2_fuel, double O2_fuel, double CO2_fuel, double N2_fuel, double tg)
        {
            double Tg = tg + TemperatureReference;
            //dodać reszte skladuhujgg
            return hs_CO(Tg) * (CO_fuel / 100) + hs_CH4(Tg) * (CH4_fuel / 100) + hs_H2(Tg) * (H2_fuel / 100) + hs_CO2(Tg)*(CO2_fuel/100)+hs_N2(Tg)*(N2_fuel/100)+hs_O2(Tg)*(O2_fuel/100);
        }

        /// <summary>
        /// strumień ciepła z gazu
        /// </summary>
        private static double countQg(double Vg_wrz, double Qi, double hsGas) => (Vg_wrz * (hsGas + Qi));

        /// <summary>
        /// strumień ciepła powietrza
        /// </summary>
        private static double countQp(double Vc, double tp)
        {
            double Tp = tp + TemperatureReference;
            double Qp = Vc * (0.21 * hs_O2(Tp) + 0.79 * hs_N2(Tp));
            return Qp;
        }

        /// <summary>
        /// adiabatyczna temperatura płomienia
        /// </summary>
        private static double countAdiabaticTemperatureOfFlame(double Qi, double VSprim, double CO2_fumes, double H2O_fumes, double N2_Fumes, double O2_fumes, double Qg, double Qp)
        {
            double ta = 0;
            double hsCO2 = 0, hsH2O = 0, hsN2 = 0, hsO2 = 0;
            double _hsp = 0, _hsp_previous = 0;
            double temp = 0, _temp_previous = 0;
            double hsp = (Qg + Qp)/ VSprim;
            double Tk = 0;
//            Console.WriteLine("hsp: {0}", hsp);
            for(int t=0; t<5000; t++)
            {
                Tk = t + TemperatureReference;
                hsCO2 = hs_CO2(Tk);
                hsH2O = hs_H2O(Tk);
                hsN2 = hs_N2(Tk);
                hsO2 = hs_O2(Tk);
                temp = t;
                //_hsp = ((CO2_fumes *0.01) * hsCO2) + ((H2O_fumes *0.01) * hsH2O) + ((N2_Fumes * 0.01) * hsN2) + ((O2_fumes *0.01) * hsO2);
                _hsp = 0.01 * (hs_CO2(Tk) * CO2_fumes + hs_H2O(Tk) * H2O_fumes + hs_N2(Tk) * N2_Fumes + hs_O2(Tk) * O2_fumes);
                if (_hsp>hsp)
                {
//                    Console.WriteLine("hsCO2: {0}", hsCO2);
//                    Console.WriteLine("hsH2O: {0}", hsH2O);
//                    Console.WriteLine("hsN2: {0}", hsN2);
//                    Console.WriteLine("hsO2: {0}", hsO2);
//                    Console.WriteLine("Temp: {0}", Tk);
                    break;
                }
                _hsp_previous = _hsp;
                _temp_previous = t;
            }
            ta = ((hsp-_hsp_previous)/(_hsp-_hsp_previous))*(temp-_temp_previous)+temp;
            return ta;
        }

        /// <summary>
        /// strumień ciepła wody
        /// </summary>
        private static double countQw(double alfa, double Vg, double Vc, double ta, double A, double twin)
        {
            double VgVc = Vg * Vc;
            return (alfa *( Math.Pow(VgVc, 0.6) * Math.Pow(ta, 0.3) * A * (ta - twin)));
        }

        /// <summary>
        /// temperatura za piecem
        /// </summary>
        private static double countTw(double mw, double Qw, double twin, double alfa, double Vg, double Vc, double A, double ta)
        {
            double tw = (Qw / (mw * hs_H2Ol(293.15))) + twin;
            double hs = (mw*hs_H2Ol(306.15))-(mw*hs_H2Ol(293.15));
//            Console.WriteLine(hs);
            double VgVc = Vg * Vc;
            double result = ((alfa * Math.Pow(VgVc, 0.6) * Math.Pow(ta, 0.3) * A * (ta - twin)) / (mw * hs_H2Ol(293.15) * twin)) + twin;
            return hs;
        }

        /// <summary>
        /// strumień ciepła spalin
        /// </summary>
        private static double countQs(double beta, double Qg, double Qp, double Qw) => ((1 - beta) * (Qg + Qp - Qw)) / 3600;

        /// <summary>
        /// strumień ciepła strat
        /// </summary>
        private static double countQstr(double beta, double Qg, double Qp, double Qw) => (beta * (Qg + Qp - Qw)) / 3600;

        /// <summary>
        /// sprawnosc pieca
        /// </summary>
        private static double countEta(double Qg, double Qw, double Qp) => (Qw / (Qg+Qp));

        #region funkcje z pliku z wytycznymi
        private static double hs_H2Ol(double T) => 1.0 / 18.015 * (2.348e3 * (T - TemperatureReference) - 4.548 * 0.5 * (T * T - TemperatureReference * TemperatureReference) + 3.437e-3 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference));
        private static double hs_H2O(double T) => 1.0 / 22.42 * (4.184 * (8.22 * (T - TemperatureReference) + 0.00015 * 0.5 * (T * T - TemperatureReference * TemperatureReference) - 0.00000134 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference)));
        private static double hs_O2(double T) => 1.0 / 22.42 * (4.184 * (8.27 * (T - TemperatureReference) + 0.000258 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        private static double hs_CO2(double T) => 1.0 / 22.42 * (4.184 * (10.34 * (T - TemperatureReference) + 0.00274 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        private static double hs_CO(double T) => 1.0 / 22.42 * (4.184 * (6.6 * (T - TemperatureReference) + 0.0012 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        private static double hs_H2(double T) => 1.0 / 22.42 * (4.184 * (6.62 * (T - TemperatureReference) + 0.00081 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        private static double hs_N2(double T) => 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        private static double hs_CH4(double T) => 1.0 / 22.42 * (4.184 * (5.34 * (T - TemperatureReference) + 0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));
        #endregion
    }
}
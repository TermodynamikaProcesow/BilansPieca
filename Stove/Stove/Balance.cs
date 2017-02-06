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
        private const double ksi = 0;

        public static OutputData Estimate(InputData input)
        {
            //tlen teoretyczny
            double Ot = countOt(input.CO, input.CH4, input.H2, input.O2);

            //tlen całkowity
            double Oc = countOc(Ot, input.lambda);

            //powietrze teoretyczne
            double Vo = countVo(Ot);

            //powietrze całkowite
            double Vc = countVc(input.lambda, Vo);

            //strumien objętości gazu w warunkach rzeczywistych
            double Vg_wrz = countVg_wrz(input.tg, input.Vg);

            //ilość i skład spalin
            double V_CO2 = countV_CO2(input.CO, input.CO2, input.CH4);
            double V_H2O = countV_H2O(input.H2, input.CH4, ksi, Vg_wrz);
            double V_N2 = countV_N2(input.N2, Vc);
            double V_O2 = countV_O2(Oc, Ot);
            double VSprim = countVSprim(V_CO2, V_H2O, V_N2, V_O2);

            double V_CO2sp = countV_PercentSp(V_CO2, VSprim);
            double V_H2Osp = countV_PercentSp(V_H2O, VSprim);
            double V_N2sp = countV_PercentSp(V_N2, VSprim);
            double V_O2sp = countV_PercentSp(V_O2, VSprim);

            //strumień objętości powietrza
            double Vc_wn = countVc_wn(Vc, input.Vg);
            double Vc_wrz = countVc_wrz(input.tp, Vc_wn);

            //strumień objętości spalin
            double Vs_wn = countVs(input.Vg, VSprim);

            //wartość opałowa gazu
            double Qi = countQi(input.CO, input.CH4, input.H2);

            //entalpia gazu
            double Hs_gas = countHsGas(input.CO, input.CH4, input.H2, input.O2, input.CO2, input.N2, input.tg);

            //strumień ciepła z gazu
            double Qg = countQg(Qi, Hs_gas);
            double Qg_Vg = countQgVg(Qg, input.Vg);

            //strumień ciepła powietrza
            double Qp = countQp(Vc, input.tp);
            double Qp_Vg = countQpVg(Qp, input.Vg);

            //temperatura adiabatyczna
            double ta = countAdiabaticTemperatureOfFlame(VSprim, V_CO2sp, V_H2Osp, V_N2sp, V_O2sp, Qg, Qp);

            //strumień ciepła wody
            double Qw = countQw(input.alfa, input.Vg, Vc_wn, ta, input.A, input.twIn);

            //temperatura za piecem
            double tw = countTw(input.mw, Qw, input.twIn);

            //strumień ciepła spalin
            double Qs = countQs(input.beta, Qg_Vg, Qp_Vg, Qw);

            double Tsp = countTsp(Qs, V_CO2sp, V_H2Osp, V_N2sp, V_O2sp, input.Vg, VSprim);
            double Vs_wrz = countVs_wrz(Tsp, Vs_wn);

            //strumień ciepła strat
            double Qstr = countQstr(input.beta, Qg_Vg, Qp_Vg, Qw);

            //sprawność pieca
            double eta = countEta(Qg_Vg, Qw, Qp_Vg);

            return new OutputData
            {
                CO2 = Math.Round(V_CO2sp, 3),
                H2O = Math.Round(V_H2Osp, 3),
                N2 = Math.Round(V_N2sp, 3),
                O2 = Math.Round(V_O2sp, 3),
                Vg = Math.Round(input.Vg, 3),
                Vc = Math.Round(Vc_wn * 3600, 3),
                VcRz = Math.Round(Vc_wrz * 3600, 3),
                Vs = Math.Round(Vs_wn * 3600, 3),
                VsRz = Math.Round(Vs_wrz * 3600, 3),
                tw = Math.Round(tw, 3),
                ta = Math.Round(ta, 3),
                ts = Math.Round(Tsp, 3),
                Qq = Math.Round(Qg_Vg, 3),
                Qp = Math.Round(Qp_Vg, 3),
                Qw = Math.Round(Qw, 3),
                Qs = Math.Round(Qs, 3),
                Qstr = Math.Round(Qstr, 3),
                eta = Math.Round(eta, 3)
            };
        }

        /// <summary>
        /// obliczenie ilości tlenu teoretycznego
        /// </summary>
        private static double countOt(double CO, double CH4, double H2, double O2)
            => 0.5 * CO / 100 + 2 * CH4 / 100 + 0.5 * H2 / 100 - O2 / 100;

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
        private static double countVg_wrz(double TemperatureGas, double VolumeGas)
            => VolumeGas * (TemperatureGas + TemperatureReference) / TemperatureReference;

        //funkcje do obliczenia ilości i składu spalin
        private static double countV_CO2(double procentCO, double procentCO2, double procentCH4)
            => procentCO / 100 + procentCO2 / 100 + procentCH4 / 100;

        private static double countV_H2O(double procentH2, double procentCH4, double ksi, double VolumeGasReal)
            => procentH2 / 100 + 2 * procentCH4 / 100 + 0.0016 * ksi * VolumeGasReal;

        private static double countV_N2(double procentN2, double V) => procentN2 / 100 + V * 0.79;

        private static double countV_O2(double Oc, double Ot) => Oc - Ot;

        private static double countVSprim(double V_CO2, double V_H2O, double V_N2, double V_O2)
            => V_CO2 + V_H2O + V_N2 + V_O2;

        private static double countV_PercentSp(double V_Component, double VSprim) => V_Component / VSprim * 100;

        /// <summary>
        /// strumień objętości powietrza w warunkach normalnych
        /// </summary>
        private static double countVc_wn(double Vc, double Vg) => Vc * (Vg / 3600.00);

        /// <summary>
        /// strumień objętości powietrza w warunkach rzeczywistych
        /// </summary>
        private static double countVc_wrz(double tp, double Vc_wn)
            => Vc_wn * (tp + TemperatureReference) / TemperatureReference;

        /// <summary>
        /// strumień objetości spalin w warunkach normalnych
        /// </summary>
        private static double countVs(double Vg, double VSprim) => Vg / 3600.00 * VSprim;

        private static double countVs_wrz(double Tsp, double Vsp)
            => Vsp * (Tsp + TemperatureReference) / TemperatureReference;

        /// <summary>
        /// wartość opałowa gazu
        /// </summary>
        private static double countQi(double CO_fuel, double CH4_fuel, double H2_fuel)
            => 0.01 * (Qco * CO_fuel + Qch4 * CH4_fuel + Qh2 * H2_fuel);

        /// <summary>
        /// entalpia gazu
        /// </summary>
        private static double countHsGas(double CO_fuel, double CH4_fuel, double H2_fuel, double O2_fuel,
            double CO2_fuel, double N2_fuel, double tg)
        {
            double Tg = tg + TemperatureReference;

            return hs_CO(Tg) * (CO_fuel / 100) + hs_CH4(Tg) * (CH4_fuel / 100) + hs_H2(Tg) * (H2_fuel / 100) +
                   hs_CO2(Tg) * (CO2_fuel / 100) + hs_N2(Tg) * (N2_fuel / 100) + hs_O2(Tg) * (O2_fuel / 100);
        }

        /// <summary>
        /// strumień ciepła z gazu
        /// </summary>
        private static double countQg(double Qi, double hsGas) => hsGas + Qi;

        /// <summary>
        /// strumien ciepła z gazu przeliczony przez strumień objetości gazu
        /// </summary>
        private static double countQgVg(double Qg, double Vg) => Vg / 3600.00 * Qg;

        /// <summary>
        /// strumień ciepła powietrza
        /// </summary>
        private static double countQp(double Vc, double tp)
        {
            double Tp = tp + TemperatureReference;
            return Vc * (0.21 * hs_O2(Tp) + 0.79 * hs_N2(Tp));
        }

        /// <summary>
        /// strumień ciepła powietrza przeliczony przez strumień objetości gazu
        /// </summary>
        private static double countQpVg(double Qp, double Vg) => Vg / 3600 * Qp;

        /// <summary>
        /// adiabatyczna temperatura płomienia
        /// </summary>
        private static double countAdiabaticTemperatureOfFlame(double VSprim, double CO2_fumes, double H2O_fumes,
            double N2_Fumes, double O2_fumes, double Qg, double Qp)
        {
            double _hsp = 0, _hsp_previous = 0;
            double temp = 0, _temp_previous = 0;
            double hsp = (Qg + Qp) / VSprim;
            double Tk;

            for (int t = 0; t < 5000; t++)
            {
                Tk = t + TemperatureReference;
                temp = t;
                _hsp = 0.01 *
                       (hs_CO2(Tk) * CO2_fumes + hs_H2O(Tk) * H2O_fumes + hs_N2(Tk) * N2_Fumes + hs_O2(Tk) * O2_fumes);
                if (_hsp > hsp) break;

                _hsp_previous = _hsp;
                _temp_previous = t;
            }
            return (hsp - _hsp_previous) / (_hsp - _hsp_previous) * (temp - _temp_previous) + _temp_previous;
        }

        private static double countTsp(double Qs, double CO2sp, double H2Osp, double N2sp, double O2sp, double Vg,
            double Vsprim)
        {
            double temp = TemperatureReference;
            double hsp = 0;
            double r = Vsprim * Vg;
            double Hsp = Qs * 3600;
            double result = Hsp / r;

            while (hsp < result)
            {
                temp += 0.001;
                hsp = hs_CO2(temp) * (CO2sp / 100) + hs_H2O(temp) * (H2Osp / 100) + hs_N2(temp) * (N2sp / 100) +
                      hs_O2(temp) * (O2sp / 100);
            }
            return temp - TemperatureReference;
        }

        /// <summary>
        /// strumień ciepła wody
        /// </summary>
        private static double countQw(double alfa, double Vg, double Vc, double ta, double A, double twin)
            => alfa * Math.Pow(Vg / 3600.00 * Vc, (double) 2 / 3) * Math.Pow(ta, 0.3) * A * (ta - twin);

        /// <summary>
        /// temperatura za piecem
        /// </summary>
        private static double countTw(double mw, double Qw, double twin)
        {
            double resultOfEquation = (Qw * 3600 + mw * hs_H2Ol(twin + TemperatureReference)) / mw;

            double hsp = 0;
            double temp = TemperatureReference;
            while (hsp <= resultOfEquation)
            {
                temp += 0.001;
                hsp = hs_H2Ol(temp);
            }
            return temp - TemperatureReference;
        }

        /// <summary>
        /// strumień ciepła spalin
        /// </summary>
        private static double countQs(double beta, double Qg, double Qp, double Qw) => (1 - beta) * (Qg + Qp - Qw);

        /// <summary>
        /// strumień ciepła strat
        /// </summary>
        private static double countQstr(double beta, double Qg, double Qp, double Qw) => beta * (Qg + Qp - Qw);

        /// <summary>
        /// sprawnosc pieca
        /// </summary>
        private static double countEta(double Qg, double Qw, double Qp) => Qw / (Qg + Qp);

        #region funkcje z pliku z wytycznymi

        private static double hs_H2Ol(double T)
            =>
                1.0 / 18.015 *
                (2.348e3 * (T - TemperatureReference) -
                 4.548 * 0.5 * (T * T - TemperatureReference * TemperatureReference) +
                 3.437e-3 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference));

        private static double hs_H2O(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (8.22 * (T - TemperatureReference) +
                  0.00015 * 0.5 * (T * T - TemperatureReference * TemperatureReference) -
                  0.00000134 / 3.0 * (T * T * T - TemperatureReference * TemperatureReference * TemperatureReference)));

        private static double hs_O2(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (8.27 * (T - TemperatureReference) +
                  0.000258 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        private static double hs_CO2(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (10.34 * (T - TemperatureReference) +
                  0.00274 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        private static double hs_CO(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (6.6 * (T - TemperatureReference) +
                  0.0012 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        private static double hs_H2(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (6.62 * (T - TemperatureReference) +
                  0.00081 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        private static double hs_N2(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (6.5 * (T - TemperatureReference) + 0.001 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        private static double hs_CH4(double T)
            =>
                1.0 / 22.42 *
                (4.184 *
                 (5.34 * (T - TemperatureReference) +
                  0.0115 * 0.5 * (T * T - TemperatureReference * TemperatureReference)));

        #endregion
    }
}
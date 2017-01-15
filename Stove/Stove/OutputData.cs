using System;
using System.Text;

namespace Stove
{
    internal class OutputData : IData
    {
        /// <summary>
        /// % - stezenie CO2 w spalinach
        /// </summary>
        public double CO2 { get; set; }
        /// <summary>
        /// % - stezenie H2O w spalinach
        /// </summary>
        public double H2O { get; set; }
        /// <summary>
        /// % - stezenie N2 w spalinach
        /// </summary>
        public double N2 { get; set; }
        /// <summary>
        /// % - stezenie O2 w spalinach
        /// </summary>
        public double O2 { get; set; }
        /// <summary>
        /// Nm3/h - objetosciowy strumien paliwa
        /// </summary>
        public double Vg { get; set; }
        /// <summary>
        /// Nm3/h - objetosciowy strumien powietrza całkowitego
        /// </summary>
        public double Vc { get; set; }
        /// <summary>
        /// m3/h - objetosciowy strumien powietrza całkowitego w warunkach rzeczywistych
        /// </summary>
        public double VcRz { get; set; }
        /// <summary>
        /// Nm3/h - objetosciowy strumien spalin wilgotnych
        /// </summary>
        public double Vs { get; set; }
        /// <summary>
        /// m3/h - objetosciowy strumien spalin wilgotnych w warunkach rzeczywistych
        /// </summary>
        public double VsRz { get; set; }
        /// <summary>
        /// oC - temp. wody za piecem
        /// </summary>
        public double tw { get; set; }
        /// <summary>
        /// oC - adiabatyczna temp. płomienia
        /// </summary>
        public double ta { get; set; }
        /// <summary>
        /// oC - temp. spalin
        /// </summary>
        public double ts { get; set; }
        /// <summary>
        /// kW - strumien ciepła z paliwa
        /// </summary>
        public double Qq { get; set; }
        /// <summary>
        /// kW - strumien ciepła z powietrza
        /// </summary>
        public double Qp { get; set; }
        /// <summary>
        /// kW - strumien ciepła do wody
        /// </summary>
        public double Qw { get; set; }
        /// <summary>
        /// kW - strumien ciepła do spalin
        /// </summary>
        public double Qs { get; set; }
        /// <summary>
        /// kW - straty do otoczenia
        /// </summary>
        public double Qstr { get; set; }
        /// <summary>
        /// sprawność pieca
        /// </summary>
        public double eta { get; set; }

        public override string ToString()
        {
            try
            {
                var ci = Program.CultureInfo;
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(ci, "CO2 = {0} %", CO2));
                sb.AppendLine(string.Format(ci, "H2O = {0} %", H2O));
                sb.AppendLine(string.Format(ci, "N2 = {0} %", N2));
                sb.AppendLine(string.Format(ci, "O2 = {0} %", O2));
                sb.AppendLine(string.Format(ci, "Vg = {0} Nm3/h", Vg));
                sb.AppendLine(string.Format(ci, "Vc = {0} Nm3/h", Vc));
                sb.AppendLine(string.Format(ci, "Vc = {0} m3/h", VcRz));
                sb.AppendLine(string.Format(ci, "Vs = {0} Nm3/h", Vs));
                sb.AppendLine(string.Format(ci, "Vs = {0} m3/h", VsRz));
                sb.AppendLine(string.Format(ci, "t,w = {0} oC", tw));
                sb.AppendLine(string.Format(ci, "t,a = {0} oC", ta));
                sb.AppendLine(string.Format(ci, "t,s = {0} oC", ts));
                sb.AppendLine(string.Format(ci, "Q,q = {0} kW", Qq));
                sb.AppendLine(string.Format(ci, "Q,p = {0} kW", Qp));
                sb.AppendLine(string.Format(ci, "Q,w = {0} kW", Qw));
                sb.AppendLine(string.Format(ci, "Q,s = {0} kW", Qs));
                sb.AppendLine(string.Format(ci, "Q,str = {0} kW", Qstr));
                sb.AppendLine(string.Format(ci, "eta = {0}", eta));

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while OutputData.ToString() - {ex}");
                return string.Empty;
            }
        }

        public void DisplayData()
        {
            Console.Write(ToString());
        }
    }
}
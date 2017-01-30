using System;
using System.Text;

namespace Stove
{
    internal class InputData
    {
        /// <summary>
        /// % - zawartosc CO w paliwie
        /// </summary>
        public double CO { get; }
        /// <summary>
        /// % - zawartosc CO2 w paliwie
        /// </summary>
        public double CO2 { get; }
        /// <summary>
        /// % - zawartosc CH4 w paliwie
        /// </summary>
        public double CH4 { get; }
        /// <summary>
        /// % - zawartosc O2 w paliwie
        /// </summary>
        public double O2 { get; }
        /// <summary>
        /// % - zawartosc N2 w paliwie
        /// </summary>
        public double N2 { get; }
        /// <summary>
        /// % - zawartosc H2 w paliwie
        /// </summary>
        public double H2 { get; }
        /// <summary>
        /// współczynnik nadmiaru powietrza
        /// </summary>
        public double lambda { get; }
        /// <summary>
        /// Nm3/h - objetosciowy strumien paliwa
        /// </summary>
        public double Vg { get; }
        /// <summary>
        /// oC - temp. paliwa
        /// </summary>
        public double tg { get; }
        /// <summary>
        /// oC - temp. powietrza
        /// </summary>
        public double tp { get; }
        /// <summary>
        /// oC - temp. wody przed piecem
        /// </summary>
        public double twIn { get; }
        /// <summary>
        /// kg/h - masowy strumien wody w wymienniku pieca
        /// </summary>
        public double mw { get; }
        /// <summary>
        /// m2 - powierzchnia wymiennika w piecu
        /// </summary>
        public double A { get; }
        /// <summary>
        /// współczynnik przejmowania ciepla wymiennika
        /// </summary>
        public double alfa { get; }
        /// <summary>
        /// współczynnik podziału ciepła do spalin i otoczenia
        /// </summary>
        public double beta { get; }

        public InputData(InputData input, int Vg_multiplication)
        {
            CO = input.CO;
            CO2 = input.CO2;
            CH4 = input.CH4;
            O2 = input.O2;
            N2 = input.N2;
            H2 = input.H2;
            lambda = input.lambda;
            Vg = Vg_multiplication * input.Vg;
            tg = input.tg;
            tp = input.tp;
            twIn = input.twIn;
            mw = input.mw;
            A = input.A;
            alfa = input.alfa;
            beta = input.beta;
        }

        public InputData(string[] inputData)
        {
            var inputCount = inputData.Length;
            if (inputCount < 15)
                throw new Exception("Too few input elements!");
            if (inputCount > 15)
                throw new Exception("Too many input elements!");

            var ci = Program.CultureInfo;

            CO = double.Parse(inputData[0], ci);
            CO2 = double.Parse(inputData[1], ci);
            CH4 = double.Parse(inputData[2], ci);
            O2 = double.Parse(inputData[3], ci);
            N2 = double.Parse(inputData[4], ci);
            H2 = double.Parse(inputData[5], ci);
            lambda = double.Parse(inputData[6], ci);
            Vg = double.Parse(inputData[7], ci);
            tg = double.Parse(inputData[8], ci);
            tp = double.Parse(inputData[9], ci);
            twIn = double.Parse(inputData[10], ci);
            mw = double.Parse(inputData[11], ci);
            A = double.Parse(inputData[12], ci);
            alfa = double.Parse(inputData[13], ci);
            beta = double.Parse(inputData[14], ci);
        }

        public override string ToString()
        {
            try
            {
                var ci = Program.CultureInfo;
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość CO w paliwie", CO));
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość CO2 w paliwie", CO2));
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość CH4 w paliwie", CH4));
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość O2 w paliwie", O2));
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość N2 w paliwie", N2));
                sb.AppendLine(string.Format(ci, "{0} % -> zawartość H2 w paliwie", H2));
                sb.AppendLine(string.Format(ci, "{0} -> współczynnik nadmiaru powietrza", lambda));
                sb.AppendLine(string.Format(ci, "{0} Nm/h -> objętościowy strumień paliwa", Vg));
                sb.AppendLine(string.Format(ci, "{0} st. C -> temperatura paliwa", tg));
                sb.AppendLine(string.Format(ci, "{0} st. C -> temperatura powietrza", tp));
                sb.AppendLine(string.Format(ci, "{0} st. C -> temperatura wody przed piecem", twIn));
                sb.AppendLine(string.Format(ci, "{0} kg/h -> strumień masowy wody w wymienniku pieca", mw));
                sb.AppendLine(string.Format(ci, "{0} m2 -> powierzchnia wymiennika", A));
                sb.AppendLine(string.Format(ci, "{0} -> współczynnik przejmowania ciepła wymiennika", alfa));
                sb.AppendLine(string.Format(ci, "{0} -> współczynnik podziału ciepła do spalin i otoczenia", beta));

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
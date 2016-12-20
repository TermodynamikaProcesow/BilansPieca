using System;

namespace Stove
{
    class InputData
    {
        /// <summary>
        /// % - zawartosc CO w paliwie
        /// </summary>
        public double CO { get; set; }
        /// <summary>
        /// % - zawartosc CO2 w paliwie
        /// </summary>
        public double CO2 { get; set; }
        /// <summary>
        /// % - zawartosc CH4 w paliwie
        /// </summary>
        public double CH4 { get; set; }
        /// <summary>
        /// % - zawartosc O2 w paliwie
        /// </summary>
        public double O2 { get; set; }
        /// <summary>
        /// % - zawartosc N2 w paliwie
        /// </summary>
        public double N2 { get; set; }
        /// <summary>
        /// % - zawartosc H2 w paliwie
        /// </summary>
        public double H2 { get; set; }
        /// <summary>
        /// współczynnik nadmiaru powietrza
        /// </summary>
        public double lambda { get; set; }
        /// <summary>
        /// Nm3/h - objetosciowy strumien paliwa
        /// </summary>
        public double V { get; set; }
        /// <summary>
        /// oC - temp. paliwa
        /// </summary>
        public double tg { get; set; }
        /// <summary>
        /// oC - temp. powietrza
        /// </summary>
        public double tp { get; set; }
        /// <summary>
        /// oC - temp. wody przed piecem
        /// </summary>
        public double twIn { get; set; }
        /// <summary>
        /// kg/h - masowy strumien wody w wymienniku pieca
        /// </summary>
        public double mw { get; set; }
        /// <summary>
        /// m2 - powierzchnia wymiennika w piecu
        /// </summary>
        public double A { get; set; }
        /// <summary>
        /// współczynnik przejmowania ciepla wymiennika
        /// </summary>
        public double alfa { get; set; }
        /// <summary>
        /// współczynnik podziału ciepła do spalin i otoczenia
        /// </summary>
        public double beta { get; set; }


        public InputData(string[] inputData)
        {
            var inputCount = inputData.Length;
            if (inputCount < 15)
                throw new Exception("Too few input elements!");
            else if (inputCount > 15)
                throw new Exception("Too many input elements!");

            CO = double.Parse(inputData[0].Replace('.', ','));
            CO2 = double.Parse(inputData[1].Replace('.', ','));
            CH4 = double.Parse(inputData[2].Replace('.', ','));
            O2 = double.Parse(inputData[3].Replace('.', ','));
            N2 = double.Parse(inputData[4].Replace('.', ','));
            H2 = double.Parse(inputData[5].Replace('.', ','));
            lambda = double.Parse(inputData[6].Replace('.', ','));
            V = double.Parse(inputData[7].Replace('.', ','));
            tg = double.Parse(inputData[8].Replace('.', ','));
            tp = double.Parse(inputData[9].Replace('.', ','));
            twIn = double.Parse(inputData[10].Replace('.', ','));
            mw = double.Parse(inputData[11].Replace('.', ','));
            A = double.Parse(inputData[12].Replace('.', ','));
            alfa = double.Parse(inputData[13].Replace('.', ','));
            beta = double.Parse(inputData[14].Replace('.', ','));
        }
    }
}

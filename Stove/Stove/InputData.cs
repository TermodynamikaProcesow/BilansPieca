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
    }
}

namespace Stove
{
    class OutputData
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
    }
}

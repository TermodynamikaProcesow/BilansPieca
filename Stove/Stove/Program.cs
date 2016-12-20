namespace Stove
{
    class Program
    {
        static void Main(string[] args)
        {
            InputData inputData = FileStream.Read("input.dat");
            OutputData outputData = Balance.Estimate(inputData);
            FileStream.Write(outputData, "output1.dat");
        }
    }
}

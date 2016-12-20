using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stove
{
    class FileStream
    {
        public static InputData Read(string fileName)
        {
            try
            {
                string data;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    data = sr.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(data)) throw new Exception("Data is null or white space!");
                
                string[] inputData = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                return new InputData(inputData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading input data - {ex}");
                return null;
            }
        }

        public static void Write(OutputData outputData, string filename)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(filename))
                {
                    sr.Write(outputData.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while writing output data - {ex}");
            }
        }
    }
}

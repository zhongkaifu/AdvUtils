using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AdvUtils;

namespace TestVectorQuantization
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("TestVectorQuantization [input file] [output file] [VQ Size]");
                return;
            }

            int VQSize = int.Parse(args[2]);
            VectorQuantization vq = new VectorQuantization();
            StreamReader sr = new StreamReader(args[0]);
            string strLine = null;

            while ((strLine = sr.ReadLine()) != null)
            {
                float f = float.Parse(strLine);
                vq.Add(f);
            }
            sr.Close();

            double distortion = vq.BuildCodebook(VQSize);
            Logger.WriteLine("Average distortion: {0}", distortion);
            vq.WriteCodebook(args[1]);
        }
    }
}

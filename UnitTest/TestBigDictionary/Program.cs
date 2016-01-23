using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvUtils;

namespace TestBigDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test BTreeDictionary for very large data set
            BTreeDictionary<long, long> sdict = new BTreeDictionary<long, long>();

            for (long i = 10000; i >= 0; i-=2)
            {
                sdict.Add(i, i);
            }

            long val = sdict.KeyList[123];
            long n = sdict.ValueList[123];
            sdict.RemoveAt(123);
            long cnt = sdict.ValueList.Count;


            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}

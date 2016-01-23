using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvUtils;

namespace TestBigArray
{
    class Program
    {
        static void Main(string[] args)
        {
            FixedBigArray<int> ba = new FixedBigArray<int>(1024, 0);
            ba[1] = 1;
        }
    }
}

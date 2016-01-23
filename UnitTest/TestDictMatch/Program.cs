using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AdvUtils;

namespace TestDictMatch
{
    class Program
    {
        public static void VerifyBinaryDict(string strTestFileName, string strRawDictFileName)
        {
            Console.WriteLine("Convert dictionary from raw text to binary format.");
            DictMatch match = new DictMatch();
            match.ConvertDictFromRawTextToBinary(strRawDictFileName, strRawDictFileName + ".bin");

            Console.WriteLine("Load binary dictionary...");
            match = new DictMatch();
            match.LoadDictFromBinary(strRawDictFileName + ".bin");

            Console.WriteLine("Verify binary dictionary...");
            Match(strTestFileName, match);
        }

        public static void VerifyRawTextDict(string strTestFileName, string strRawDictFileName)
        {
            Console.WriteLine("Load raw text dictionary...");
            DictMatch match = new DictMatch();
            match.LoadDictFromRawText(strRawDictFileName);

            Console.WriteLine("Verify raw text dictionary...");
            Match(strTestFileName, match);
        }


        //Read each line from strTextFileName, and verify wether terms in every line are in strDictFileName
        public static void Match(string strTextFileName, DictMatch match)
        {
            List<Lemma> dm_r = new List<Lemma>();
            List<int> offsetList = new List<int>();

            StreamReader sr = new StreamReader(strTextFileName);
            while (sr.EndOfStream == false)
            {
                string strLine = sr.ReadLine();
                if (strLine.Length == 0)
                {
                    continue;
                }

                dm_r.Clear();
                offsetList.Clear();
                match.Search(strLine, ref dm_r, ref offsetList, DictMatch.DM_OUT_FMM);

                //if dm_r.Count > 0, it means some contigous terms in strLine have matched terms in the dictionary.
                for (int i = 0; i < dm_r.Count; i++)
                {
                    uint len = dm_r[i].len;
                    int offset = offsetList[i];
                    string strProp = dm_r[i].strProp;
                    string strTerm = strLine.Substring(offset, (int)len);
                    Console.WriteLine("Matched term: {0}[offset:{1}, len:{2}, prop:{3}]", strTerm, offset, len, strProp);
                }
            }
            sr.Close();

        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("TestDictMatch [raw lexical dictionary file name] [test text file name]");
                return;
            }

            string strTestFileName = args[1];
            string strDictFileName = args[0];

            VerifyRawTextDict(strTestFileName, strDictFileName);
            VerifyBinaryDict(strTestFileName, strDictFileName);
        }
    }
}

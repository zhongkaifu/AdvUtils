using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AdvUtils;

namespace TestDoubleArrayTrie
{
    class Program
    {
        //Build double array trie-tree from text file
        //strTextFileName: raw text file name used to build DA trie-tree
        //  text file format: key \t value
        //  key as string type
        //  value as non-netgive integer
        //strDAFileName: double array trie-tree binary file name built from strTextFileName
        private static void Build(string strTextFileName, string strDAFileName)
        {
            StreamReader sr = new StreamReader(strTextFileName);
            //Load raw data from text file and sort them as ordinal
            SortedDictionary<string, int> sdict = new SortedDictionary<string, int>(StringComparer.Ordinal);
            Console.WriteLine("Loading key value pairs from raw text file and sort them...");
            while (sr.EndOfStream == false)
            {
                string strLine = sr.ReadLine();
                if (strLine.Length == 0)
                {
                    continue;
                }

                string[] items = strLine.Split('\t');
                sdict.Add(items[0], int.Parse(items[1]));
            }

            //test case for SearchAsKeyPrefix and SearchByPrefix
            sdict.Add("TestSearchPrefix_case0", 1234567);
            sdict.Add("TestSearchPrefix_case01", 2345678);
            sdict.Add("TestSearchPrefix_case012", 3456789);

            DoubleArrayTrieBuilder dab = new DoubleArrayTrieBuilder(4);
            Console.WriteLine("Begin to build double array trie-tree...");
            dab.build(sdict);
            dab.save(strDAFileName);
            Console.WriteLine("Done!");
        }

        //Verify whether double array trie-tree correct
        //strTextFileName: raw text file name used to build DA trie-tree
        //  text file format: key \t value
        //  key as string type
        //  value as non-netgive integer
        //strDAFileName: double array trie-tree binary file name built from strTextFileName
        private static void Verify(string strTextFileName, string strDAFileName)
        {
            StreamReader sr = new StreamReader(strTextFileName);
            DoubleArrayTrieSearch das = new DoubleArrayTrieSearch();

            das.Load(strDAFileName);
            while (sr.EndOfStream == false)
            {
                string strLine = sr.ReadLine();
                if (strLine.Length == 0)
                {
                    continue;
                }
                string[] items = strLine.Split('\t');
                int val = int.Parse(items[1]);
                string fea = items[0];

                int rval = das.SearchByPerfectMatch(fea);
                if (rval != val)
                {
                    Console.WriteLine("Values in raw text file and double array trie-tree is different");
                    Console.WriteLine("Key-Value in text file: {0}", strLine);
                    Console.WriteLine("Value in DA trie: {0}", rval);
                }
            }

            //Test SearchAsKeyPrefix function.
            //TestSearchPrefix_case0, TestSearchPrefix_case01, TestSearchPrefix_case012 should be in result list
            List<int> resultList = new List<int>();
            int rlistCnt = das.SearchAsKeyPrefix("TestSearchPrefix_case012", resultList);

            //Test SearchByPrefix
            resultList = new List<int>();
            rlistCnt = das.SearchByPrefix("TestSearchPrefix_case0", resultList);
            rlistCnt = das.SearchByPrefix("U04:京", resultList);

            Console.WriteLine("Done!");
        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("TestDoubleArrayTrie [raw key-value set file name]");
                return;
            }

            string strTextFileName = args[0];
            string strDAFileName = args[0] + ".da";

            DateTime dt = DateTime.Now;

            Console.WriteLine("Building double array trie-tree...");
            Build(strTextFileName, strDAFileName);
            Console.WriteLine("Verify double array trie-tree...");
            Verify(strTextFileName, strDAFileName);

            TimeSpan ts = DateTime.Now - dt;

            Console.WriteLine("Time Span: {0}", ts);
        }
    }
}
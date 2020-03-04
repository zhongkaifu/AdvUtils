using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AdvUtils
{
	public class ArgParser
	{

        object m_o;
        List<ArgField> m_arrayArgs;

		public ArgParser(string[] args, object o)
		{
			m_o = o;
            m_arrayArgs = new List<ArgField>();
			Type typeArgAttr = typeof(Arg);
			Type t = o.GetType();
			foreach (FieldInfo fi in t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				foreach (Arg arg in fi.GetCustomAttributes(typeArgAttr, true))
				{
					m_arrayArgs.Add(new ArgField(o, fi, arg));
				}
			}

			try
			{
				for (int i = 0; i < args.Length; i++)
				{
                    if (args[i].StartsWith("-"))
                    {
                        string strArgName = args[i].Substring(1);
                        string strArgValue = args[i + 1];

                        ArgField intarg = GetArgByName(strArgName);
                        intarg.Set(strArgValue);

                        i++;
                    }
				}

				foreach (ArgField a in m_arrayArgs)
					a.Validate();
			}
			catch (Exception err)
			{
				Usage(err.Message);
			}
		}

        ArgField GetArgByName(string name)
		{
			foreach (ArgField a in m_arrayArgs)
				if (a.Arg.Name.ToLower() == name.ToLower())
					return a;
			return null;
		}

		void Usage(string error)
		{
			Console.Error.WriteLine("\nERROR: {0}", error);
			Usage();
		}

		public void Usage()
		{
			Type t = typeof(Arg);
			string strAppName = Process.GetCurrentProcess().ProcessName;
            Console.Error.WriteLine("Usage: {0} [parameters...]", strAppName);

            foreach (var item in m_arrayArgs)
            {
                Console.Error.WriteLine($"\t[-{item.Arg.Name}: {item.Arg.Title}]");
            }

            System.Environment.Exit(1);
        }

	}
}


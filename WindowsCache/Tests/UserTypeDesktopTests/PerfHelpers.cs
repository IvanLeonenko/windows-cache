using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserTypeDesktopTests.UserTypes;

namespace DesktopTests
{
    class PerfHelpers
    {   
        public static void Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action.Invoke();
            sw.Stop();
            Console.WriteLine("Elapsed:" + sw.ElapsedMilliseconds);
        }

        public static byte[][] GetByteArrays(int number)
        {
            var result = new byte[number][];

            for (int i = 1; i <= number; i++)
            {
                if (i > 99)
                {
                    result[i - 1] = GetResourceBytes(i.ToString());
                }
                else if (i > 9)
                {
                    result[i - 1] = GetResourceBytes("0" + i);
                }
                else
                    result[i - 1] = GetResourceBytes("00" + i);
            }

            return result;

        }

        public static TestObject[] GetUserTypeArrays(int number)
        {
            var result = new TestObject[number];
            for (int i = 1; i <= number; i++)
            {
                result[i-1] = new TestObject();
                result[i-1].Init(1000);
            }
            return result;
        }
        
        public static byte[] GetResourceBytes(string name)
        {
            name = "DesktopTests.Bytes.bytes." + name;
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (stream == null)
                throw new Exception(String.Format("Resource is missing: {0}", name));
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static Stream GetResourceStream(string name)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DesktopTests.Bytes.bytes" + name);
            if (stream == null)
                throw new Exception(String.Format("Resource is missing: {0}", name));
            return stream;
        }

    }
}

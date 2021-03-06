#if false

using System;
using System.Globalization;
using System.Threading;
using System.Web.Compilation;
using Westwind.Globalization.Web;
using NUnit.Framework;

namespace Westwind.Globalization.Test
{
    [TestFixture]
    public class DbSimpleResourceProviderTests
    {

        [Test]
        public void DbSimpleResourceProviderBasic()
        {
            var resRaw = new DbSimpleResourceProvider(null,"Resources");
            var res = resRaw as IResourceProvider;

            Console.WriteLine(res.GetObject("Today", new CultureInfo("de-de")));
            Console.WriteLine(res.GetObject("Today", new CultureInfo("en-us")));
        }

        static IResourceProvider resProvider;
        static bool start = false;
        [Test]
        public void DbSimpleResourceProviderHeavyLoad()
        {
            resProvider = new DbSimpleResourceProvider(null, "Resources") as IResourceProvider;

            var dt = DateTime.Now;
            for (int i = 0; i < 500; i++)
            {
                var t = new Thread(threadedDbSimpleResourceProvider);
                t.Start(dt);
            }
            
            Thread.Sleep(150);
            start = true;
            Console.WriteLine("Started:  " + DateTime.Now.Ticks);

            // allow threads to run
            Thread.Sleep(4000);
        }


        void threadedDbSimpleResourceProvider(object dt)
        {
            while (!start)
            {
                Thread.Sleep(1);
            }

            try
            {                
                Console.WriteLine(resProvider.GetObject("Today", new CultureInfo("de-de")) + " - " + Thread.CurrentThread.ManagedThreadId + " - " + DateTime.Now.Ticks);
                Console.WriteLine(resProvider.GetObject("Today", new CultureInfo("en-us")) + " - " + Thread.CurrentThread.ManagedThreadId + " - " + DateTime.Now.Ticks);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** ERROR: " + ex.Message);
            }
        }
    }
}
#endif
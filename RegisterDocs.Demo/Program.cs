using RegisterDocs.Models;
using RegisterDocs.Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs.Demo
{
  internal static class Program
  {
    private static void Main(string[] args)
    {
      var timer = new Stopwatch();

      timer.Start();
      DocTrigger.Execute();
      timer.Stop();

      Console.WriteLine("Done!({0} sek)", timer.ElapsedMilliseconds / 1000);

      Console.ReadKey();
    }
  }
}

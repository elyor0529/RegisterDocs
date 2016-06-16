using RegisterDocs.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs.Scheduler
{
  public static class DocTrigger
  {
    public static void Execute()
    {
      using (var db = new DocDatabase())
      {
        var docs = db.GetCollection<Docs>();
        var ids = docs.Find(w => w.IsActive == true).Select(s => s.Id).ToArray();
        var partitions = Partitioner.Create(0, ids.Length, 100);

        Console.WriteLine("Total {0} items", ids.Length);

        partitions.AsParallel()
          .WithDegreeOfParallelism(Environment.ProcessorCount)
          .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
          .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
          .ForAll(idRange =>
      {

        for (var i = idRange.Item1; i < idRange.Item2; i++)
        {
          var id = ids[i];
          var doc = docs.FindById(id);

          doc.CalculateColourStatus();

          docs.Update(doc);
        }

        Console.WriteLine("Saved Items - {0} to {1}", idRange.Item1, idRange.Item2);

      });
      }
    }
  }
}

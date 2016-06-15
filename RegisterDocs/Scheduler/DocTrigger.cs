using RegisterDocs.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs.Scheduler
{
  public static class DocTrigger
  {
    public static async Task Execute()
    {
      var ids = await new RegisterDocsDbContext().Docs.Where(w => w.IsActive == true).Select(s => s.Id).ToArrayAsync();
      var partitions = Partitioner.Create(0, ids.Length, 100);

      Console.WriteLine("Total {0} items", ids.Length);

      partitions.AsParallel()
        .WithDegreeOfParallelism(Environment.ProcessorCount)
        .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
        .ForAll(async idRange =>
      {
        var db = new RegisterDocsDbContext();

        for (var i = idRange.Item1; i < idRange.Item2; i++)
        {
          var id = ids[i];
          var doc = await db.Docs.FindAsync(id);

          doc.CalculateColourStatus();

          db.Entry(doc).State = EntityState.Modified;
        }

        await db.SaveChangesAsync();

        Console.WriteLine("Saved Items - {0} to {1}", idRange.Item1, idRange.Item2);

      });

    }
  }
}

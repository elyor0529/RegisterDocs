using LiteDB;
using RegisterDocs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs
{
  public class DocDatabase : LiteDatabase
  {

    private static readonly string _connectionString = Path.Combine(Environment.CurrentDirectory, "RegisterDoc.db");

    public DocDatabase() : base(_connectionString)
    {
      Docs = GetCollection<Doc>(typeof(Doc).Name);
    }

    public LiteCollection<Doc> Docs { get; set; }

    public LiteCollection<T> GetCollection<T>() where T : new()
    {
      var tableName = typeof(Doc).Name;

      return GetCollection<T>(tableName);
    }
  }
}

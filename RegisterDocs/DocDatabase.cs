using LiteDB;
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

    public DocDatabase(string connectionString) : base(connectionString)
    {
    }

    public DocDatabase() : this(_connectionString)
    {
    }

    public LiteCollection<T> GetCollection<T>() where T : new()
    {
      var name = typeof(T).Name;

      return GetCollection<T>(name);
    }
  }
}

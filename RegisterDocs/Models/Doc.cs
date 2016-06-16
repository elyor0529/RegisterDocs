
namespace RegisterDocs.Models
{
  using LiteDB;
  using System;
  using System.Collections.Generic;

  public class Doc
  {
    [BsonId]
    public int Id { get; set; }
    public string QayerdanKelibTushgan { get; set; }
    public DateTime? KelibTushganSana { get; set; }
    public string MurojaatMazmun { get; set; }
    public string IshtirokchiXodim { get; set; }
    public int? HalEtishMuddat { get; set; }
    public DateTime? MuddatiUzaytirilganSana { get; set; }
    public DateTime? TegishliBoychaOrganSana { get; set; }
    public int? TegishliBoychaOrganQolganKun { get; set; }
    public string TegishliBoychaOrganIdora { get; set; }
    public int? YuridikShaxsTur { get; set; }
    public int? JismoniyShaxsTur { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? BugungiSana { get; set; }
  }
}

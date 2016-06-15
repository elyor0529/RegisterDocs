using ClosedXML.Excel;
using RegisterDocs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs.Helpers
{
  public static class ExcelHelper
  {
    public static string Export(this IEnumerable<Docs> items)
    {
      var templateFile = String.Format("{0}\\Templates\\doc_template.xlsx", Environment.CurrentDirectory);
      var exportPath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), ".xlsx"));

      using (var wb = new XLWorkbook(templateFile))
      {
        var ws = wb.Worksheet(1);
        var lastRowIndex = 3;
        var titlesStyle = wb.Style;
        titlesStyle.Font.Bold = true;
        titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        titlesStyle.Fill.BackgroundColor = XLColor.Green;

        for (var i = 0; i < items.Count(); i++)
        {
          var item = items.ElementAt(i);

          ws.Cell(lastRowIndex + i, 1).Value = i + 1;
          ws.Cell(lastRowIndex + i, 2).Value = item.QayerdanKelibTushgan;
          ws.Cell(lastRowIndex + i, 3).Value = item.KelibTushganSana?.ToLongDateString();
          ws.Cell(lastRowIndex + i, 4).Value = item.JismoniyShaxsTur;
          ws.Cell(lastRowIndex + i, 5).Value = item.YuridikShaxsTur;
          ws.Cell(lastRowIndex + i, 6).Value = item.MurojaatMazmun;
          ws.Cell(lastRowIndex + i, 7).Value = item.IshtirokchiXodim;
          ws.Cell(lastRowIndex + i, 8).Value = item.BugungiSana?.ToLongDateString();
          ws.Cell(lastRowIndex + i, 9).Value = item.HalEtishMuddat;
          ws.Cell(lastRowIndex + i, 10).Value = item.MuddatiUzaytirilganSana?.ToLongDateString();
          ws.Cell(lastRowIndex + i, 11).Value = item.TegishliBoychaOrganSana?.ToLongDateString();
          ws.Cell(lastRowIndex + i, 12).Value = item.TegishliBoychaOrganQolganKun;
          ws.Cell(lastRowIndex + i, 13).Value = item.TegishliBoychaOrganIdora;

        }

        wb.SaveAs(exportPath);
      }

      return exportPath;
    }
  }
}

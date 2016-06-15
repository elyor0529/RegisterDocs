using RegisterDocs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs.GUI.ViewModels
{
  public class FilterViewModel
  {

    public bool? IsActive { get; set; }

    public DocColours? Colour { get; set; }

    public string KelibTushgan { get; set; }

  }
}

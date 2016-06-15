using RegisterDocs.Enums;
using RegisterDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDocs
{
  public static class Extensions
  {

    public static DocColours GetColour(this int? number)
    {
      if (number < 5)
        return DocColours.Red;

      if (number >= 5 && number < 10)
        return DocColours.Yellow;

      if (number >= 10 && number < 15)
        return DocColours.Green;

      return DocColours.White;
    }

    public static bool CheckColour(this DocColours? colour, int? number)
    {
      switch (colour)
      {
        case DocColours.White:
          return number >= 15;

        case DocColours.Green:
          return number >= 10 && number < 15;

        case DocColours.Yellow:
          return number >= 5 && number < 10;

        case DocColours.Red:
          return number < 5;
      }

      return false;
    }

    public static void CalculateColourStatus(this Docs doc)
    {
      //calculate
      doc.TegishliBoychaOrganQolganKun = (doc.MuddatiUzaytirilganSana - doc.TegishliBoychaOrganSana)?.Days;
      doc.HalEtishMuddat = (doc.KelibTushganSana - doc.BugungiSana)?.Days;

      if (doc.TegishliBoychaOrganQolganKun == 0 &&
        doc.HalEtishMuddat == 0)
      {
        doc.IsActive = false;
      }

    }

    #region Value convert functions 

    /// <summary>
    /// Converts this value to int
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns Nullable&lt;int&gt;</returns>
    public static int? AsInt(this string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        int k;
        if (int.TryParse(value, out k))
        {
          return k;
        }
      }

      return default(int?);
    }

    /// <summary>
    /// Converts this value to double
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns Nullable&lt;double&gt;</returns>
    public static double? AsDouble(this string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        double k;
        if (double.TryParse(value, out k))
        {
          return k;
        }
      }

      return default(double?);
    }

    /// <summary>
    /// Converts this value to decimal
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns Nullable&lt;decimal&gt;</returns>
    public static decimal? AsDecimal(this string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        decimal k;
        if (decimal.TryParse(value, out k))
        {
          return k;
        }
      }

      return default(decimal?);
    }

    /// <summary>
    /// Converts this value to DateTime
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns Nullable&lt;DateTime&gt;</returns>
    public static DateTime? AsDateTime(this string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        DateTime d;
        if (DateTime.TryParse(value, out d))
        {
          return d;
        }
      }

      return default(DateTime?);
    }

    public static int[] AsInt(this string[] values)
    {
      if (values != null && values.Length > 0)
      {
        int[] result = new int[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
          int r = 0;
          if (int.TryParse(values[i], out r)) result[i] = r;
        }
        return result;
      }
      return new int[0];
    }
    #endregion
  }
}

using System.Collections.Generic;
using System.Drawing;
using System;

namespace Illallangi.Cartography
{
public static class GeoLineExtensionMethods
{
  public static IEnumerable<KeyValuePair<Point, Point>> GetLine(this IEnumerable<GeoLine> g, Bitmap bitmap)
  {
    IEnumerator<GeoLine> enumerator = g.GetEnumerator();
    try
    {
      while (true)
      {
      Label0:
        bool flag = enumerator.MoveNext();
        if (!flag)
        {
          break;
        }
        GeoLine current = enumerator.Current;
        IEnumerator<KeyValuePair<Point, Point>> enumerator1 = current.GetLine(bitmap).GetEnumerator();
        try
        {
          while (true)
          {
            flag = enumerator1.MoveNext();
            if (!flag)
            {
              goto Label0;
            }
            KeyValuePair<Point, Point> keyValuePair = enumerator1.Current;
            yield return keyValuePair;
          }
        }
        finally
        {
          bool flag1 = enumerator1 == null;
          if (!flag1)
          {
            enumerator1.Dispose();
          }
        }
      }
    }
    finally
    {
      bool flag2 = enumerator == null;
      if (!flag2)
      {
        enumerator.Dispose();
      }
    }
  }
}
}
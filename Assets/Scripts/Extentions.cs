using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
  public static bool IsEven(this int v) => v % 2 == 0;

  public static Vector2Int RandomPoint(this RectInt r) => new Vector2Int(UnityEngine.Random.Range(r.xMin, r.xMax), UnityEngine.Random.Range(r.yMin, r.yMax));
  public static Vector2Int Round(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

  public static List<Vector3Int> Neighbors(this Vector3Int v)
  {
    var result = new List<Vector3Int>();
    result.Add(new Vector3Int(v.x - 1, v.y, v.z + 1));
    result.Add(new Vector3Int(v.x + 1, v.y, v.z + 1));
    result.Add(new Vector3Int(v.x, v.y - 1, v.z + 1));
    result.Add(new Vector3Int(v.x, v.y + 1, v.z + 1));
    return result;
  }

  public static T Random<T>(this List<T> l) => l.Count > 0 ? l[UnityEngine.Random.Range(0, l.Count)] : default;

  public static int Random(this RangeInt r) => UnityEngine.Random.Range(r.Min, r.Max + 1);

  public static float ToAngle(this Vector2 v) => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
  public static float ToAngle(this Vector3 v) => ((Vector2)v).ToAngle();

  public static Vector2 ToVector2(this float deg) => new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
}
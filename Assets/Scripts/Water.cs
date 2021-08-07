using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{
  [SerializeField] private List<SpriteWithMask> _sprites;

  private SpriteRenderer _sr;

  private void Awake()
  {
    _sr = GetComponent<SpriteRenderer>();
    for (int i = 0; i < _sprites.Count; i++)
      _sprites[i].BMask = BMaskFromStringMask(_sprites[i].Mask);
  }

  private byte BMaskFromStringMask(string mask)
  {
    byte res = 0;
    for (int i = 0; i < 8; i++)
      if (mask[i < 4 ? i : i + 1] == 'w') res |= (byte)(1 << i);
    // Debug.Log($"> {mask} -> [{Convert.ToString(res, 2)}]");
    return res;
  }

  public void SelectSpriteByMask(byte mask)
  {
    var swm = _sprites.Where(s => s.BMask == mask).FirstOrDefault();
    // Debug.Log($" {Convert.ToString(mask, 2)} -> {swm != null}");
    if (swm != null)
      _sr.sprite = swm.Sprite;
  }
}

[Serializable]
public class SpriteWithMask
{
  public Sprite Sprite;
  public String Mask;
  private byte _mask;

  public byte BMask
  {
    get => _mask;
    set => _mask = value;
  }
}

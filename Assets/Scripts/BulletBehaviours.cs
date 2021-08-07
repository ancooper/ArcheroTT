using System;

[Serializable]
public struct BulletBehaviours
{
  public bool Through;
  public bool TurnBack;
  public bool OverTheStone;
  public bool Ricochet;
  public bool WallBounce;

  public static BulletBehaviours Combine(BulletBehaviours a, BulletBehaviours b)
  {
    var result = new BulletBehaviours();
    result.Through = a.Through || b.Through;
    result.TurnBack = a.TurnBack || b.TurnBack;
    result.OverTheStone = a.OverTheStone || b.OverTheStone;
    result.Ricochet = a.Ricochet || b.Ricochet;
    result.WallBounce = a.WallBounce || b.WallBounce;
    return result;
  }
}
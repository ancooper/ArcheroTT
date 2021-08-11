using System;

[Serializable, Flags]
public enum BulletBehaviours
{
  None = 0,
  Through = 1,
  TurnBack = 2,
  OverTheStone = 4,
  Ricochet = 8,
  WallBounce = 16,
}

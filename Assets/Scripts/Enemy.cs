using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Unit))]
public class Enemy : Character
{
  [Header("Enemy settings")]
  [SerializeField] private float _moveTime;
  [SerializeField] private float _aimTime;
  [SerializeField] private float _attackTime;
  [SerializeField] private float _rotateSpeed;
  [SerializeField] private float _contactDamage;

  [Header("Drops by destroy")]
  [SerializeField] private RangeInt _coinsRange;

  public float ContactDamage => _contactDamage;

  public float MoveTime => _moveTime;
  public float AimTime => _aimTime;
  public float AttackTime => _attackTime;
  public float RotateSpeed => _rotateSpeed;
  public int AmountDroppedCoins => _coinsRange.Random();

  protected override void Dead(Unit unit)
  {
    _field.EnemyDead(this);
  }
}

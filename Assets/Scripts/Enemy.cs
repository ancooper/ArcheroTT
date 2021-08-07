using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Unit))]
public class Enemy : MonoBehaviour
{
  [Header("Enemy settings")]
  [SerializeField] private float _moveTime;
  [SerializeField] private float _aimTime;
  [SerializeField] private float _attackTime;
  [SerializeField] private float _rotateSpeed;
  [SerializeField] private float _contactDamage;

  [Header("Drops by destroy")]
  [SerializeField] private RangeInt _coinsRange;

  private Field _field;

  public int AmountDroppedCoins => _coinsRange.Random();
  public float ContactDamage => _contactDamage;

  public float MoveTime => _moveTime;
  public float AimTime => _aimTime;
  public float AttackTime => _attackTime;
  public float RotateSpeed => _rotateSpeed;
  public Field Field => _field;

  public void Init(Field field, Vector3 position)
  {
    _field = field;
    transform.localPosition = position;
  }
}

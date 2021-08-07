using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Weapon : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private WeaponData _weaponData;

  [SerializeField] private Bullet _bulletPrefab;

  private Unit _unit;
  private float _coolDown;

  public float CoolDown => _coolDown;

  private void Awake()
  {
    _unit = GetComponent<Unit>();
  }

  private void Update()
  {
    _coolDown -= Time.deltaTime;
  }

  public void Shoot()
  {
    if (_unit.Weapon.CoolDown > 0)
      return;

    _coolDown = 1f / (_weaponData.AttackRate * _unit.AttackRateMultiplier);

    var angleOffset = (1f - _weaponData.Accuracy) * 30f;
    var speedOffset = (1f - _weaponData.Accuracy) * _weaponData.Speed * 0.3f;
    var direction = (_unit.ViewAngle + Random.Range(-angleOffset, angleOffset)).ToVector2();
    var speed = _weaponData.Speed + Random.Range(-speedOffset, speedOffset);

    var bullet = Instantiate(_bulletPrefab, _unit.transform.parent, false);
    bullet.Init(
      _unit.IsPlayer,
      (Vector2)_unit.Head.position + direction * 0.5f,
      direction * speed,
      _weaponData.Damage * _unit.DamageMultiplier,
      BulletBehaviours.Combine(_weaponData.BulletBehaviours, _unit.BulletBehaviours)
    );
  }
}

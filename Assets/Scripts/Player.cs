using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
  private Rigidbody2D _rb;
  private Vector2 _direction;
  private bool _isStoped;

  protected override void Awake()
  {
    base.Awake();
    _rb = GetComponent<Rigidbody2D>();
    _direction = Vector2.zero;
    _isStoped = true;
  }

  public void Move(Vector2 direction)
  {
    _direction = direction.normalized;
    _isStoped = _direction.SqrMagnitude() < Mathf.Epsilon;
  }

  private void Update()
  {
    if (!_unit.Active)
      return;

    if (_unit.IsALive)
    {
      SearchTargetWhenStoped();
      SetViewDirection();

      if (_isStoped && _unit.Target != null && _unit.Weapon.CoolDown <= 0)
        _unit.Weapon.Shoot();

      _rb.drag = _isStoped ? 100f : 0f;
      _rb.velocity = _direction * _unit.Speed;
    }
  }

  private void SearchTargetWhenStoped()
  {
    if (_isStoped && _unit.Target == null)
    {
      var enemies = FindObjectsOfType<Enemy>();
      if (enemies.Length != 0)
        _unit.Target = enemies.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First().GetComponent<Unit>();
    }

    if (!_isStoped)
      _unit.Target = null;
  }

  private void SetViewDirection()
  {
    if (_isStoped && _unit.Target != null)
      _unit.ViewAngle = (_unit.Target.transform.position - transform.position).ToAngle();

    if (!_isStoped)
      _unit.ViewAngle = _direction.ToAngle();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    var enemy = other.gameObject.GetComponent<Enemy>();
    if (enemy != null)
      _unit.Damage(enemy.ContactDamage, false);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Spike>(out Spike spike))
      _unit.Damage(spike.Damage, false);
  }

  protected override void Dead(Unit unit)
  {
    _field.PlayerDead();
  }
}

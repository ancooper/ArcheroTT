using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
  private Rigidbody2D _rb;
  private Unit _owner;
  private float _damage;
  private bool _crit;
  private BulletBehaviours _bulletBehaviours;
  private float _speed;
  private float _lifeTime;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  public void Init(Unit owner, Vector2 position, Vector2 velocity, float damage, bool crit, BulletBehaviours bb, float lifeTime)
  {
    _bulletBehaviours = bb;
    _owner = owner;
    transform.position = position;
    _rb.velocity = velocity;
    _speed = velocity.magnitude;
    _damage = damage;
    _crit = crit;
    _lifeTime = lifeTime;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Unit>(out Unit unit))
      OnTriggerWithUnit(unit);

    if (other.TryGetComponent<Wall>(out Wall wall))
      OnTriggerWithWall();

    if (other.TryGetComponent<Stone>(out Stone stone))
      OnTriggerWithStone();
  }

  private void OnTriggerWithUnit(Unit unit)
  {
    if (_owner.IsPlayer != unit.IsPlayer)
    {
      unit.Damage(_damage, _crit);

      if ((_bulletBehaviours & BulletBehaviours.Through) == 0)
        Destroy(gameObject);
    }
  }

  private void OnTriggerWithWall()
  {
    if (!TryRicochet() && !TryTurnBack())
      Destroy(gameObject);
  }

  private void OnTriggerWithStone()
  {
    if ((BulletBehaviours.OverTheStone & _bulletBehaviours) == 0 && !TryRicochet() && !TryTurnBack())
      Destroy(gameObject);
  }

  private bool TryRicochet()
  {
    return (BulletBehaviours.Ricochet & _bulletBehaviours) != 0;
    // TODO: Ricochet bullet
  }

  private bool TryTurnBack()
  {
    var turnBack = (BulletBehaviours.TurnBack & _bulletBehaviours) != 0;
    if (turnBack)
    {
      var vectorToOwner = _owner.transform.position - transform.position;
      _lifeTime = vectorToOwner.magnitude / _speed;
      _rb.velocity = vectorToOwner.normalized * _speed;
      _bulletBehaviours |= BulletBehaviours.OverTheStone;
    }
    return turnBack;
  }

  private void Update()
  {
    LifeTimeControl();
  }

  private void LifeTimeControl()
  {
    _lifeTime -= Time.deltaTime;
    if (_lifeTime < 0)
      Destroy(gameObject);
  }
}

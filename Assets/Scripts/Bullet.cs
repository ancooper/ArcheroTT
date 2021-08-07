using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

  private Rigidbody2D _rb;
  private float _damage;
  private bool _crit;
  private BulletBehaviours _bulletBehaviours;
  private bool _turnBack;
  private bool _fromPlayer;
  private float _speed;

  private void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  public void Init(bool fromPlayer, Vector2 position, Vector2 velocity, float damage, bool crit, BulletBehaviours bb)
  {
    _bulletBehaviours = bb;
    transform.position = position;
    _rb.velocity = velocity;
    _speed = velocity.magnitude;
    _damage = damage;
    _crit = crit;
    _turnBack = false;
    _fromPlayer = fromPlayer;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Enemy>(out Enemy enemy))
    {
      if (_fromPlayer)
        enemy.GetComponent<Unit>().Damage(_damage, _crit);
      if ((_fromPlayer && !_bulletBehaviours.Through) || (!_fromPlayer && _turnBack))
      {
        Destroy(gameObject);
        return;
      }
    }

    if (other.TryGetComponent<Player>(out Player player))
    {
      if (!_fromPlayer)
        player.GetComponent<Unit>().Damage(_damage, _crit);
      if ((!_fromPlayer && !_bulletBehaviours.Through) || (_fromPlayer && _turnBack))
      {
        Destroy(gameObject);
        return;
      }
    }

    if (other.TryGetComponent<Wall>(out Wall wall))
    {
      if (!TryRicochet())
        if (!TryTurnBack())
        {
          Destroy(gameObject);
          return;
        }
    }

    if (other.TryGetComponent<Stone>(out Stone stone))
    {
      if (!_bulletBehaviours.OverTheStone)
        if (!TryRicochet())
          if (!TryTurnBack())
          {
            Destroy(gameObject);
            return;
          }
    }
  }

  private bool TryRicochet()
  {
    if (!_bulletBehaviours.Ricochet)
    {
      return false;
    }
    else
    {
      // TODO: Ricochet bullet
      return true;
    }
  }

  private bool TryTurnBack()
  {
    if (!_bulletBehaviours.TurnBack)
    {
      return false;
    }
    else
    {
      _turnBack = true;
      _bulletBehaviours.OverTheStone = true;
      return true;
    }
  }

  private void Update()
  {
    if (_turnBack)
      _rb.velocity = (FindObjectOfType<Player>().transform.position - transform.position).normalized * _speed;
  }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Unit)), RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
  protected Enemy _enemy;
  protected Unit _unit;
  protected Rigidbody2D _rb;
  private Phase _phase;
  private float _phaseTime;
  private float _rotateSpeed;
  protected bool _needAiming;
  protected bool _needMoveTarget;
  protected Vector2 _targetPosition;
  private float _currentAngle;

  private void Awake()
  {
    _enemy = GetComponent<Enemy>();
    _unit = GetComponent<Unit>();
    _rb = GetComponent<Rigidbody2D>();

    InitPhase();
  }

  private void InitPhase()
  {
    _phase = Phase.Aim;
    _needAiming = true;
    _phaseTime = PhaseLengthWithRandom(_phase);
  }

  private void Update()
  {
    if (!_unit.Active)
      return;

    if (!_unit.IsALive)
      return;

    _phaseTime -= Time.deltaTime;
    if (_phaseTime <= 0)
      ChangePhase();

    switch (_phase)
    {
      case Phase.Aim:
        Aiming();
        break;
      case Phase.Attack:
        Shooting();
        break;
      case Phase.Move:
        Moving();
        break;
    }
  }

  protected bool RotateTo(Vector2 direction)
  {
    var deltaAngle = _enemy.RotateSpeed * Time.deltaTime;
    var angle = Vector2.SignedAngle(_unit.ViewAngle.ToVector2(), direction);

    if (Mathf.Abs(angle) < deltaAngle)
    {
      _unit.ViewAngle += angle;
      return true;
    }

    _unit.ViewAngle += Mathf.Sign(angle) * deltaAngle;
    return false;
  }

  protected virtual void Aiming()
  {
  }

  protected virtual void Shooting()
  {
    _unit.Weapon.Shoot();
  }

  protected virtual void Moving()
  {
  }

  private void ChangePhase()
  {
    if (_phase == Phase.Attack)
    {
      _phase = Phase.Move;
      _needMoveTarget = true;
    }
    else if (_phase == Phase.Move)
    {
      _phase = Phase.Aim;
      _needAiming = true;
    }
    else
      _phase = Phase.Attack;

    _rb.drag = _phase == Phase.Move ? 0f : 100f;
    _phaseTime += PhaseLengthWithRandom(_phase);
  }

  private float PhaseLengthWithRandom(Phase phase)
  {
    float result;
    switch (phase)
    {
      case Phase.Move:
        result = _enemy.MoveTime;
        break;
      case Phase.Aim:
        result = _enemy.AimTime;
        break;
      case Phase.Attack:
      default:
        result = _enemy.AttackTime;
        break;
    }
    return result * Random.Range(0.8f, 1.2f);
  }

  protected void FindPlayer()
  {
    var player = FindObjectOfType<Player>();
    if (player != null && player.GetComponent<Unit>().IsALive)
      _targetPosition = player.transform.localPosition;
    else
      _unit.SetActive(false);
  }

  enum Phase { Move, Aim, Attack }
}


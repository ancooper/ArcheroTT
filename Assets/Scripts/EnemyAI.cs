using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit)), RequireComponent(typeof(Enemy))]
public abstract class EnemyAI : MonoBehaviour
{
  protected EnemyAIContext _context;
  protected Enemy _enemy;
  protected Unit _unit;

  private void Awake()
  {
    _enemy = GetComponent<Enemy>();
    _unit = GetComponent<Unit>();

    Init();
  }

  protected abstract void Init();

  private void Update()
  {
    if (!_unit.Active)
      return;

    if (!_unit.IsALive)
      return;

    _context.LifeTime();
    if (_context.TimeIsOver)
      _context.ChangePhaseToNext();
    _context.Update();
  }
}

public class EnemyAIContext
{
  private Dictionary<System.Type, float> _lifeTimesByPhase;
  private EnemyPhase _phase = null;
  private Unit _unit;
  private Enemy _enemy;

  public Unit Unit => _unit;
  public Enemy Enemy => _enemy;

  public bool TimeIsOver => _phase.Time < 0;

  public EnemyAIContext(Unit unit, Enemy enemy, Dictionary<System.Type, float> lifeTimesByPhase, EnemyPhase startPhase)
  {
    _lifeTimesByPhase = lifeTimesByPhase;
    _unit = unit;
    _enemy = enemy;
    TransitionTo(startPhase);
  }

  public void TransitionTo(EnemyPhase phase)
  {
    _phase = phase;
    _phase.SetContext(this);
    _phase.SetLifeTime(_lifeTimesByPhase[phase.GetType()]);
  }

  public void ChangePhaseToNext() => _phase.NextPhase();
  public void Update() => _phase.Update();
  public void LifeTime() => _phase.LifeTime();

  public Vector2 DirectionToPlayer()
  {
    var player = Object.FindObjectOfType<Player>();
    if (player != null && player.GetComponent<Unit>().IsALive)
      return player.transform.position - _unit.transform.position;
    else
      return Vector2.zero;
  }

  public bool RotateTo(Vector2 direction)
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
}

public abstract class EnemyPhase
{
  protected EnemyAIContext _context;
  private float _time;

  public float Time => _time;

  public void SetContext(EnemyAIContext context) => _context = context;
  public void SetLifeTime(float time) => _time = time;

  public abstract void NextPhase();
  public abstract void Update();

  public void LifeTime() => _time -= UnityEngine.Time.deltaTime;
}

public class EnemyAimPhase : EnemyPhase
{
  private bool _needAiming = true;
  private Vector2 _direction;

  public override void NextPhase() => _context.TransitionTo(new EnemyAttackPhase());

  public override void Update()
  {
    if (_needAiming)
    {
      _context.Unit.SetVelocity(Vector2.zero);
      _direction = _context.DirectionToPlayer();
      _needAiming = false;
    }

    _context.RotateTo(_direction);
  }
}

public class EnemyAttackPhase : EnemyPhase
{
  public override void NextPhase() => _context.TransitionTo(new EnemyMovePhase());
  public override void Update() => _context.Unit.Weapon.Shoot();
}

public class EnemyMovePhase : EnemyPhase
{
  public override void NextPhase() => _context.TransitionTo(new EnemyAimPhase());
  public override void Update() { }
}

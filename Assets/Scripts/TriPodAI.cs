using System.Collections.Generic;
using UnityEngine;

public class TriPodAI : EnemyAI
{
  protected override void Init()
  {
    var lifeTimesByPhase = new Dictionary<System.Type, float>(){
      {typeof(TriPodAimPhase), _enemy.AimTime * Random.Range(0.8f, 1.2f)},
      {typeof(TriPodAttackPhase), _enemy.AttackTime * Random.Range(0.8f, 1.2f)},
      {typeof(TriPodMovePhase), _enemy.MoveTime * Random.Range(0.8f, 1.2f)},
    };
    _context = new EnemyAIContext(_unit, _enemy, lifeTimesByPhase, new TriPodAimPhase());
  }
}

public class TriPodAimPhase : EnemyAimPhase
{
  public override void NextPhase() => _context.TransitionTo(new TriPodAttackPhase());
}

public class TriPodAttackPhase : EnemyAttackPhase
{
  public override void NextPhase() => _context.TransitionTo(new TriPodMovePhase());
}

public class TriPodMovePhase : EnemyMovePhase
{
  private bool _needMoveTarget = true;
  private Vector2 _point;
  private Vector2 _direction;
  private Queue<Vector2> _path;

  public override void NextPhase() => _context.TransitionTo(new TriPodAimPhase());

  public override void Update()
  {
    if (_needMoveTarget)
    {
      _direction = _context.DirectionToPlayer();

      _path = _context.Enemy.Field.FindPathOnGround((Vector2)_context.Unit.transform.localPosition, (Vector2)_context.Unit.transform.localPosition + _direction);
      _path.Dequeue();
      _point = _path.Count > 0 ? _path.Dequeue() : (Vector2)_context.Unit.transform.localPosition;
      _needMoveTarget = false;
    }

    var dir = _point - (Vector2)_context.Unit.transform.localPosition;
    if (dir.magnitude < 0.1f && _path.Count > 0)
    {
      _point = _path.Dequeue();
      dir = _point - (Vector2)_context.Unit.transform.localPosition;
    }

    _context.Unit.ViewAngle = dir.ToAngle();
    _context.Unit.SetVelocity(dir.magnitude > 0.1f ? _context.Unit.ViewAngle.ToVector2() * _context.Unit.Speed : Vector2.zero);
  }
}

using System.Collections.Generic;
using UnityEngine;

public class ZuBatAI : EnemyAI
{
  protected override void Init()
  {
    var lifeTimesByPhase = new Dictionary<System.Type, float>(){
      {typeof(ZuBatAimPhase), _enemy.AimTime * Random.Range(0.8f, 1.2f)},
      {typeof(ZuBatAttackPhase), _enemy.AttackTime * Random.Range(0.8f, 1.2f)},
      {typeof(ZuBatMovePhase), _enemy.MoveTime * Random.Range(0.8f, 1.2f)},
    };
    _context = new EnemyAIContext(_unit, _enemy, lifeTimesByPhase, new ZuBatAimPhase());
  }
}

public class ZuBatAimPhase : EnemyAimPhase
{
  public override void NextPhase() => _context.TransitionTo(new ZuBatAttackPhase());
}

public class ZuBatAttackPhase : EnemyAttackPhase
{
  public override void NextPhase() => _context.TransitionTo(new ZuBatMovePhase());
}

public class ZuBatMovePhase : EnemyMovePhase
{
  private bool _needMoveTarget = true;
  private bool _needRotation;
  private Vector2 _direction;

  public override void NextPhase() => _context.TransitionTo(new ZuBatAimPhase());
  public override void Update()
  {
    var localPosition = (Vector2)_context.Unit.transform.localPosition;
    if (_needMoveTarget)
    {
      _direction = _context.DirectionToPlayer();

      var strafePoints = new List<Vector2>();

      var point = PointByAngleFromDirection(localPosition, _direction, 60f, 3f);
      if (_context.Enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, -60f, 3f);
      if (_context.Enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, 120f, 3f);
      if (_context.Enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, -120f, 3f);
      if (_context.Enemy.Field.PointInField(point)) strafePoints.Add(point);

      _direction = strafePoints.Count > 0 ? strafePoints.Random() - localPosition : Vector2.zero;
      _needMoveTarget = false;
      _needRotation = true;
    }
    else
    {

      if (_needRotation)
      {
        if (_context.RotateTo(_direction))
          _needRotation = false;
      }
      else
      {
        _context.Unit.SetVelocity(_direction.magnitude > 0.1f ? _context.Unit.ViewAngle.ToVector2() * _context.Unit.Speed : Vector2.zero);
      }
    }
  }

  private Vector2 PointByAngleFromDirection(Vector2 localPosition, Vector2 direction, float angle, float distance) => localPosition + (direction.ToAngle() + angle).ToVector2() * distance;
}

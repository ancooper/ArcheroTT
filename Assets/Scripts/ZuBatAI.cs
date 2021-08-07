using System.Collections.Generic;
using UnityEngine;

public class ZuBatAI : EnemyAI
{
  private bool _needRotation;
  private Vector2 _direction;

  protected override void Aiming()
  {
    if (_needAiming)
    {
      FindPlayer();
      _needAiming = false;
    }

    var direction = (_targetPosition - (Vector2)transform.localPosition).normalized;
    RotateTo(direction);
  }

  protected override void Moving()
  {
    var localPosition = (Vector2)transform.localPosition;
    if (_needMoveTarget)
    {
      FindPlayer();
      _direction = _targetPosition - localPosition;

      var strafePoints = new List<Vector2>();

      var point = PointByAngleFromDirection(localPosition, _direction, 60f, 3f);
      if (_enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, -60f, 3f);
      if (_enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, 120f, 3f);
      if (_enemy.Field.PointInField(point)) strafePoints.Add(point);
      point = PointByAngleFromDirection(localPosition, _direction, -120f, 3f);
      if (_enemy.Field.PointInField(point)) strafePoints.Add(point);

      _targetPosition = strafePoints.Count > 0 ? strafePoints.Random() : localPosition;
      _needMoveTarget = false;
      _needRotation = true;
    }

    _direction = (_targetPosition - (Vector2)transform.localPosition);
    if (_needRotation)
    {
      if (RotateTo(_direction))
        _needRotation = false;
    }
    else
    {
      if (_direction.magnitude > 0.1f)
      {
        _rb.drag = 0f;
        _rb.velocity = _unit.ViewAngle.ToVector2() * _unit.Speed;
      }
      else
        _rb.drag = 100f;
    }
  }

  private Vector2 PointByAngleFromDirection(Vector2 localPosition, Vector2 direction, float angle, float distance) => localPosition + (direction.ToAngle() + angle).ToVector2() * distance;
}

using System.Collections.Generic;
using UnityEngine;

public class TriPodAI : EnemyAI
{
  private Queue<Vector2> _path;
  private Vector2 _point;

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
    if (_needMoveTarget)
    {
      FindPlayer();
      _path = _enemy.Field.FindPathOnGround(transform.localPosition, _targetPosition);
      _path.Dequeue();
      _point = _path.Count > 0 ? _path.Dequeue() : (Vector2)transform.localPosition;
      _needMoveTarget = false;
    }

    var dir = _point - (Vector2)transform.localPosition;
    if (dir.magnitude < 0.1f && _path.Count > 0)
    {
      _point = _path.Dequeue();
      dir = _point - (Vector2)transform.localPosition;
    }

    _unit.ViewAngle = dir.ToAngle();
    if (dir.magnitude > 0.1f)
    {
      _rb.drag = 0f;
      _rb.velocity = _unit.ViewAngle.ToVector2() * _unit.Speed;
    }
    else
      _rb.drag = 100f;
  }
}

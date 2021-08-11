using UnityEngine;

public abstract class Character : MonoBehaviour
{
  protected Unit _unit;
  protected Field _field;

  public Field Field => _field;

  protected virtual void Awake()
  {
    _unit = GetComponent<Unit>();
  }

  public void Init(Field field, Vector2 position)
  {
    _field = field;
    transform.localPosition = position;
  }

  private void OnEnable()
  {
    _unit.OnDead += Dead;
  }

  private void OnDisable()
  {
    _unit.OnDead -= Dead;
  }

  protected abstract void Dead(Unit unit);
}

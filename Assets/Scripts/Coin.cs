using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Coin : MonoBehaviour
{
  [SerializeField] private float _speed = 2.5f;

  private Rigidbody2D _rb;
  private Transform _target;
  private float _flySpeed = 10f;
  private bool _spawningComplete;

  private void Awake() => _rb = GetComponent<Rigidbody2D>();

  public void Init(Vector2 position)
  {
    transform.localPosition = position;
    ApplyRandomImpulse();
  }

  private void ApplyRandomImpulse()
  {
    _rb.AddForce(Random.Range(0f, 360f).ToVector2() * Random.Range(0.1f, 0.3f), ForceMode2D.Impulse);
    _spawningComplete = false;
  }

  public void FlyTo(Transform transform)
  {
    GetComponent<CircleCollider2D>().isTrigger = true;
    _target = transform;
  }

  private void Update()
  {
    if (!_spawningComplete && _rb.velocity.magnitude < 0.1f)
      _spawningComplete = true;

    if (_target != null && _spawningComplete)
    {
      var dir = ((Vector2)_target.position - _rb.position).normalized;
      _rb.AddForce(dir * _speed);

      if (Vector2.Distance(_rb.position, _target.position) < 0.5f)
        Destroy(gameObject);
    }
  }
}

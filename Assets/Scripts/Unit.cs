using System;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class Unit : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float _speed = 3f;
  [SerializeField] private float _health = 1000f;

  [Header("Weapon")]
  [SerializeField] private float _attackRateMultiplier = 1f;
  [SerializeField] private float _damageMultplier = 1f;
  [SerializeField] private float _critProbability = 0f;

  [Header("Bullet Behaviours")]
  [SerializeField] private BulletBehaviours _bulletBehaviours;

  [Space]
  [SerializeField] private HealthBar _healthBar;
  [SerializeField] private DamagePresenter _damagePresenterPrefab;
  [SerializeField] private Transform _head;

  private bool _active;
  private Weapon _weapon;
  private bool _isPlayer;
  private float _maxHealth;

  public bool Active => _active;
  public float Speed => _speed;
  public bool IsALive => _health > 0;
  public float AttackRateMultiplier => _attackRateMultiplier;
  public float DamageMultiplier => _damageMultplier;
  public float CritProbability => _critProbability;
  public BulletBehaviours BulletBehaviours => _bulletBehaviours;
  public Weapon Weapon => _weapon;
  public bool IsPlayer => _isPlayer;
  public Transform Head => _head;

  public float ViewAngle { get; set; }
  public Unit Target { get; set; }

  [HideInInspector] public event Action<Unit> OnDead;

  private void Awake()
  {
    _active = false;
    _weapon = GetComponent<Weapon>();
    _isPlayer = TryGetComponent<Player>(out Player _);
    _maxHealth = _health;
    _healthBar.Init(_maxHealth, _health, _isPlayer ? new Color(0.39f, 0.78f, 0.3f) : new Color(0.97f, 0.47f, 0.13f));

    ViewAngle = _isPlayer ? 90 : 270;
    if (_head == null)
      _head = transform;
    _head.eulerAngles = new Vector3(0, 0, ViewAngle);
  }

  public void Damage(float damage, bool crit)
  {
    if (crit)
      damage *= 2f;
    _health -= damage;
    _healthBar.Health = _health;

    var dp = Instantiate(_damagePresenterPrefab, transform, false);
    dp.Init((int)damage, crit);

    if (!IsALive)
    {
      Destroy(gameObject, 0.01f);
      OnDead?.Invoke(this);
    }
  }

  private void Update()
  {
    if (!_active)
      return;

    _head.eulerAngles = new Vector3(0, 0, ViewAngle);
  }

  public void SetActive(bool active) => _active = active;
}

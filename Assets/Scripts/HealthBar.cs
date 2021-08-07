using UnityEngine;

[ExecuteAlways]
public class HealthBar : MonoBehaviour
{
  [Header("Values")]
  public float MaxHealth = 100;
  public float Health = 100;

  [Space]
  [SerializeField] private Transform _bar;
  [SerializeField] private Color _barColor;

  private SpriteRenderer _sr;
  private Color _lastColor;
  private float _lastMaxHealth;
  private float _lastHealth;

  private void Awake()
  {
    _sr = _bar.GetComponent<SpriteRenderer>();
  }

  public void Init(float maxHealth, float health, Color color)
  {
    MaxHealth = maxHealth;
    Health = health;
    _barColor = color;
  }

  private void OnValidate()
  {
    if (MaxHealth < 1)
      MaxHealth = 1;
    if (Health < 0)
      Health = 0;
    if (Health > MaxHealth)
      Health = MaxHealth;
  }

  private void Update()
  {
    if (_lastColor != _barColor)
    {
      _lastColor = _barColor;
      _sr.color = _barColor;
    }

    if (_lastMaxHealth != MaxHealth || _lastHealth != Health)
    {
      _lastMaxHealth = MaxHealth;
      _lastHealth = Health;
      Resize();
    }
  }

  private void Resize()
  {
    var t = Health / MaxHealth;
    _bar.localPosition = new Vector2((t - 1f) * 0.5f, 0);
    _bar.localScale = new Vector2(t, 1);
  }

}

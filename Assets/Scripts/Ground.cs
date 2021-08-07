using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ground : MonoBehaviour
{
  private SpriteRenderer _sr;

  void Awake()
  {
    _sr = GetComponent<SpriteRenderer>();
  }

  void Start()
  {
    GroundChessStyle();
  }

  private void GroundChessStyle()
  {
    var lighter = ((int)transform.position.x + (int)transform.position.y).IsEven();

    var gNoise = Mathf.PerlinNoise(transform.position.x * 2.3f + (lighter ? 0.5f : 0), transform.position.y * 2.3f + (lighter ? 0.5f : 0));
    var rNoise = Mathf.PerlinNoise(transform.position.x * 2.3f + (lighter ? 0.6f : 0.1f), transform.position.y * 2.3f + (lighter ? 0.3f : -0.2f));

    _sr.color = new Color(_sr.color.r + (0.5f - rNoise) * 0.01f, _sr.color.g + (0.5f - gNoise) * 0.02f, _sr.color.b);

    if (lighter)
      _sr.color = Color.Lerp(_sr.color, Color.black, Random.Range(0.02f, 0.04f));
  }
}

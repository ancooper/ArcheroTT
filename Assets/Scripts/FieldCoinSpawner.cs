using UnityEngine;

public class FieldCoinSpawner : MonoBehaviour
{
  [SerializeField] private Coin _coinPrefab;

  private Field _field;

  private void Awake() => _field = GetComponent<Field>();

  public void Spawn(Vector2 position, int amount)
  {
    for (int c = 0; c < amount; c++)
    {
      var coin = Instantiate(_coinPrefab, transform, false);
      coin.Init(position);
    }
  }
}

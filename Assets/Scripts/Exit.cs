using UnityEngine;

public class Exit : MonoBehaviour
{
  [SerializeField] private GameManager _manager;

  public void Init(GameManager manager) => _manager = manager;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Player>(out Player player))
      _manager.PlayerInExit();
  }
}

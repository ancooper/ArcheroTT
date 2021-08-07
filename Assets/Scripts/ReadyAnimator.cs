using UnityEngine;

public class ReadyAnimator : MonoBehaviour
{
  [SerializeField] private GameManager _gameManager;

  private Animator _anim;

  private void Awake()
  {
    _anim = GetComponent<Animator>();
  }

  public void Play()
  {
    gameObject.SetActive(true);
    _anim.SetBool("Ready", true);
  }

  public void StartGame()
  {
    _anim.SetBool("Ready", false);
    gameObject.SetActive(false);

    _gameManager.PlayGame();
  }
}

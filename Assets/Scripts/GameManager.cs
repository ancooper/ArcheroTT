using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [Header("Game")]
  [SerializeField] private Field _field;
  [SerializeField] private FieldUnitSpawner _unitSpawner;
  [SerializeField] private FieldCoinSpawner _coinSpawner;

  [Header("UI Canvas")]
  [SerializeField] private Joystick _joystick;
  [SerializeField] private Animator _blackScreenAnimator;
  [SerializeField] private ReadyAnimator _readyPanelAnimator;
  [SerializeField] private Animator _victoryPanelAnimator;
  [SerializeField] private Animator _gameOverPanelAnimator;

  private Status _lastFieldStatus;

  private void Start()
  {
    _field.Create(this);
    _unitSpawner.Spawn();

    FadeIn();
    CountDownPlay();
  }

  private void FadeIn() => _blackScreenAnimator.SetBool("BlackScreen", false);
  private void FadeOut() => _blackScreenAnimator.SetBool("BlackScreen", true);
  private void CountDownPlay() => _readyPanelAnimator.Play();

  public void PlayGame()
  {
    _joystick.gameObject.SetActive(true);
    _field.Play();
  }

  public void PlayerInExit()
  {
    var player = FindObjectOfType<Player>();
    if (player != null && player.GetComponent<Unit>().IsALive)
    {
      _joystick.gameObject.SetActive(false);
      Destroy(player.gameObject);

      _field.Victory();
      _victoryPanelAnimator.gameObject.SetActive(true);
      StartCoroutine(RestartLevelAfterSeconds());
    }
  }

  public IEnumerator RestartLevelAfterSeconds()
  {
    yield return new WaitForSeconds(1f);
    FadeOut();
    yield return new WaitForSeconds(2f);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void PlayerDead()
  {
    _gameOverPanelAnimator.gameObject.SetActive(true);
    StartCoroutine(RestartLevelAfterSeconds());
  }
}

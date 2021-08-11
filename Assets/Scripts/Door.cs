using UnityEngine;

public class Door : MonoBehaviour
{
  [SerializeField] private GameObject _lock;
  [SerializeField] private GameObject _closedLeftDoor;
  [SerializeField] private GameObject _closedRightDoor;
  [SerializeField] private GameObject _openedLeftDoor;
  [SerializeField] private GameObject _openedRightDoor;

  [Space]
  [SerializeField] private Exit _exit;

  public void Init(GameManager manager)
  {
    _exit.Init(manager);
  }

  public void Open()
  {
    _lock.SetActive(false);
    _closedLeftDoor.SetActive(false);
    _closedRightDoor.SetActive(false);
    _openedLeftDoor.SetActive(true);
    _openedRightDoor.SetActive(true);
  }
}

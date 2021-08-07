using UnityEngine;

public class DamagePresenter : MonoBehaviour
{
  [SerializeField] private TMPro.TMP_Text _text;

  public void Init(int damage, bool crit)
  {
    _text.text = $"-{damage}{(crit ? "!" : "")}";
    _text.fontWeight = damage > 150 ? TMPro.FontWeight.Bold : TMPro.FontWeight.Regular;
    _text.color = crit ? Color.red : Color.white;
  }

  public void EndShow()
  {
    Destroy(gameObject);
  }
}

using UnityEngine;

public class Spike : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float _damage = 50;

  public float Damage => _damage;
}

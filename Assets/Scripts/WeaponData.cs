using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData/NewWeaponData", order = 2)]
public class WeaponData : ScriptableObject
{
  [Header("Weapon settings")]
  public string Name;
  public float Speed = 10f;
  public float AttackRate = 1f;
  public float Damage = 1f;
  public float Accuracy = 1f;

  [Header("Bullet Behaviours")]
  public BulletBehaviours BulletBehaviours;
}

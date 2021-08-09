using UnityEngine;

[CreateAssetMenu(fileName = "FieldData", menuName = "FieldData/NewFieldData", order = 1)]
public class FieldData : ScriptableObject
{
  [Header("Field Settings")]
  public string Name;
  public Vector2Int Size;

  [Header("Field Obsticles")]
  public RectInt[] WaterAreas;
  public RectInt[] StounAreas;
  public RectInt[] SpikeAreas;

  [Header("Spawn")]
  public Player PlayerPrefab;
  public RectInt PlayerSpawnArea;
  public EnemySpawnData[] Enemies;

  private void OnValidate()
  {
    if (Size.x < 3)
      Size.x = 3;
    if (Size.x.IsEven())
      Size.x++;
    if (Size.y < 3)
      Size.y = 3;
    if (Size.y.IsEven())
      Size.y++;
  }
}

using System;
using UnityEngine;

[Serializable]
public struct EnemySpawnData
{
  public Enemy EnemyPrefab;
  public int Amount;
  public RectInt SpawnArea;
}

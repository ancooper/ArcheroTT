using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Field))]
public class FieldUnitSpawner : MonoBehaviour
{
  private readonly Vector2Int INVALIDPOINT = new Vector2Int(int.MinValue, int.MinValue);

  private Field _field;
  private HashSet<Vector2Int> _usedPoints;

  private void Awake()
  {
    _field = GetComponent<Field>();
  }

  public void Spawn()
  {
    SpawnPlayer();
    SpawnEnemy();

    _field.Ready();
  }

  private void SpawnPlayer()
  {
    TrySpawnEntity(_field.Room, _field.Data.PlayerSpawnArea, _field.Data.PlayerPrefab);
  }

  private void SpawnEnemy()
  {
    _usedPoints = new HashSet<Vector2Int>();
    foreach (var enemy in _field.Data.Enemies)
      for (var i = 0; i < enemy.Amount; i++)
        TrySpawnEntity(_field.Room, enemy.SpawnArea, enemy.EnemyPrefab);
  }

  private void TrySpawnEntity(Room room, RectInt area, Entity entity)
  {
    int tries = 10;
    var spawnPoint = FindEmptyPoint(room, area, tries);
    if (spawnPoint != INVALIDPOINT)
      Instantiate(entity, transform, false).Init(_field, _field.LocalPositionByPoint(room, spawnPoint));
  }

  private Vector2Int FindEmptyPoint(Room room, RectInt area, int tries)
  {
    var spawnPoint = area.RandomPoint();

    while (room[spawnPoint] != CellType.Ground)
    {
      tries--;
      if (tries == 0)
      {
        Debug.Log($"Player spawn problem {area}");
        return INVALIDPOINT;
      }
      spawnPoint = area.RandomPoint();
    }

    return spawnPoint;
  }
}

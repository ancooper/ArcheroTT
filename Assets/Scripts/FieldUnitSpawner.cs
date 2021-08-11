using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Field))]
public class FieldUnitSpawner : MonoBehaviour
{
  private readonly Vector2Int InvalidPoint = new Vector2Int(int.MinValue, int.MinValue);

  private Field _field;
  private HashSet<Vector2Int> _usedPoints;

  private void Awake()
  {
    _field = GetComponent<Field>();
  }

  public void Spawn()
  {
    _usedPoints = new HashSet<Vector2Int>();
    SpawnPlayer();
    SpawnEnemy();

    _field.Ready();
  }

  private void SpawnPlayer()
  {
    TrySpawnCharacter(_field.Room, _field.Data.PlayerSpawnArea, _usedPoints, _field.Data.PlayerPrefab);
  }

  private void SpawnEnemy()
  {
    foreach (var enemy in _field.Data.Enemies)
      for (var i = 0; i < enemy.Amount; i++)
        TrySpawnCharacter(_field.Room, enemy.SpawnArea, _usedPoints, enemy.EnemyPrefab);
  }

  private void TrySpawnCharacter(Room room, RectInt area, HashSet<Vector2Int> usedPoints, Character entity)
  {
    int tries = 10;
    var spawnPoint = FindEmptyPoint(room, area, usedPoints, tries);
    if (spawnPoint != InvalidPoint)
      Instantiate(entity, transform, false).Init(_field, _field.LocalPositionByPoint(room, spawnPoint));
  }

  private Vector2Int FindEmptyPoint(Room room, RectInt area, HashSet<Vector2Int> usedPoints, int tries)
  {
    var spawnPoint = area.RandomPoint();

    while (room[spawnPoint] != CellType.Ground || usedPoints.Contains(spawnPoint))
    {
      tries--;
      if (tries == 0)
      {
        Debug.Log($"not find point to spawn {area}");
        return InvalidPoint;
      }
      spawnPoint = area.RandomPoint();
    }
    usedPoints.Add(spawnPoint);

    return spawnPoint;
  }
}

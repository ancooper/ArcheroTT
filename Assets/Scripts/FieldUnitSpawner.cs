using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Field))]
public class FieldUnitSpawner : MonoBehaviour
{
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
    TrySpawnPlayer(_field.Room, _field.Data);
  }

  private void SpawnEnemy()
  {
    _usedPoints = new HashSet<Vector2Int>();
    foreach (var enemy in _field.Data.Enemies)
      for (var i = 0; i < enemy.Amount; i++)
        TrySpawnOneEnemy(_field.Room, enemy);
  }

  private void TrySpawnPlayer(Room room, FieldData data)
  {
    int tries = 10;
    var spawnPoint = data.PlayerSpawnArea.RandomPoint();

    while (tries > 0 && room[spawnPoint] != CellType.Ground)
    {
      spawnPoint = data.PlayerSpawnArea.RandomPoint();
      tries--;
    }

    if (tries == 0)
    {
      Debug.Log($"Player spawn problem {data.PlayerSpawnArea}");
      return;
    }

    var player = Instantiate(data.PlayerPrefab, transform, false);
    player.transform.localPosition = _field.LocalPositionByPoint(room, spawnPoint);
    player.GetComponent<Unit>().OnDead.AddListener(_field.Manager.UnitDeadListener);
  }

  private void TrySpawnOneEnemy(Room room, EnemySpawnData enemyData)
  {
    int tries = 10;
    var spawnPoint = enemyData.SpawnArea.RandomPoint();

    while (tries > 0 && (room[spawnPoint] != CellType.Ground || _usedPoints.Contains(spawnPoint)))
    {
      spawnPoint = enemyData.SpawnArea.RandomPoint();
      tries--;
    }

    if (tries == 0)
    {
      Debug.Log($"Enemyes spawn problem {enemyData}");
      return;
    }

    _usedPoints.Add(spawnPoint);
    var enemy = Instantiate(enemyData.EnemyPrefab, transform, false);
    enemy.Init(_field, _field.LocalPositionByPoint(room, spawnPoint));
    enemy.GetComponent<Unit>().OnDead.AddListener(_field.Manager.UnitDeadListener);
  }
}

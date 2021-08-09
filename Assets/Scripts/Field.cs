using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Field : MonoBehaviour
{
  [Header("Field settings")]
  [SerializeField] private FieldData _data;

  [Header("Cell prefabs")]
  [SerializeField] private Cell _groundPrefab;
  [SerializeField] private Cell _waterPrefab;
  [SerializeField] private Cell _StonePrefab;
  [SerializeField] private Cell _spikePrefab;

  [Header("Border prefabs")]
  [SerializeField] private Border _borderPrefab;

  private Room _room;
  private Status _status;
  private GameManager _manager;
  private Border _border;
  private int[,] _distances;
  private Queue<Vector3Int> _queue;

  public FieldData Data => _data;
  public Room Room => _room;
  public Status Status => _status;
  public GameManager Manager => _manager;

  public void Create(GameManager manager)
  {
    _status = Status.Pause;

    _manager = manager;

    Debug.Assert(_data != null);
    _room = new Room(_data);

    CreateField(_room);
    CreateBorder(_room.Size);
  }

  public void Ready() => _status = Status.Ready;

  public void GameOver() => _status = Status.Gameover;

  public void Victory() => _status = Status.Victory;

  public void Play()
  {
    var units = FindObjectsOfType<Unit>();
    foreach (var unit in units)
      unit.SetActive(true);

    var player = FindObjectOfType<Player>();
    var joystick = FindObjectOfType<Joystick>();
    joystick.JoystickEvent.AddListener(player.Move);

    _status = Status.Play;
  }

  public void EnemyDead(Enemy enemy)
  {
    GetComponent<FieldCoinSpawner>().Spawn(enemy.transform.localPosition, enemy.AmountDroppedCoins);

    var enemyCount = FindObjectsOfType<Enemy>().Length;
    if (enemyCount <= 1)
    {
      CollectCoins();
      OpenDoor();
    }
  }

  public void OpenDoor()
  {
    _border.Door.OpenDoor();
  }

  public void CollectCoins()
  {
    var player = FindObjectOfType<Player>();
    if (player == null)
      return;

    var coins = FindObjectsOfType<Coin>();
    foreach (var coin in coins)
      coin.FlyTo(player.transform);
  }

  private Cell getCellPrefabByType(CellType type)
  {
    switch (type)
    {
      case CellType.Ground:
        return _groundPrefab;
      case CellType.Water:
        return _waterPrefab;
      case CellType.Stone:
        return _StonePrefab;
      case CellType.Spike:
        return _spikePrefab;
      default:
        Debug.LogError($"Not expected cell type: {type}");
        return _groundPrefab;
    }
  }

  public void PlayerDead()
  {
    GameOver();
    _manager.PlayerDead();
  }

  private void CreateField(Room room)
  {
    for (int y = 0; y < room.Size.y; y++)
      for (int x = 0; x < room.Size.x; x++)
      {
        var cell = Instantiate(getCellPrefabByType(room[x, y]), transform, false);
        cell.transform.localPosition = LocalPositionByPoint(room, x, y);
        if (room[x, y] == CellType.Water)
        {
          byte mask = 0;
          int k = 0;
          for (int j = -1; j <= 1; j++)
            for (int i = -1; i <= 1; i++)
            {
              if (i == 0 && j == 0) continue;
              var point = new Vector2Int(x + i, y + j);
              if (room.Contains(point) && room[point] == CellType.Water)
                mask |= (byte)(1 << k);
              k++;
            }
          cell.GetComponent<Water>().SelectSpriteByMask(mask);
        }
      }
  }

  private void CreateBorder(Vector2Int size)
  {
    _border = Instantiate(_borderPrefab, transform, false);
    _border.Init(_manager, size);
  }

  public Vector2 LocalPositionByPoint(Room room, int x, int y) => new Vector2(x - (room.Size.x - 1) / 2, y - (room.Size.y - 1) / 2);
  public Vector2 LocalPositionByPoint(Room room, Vector2Int v) => LocalPositionByPoint(room, v.x, v.y);
  public Vector2Int PointByLocalPosition(Room room, Vector2 position) => new Vector2Int(Mathf.RoundToInt(position.x + (room.Size.x - 1) / 2), Mathf.RoundToInt(position.y + (room.Size.y - 1) / 2));

  public bool PointInField(Vector2 position)
  {
    var point = PointByLocalPosition(_room, position);
    return _room.Contains(point);
  }

  public Queue<Vector2> FindPathOnGround(Vector2 startPosition, Vector2 endPosition)
  {
    _distances = new int[_room.Size.x, _room.Size.y];
    for (int y = 0; y < _room.Size.y; y++)
      for (int x = 0; x < _room.Size.x; x++)
        _distances[x, y] = int.MaxValue;

    _queue = new Queue<Vector3Int>();
    var start = PointByLocalPosition(_room, startPosition);
    SetDistance(new Vector3Int(start.x, start.y, 0));
    while (_queue.Count > 0)
      SetDistance(_queue.Dequeue());
    // DistancesToLog();

    var end = PointByLocalPosition(_room, endPosition);
    var path = new List<Vector3Int>();
    var point = new Vector3Int(end.x, end.y, _distances[end.x, end.y]);
    if (point.z < int.MaxValue)
    {
      while (point.z >= 0)
      {
        var newpoint = point.Neighbors().Select(n => new Vector3Int(n.x, n.y, _room.Contains((Vector2Int)n) ? _distances[n.x, n.y] : int.MaxValue)).OrderBy(n => n.z).First();
        if (point.z > newpoint.z)
          point = newpoint;
        else
          point.z = -1;
        path.Add(point);
      }
    }
    PathToLog(path);

    path.Reverse();
    return new Queue<Vector2>(path.Select(p => LocalPositionByPoint(_room, (Vector2Int)p)));
  }

  private void SetDistance(Vector3Int cellValue)
  {
    if (!_room.Contains((Vector2Int)cellValue) || cellValue.z >= _distances[cellValue.x, cellValue.y])
      return;
    if (_room[(Vector2Int)cellValue] != CellType.Ground)
      return;
    _distances[cellValue.x, cellValue.y] = cellValue.z;
    foreach (var neighbor in cellValue.Neighbors())
      _queue.Enqueue(neighbor);
  }

  private void PathToLog(List<Vector3Int> path)
  {
    var sb = new StringBuilder();
    sb.Append("[");
    var first = true;
    var spoint = new Vector3Int();
    var t = 0f;
    var dt = 1f / (path.Count - 1);
    foreach (var point in path.OrderBy(p => p.z))
    {
      if (!first)
      {
        sb.Append(", ");
        Debug.DrawLine(
          transform.TransformPoint(LocalPositionByPoint(_room, (Vector2Int)spoint)) + new Vector3(0, 0, -2),
          transform.TransformPoint(LocalPositionByPoint(_room, (Vector2Int)point)) + new Vector3(0, 0, -2),
          new Color(1 - t, 0, t),
          2f);
        t += dt;
        spoint = point;
      }
      else
        spoint = point;
      first = false;
      sb.Append(point);
    }
    sb.Append("]");
    // Debug.Log($"Path {sb}");
  }

  private void DistancesToLog()
  {
    Debug.Log(" -= Distances =-");
    for (int y = 0; y < _room.Size.y; y++)
    {
      var sb = new StringBuilder();
      sb.Append('[');
      for (int x = 0; x < _room.Size.x; x++)
      {
        if (_distances[x, y] < int.MaxValue)
          sb.Append(_distances[x, y]);
        else
          sb.Append("XX");
        if (x < _room.Size.x - 1)
          sb.Append(", ");
      }
      sb.Append(']');
      Debug.Log($"{sb}");
    }
    Debug.Log(" ------------------ ");
  }
}

public enum Status { Pause, Ready, Play, Gameover, Victory }

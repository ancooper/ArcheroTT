using UnityEngine;

public class Room
{
  private Vector2Int _size;
  private CellType[,] _cells;

  public Vector2Int Size => _size;
  public CellType this[int x, int y] => _cells[x, y];
  public CellType this[Vector2Int v] => this[v.x, v.y];

  public Room(FieldData data)
  {
    CreateEmpty(data.Size);

    FillAreasByCellType(data.WaterAreas, CellType.Water);
    FillAreasByCellType(data.StounAreas, CellType.Stone);
    FillAreasByCellType(data.SpikeAreas, CellType.Spike);
  }

  public void CreateEmpty(int width, int height)
  {
    _size = new Vector2Int(width, height);
    _cells = new CellType[width, height];
    for (int y = 0; y < height; y++)
      for (int x = 0; x < width; x++)
        _cells[x, y] = CellType.Ground;
  }

  public void CreateEmpty(Vector2Int size) => CreateEmpty(size.x, size.y);

  private void FillAreasByCellType(RectInt[] rects, CellType type)
  {
    foreach (var rect in rects)
    {
      for (int y = rect.yMin; y < rect.yMax; y++)
        for (int x = rect.xMin; x < rect.xMax; x++)
          _cells[x, y] = type;
    }
  }

  public bool Contains(Vector2Int point) => point.x >= 0 && point.x < _size.x && point.y >= 0 && point.y < _size.y;
}

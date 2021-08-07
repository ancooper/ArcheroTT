using UnityEngine;

[ExecuteAlways]
public class Border : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private Vector2Int _size = new Vector2Int(3, 3);

  [Header("Walls & Door")]
  [SerializeField] private Door _door;
  [SerializeField] private Transform _leftWall;
  [SerializeField] private Transform _rightWall;
  [SerializeField] private Transform _topWall;
  [SerializeField] private Transform _bottomLeftWall;
  [SerializeField] private Transform _bottomRightWall;
  [SerializeField] private Transform _leftTopCorner;
  [SerializeField] private Transform _leftBottomCorner;
  [SerializeField] private Transform _rightTopCorner;
  [SerializeField] private Transform _rightBottomCorner;

  private Vector2Int _lastSize;

  public Door Door => _door;

  public void Init(GameManager manager, Vector2Int size)
  {
    _size = size;
    _door.Init(manager);
  }

  private void OnValidate()
  {
    if (_size.x < 3)
      _size.x = 3;
    if (_size.x % 2 == 0)
      _size.x++;
    if (_size.y < 3)
      _size.y = 3;
    if (_size.y % 2 == 0)
      _size.y++;
  }

  private void Update()
  {
    if (_size != _lastSize)
      Resize();
    _lastSize = _size;
  }

  private void Resize()
  {
    var x = (_size.x + 1) / 2;
    var y = (_size.y + 1) / 2;

    _leftWall.localPosition = new Vector3(-x, 0, 0);
    _rightWall.localPosition = new Vector3(x, 0, 0);
    _topWall.localPosition = new Vector3(0, -y, 0);
    _door.transform.localPosition = new Vector3(0, y, 0);
    _bottomLeftWall.localPosition = new Vector3(-(1 + (_size.x - 2) / 4f), y, 0);
    _bottomRightWall.localPosition = new Vector3((1 + (_size.x - 2) / 4f), y, 0);

    _leftTopCorner.localPosition = new Vector3(-x, -y, 0);
    _leftBottomCorner.localPosition = new Vector3(-x, y, 0);
    _rightTopCorner.localPosition = new Vector3(x, -y, 0);
    _rightBottomCorner.localPosition = new Vector3(x, y, 0);

    _leftWall.GetComponent<SpriteRenderer>().size = new Vector2(1, _size.y);
    _rightWall.GetComponent<SpriteRenderer>().size = new Vector2(1, _size.y);
    _topWall.GetComponent<SpriteRenderer>().size = new Vector2(_size.x, 1);
    _bottomLeftWall.GetComponent<SpriteRenderer>().size = new Vector2((_size.x - 2) / 2f, 1);
    _bottomRightWall.GetComponent<SpriteRenderer>().size = new Vector2((_size.x - 2) / 2f, 1);
  }
}

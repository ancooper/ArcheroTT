using UnityEngine;

[RequireComponent(typeof(Camera)), ExecuteAlways]
public class CameraResizer : MonoBehaviour
{
  [SerializeField] private Vector2 _size = new Vector2(14, 24);

  private Camera _camera;
  private Vector2 _lastSize;

  private void Awake()
  {
    _camera = GetComponent<Camera>();
  }

  private void OnValidate()
  {
    if (_size.x < 1)
      _size.x = 1;
    if (_size.y < 1)
      _size.y = 1;
  }

  void Update()
  {
    ApplySizeWhenChanged();
  }

  private void ApplySizeWhenChanged()
  {
    if (_lastSize != _size)
    {
      _lastSize = _size;
      var verticalSize = _size.y * 0.5f;
      var horizontalSize = _size.x * 0.5f / _camera.aspect;

      _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
  }
}

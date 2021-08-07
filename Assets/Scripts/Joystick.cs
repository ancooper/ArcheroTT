using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Joystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  [Header("Settings")]
  [SerializeField, Range(0f, 2f)] private float _dragRange = 1;
  [SerializeField, Range(0f, 0.5f)] private float _deadZone = 0;
  [SerializeField] private bool _showOnlyDraging = false;
  [SerializeField] private bool _readKeyboardInputToo = true;

  [Header("Images")]
  [SerializeField] private RectTransform _background;
  [SerializeField] private RectTransform _handle;

  [Header("Events")]
  public UnityEvent<Vector2> JoystickEvent;

  private Vector3 _defaultPosition;
  private bool _isDraging = false;
  private Vector2 _startDragPosition;
  private bool _keyboardInput;
  private Vector2 _offset;
  private Vector2 _lastDirection;

  void Awake()
  {
    _defaultPosition = _background.localPosition;

    if (_showOnlyDraging)
      _background.gameObject.SetActive(false);
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.pointerCurrentRaycast.screenPosition, eventData.enterEventCamera, out Vector2 position))
    {
      _isDraging = true;
      _startDragPosition = position;
      _background.localPosition = _defaultPosition + (Vector3)position;

      if (_showOnlyDraging)
        _background.gameObject.SetActive(true);
    }
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    _isDraging = false;
    _background.localPosition = _defaultPosition;
    _handle.localPosition = Vector3.zero;
    _offset = Vector2.zero;
    JoystickEvent.Invoke(_offset);
    if (_showOnlyDraging)
      _background.gameObject.SetActive(false);
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.pointerCurrentRaycast.screenPosition, eventData.enterEventCamera, out Vector2 joystickPosition))
    {
      var offset = joystickPosition * 2f / _background.sizeDelta.x;
      if (offset.sqrMagnitude > 1)
        offset = offset.normalized;
      if (offset.magnitude <= _deadZone)
        offset = Vector2.zero;

      _offset = offset;
    }
  }

  private void Update()
  {
    if (_readKeyboardInputToo)
      ReadKeyboardInput();

    if (_isDraging || _keyboardInput)
    {
      SetHandlePosition(_offset);
      JoystickEvent.Invoke(_offset);
    }
  }

  private void ReadKeyboardInput()
  {
    var offset = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    _keyboardInput = offset != _lastDirection;
    _lastDirection = offset;
    if (_keyboardInput)
      _offset = offset;
  }

  private void SetHandlePosition(Vector2 offset) => _handle.localPosition = offset * _background.sizeDelta.x * 0.5f * _dragRange;
}

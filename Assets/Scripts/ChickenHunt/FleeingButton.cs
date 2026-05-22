using UnityEngine;
using UnityEngine.EventSystems;

public class FleeingButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private float _fleeDistance = 100f; 
    [SerializeField] private float _speed = 5f;       

    private RectTransform _rectTransform;
    private Vector2 _targetPosition;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _targetPosition = _rectTransform.anchoredPosition;
    }

    void Update()
    {
        //Рух кнопки
        _rectTransform.anchoredPosition = Vector2.Lerp(
            _rectTransform.anchoredPosition,
            _targetPosition,
            Time.unscaledDeltaTime * _speed 
        );
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //випадковий напрямок втечі
        float randomX = Random.Range(-_fleeDistance, _fleeDistance);
        float randomY = Random.Range(-_fleeDistance, _fleeDistance);

        _targetPosition = new Vector2(
            _rectTransform.anchoredPosition.x + randomX,
            _rectTransform.anchoredPosition.y + randomY
        );
        //Обмеження
        KeepInsideCanvas();
    }

    private void KeepInsideCanvas()
    {
        //перевірку меж екрана
        float margin = 100f;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -Screen.width / 2 + margin, Screen.width / 2 - margin);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, -Screen.height / 2 + margin, Screen.height / 2 - margin);
    }
}
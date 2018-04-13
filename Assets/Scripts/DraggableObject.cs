using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static System.Action<DraggableObject> OnFinishedDrag;

    [SerializeField] private bool dragOnSurfaces = false;
    [SerializeField] private bool _canDrag = true;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private Color _colorBackground;
    [SerializeField] private bool _canDebug = false;

    #region Private variables
    private RectTransform m_DraggingPlane;
    private Canvas canvasParent = null;
    private float distanceAllowToBeCorrect = 0;
    private Vector3 originalPosition;
    private int contentValue = -1;
    #endregion

    public float DistanceAllowToBeCorrect { get { return distanceAllowToBeCorrect; } set { distanceAllowToBeCorrect = value; } }
    public Vector3 OriginalPosition { get { return originalPosition; } set { originalPosition = value; } }
    public bool CanDrag { get { return _canDrag; } set { _canDrag = value; } }
    public int ContentValue { get { return contentValue;  }  set { contentValue = value; } }

    private void Start()
    {
        canvasParent = GetComponentInParent<Canvas>();
        m_DraggingPlane = transform as RectTransform;
        originalPosition = transform.position;
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            m_DraggingPlane.position = globalMousePos;
            m_DraggingPlane.rotation = m_DraggingPlane.rotation;
        }

        if (_canDebug)
            Debug.Log("<color=yellow>Drag</color> pos: " + m_DraggingPlane.anchoredPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //If not can drag then avoid this action
        if (!_canDrag)
            return;

        if (canvasParent == null)
            return;

        SetDraggedPosition(eventData);

        if (_canDebug)
            Debug.Log("<color=green>BeginDrag</color>");
    }

    public void OnDrag(PointerEventData data)
    {
        SetDraggedPosition(data);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_canDebug)
            Debug.Log("<color=red>EndDrag</color>");

        OnFinishedDrag(this);
    }

    public void BackOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void SetContentValue(int val)
    {
        contentValue = val;

        _text.text = string.Format("{00:0}", val);
    }
}
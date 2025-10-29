using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Represents a draggable money piece with a specific value
/// </summary>
public class MoneyPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI References")]
    public TextMeshProUGUI valueLabel;
    public Image background;

    [Header("State")]
    public int valueCents;

    private MainGameController controller;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Add CanvasGroup if it doesn't exist
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // If no label exists, try to find it in children
        if (valueLabel == null)
        {
            valueLabel = GetComponentInChildren<TextMeshProUGUI>();
        }

        // If no background exists, try to find it
        if (background == null)
        {
            background = GetComponent<Image>();
        }
    }

    public void Initialize(int cents, MainGameController gameController)
    {
        valueCents = cents;
        controller = gameController;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (valueLabel != null)
        {
            // Format as dollars and cents
            float dollars = valueCents / 100f;
            valueLabel.text = $"${dollars:F2}";
        }

        if (background != null)
        {
            // Color based on value (green for larger, lighter for smaller)
            float normalizedValue = Mathf.Clamp01(valueCents / 10000f);
            background.color = Color.Lerp(new Color(0.8f, 0.9f, 0.8f), new Color(0.2f, 0.8f, 0.2f), normalizedValue);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Right click or ctrl+click to split
        if (eventData.button == PointerEventData.InputButton.Right || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (controller != null)
            {
                controller.OnMoneyPieceSplit(this);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Make it semi-transparent while dragging
        canvasGroup.alpha = 0.6f;

        // Disable raycasting so we can detect the drop zone underneath
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore opacity
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Check if we dropped on the cashier zone
        if (eventData.pointerEnter != null)
        {
            CashierDropZone dropZone = eventData.pointerEnter.GetComponent<CashierDropZone>();
            if (dropZone != null)
            {
                dropZone.OnMoneyDropped(this);
                return;
            }
        }

        // If not dropped on cashier, return to original position
        rectTransform.anchoredPosition = originalPosition;
    }
}

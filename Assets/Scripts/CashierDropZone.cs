using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Drop zone where players drag money pieces to pay
/// </summary>
public class CashierDropZone : MonoBehaviour, IDropHandler
{
    [Header("Visual Feedback")]
    public Color normalColor = new Color(1f, 1f, 0.8f, 0.3f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 0.5f);

    private UnityEngine.UI.Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<UnityEngine.UI.Image>();
        if (backgroundImage != null)
        {
            backgroundImage.color = normalColor;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on CashierDropZone");
        // This is called by Unity's event system, but we handle it in MoneyPiece.OnEndDrag
    }

    public void OnMoneyDropped(MoneyPiece piece)
    {
        Debug.Log($"Money piece dropped: {piece.valueCents} cents");

        if (GameState.Instance != null)
        {
            // Add to paid amount
            GameState.Instance.AddPayment(piece.valueCents);

            // Destroy the piece
            Destroy(piece.gameObject);

            // Check if round is complete
            if (GameState.Instance.IsRoundComplete())
            {
                Debug.Log("Round complete! Player paid enough.");
                GameState.Instance.EndRound();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = highlightColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = normalColor;
        }
    }
}

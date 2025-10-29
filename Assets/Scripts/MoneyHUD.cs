using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// HUD that displays game information and provides start/reset functionality
/// </summary>
public class MoneyHUD : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetLabel;
    public TextMeshProUGUI paidLabel;
    public TextMeshProUGUI remainingLabel;
    public TextMeshProUGUI statusLabel;
    public Button startButton;
    public GameObject startPanel;

    [Header("Events")]
    public UnityEvent StartGameRequested;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        // Show start panel initially
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        UpdateDisplay();
    }

    private void Update()
    {
        if (GameState.Instance != null && GameState.Instance.roundActive)
        {
            UpdateDisplay();
        }
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");

        // Hide start panel
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        // Trigger event
        StartGameRequested?.Invoke();

        // Update display
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (GameState.Instance == null)
        {
            if (statusLabel != null)
            {
                statusLabel.text = "Waiting for GameState...";
            }
            return;
        }

        // Update target
        if (targetLabel != null)
        {
            float targetDollars = GameState.Instance.targetCents / 100f;
            targetLabel.text = $"Target: ${targetDollars:F2}";
        }

        // Update paid
        if (paidLabel != null)
        {
            float paidDollars = GameState.Instance.paidCents / 100f;
            paidLabel.text = $"Paid: ${paidDollars:F2}";
        }

        // Update remaining
        if (remainingLabel != null)
        {
            float remainingDollars = GameState.Instance.GetRemainingCents() / 100f;
            remainingLabel.text = $"Remaining: ${remainingDollars:F2}";
        }

        // Update status
        if (statusLabel != null)
        {
            if (!GameState.Instance.roundActive)
            {
                statusLabel.text = "Press Start to begin!";
            }
            else if (GameState.Instance.IsRoundComplete())
            {
                statusLabel.text = "Complete! Well done!";
                statusLabel.color = Color.green;
            }
            else if (GameState.Instance.paidCents > GameState.Instance.targetCents)
            {
                statusLabel.text = "Too much! Try again.";
                statusLabel.color = Color.red;
            }
            else
            {
                statusLabel.text = "Drag money to the cashier!";
                statusLabel.color = Color.white;
            }
        }
    }

    public void OnResetButtonClicked()
    {
        // Show start panel again
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        // Clear any existing money pieces
        MoneyPiece[] pieces = FindObjectsOfType<MoneyPiece>();
        foreach (var piece in pieces)
        {
            Destroy(piece.gameObject);
        }

        // Reset game state
        if (GameState.Instance != null)
        {
            GameState.Instance.EndRound();
        }

        UpdateDisplay();
    }
}

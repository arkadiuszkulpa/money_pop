using UnityEngine;

/// <summary>
/// Central game state singleton that tracks the current round's target, what the player has paid, etc.
/// </summary>
public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("Current Round")]
    public int targetCents = 0;
    public int paidCents = 0;
    public bool roundActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewRound(int target)
    {
        targetCents = target;
        paidCents = 0;
        roundActive = true;
        Debug.Log($"New round started! Target: {targetCents} cents");
    }

    public void AddPayment(int cents)
    {
        paidCents += cents;
        Debug.Log($"Payment added: {cents} cents. Total paid: {paidCents} cents");
    }

    public void EndRound()
    {
        roundActive = false;
        Debug.Log("Round ended");
    }

    public int GetRemainingCents()
    {
        return Mathf.Max(0, targetCents - paidCents);
    }

    public bool IsRoundComplete()
    {
        return paidCents >= targetCents;
    }
}

using UnityEngine;

/// <summary>
/// Main game controller that spawns money pieces and coordinates game flow
/// </summary>
public class MainGameController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject piecePrefab;

    [Header("Spawn Settings")]
    public Transform playArea;
    public Vector2 spawnPosition = new Vector2(0, 0);

    [Header("Denomination Settings")]
    public int[] availableDenominations = new int[] { 1, 5, 10, 25, 50, 100, 500, 1000, 2000, 5000, 10000 }; // cents

    private void Start()
    {
        if (playArea == null)
        {
            playArea = transform;
        }
    }

    public void OnStartGameRequested()
    {
        Debug.Log("Start game requested");

        // Generate a random target amount (between $1 and $50)
        int targetCents = Random.Range(100, 5000);

        // Start new round
        GameState.Instance.StartNewRound(targetCents);

        // Spawn initial money piece with the largest denomination that fits
        int startingDenomination = GetLargestDenominationUnder(targetCents);
        SpawnMoneyPiece(startingDenomination, spawnPosition);
    }

    private int GetLargestDenominationUnder(int cents)
    {
        for (int i = availableDenominations.Length - 1; i >= 0; i--)
        {
            if (availableDenominations[i] <= cents)
            {
                return availableDenominations[i];
            }
        }
        return availableDenominations[0]; // Return smallest if nothing else fits
    }

    public void SpawnMoneyPiece(int valueCents, Vector2 position)
    {
        if (piecePrefab == null)
        {
            Debug.LogError("MoneyPiece prefab not assigned!");
            return;
        }

        GameObject pieceObj = Instantiate(piecePrefab, playArea);
        RectTransform rectTransform = pieceObj.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }

        MoneyPiece piece = pieceObj.GetComponent<MoneyPiece>();
        if (piece != null)
        {
            piece.Initialize(valueCents, this);
        }
        else
        {
            Debug.LogError("MoneyPiece component not found on prefab!");
        }
    }

    public void OnMoneyPieceSplit(MoneyPiece originalPiece)
    {
        int originalValue = originalPiece.valueCents;

        // Find next smaller denomination
        int smallerDenomination = GetNextSmallerDenomination(originalValue);

        if (smallerDenomination <= 0)
        {
            Debug.Log("Cannot split further - already at smallest denomination");
            return;
        }

        // Calculate how many pieces we can make
        int numPieces = originalValue / smallerDenomination;

        if (numPieces <= 1)
        {
            Debug.Log("Cannot split - value too small");
            return;
        }

        // Destroy original piece
        Vector2 originalPos = originalPiece.GetComponent<RectTransform>().anchoredPosition;
        Destroy(originalPiece.gameObject);

        // Spawn new pieces in a spread pattern
        float spreadRadius = 50f;
        for (int i = 0; i < numPieces; i++)
        {
            float angle = (360f / numPieces) * i * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spreadRadius;
            SpawnMoneyPiece(smallerDenomination, originalPos + offset);
        }
    }

    private int GetNextSmallerDenomination(int currentValue)
    {
        for (int i = availableDenominations.Length - 1; i >= 0; i--)
        {
            if (availableDenominations[i] < currentValue)
            {
                return availableDenominations[i];
            }
        }
        return -1; // No smaller denomination available
    }
}

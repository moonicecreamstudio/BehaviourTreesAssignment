using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NodeCanvas.Framework;

public class CardManager : MonoBehaviour
{
    public List<Cards> listOfPlayerCards;
    public List<Cards> playerDrawPile;
    public List<Cards> playerDiscardPile;
    public List<Cards> playerHandPile;

    public GameObject cardPrefab;
    public Transform playerHandArea;

    public Blackboard monsterBlackboard;

    public int maxHandSize;

    [SerializeField] private int selectedCard;

    public bool playerTurn;
    public float playerTurnTimer;
    public float playerMaxTurnTimer;



    public void Start()
    {

        InitializeDrawPile();
        ShuffleDeck();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawCard();
        }

        if (playerHandPile.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SelectPreviousCard();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SelectNextCard();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaySelectedCard();
            }
        }
    }

    // Turn Order

    

    // Ghost



    // Player
    public void InitializeDrawPile()
    {
        playerDrawPile = new List<Cards>(listOfPlayerCards);
        playerHandPile = new List<Cards>();
    }

    public void ShuffleDeck()
    {
        for (int i = playerDrawPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Cards tempPile = playerDrawPile[i];
            playerDrawPile[i] = playerDrawPile[randomIndex];
            playerDrawPile[randomIndex] = tempPile;
        }
    }

    public void DrawCard()
    {
        if (playerHandPile.Count >= maxHandSize)
        {
            Debug.Log("Your hand is full");
            return;
        }

        if (playerDrawPile.Count == 0)
        {
            RefreshDrawPile();
        }

        if (playerDrawPile.Count > 0)
        {
            Cards drawnCard = playerDrawPile[0];
            playerDrawPile.RemoveAt(0);
            playerHandPile.Add(drawnCard);

            Debug.Log("Drew the card: " + drawnCard.title);

            DisplayCard(drawnCard);
            selectedCard = playerHandPile.Count - 1; // Last drawn card is selectred
            UpdateCardSelection();
        }
    }

    public void DisplayCard(Cards card)
    {
        GameObject newCard = Instantiate(cardPrefab, playerHandArea);
        newCard.transform.SetParent(playerHandArea, false);

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.cardData = card;
            cardDisplay.UpdateCardDisplay();
        }

        UpdateCardSelection();
    }

    public void SelectPreviousCard()
    {
        if (playerHandPile.Count == 0) return;
        selectedCard = (selectedCard - 1 + playerHandPile.Count) % playerHandPile.Count;
        UpdateCardSelection();
    }

    public void SelectNextCard()
    {
        if (playerHandPile.Count == 0) return;
        selectedCard = (selectedCard + 1) % playerHandPile.Count;
        UpdateCardSelection();
    }

    public void UpdateCardSelection()
    {
        // Loop through all cards and update their selection state
        for (int i = 0; i < playerHandArea.childCount; i++)
        {
            GameObject cardObj = playerHandArea.GetChild(i).gameObject;
            Image cardImage = cardObj.GetComponentInChildren<Image>();

            // Highlight the selected card
            // Not sure why highlighted card breaks when a card is played
            if (cardImage != null)
            {
                if (i == selectedCard)
                {
                    cardImage.color = Color.yellow; // Highlight selected card
                }
                else
                {
                    cardImage.color = Color.white; // Reset color for unselected cards
                }
            }
        }
    }

    public void PlaySelectedCard()
    {
        if (playerHandPile.Count == 0) return;

        Cards playedCard = playerHandPile[selectedCard];
        playerHandPile.RemoveAt(selectedCard);

        Debug.Log("Player played the card: " + playedCard.title);
        PlayedCard(playedCard.cardID);
        playerDiscardPile.Add(playedCard);

        // Destroy the palyed card
        Destroy(playerHandArea.GetChild(selectedCard).gameObject);

        if (playerHandPile.Count > 0)
        {
            selectedCard = 0; // Select the first card
        }

        // Update highlighted card
        UpdateCardSelection();
    }

    public void RefreshDrawPile()
    {
        if (playerDiscardPile.Count > 0)
        {
            playerDrawPile.AddRange(playerDiscardPile);
            playerDiscardPile.Clear();
            ShuffleDeck();
            Debug.Log("Reshuffle time");
        }
    }

    public void PlayedCard(int cardID)
    {
        // When time permits, using scriptable objects here would be better
        var isAwake = monsterBlackboard.GetVariable<bool>("isAwake");
        var canSeePlayer = monsterBlackboard.GetVariable<bool>("canSeePlayer");

        if (cardID == 1)
        {
            isAwake.value = true;
        }

        if (cardID == 2)
        {
            //key
        }

        if (cardID == 3)
        {
            //player speed
        }

        if (cardID == 4)
        {
            //all seeing
        }

        if (cardID == 5)
        {
            //discard random card
        }

        if (cardID == 6)
        {
            // draw 2 card
        }

        if (cardID == 7)
        {
            isAwake.value = false;
        }

        if (cardID == 8)
        {
            canSeePlayer.value = false;
        }
    }
}

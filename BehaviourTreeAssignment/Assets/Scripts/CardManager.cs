using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NodeCanvas.Framework;
using System.Collections;

public class CardManager : MonoBehaviour
{
    public List<Cards> listOfPlayerCards;
    public List<Cards> playerDrawPile;
    public List<Cards> playerDiscardPile;
    public List<Cards> playerHandPile;

    public List<Cards> listOfGhostCards;
    public List<Cards> ghostDrawPile;
    public List<Cards> ghostDiscardPile;
    public List<Cards> ghostHandPile;

    public GameObject cardPrefab;
    public Transform playerHandArea;
    public Transform ghostHandArea;

    public Blackboard monsterBlackboard;
    public Blackboard ghostBlackboard;

    public int maxPlayerHandSize;
    public int maxGhostHandSize;

    [SerializeField] private int selectedCard;
    [SerializeField] private int ghostSelectedCard;

    public bool playerTurn;
    public float playerTurnTimer;
    public float playerMaxTurnTimer;

    // Keely suggests using this
    // See Week 12 Slides
    public enum GameState
    {
        player_turn, ghost_turn, ghost_deciding
    }

    public GameState gameState;

    public void Start()
    {

        playerTurn = false;
        InitializePlayerDrawPile();
        ShufflePlayerDeck();
        PlayerDrawCard();
        PlayerDrawCard();
        PlayerDrawCard();
        PlayerDrawCard();
        PlayerDrawCard();

        InitializeGhostDrawPile();
        ShuffleGhostDeck();
        GhostDrawCard();
        GhostDrawCard();
        GhostDrawCard();
        GhostDrawCard();
        GhostDrawCard();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerDrawCard();
        }

        if (playerHandPile.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                SelectPreviousCard();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                SelectNextCard();
            }

            if (Input.GetKeyDown(KeyCode.Space) && playerTurn == true)
            {
                PlayerPlaySelectedCard();
                var isPlayerTurn = ghostBlackboard.GetVariable<bool>("isPlayerTurn");
                isPlayerTurn.value = false;
                playerTurn = false;
            }
        }
    }

    // Turn Order



    // Ghost
    public void InitializeGhostDrawPile()
    {
        ghostDrawPile = new List<Cards>(listOfGhostCards);
        ghostHandPile = new List<Cards>();
    }

    public void ShuffleGhostDeck()
    {
        for (int i = ghostDrawPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Cards tempPile = ghostDrawPile[i];
            ghostDrawPile[i] = ghostDrawPile[randomIndex];
            ghostDrawPile[randomIndex] = tempPile;
        }
    }

    public void GhostDrawCard()
    {
        if (ghostHandPile.Count >= maxGhostHandSize)
        {
            Debug.Log("Ghost hand is full");
            return;
        }

        if (ghostDrawPile.Count == 0)
        {
            RefreshGhostDrawPile();
        }

        if (playerDrawPile.Count > 0)
        {
            Cards drawnCard = ghostDrawPile[0];
            ghostDrawPile.RemoveAt(0);
            ghostHandPile.Add(drawnCard);

            Debug.Log("Ghost drew the card: " + drawnCard.title);

            DisplayGhostCard(drawnCard);
            //selectedCard = 0; // First drawn card is selectred
        }
    }

    public void DisplayGhostCard(Cards card)
    {
        GameObject newCard = Instantiate(cardPrefab, ghostHandArea);
        newCard.transform.SetParent(ghostHandArea, false);

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.cardData = card;
            cardDisplay.UpdateCardDisplay();
        }
    }

    public void RefreshGhostDrawPile()
    {
        if (ghostDiscardPile.Count > 0)
        {
            ghostDrawPile.AddRange(ghostDiscardPile);
            ghostDiscardPile.Clear();
            ShuffleGhostDeck();
            Debug.Log("Reshuffle for Ghost");
        }
    }

    public void GhostSelectCard(int selectedCard)
    {
        if (ghostHandPile.Count == 0) return;
        ghostSelectedCard = selectedCard;
    }

    public void GhostPlaySelectedCard()
    {
        if (ghostHandPile.Count == 0)
        {
            return;
        }

        GhostDrawCard(); // Temp

        Cards playedCard = ghostHandPile[ghostSelectedCard];
        ghostHandPile.RemoveAt(ghostSelectedCard);

        Debug.Log("Ghost played the card: " + playedCard.title);
        PlayedCard(playedCard.cardID);
        ghostDiscardPile.Add(playedCard);

        // Destroy the palyed card
        Destroy(ghostHandArea.GetChild(ghostSelectedCard).gameObject);

        playerTurn = true;
        var isPlayerTurn = ghostBlackboard.GetVariable<bool>("isPlayerTurn");
        isPlayerTurn.value = true;
    }

    public void UpdateGhostCardSelection(int selectedCard)
    {
        // Loop through all cards and update their selection state
        for (int i = 0; i < ghostHandArea.childCount; i++)
        {
            GameObject cardObj = ghostHandArea.GetChild(i).gameObject;
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
        StartCoroutine(Wait(1f));
    }

    public IEnumerator Wait(float seconds)
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(seconds);
    }

    // Player
    public void InitializePlayerDrawPile()
    {
        playerDrawPile = new List<Cards>(listOfPlayerCards);
        playerHandPile = new List<Cards>();
    }

    public void ShufflePlayerDeck()
    {
        for (int i = playerDrawPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Cards tempPile = playerDrawPile[i];
            playerDrawPile[i] = playerDrawPile[randomIndex];
            playerDrawPile[randomIndex] = tempPile;
        }
    }

    public void PlayerDrawCard()
    {
        if (playerHandPile.Count >= maxPlayerHandSize)
        {
            Debug.Log("Your hand is full");
            return;
        }

        if (playerDrawPile.Count == 0)
        {
            RefreshPlayerDrawPile();
        }

        if (playerDrawPile.Count > 0)
        {
            Cards drawnCard = playerDrawPile[0];
            playerDrawPile.RemoveAt(0);
            playerHandPile.Add(drawnCard);

            Debug.Log("Drew the card: " + drawnCard.title);

            DisplayPlayerCard(drawnCard);
            selectedCard = 0; // First drawn card is selectred
            UpdatePlayerCardSelection();
        }
    }

    public void DisplayPlayerCard(Cards card)
    {
        GameObject newCard = Instantiate(cardPrefab, playerHandArea);
        newCard.transform.SetParent(playerHandArea, false);

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.cardData = card;
            cardDisplay.UpdateCardDisplay();
        }

        UpdatePlayerCardSelection();
    }

    public void SelectPreviousCard()
    {
        if (playerHandPile.Count == 0) return;
        selectedCard = (selectedCard - 1 + playerHandPile.Count) % playerHandPile.Count;
        UpdatePlayerCardSelection();
    }

    public void SelectNextCard()
    {
        if (playerHandPile.Count == 0) return;
        selectedCard = (selectedCard + 1) % playerHandPile.Count;
        UpdatePlayerCardSelection();
    }

    public void UpdatePlayerCardSelection()
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

    public void PlayerPlaySelectedCard()
    {
        if (playerHandPile.Count == 0)
        {
            return;
        }

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
        StartCoroutine(WaitAndUpdatePlayerSelection());
    }

    public IEnumerator WaitAndUpdatePlayerSelection()
    {
        yield return new WaitForSeconds(0.1f);
        UpdatePlayerCardSelection();
    }

    public void RefreshPlayerDrawPile()
    {
        if (playerDiscardPile.Count > 0)
        {
            playerDrawPile.AddRange(playerDiscardPile);
            playerDiscardPile.Clear();
            ShufflePlayerDeck();
            Debug.Log("Reshuffle time");
        }
    }

    public void PlayedCard(int cardID)
    {
        // When time permits, using scriptable objects here would be better
        var isAwake = monsterBlackboard.GetVariable<bool>("isAwake");
        var canSeePlayer = monsterBlackboard.GetVariable<bool>("canSeePlayer");
        var debuffSlow = monsterBlackboard.GetVariable<bool>("debuffSlow");

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
        if (cardID == 9)
        {
            // player speed
        }
        if (cardID == 10)
        {
            PlayerDrawCard(); // Something about this here breaks how cards will be played
        }
        if (cardID == 11)
        {
            //unlock
        }
        if (cardID == 12)
        {
            debuffSlow.value = true;
        }
        if (cardID == 13)
        {
            canSeePlayer.value = false;
        }
    }
}

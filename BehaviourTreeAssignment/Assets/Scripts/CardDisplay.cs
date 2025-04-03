using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Cards cardData;

    public int cardID;
    public TextMeshProUGUI cardTitle;
    public TextMeshProUGUI cardDescription;
    public int priorityType;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCardDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCardDisplay()
    {
        cardID = cardData.cardID;
        cardTitle.text = cardData.title.ToString();
        cardDescription.text = cardData.description.ToString();
        priorityType = cardData.priorityType;
    }
}

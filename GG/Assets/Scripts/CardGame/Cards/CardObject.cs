using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    #region Fields

    private static GameObject cardPrefab;

    private CardInfo cardInfo = new CardInfo();

    private Image backgroundUI;
    private Image imageUI;
    private Text nameUI;
    private Text descriptionUI;

    #endregion Fields

    public static CardObject Instanciate(CardInfo cardInfo, Vector2 position)
    {
        if (cardInfo == null)
            return null;

        if (cardPrefab == null)
            cardPrefab = (GameObject)Resources.Load("Prefabs/CardGame/Card");

        GameObject obj = GameObject.Instantiate(cardPrefab, GameObject.FindObjectOfType<Canvas>().transform);
        obj.transform.localPosition = position;

        CardObject card = obj.GetComponent<CardObject>();
        card.cardInfo = cardInfo;

        return card;
    }

    #region Main-Loop

    private void Awake()
    {
        Image[] images = this.GetComponentsInChildren<Image>();
        Text[] texts = this.GetComponentsInChildren<Text>();

        backgroundUI = images[0];
        imageUI = images[1];

        nameUI = texts[0];
        descriptionUI = texts[1];
    }

    // Start is called before the first frame update
    private void Start()
    {
        updateCardUI();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void updateCardUI()
    {
        nameUI.text = cardInfo.Name;
        descriptionUI.text = cardInfo.Group.ToString();
    }

    #endregion Main-Loop
}
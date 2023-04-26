using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields

    private static GameObject cardPrefab;
    private static CardObject hoveredCard = null;

    private CardInfo cardInfo = new CardInfo();

    private Image backgroundUI;
    private Image imageUI;
    private Text nameUI;
    private Text descriptionUI;

    private bool draging = false;
    private Vector2 dragOffset;

    #endregion Fields

    public static CardObject Instantiate(CardInfo cardInfo, Vector2 position)
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

    #region Properties

    public CardInfo Info => cardInfo;
    public bool Hovered => this == hoveredCard;
    public bool Draging => draging;

    public static CardObject HoveredCard => hoveredCard;

    #endregion Properties

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
        updateCardUI();
    }

    private void updateCardUI()
    {
        nameUI.text = cardInfo.Name;
        descriptionUI.text =
            $"Group: {cardInfo.Group}\n" +
            $"Hovered: {Hovered}\n" +
            $"Draging: {Draging}";
    }

    #endregion Main-Loop

    #region Hover

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredCard = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Hovered)
            hoveredCard = null;
    }

    #endregion Hover

    #region Drag

    public void OnBeginDrag(PointerEventData eventData)
    {
        draging = true;
        dragOffset = (Vector2)transform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Move GameObject
        transform.position = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draging = false;
        //Snap GameObject back in bounds if outside
    }

    #endregion Drag
}
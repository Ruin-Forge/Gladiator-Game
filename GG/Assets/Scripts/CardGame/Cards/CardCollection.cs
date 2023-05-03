using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CardCollectionType
{
    Stack, List, Fan
}

public class CardCollection : MonoBehaviour
{
    #region Fields

    public CardCollectionType collectionType;
    public bool shiftHoveredUp = false;
    private List<CardObject> cards = new List<CardObject>();

    #endregion Fields

    #region Properties

    public int Count => cards.Count;
    public CardObject[] Cards => cards.ToArray();

    #endregion Properties

    #region Main-Loop

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = cards.Count - 1; i > -1; i--)
        {
            CardObject current = cards[i];

            if (current == null || current.Collection != this)
            {
                cards.RemoveAt(i);
                continue;
            }

            if (!current.Draging)
            {
                if (this.transform is RectTransform transform)
                {
                    float width = transform.rect.width;

                    current.transform.SetSiblingIndex(i);

                    switch (collectionType)
                    {
                        case CardCollectionType.Stack:
                            current.transform.localPosition = new Vector3(0, 0, 0);
                            current.transform.rotation = new Quaternion();
                            break;

                        case CardCollectionType.List:
                            {
                                float offset = 0;
                                if (cards.Count > 1)
                                    offset = width / (cards.Count - 1) * i - width * 0.5f;
                                if (shiftHoveredUp && current.Hovered)
                                    current.transform.localPosition = new Vector3(offset, 300, 0);
                                else
                                    current.transform.localPosition = new Vector3(offset, 0, 0);
                                current.transform.rotation = new Quaternion();
                                break;
                            }

                        case CardCollectionType.Fan:
                            float angle = 0;
                            if (cards.Count > 1)
                            {
                                angle = 90 / (cards.Count - 1) * i - 45;
                            }
                            current.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                            angle += 90;
                            current.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 100;
                            break;
                    }
                }
            }
        }

        if (CardObject.HoveredCard != null)
            CardObject.HoveredCard.transform.SetAsLastSibling();
    }

    #endregion Main-Loop

    public void Shuffle()
    {
        var cards = this.cards.ToArray();
        cards.Shuffle();
        this.cards = cards.ToList();
    }

    public void Add(CardObject card)
    {
        card.Collection = this;
        cards.Add(card);
        card.transform.parent = this.transform;
    }

    public void Remove(CardObject card)
    {
        if (card.Collection == this)
            card.Collection = null;

        cards.Remove(card);
    }

    public CardObject DrawCard()
    {
        if (cards.Count > 0)
        {
            int index = cards.Count - 1;
            CardObject card = cards[index];
            cards.RemoveAt(index);
            return card;
        }

        return null;
    }

    public void MoveAllTo(CardCollection collection)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            collection.Add(cards[i]);
        }
        cards = new List<CardObject>();
    }
}
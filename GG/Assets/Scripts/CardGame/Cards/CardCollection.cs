using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardCollectionType
{
    Stack, List, Fan
}

public class CardCollection : MonoBehaviour
{
    public CardCollectionType collectionType;
    private List<CardObject> cards = new List<CardObject>();

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardObject current = cards[i];
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
                            break;

                        case CardCollectionType.List:
                            {
                                float offset = 0;
                                if (cards.Count > 1)
                                    offset = width / (cards.Count - 1) * i - width * 0.5f;
                                current.transform.localPosition = new Vector3(offset, 0, 0);
                                break;
                            }

                        case CardCollectionType.Fan:
                            float angle = Mathf.PI / 4 / cards.Count * i;
                            current.transform.rotation.SetAxisAngle(Vector3.back, angle);
                            break;
                    }
                }
            }
        }

        if (CardObject.HoveredCard != null)
            CardObject.HoveredCard.transform.SetAsLastSibling();
    }

    public void Add(CardObject card)
    {
        cards.Add(card);
        card.transform.parent = this.transform;
    }
}
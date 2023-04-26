using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum CardGroup
{
    None, Health, Item, Sword, Shield
}

public class CardInfo
{
    #region Fields

    private string name = "<Error>";
    private CardGroup group = CardGroup.None;
    private EffectInfo effect = null;

    #endregion Fields

    #region ctor

    public CardInfo(string name, CardGroup group, EffectInfo effect)
    {
        this.name = name;
        this.group = group;
        this.effect = effect;
    }

    public CardInfo(XmlNode node)
    {
        name = node.Name;
    }

    public CardInfo()
    {
    }

    #endregion ctor

    #region Properties

    public string Name => name;
    public string TranslatedName => name;
    public CardGroup Group => group;
    public EffectInfo Effect => effect;

    #endregion Properties
}
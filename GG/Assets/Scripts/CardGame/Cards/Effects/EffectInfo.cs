using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Attack, Block, Status
}

public class EffectInfo
{
    private EffectType type;
    private int power = 0;
    private int cost = 1;

    public EffectInfo(EffectType type)
    {
        this.type = type;
    }

    public EffectType Type => type;

    public int Power { get => power; set => power = value; }
    public int Cost => cost;
}
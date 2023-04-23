using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCardGame : MonoBehaviour
{
    public float delta = 10;
    private int index = 0;

    private DieObject die;

    public void SpawnCard()
    {
        float position = index * delta;
        CardObject.Instanciate(new CardInfo("Test", CardGroup.Item, new EffectInfo()), new Vector2(position, -position));

        index++;
    }

    public void SpawnDie()
    {
        int[] dice = new int[] { 4, 6, 8, 12, 20 };
        float position = index * delta;
        die = DieObject.Instanciate(new DieInfo(dice[Random.Range(0, dice.Length)]), new Vector2(0, 0));
    }
}
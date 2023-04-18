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
        if (die == null)
            die = DieObject.Instanciate(new DieInfo(6), new Vector2(0, 0));

        die.Roll();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardGameManager : MonoBehaviour
{
    #region Fields

    private static readonly int[] diceValues = new int[] { 4, 6, 8, 12, 20 };

    public Text debugTxt;

    public CardCollection deck;
    public CardCollection hand;
    public CardCollection block;
    public CardCollection discard;

    private DieObject[] dice;

    private int drawAmount = 4;
    private int diceAmount = 3;
    private int dicePower = 0;

    private int playerHealth = 20;
    private int enemyHealth = 20;
    private int enemyAtk = 0;

    #endregion Fields

    #region Main-Loop

    private void Start()
    {
        for (int j = 0; j < 10; j++)
            deck.Add(CardObject.Instantiate(
                new CardInfo($"Test {j}", CardGroup.Weapon,
                new EffectInfo(j < 5 ? EffectType.Attack : EffectType.Block)),
                new Vector2(0, 0)));

        deck.Shuffle();
        EndRound();
    }

    private void Update()
    {
        CardObject[] handCards = hand.Cards;
        for (int i = 0; i < handCards.Length; i++)
        {
            if (handCards[i].transform.localPosition.y > 500)
                Play(handCards[i]);
        }

        if (debugTxt != null)
            debugTxt.text =
                $"Enemy: \n" +
                $"HP = {enemyHealth}\n" +
                $"ATK = {enemyAtk}\n" +
                $"Health: {playerHealth}";
    }

    #endregion Main-Loop

    public void Play(CardObject card)
    {
        if (card.Info.Effect.Cost == dice.Count(x => x.Selected))
        {
            //Apply die power
            int power = 0;
            {
                List<DieObject> dice = new List<DieObject>();
                for (int i = 0; i < this.dice.Length; i++)
                {
                    if (this.dice[i].Selected)
                    {
                        power += this.dice[i].Info.Value;
                        GameObject.Destroy(this.dice[i].gameObject);
                    }
                    else
                        dice.Add(this.dice[i]);
                }
                this.dice = dice.ToArray();
            }
            card.Info.Effect.Power = power;

            //Move card to correct collection
            if (card.Info.Effect.Type != EffectType.Block)
            {
                //Apply effect
#warning todo
                enemyHealth -= card.Info.Effect.Power;
                //Hand -> Discard
                Discard(card);
            }
            else
            {
                //Hand -> Block
                block.Add(card);

                //Discard if block is overfilled
                if (block.Count > 3)
                    Discard(block.Cards[0]);
            }
        }
    }

    public void Discard(CardObject card)
    {
        card.Info.Effect.Power = 0;
        if (card.Info.Group != CardGroup.Item)
            discard.Add(card);
        else
        {
            if (card.Collection != null)
                card.Collection.Remove(card);
            GameObject.Destroy(card.gameObject);
        }
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (deck.Count < 1)
            {
                discard.MoveAllTo(deck);
                deck.Shuffle();
            }

            if (deck.Count < 1)
                break;

            hand.Add(deck.DrawCard());
        }
    }

    public void RollDice()
    {
        //Destroy old dice
        if (dice != null)
            for (int i = 0; i < dice.Length; i++)
            {
                if (!dice[i].IsDestroyed())
                    GameObject.Destroy(dice[i].gameObject);
            }

        if (diceAmount > 0)
        {
            //Calculate die values
            int upgradeIndex = dicePower % diceAmount;
            int upgradePower = dicePower / diceAmount;

            //Create new dice
            dice = new DieObject[diceAmount];
            for (int i = 0; i < dice.Length; i++)
            {
                int dieIndex = upgradePower;
                if (i < upgradeIndex)
                    dieIndex += 1;
                dieIndex = Mathf.Min(dieIndex, diceValues.Length - 1);

                dice[i] = DieObject.Instantiate(new DieInfo(diceValues[dieIndex]), new Vector2(350, 600) - new Vector2(0, 150 * i));
                dice[i].Roll();
            }
        }
    }

    public void EndRound()
    {
        hand.MoveAllTo(discard);

        EnemyAttack();

        RollDice();
        DrawCards(drawAmount);
    }

    public void EnemyAttack()
    {
        if (enemyAtk > 0)
        {
            int atk = enemyAtk;
            CardObject[] blockCards = block.Cards;
            for (int i = 0; i < blockCards.Length; i++)
            {
                if (atk < blockCards[i].Info.Effect.Power)
                {
                    blockCards[i].Info.Effect.Power -= atk;
                    atk = 0;
                    break;
                }
                else
                {
                    atk -= blockCards[i].Info.Effect.Power;
                    //Apply block resolve effect
#warning todo
                    //Block -> Discard
                    Discard(blockCards[i]);
                }
            }
            playerHealth -= atk;
        }

        enemyAtk = Random.Range(2, 5);
    }
}
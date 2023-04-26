using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Activity
{
    Idle, Training, Healing, Diging, Sleeping, Fighting
}

public class StatsKeeper : MonoBehaviour
{
    private int DAY_START = 8;
    private int DAY_END = 18;
    private static float TIME_SPEED_ACTIVITY = 1800;
    private static float TIME_SPEED_NIGHT = 36000;

    public Text stats;
    public Image display;
    public TimeKeeper timeKeeper;
    public Sprite[] sprites;

    private float health = 500;
    private float strength = 1;
    private float tunnel = 0;

    private float enemyHealth;
    private float enemyStrength;
    private float attackTimer;
    private bool turn = true;

    private int skipStart = 0;

    private Activity currentActivity = Activity.Idle;

    // Start is called before the first frame update
    private void Start()
    {
        if (timeKeeper != null)
            timeKeeper.TimePerSecond = TIME_SPEED_ACTIVITY;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentActivity != Activity.Sleeping)
        {
            if (timeKeeper.CurrentTime.Hour > DAY_END - 1)
            {
                SkipDay();
            }
            else
            {
                float statChange = Time.deltaTime;

                switch (currentActivity)
                {
                    case Activity.Training:
                        strength += statChange;
                        break;

                    case Activity.Healing:
                        health += statChange * 15;
                        break;

                    case Activity.Diging:
                        if (health > 1)
                        {
                            health -= statChange * 3.5f;
                            tunnel += statChange * 2;
                        }
                        else
                            ChangeActivity(Activity.Idle);
                        break;

                    case Activity.Fighting:
                        if (health > 0)
                        {
                            //Fight
                            if (enemyHealth > 0)
                            {
                                if (attackTimer > 0)
                                    attackTimer -= Time.deltaTime;
                                else
                                {
                                    if (turn)
                                        enemyHealth -= strength;
                                    else
                                        health -= enemyStrength;

                                    turn = !turn;
                                    attackTimer = 1;
                                }
                            }
                            //Win
                            else
                            {
                                SkipDay();
                            }
                        }
                        //Lose
                        else
                        {

                        }
                        break;
                }
            }
        }
        else
        {
            if (!(timeKeeper.CurrentTime.Hour < DAY_START) && timeKeeper.CurrentDay > skipStart)
            {
                currentActivity = Activity.Idle;
                timeKeeper.TimePerSecond = TIME_SPEED_ACTIVITY;
                timeKeeper.Stopped = true;

                int day = timeKeeper.CurrentDay;
                if (day % 7 == 0)
                {
                    currentActivity = Activity.Fighting;
                    turn = true;

                    enemyHealth = 500 * day * 0.1f;
                    enemyStrength = 30 * day * 0.075f;
                }
            }
        }

        if (stats != null)
        {
            if (tunnel < 1000)
            {
                if (health > 0)
                {
                    string info =
                        $"Health: {health:f0}\n" +
                        $"Strength: {strength:f0}\n" +
                        $"Tunnel: {tunnel:f0}m";

                    if (currentActivity == Activity.Fighting)
                        info += $"\n" +
                            $"EnemyHealth: {enemyHealth:f0}\n" +
                            $"EnemyStrength: {enemyStrength:f0}";

                    stats.text = info;
                }
                else
                    stats.text = "You Lose";
            }
            else
                stats.text = "You Win";
        }

        if (display != null && (int)currentActivity < sprites.Length)
        {
            Sprite sprite = sprites[(int)currentActivity];
            display.sprite = sprite;
        }
    }

    public void ChangeActivity(Activity activity)
    {
        if (currentActivity != Activity.Sleeping && currentActivity != Activity.Fighting)
        {
            currentActivity = activity;
            timeKeeper.Stopped = activity == Activity.Idle;
        }
    }

    public void ChangeActivity(int activity)
    {
        ChangeActivity((Activity)activity);
    }

    public void SkipDay()
    {
        health += 50;

        currentActivity = Activity.Sleeping;
        timeKeeper.TimePerSecond = TIME_SPEED_NIGHT;
        timeKeeper.Stopped = false;
        skipStart = timeKeeper.CurrentDay;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Activity
{
    Idle, Training, Healing, Diging, Sleeping
}

public class StatsKeeper : MonoBehaviour
{
    private int DAY_START = 8;
    private int DAY_END = 18;
    private static float TIME_SPEED_ACTIVITY = 600;
    private static float TIME_SPEED_NIGHT = 3600;

    public Text stats;
    public Image display;
    public TimeKeeper timeKeeper;
    public Sprite[] sprites;

    private float health = 50;
    private float strength = 1;
    private float tunnel = 0;

    private int skipStart = 0;

    private Activity currentActivity = Activity.Idle;

    // Start is called before the first frame update
    private void Start()
    {
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
                float statChange = Time.deltaTime * 0.1f;

                switch (currentActivity)
                {
                    case Activity.Training:
                        strength += statChange;
                        break;

                    case Activity.Healing:
                        health += statChange;
                        break;

                    case Activity.Diging:
                        if (health > 1)
                        {
                            statChange *= 5;

                            health -= statChange;
                            tunnel += statChange;
                        }
                        else
                            ChangeActivity(Activity.Idle);
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
            }
        }

        if (stats != null)
        {
            stats.text =
                $"Health: {health:f0}\n" +
                $"Strength: {strength:f0}\n" +
                $"Tunnel: {tunnel:f0}m";
        }

        if (display != null && (int)currentActivity < sprites.Length)
        {
            Sprite sprite = sprites[(int)currentActivity];
            display.sprite = sprite;
        }
    }

    public void ChangeActivity(Activity activity)
    {
        if (currentActivity != Activity.Sleeping)
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
        currentActivity = Activity.Sleeping;
        timeKeeper.TimePerSecond = TIME_SPEED_NIGHT;
        timeKeeper.Stopped = false;
        skipStart = timeKeeper.CurrentDay;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
    public Text timeLabel;

    private DateTime time = DateTime.MinValue + TimeSpan.FromHours(8);
    private float timePerSecond = 600;
    private bool stopped = true;

    public DateTime CurrentTime => time;
    public int CurrentDay => (int)(time - DateTime.MinValue).TotalDays + 1;
    public float TimePerSecond { get => timePerSecond; set => timePerSecond = value; }
    public bool Stopped { get => stopped; set => stopped = value; }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!stopped)
            time += TimeSpan.FromSeconds(Time.deltaTime * timePerSecond);

        if (timeLabel != null)
        {
            timeLabel.text = $"{time.ToString("HH:mm")} Day: {CurrentDay}";
        }
    }
}
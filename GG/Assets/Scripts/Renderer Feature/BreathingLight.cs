using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingLight : MonoBehaviour
{
    public Light[] lights;
    public AnimationCurve animationCurve;
    public float speed = 1;
    private float passtime = 0;

    void Update()
    {
        foreach (Light light in lights)
        {
            light.range = animationCurve.Evaluate(passtime) * 10;
            passtime += speed * Time.deltaTime;
            if( passtime > 2) 
                passtime = 0;
        }
    }
}

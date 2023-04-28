using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingLight : MonoBehaviour
{

    public Light[] l;
    public AnimationCurve a;
    public float speed = 1;
    float passtime = 0;

    void Update()
    {
        foreach (Light light in l)
        {
            light.range = a.Evaluate(passtime) * 10;
            passtime += speed * Time.deltaTime;
            if( passtime > 2) 
                passtime = 0;
        }
    }
}

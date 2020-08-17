using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFeedback : AFeedback
{

    public float slowTime;
    public float duration;

    public bool Smoothed;

    public Action OnComplete;

    public override void Play(GameObject owner,object objectToPass)
    {
        StartCoroutine(PlayCoroutine());
    }

    public override IEnumerator PlayCoroutine()
    {
        float timeTemp = Time.timeScale;

        if(!Smoothed)
            Time.timeScale = slowTime;
        else 
        {
            while(Time.timeScale > slowTime)
            {
                if(Time.timeScale - 0.1f < 0)
                    break;
                
                Time.timeScale -= Time.deltaTime / 0.1f;

                yield return waitFrameUtil;
            }
        }

        yield return new WaitForSeconds(duration);

        while (Time.timeScale < timeTemp)
        {
            Time.timeScale +=  Mathf.Clamp(Time.deltaTime + 0.1f,0,timeTemp);

            yield return waitFrameUtil;
        }

        Time.timeScale = 1;

        OnComplete?.Invoke();

    }
}

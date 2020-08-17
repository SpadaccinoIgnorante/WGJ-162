using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AFeedback : MonoBehaviour
{
    protected Coroutine playCoroutine;
    protected GameObject owner;

    protected WaitForEndOfFrame waitFrameUtil;
    
    public abstract void Play(GameObject owner,object objectToPass);

    public abstract IEnumerator PlayCoroutine();

    public virtual void Stop()
    {
        if (playCoroutine != null) StopCoroutine(playCoroutine);
    }
}

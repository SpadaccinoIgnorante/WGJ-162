using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seashell : MonoBehaviour
{
    public float radius;

    public LayerMask mask;
    
    public TimeFeedback timeFeedback;

    public static bool IsSlowing;

    void Start()
    {
        timeFeedback.OnComplete += () => 
        {
            IsSlowing = false;
        };
    }

    void FixedUpdate()
    {
        var cols =  Physics.OverlapSphere(transform.position,radius,mask);

        if(cols.Length == 0)return;

        foreach (var col in cols)
        {
            if(!LayerUtils.IsInLayerMask(col.gameObject.layer,mask))continue;

            // TODO: Slow Time

            IsSlowing = true;

            timeFeedback?.Play(null,null);

            return;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
          
        Gizmos.DrawSphere(transform.position,radius);
    }

    void OnDestroy()
    {
        IsSlowing = false;
    }
}


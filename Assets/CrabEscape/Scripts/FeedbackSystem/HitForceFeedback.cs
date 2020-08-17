using System.Collections;
using UnityEngine;

public class HitForceFeedback : AFeedback
{
    [SerializeField] protected float impactForce;
    [SerializeField] protected ForceMode forceToApply;

    protected Rigidbody rb;

    public override void Play(GameObject owner, object objectToPass)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        Vector3 direction = (Vector3)objectToPass;

        ApplyHit(impactForce, direction);
    }

    public override IEnumerator PlayCoroutine()
    {
        yield return null;
    }

    protected virtual void ApplyHit(float forceOnHit, Vector3 direction)
    {
        if (rb == null || impactForce <= 0) return;

        rb.AddForce(direction * forceOnHit, forceToApply);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashingFeedback : AFeedback
{
    [SerializeField]
    private float _flashingDuration;
    [SerializeField]
    private float _flashingFrequency;

    [SerializeField]
    private List<MeshRenderer> _meshRenderersToFlash = new List<MeshRenderer>();

    private bool _isFlashing;

    public override void Play(GameObject owner, object objectToPass)
    {
        if (objectToPass != null)
            _flashingDuration = (float)objectToPass;

        var mrs = owner.GetComponents<MeshRenderer>();
        var mrsChild = owner.GetComponentsInChildren<MeshRenderer>();
        var mrsParent = owner.GetComponentsInParent<MeshRenderer>();

        foreach (var mr in mrs)
        {
            if (mr == null) continue;

            if (!_meshRenderersToFlash.Contains(mr))
                _meshRenderersToFlash.Add(mr);
        }

        foreach (var mr in mrsChild)
        {
            if (mr == null) continue;
            if (!_meshRenderersToFlash.Contains(mr))
                _meshRenderersToFlash.Add(mr);
        }

        foreach (var mr in mrsParent)
        {
            if (mr == null) continue;

            if (!_meshRenderersToFlash.Contains(mr))
                _meshRenderersToFlash.Add(mr);
        }

        playCoroutine = StartCoroutine(PlayCoroutine());
    }

    public override IEnumerator PlayCoroutine()
    {
        _isFlashing = true;

        var fRoutine = StartCoroutine(FlashRoutine());

        yield return new WaitForSeconds(_flashingDuration);

        _isFlashing = false;

        StopCoroutine(fRoutine);

        Stop();
    }

    public override void Stop()
    {
        base.Stop();

        foreach (var mr in _meshRenderersToFlash)
        {
            if (mr == null) continue;

            mr.enabled = true;
        }
    }

    private IEnumerator FlashRoutine()
    {
        while (_isFlashing)
        {
            foreach (var mr in _meshRenderersToFlash)
            {
                if (mr == null) continue;

                mr.enabled = false;
            }

            yield return new WaitForSeconds(_flashingFrequency / 2);

            foreach (var mr in _meshRenderersToFlash)
            {
                if (mr == null) continue;

                mr.enabled = true;
            }

            yield return new WaitForSeconds(_flashingFrequency / 2);
        }
    }
}

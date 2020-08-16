using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RaycastSide
{
    Left,
    Right
}

[Serializable]
public class WallEventHit : UnityEvent<Transform,RaycastSide> { }

public class SideRaycaster : MonoBehaviour
{
    [SerializeField]
    private RaycastSide side;

    [SerializeField]
    private float raycastLenght;

    [SerializeField]
    private Vector3 rayacastPos;

    [SerializeField]
    private LayerMask layerMask;

    [Space(10)]
    public WallEventHit OnRaycastHit;

    [Header("DEBUG")]
    [SerializeField]
    private bool _isOnWall;

    private Vector3 vectorDirection;

    private void Start()
    {
        if (side == RaycastSide.Left)
            vectorDirection = Vector3.left * raycastLenght;
        else
            vectorDirection = Vector3.right * raycastLenght;
    }

    private void FixedUpdate()
    {
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + rayacastPos,
                           vectorDirection,
                           out hit,
                           raycastLenght, layerMask))
        {
            if (!LayerUtils.IsInLayerMask(hit.transform.gameObject.layer, layerMask)) return;

            Debug.Log("Hitted : " + hit.transform.name);

            _isOnWall = true;

            OnRaycastHit?.Invoke(hit.transform, side);
        }
        else
        {
            if (_isOnWall)
                OnRaycastHit?.Invoke(null,side);
            _isOnWall = false;
        }
    }

    private void OnDrawGizmos()
    {
        switch (side)
        {
            case RaycastSide.Left:
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position + rayacastPos,Vector3.left * raycastLenght);
                break;
            case RaycastSide.Right:
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position + rayacastPos, Vector3.right * raycastLenght);
                break;
        }
        
    }
}

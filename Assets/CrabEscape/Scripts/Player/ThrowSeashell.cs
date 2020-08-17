using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrowSeashell : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTr;

    [SerializeField]
    private GameObject _seaShell;

    [SerializeField]
    private float _throwForceX;

    [SerializeField]
    private float _throwForceY;

    [SerializeField]
    private float _throwForceZ;

    [SerializeField]
    private float _parabolaXTimer;

    [SerializeField]
    private float _parabolaYTimer;

    public ForceMode ThrowXForce;


    public ForceMode ThrowYForce;

    public Transform _spawnTr;

    private MovementController _mC;

    void Start()
    {
        _mC = GetComponent<MovementController>();
    }

    void Update()
    {
        if(CheckThrow())
        {
            Throw(GameObject.Instantiate(_seaShell,_spawnTr.position,Quaternion.identity));
        }
    }

    private bool CheckThrow()
    {
        return Input.GetButtonDown("Fire1");
    }

    void Throw(GameObject projectile)
    {
        var rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(_mC.GetDirection() * (_throwForceX),ThrowXForce);

        StartCoroutine(SeaShellRoutine(rb));
        
    }  

    IEnumerator SeaShellRoutine(Rigidbody rb)
    {
        yield return new WaitForSeconds(_parabolaXTimer);

        rb.GetComponent<Collider>().isTrigger = false;

        rb.AddForce(rb.velocity.x,_throwForceY,rb.velocity.z,ThrowYForce);

        yield return new WaitForSeconds(_parabolaYTimer);


        while (rb.velocity.y > 0)
        {
            rb.velocity = new Vector3(0,-0.1f * Time.fixedDeltaTime,0);

            yield return null;
        }
        //rb.AddForce(0,-_throwForceY,0,ThrowYForce);

        rb.useGravity = true;
    }
}

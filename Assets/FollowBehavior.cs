using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour
{
    [SerializeField] Transform leader;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] float followDistance = 2f;
    [SerializeField] LayerMask neighborLayer;
    [SerializeField] float neighborRadius = 2f;
    [SerializeField] float maxSeparation = 2f;

    private Vector3 leaderBehind;

    private List<Transform> context = new List<Transform>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Que sigan directamente al lider para rodearlo, en lugar de su behind
        leaderBehind = leader.position - (leader.forward * followDistance);
        Vector3 flockMove = Vector3.zero;
        GetContext();
        flockMove += ArriveForce();
        flockMove += SeparationForce();
        flockMove += AlignmentForce();

        transform.position += flockMove * Time.deltaTime;
	}

    public void GetContext()
    {
        var colliders = Physics.OverlapSphere(transform.position, neighborRadius, neighborLayer);
        context = new List<Transform>();
        foreach (var c in colliders)
        {
            if (c.transform != this.transform)
            {
                context.Add(c.transform);
            }
        }
    }

    public Vector3 ArriveForce()
    {
        Vector3 dir = (leaderBehind - transform.position).normalized;
        return maxSpeed * dir;
    }

    public Vector3 SeparationForce()
    {
        if (context.Count == 0)
            return Vector3.zero;

        Vector3 separationVector = Vector3.zero;
        int neighborCount = 0;

        for (int i = 0; i < context.Count; i++)
        {
            if (Vector3.Distance(context[i].position, transform.position) <= neighborRadius)
            {
                neighborCount++;
                Vector3 dist = transform.position - context[i].position;
                separationVector += dist;
            }
        }
        if (neighborCount > 0)
        {
            separationVector /= neighborCount;
            separationVector.Normalize();
            separationVector *= maxSeparation;
        }

        return separationVector;
    }

    public Vector3 AlignmentForce()
    {
        if (context.Count == 0)
            return Vector3.zero;

        Vector3 alignmentMove = Vector3.zero;

        foreach (Transform t in context)
        {
            alignmentMove += t.forward;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask ground;
    public Vector3 destination;
    public delegate void State();
    public State doAction;
    public int firingRange = 25;

    void Start()
    {
        destination = transform.position;
        doAction = Patrol;
        
    }

    public void Patrol()
    {
        agent.stoppingDistance = 0;
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void Engage()
    {
        agent.stoppingDistance = firingRange;
        if (getDistanceTo(transform.position, destination) < firingRange)
        {
            transform.LookAt(destination);
            Ray lineOfSight = new Ray(transform.position, transform.forward * firingRange);
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(lineOfSight, out hitInfo, firingRange);
            Debug.DrawRay(transform.position, transform.forward * firingRange, Color.blue);
            if(hitInfo.collider != null && hitInfo.collider.gameObject.tag == tag) 
            {
                Debug.Log(" friendly fire ");
 
            }
            else
            {
                Fire();
            }

        }
        else
        {
            agent.SetDestination(destination);
        }

    }
    private double getDistanceTo(Vector3 from, Vector3 to)
    {
        return Math.Sqrt((from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y) + (from.z - to.z) * (from.z - to.z));
    }
    private void Fire()
    {
        Debug.DrawLine(transform.position, destination,Color.red,1);
    }

    //private void Spread(Vector3 dir, )
    
    void Update()
    {
        doAction();
        
    }
}

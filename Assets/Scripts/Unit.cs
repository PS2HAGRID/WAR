using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public LayerMask ground;
    public Vector3 destination;
    public delegate void State();
    public State doAction;
    public int firingRange = 25;
    public int stoppingDistanceOffset = 2;
    bool dead = false;
    Coroutine rateOfFire;
    public float spread;
    void Start()
    {
        
    }

    public void Patrol()
    {
        target = null;
        agent.stoppingDistance = stoppingDistanceOffset;
        agent.isStopped = false;
        agent.SetDestination(destination);
        Debug.Log(agent.SetDestination(destination));
    }

    public void Engage()
    {
        agent.stoppingDistance = firingRange - stoppingDistanceOffset;
        if (getDistanceTo(transform.position, target.transform.position) < firingRange)
        {
            transform.LookAt(target.transform.position);
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
                if(hitInfo.collider.gameObject.tag != "Untagged")
                {
                    Fire();
                }
                else
                {
                    target = null;
                }

            }

        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag != tag && collider.gameObject.tag != "Untagged")
        {
            target = collider.gameObject;
        }
    }


    private double getDistanceTo(Vector3 from, Vector3 to)
    {
        return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y) + (from.z - to.z) * (from.z - to.z));
    }
    private void Fire()
    {
        if (rateOfFire == null)
            rateOfFire = StartCoroutine(DoShoot());

        IEnumerator DoShoot()
        {
            RaycastHit hit = new RaycastHit();
            Ray bullet = new Ray(transform.position, Spread(transform.forward, spread));
            if (Physics.Raycast(bullet, out hit))
            {
                if (hit.transform.tag != "Untagged")
                {
                    hit.transform.SendMessage("Hit");
                }
            }


            Debug.DrawRay(transform.position, transform.forward * 9999);
            yield return new WaitForSeconds(0.3f);
            rateOfFire = null;
        }
    }

    private void Hit()
    {
        Destroy(agent);
        dead = true;
        tag = "Untagged";
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0.05f;
    }


    private Vector3 Spread(Vector3 dir, float maxSpreadAngle)
    {
        maxSpreadAngle *= 0.5f;
        Vector3 spread = new Vector3(dir.x * Random.Range(-maxSpreadAngle, maxSpreadAngle), dir.y * Random.Range(-maxSpreadAngle, maxSpreadAngle), dir.z) * firingRange;
        return spread;
    }
    
    void Update()
    {
        if (dead)
        {
            return;
        }



        if (target != null)
        {
            if (target.tag == "Untagged")
            {
                target = null;
                return;
            }
            else
            {
                doAction = Engage;
            }

        }
        else
        {
            doAction = Patrol;
        }






        doAction();
        
    }
}

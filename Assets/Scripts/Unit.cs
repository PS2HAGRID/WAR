using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public GameObject parent;

    public NavMeshAgent agent;
    public GameObject bulletPrefab;
    public GameObject target;
    public LayerMask ground;
    public Vector3 destination;
    public delegate void State();
    public State doAction;
    public int firingRange = 25;
    public int stoppingDistanceOffset = 2;
    bool dead = false;
    Coroutine rateOfFire;
    Coroutine deadTimer;
    float timeBeforeDeath = 3;

    public float spread;
    public int rpm = 600;
    float timeBetweenBullets;
    public int ammoInMag = 30;
    int shotsFired;
    public float timeToReload = 2.5f;


    void Start()
    {
        timeBetweenBullets = 60 / rpm;
    }

    public void Patrol()
    {
        target = null;
        agent.stoppingDistance = stoppingDistanceOffset;
        agent.isStopped = false;
        agent.SetDestination(destination);
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
            }
            else
            {
                if(hitInfo.collider == null)
                {
                    return;
                }

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


            
            if(shotsFired < ammoInMag)
            {
                BulletScript bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.rotation.eulerAngles)).GetComponent<BulletScript>();
                bullet.speed = 1;
                bullet.parent = gameObject;

                Debug.Log("firing");

                shotsFired++;

                yield return new WaitForSeconds(timeBetweenBullets);
                rateOfFire = null;
            } 
            else
            {
                Debug.Log("reloading");
                shotsFired = 0;
                yield return new WaitForSeconds(timeToReload);
                rateOfFire = null;
                Debug.Log("finished reloading");
            }

        }

    }

    private void Hit()
    {
        if (dead)
        {
            return;
        }
        
        Destroy(agent);
        dead = true;
        tag = "Untagged";
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0.05f;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.interpolation = RigidbodyInterpolation.None;
        deadTimer = StartCoroutine(DoDeath());

        IEnumerator DoDeath()
        {
            yield return new WaitForSeconds(timeBeforeDeath);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<CapsuleCollider>().enabled = false;
            deadTimer = null;
        }
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

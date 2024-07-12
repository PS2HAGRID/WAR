using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector3 velocity;
    public float speed;
    public float lifetime = 5;
    public GameObject parent;
    private void Start()
    {
        velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 lastPos = transform.position;
        transform.position += velocity;
        CheckForCollision(lastPos);
    }

    private void CheckForCollision(Vector3 lastPos)
    {
        Vector3 currentPos = transform.position;
        Vector3 directionToCurrentPos = new Vector3(lastPos.x - currentPos.x, lastPos.y - currentPos.y, lastPos.z - currentPos.z).normalized;
        Ray collisionCheckRay = new Ray(lastPos, directionToCurrentPos);
        RaycastHit hit;

        if (Physics.Raycast(collisionCheckRay, out hit, velocity.z))
        {
            if (hit.collider == null || hit.collider.gameObject == parent)
            {
                return;
            }

            transform.position = hit.collider.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == parent)
        {
            return;
        }


        collision.transform.SendMessage("Hit");
        Destroy(gameObject);
    }
}

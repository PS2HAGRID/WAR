using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{ 
    Vector3 velocity;
    public bullet(float speed)
    {
        velocity = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lastPos = transform.position;
        transform.position += velocity;
        CheckForCollision(lastPos);
    }

    private void CheckForCollision(Vector3 lastPos)
    {

    }
}

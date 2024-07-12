using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainCam.enabled = !mainCam.enabled;

        }
    }
}

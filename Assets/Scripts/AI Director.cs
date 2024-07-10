using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    public Camera MainCamera;
    public Unit[] units = null;

    void Update()
    {
        if(units == null)
        {
            return;
        }

        foreach (Unit unit in units)
        {
            Vector3 destination;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                
                Ray rey = MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(rey, out hit))
                {
                    destination = hit.point;

                    unit.destination = destination;
                }


                if (Input.GetMouseButtonDown(1))
                {
                    unit.doAction = unit.Engage;
                }
                else
                {
                    unit.doAction = unit.Patrol;
                }
            }
        }
    }

}

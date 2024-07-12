using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{

    //debug
    //public Unit[] units = null;

    public int ID;
    public GameObject target;
    public GameObject unit;
    private int unitcap;
    public List<List<Unit>> Squads;
    public BoxCollider boundingBox;

    public AIDirector(int unitCap)
    {
        unitcap = unitCap;
    }

    private void Start()
    {
        Squads = new List<List<Unit>>();

        CreateSquad(10);

        //debug
        //Squads[0].Add(units[0]);

    }

    void Update()
    {
        if(Squads == null)
        {
            return;
        }
        if(target != null) 
        {
            for(int i = 0; i < Squads.Count; i++)
            {
                Investigate(i, target);
            }

            
        }
    }

    private void EngageTarget(int SquadID, GameObject target)
    {

        Ray POIonNavMesh = new Ray(target.transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(POIonNavMesh,out hit))
        {
            Vector3 destination = hit.point;
            foreach (Unit unit in Squads[SquadID])
            {
                unit.target = target;
                unit.destination = destination;
            }
        }
    }

    private void Investigate(int SquadID, GameObject target)
    {
        Ray POIonNavMesh = new Ray(target.transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(POIonNavMesh, out hit))
        {
            Vector3 destination = hit.point;
            foreach (Unit unit in Squads[SquadID])
            {
                unit.destination = destination;
            }
        }
    }


    private void CreateSquad(int numUnits)
    {

        List<Unit> currentSquad = new List<Unit>();

        for (int i = 0; i < numUnits; i++)
        {
            Unit currentUnit = Instantiate(unit, transform).GetComponent<Unit>();
            currentSquad.Add(currentUnit);
            currentUnit.destination = transform.position;
            currentUnit.tag = "Team" + ID;
        }

        Squads.Add(currentSquad);
        Debug.Log("created squad");
    }
}

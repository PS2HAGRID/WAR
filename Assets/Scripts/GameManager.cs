using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int unitCap = 1000;
    private int numOfDirectors = 2;
    public GameObject AIDirector;
    public GameObject enemyContainer;
    private AIDirector[] aIDirectors = new AIDirector[2];
    public GameObject[] aIDirectorsSpawns = new GameObject[2];

    public GameObject debugTarget;

    private void CreateDirectors(int numOfDirectors)
    {
        for (int i = 0; i < numOfDirectors; i++)
        {
            Transform point = aIDirectorsSpawns[i].transform.GetChild(0).gameObject.transform;
            BoxCollider spawnBounds = aIDirectorsSpawns[i].GetComponent<BoxCollider>();
            AIDirector currentDirector = Instantiate(AIDirector, ChooseRandomPosInArea(point, spawnBounds)).GetComponent<AIDirector>() ;
            currentDirector.transform.parent = enemyContainer.transform;
            aIDirectors[i] = currentDirector;
            
            if(debugTarget != null)
            {
                currentDirector.target = debugTarget;
            }

            currentDirector.ID = i + 1;
            currentDirector.boundingBox = aIDirectorsSpawns[i].GetComponent<BoxCollider>();
        }


    }

    private Transform ChooseRandomPosInArea(Transform point, BoxCollider bounds)
    {
        Vector3 extents = bounds.size * 0.5f;
        point.localPosition = new Vector3(Random.Range(-extents.x, extents.x),
                                     Random.Range(-extents.y, extents.y),
                                     Random.Range(-extents.z, extents.z));
        return point;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDirectors(numOfDirectors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

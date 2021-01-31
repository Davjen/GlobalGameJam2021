using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalziniSpawnerNonsense : MonoBehaviour
{
    public List<Transform> SpawnPoints, TargetPoints;
    public GameObject[] CalzinoPrefab;

    Queue<GameObject> CalzinoPooling = new Queue<GameObject>();
    public float AmountOfCalzini, Speed,timer,AmountofTime;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int rnd = Random.Range(0, 2);
            int rndSPawn = Random.Range(0, SpawnPoints.Count);
            GameObject go = Instantiate(CalzinoPrefab[rnd], SpawnPoints[rndSPawn].position, Quaternion.identity, transform);
            CalzinoPooling.Enqueue(go);
        }
    }

    // Update is called once per frame
    void Update()
    {

            int rnd = Random.Range(0, TargetPoints.Count);
            transform.position = Vector3.Lerp(transform.position, TargetPoints[rnd].position, timer / AmountofTime);


    }
}

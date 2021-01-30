using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabObstacles;
    private List<GameObject> activeObj;
    public int MaxObjActive;
    public float delaySpawn;
    public int minDistSpawn, maxDistSpawn;
    private int currentObjSpawned;
    private bool CanSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        StartGame(1);
        activeObj = new List<GameObject>();
    }

    //EVENT
    private void StartGame(int maxObjToSpawn)
    {
        CanSpawn = true;
        currentObjSpawned = 0;
        InvokeRepeating("Spawn", 0.1f, delaySpawn);
    }
    public void DestroyObj(GameObject objToRemove)
    {
        currentObjSpawned--;
        activeObj.Remove(objToRemove);
        Destroy(objToRemove);
    }
    private void Spawn()
    {
        if (CanSpawn)
        {
            int index = Random.Range(0, prefabObstacles.Count);
            float radius = Random.Range(minDistSpawn, maxDistSpawn);
            float angle = Random.Range(0, 361);

            float newPosX = transform.position.x + (radius * Mathf.Cos((angle * Mathf.PI) / 180));
            float newPosZ = transform.position.z + (radius * Mathf.Sin((angle * Mathf.PI) / 180));

            Vector3 pos = new Vector3(newPosX, transform.position.y, newPosZ);
            GameObject objToSpawn = prefabObstacles[index];
            GameObject obj = Instantiate(objToSpawn,transform);
            ObstacleMovement script;
            obj.TryGetComponent<ObstacleMovement>(out script);
            script.spawnCenter = transform;
            
            activeObj.Add(obj);
            obj.transform.position = pos;
            currentObjSpawned++;
            if (currentObjSpawned >= MaxObjActive)
            {
                CanSpawn = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

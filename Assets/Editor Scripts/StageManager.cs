using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EditMode { OrbitsEditor, PlatformsEditor, ConfigurationsEditor, VelocitiesEditor}

public enum Configuration { Config1, Config2, Config3, None}

[ExecuteInEditMode]
public class StageManager : MonoBehaviour
{
    public Transform WashingMachine;
    public GameObject OrbitPrefab, PlatformPrefab;
    public EditMode Mode = EditMode.OrbitsEditor;
    public int CurrentOrbit = 0;
    public float OrbitRadius = 0f;
    public float PlatformAngle = 0f;
    public Vector2 PlatformOffset;
    public Configuration Configuration= Configuration.None;
    public float OrbitVelocity;
    public bool SpawnOrbit;
    public bool RemoveOrbit;
    public bool SpawnPlatform;
    public bool RemoveLastPlatform;
    public bool SpawnConfiguration;
    public bool SaveConfiguration;
    public bool SetVelocity;
    public bool Reset;

    Dictionary<int, Transform> orbits = new Dictionary<int, Transform>();
    Dictionary<int,List<Transform>> platforms=new Dictionary<int, List<Transform>>();
    Dictionary<int, float> velocities = new Dictionary<int, float>();
    Dictionary<int, Vector3[][]> configurationsPosition = new Dictionary<int, Vector3[][]>();
    Dictionary<int, Vector3[][]> configurationsScale = new Dictionary<int, Vector3[][]>();



    int orbitsCounter = 0;
    GameObject lastPlatform;

   
    void Update()
    {
        switch (Mode)
        {
            case EditMode.OrbitsEditor:
                if (SpawnOrbit)
                    OnSpawnOrbit();

                if (RemoveOrbit)
                    OnRemoveOrbit();

                break;
            case EditMode.PlatformsEditor:
                if (SpawnPlatform)
                    OnSpawnPlatform();

                if (RemoveLastPlatform)
                    OnRemovePlatform();
                break;
            case EditMode.ConfigurationsEditor:
                if (SaveConfiguration)
                    OnSaveConfiguration();

                if (SpawnConfiguration)
                    OnSpawnConfiguration();
                break;

        }

        if (Reset)
            OnReset();
            
    }


    public void OnSpawnOrbit()
    {
        if (OrbitRadius != 0)
        {
            GameObject go = Instantiate(OrbitPrefab, WashingMachine.position, Quaternion.identity, WashingMachine);
            go.transform.localScale = new Vector3(OrbitRadius, go.transform.localScale.y, OrbitRadius);

            while (orbitsCounter == 0 || orbits.ContainsKey(orbitsCounter))
            {
                orbitsCounter++;

            }

            orbits[orbitsCounter] = go.transform;

        }
        SpawnOrbit = false;
    }

    public void OnRemoveOrbit()
    {
        if (CurrentOrbit != 0)
        {
            if (orbits.ContainsKey(CurrentOrbit))
            {
                DestroyImmediate(orbits[CurrentOrbit].gameObject);
                orbits.Remove(CurrentOrbit);
                platforms.Remove(CurrentOrbit);
                configurationsScale.Remove(CurrentOrbit);
                orbitsCounter = 0;
            }
        }


        RemoveOrbit = false;
    }

    public void OnSpawnPlatform()
    {
        if (PlatformPrefab != null)
        {
            if (CurrentOrbit != 0 && orbits.ContainsKey(CurrentOrbit))
            {
                float radius = orbits[CurrentOrbit].transform.localScale.x * 0.5f;
                float newPosX = WashingMachine.position.x + (radius * Mathf.Cos((PlatformAngle * Mathf.PI) / 180));
                float newPosZ = WashingMachine.position.z + (radius * Mathf.Sin((PlatformAngle * Mathf.PI) / 180));

                Vector3 pos = new Vector3(newPosX + PlatformOffset.x, WashingMachine.position.y, newPosZ + PlatformOffset.y);

                lastPlatform = Instantiate(PlatformPrefab, pos, Quaternion.identity);
                Vector3 fwd = (WashingMachine.position - lastPlatform.transform.position).normalized;
                lastPlatform.transform.rotation = Quaternion.LookRotation(fwd);
                lastPlatform.transform.SetParent(orbits[CurrentOrbit]);

                if (!platforms.ContainsKey(CurrentOrbit))
                    platforms[CurrentOrbit] = new List<Transform>();

                platforms[CurrentOrbit].Add(lastPlatform.transform);
            }


        }

        SpawnPlatform = false;
    }

    public void OnRemovePlatform()
    {
        if (lastPlatform != null)
            DestroyImmediate(lastPlatform);

        RemoveLastPlatform = false;
    }


    public void OnSaveConfiguration()
    {
        if (Configuration != Configuration.None)
        {


            if (orbits.ContainsKey(CurrentOrbit))
            {
                if (platforms.ContainsKey(CurrentOrbit) && platforms[CurrentOrbit] != null)
                {
                    if (!configurationsPosition.ContainsKey(CurrentOrbit))
                        configurationsPosition[CurrentOrbit] = new Vector3[(int)Configuration.None][];

                    if (!configurationsScale.ContainsKey(CurrentOrbit))
                        configurationsScale[CurrentOrbit] = new Vector3[(int)Configuration.None][];

                    configurationsPosition[CurrentOrbit][(int)Configuration] = new Vector3[platforms[CurrentOrbit].Count];
                    configurationsScale[CurrentOrbit][(int)Configuration] = new Vector3[platforms[CurrentOrbit].Count];

                    for (int i = 0; i < platforms[CurrentOrbit].Count; i++)
                    {
                        if (platforms[CurrentOrbit][i] != null)
                        {
                            configurationsPosition[CurrentOrbit][(int)Configuration][i] = platforms[CurrentOrbit][i].position;
                            configurationsScale[CurrentOrbit][(int)Configuration][i] = platforms[CurrentOrbit][i].localScale;

                        }
                    }



                }
            }

        }
        SaveConfiguration = false;
    }

    public void OnSpawnConfiguration()
    {
        if (Configuration != Configuration.None)
        {
            if (orbits.ContainsKey(CurrentOrbit))
            {
                if (configurationsPosition[CurrentOrbit][(int)Configuration] != null && configurationsScale[CurrentOrbit][(int)Configuration]!=null)
                {
                    for (int i = 0; i < platforms[CurrentOrbit].Count; i++)
                    {
                        if (platforms[CurrentOrbit][i] != null)
                            DestroyImmediate(platforms[CurrentOrbit][i].gameObject);
                    }

                    for (int i = 0; i < configurationsPosition[CurrentOrbit][(int)Configuration].Length; i++)
                    {
                        GameObject go = Instantiate(PlatformPrefab, configurationsPosition[CurrentOrbit][(int)Configuration][i], Quaternion.identity);
                        Vector3 fwd = (WashingMachine.position - go.transform.position).normalized;
                        go.transform.rotation = Quaternion.LookRotation(fwd);
                        go.transform.SetParent(orbits[CurrentOrbit]);
                        go.transform.localScale = configurationsScale[CurrentOrbit][(int)Configuration][i];

                        if (!platforms.ContainsKey(CurrentOrbit))
                            platforms[CurrentOrbit] = new List<Transform>();

                        platforms[CurrentOrbit].Add(go.transform);
                    }

                }




            }



        }


        SpawnConfiguration = false;
    }
    public void OnReset()
    {
            foreach (KeyValuePair<int, Transform> pairs in orbits)
            {
                DestroyImmediate(pairs.Value.gameObject);
            }

            orbits.Clear();
            for (int i = WashingMachine.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(WashingMachine.GetChild(i).gameObject);
            }

            orbitsCounter = 0;
            Reset = false;
        
    }

}

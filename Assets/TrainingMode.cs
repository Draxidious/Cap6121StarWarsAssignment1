using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMode : MonoBehaviour
{
    [SerializeField] private List<GameObject> droidObjects = new List<GameObject>();
    public List<Vector3> getSpawnLocations()
    {
        List<Vector3> spawnLocations = new List<Vector3>();
        for (int i = 0; i < droidObjects.Count; i++)
        {
            spawnLocations.Add(droidObjects[i].transform.position);
        }
        return spawnLocations;
    }

    public List<GameObject> getDroidObjects()
    {
        List<GameObject> returnDroidObjects = new List<GameObject>();
        for (int i = 0; i < droidObjects.Count; i++)
        {
            returnDroidObjects.Add(droidObjects[i]);
        }
        return returnDroidObjects;
    }
}

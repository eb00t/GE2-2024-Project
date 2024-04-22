using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    public int bugsToSpawn = 5;
    public int currentBugs;
    private GameObject _bug;
    private float _bugTimer;
    private List<GameObject> _bugSpawners;
    
    void Start()
    {
        _bug = Resources.Load<GameObject>("Prefabs/Bug");
        StartCoroutine(CountBugs());
        _bugSpawners = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("BugSpawner"))
        {
           _bugSpawners.Add(go); 
        }

        Debug.Log(_bugSpawners.Count);
    }

    
   void Update()
    {
        if (currentBugs < (bugsToSpawn * _bugSpawners.Count) && _bugTimer <= 0)
        {
            _bugTimer = 1;
            Instantiate(_bug, gameObject.transform.position, Quaternion.identity);
            currentBugs++;
        }
        _bugTimer = Mathf.Clamp(_bugTimer,0, 1);
        _bugTimer -= Time.deltaTime;
        
    }

    IEnumerator CountBugs()
    {
        while (true)
        {
            int tempCurrentBugs = 0;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bug"))
            {
                tempCurrentBugs++;
            }

            currentBugs = tempCurrentBugs;
            Debug.Log("There are currently " + currentBugs + " bugs.");
            yield return new WaitForSecondsRealtime(10f);
        }
    }
}

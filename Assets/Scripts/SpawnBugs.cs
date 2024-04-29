using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    private int _bugsToSpawn;
    public int currentBugs;
    private GameObject _bug;
    private float _bugTimer;
    private List<GameObject> _bugSpawners;
    private GlobalVariables _globalVariables;
    
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        //_bugsToSpawn = _globalVariables.totalBugsAllowed;
        _bug = Resources.Load<GameObject>("Prefabs/Bug");
        _bugSpawners = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("BugSpawner"))
        {
           _bugSpawners.Add(go); 
        }

        Debug.Log(_bugSpawners.Count);
    }

    
   void Update()
    {
        if (_globalVariables.bugCount < (_globalVariables.totalBugsAllowed * _bugSpawners.Count) && _bugTimer <= 0)
        {
            _bugTimer = 1;
            Instantiate(_bug, gameObject.transform.position, Quaternion.identity);
            _globalVariables.bugCount++;
        }
        _bugTimer = Mathf.Clamp(_bugTimer,0, 1);
        _bugTimer -= Time.deltaTime;
    }
}

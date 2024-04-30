using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int _bugsToSpawn;
    private GameObject _bug, _orangeFish;
    private float _bugTimer, _fishTimer;
    private List<GameObject> _bugSpawners, _fishSpawners;
    private GlobalVariables _globalVariables;
    public enum Spawns
    {
        Bugs,
        OrangeFish,
        RedFish,
        SchoolFish
    }
    public Spawns toSpawn;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        //_bugsToSpawn = _globalVariables.totalBugsAllowed;
        _bug = Resources.Load<GameObject>("Prefabs/Bug");
        _orangeFish = Resources.Load<GameObject>("Prefabs/OrangeFish");
        _bugSpawners = new List<GameObject>();
    }


    void Update()
    {
        switch (toSpawn)
        {
            //FOR BUGS
            case Spawns.Bugs:
            if (_globalVariables.bugCount < (_globalVariables.totalBugsAllowed * _bugSpawners.Count) && _bugTimer <= 0)
            {
                _bugTimer = 1;
                Instantiate(_bug, gameObject.transform.position, Quaternion.identity);
                _globalVariables.bugCount++;
            }
            _bugTimer = Mathf.Clamp(_bugTimer, 0, 1);
            _bugTimer -= Time.deltaTime;
            break;
            
            //FOR ORANGE FISH
            case Spawns.OrangeFish: 
                if (_globalVariables.orangeFishCount < (_globalVariables.totalOrangeFishAllowed) && _fishTimer <= 0)
                {
                    _fishTimer = 2;
                    Instantiate(_orangeFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.orangeFishCount++;
                }
                _fishTimer = Mathf.Clamp(_fishTimer, 0, 2);
                _fishTimer -= Time.deltaTime;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int _bugsToSpawn;
    private GameObject _bug, _orangeFish, _redFish;
    private float _timer;
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
        _redFish = Resources.Load<GameObject>("Prefabs/Red Fish");
    }


    void Update()
    {
        switch (toSpawn)
        {
            //FOR BUGS
            case Spawns.Bugs:
            if (_globalVariables.bugCount < (_globalVariables.totalBugsAllowed) && _timer <= 0)
            {
                _timer = 1;
                Instantiate(_bug, gameObject.transform.position, Quaternion.identity);
                _globalVariables.bugCount++;
            }
            _timer = Mathf.Clamp(_timer, 0, 1);
            _timer -= Time.deltaTime;
            break;
            
            //FOR ORANGE FISH
            case Spawns.OrangeFish: 
                if (_globalVariables.orangeFishCount < (_globalVariables.totalOrangeFishAllowed) && _timer <= 0)
                {
                    _timer = 2;
                    Instantiate(_orangeFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.orangeFishCount++;
                }
                _timer = Mathf.Clamp(_timer, 0, 2);
                _timer -= Time.deltaTime;
                break; 
            //FOR RED FISH
            case Spawns.RedFish: 
                if (_globalVariables.predatorFishCount < (_globalVariables.totalPredatorFishAllowed) && _timer <= 0)
                {
                    _timer = 4;
                    Instantiate(_redFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.orangeFishCount++;
                }
                _timer = Mathf.Clamp(_timer, 0, 4);
                _timer -= Time.deltaTime;
                break;
        }
    }
}

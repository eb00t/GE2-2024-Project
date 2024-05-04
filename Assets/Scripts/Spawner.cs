using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int _bugsToSpawn;
    private GameObject _bug, _orangeFish, _redFish, _schoolFish, _schoolFishLeaders;
    private float _timer, _timerMax;
    private List<GameObject> _bugSpawners, _fishSpawners;
    private GlobalVariables _globalVariables;
    public enum Spawns
    {
        Bugs,
        OrangeFish,
        RedFish,
        SchoolFish,
        SchoolFishLeader
    }
    public Spawns toSpawn;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        //_bugsToSpawn = _globalVariables.totalBugsAllowed;
        _bug = Resources.Load<GameObject>("Prefabs/Bug");
        _orangeFish = Resources.Load<GameObject>("Prefabs/OrangeFish");
        _redFish = Resources.Load<GameObject>("Prefabs/Red Fish");
        _schoolFish = Resources.Load<GameObject>("Prefabs/SchoolingFish");
        _schoolFishLeaders = Resources.Load<GameObject>("Prefabs/SchoolingFishLeader");
    }


    void Update()
    {
        switch (toSpawn)
        {
            //FOR BUGS
            case Spawns.Bugs:
                _timerMax = 1;
                if (_globalVariables.bugCount < (_globalVariables.totalBugsAllowed) && _timer <= 0)
                {
                    _timer = _timerMax;
                    Instantiate(_bug, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.bugCount++;
                }
                break;
            
            //FOR ORANGE FISH
            case Spawns.OrangeFish:
                _timerMax = 2;
                if (_globalVariables.orangeFishCount < (_globalVariables.totalOrangeFishAllowed) && _timer <= 0)
                {
                    _timer = _timerMax;
                    Instantiate(_orangeFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.orangeFishCount++;
                }
                break; 
            
            //FOR RED FISH
            case Spawns.RedFish:
                _timerMax = 4;
                if (_globalVariables.predatorFishCount < (_globalVariables.totalPredatorFishAllowed) && _timer <= 0)
                {
                    _timer = _timerMax;
                    Instantiate(_redFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.orangeFishCount++;
                }
                break;
            
            //FOR SCHOOL FISH
            case Spawns.SchoolFish:
                _timerMax = 1.5f;
                if (_globalVariables.schoolFishCount < _globalVariables.totalSchoolFishAllowed && _timer <= 0)
                {
                    _timer = _timerMax;
                    Instantiate(_schoolFish, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.schoolFishCount++;
                }
                break;
            
            
            //FOR SCHOOL FISH LEADERS
            case Spawns.SchoolFishLeader:
                _timerMax = 0.5f;
                if (_globalVariables.schoolFishLeaderCount < _globalVariables.totalSchoolFishLeadersAllowed && _timer <= 0)
                {
                    _timer = _timerMax;
                    Instantiate(_schoolFishLeaders, gameObject.transform.position, Quaternion.identity);
                    _globalVariables.schoolFishLeaderCount++;
                }
                break;
        }
        _timer = Mathf.Clamp(_timer, 0, _timerMax);
        _timer -= Time.deltaTime;
    }
}

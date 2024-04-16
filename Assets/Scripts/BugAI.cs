using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BugAI : MonoBehaviour
{
    private Boid _boid;
    private Seek _seek;
    private ObstacleAvoidance _obstacleAvoidance;
    private NoiseWander _noiseWander;
    public float changeStateTimer;
    private int _behaviourNumber = 2;
    private List<GameObject> _lights;
    private List<GameObject> _glass;
    private int _randomLight, _randomGlass;
    public enum WhatAmIDoing
    {
        Wandering,
        SeekLight,
        Landed,
        CrashingIntoWindow
    }
    public WhatAmIDoing doingWhat;
    
    void Start()
    {
        changeStateTimer = Random.Range(15, 76);
        _boid = GetComponent<Boid>();
        _seek = GetComponent<Seek>();
        _noiseWander = GetComponent<NoiseWander>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _lights = new List<GameObject>();
        _glass = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Light"))
        {
            _lights.Add(go);
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Glass"))
        {
            _glass.Add(go);
        }
        _randomLight = Random.Range(0, _lights.Count);
        _randomGlass = Random.Range(0, _glass.Count);
        _behaviourNumber = Random.Range(0, 4);
        StartCoroutine(Die());
    }
    
    void Update()
    {
        changeStateTimer -= Time.deltaTime;
        if (changeStateTimer <= 0)
        {
            _randomLight = Random.Range(0, _lights.Count);
            _randomGlass = Random.Range(0, _glass.Count);
            changeStateTimer = Random.Range(15, 76);
            _behaviourNumber = Random.Range(0, 4);
        }

        switch (_behaviourNumber)
        {
            case 0: //Wandering
                doingWhat = WhatAmIDoing.Wandering;
                break;
            case 1: //Landed
                doingWhat = WhatAmIDoing.Landed;
                break;
            case 2: //Seek Light
                doingWhat = WhatAmIDoing.SeekLight;
                break;
            case 3: //Crash into Window
                doingWhat = WhatAmIDoing.CrashingIntoWindow;
                break;
        }
        
        switch (doingWhat)
        {
            case WhatAmIDoing.SeekLight:
                _obstacleAvoidance.forwardFeelerDepth = 3;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 2;
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _lights[_randomLight];
                break;
            case WhatAmIDoing.Wandering:
                _obstacleAvoidance.forwardFeelerDepth = 3;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 2;
                _seek.enabled = false;
                _noiseWander.enabled = true;
                break;
            case WhatAmIDoing.Landed:
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _glass[_randomGlass];
                break;
            case WhatAmIDoing.CrashingIntoWindow:
                _obstacleAvoidance.forwardFeelerDepth = 1;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 10;
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _glass[_randomGlass];
                break;
        }
    }

    IEnumerator Die()
    {
       yield return new WaitForSecondsRealtime(Random.Range(30, 121));
       _boid.enabled = false;
       _seek.enabled = false;
       _obstacleAvoidance.enabled = false;
       _noiseWander.enabled = false;
       gameObject.AddComponent<Rigidbody>();
       yield return new WaitForSecondsRealtime(Random.Range(1, 4));
       Debug.Log("Bug down!!!!!1");
       Destroy(gameObject);
    }
}

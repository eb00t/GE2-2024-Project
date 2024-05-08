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
    private Pursue _pursue;
    public float changeStateTimer;
    private int _behaviourNumber = 2;
    private Animator _animator;
    private List<GameObject> _lights;
    private List<GameObject> _glass;
    private int _randomLight, _randomGlass;
    private Light _bugLight;
    //private bool _landed;
    private WingsAnim _wingsAnim;
    private Vector3 _trueFlatPos;
    public bool canDieFromLights = true;
    private int _willSuicide;
    private GameObject _player;
    private Boid _playerBoid;
    private bool _dying;
    public enum WhatAmIDoing
    {
        Wandering,
        SeekLight,
        //Landed,
        FollowPlayer,
        CrashingIntoWindow
    }
    public WhatAmIDoing doingWhat;
    private static readonly int Shrink = Animator.StringToHash("Shrink");

    void Start()
    {
        _player = GameObject.Find("Player");
        _playerBoid = _player.GetComponentInChildren<Boid>();
        _pursue = GetComponent<Pursue>();
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
        _behaviourNumber = Random.Range(0, 4);
        _bugLight = transform.GetChild(1).GetComponent<Light>();
        //_landed = false;
        _wingsAnim = GetComponentInChildren<WingsAnim>();
        _wingsAnim.enabled = true;
        StartCoroutine(DieTimer());
        GetComponent<AudioSource>();
        switch (_behaviourNumber) //only randomises what it needs to
        {
            case 2: //Seek Light
                _randomLight = Random.Range(0, _lights.Count);
                _willSuicide = Random.Range(0, 11);
                break;
            case 3: //Crash into Window
                _randomGlass = Random.Range(0, _glass.Count);
                break;
        }

        _animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        _bugLight.intensity = Mathf.Lerp(0, 15,0.05f * Time.time);
        changeStateTimer -= Time.deltaTime;
        if (changeStateTimer <= 0)
        {
            _behaviourNumber = Random.Range(0, 4);
            switch (_behaviourNumber) //only randomises what it needs to
            {
                case 2: //Seek Light
                    _randomLight = Random.Range(0, _lights.Count);
                    _willSuicide = Random.Range(0, 11);
                    break;
                case 3: //Crash into Window
                    _randomGlass = Random.Range(0, _glass.Count);
                    break;
            }
            changeStateTimer = Random.Range(15, 76);
           // _landed = false;
        }

        switch (_behaviourNumber)
        {
            case 0: //Wandering
                doingWhat = WhatAmIDoing.Wandering;
                break;
            case 1: //Landed
                doingWhat = WhatAmIDoing.FollowPlayer;
                break;
            case 2: //Seek Light
                doingWhat = WhatAmIDoing.SeekLight;
                break;
            case 3: //Crash into Window
                doingWhat = WhatAmIDoing.CrashingIntoWindow;
                break;
        }

        Vector3 position = transform.position;
        switch (doingWhat)
        {
            case WhatAmIDoing.SeekLight:
                _pursue.enabled = false;
                _boid.enabled = true;
                _wingsAnim.enabled = true;
                switch (_willSuicide)
                {
                   case 1: //false
                       _obstacleAvoidance.enabled = false;
                       break;
                   case not 1: //true
                       _obstacleAvoidance.enabled = true;
                       _obstacleAvoidance.forwardFeelerDepth = 3;
                       _obstacleAvoidance.sideFeelerDepth = 1;
                       _obstacleAvoidance.scale = 2;
                       break;
                }
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _lights[_randomLight];
                if (Vector3.Distance(position, _seek.targetGameObject.transform.position) <= 0.1f && canDieFromLights)
                {
                    _dying = false;
                    if (!_dying)
                    {
                        _dying = true;
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Zap, gameObject.transform.position);
                        ActuallyDieForReal();
                      
                    }
                }
                break;
            case WhatAmIDoing.Wandering:
                _pursue.enabled = false;
                _wingsAnim.enabled = true;
                _boid.enabled = true;
                _obstacleAvoidance.enabled = true;
                _obstacleAvoidance.forwardFeelerDepth = 3;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 2;
                _seek.enabled = false;
                _noiseWander.enabled = true;
                break;
            case WhatAmIDoing.FollowPlayer:
                _pursue.enabled = true;
                _wingsAnim.enabled = true;
                _boid.enabled = true;
                _obstacleAvoidance.enabled = true;
                _obstacleAvoidance.forwardFeelerDepth = 3;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 1;
                _seek.enabled = false;
                _pursue.target = _playerBoid;
                break;
            /*case WhatAmIDoing.Landed: //This was buggy as hell and also sucked :<
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _flats[_randomFlat];
                Vector3 transformPosition = _seek.targetGameObject.transform.position;
               /* _seek.target = new Vector3(
                    Random.Range(transformPosition.x - _seek.transform.localScale.x / 2,
                        transformPosition.x + _seek.transform.localScale.x),
                    (transformPosition.y + _seek.targetGameObject.transform.localScale.y / 2 +
                     gameObject.transform.localScale.y / 2),
                    Random.Range(transformPosition.z - _seek.transform.localScale.z / 2,
                        transformPosition.z + _seek.transform.localScale.z / 2));*/
                /*_seek.target = new Vector3(transformPosition.x,
                    transformPosition.y + _seek.targetGameObject.transform.position.y + transform.localScale.y / 2 + 0.5f, transformPosition.z);
                
                if (Vector3.Distance(position,_seek.target) <= 3)
                {
                    _obstacleAvoidance.enabled = false;
                }
                else
                {
                    _obstacleAvoidance.enabled = true;
                }
                
                if (Vector3.Distance(position,_seek.target) <= .1 && position.y >= _seek.target.y)
                {
                    _landed = true;
                    var transformRotation = transform.rotation;
                    transformRotation.x = 0;
                    transformRotation.z = 0;
                    transform.rotation = transformRotation;
                    position.y = transformPosition.y / 2 - transform.localScale.y;
                }
                else
                {
                    _landed = false;
                }
                
                switch (_landed)
                {
                    case true:
                        _boid.enabled = false;
                        _wingsAnim.enabled = false;
                        break;
                    case false:
                        _boid.enabled = true;
                        _wingsAnim.enabled = true;
                        break;
                }
                
                break;*/
            case WhatAmIDoing.CrashingIntoWindow:
                _pursue.enabled = false;
                _wingsAnim.enabled = true;
                _boid.enabled = true;
                _obstacleAvoidance.enabled = true;
                _obstacleAvoidance.forwardFeelerDepth = 1;
                _obstacleAvoidance.sideFeelerDepth = 1;
                _obstacleAvoidance.scale = 10;
                _noiseWander.enabled = false;
                _seek.enabled = true;
                _seek.targetGameObject = _glass[_randomGlass];
                break;
        }
    }

    IEnumerator DieTimer()
    {
       yield return new WaitForSecondsRealtime(Random.Range(30, 121));
       ActuallyDieForReal();
    }

    public void ActuallyDieForReal()
    {
        _boid.enabled = false;
        _seek.enabled = false;
        _obstacleAvoidance.enabled = false;
        _noiseWander.enabled = false;
        _wingsAnim.enabled = false;
        gameObject.AddComponent<BoxCollider>();
        gameObject.AddComponent<Rigidbody>();
        _bugLight.intensity = Mathf.Lerp(15, 0,0.05f * Time.time);
        StartCoroutine(DiePart2());
    }

    private IEnumerator DiePart2()
    {  
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        //Debug.Log("Bug down!!!!!1");
        _animator.SetBool(Shrink, true);
    }

    public void DestroyMeCompletely()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderAI : MonoBehaviour
{
    private Boid _boid;
    private NoiseWander _noiseWander;
    private Flee _flee;
    private ObstacleAvoidance _obstacleAvoidance;
    private SpineAnimator _spineAnimator;
    private Pursue _pursue;
    private Harmonic _harmonic;
    public List<GameObject> allSegments;
    private GameObject _fishRoot;
    private Animator _animator;//THIS IS JUST FOR SHRINKING, I AM SO TIRED OF THE EDITOR SOFT CRASHING
    private GlobalVariables _globalVariables;
    private CameraManager _cameraManager;
    public bool isFearless;
    private bool _fleeing;
    public List<GameObject> evilFish;
    public float changeStateTimer;
    private int _behaviourNumber;

    public enum AIStates
    {
        Wandering,
        FollowRandomOrangeFish,
        Fleeing
    }

    public AIStates states;
    private static readonly int Shrink = Animator.StringToHash("Shrink");

    void Start()
    {
        changeStateTimer = Random.Range(15, 76);
        _animator = GetComponent<Animator>();
        _boid = GetComponent<Boid>();
        _noiseWander = GetComponent<NoiseWander>();
        _pursue = GetComponent<Pursue>();
        _flee = GetComponent<Flee>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _spineAnimator = GetComponent<SpineAnimator>();
        _harmonic = GetComponent<Harmonic>();
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();

        evilFish = new List<GameObject>();
        _flee.enabled = false;
        states = AIStates.Wandering;
        StartCoroutine(CheckForBadGuys());
        allSegments = new List<GameObject>();
        _fishRoot = transform.root.gameObject;
        foreach (Transform tf in _fishRoot.GetComponentsInChildren<Transform>())
        {
            allSegments.Add(tf.gameObject);
        }

        _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
        GetNewTarget();
    }

    void Update()
    {
        changeStateTimer -= Time.deltaTime;
        if (changeStateTimer <= 0 && !_fleeing)
        {
            _behaviourNumber = Random.Range(0, 1);
            switch (_behaviourNumber)
            {
                case 0:
                    states = AIStates.Wandering;
                    break;
                case 1:
                    GetNewTarget();
                    states = AIStates.FollowRandomOrangeFish;
                    break;
            }
        }
        
        switch (states)
        {
            case AIStates.Wandering:
                _noiseWander.enabled = true;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 5;
                _spineAnimator.angularBondDamping = 3;
                _flee.enabled = false;
                break;
            case AIStates.Fleeing:
                _noiseWander.enabled = false;
                _boid.maxSpeed = 8;
                _spineAnimator.bondDamping = 7.5f;
                _spineAnimator.angularBondDamping = 3.9f;
                _flee.enabled = true;
                break;
            case AIStates.FollowRandomOrangeFish:
                if (_pursue.target == null)
                {
                    states = AIStates.Wandering;
                }
                _noiseWander.enabled = false;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 5;
                _spineAnimator.angularBondDamping = 3;
                _flee.enabled = false;
                break;
        }

    }

    private void GetNewTarget()
    {
        _pursue.target = _globalVariables
            .allOrangeFish[Random.Range(0, _globalVariables.allOrangeFish.Count)].GetComponent<Boid>();
    }

    IEnumerator RunAway(GameObject target)
    {
        states = AIStates.Fleeing;
        _fleeing = true;
        _flee.targetGameObject = target;
        yield return new WaitForSecondsRealtime(10f);
        _fleeing = false;
        states = AIStates.Wandering;
    }

    IEnumerator CheckForBadGuys()
    {
        while (true)
        {
            switch (isFearless)
            {
                case false:
                {
                    evilFish.Clear();
                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("PredatorFish"))
                    {
                        evilFish.Add(go);
                    }

                    foreach (GameObject go in evilFish)
                    {
                        Debug.Log("Checking for bad guys.");
                        if (Vector3.Distance(go.transform.position, gameObject.transform.position) < 35f)
                        {
                            StartCoroutine(RunAway(go));
                        }
                    }
                    yield return new WaitForSecondsRealtime(1f);
                    break;
                }
            }
        }
    }

    public void DieStart()
    {
        _boid.enabled = false;
        _flee.enabled = false;
        _obstacleAvoidance.enabled = false;
        _harmonic.enabled = false;
        _noiseWander.enabled = false;
        _spineAnimator.enabled = false;
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        foreach (GameObject go in allSegments)
        {
            go.AddComponent<Rigidbody>().useGravity = false;
            go.transform.SetParent(gameObject.transform);
        }
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        Debug.Log("A leader has died. State funeral processions are underway.");
        _animator.SetBool(Shrink, true);
    }

    public void DestroyMeCompletely()
    {
        gameObject.tag = "Untagged";
        _globalVariables.allSchoolingFishLeaders.Remove(gameObject);
        _globalVariables.allPreyFish.Remove(gameObject);
        if (_cameraManager.fishCam.Follow == gameObject.transform && _cameraManager.fishCam.Priority == 12)
        {
            _cameraManager.PreyCamActivate();
        }
        Destroy(gameObject.transform.root.gameObject);
    }
}

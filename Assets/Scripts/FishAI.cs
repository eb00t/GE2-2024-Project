using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private Boid _boid;
    private NoiseWander _noiseWander;
    private Flee _flee;
    private ObstacleAvoidance _obstacleAvoidance;
    private SpineAnimator _spineAnimator;
    private Harmonic _harmonic;
    public List<GameObject> allSegments;
    private GameObject _fishRoot;
    private Animator _animator;//THIS IS JUST FOR SHRINKING, I AM SO TIRED OF THE EDITOR SOFT CRASHING
    private GlobalVariables _globalVariables;
    private CameraManager _cameraManager;
    public bool isFearless;

    public List<GameObject> evilFish;

    public enum AIStates
    {
        Wandering,
        Fleeing
    }

    public AIStates states;
    private static readonly int Shrink = Animator.StringToHash("Shrink");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _boid = GetComponent<Boid>();
        _noiseWander = GetComponent<NoiseWander>();
        _flee = GetComponent<Flee>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _spineAnimator = GetComponent<SpineAnimator>();
        _harmonic = GetComponent<Harmonic>();
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();

        evilFish = new List<GameObject>();
        _flee.enabled = false;
        states = AIStates.Wandering;
        TurnEnemyCheckerOn();
        allSegments = new List<GameObject>();
        _fishRoot = transform.root.gameObject;
        foreach (Transform tf in _fishRoot.GetComponentsInChildren<Transform>())
        {
            allSegments.Add(tf.gameObject);
        }
    }

   
    void Update()
    {
        switch (states)
        {
            case AIStates.Wandering:
                _noiseWander.enabled = true;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 3;
                _spineAnimator.angularBondDamping = 2;
                _flee.enabled = false;
                break;
            case AIStates.Fleeing:
                _noiseWander.enabled = false;
                _boid.maxSpeed = 8;
                _spineAnimator.bondDamping = 5;
                _spineAnimator.angularBondDamping = 2.6f;
                _flee.enabled = true;
                break;
        }

    }

    IEnumerator RunAway(GameObject target)
    {
        if (!isFearless)
        {
            states = AIStates.Fleeing;
            _flee.targetGameObject = target;
            yield return new WaitForSecondsRealtime(10f);
            states = AIStates.Wandering;
        }

        yield return null;
    }

    IEnumerator CheckForBadGuys()
    {
        while (true)
        {
            if (!isFearless)
            {
                evilFish.Clear();
                foreach (GameObject go in _globalVariables.allPredatorFish)
                {
                    evilFish.Add(go);
                }
                
                foreach (GameObject go in evilFish)
                { 
                    //Debug.Log("Checking for bad guys.");
                    if (Vector3.Distance(go.transform.position, gameObject.transform.position) < 50f)
                    {
                        StartCoroutine(RunAway(go));
                    }
                }
            }
            yield return new WaitForSecondsRealtime(1f);
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
        //Debug.Log("Orange fish has died.");
        _animator.SetBool(Shrink, true);
     
    }

    public void DestroyMeCompletely()
    {
        gameObject.tag = "Untagged";
        _globalVariables.allOrangeFish.Remove(gameObject);
        _globalVariables.allPreyFish.Remove(gameObject);
        if (_cameraManager.fishCam.Follow == gameObject.transform && _cameraManager.fishCam.Priority == 12)
        {
            _cameraManager.PreyCamActivate();
        }
        Destroy(gameObject.transform.root.gameObject);
    }
    
    public void TurnEnemyCheckerOn()
    {
        StartCoroutine(CheckForBadGuys());
    }

}

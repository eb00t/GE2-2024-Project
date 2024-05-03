using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SchoolingFishAI : MonoBehaviour
{
    private Boid _boid;
    private OffsetPursue _offsetPursue;
    private NoiseWander _noiseWander;
    private Flee _flee;
    private ObstacleAvoidance _obstacleAvoidance;
    private SpineAnimator _spineAnimator;
    private Harmonic _harmonic;
    public List<GameObject> allSegments;
    private GameObject _fishRoot;
    private Animator _animator; //THIS IS JUST FOR SHRINKING, I AM SO TIRED OF THE EDITOR SOFT CRASHING
    private GlobalVariables _globalVariables;
    public List<GameObject> evilFish;

    public enum AIStates
    {
        Following,
        Wandering,
        Fleeing
    }

    public AIStates states;
    private static readonly int Shrink = Animator.StringToHash("Shrink");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _boid = GetComponent<Boid>();
        _offsetPursue = GetComponent<OffsetPursue>();
        _flee = GetComponent<Flee>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _spineAnimator = GetComponent<SpineAnimator>();
        _harmonic = GetComponent<Harmonic>();
        _noiseWander = GetComponent<NoiseWander>();
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        evilFish = new List<GameObject>();
        _flee.enabled = false;
        states = AIStates.Following;
        _offsetPursue.leader = _globalVariables.allSchoolingFishLeaders[Rng()].GetComponent<Boid>();
        StartCoroutine(CheckForBadGuys());
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
            case AIStates.Following:
                if (_offsetPursue.leader == null)
                {
                    states = AIStates.Wandering;
                }
                _offsetPursue.enabled = true;
                _noiseWander.enabled = false;
                _flee.enabled = false;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 5;
                _spineAnimator.angularBondDamping = 3;
                break;
            case AIStates.Wandering:
                _offsetPursue.enabled = false;
                _noiseWander.enabled = true;
                _flee.enabled = false;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 5;
                _spineAnimator.angularBondDamping = 3;
                break;
            case AIStates.Fleeing:
                _offsetPursue.enabled = false;
                _noiseWander.enabled = false;
                _flee.enabled = true;
                _boid.maxSpeed = 8;
                _spineAnimator.bondDamping = 7.5f;
                _spineAnimator.angularBondDamping = 3.9f;
                break;
        }
    }

    private int Rng()
    {
        int randomNum = Random.Range(0, _globalVariables.schoolFishLeaderCount);
        return randomNum;
    }

IEnumerator RunAway(GameObject target)
    {
        states = AIStates.Fleeing;
        _flee.targetGameObject = target;
        yield return new WaitForSecondsRealtime(10f);
        states = AIStates.Following;
        _offsetPursue.leader = _globalVariables.allSchoolingFishLeaders[Rng()].GetComponent<Boid>();
    }

    IEnumerator CheckForBadGuys()
    {
        while (true)
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
        Debug.Log("A schooling fish has died.");
        _animator.SetBool(Shrink, true);
     
    }

    public void DestroyMeCompletely()
    {
        gameObject.tag = null;
        _globalVariables.allOrangeFish.Remove(gameObject);
        _globalVariables.allPreyFish.Remove(gameObject);
        Destroy(gameObject.transform.root.gameObject);
    }
}

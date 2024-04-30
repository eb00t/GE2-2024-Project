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
    
    public List<GameObject> evilFish;
    public enum AIStates
    {
        Wandering,
        Fleeing
    }

    public AIStates states;

    void Start()
    {
        _boid = GetComponent<Boid>();
        _noiseWander = GetComponent<NoiseWander>();
        _flee = GetComponent<Flee>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _spineAnimator = GetComponent<SpineAnimator>();
        _harmonic = GetComponent<Harmonic>();
        
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
        states = AIStates.Fleeing;
        _flee.targetGameObject = target;
        yield return new WaitForSecondsRealtime(10f);
        states = AIStates.Wandering;
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
                if (Vector3.Distance(go.transform.position, gameObject.transform.position) < 50f)
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
        }
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        Debug.Log("A fish has been eaten!");
        Destroy(gameObject.transform.root.gameObject);
    }
}
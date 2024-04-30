using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private NoiseWander _noiseWander;
    public List<GameObject> evilFish;
    private Flee _flee;
    private Boid _boid;
    private SpineAnimator _spineAnimator;
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
        _spineAnimator = GetComponent<SpineAnimator>();
        evilFish = new List<GameObject>();
        _flee.enabled = false;
        states = AIStates.Wandering;
        StartCoroutine(CheckForBadGuys());
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
}
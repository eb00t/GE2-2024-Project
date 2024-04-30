using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private NoiseWander _noiseWander;
    public List<GameObject> evilFish;
    private Flee _flee;
    public enum AIStates
    {
        Wandering,
        Fleeing
    }

    public AIStates states;

    void Start()
    {
        _noiseWander = GetComponent<NoiseWander>();
        _flee = GetComponent<Flee>();
        evilFish = new List<GameObject>();
        _flee.enabled = false;
        states = AIStates.Wandering;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PredatorFish"))
        {
            evilFish.Add(go);
        }
        StartCoroutine(CheckForBadGuys());
    }

    void Update()
    {
        switch (states)
        {
            case AIStates.Wandering:
                _noiseWander.enabled = true;
                _flee.enabled = false;
                break;
            case AIStates.Fleeing:
                _noiseWander.enabled = false;
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
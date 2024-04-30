using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalVariables : MonoBehaviour
{
    [Header("Bug Info")] public int bugCount = 0;
    public int totalBugsAllowed;
    public bool canBugsDieFromLights;
    public List<GameObject> allBugs;

    [Header("Prey Fish Info")] 
    public int orangeFishCount;
    public int preyFishCount;
    public int totalOrangeFishAllowed;
    public int totalSchoolFishAllowed;
    public int totalPreyAllowed;
    public List<GameObject> allPreyFish;
    
    [Header("Predator Fish Info")]
    public int predatorFishCount;
    public int totalPredatorFishAllowed;
    public List<GameObject> allPredatorFish;

    private int _totalFishAllowed;
    public float counterUpdateTime = 1f;

    void Start()
    {
        StartCoroutine(CountBugs());
        StartCoroutine(CountPreyFish());
        StartCoroutine(CountPredatorFish());
        allBugs = new List<GameObject>();
        allPreyFish = new List<GameObject>();
        allPredatorFish = new List<GameObject>();
        canBugsDieFromLights = true;
    }

    void Update()
    {
        totalPreyAllowed = totalOrangeFishAllowed + totalSchoolFishAllowed;
        _totalFishAllowed = totalPreyAllowed + totalPredatorFishAllowed;
    }

    IEnumerator CountBugs()
    {
        while (true)
        {
            int tempCurrentBugs = 0;
            allBugs.Clear();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bug"))
            {
                tempCurrentBugs++;
                allBugs.Add(go);
            }
            bugCount = tempCurrentBugs;
            Debug.Log("There are currently " + bugCount + " bugs.");
            yield return new WaitForSecondsRealtime(counterUpdateTime);
        }
    }
    
    IEnumerator CountPreyFish()
    {
        while (true)
        {
            int tempCurrentFish = 0;
            allPreyFish.Clear();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PreyFish"))
            {
                tempCurrentFish++;
                allPreyFish.Add(go);
            }

            preyFishCount = tempCurrentFish;
            Debug.Log("There are currently " + preyFishCount + " edible fish.");
            yield return new WaitForSecondsRealtime(counterUpdateTime);
        }
    }
    
    IEnumerator CountPredatorFish()
    {
        while (true)
        {
            int tempCurrentFish = 0;
            allPredatorFish.Clear();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PredatorFish"))
            {
                tempCurrentFish++;
                allPredatorFish.Add(go);
            }

            predatorFishCount = tempCurrentFish;
            Debug.Log("There are currently " + predatorFishCount + " inedible fish.");
            yield return new WaitForSecondsRealtime(counterUpdateTime);
        }
    }

    public void ToggleLightDeath()
    {
        canBugsDieFromLights = !canBugsDieFromLights;
        Debug.Log(canBugsDieFromLights);
        GetImmediateBugNumber();
        foreach (GameObject go in allBugs)
        {
            go.GetComponent<BugAI>().canDieFromLights = canBugsDieFromLights;
        }
    }

    public void KillAllBugs()
    {
        Debug.Log("Killed all bugs.");
        foreach (GameObject go in allBugs)
        {
            go.GetComponent<BugAI>().ActuallyDieForReal();
        }
        GetImmediateBugNumber();
    }

    public void KillAllPreyFish()
    {
        Debug.Log("Killed all prey fish. The predators are going to be hungry.");
        foreach (GameObject go in allPreyFish)
        {
            go.GetComponent<FishAI>().DieStart();
        }
        GetImmediatePreyFishNumber();
    }
    
    private void GetImmediateBugNumber()
    {
        StopCoroutine(CountBugs());
        StartCoroutine(CountBugs());
    }

    private void GetImmediatePreyFishNumber()
    {
        StopCoroutine(CountPreyFish());
        StartCoroutine(CountPreyFish());
    }
    
    private void GetImmediatePredatorFishNumber()
    {
        StopCoroutine(CountPredatorFish());
        StartCoroutine(CountPredatorFish());
    }

    
}


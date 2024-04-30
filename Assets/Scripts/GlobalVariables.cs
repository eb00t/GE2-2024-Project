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

    [Header("Fish Info")] 
    public int orangeFishCount;
    public int preyFishCount;
    public int totalOrangeFishAllowed;
    public int totalSchoolFishAllowed;
    public int totalPreyAllowed;
    public List<GameObject> allPreyFish;

    void Start()
    {
        StartCoroutine(CountBugs());
        StartCoroutine(CountPreyFish());
        allBugs = new List<GameObject>();
        allPreyFish = new List<GameObject>();
        canBugsDieFromLights = true;
    }

    void Update()
    {
        totalPreyAllowed = totalOrangeFishAllowed + totalSchoolFishAllowed;
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
            yield return new WaitForSecondsRealtime(1f);
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

    private void GetImmediateBugNumber()
    {
        StopCoroutine(CountBugs());
        StartCoroutine(CountBugs());
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
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private void GetImmediatePreyFishNumber()
    {
        StopCoroutine(CountPreyFish());
        StartCoroutine(CountPreyFish());
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
}


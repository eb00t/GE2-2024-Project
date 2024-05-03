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
    public int schoolFishCount;
    public int schoolFishLeaderCount;
    public int preyFishCount;
    public int totalOrangeFishAllowed;
    public int totalSchoolFishAllowed;
    public int totalSchoolFishLeadersAllowed;
    public int totalPreyAllowed;
    public List<GameObject> allPreyFish;
    public List<GameObject> allOrangeFish;
    public List<GameObject> allSchoolingFish;
    public List<GameObject> allSchoolingFishLeaders;
    
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
            allBugs.Clear();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bug"))
            {
                allBugs.Add(go);
            }
            bugCount = allBugs.Count;
            Debug.Log("There are currently " + bugCount + " bugs.");
            yield return new WaitForSecondsRealtime(counterUpdateTime);
        }
    }
    
    IEnumerator CountPreyFish()
    {
        while (true)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("OrangeFish"))
            {
                if (!allOrangeFish.Contains(go))
                {
                    allOrangeFish.Add(go);
                }
            }

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("SchoolFish"))
            {
                if (!allSchoolingFish.Contains(go))
                {
                    allSchoolingFish.Add(go);
                }
            }
            
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("SchoolFishLeader"))
            {
                if (!allSchoolingFishLeaders.Contains(go))
                {
                    allSchoolingFishLeaders.Add(go);
                }
            }
            
            foreach (GameObject go in allOrangeFish)
            {
                if (!allPreyFish.Contains(go))
                {
                    allPreyFish.Add(go);
                }
            }
            foreach (GameObject go in allSchoolingFish)
            {
                if (!allPreyFish.Contains(go))
                {
                    allPreyFish.Add(go);
                }
            }
            orangeFishCount = allOrangeFish.Count;
            schoolFishCount = allSchoolingFish.Count;
            schoolFishLeaderCount = allSchoolingFishLeaders.Count;
            preyFishCount = allPreyFish.Count;
            Debug.Log("There are currently " + preyFishCount + " edible fish, " + orangeFishCount + " orange fish and " + (schoolFishCount+ schoolFishLeaderCount) + " schooling fish.");
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
    }

    public void KillAllPreyFish()
    {
        Debug.Log("Killed all prey fish. The predators are going to be hungry.");
        foreach (GameObject go in allPreyFish)
        {
            go.GetComponent<FishAI>().DieStart();
        }
    }
    

    
}


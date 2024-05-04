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
    public bool fishAreFearful = true;
    public List<GameObject> allPreyFish;
    public List<GameObject> allOrangeFish;
    public List<GameObject> allSchoolingFish;
    public List<GameObject> allSchoolingFishLeaders;
    
    [Header("Predator Fish Info")]
    public int predatorFishCount;
    public int totalPredatorFishAllowed;
    public bool canRedFishEat = true;
    public bool canRedFishStarve = true;
    public bool redFishOmnicidal = false;
    public List<GameObject> allPredatorFish;

    private int _totalFishAllowed;

    public List<GameObject> allFish;
    public float counterUpdateTime = 1f;

    void Start()
    {
        StartCoroutine(CountBugs());
        StartCoroutine(CountPreyFish());
        StartCoroutine(CountPredatorFish());
        StartCoroutine(MaintainVariables());
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
            foreach (GameObject go in allSchoolingFishLeaders)
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

    IEnumerator CountAllFish()
    {
        foreach (GameObject go in allPreyFish)
        {
            if (!allFish.Contains(go))
            {
                allFish.Add(go);
            }
        }

        foreach (GameObject go in allPredatorFish)
        {
            if (!allFish.Contains(go))
            {
                allFish.Add(go);
            }
        }

        yield return new WaitForSecondsRealtime(counterUpdateTime);
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

    public void FearlessFish() //This doesn't work.
    {
        fishAreFearful = !fishAreFearful;
        Debug.Log("Fish Fear = " + fishAreFearful);
        foreach (GameObject go in allPreyFish)
        {
            if (go.GetComponent<FishAI>() != null)
            {
                go.GetComponent<FishAI>().isFearless = fishAreFearful;
                go.GetComponent<FishAI>().TurnEnemyCheckerOn();
            }
            else if (go.GetComponent<SchoolingFishAI>() != null)
            {
                go.GetComponent<SchoolingFishAI>().isFearless = fishAreFearful;
                go.GetComponent<SchoolingFishAI>().TurnEnemyCheckerOn();
            }
            else if (go.GetComponent<LeaderAI>() != null)
            {
                go.GetComponent<LeaderAI>().isFearless = fishAreFearful;
                go.GetComponent<LeaderAI>().TurnEnemyCheckerOn();
            }
        }
    }

    public void CanFishEat()
    {
        canRedFishEat = !canRedFishEat;
        Debug.Log("Red Fish Eat?" + canRedFishEat);
        foreach (GameObject go in allPredatorFish)
        {
            go.GetComponent<RedFishAI>().canEat = canRedFishEat;
        }
    }

    public void CanFishStarve()
    {
        canRedFishStarve = !canRedFishStarve;
        Debug.Log("Red Fish Starve? " + canRedFishStarve);
        foreach (GameObject go in allPredatorFish)
        {
            go.GetComponent<RedFishAI>().canStarve = canRedFishStarve;
        }
    }

    public void OmnicidalFish()
    {
        redFishOmnicidal = !redFishOmnicidal;
        Debug.Log("Red Fish Eat Everything? " + redFishOmnicidal);
        foreach (GameObject go in allPredatorFish)
        {
            go.GetComponent<RedFishAI>().isOmnicidal = redFishOmnicidal;
            go.GetComponent<RedFishAI>()._hunger = 50;
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
            if (go.GetComponent<FishAI>() != null)
            {
                go.GetComponent<FishAI>().DieStart();
            }
            else if (go.GetComponent<SchoolingFishAI>() != null)
            {
                go.GetComponent<SchoolingFishAI>().DieStart();
            }
            else if (go.GetComponent<LeaderAI>() != null)
            {
                go.GetComponent<LeaderAI>().DieStart();
            }
        }
    }

    public void KillAllPredatorFish()
    {
        Debug.Log("Killed all predator fish. The prey were praying for this.");
        foreach (GameObject go in allPredatorFish)
        {
            go.GetComponent<RedFishAI>().DieStart();
        }
    }

    IEnumerator MaintainVariables()
    {
        while (true)
        {
            //Fear
            foreach (GameObject go in allPreyFish) //DOESN'T WORK; NOT FIXING; NOT WORTH IT.
            {
                if (go.GetComponent<FishAI>() != null)
                {
                    go.GetComponent<FishAI>().isFearless = fishAreFearful;
                }
                else if (go.GetComponent<SchoolingFishAI>() != null)
                {
                    go.GetComponent<SchoolingFishAI>().isFearless = fishAreFearful;
                }
                else if (go.GetComponent<LeaderAI>() != null)
                {
                    go.GetComponent<LeaderAI>().isFearless = fishAreFearful;
                }
            }
            yield return new WaitForSecondsRealtime(0.5f);
                
            //Bugs die to light
            foreach (GameObject go in allBugs)
            {
                go.GetComponent<BugAI>().canDieFromLights = canBugsDieFromLights;
            }

            yield return new WaitForSecondsRealtime(0.5f);
            
            //Fish Eating
            RedFishAI rFAI;
            foreach (GameObject go in allPredatorFish)
            {
                rFAI = go.GetComponent<RedFishAI>();
                rFAI.canEat = canRedFishEat;
                rFAI.canStarve = canRedFishStarve;
                rFAI.isOmnicidal = redFishOmnicidal;
            }
            yield return new WaitForSecondsRealtime(0.5f);
            
            
        }
    }
    
    
}



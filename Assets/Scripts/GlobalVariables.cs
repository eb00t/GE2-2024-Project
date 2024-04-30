using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalVariables : MonoBehaviour
{
    public int bugCount = 0;
    public int totalBugsAllowed;
    public List<GameObject> allBugs;
    public bool canBugsDieFromLights;
    void Start()
    {
        StartCoroutine(CountBugs());
        allBugs = new List<GameObject>();
        canBugsDieFromLights = true;
    }

    
    void Update()
    {
        
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
            yield return new WaitForSecondsRealtime(10f);
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
        GetImmediateBugNumber();
        foreach (GameObject go in allBugs)
        {
            go.GetComponent<BugAI>().ActuallyDieForReal();
        }
    }

    private void GetImmediateBugNumber()
    {
        StopCoroutine(CountBugs());
        StartCoroutine(CountBugs());
    }
}

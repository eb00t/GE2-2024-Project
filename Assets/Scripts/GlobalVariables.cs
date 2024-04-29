using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalVariables : MonoBehaviour
{
    public int bugCount = 0;
    public int totalBugsAllowed;
    void Start()
    {
        StartCoroutine(CountBugs());
    }

    
    void Update()
    {
        
    }
    
    IEnumerator CountBugs()
    {
        while (true)
        {
            int tempCurrentBugs = 0;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bug"))
            {
                tempCurrentBugs++;
            }

            bugCount = tempCurrentBugs;
            Debug.Log("There are currently " + bugCount + " bugs.");
            yield return new WaitForSecondsRealtime(10f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAnimDestroy : MonoBehaviour
{
    private BugAI _bugAI;
    void Start()
    {
        _bugAI = transform.parent.GetComponent<BugAI>();
    }

    public void DestroyMeCompletely()
    {
        _bugAI.DestroyMeCompletely();
    }
}

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }

    //AMBIENCE
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference Ambience { get; private set; }
    
    //BUG ZAPPER
    [field: Header("Bug Zap")] 
    [field: SerializeField] public EventReference Zap { get; private set; }
    
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one FMOD Events script in scene");
        }
        Instance = this;
    }
}

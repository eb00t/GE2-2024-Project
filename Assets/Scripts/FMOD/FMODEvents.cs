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
    
    [field: Header("Pause Sounds")]
    [field: SerializeField] public EventReference Pause { get; private set; }
    
    [field: Header("Sound Effects")]
    //BUG ZAPPER
    [field: Header("Bug Zap")] 
    [field: SerializeField] public EventReference Zap { get; private set; }
    
    //FOOTSTEPS
    [field: Header("Footsteps")]
    [field: SerializeField] public EventReference Footsteps { get; private set; }
    
    //THUD
    [field: Header("Metal Thud")]
    [field: SerializeField] public EventReference MetalThud { get; private set; }
    
    [field: Header("Wood Thud")]
    [field: SerializeField] public EventReference WoodThud { get; private set; }
    
    //THROW
    [field: Header("Throw")]
    [field: SerializeField] public EventReference Throw { get; private set; }
    
    [field: Header("Buttons")]
    [field: SerializeField] public EventReference Button { get; private set; }
    [field: SerializeField] public EventReference BadButton { get; private set; }
    [field: SerializeField] public EventReference GoodButton { get; private set; }
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one FMOD Events script in scene");
        }
        Instance = this;
    }
}

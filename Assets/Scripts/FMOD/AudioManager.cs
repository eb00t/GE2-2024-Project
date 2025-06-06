using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;
    private EventInstance _ambienceEventInstance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one AudioManager script in the scene.");
        }

        Instance = this;
        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
        InitialiseAmbience(FMODEvents.Instance.Ambience);
    }
    

    //GENERAL
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }
    
    //AMBIENCE
    private void InitialiseAmbience(EventReference ambienceEventReference)
    {
        _ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        _ambienceEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue) //Changes the value of a parameter of the ambience.
    {
        _ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetAmbienceArea(AmbienceArea area)
    {
        _ambienceEventInstance.setParameterByName("area", (float)area);
    }
    
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }  
        
        foreach (StudioEventEmitter studioEventEmitter in _eventEmitters)
        {
            studioEventEmitter.Stop();
        }   
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}

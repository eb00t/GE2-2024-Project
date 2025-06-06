using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class PickUp : MonoBehaviour
{
    private GlobalVariables _globalVariables;
    private SphereCollider _trigger;
    private PlayerController _playerController;
    public Vector3 spawnPos;
    public Quaternion spawnRot;
    private RandomText _randomText;
    private EventInstance _thud;
    private float _soundCd = 0;

    void Start()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _randomText = GetComponent<RandomText>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER IN RANGE");
            foreach (GameObject go in _globalVariables.allPickups)
            {
                if (go == gameObject)
                {
                    SphereCollider coll = go.GetComponent<SphereCollider>();
                    coll.enabled = true;
                    Debug.Log("Currently able to pick up: " + go);
                    _playerController.objToPickUp = go;
                }
                else
                {
                    SphereCollider coll = go.GetComponent<SphereCollider>();
                    coll.enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        _soundCd -= Time.deltaTime;
        _soundCd = Mathf.Clamp(_soundCd, 0, .25f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController.objToPickUp = null;
            foreach (GameObject go in _globalVariables.allPickups)
            {
                Collider coll = go.GetComponent<Collider>();
                coll.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || !other.gameObject.CompareTag("InHand"))
        {
            if (_soundCd == 0)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.MetalThud, transform.position);
                _soundCd = 0.25f;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFishAI : MonoBehaviour
{
    public float _hunger = 100;
    private Boid _boid;
    private Pursue _pursue;
    private ObstacleAvoidance _obstacleAvoidance;
    private NoiseWander _noiseWander;
    private Harmonic _harmonic;
    private CameraManager _cameraManager;

    private GlobalVariables _globalVariables;
    private GameObject _fishRoot;
    private SpineAnimator _spineAnimator;
    public List<GameObject> _allSegments;
    public bool canEat = true;
    public bool canStarve = true;
    public bool isOmnicidal = false;
    private Vector3 _size;
    private Animator _animator; //THIS IS JUST FOR SHRINKING, I AM SO TIRED OF THE EDITOR SOFT CRASHING

    public enum AIStates
    {
        Wandering,
        Chasing
    }

    public AIStates states;
    private static readonly int Shrink = Animator.StringToHash("Shrink");

    void Start()
    {
        _size = gameObject.transform.localScale;
        _boid = GetComponent<Boid>();
        _hunger = Random.Range(70, 110);
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _noiseWander = GetComponent<NoiseWander>();
        _harmonic = GetComponent<Harmonic>();
        _obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        _pursue = GetComponent<Pursue>();
        _spineAnimator = GetComponent<SpineAnimator>();
        StartCoroutine(Hunger());
        _allSegments = new List<GameObject>();
        _fishRoot = transform.root.gameObject;
        foreach (Transform tf in _fishRoot.GetComponentsInChildren<Transform>())
        {
            if (tf.gameObject.name.Contains("Red Fish")) //The root object should not move or do anything.
            {
                //Get outta here.
            }
            else
            {
                _allSegments.Add(tf.gameObject);
            }
        }
        _animator = GetComponent<Animator>();
        _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
    }

    void Update()
    {
        switch (states)
        {
            case AIStates.Wandering:
                _noiseWander.enabled = true;
                _pursue.enabled = false;
                _harmonic.frequency = 0.3f;
                _boid.maxSpeed = 5;
                _spineAnimator.bondDamping = 1.75f;
                _spineAnimator.angularBondDamping = 1.5f;
                break;
            case AIStates.Chasing:
                _pursue.enabled = true;
                if (_pursue.target == null)
                {
                    _pursue.target = _globalVariables.allPreyFish[Rng()].GetComponent<Boid>();
                }
                _noiseWander.enabled = false;
                _boid.maxSpeed = 10;
                _harmonic.frequency = 0.4f;
                _spineAnimator.bondDamping = 3f;
                _spineAnimator.angularBondDamping = 2.5f;
                if (Vector3.Distance(gameObject.transform.position, _pursue.target.gameObject.transform.position) < 10f && canEat)
                {
                    switch (isOmnicidal)
                    {
                        case true:
                            _hunger = 59;
                            break;
                        default:
                            _hunger = 100;
                            break;
                    }

                    if (_pursue.target.gameObject.GetComponent<FishAI>()!= null)
                    {
                        _pursue.target.gameObject.GetComponent<FishAI>().DieStart();
                    }
                    else  if (_pursue.target.gameObject.GetComponent<SchoolingFishAI>()!= null)
                    {
                        _pursue.target.gameObject.GetComponent<SchoolingFishAI>().DieStart();
                    }
                    else
                    {
                        _pursue.target.GetComponent<LeaderAI>().DieStart();
                    }
                        
                }
                break;
        }
    }

    private int Rng()
    {
        int randomNum = Random.Range(0, _globalVariables.preyFishCount);
        return randomNum;
    }
    IEnumerator Hunger()
    {
        while (true)
        {
            if (canStarve)
            {
                _hunger--;
                yield return new WaitForSecondsRealtime(1f);
                Debug.Log("I am starving " + _hunger);
            }

            switch (_hunger)
            {
                case 0:
                    StartCoroutine(Die());
                    canEat = false;
                    break;
                case < 60 and >= 0:
                    states = AIStates.Chasing;
                    break;
                case >= 60:
                    states = AIStates.Wandering;
                    break;
            }

            yield return null;
        }
    }

    public void DieStart()
    {
        _boid.enabled = false;
        _pursue.enabled = false;
        _obstacleAvoidance.enabled = false;
        _harmonic.enabled = false;
        _noiseWander.enabled = false;
        _spineAnimator.enabled = false;
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        foreach (GameObject go in _allSegments)
        {
            go.AddComponent<Rigidbody>().useGravity = false;
            go.transform.SetParent(gameObject.transform);
        }
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        Debug.Log("A red fish has died.");
      _animator.SetBool(Shrink, true);
    }

    public void DestroyMeCompletely()
    {
        gameObject.tag = "Untagged";
        _globalVariables.allPredatorFish.Remove(gameObject);
        if (_cameraManager.fishCam.Follow == gameObject.transform && _cameraManager.fishCam.Priority == 12)
        {
            _cameraManager.PredatorCamActivate();
        }
        Destroy(gameObject.transform.root.gameObject);
    }
    
}

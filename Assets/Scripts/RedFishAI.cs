using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFishAI : MonoBehaviour
{
    private float _hunger = 100;
    private Boid _boid;
    private Pursue _pursue;
    private ObstacleAvoidance _obstacleAvoidance;
    private NoiseWander _noiseWander;
    private Harmonic _harmonic;

    private GlobalVariables _globalVariables;
    private GameObject _fishRoot;
    private SpineAnimator _spineAnimator;
    public List<GameObject> _allSegments;
    public bool canEat = true;
    private int _rngNumber;
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
                break;
            case AIStates.Chasing:
                _pursue.enabled = true;
                if (_pursue.target == null)
                {
                    _pursue.target = _globalVariables.allPreyFish[_rngNumber].GetComponent<Boid>();
                }

                _noiseWander.enabled = false;
                _boid.maxSpeed = 10;
                _harmonic.frequency = 0.4f;
                if (Vector3.Distance(gameObject.transform.position, _pursue.target.gameObject.transform.position) < 12.5f)
                {
                    _hunger = 100;
                    _pursue.target.gameObject.GetComponent<FishAI>().DieStart();
                }

                break;
        }
    }

    IEnumerator Hunger()
    {
        while (true)
        {
            _hunger--;
            yield return new WaitForSecondsRealtime(0.5f);
            switch (_hunger)
            {
                case 0:
                    StartCoroutine(Die());
                    canEat = false;
                    break;
                case < 60 and >= 0:
                    states = AIStates.Chasing;
                    _rngNumber = Random.Range(0, _globalVariables.allPreyFish.Count);
                    break;
                case >= 60:
                    states = AIStates.Wandering;
                    break;
            }
        }
    }

    IEnumerator Die()
    {
        _boid.enabled = false;
        _pursue.enabled = false;
        _obstacleAvoidance.enabled = false;
        _harmonic.enabled = false;
        _noiseWander.enabled = false;
        //_spineAnimator.enabled = false;
        foreach (GameObject go in _allSegments)
        {
            go.AddComponent<Rigidbody>().useGravity = false;
            go.transform.SetParent(gameObject.transform);
        }
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        Debug.Log("Red fish has starved to death.");
      _animator.SetBool(Shrink, true);
    }

    public void DestroyMeCompletely()
    {
        gameObject.tag = null;
        _globalVariables.allOrangeFish.Remove(gameObject);
        _globalVariables.allPreyFish.Remove(gameObject);
        Destroy(gameObject.transform.root.gameObject);
    }
    
}

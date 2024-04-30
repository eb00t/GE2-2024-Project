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

    public enum AIStates
    {
        Wandering,
        Chasing
    }

    public AIStates states;

    void Start()
    {
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
            if (tf.gameObject.name.Contains("Red Fish"))
            {
                //Get outta here.
            }
            else
            {
                _allSegments.Add(tf.gameObject);
            }
        }
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
                if (Vector3.Distance(gameObject.transform.position, _pursue.target.gameObject.transform.position) < 5)
                {
                    _pursue.target.gameObject.GetComponent<FishAI>().Die();
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
            Vector3 size = go.transform.localScale;
            go.AddComponent<Rigidbody>().useGravity = false;
            StartCoroutine(ShrinkEm(size, go));
        }
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
        Debug.Log("Red fish has starved to death.");

        if (transform.localScale == new Vector3(0, 0, 0))
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }

    IEnumerator ShrinkEm(Vector3 size, GameObject go)
    {
        while (true)
        {
            go.transform.localScale = Vector3.Lerp(size, new Vector3(0,0,0), 0.005f * Time.time);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class SpineAnimator : MonoBehaviour
{
    public string[] bonePaths = new string[0];
    public float damping = 7f;
    public float angularDamping = 20f;

    private Transform[] bones;
    private Vector3[] offsets;

    void Start()
    {
        CalculateOffsets();
    }

    void CalculateOffsets()
    {
        bones = new Transform[bonePaths.Length];
        offsets = new Vector3[bonePaths.Length - 1];

        for (int i = 0; i < bonePaths.Length; i++)
        {
            Transform bone = transform.Find(bonePaths[i]);
            bones[i] = bone;

            if (i > 0)
            {
                Vector3 offset = bones[i].position - bones[i - 1].position;
                offset = Quaternion.Inverse(bones[i - 1].rotation) * offset;
                offsets[i - 1] = offset;
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < offsets.Length; i++)
        {
            Transform prev = bones[i];
            Transform next = bones[i + 1];

            Vector3 wantedPos = prev.TransformPoint(offsets[i]);

            Vector3 lerped = Vector3.Lerp(next.position, wantedPos, Time.fixedDeltaTime * damping);
            Vector3 limitLength = (lerped - prev.position).normalized * offsets[i].magnitude;
            Vector3 pos = prev.position + limitLength;
            next.position = pos;

            Quaternion prevRot = prev.rotation;

            Quaternion targetRot = Quaternion.LookRotation(next.position - prev.position, prev.up);
            next.rotation = Quaternion.Slerp(next.rotation, targetRot, angularDamping * Time.fixedDeltaTime);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start() {
        Vector3 position = transform.position;

        SegmentPosition sp = new SegmentPosition() {
            x = position.x,
            z = position.z
        };

        SegmentPosition[] spArr = new SegmentPosition[3];
        spArr[0] = sp;
        spArr[1] = sp;
        spArr[2] = sp;

        SegmentsPositions segmentPositions = new SegmentsPositions() {
            sPs = spArr
        };

        string s = JsonUtility.ToJson(segmentPositions);

        Debug.Log(s);
    }
}


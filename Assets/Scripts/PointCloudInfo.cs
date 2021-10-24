using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System;

public class PointCloudInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Log;

    ARPointCloud arPointCloud;
    // Start is called before the first frame update
    private void OnEnable()
    {
        arPointCloud = GetComponent<ARPointCloud>();
        arPointCloud.updated += OnPointCloudChanged;
    }

    private void OnDisable()
    {
        arPointCloud.updated -= OnPointCloudChanged;
    }

    private void OnPointCloudChanged(ARPointCloudUpdatedEventArgs data)
    {
        if (!arPointCloud.positions.HasValue ||
            !arPointCloud.identifiers.HasValue ||
            !arPointCloud.confidenceValues.HasValue)
            return;

        var positions = arPointCloud.positions.Value;
        var identifiers = arPointCloud.identifiers.Value;
        var confidenceValues = arPointCloud.confidenceValues.Value;

        if (positions.Length == 0) return;

        var logText = "Number of points: " + positions.Length
                       + "\nPoint info: x = " + positions[0].x +
                       " y = " + positions[0].y +
                       " z = " + positions[0].z +
                       ",\n Identifier = " + identifiers[0] + ", Confidence = " + confidenceValues[0];

        if (Log)
        {
            Log.text = logText;
        }
        else
        {
            Debug.Log(logText);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceHologram : MonoBehaviour
{

    [SerializeField] GameObject prefabToPlace;
    ARRaycastManager raycastManager;
    ARAnchorManager anchorManager;

    static List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    private void Update()
    {
        Touch touch;

        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) return;

        if(raycastManager.Raycast(touch.position, Hits, TrackableType.All))
        { 
            CreateAnchor(Hits[0]);

            Debug.Log("Instantiated on: " + Hits[0].hitType);
        }
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        ARAnchor anchor;


        //if we hit a plane
        if(hit.trackable is ARPlane plane)
        {
            var planeManager = GetComponent<ARPlaneManager>();
            if (planeManager)
            {
                var oldPrefab = anchorManager.anchorPrefab;
                anchorManager.anchorPrefab = prefabToPlace;
                anchor = anchorManager.AttachAnchor(plane, hit.pose);
                anchorManager.anchorPrefab = oldPrefab;

                return anchor;
            }
        }

        var instantiatedObj = Instantiate(prefabToPlace, hit.pose.position, hit.pose.rotation);

        anchor = instantiatedObj.GetComponent<ARAnchor>();

        if(anchor == null)
        {
            instantiatedObj.AddComponent<ARAnchor>();
        }

        return anchor;
    }
}

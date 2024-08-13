using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceImage : MonoBehaviour
{
    //reference to ARTrackedImageManager Component
    private ARTrackedImageManager trackedImageManager;
    //reference to prefabs of model collection
    [SerializeField] private GameObject[] models;
    //dictionary to keep tracked of model in scene
    private Dictionary<String, GameObject> modelDict;

    void Awake(){
        modelDict = new Dictionary<string, GameObject>();
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    void OnEnable(){
        trackedImageManager.trackedImagesChanged += OnTrackedImageChange;
    }
    void OnDisable(){
        trackedImageManager.trackedImagesChanged -= OnTrackedImageChange;
    }

    private void OnTrackedImageChange(ARTrackedImagesChangedEventArgs args)
    {
        Debug.Log("Ke-Track");
        // Add Model to Scene
        foreach (var trackedItem in args.added)
        {
            Debug.Log("Ada yang masuk");
            String itemName = trackedItem.referenceImage.name;
            foreach (var model in models)
            {
                if(!modelDict.ContainsKey(itemName) && String.Compare(itemName, model.name, StringComparison.OrdinalIgnoreCase) == 0){
                    GameObject newItem = Instantiate(model, trackedItem.transform);
                    modelDict.Add(itemName,newItem);
                }
            }
        }

        // Update existing Model
        foreach (var trackedItem in args.updated)
        {
            String itemName = trackedItem.name;
            // Debug.Log(itemName);
            if(modelDict.ContainsKey(itemName)) modelDict[itemName].SetActive(trackedItem.trackingState == TrackingState.Tracking);
        }

        // Remove Model
        foreach (var trackedItem in args.removed)
        {
            String itemName = trackedItem.name;
            Destroy(modelDict[itemName]);
            modelDict.Remove(itemName);
        }
    }
}

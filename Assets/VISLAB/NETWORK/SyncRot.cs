using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncRot : MonoBehaviour
{
    public Transform objectToSync;

    void Start()
    {
        if (objectToSync == null)
        {
            throw new ArgumentNullException("Object to sync can not be null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        objectToSync.transform.rotation = this.transform.rotation;
    }
}
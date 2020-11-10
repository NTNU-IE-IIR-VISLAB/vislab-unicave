using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCopy : MonoBehaviour
{

    public Transform copyFrom;
    public Transform copyTo;

    public Boolean rotation;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);

    private void Update()
    {
        copyTo.position = copyFrom.position + offset;
        if (rotation)
        {
            copyTo.rotation = copyFrom.rotation;
        }
    }

}
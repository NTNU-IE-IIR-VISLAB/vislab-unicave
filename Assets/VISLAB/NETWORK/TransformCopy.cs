using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Copies an objects position to another object, with optional rotational copy.
/// The transform copy can also have an offset, where the offest is applied to the 
/// object the transform is copied to. 
/// </summary>
public class TransformCopy : MonoBehaviour
{
    [SerializeField]
    private Transform copyFrom;

    [SerializeField]
    private Transform copyTo;

    [SerializeField]
    private Boolean rotation;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 7.5f, 0f);

    private void Start()
    {
        if (this.copyFrom is null) throw new MissingComponentException("The object to copy from can not be null");
        if (this.copyTo is null) throw new MissingComponentException("The object to copy to can not be null");
    }

    private void Update()
    {
        copyTo.position = copyFrom.position + offset;
        if (rotation)
        {
            copyTo.rotation = copyFrom.rotation;
        }
    }

}
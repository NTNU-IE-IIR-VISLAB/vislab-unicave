using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates this game object around a target game object at given speed.
/// The speed and target is set in the inspector for this game object.
/// </summary>
public class RotateAround : MonoBehaviour
{

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed = 10;
    // Update is called onctargeter frame
    void Update()
    {
        this.transform.RotateAround(this.target.transform.position, Vector3.up, this.speed * Time.deltaTime);
    }
}

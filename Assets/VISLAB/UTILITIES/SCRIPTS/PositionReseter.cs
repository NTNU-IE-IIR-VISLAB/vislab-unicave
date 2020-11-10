using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script that allows for restting the position of the game object to the position the 
/// game object had when it was first started.
/// The key for resetting is configured in the inspector.
/// </summary>
public class PositionReseter : MonoBehaviour
{

    [SerializeField]
    KeyCode keycode = KeyCode.F12;

    Vector3 startpos;
    void Start()
    {
        this.startpos = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKey(this.keycode))
        {
            this.transform.position = startpos;
        }
    }
}
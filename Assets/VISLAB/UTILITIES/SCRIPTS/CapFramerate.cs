using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFramerate : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
    }

}
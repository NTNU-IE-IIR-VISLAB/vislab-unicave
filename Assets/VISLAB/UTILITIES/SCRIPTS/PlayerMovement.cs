using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides simple WASD controller for game objects.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * speed * 3);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += -transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed * 3);
        }
    }
}

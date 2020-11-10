using UnityEngine;

/// <summary>
/// Provides mouse look functionality by holding mouse button and moving the mouse.
/// 
/// This script comes from UnityStands assets script bundle
/// </summary>
public class LookCamera : MonoBehaviour 
{
    public float speedNormal = 10.0f;
    public float speedFast   = 50.0f;

    public float mouseSensitivityX = 5.0f;
	public float mouseSensitivityY = 5.0f;
    
	float rotY = 0.0f;
    
	void Start()
	{
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void Update()
	{	   
        if (Input.GetMouseButton(1)) 
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }		

	}
}

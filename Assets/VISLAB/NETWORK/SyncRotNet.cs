using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.006f, channel = 2)]
public class SyncRotNet : NetworkBehaviour
{
    [SyncVar]
    Quaternion newRot;

    [SyncVar]
    Vector3 newPos;
    // Start is called before the first frame update
    void Start()
    {
        // this.gameObject.GetComponent<CharacterController>().enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isServer)
        {
            this.newRot = this.transform.rotation;
            this.newPos = this.transform.position;
            return;
        };
        this.transform.position = newPos;
        this.transform.rotation = newRot;

    }

}
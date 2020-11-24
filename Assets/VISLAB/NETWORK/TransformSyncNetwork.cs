using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Synchronizes this gameobjects position and rotation from the server to all clients.
/// Only the server will update the rotation and postion, and the clients will set the
/// position of the gameobject to the position provided by the server.
/// </summary>
[NetworkSettings(sendInterval = 0.006f, channel = 2)]
public class TransformSyncNetwork : NetworkBehaviour
{
    [SyncVar]
    Quaternion newRot;

    [SyncVar]
    Vector3 newPos;

    /// <summary>
    /// Using late update to ensure all positional calculations are applied
    /// by the engine before setting comitting to the position/rotation for this frame.
    /// </summary>
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
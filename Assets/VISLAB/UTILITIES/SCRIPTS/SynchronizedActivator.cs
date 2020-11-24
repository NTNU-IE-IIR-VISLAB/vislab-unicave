using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ObjectActivate
{
    public bool serverOnly = false;

    public GameObject gameobjectToToggle;

    public KeyCode keycode;

    public bool startEnabled;

}

/// <summary>
/// Provides synchronized activation/deactivation for given game object over the network.
/// A object can be activated/deactivated with a keycode, and can be configured to start enabled/disabled and if the game object
/// should only be activated/deactivated on the server or if it should propagate to all clients.
/// Only the server is allowed to control the object state.
/// </summary>
[RequireComponent(typeof(NetworkIdentity))]
public class SynchronizedActivator : NetworkBehaviour
{
    [SerializeField]
    private ObjectActivate[] toggableObjects;

    [SerializeField]

    private List<KeyCode> availableKeyCodes = new List<KeyCode>();

    /// <summary>
    /// Iterates over all toggleable objects and activate/deactivates the by
    /// their set start state.
    /// </summary>
    private void Start()
    {
        foreach (var ob in toggableObjects)
        {
            ob.gameobjectToToggle.SetActive(ob.startEnabled);
            availableKeyCodes.Add(ob.keycode);
        }
    }

    private void Update()
    {
        if (!isServer) return;
        for (int i = 0; i < toggableObjects.Length; i++)
        {
            var ob = toggableObjects[i];
            if (Input.GetKeyDown(ob.keycode))
            {
                var isActive = !ob.gameobjectToToggle.activeSelf;
                ob.gameobjectToToggle.SetActive(isActive);
                if (!ob.serverOnly)
                {
                    RpcToggleObject(i, isActive);
                }
            }
        }

    }

    /// <summary>
    /// Sends a RPC command to all clients that the game object at provided id should either deactive or activate.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="toggle"></param>
    [ClientRpc]
    private void RpcToggleObject(int index, bool toggle)
    {
        toggableObjects[index].gameobjectToToggle.SetActive(toggle);
    }
}

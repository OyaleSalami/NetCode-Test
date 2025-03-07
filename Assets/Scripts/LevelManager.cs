using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform[] sides;
    [SerializeField] PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            int i = 0;
            foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
            {
                NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<Player>().SetPosition(sides[i]);
                i++;
            }
        }
        else
        {
            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            playerObject.GetComponent<Player>().SetPosition(sides[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

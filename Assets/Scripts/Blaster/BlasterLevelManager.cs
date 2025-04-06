using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BlasterLevelManager : NetworkBehaviour
{
    public Transform[] sides;

    [Header("Touch Controls")]
    [SerializeField] Image coolDownImage;
    [SerializeField] Button fireButton;
    [SerializeField] PButton leftButton;
    [SerializeField] PButton rightButton;

    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            int i = 0;
            foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(i >= 2) //Spectator joined
                {
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<BlasterPlayer>().playerType = PlayerType.spectator;
                }
                else
                {
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<BlasterPlayer>().SetPosition(sides[i]);
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<BlasterPlayer>().playerType = PlayerType.player;
                }
                i++;
            }
        }

        NetworkManager.Singleton.OnClientStarted += SetTouchControls;
    }

    void SetTouchControls()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BlasterPlayer>();

        if(playerObject != null)
        {
            leftButton.onClick.AddListener(playerObject.MoveLeft);
            rightButton.onClick.AddListener(playerObject.MoveRight);
            fireButton.onClick.AddListener(playerObject.Fire);
            playerObject.coolDownImage = coolDownImage;
        }
    }
}

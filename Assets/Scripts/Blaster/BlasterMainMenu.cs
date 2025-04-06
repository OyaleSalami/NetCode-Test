using Unity.Netcode;
using UnityEngine;

public class BlasterMainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject hudMenu;

    public void HostGame()
    {
        var check = NetworkManager.Singleton.StartHost();
        if (check)
        {
            mainMenu.SetActive(false);
            hudMenu.SetActive(true);
        }
    }

    public void JoinGame()
    {
        var check = NetworkManager.Singleton.StartClient();
        if (check)
        {
            mainMenu.SetActive(false);
            hudMenu.SetActive(true);
        }
    }
}

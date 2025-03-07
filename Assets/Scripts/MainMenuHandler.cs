using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text logText;
    [SerializeField] Text modeText;
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] GameObject modePanel;
    [SerializeField] GameObject hostPanel;
    [SerializeField] GameObject joinPanel;

    private bool allowSpecator;
    private int maxPlayerCount = 2;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
    }

    #region UI Functions
    public void SetLog(string text)
    {
        logText.text = text;
        Invoke(nameof(ClearText), 3f); //Set timer to clear text
    }

    public void ClearText()
    {
        logText.text = "";
    }

    public void HostGameButtonClicked()
    {
        if (usernameField.text == "")
        {
            SetLog("Enter a proper username");
            return;
        }
        modePanel.SetActive(false);
        hostPanel.SetActive(true);
    }

    public void JoinGameButtonClicked()
    {
        if (usernameField.text == "")
        {
            SetLog("Enter a proper username");
            return;
        }
        modePanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    #endregion

    public void HostGame()
    {
        if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient) //Not a server nor a client
        {
            if (NetworkManager.Singleton.StartHost()) //Start as the host (Successful)
            {
                hostPanel.SetActive(false);
                modeText.gameObject.SetActive(true);
                modeText.text = "You are the host";
            }
            else //Failure
            {
                SetLog("Unable to start as a host!");
            }
        }
    }

    public void JoinGame()
    {
        if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.StartClient()) //Start as a Client (Succesful)
            {
                modePanel.SetActive(false);
                modeText.gameObject.SetActive(true);
                modeText.GetComponent<TMP_Text>().text = "You have joined a game succesfully";
            }
            else //Failure
            {
                Debug.Log("Unable to join game!");

            }
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }

    private void OnClientDisconnectCallback(ulong obj)
    {
        if (!NetworkManager.Singleton.IsServer && NetworkManager.Singleton.DisconnectReason != string.Empty)
        {
            SetLog($"Approval Declined Reason: {NetworkManager.Singleton.DisconnectReason}");
        }
    }

}

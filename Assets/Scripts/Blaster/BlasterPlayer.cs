using Unity.Netcode;
using UnityEngine;

public class BlasterPlayer : NetworkBehaviour
{
    public PlayerType playerType;

    [SerializeField] GameObject prop;
    [SerializeField] float movementSpeed = 5.0f;

    [Header("Extra Stuff")]
    [SerializeField] GameObject cam;


    private void Start()
    {
        if (playerType == PlayerType.spectator)
        {

        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) //Disable a bunch of stuff (Player Movement, Camera, Audio)
        {
            cam.SetActive(false);
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            prop.transform.position = MovePlayer(MoveDirection.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            prop.transform.position = MovePlayer(MoveDirection.right);
        }
    }

    public Vector3 MovePlayer(MoveDirection dir)
    {
        var temp = ((dir == MoveDirection.right) ? Vector3.right : Vector3.left) * (Time.deltaTime * movementSpeed) + prop.transform.position;
        temp.x = Mathf.Clamp(temp.x, -3, 3);
        return temp;
    }

    public void MoveLeft()
    {
        prop.transform.position = MovePlayer(MoveDirection.left);
    }

    public void MoveRight()
    {
        prop.transform.position = MovePlayer(MoveDirection.right);
    }

    public void SetPosition(Transform side)
    {
        transform.SetPositionAndRotation(side.position, side.rotation);
    }
}



using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] PlayerType type;
    [SerializeField] GameObject prop;
    [SerializeField] float movementSpeed = 5.0f;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //Select a side
            //transform.position = side;
        }
    }

    void Update()
    {
        transform.position = Position.Value; //Update the player position
    }

    #region Player Actions
    public void MoveLeft()
    {
        //TODO: Server Authentication
        if (!(transform.position.x < -3))
        {
            transform.Translate(Time.deltaTime * movementSpeed * Vector3.right);
        }
    }

    public void MoveRight()
    {
        //TODO: Server Authentication
        if (!(transform.position.x > 3))
        {
            transform.Translate(Time.deltaTime * movementSpeed * Vector3.left);
        }
    }
    #endregion


    [ServerRpc] public void SubmitMoveRequestServerRpc(MoveDirection dir, ServerRpcParams rpcParams  = default)
    {
        Position.Value = MovePlayer(dir);
    }

    public Vector3 MovePlayer(MoveDirection dir)
    {
        var temp = ((dir == MoveDirection.left) ? Vector3.right : Vector3.left) * (Time.deltaTime * movementSpeed) + prop.transform.position;
        temp.x = Mathf.Clamp(temp.x, -3, 3);
        return temp;
    }

    public void SetPosition(Transform side)
    {
        transform.SetPositionAndRotation(side.position, side.rotation);
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc] void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }
}



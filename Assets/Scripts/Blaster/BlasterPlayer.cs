using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BlasterPlayer : NetworkBehaviour
{
    public PlayerType playerType;

    [Header("Player Movement")]
    [SerializeField] GameObject prop;
    [SerializeField] float movementSpeed = 5.0f;

    [Header("Canon")]
    [SerializeField] float blastForce = 500.0f;
    [SerializeField] GameObject canonPrefab;
    [SerializeField] Transform firingPoint;
    [SerializeField] Image coolDownImage;
    [SerializeField] float coolDownTime = 1f;
    private bool canShoot = true;
    private float c = 0;

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

        if (canShoot == false)
        {
            c += Time.deltaTime;
            coolDownImage.fillAmount = c / coolDownTime;

            if (c >= coolDownTime)
            {
                canShoot = true;
            }
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

    public void Fire()
    {
        if (canShoot)
        {
            var temp = Instantiate(canonPrefab, firingPoint);
            temp.GetComponent<Rigidbody>().AddForce(Vector3.forward * blastForce);
            canShoot = false;
            c = 0f;
        }
    }
}



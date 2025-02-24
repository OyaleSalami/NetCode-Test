using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5.0f;

    [Header("Canon Firing")]
    [SerializeField] float blastForce = 500.0f;
    [SerializeField] GameObject canonPrefab;
    [SerializeField] Transform firingPoint;
    [SerializeField] Image coolDownImage;
    [SerializeField] float coolDownTime = 1f;
    private bool canShoot = true;
    private float c = 0;

    private void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed * Vector3.left);
        
        if ((transform.position.x <= -3))
        {
            transform.position = new Vector3(-3f, transform.position.y, transform.position.z);
        }

        if ((transform.position.x >= 3))
        {
            transform.position = new Vector3(3f, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    
        if(canShoot == false)
        {
            c += Time.deltaTime;
            coolDownImage.fillAmount = c/coolDownTime;
            
            if(c >= coolDownTime)
            {
                canShoot = true;
            }
        }
    }

    public void MoveLeft()
    {
        if (!(transform.position.x < -3))
        {
            transform.Translate(Time.deltaTime * movementSpeed * Vector3.right);
        }
    }

    public void MoveRight()
    {
        if(!(transform.position.x > 3))
        {
            transform.Translate(Time.deltaTime * movementSpeed * Vector3.left);
        }
    }

    public void Fire()
    {
        if(canShoot)
        {
            var temp = Instantiate(canonPrefab, firingPoint);
            temp.GetComponent<Rigidbody>().AddForce(Vector3.forward * blastForce);
            canShoot = false;
            c = 0f;
        }
    }
}

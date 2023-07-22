using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaunchProiectile : MonoBehaviour
{
    public GameObject projectile;
    public float speed;
    public Camera playerCam;
    public Transform shootPoint;
    public bool auto;
    public float ammo, maxAmmo;
    public bool shooting, canShoot, reloading;
    public float reloadSpeed;
    public float timeBetweenShots;
    public Image[] bulletImage;
    public Sprite bulletSprite;
    public float bulletsFired;
    public GameObject bulletUI;
    public float spread;
    public Rigidbody player;
    public float recoil;
    public float distanceBetweenIcons;

    // Start is called before the first frame update
    void Start()
    {

        canShoot = true;
        AmmoIconControl();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AmmoIconControl();

        if (Input.GetKey(KeyCode.Mouse0) && auto)
        {
            shooting = true;
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (shooting && canShoot && ammo > 0)
        {
            canShoot = false;
            Invoke("ResetShot", timeBetweenShots);
            Shoot();
            
            

        }
        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && !reloading)
        {
            Reload();
        }
        if (ammo <= 0)
        {
            canShoot = false;
        }
        if (ammo > 0 && !reloading && !canShoot)
        {
            
            Invoke("ResetShot", timeBetweenShots);
        }



    }


    void Shoot()
    {

        canShoot = false;
        
        AmmoIconControl();

        RaycastHit hit;
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);


        }
        Vector3 direction = targetPoint - shootPoint.position;
        player.AddForce(direction.normalized * -1 * recoil);

        for (int i = 0; i < bulletsFired; i++)
        {
            canShoot = false;
            GameObject currentBullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);

            currentBullet.transform.forward = direction.normalized;
            currentBullet.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));

            currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * speed, ForceMode.Impulse);

        }



        Invoke("ResetShot", timeBetweenShots);

        ammo--;
    }

    void ResetShot()
    {
        canShoot = true;
    }

    void Reload()
    {
        reloading = true;
        canShoot= false;
        Invoke("FinishReload", reloadSpeed);
    }

    void FinishReload()
    {
        ammo = maxAmmo;
        reloading = false;
    }

    void AmmoIconControl()
    {
        
        for(int i = 0; i < bulletImage.Length; i++)
        {
            Debug.Log(i);
            
            bulletImage[i].sprite = bulletSprite;
            bulletImage[i].gameObject.transform.position = new Vector3(bulletImage[0].transform.position.x + distanceBetweenIcons * i, bulletImage[0].transform.position.y, bulletImage[0].transform.position.z);
            if(i >= ammo)
            {
                bulletImage[i].enabled = false;
            } else
            {
                bulletImage[i].enabled = true;  
            }
            
        }

    }
}

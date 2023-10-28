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
    public bool resetting;
    public Animator animator;
    public bool firing;

    // Start is called before the first frame update
    void Start()
    {
        firing = false;
        canShoot = true;
        AmmoIconControl();
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        AmmoIconControl();





        if ((reloading || resetting || ammo <= 0))
        {
            canShoot = false;
        }
        else if (!resetting && ammo > 0 || !reloading && ammo > 0) canShoot = true; 
        if(firing)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
        } else
        {
            animator.SetBool("Idle", true) ;
            animator.SetBool("Fire", false) ;
        }

    }

     void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && auto)
        {
            shooting = true;
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && !reloading)
        {
            Reload();
        }

        if (shooting && canShoot && ammo > 0)
        {
            firing = true;
            canShoot = false;
            Shoot();

        }
    }

    void Shoot()
    {
        animator.SetBool("Fire", true);
        
        ResetShot();
        
        
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



        
        ammo--;
    }

    void ResetShot()
    {
        resetting = true;
        canShoot= false;
        Invoke("FinishReset", timeBetweenShots);
        
    }

    void FinishReset()
    {
        
        resetting = false;
        firing = false;
    }

    void Reload()
    {
        reloading = true;
        canShoot= false;
        Invoke("FinishReload", reloadSpeed);
        animator.SetBool("Reloading", true);
    }

    void FinishReload()
    {
        ammo = maxAmmo;
        animator.SetBool("Reloading", false);
        reloading = false;
    }

    void AmmoIconControl()
    {
        
        for(int i = 0; i < bulletImage.Length; i++)
        {
           
            
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

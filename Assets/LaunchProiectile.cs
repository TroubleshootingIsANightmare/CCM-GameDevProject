using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) && auto)
        {
            shooting = true;
        } else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if(shooting && canShoot)
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && !reloading)
        {
            Reload();
        }
        if(ammo <= 0)
        {
            canShoot= false;
        }
    }


    void Shoot()
    {
        canShoot = false;
        RaycastHit hit;
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(75);

            
        }
        Vector3 direction = targetPoint - shootPoint.position;

        GameObject currentBullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);

        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * speed, ForceMode.Impulse);


        Invoke("ResetShot", timeBetweenShots);

        ammo--;
    }

    void ResetShot()
    {
        canShoot = true;
    }

    void Reload()
    {
        reloading= true;
        Invoke("FinishReload", reloadSpeed);
    }

    void FinishReload()
    {
        ammo = maxAmmo;
        reloading = false;
    }
}

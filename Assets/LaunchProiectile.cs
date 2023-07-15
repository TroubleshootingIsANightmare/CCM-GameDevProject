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

    public float bulletsFired;
    public float spread;

    // Start is called before the first frame update
    void Start()
    {

        canShoot = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        Invoke("FinishReload", reloadSpeed);
    }

    void FinishReload()
    {
        ammo = maxAmmo;
        reloading = false;
    }
}

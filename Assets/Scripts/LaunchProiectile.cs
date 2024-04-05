using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LaunchProiectile : MonoBehaviour
{
    public GameObject projectile;
    public Vector3 originalPosition;
    public bool chargable, charging;
    public float chargeTimer, chargeForce, chargeMinimum;
    public GameObject chargeBullet, originalBullet;
    public float speed;
    public Camera playerCam;
    public Transform shootPoint;
    public bool auto;
    public float ammo, maxAmmo;
    public bool shooting, canshoot, reloading;
    public float reloadSpeed;
    public float timeBetweenShots;
    public Image[] bulletImage;
    public Sprite bulletSprite;
    public float bulletsFired;
    public float spread;
    public Rigidbody player;
    public float recoil;
    public float distanceBetweenIcons;
    public bool resetting;
    public Animator animator;
    public bool firing;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {

        originalPosition = transform.position;
        firing = false;
        canshoot = true;
        ammoIconControl();
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        animator.SetBool("Idle", true);
        animator.SetBool("Fire", false);
        animator.SetBool("Reloading", false);
        projectile = originalBullet;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        ammoIconControl();

        checkCanshoot();




        if (firing)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Fire", false);
        }

    }

    void Update()
    {
        checkShooting();

        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && !reloading)
        {
            reload();
        }


        if (charging)
        {
            chargeTimer += Time.deltaTime * 3f;

        }
        if (chargeTimer >= chargeMinimum)
        {
            projectile = chargeBullet;

            if (chargeTimer > 20f)
            {
                chargeTimer = 20f;
            }
        }
        else
        {
            projectile = originalBullet;
        }
        chargeForce = chargeTimer * 150f;
    }

    void shoot()
    {
        animator.SetBool("Fire", true);

        resetShot();


        ammoIconControl();

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

        direction = targetPoint - shootPoint.position;
        player.AddForce(direction.normalized * -1 * (recoil + chargeForce));



        multishot();


        ammo--;
    }

    void multishot() {
        for (int i = 0; i < bulletsFired; i++)
        {
            canshoot = false;
            GameObject currentBullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);

            currentBullet.transform.forward = direction.normalized;
            currentBullet.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));

            currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * (speed), ForceMode.Impulse);

        }
    }

    void resetShot()
    {
        resetting = true;
        canshoot = false;
        Invoke("finishReset", timeBetweenShots);
    }

    void finishReset()
    {
        resetting = false;
        firing = false;
    }

    void reload()
    {
        reloading = true;
        canshoot = false;
        charging = false;
        Invoke("FinishReload", reloadSpeed);
        animator.SetBool("Reloading", true);
    }

    void finishReload()
    {
        ammo = maxAmmo;
        animator.SetBool("Reloading", false);
        reloading = false;
    }

    void ammoIconControl()
    {

        for (int i = 0; i < bulletImage.Length; i++)
        {


            bulletImage[i].sprite = bulletSprite;
            bulletImage[i].gameObject.transform.position = new Vector3(bulletImage[0].transform.position.x + distanceBetweenIcons * i, bulletImage[0].transform.position.y, bulletImage[0].transform.position.z);
            if (i >= ammo)
            {
                bulletImage[i].enabled = false;
            }
            else
            {
                bulletImage[i].enabled = true;
            }

        }

    }

    void checkCanshoot()
    {
        if ((reloading || resetting || ammo <= 0))
        {
            canshoot = false;
        }
        else if (ammo > 0)
        {
            if (!resetting || !reloading)
            {
                canshoot = true;
            }
        }
    }
    void checkShooting() {
        if(auto) {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            if(!chargable) shooting = Input.GetKeyDown(KeyCode.Mouse0);
            if(chargable && canshoot) charging = Input.GetKey(KeyCode.Mouse0); else chargeTimer = 0f;
            if(charging) shooting = Input.GetKeyUp(KeyCode.Mouse0);
        }
        if (shooting && canshoot && ammo > 0)
        {
            firing = true;
            canshoot = false;
            shoot();
        }
    }
}

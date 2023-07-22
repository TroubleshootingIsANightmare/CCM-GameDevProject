using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeapon = 0;
    private int nrWeapons;

    // Start is called before the first frame update
    void Start()
    {
        nrWeapons = weapons.Length;

        SwitchWeapon(currentWeapon); // Set default gun
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 1; i <= nrWeapons; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                currentWeapon = i - 1;

                SwitchWeapon(currentWeapon);

            }
        }
    }

    void SwitchWeapon(int index)
    {
        for (int i = 0; i < nrWeapons; i++)
        {
            if (i == index)
            {
                weapons[index].SetActive(true);
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
    }
}

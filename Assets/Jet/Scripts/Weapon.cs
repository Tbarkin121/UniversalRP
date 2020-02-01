using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePointLeft;
    public Transform firePointRight;
    public GameObject missilePrefab;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            ShootMissile();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            ShootMachineGun();
        }
        void ShootMissile()
        {
            Instantiate(missilePrefab, firePointRight.position, Quaternion.Euler(firePointRight.up));
        }
        void ShootMachineGun()
        {

        }
    }
}

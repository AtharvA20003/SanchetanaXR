
using System;
using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    public int selectedWeapon = 0;

    private PlayerController playerController;
    private PlayerController.WeaponActions weaponActions;

    void Awake()
    {
        playerController = new PlayerController();
        weaponActions = playerController.Weapon;
        weaponActions.SwitchWeapon.started += ctx => SelectedWeaponIncrement();
        //{ int temp = selectedWeapon; temp++; selectedWeapon = temp % 3; SelectWeapon(); };
        
    }

    private void SelectedWeaponIncrement()
    {
        Debug.Log("CAlled increment method");
        selectedWeapon++;
        selectedWeapon = selectedWeapon % 3;
        SelectWeapon();
    }


    private void OnEnable()
    {
        weaponActions.Enable();
    }

    private void OnDisable()
    {
        weaponActions.Disable();
    }

    /*void Start()
    {
        SelectWeapon();
    }*/

    void SelectWeapon()
    {
        Debug.Log("SelectWeapon() called with selectedWeapon = " + selectedWeapon);
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}

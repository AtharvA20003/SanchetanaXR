using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInputHandler : MonoBehaviour
{

    private PlayerController playerController;
    private PlayerController.WeaponActions weaponActions;
    private Weapon weapon;

    void Awake()
    {
        playerController = new PlayerController();
        weaponActions = playerController.Weapon;
        weapon = GetComponent<Weapon>();
        weaponActions.Fire.started += ctx => weapon.FireWeapon();
        weaponActions.Reload.started += ctx => weapon.ReloadWeapon();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        weaponActions.Enable();
    }

    private void OnDisable()
    {
        weaponActions.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerController.GeneralTasksActions generalTasks;
    private Vector2 movementInput;
    private PlayerMotor motor;

    //private PlayerController.WeaponActions weaponActions;
    //private Weapon weapon;
    void Awake()
    {
        playerController = new PlayerController();
        generalTasks = playerController.GeneralTasks;
        motor = GetComponent<PlayerMotor>();
        generalTasks.Jump.performed += ctx => motor.Jump();


      /*  playerController2 = new PlayerController();
        weaponActions = playerController2.Weapon;
        weapon = GetComponent<Weapon>();
        weaponActions.Fire.performed += ctx => weapon.FireWeapon(); */
    }


    void Update()
    {
        motor.ProcessMove(generalTasks.Movement.ReadValue<Vector2>());  
    }
    private void OnEnable()
    {
        generalTasks.Enable();
        //playerController.Weapon.Enable();
    }

    private void OnDisable()
    {
        generalTasks.Disable();
        //playerController.Weapon.Disable();
    }
}

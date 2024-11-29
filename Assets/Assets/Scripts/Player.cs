using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControlls Controls { get; private set; }
    public PlayerAim PlayerAim { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public WeaponController WeaponController { get; private set; }
    public PlayerWeaponVisual WeaponVisual { get; private set; }
    private void Awake()
    {
        Controls = new PlayerControlls();
        PlayerAim = GetComponent<PlayerAim>();
        PlayerMovement = GetComponent<PlayerMovement>();
        WeaponController = GetComponent<WeaponController>();
        WeaponVisual = GetComponent<PlayerWeaponVisual>();
    }

    private void OnEnable()
    {
       Controls.Enable();
    }
    private void OnDisable()
    {
       Controls.Disable();
    }
}
 
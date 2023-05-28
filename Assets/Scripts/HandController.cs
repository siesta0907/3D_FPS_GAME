using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : ClossWeaponController
{
    public static bool isActivate = false;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        
            while (isSwing)
            {
                if (CheckObject())
                {
                    //충돌했음.
                    isSwing = false;

                }
                yield return null;
            }
        }
    public override void CloseWeaponChange(ClossWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}

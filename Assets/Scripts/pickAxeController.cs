using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickAxeController : ClossWeaponController
{
    public static bool isActivate = true;


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
                if (hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
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

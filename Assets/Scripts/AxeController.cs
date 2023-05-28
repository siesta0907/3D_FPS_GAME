using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : ClossWeaponController
{
    public static bool isActivate = false;

    private void Start()
    {

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
                if (hitInfo.transform.tag == "Tree")
                {
                    hitInfo.transform.GetComponent<Tree>().Mining();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private GunController gunController;
    //정확도
    private float gunAccuracy;

    //크로스 헤어 비활성을 위한 부모 객체
    [SerializeField]
    private GameObject go_CrossGairHud;

    
    public void WalkingAnimation (bool _flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", _flag); 
        Animator.SetBool("walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
        Animator.SetBool("running", _flag);
    }
    public void JumpAnimation(bool _flag)
    {
        Animator.SetBool("running", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        Animator.SetBool("crouching", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        Animator.SetBool("finesight", _flag);
    }

    public void FireAinmation()
    {
        if(Animator.GetBool("walking"))
        {
            Animator.SetTrigger("walkFire");
        }
        else if (Animator.GetBool("crouching"))
        {
            Animator.SetTrigger("CrouchFire");
        }
       
        else
        {
            Animator.SetTrigger("idleFire");
        }
    }

    public float GetAccuracy()
    {
        if (Animator.GetBool("walking")) gunAccuracy = 0.06f;
        else if (Animator.GetBool("crouching")) gunAccuracy = 0.015f;
        else if (gunController.GetFineSightMode()) gunAccuracy = 0.001f;
        else gunAccuracy = 0.035f;
        return gunAccuracy;
    }
}

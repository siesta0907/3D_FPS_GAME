using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClossWeaponController : MonoBehaviour
{


    //현재 장착된 무기 타입
    [SerializeField]
    protected ClossWeapon currentCloseWeapon;


    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo;


    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);

        isSwing = true;
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);

        isSwing = false;
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }
    protected abstract IEnumerator HitCoroutine();
   
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            Debug.Log(hitInfo);
            return true;
        }
        return false;
    }

    //완성함수이지만, 추가 편집 가능한 함수
    public virtual void CloseWeaponChange(ClossWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _closeWeapon;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
        //초기화
        //currentCloseWeapon.transform.localPosition = new Vector3(1.38f, 0.47f, 0);  //맨손으로 바꿀때 위치가 이상해서 바꿈

        currentCloseWeapon.gameObject.SetActive(true);

    }
}

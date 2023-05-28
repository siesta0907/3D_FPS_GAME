using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClossWeaponController : MonoBehaviour
{


    //���� ������ ���� Ÿ��
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
                //�ڷ�ƾ ����
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

    //�ϼ��Լ�������, �߰� ���� ������ �Լ�
    public virtual void CloseWeaponChange(ClossWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _closeWeapon;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
        //�ʱ�ȭ
        //currentCloseWeapon.transform.localPosition = new Vector3(1.38f, 0.47f, 0);  //�Ǽ����� �ٲܶ� ��ġ�� �̻��ؼ� �ٲ�

        currentCloseWeapon.gameObject.SetActive(true);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClossWeaponController : MonoBehaviour
{
    public static bool isActivate = false;

    //현재 장착된 무기 타입
    [SerializeField]
    private ClossWeapon currentHand;


    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;



    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    private void TryAttack()
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
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);

        isSwing = true;
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayB);

        isSwing = false;
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //충돌했음.
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void HandChange(ClossWeapon _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = _hand;

        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;
        //초기화
        currentHand.transform.localPosition = new Vector3(1.38f, 0.47f, 0);  //맨손으로 바꿀때 위치가 이상해서 바꿈

        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //���� ������ ���� Ÿ��
    [SerializeField]
    private Hand currentHand;

    
    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;



    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
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
                //�浹����.
                isSwing = false;
                Debug.Log("��");
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
}

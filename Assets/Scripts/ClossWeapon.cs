using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClossWeapon : MonoBehaviour
{
    public string ClossWeaponName; //�տ� ���

    //���� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickAxe;

    public float range; //���ݹ���
    public int damage; //���ݷ�
    public float workspeed; //�۾� �ӵ�
    public float attackDelay; //���� ������
    public float attackDelayA; //���� Ȱ��ȭ ����
    public float attackDelayB; //���� ��Ȱ��ȭ ����

    public Animator anim; 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range; //��������
    public float accuracy; //��Ȯ��
    public float firerate; //����ӵ�
    public float reloadTime; //������ �ӵ�
    public int damage;
    public int reloadBulletCount; //�Ѿ� ������ ����
    public int currentBulletCount; //���� �����ִ� �Ѿ� ����

    public int maxBulletCount; //�ִ���� ����
    public int carryBulletCount; //���� ����

    public float retroActionForce; //�ݵ� ����
    public float retroActionFineSightForce; //�����ؽ� �ݵ�����
    public Vector3 fineSightOriginPos;
    public Animator anim;

    public ParticleSystem muzzleFalsh;
    public AudioClip fire_Sound;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ����
    public static bool isChangeWeapon = false;     //�����ڿ�, ���� ���� ����, static ����, ��ȣ���� ������, �޸� �����
    //���� ����� �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;
    //���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    //���� ��ü ������, ���� ��ü�� ������ ���� ����
    [SerializeField]
    private float chageWeaponDelayTime;
    [SerializeField]
    private float chageWeaponEndDelayTime;

    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private ClossWeapon[] hands;

    //������������ ���� ����
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, ClossWeapon> clossWeaponDic = new Dictionary<string, ClossWeapon>();

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            clossWeaponDic.Add(hands[i].ClossWeaponName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               // Debug.Log("�Ǽ����� �ٲٱ�");
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Debug.Log("��1���� �ٲٱ�");
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_out");
        yield return new WaitForSeconds(chageWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(chageWeaponEndDelayTime);
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                gunController.CancelFIneSight();
                gunController.cancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        
            gunController.GunChange(gunDic[_name]);
        
        else if (_type == "HAND")
        
            handController.HandChange(clossWeaponDic[_name]); 
        
    }

}

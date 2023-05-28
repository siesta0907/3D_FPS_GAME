using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 방지
    public static bool isChangeWeapon = false;     //공유자원, 쉽게 접근 가능, static 정의, 보호수준 떨어짐, 메모리 낭비됨
    //현재 무기와 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;
    //현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    //무기 교체 딜레이, 무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float chageWeaponDelayTime;
    [SerializeField]
    private float chageWeaponEndDelayTime;

    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private ClossWeapon[] hands;
    [SerializeField]
    private ClossWeapon[] axes;
    [SerializeField]
    private ClossWeapon[] pickaxes;

    //관리차원에서 쉽게 만듬
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, ClossWeapon> handDic = new Dictionary<string, ClossWeapon>();
    private Dictionary<string, ClossWeapon> axeDic = new Dictionary<string, ClossWeapon>();
    private Dictionary<string, ClossWeapon> pickaxeDic = new Dictionary<string, ClossWeapon>();

    //필요한 컴포넌트
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private AxeController axeController;
    [SerializeField]
    private pickAxeController pickaxeController;
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].ClossWeaponName, hands[i]);
            Debug.Log(hands[i].ClossWeaponName );
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDic.Add(axes[i].ClossWeaponName,axes[i]);
            Debug.Log(axes[i].ClossWeaponName );
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDic.Add(pickaxes[i].ClossWeaponName, pickaxes[i]);
            Debug.Log(pickaxes[i].ClossWeaponName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               Debug.Log("맨손으로 바꾸기");
                StartCoroutine(ChangeWeaponCoroutine("HAND", "Hand"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("총1으로 바꾸기");
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("도끼로 바꾸기");
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("곡괭이로 바꾸기");
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe"));
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
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                pickAxeController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            gunController.GunChange(gunDic[_name]);
        }
        else if (_type == "HAND")
        {
            handController.CloseWeaponChange(handDic[_name]);
        }
        else if (_type == "AXE")
        {
            axeController.CloseWeaponChange(axeDic[_name]);
        }
        else if (_type == "PICKAXE")
        {
            pickaxeController.CloseWeaponChange(pickaxeDic[_name]);
        }


    }

}
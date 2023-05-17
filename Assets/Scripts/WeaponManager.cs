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

    //관리차원에서 쉽게 만듬
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, ClossWeapon> clossWeaponDic = new Dictionary<string, ClossWeapon>();

    //필요한 컴포넌트
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
               // Debug.Log("맨손으로 바꾸기");
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Debug.Log("총1으로 바꾸기");
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

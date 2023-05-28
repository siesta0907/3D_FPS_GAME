using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //���� Ȱ��ȭ
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun; //���� ��� �ִ� ��
    private CrossHair theCrossHair;
    
    private float currentFireRate; //����ӵ�
    private AudioSource audioSource; //ȿ����
    private bool isReload; //���������ΰ�
    [HideInInspector]
    public bool isfineSightMode = false; //������ ����
    private Vector3 originPos; //���� ��ǥ
    private RaycastHit hitInfo;
    
    [SerializeField]
    private Camera theCam;
    [SerializeField]
    private GameObject hit_effect;

    private void Start()
    {
        currentGun.transform.localPosition = Vector3.zero;
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrossHair = FindObjectOfType<CrossHair>();

        
    }
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }

    }
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            FindSight();
        }
    }
    public void CancelFIneSight()
    {
        if (isfineSightMode)
        {
            StopAllCoroutines();
            isfineSightMode = false;
        }
    }
    private void FindSight()
    {
        isfineSightMode = !isfineSightMode;

        currentGun.anim.SetBool("FindSightMode", isfineSightMode);

        theCrossHair.FineSightAnimation(isfineSightMode);

        if (isfineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FinsGightActivateCoroutine());
        }
        else{
            StopAllCoroutines();
            StartCoroutine(FinsGightDeActivateCoroutine());
        }
    }

    IEnumerator FinsGightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f); //
            yield return null;
        }
    }
    IEnumerator FinsGightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //lerp�� ���� �߰� ���� �ۼ�
            yield return null;
        }
    }

    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && isReload == false && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void cancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //�߻�õ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && isReload == false)
        {
            Fire();
        }
    }
    private void Fire() //�߻��� ���
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0) Shoot();
            else StartCoroutine( ReloadCoroutine());
        }
   
    }
   IEnumerator ReloadCoroutine() //������
    {
        isReload = true;
        if(currentGun.carryBulletCount > 0)
        {
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) //���� ���濡 ���� �ٴϴ°� �����Ǵ� �Ѿ� �������� ���� ��
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; //�ְ� �Ѿ� ������ �Ѿ� ����
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; 
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; //���� �Ѿ��� ���� �Ѿ˿� ����
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
     
    }
    private void Shoot() //�߻���
    {
        theCrossHair.FireAinmation();
        currentGun.currentBulletCount -= 1;
        currentFireRate = currentGun.firerate; //����ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFalsh.Play();
        Hit(); 
        //�ѱ� �ݵ� �ڷ�ƾ > �߻�� �ݵ��� ���� �̷����ߵǱ⶧����
        StopAllCoroutines(); //������ �ݺ����� ���� ���� 
        StartCoroutine(RetroActionCoroutine());
    }
    private void Hit() //�Ѿ��� ������ ������ ����. ���� �Ѿ��� ����� �ʹٸ� ������Ʈ Ǯ���̶�� ����� ����ؾߵ�
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy), 
                            Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                            0)
            , out hitInfo, currentGun.range))
        {
           var clone =  Instantiate(hit_effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    //�ѱ� �ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine() 
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3 (currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //�������ϰ� ������
        if (isfineSightMode)
        {

            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.01f);
                yield return null;
            }
        }
        else //�������ϰ� ���� ������
        {
            
            currentGun.transform.localPosition = originPos; //����ġ
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) //�ݵ� �ڷ� ������
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }
            while (currentGun.transform.localPosition != originPos) //�ݵ� ������ õõ��
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.01f);
                yield return null;
            }
        }
    }

    //����ӵ� ���
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0) currentFireRate -= Time.deltaTime; // 1������(1/60��)

    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isfineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon!= null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentGun = _gun;

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;
        //�ʱ�ȭ
        currentGun.transform.localPosition = Vector3.zero;

        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}



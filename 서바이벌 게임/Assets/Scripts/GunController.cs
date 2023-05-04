using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Gun currentGun; //현재 들고 있는 총
    private CrossHair theCrossHair;
    
    private float currentFireRate; //연사속도
    private AudioSource audioSource; //효과음
    private bool isReload; //재장전중인가
    [HideInInspector]
    private bool isfindSightMode = false; //정조준 상태
    private Vector3 originPos; //원래 좌표
    private RaycastHit hitInfo;
    
    [SerializeField]
    private Camera theCam;
    [SerializeField]
    private GameObject hit_effect;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrossHair = FindObjectOfType<CrossHair>();
    }
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFindSight();
    }
    private void TryFindSight()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            FindSight();
        }
    }
    private void FindSight()
    {
        isfindSightMode = !isfindSightMode;

        currentGun.anim.SetBool("FindSightMode", isfindSightMode);

        theCrossHair.FineSightAnimation(isfindSightMode);

        if (isfindSightMode)
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
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f); //lerp에 대한 추가 설명 작성
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
    //발사시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && isReload == false)
        {
            Fire();
        }
    }
    private void Fire() //발사전 계산
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0) Shoot();
            else StartCoroutine( ReloadCoroutine());
        }
   
    }
   IEnumerator ReloadCoroutine() //재장전
    {
        isReload = true;
        if(currentGun.carryBulletCount > 0)
        {
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) //현재 가방에 갖고 다니는게 장전되는 총알 개수보다 많을 때
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; //최고 총알 개수로 총알 장전
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; 
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; //남은 총알을 현재 총알에 장전
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
     
    }
    private void Shoot() //발사후
    {
        theCrossHair.FireAinmation();
        currentGun.currentBulletCount -= 1;
        currentFireRate = currentGun.firerate; //연사속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFalsh.Play();
        Hit(); 
        //총기 반동 코루틴 > 발사와 반동은 같이 이뤄져야되기때문에
        StopAllCoroutines(); //영원한 반복문을 막기 위해 
        StartCoroutine(RetroActionCoroutine());
    }
    private void Hit() //총알을 실제로 만들진 않음. 만약 총알을 만들고 싶다면 오브젝트 풀링이라는 기법을 사용해야됨
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

    //총기 반동 코루틴
    IEnumerator RetroActionCoroutine() 
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3 (currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //정조준하고 있을때
        if (isfindSightMode)
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
        else //정조준하고 있지 있을때
        {
            
            currentGun.transform.localPosition = originPos; //원위치
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) //반동 뒤로 빠르게
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }
            while (currentGun.transform.localPosition != originPos) //반동 앞으로 천천히
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.01f);
                yield return null;
            }
        }
    }

    //연사속도 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0) currentFireRate -= Time.deltaTime; // 1프레임(1/60초)

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
        return isfindSightMode;
    }
}



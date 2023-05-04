using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GunController gunController;
    private Gun currentGun;

    [SerializeField]
    private GameObject go_BulletHUD;

    //�Ѿ� ���� �ݿ� �ؽ�Ʈ��
    [SerializeField]

    //�Ѿ� ���� �ݿ� �ؽ�Ʈ��
    private TextMeshProUGUI[] text_Bullet;
    
    private void Start()
    {
        
    }
    


    // Update is called once per frame
    void Update()
    {
        CheckBullet();  
    }
    private void CheckBullet()
    {
        currentGun = gunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}

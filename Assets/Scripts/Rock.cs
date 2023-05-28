using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    
    [SerializeField]
    private int hp;

    [SerializeField]
    string destroySound;
    [SerializeField]
    string strikeSound;
    [SerializeField]
    GameObject RockPrefab;
    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        hp--;
        if(hp <= 0)
        {
            SoundManager.instance.PlaySE(destroySound);
            int num = Random.Range(1, 6);
            for (int i = 0; i <num; i++)
            {
                Instantiate(RockPrefab, gameObject.transform.position + new Vector3(0,1,0), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}

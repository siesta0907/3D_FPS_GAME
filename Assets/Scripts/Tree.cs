using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    [SerializeField]
    private int hp;

    [SerializeField]
    string destroySound;
    [SerializeField]
    string strikeSound;
    [SerializeField]
    GameObject TreePrefab;
    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        hp--;
        if (hp <= 0)
        {
            SoundManager.instance.PlaySE(destroySound);
Instantiate(TreePrefab, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}

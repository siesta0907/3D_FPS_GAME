using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;
    
    //ȿ���� ���
    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;
    public string[] playSoundName;
    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Awake() //��ü �����ÿ� ���� ���� 
    {
        //�̱��� -> ���� �ٲ���� �ϳ��� �����ǰԲ� �����!
        //���� �ٲ�� ��� ������Ʈ�� �ı��ȴ�. �׷��⿡ �ϳ��� �����ϴ� �۾��� �ʿ��ѵ� �̰Ÿ� �̱��� �۾��̶�� �Ѵ�. 
        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }

    /*private void OnEnable() //�Ź� Ȱ��Ȱ �� �� ����, �ڷ�ƾ �Ұ� 
    {
        
    }
    void Start() //�Ź� Ȱ��Ȱ �� �� ����, �ڷ�ƾ ���� 
    {
        
    }*/


    //ȿ���� ���
 public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return ;
                    }
                }
            }
        }
    }


    //��� ȿ���� ��ž
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
    }
}

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
    
    //효과음 브금
    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;
    public string[] playSoundName;
    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Awake() //객체 생성시에 최초 실행 
    {
        //싱글톤 -> 씬이 바뀌더라도 하나로 유지되게끔 만드는!
        //씬이 바뀌면 모든 오브젝트가 파괴된다. 그렇기에 하나로 유지하는 작업이 필요한데 이거를 싱글톤 작업이라고 한다. 
        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }

    /*private void OnEnable() //매번 활성활 될 때 실행, 코루틴 불가 
    {
        
    }
    void Start() //매번 활성활 될 때 실행, 코루틴 가능 
    {
        
    }*/


    //효과음 재생
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


    //모든 효과음 스탑
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

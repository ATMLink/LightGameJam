using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // public static MusicManager Instance; // 单例模式

    private Dictionary<string, AudioClip> soundClips;
    public AudioSource audioSource;

    // 在 Inspector 中设置各个音效
    public AudioClip buttonClickSound;
    public AudioClip explosionSound;
    public AudioClip backgroundMusic;

    public void Initialize()
    {
        // // 单例模式确保只有一个 MusicManager 实例
        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // 初始化音效字典
        soundClips = new Dictionary<string, AudioClip>
        {
            { "ButtonClick", buttonClickSound },
            { "Explosion", explosionSound },
            { "BackgroundMusic", backgroundMusic }
        };
    }

    // 播放音效
    public void PlaySound(string soundName)
    {
        if (soundClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundClips[soundName]);
        }
        else
        {
            Debug.LogWarning($"音效 {soundName} 不存在！");
        }
    }

}

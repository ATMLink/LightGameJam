using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGM : MonoBehaviour
{
    private Dictionary<string, AudioClip> soundClips;
    [SerializeField] private AudioSource bgmAudioSource;

    public AudioClip titleBGM;
    
    void Start()
    {
        Initialize();
        PlayBGM("Title");
    }
    public void Initialize()
    {
        // 检查 audioSource 是否已分配
        if (bgmAudioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        // 初始化音效字典
        soundClips = new Dictionary<string, AudioClip>
        {
            {"Title", titleBGM}
        };
    }
    public void PlayBGM(string bgmName)
    {
        if (bgmAudioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        if (soundClips.ContainsKey(bgmName))
        {
            // 停止当前BGM
            bgmAudioSource.Stop();

            // 更换BGM剪辑
            bgmAudioSource.clip = soundClips[bgmName];
        
            // 设置为循环播放
            bgmAudioSource.loop = true;

            // 播放新的BGM
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogError($"BGM '{bgmName}' not found in soundClips dictionary.");
        }
    }
    
}

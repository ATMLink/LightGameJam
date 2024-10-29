using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> soundClips;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource soundEffectAudioSource;

    public AudioClip towerClickSound;
    public AudioClip battleBGM;
    public AudioClip winBGM;
    public AudioClip buttonPassSound;
    public AudioClip towerAttackBeam;
    public AudioClip towerAttackShells;
    public AudioClip beginBuildTower;
    public AudioClip finishBuildTower;
    public AudioClip finishWonderBuild;
    public AudioClip towerDestroyed;
    public AudioClip destructTower;
    public AudioClip enemyNervousBGM;
    public AudioClip failedBGM;
    public AudioClip enemyWaveBegin;

    void Start()
    {
        Initialize();
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
            { "TowerClick", towerClickSound },
            { "WinBGM", winBGM },
            { "BattleBGM", battleBGM },
            { "ButtonPass", buttonPassSound },
            { "TowerAttackBeam", towerAttackBeam },
            { "TowerAttackShells", towerAttackShells },
            { "BeginBuildTower", beginBuildTower },
            { "FinishBuildTower", finishBuildTower },
            { "FinishWonderBuild", finishWonderBuild },
            { "TowerDestroyed", towerDestroyed },
            { "DestructTower", destructTower },
            { "EnemyNervousBGM", enemyNervousBGM },
            {"EnemyWaveBegin", enemyWaveBegin},
            {"FailedBGM", failedBGM}
        };
    }

    public void PlaySound(string soundName)
    {
        if (soundEffectAudioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        if (soundClips.ContainsKey(soundName))
        {
            soundEffectAudioSource.PlayOneShot(soundClips[soundName]);
        }
        else
        {
            //Debug.LogError($"Sound '{soundName}' not found in soundClips dictionary.");
        }
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

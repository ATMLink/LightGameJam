using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> soundClips;
    [SerializeField] private AudioSource audioSource;

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
    public AudioClip towerRotate;
    public AudioClip enemyWaveBegin;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // 检查 audioSource 是否已分配
        if (audioSource == null)
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
            { "TowerRotate", towerRotate },
            { "EnemyWaveBegin", enemyWaveBegin }
        };
    }

    public void PlaySound(string soundName)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        if (soundClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundClips[soundName]);
        }
        else
        {
            Debug.LogError($"Sound '{soundName}' not found in soundClips dictionary.");
        }
    }

    public void PlayBGM(string bgmName)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        if (soundClips.ContainsKey(bgmName))
        {
            audioSource.clip = soundClips[bgmName];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError($"BGM '{bgmName}' not found in soundClips dictionary.");
        }
    }
}

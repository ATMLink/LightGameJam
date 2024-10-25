using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // public static MusicManager Instance; // 单例模式

    private Dictionary<string, AudioClip> soundClips;
    public AudioSource audioSource;

    // 在 Inspector 中设置各个音效
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

    public void Initialize()
    {
        

        // 初始化音效字典
        soundClips = new Dictionary<string, AudioClip>
        {
            { "TowerClick", towerClickSound },
            { "WinBGM", winBGM },
            { "BattleBGM", battleBGM },
            {"ButtonPass", buttonPassSound},
            {"TowerAttackBeam", towerAttackBeam},
            {"TowerAttackShells", towerAttackShells},
            {"BeginBuildTower", beginBuildTower},
            {"FinishBuildTower", finishBuildTower},
            {"FinishWonderBuild", finishBuildTower},
            {"FinishWonderBuild", finishWonderBuild},
            {"TowerDestroyed", towerDestroyed},
            {"DestructTower", destructTower},
            {"TowerRotate", towerRotate},
            {"EnemyWaveBegin", enemyWaveBegin}
        };
    }

    // 播放音效
    public void PlaySound(string soundName)
    {
        if (soundClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundClips[soundName]);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDriver : MonoBehaviour
{
    // 各个系统引用
    [Header("Managers")] 
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private TilemapManager _tilemapManager;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private LaserManager _laserManager;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private MainResourceManagement resourceManagement;
    float time = 0;

    
    // 游戏状态变量
    [Header("Variables")]
    [SerializeField] private BoolVariable isPaused;
    [SerializeField] private FloatVariable wonderWinCount;
    private bool gameIsRunning = true;
    private float gameTime = 0f;
    private float gameSpeed = 1f;
    private float gameTwiceFastSpeed = 2f;
    private int currentTurn;
    private bool hasPlayedNervousMusic;
    
    // initialize
    private void Start()
    {
        StartGame();
    }
    
    // main loop
    private void Update()// 不依赖物理逻辑相关的更新
    {
        if (gameIsRunning && !isPaused.Value)
        {
            time += Time.deltaTime;   
            _inputManager.UpdateState();
            _cameraController.UpdateState();
            if (time >= 0.1f) { _laserManager.UpdateState();time = 0; }
            //_laserManager.UpdateState();
            _uiManager.UpdateState();
            resourceManagement.UpdateState();
            ChangeNervousMusic();
        }
    }

    private void FixedUpdate()// 依赖物理逻辑相关的更新
    {
        if (gameIsRunning)
        {
            UpdateGameTime();
            _towerManager.UpdateState();
            //_laserManager.UpdateState();
        }
    }

    private void StartGame()
    {
        gameTime = 0f;
        isPaused.SetValue(false);
        wonderWinCount.SetValue(0);
        _cameraController.Initialize();
        _tilemapManager.Initialize();
        _towerManager.Initialize();
        _uiManager.Initialize();
        _laserManager.Initialize();
        resourceManagement.Initialize();
        hasPlayedNervousMusic = false;
        _musicManager.PlayBGM("BattleBGM");
    }

    private void UpdateGameTime()
    {
        gameTime += Time.deltaTime * gameSpeed;
    }

    public void PauseGame()
    {
        if (isPaused.Value)
        {
            Time.timeScale = 0f;
            Debug.Log("game paused");
        }
    }

    public void ResumeGame()
    {
        if (!isPaused.Value)
        {
            Time.timeScale = gameSpeed;
            Debug.Log("Game resumed");
        }
    }

    public void TwiceFastGame()
    {
        //isPaused = false;
        Time.timeScale = gameTwiceFastSpeed;
        Debug.Log("Game twice as fast");
    }
    
    public void EndGame()
    {
        gameIsRunning = false;
        if(_musicManager!= null)
            _musicManager.PlayBGM("FailedBGM");
        _uiManager.ShowGameOverScreen();
        Debug.Log("game over");
    }

    public void WonderWin()
    {
        if (wonderWinCount.Value == 3)
        {
            gameIsRunning = false;
            if(_musicManager != null)
                _musicManager.PlayBGM("WinBGM");
            _uiManager.ShowWinPanel();
        }
    }
    public float GetGameTime()
    {
        return gameTime;
    }

    public void ChangeNervousMusic()
    {
        currentTurn = _enemyManager.GetCurrentTurn();
        if(currentTurn >= 16&& !hasPlayedNervousMusic)
        {
            Debug.Log("start nervous bgm");
            if (_musicManager != null)
                _musicManager.PlayBGM("EnemyNervousBGM");
            hasPlayedNervousMusic = true;
        }
    }
    public void SetTimeScale() {
        Time.timeScale = 1;
        gameIsRunning=true;
        _musicManager.PlayBGM("BattleBGM");
    }
}

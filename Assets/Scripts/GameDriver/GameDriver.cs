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
    
    // 游戏状态变量
    [SerializeField] private BoolVariable isPaused;
    private bool gameIsRunning = true;
    private float gameTime = 0f;
    private float gameSpeed = 1f;
    private float gameTwiceFastSpeed = 2f;
    
    // initialize
    private void Start()
    {
        StartGame();
    }
    
    // main loop
    private void Update()// 不依赖物理逻辑相关的更新
    {
        if (gameIsRunning && !isPaused)
        {
            _inputManager.UpdateState();
            _cameraController.UpdateState();
            _uiManager.UpdateState();
        }
    }

    private void FixedUpdate()// 依赖物理逻辑相关的更新
    {
        if (gameIsRunning)
        {
            UpdateGameTime();
            _towerManager.UpdateState();
        }
    }

    private void StartGame()
    {
        gameTime = 0f;
        _cameraController.Initialize();
        _tilemapManager.Initialize();
        _towerManager.Initialize();
        _uiManager.Initialize();
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
        if (!isPaused)
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
        _uiManager.ShowGameOverScreen();
        Debug.Log("game over");
    }

    public float GetGameTime()
    {
        return gameTime;
    }
}

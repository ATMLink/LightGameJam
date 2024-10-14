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
    [SerializeField] private ConstructManager _constructManager;
    [SerializeField] private TilemapManager _tilemapManager;
    
    // 游戏状态变量
    private bool gameIsRunning = true;
    
    // initialize
    private void Start()
    {
        StartGame();
    }
    
    // main loop
    private void Update()
    {
        if (gameIsRunning)
        {
            UpdateSystems();
        }
    }

    private void StartGame()
    {
        _cameraController.Initialize();
        _constructManager.Initialize();
        _tilemapManager.Initialize();
    }

    private void UpdateSystems()
    {
        _cameraController.UpdateState();
        // _inputManager.HandleInput();
    }

    public void EndGame()
    {
        gameIsRunning = false;
        // uiManager.ShowGameOverScreen();
        Debug.Log("game ended");
    }
}

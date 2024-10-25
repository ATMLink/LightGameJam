using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;


public class UiManager : MonoBehaviour
{


    // 属性
    [SerializeField] private Canvas uiCanvas; // 引用Canvas，通常用于设置渲染模式或层级  
    [SerializeField] private ConstructManager _constructManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private TowerManager _towerManager;
    [SerializeField] private MainResourceManagement _resourceManager;
    

    public void Initialize()//UI初始化
    {
        introduction.SetActive(false);
        gameover.SetActive(false);
        pauseGame.SetActive(false);
        showConstructionMenu.SetActive(false);
        showTowerMenu.gameObject.SetActive(false); 
        updateResources.SetActive(true);
        expandButton.gameObject.SetActive(false);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);//暂停按钮
        retractButton.onClick.AddListener(OnRetractButtonClicked);//收起资源面板按钮
        expandButton.onClick.AddListener(OnExpandButtonClicked);//展开资源面板按钮
        showConstructionMenuButton.onClick.AddListener(OnShowConstructionMenuButtonClicked);//建筑菜单按钮
        openIntroductionButton.onClick.AddListener(OnOpenIntroductionButtonClicked);
        leftPage.onClick.AddListener(OnLeftButtonClicked);
        rightPage.onClick.AddListener(OnRightButtonClicked);
        closeIntroduction.onClick.AddListener(OnCloseIntroduction);
        UpdateResources();

    }
    //****************************************************************
    //gameover相关属性
    [Header("gameover")]//游戏结束
    [SerializeField] private GameObject gameover; //整体
    [SerializeField] private Image gameoverImg;//gameover图片
    [SerializeField] private Button exitButton;//退出
    [SerializeField] private Button titleButton;//返回标题
    //gameover相关方法
    public void ShowGameOverScreen()//游戏结束时，调用该方法
    {
        gameover.SetActive(true);
        titleButton.onClick.AddListener(OnTitleButtonClick);
        exitButton.onClick.AddListener(OnExitButtomClick);
    }
    void OnExitButtomClick()//退出键
    {
        Application.Quit();
    }
    void OnTitleButtonClick()//返回标题键
    {
        SceneManager.LoadScene(2);//根据实际修改：返回标题
    }



    //****************************************************************
    //introduction相关属性
    [Header("introduction")]//新手教程
    [SerializeField] private GameObject introduction;//整体
    [SerializeField] private Image currentImage;// UI上的Image组件  
    [SerializeField] private Sprite[] images;// 所有要展示的图片数组  
    [SerializeField] private Button leftPage; // 左翻按钮  
    [SerializeField] private Button rightPage;// 右翻按钮
    [SerializeField] private Button closeIntroduction;//关闭按钮
    private int currentIndex = 0;
    [SerializeField] private Button openIntroductionButton;

    //introduction相关方法
    void ShowIntroduction()//需要出现新手教程时候调用该方法
    {
        introduction.SetActive(true);
        UpdateImage();
    }

    void OnOpenIntroductionButtonClicked()
    {
        currentIndex = 0;
        ShowIntroduction();
    }
    void OnLeftButtonClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateImage();
        }
    }
    void OnRightButtonClicked()
    {
        if (currentIndex < images.Length - 1)
        {
            currentIndex++;
            UpdateImage();
        }
    }
    void OnCloseIntroduction()
    {
        introduction.SetActive(false);
    }
    void UpdateImage()
    {
        // 更新UI上的图片显示  
        if (currentImage != null && images.Length > 0)
        {
            currentImage.sprite = images[currentIndex];
        }
    }


    //***************************************************************
    //pauseGame相关属性
    [Header("pauseGame")]//暂停游戏
    [SerializeField] private GameObject pauseGame;//整体
    [SerializeField] private Button continueButton; //继续按钮  
    [SerializeField] private Button titleButton2;//返回标题按钮
    [SerializeField] private Button exitButton2;//关闭按钮
    [SerializeField] private Button pauseButton;//暂停按钮
    [SerializeField] private BoolVariable isPaused;
    //pauseGame相关方法
    void PauseGame()
    {
        pauseGame.SetActive(true);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        titleButton2.onClick.AddListener(OnTitleButtonClick);
        exitButton2.onClick.AddListener(OnExitButtomClick);
    }
    void OnContinueButtonClicked()
    {
        isPaused.SetValue(false);
        pauseGame.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
    void OnPauseButtonClicked()
    {
        PauseGame();
        isPaused.SetValue(true);
        pauseButton.gameObject.SetActive(false);
    }




    //**************************************************************
    //ShowConstructionMenu相关属性
    [Header("showConstructionMenu")]//塔建造菜单
    [SerializeField] private GameObject showConstructionMenu;//整体
    [SerializeField] private Button closeConstructionMenu;
    [SerializeField] private Button tower1;
    [SerializeField] private Button tower2;
    [SerializeField] private Button tower3;
    [SerializeField] private Button tower4;
    [SerializeField] private Button tower5;
    [SerializeField] private Button tower6;
    [SerializeField] private Button tower7;
    [SerializeField] private Button tower8;
    [SerializeField] private Button tower9;
    [SerializeField] private Button tower10;
    [SerializeField] private Button showConstructionMenuButton;
    
    // tower attributes
    [SerializeField] private TowerAttributes tower1Attributes;
    //ShowConstructionMenu相关方法
    void ShowConstructionMenu()
    {
        showConstructionMenu.SetActive(true);
        closeConstructionMenu.onClick.AddListener(OnCloseConstructionMenuClicked);



    }
    void OnCloseConstructionMenuClicked()
    {
        showConstructionMenu.gameObject.SetActive(false);
        showConstructionMenuButton.gameObject.SetActive(true);
    }//关闭
    void OnShowConstructionMenuButtonClicked()
    {
        showConstructionMenuButton.gameObject.SetActive(false);
        ShowConstructionMenu();
    }


    //**************************************************************
    //ShowTowerMenu相关属性
    [Header("ShowTowerMenu")]//塔更改菜单
    [SerializeField] private RectTransform showTowerMenu;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Button closeTowerMenu;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button clockwiseButton;
    [SerializeField] private Button anticlockwiseButton;
    [SerializeField] private Button deleteButton;
    

    private Tower selectedTower;


    public void ShowTowerMenu(Tower tower)
    {
        selectedTower = tower;
        if (selectedTower != null && Camera.main != null)
        {
            // 获取游戏对象的世界坐标  
            Vector3 worldPosition = selectedTower.transform.position;

            // 将世界坐标转换为屏幕坐标  
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            Vector2 localPoint;
            
            // 计算本地坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null, out localPoint);

            showTowerMenu.localPosition = localPoint;
        }
        
        // 在添加之前移除现有监听器
        closeTowerMenu.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        clockwiseButton.onClick.RemoveAllListeners();
        anticlockwiseButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();

        // 然后添加新的监听器
        closeTowerMenu.onClick.AddListener(OnCloseTowerMenuClicked);
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        clockwiseButton.onClick.AddListener(OnClockwiseButtonClicked);
        anticlockwiseButton.onClick.AddListener(OnAnticlockwiseButtonClicked);
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        showTowerMenu.gameObject.SetActive(true);
    }
    void OnCloseTowerMenuClicked()
    {
        showTowerMenu.gameObject.SetActive(false);//关闭面板
        selectedTower = null;
    }
    void OnUpgradeButtonClicked()
    {
        if (selectedTower != null)
        {
            _towerManager.UpgradeTower(selectedTower);
            showTowerMenu.gameObject.SetActive(false);
            selectedTower = null;
        }
    }
    void OnClockwiseButtonClicked()
    {
        //顺时针旋转
        if (selectedTower != null)
        {
            _towerManager.RotateTower(selectedTower, false);
        }
            
            
    }
    void OnAnticlockwiseButtonClicked() 
    {
        //逆时针旋转的接口
        if (selectedTower != null)
        {
            _towerManager.RotateTower(selectedTower, true);
        }
            
    }
    void OnDeleteButtonClicked()
    {
        if (selectedTower != null)
        {
            _towerManager.RemoveTower(selectedTower);
            showTowerMenu.gameObject.SetActive(false);
            selectedTower = null;
        }
            
    }




    //**************************************************************
    //UpdateResources相关属性
    [Header("UpdateResources")]//资源信息更新
    [SerializeField] private GameObject updateResources; //整体
    [SerializeField] private TextMeshProUGUI[] resources;
    [SerializeField] private Button retractButton;//收起、
    [SerializeField] private Button expandButton;//展开
    [SerializeField] private TextMeshProUGUI waves;
    [SerializeField] private EnemyManager enemyManager;
    void OnRetractButtonClicked()//收起
    {
        updateResources.SetActive(false);
        expandButton.gameObject.SetActive(true);
    }
    void OnExpandButtonClicked()//展开
    {
        updateResources.SetActive(true);
        expandButton.gameObject.SetActive(false);
    }


    public void UpdateResources()//每帧更新
    {
        resources[0].text = "Si" + " " + _resourceManager.GetResourceNumber(element.si);
        resources[1].text = "K" + " " + _resourceManager.GetResourceNumber(element.k);
        resources[2].text = "Na" + " " + _resourceManager.GetResourceNumber(element.na);
        resources[3].text = "Cu" + " " + _resourceManager.GetResourceNumber(element.cu);
        resources[4].text = "Li" + " " + _resourceManager.GetResourceNumber(element.li);
        resources[5].text = "Cs" + " " + _resourceManager.GetResourceNumber(element.cs);

        //以上在实际应用中传递数值

        //波次显示
        //有点问题，暂时不做

        //waves.text = +"" +;
        

    }
    
}
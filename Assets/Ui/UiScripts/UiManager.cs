using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.ShaderGraph.Drawing;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;


public class UiManager : MonoBehaviour
{


    // ����
    [SerializeField] private Canvas uiCanvas; // ����Canvas��ͨ������������Ⱦģʽ��㼶  


    // Start is called before the first frame update
    void Start()//�����ã�����ɾ��
    {
        UiInitialization();
        //ShowGameOverScreen();//������
        ShowIntroduction();
        //ShowConstructionMenu();
        ShowTowerMenu();

    }

    // Update is called once per frame
    void Update()//����ɾ��
    {
        UpdateResources();
    }

    void UiInitialization()//UI��ʼ��
    {
        introduction.SetActive(false);
        gameover.SetActive(false);
        pauseGame.SetActive(false);
        showConstructionMenu.SetActive(false);
        showTowerMenu.SetActive(false); 
        updateResources.SetActive(true);
        expandButton.gameObject.SetActive(false);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);//��ͣ��ť
        retractButton.onClick.AddListener(OnRetractButtonClicked);//������Դ��尴ť
        expandButton.onClick.AddListener(OnExpandButtonClicked);//չ����Դ��尴ť
        showConstructionMenuButton.onClick.AddListener(OnShowConstructionMenuButtonClicked);//�����˵���ť


    }
    //****************************************************************
    //gameover�������
    [Header("gameover")]//��Ϸ����
    [SerializeField] private GameObject gameover; //����
    [SerializeField] private Image gameoverImg;//gameoverͼƬ
    [SerializeField] private Button exitButton;//�˳�
    [SerializeField] private Button titleButton;//���ر���
    //gameover��ط���
    void ShowGameOverScreen()//��Ϸ����ʱ�����ø÷���
    {
        gameover.SetActive(true);
        titleButton.onClick.AddListener(OnTitleButtonClick);
        exitButton.onClick.AddListener(OnExitButtomClick);
    }
    void OnExitButtomClick()//�˳���
    {
        Application.Quit();
    }
    void OnTitleButtonClick()//���ر����
    {
        SceneManager.LoadScene(2);//����ʵ���޸ģ����ر���
    }



    //****************************************************************
    //introduction�������
    [Header("introduction")]//���ֽ̳�
    [SerializeField] private GameObject introduction;//����
    [SerializeField] private Image currentImage;// UI�ϵ�Image���  
    [SerializeField] private Sprite[] images;// ����Ҫչʾ��ͼƬ����  
    [SerializeField] private Button leftPage; // �󷭰�ť  
    [SerializeField] private Button rightPage;// �ҷ���ť
    [SerializeField] private Button closeIntroduction;//�رհ�ť
    private int currentIndex = 0;

    //introduction��ط���
    void ShowIntroduction()//��Ҫ�������ֽ̳�ʱ����ø÷���
    {
        introduction.SetActive(true);
        leftPage.onClick.AddListener(OnLeftButtonClicked);
        rightPage.onClick.AddListener(OnRightButtonClicked);
        closeIntroduction.onClick.AddListener(OnCloseIntroduction);
        UpdateImage();
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
        // ����UI�ϵ�ͼƬ��ʾ  
        if (currentImage != null && images.Length > 0)
        {
            currentImage.sprite = images[currentIndex];
        }
    }


    //***************************************************************
    //pauseGame�������
    [Header("pauseGame")]//��ͣ��Ϸ
    [SerializeField] private GameObject pauseGame;//����
    [SerializeField] private Button continueButton; //������ť  
    [SerializeField] private Button titleButton2;//���ر��ⰴť
    [SerializeField] private Button exitButton2;//�رհ�ť
    [SerializeField] private Button pauseButton;//��ͣ��ť
    static public bool isPause;
    //pauseGame��ط���
    void PauseGame()
    {
        pauseGame.SetActive(true);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        titleButton2.onClick.AddListener(OnTitleButtonClick);
        exitButton2.onClick.AddListener(OnExitButtomClick);
        
    }
    void OnContinueButtonClicked()
    {
        isPause = false;
        pauseGame.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
    void OnPauseButtonClicked()
    {
        PauseGame();
        isPause = true;
        pauseButton.gameObject.SetActive(false);
    }




    //**************************************************************
    //ShowConstructionMenu�������
    [Header("showConstructionMenu")]//������˵�
    [SerializeField] private GameObject showConstructionMenu;//����
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
    //ShowConstructionMenu��ط���
    void ShowConstructionMenu()
    {
        showConstructionMenu.SetActive(true);
        tower1.onClick.AddListener(OnTower1Clicked);
        tower2.onClick.AddListener(OnTower2Clicked);
        tower3.onClick.AddListener(OnTower3Clicked);
        tower4.onClick.AddListener(OnTower4Clicked);
        tower5.onClick.AddListener(OnTower5Clicked);
        tower6.onClick.AddListener(OnTower6Clicked);
        tower7.onClick.AddListener(OnTower7Clicked);
        tower8.onClick.AddListener(OnTower8Clicked);
        tower9.onClick.AddListener(OnTower9Clicked);
        tower10.onClick.AddListener(OnTower10Clicked);
        closeConstructionMenu.onClick.AddListener(OnCloseConstructionMenuClicked);



    }
    void OnCloseConstructionMenuClicked()
    {
        showConstructionMenu.gameObject.SetActive(false);
        showConstructionMenuButton.gameObject.SetActive(true);
    }//�ر�
    void OnShowConstructionMenuButtonClicked()
    {
        showConstructionMenuButton.gameObject.SetActive(false);
        ShowConstructionMenu();
    }
    //Ԥ����10�����Ľӿ�
    void OnTower1Clicked()
    {

    }
    void OnTower2Clicked()
    {

    }
    void OnTower3Clicked()
    {

    }
    void OnTower4Clicked()
    {

    }
    void OnTower5Clicked()
    {

    }
    void OnTower6Clicked()
    {

    }
    void OnTower7Clicked()
    {

    }
    void OnTower8Clicked()
    {

    }
    void OnTower9Clicked()
    {

    }
    void OnTower10Clicked()
    {

    }  


    //**************************************************************
    //ShowTowerMenu�������
    [Header("ShowTowerMenu")]//�����Ĳ˵�
    [SerializeField] private GameObject showTowerMenu; //����
    [SerializeField] private RectTransform showTowerMenuPanel;//�˵����
    [SerializeField] private Button closeTowerMenu;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button clockwiseButton;
    [SerializeField] private Button anticlockwiseButton;
    [SerializeField] private Button deleteButton;

    [SerializeField] private Transform testobject;






    void ShowTowerMenu()
    {
        if (testobject != null && Camera.main != null)
        {
            // ��ȡ��Ϸ�������������  
            Vector3 worldPosition = testobject.position;

            // ����������ת��Ϊ��Ļ����  
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // ��Ļ�����z����ͨ������Ҫ������ĳЩ����¿�������  
            // ���磬��UIԪ�ص����������  
            float screenDepth = screenPosition.z;

            // ����UIԪ��ͨ��ֻ����x��y���꣬���ǿ��Դ���һ��Vector2  
            Vector2 screenPosition2D = new Vector2(screenPosition.x, screenPosition.y);


            // ���������ʹ��screenPosition2D������UI��λ����������  
            // ���磬��UIԪ���ƶ�����Ļ�ϵ����λ��  
            // ע�⣺ֱ������UIԪ�ص�ê��λ�ÿ�����Ҫ��һ����ת����  
            // ��ΪUIԪ������������ǵĸ�RectTransform��λ�ġ�  

            // ����ֻ�Ǵ�ӡ������Ϊʾ��  
        }
        else
        {
            Debug.LogWarning("Target object or main camera is missing.");
        }




        showTowerMenu.SetActive(true);
        closeTowerMenu.onClick.AddListener(OnCloseTowerMenuClicked);
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        clockwiseButton.onClick.AddListener(OnClockwiseButtonClicked);
        anticlockwiseButton.onClick.AddListener(OnAnticlockwiseButtonClicked);
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }
    void OnCloseTowerMenuClicked()
    {
        showTowerMenu.SetActive(false);//�ر����
    }
    void OnUpgradeButtonClicked()
    {
        //�����Ľӿ�
    }
    void OnClockwiseButtonClicked()
    {
        //˳ʱ����ת�Ľӿ�
    }
    void OnAnticlockwiseButtonClicked() 
    {
        //��ʱ����ת�Ľӿ�
    }
    void OnDeleteButtonClicked()
    {
        //ɾ�����Ľӿ�
    }




    //**************************************************************
    //UpdateResources�������
    [Header("UpdateResources")]//��Դ��Ϣ����
    [SerializeField] private GameObject updateResources; //����
    [SerializeField] private TextMeshProUGUI[] resources;
    [SerializeField] private Button retractButton;//����
    [SerializeField] private Button expandButton;//չ��
    void OnRetractButtonClicked()//����
    {
        updateResources.SetActive(false);
        expandButton.gameObject.SetActive(true);
    }
    void OnExpandButtonClicked()//չ��
    {
        updateResources.SetActive(true);
        expandButton.gameObject.SetActive(false);
    }


    //������
    int Sinum=10000;
    void UpdateResources()//ÿ֡����
    {
        resources[0].text = "Si" + " " + Sinum;
        //resources[1].text = "" + " " + ;
        //resources[1].text = "" + " " + ;
        //resources[1].text = "" + " " + ;
        //resources[1].text = "" + " " + ;
        //resources[1].text = "" + " " + ;

        //������ʵ��Ӧ���д�����ֵ

    }
    
}

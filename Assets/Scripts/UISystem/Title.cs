using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject titlePanel;
    // Start is called before the first frame update
    void Start()
    {
        titlePanel.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(OnStartButtonClick);
    }


    void OnStartButtonClick()
    {
        startButton.gameObject.SetActive(false);
        titlePanel.SetActive(true);
        enterButton.onClick.AddListener(OnEnterButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    void OnEnterButtonClick()
    {

    }
    void OnExitButtonClick()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

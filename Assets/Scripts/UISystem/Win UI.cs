using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
    {
    public void OnExitButtomClick()//退出键
        {
        Application.Quit();
        }
    public void OnTitleButtonClick()//返回标题键
        {
        SceneManager.LoadScene(0);//根据实际修改：返回标题
        }
    }

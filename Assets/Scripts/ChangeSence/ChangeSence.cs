using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSence : MonoBehaviour
{
    public void ChangeMain() { SceneManager.LoadScene(1);}
    public void Quit() { Application.Quit(); }
}

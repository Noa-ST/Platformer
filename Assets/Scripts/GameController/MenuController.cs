using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Play"); // Tên của scene Play, phải đúng với tên scene của bạn
    }

    public void Quit()
    {
        Application.Quit();
    }
}

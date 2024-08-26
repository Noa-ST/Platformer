using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public TextMeshProUGUI coinsCollectedText;
    private CoinManager coinManager;
    public List<Transform> startPoints; // Danh sách các điểm xuất phát
    public List<GameObject> levels; // Danh sách các level
    private int currentLevelIndex = 0; // Chỉ số của level hiện tại
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToNextLevel(GameObject player)
    {
        if (currentLevelIndex < levels.Count - 1)
        {
            levels[currentLevelIndex].SetActive(false); // Tắt level hiện tại
            currentLevelIndex++;
            levels[currentLevelIndex].SetActive(true); // Bật level tiếp theo
            MovePlayerToStartPoint(player);
        }
        else
        {
            Debug.Log("No more levels to transition to.");
        }
    }

    public void MovePlayerToStartPoint(GameObject player)
    {
        if (startPoints != null && startPoints.Count > currentLevelIndex)
        {
            player.transform.position = startPoints[currentLevelIndex].position;
        }
        else
        {
            Debug.LogWarning("Start point for the next level is not assigned!");
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void gameWin()
    {
        gameWinUI.SetActive(true);
    }

    public void Restart()
    {
        ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGame()
    {
        currentLevelIndex = 0;
        foreach (GameObject level in levels)
        {
            level.SetActive(false); // Tắt tất cả các level
        }
        levels[currentLevelIndex].SetActive(true); // Bật level đầu tiên
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}

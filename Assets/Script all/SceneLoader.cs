using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string LastSceneKey = "LastPlayedScene";
    public string gameOverScene = "GameOver"; // Đặt tên scene Game Over ở đây

    void Start()
    {
        // Nếu không phải scene Game Over thì lưu lại
        if (SceneManager.GetActiveScene().name != gameOverScene)
        {
            PlayerPrefs.SetString(LastSceneKey, SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("MainGame"); 
    }

    public void LoadLastPlayedScene()
    {
        if (PlayerPrefs.HasKey(LastSceneKey))
        {
            string lastScene = PlayerPrefs.GetString(LastSceneKey);
            SceneManager.LoadScene(lastScene); 
        }
        else
        {
            Debug.LogWarning("⚠ Không tìm thấy scene đã chơi trước đó!");
        }
    }
}

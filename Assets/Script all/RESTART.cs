using UnityEngine;
using UnityEngine.SceneManagement;

public class RESTART : MonoBehaviour
{
    private static int lastPlayedSceneIndex = 0; // Lưu scene vừa chơi

    // Gọi khi bắt đầu scene chơi để lưu lại
    public static void SaveCurrentScene()
    {
        lastPlayedSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Khi bấm nút "Chơi lại", quay lại scene vừa chơi
    public void RestartGame()
    {
        SceneManager.LoadScene(lastPlayedSceneIndex);
    }
}

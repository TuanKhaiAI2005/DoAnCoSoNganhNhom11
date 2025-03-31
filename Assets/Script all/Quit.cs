using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit(); // Thoát game (chỉ hoạt động khi build game)
        Debug.Log("Game đã thoát!"); // Debug để kiểm tra trên Editor
    }
}

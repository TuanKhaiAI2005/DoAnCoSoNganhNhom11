using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadMainScene : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("MainGame"); 
    }
}

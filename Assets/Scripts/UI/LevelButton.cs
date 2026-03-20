using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string sceneName; // Assign in Inspector

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
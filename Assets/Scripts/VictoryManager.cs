using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();
        }
        
        Debug.Log("VictoryManager: Loading MapScene1");
        SceneManager.LoadScene("MapScene1");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
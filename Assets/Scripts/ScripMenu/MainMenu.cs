using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string _gameSceneName = "Map_Cours1";
    
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    
    private void Start()
    {
        ShowMainMenu();
        Time.timeScale = 1f;
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }
    
    public void ShowOptions()
    {
        _mainMenuPanel.SetActive(false);
        _optionsPanel.SetActive(true);
        
        if (_creditsPanel != null)
        {
            _creditsPanel.SetActive(false);
        }
    }
    
    public void ShowCredits()
    {
        _mainMenuPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        
        if (_creditsPanel != null)
        {
            _creditsPanel.SetActive(true);
        }
    }
    
    public void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _optionsPanel.SetActive(false);
        
        if (_creditsPanel != null)
        {
            _creditsPanel.SetActive(false);
        }
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
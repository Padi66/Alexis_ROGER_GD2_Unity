using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTransitionBox : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    
    [Header("Level Settings")]
    [Tooltip("Nom exact de la scène à charger (ex: MapScene2)")]
    public string nextSceneName = "";
    
    [Header("Debug")]
    [Tooltip("Afficher les messages de debug dans la console")]
    public bool showDebugMessages = true;
    
    private void OnTriggerEnter(Collider other)
    {
        if (showDebugMessages)
        {
            Debug.Log($"LevelTransitionBox: Collision détectée avec {other.gameObject.name} (Tag: {other.tag})");
        }
        
        if (other.CompareTag(PLAYER_TAG))
        {
            if (showDebugMessages)
            {
                Debug.Log($"LevelTransitionBox: Joueur détecté! Chargement de la scène: {nextSceneName}");
            }
            LoadNextLevel();
        }
    }
    
    private void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("LevelTransitionBox: ERREUR - Aucun nom de scène spécifié! Remplissez le champ 'Next Scene Name' dans l'Inspector.");
            return;
        }
        
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        bool sceneExists = false;
        
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneName == nextSceneName)
            {
                sceneExists = true;
                break;
            }
        }
        
        if (!sceneExists)
        {
            Debug.LogError($"LevelTransitionBox: ERREUR - La scène '{nextSceneName}' n'existe pas ou n'est pas dans les Build Settings!");
            Debug.LogError("Ajoutez votre scène dans Edit > Build Settings ou utilisez le nom exact de la scène.");
            return;
        }
        
        if (showDebugMessages)
        {
            Debug.Log($"LevelTransitionBox: Chargement de la scène '{nextSceneName}'...");
        }
        
        SceneManager.LoadScene(nextSceneName);
    }
}

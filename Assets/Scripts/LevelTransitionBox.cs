using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionBox : MonoBehaviour
{
    // Constante pour éviter les erreurs de frappe et faciliter la maintenance
    // Si le tag change, on ne modifie qu'à un seul endroit
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
        // Validation : vérifie que le nom de scène n'est pas vide ou null
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("LevelTransitionBox: ERREUR - Aucun nom de scène spécifié! Remplissez le champ 'Next Scene Name' dans l'Inspector.");
            return;
        }
        
        // Récupère le nombre total de scènes dans les Build Settings
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        bool sceneExists = false;
        
        // Parcourt toutes les scènes dans les Build Settings pour vérifier l'existence
        for (int i = 0; i < sceneCount; i++)
        {
            // Récupère le chemin complet de la scène (ex: "Assets/Scenes/MapScene2.unity")
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            
            // Extrait uniquement le nom sans l'extension ni le chemin (ex: "MapScene2")
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            // Compare avec le nom recherché
            if (sceneName == nextSceneName)
            {
                sceneExists = true;
                break;
            }
        }
        
        // Sécurité : si la scène n'existe pas dans les Build Settings, affiche une erreur
        // Évite un crash et guide l'utilisateur vers la solution
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
        
        // Charge la nouvelle scène de manière synchrone
        SceneManager.LoadScene(nextSceneName);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "AccessCardData", menuName = "Scriptable Objects/AccessCardData")]
public class AccessCardData : ScriptableObject
{
    public string cardName;
    public Color cardColor = Color.white;
    public Sprite cardIcon; 
}

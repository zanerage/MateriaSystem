using UnityEngine;

[CreateAssetMenu(fileName = "New Materia", menuName = "MateriaSystem/Materia")]
public class Materia : ScriptableObject
{
    public string materiaName;
    public MateriaType materiaType; 
    public int level;
    public string description;
    public Sprite icon;
}

public enum MateriaType
{
    Magic,
    Support,
    Ability,
    StatBoost
}

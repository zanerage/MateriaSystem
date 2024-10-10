using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Weapon", menuName = "MateriaSystem/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public List<MateriaSlot> materiaSlots;
    
    public void EquipMateria(int slotIndex, Materia materia)
    {
        if (slotIndex < 0 || slotIndex >= materiaSlots.Count)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        // Equip the Materia if the slot is compatible
        if (materiaSlots[slotIndex].IsCompatibleMateria(materia))
        {
            materiaSlots[slotIndex].EquipMateria(materia);
        }
    }

    // Method to unequip Materia from a specific slot by index
    public void UnequipMateria(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= materiaSlots.Count)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        materiaSlots[slotIndex].UnequipMateria();
    }
}

[System.Serializable]
public class MateriaSlot
{
    public int slotID;
    public bool isLinked; 
    public Materia equippedMateria; 
    public MateriaType[] compatibleMateriaTypes; 

    // Check if the slot is empty
    public bool IsEmpty()
    {
        return equippedMateria == null;
    }

    // Equip a Materia if compatible
    public bool EquipMateria(Materia materia)
    {
        if (IsCompatibleMateria(materia))
        {
            equippedMateria = materia;
            return true;
        }
        return false;
    }

    
    public void UnequipMateria()
    {
        equippedMateria = null;
    }

    
    public bool IsCompatibleMateria(Materia materia)
    {
        return compatibleMateriaTypes != null && System.Array.Exists(compatibleMateriaTypes, type => type == materia.materiaType);
    }
}

using UnityEngine;

public class MateriaLinkingSystem : MonoBehaviour
{
    public void LinkMateria(MateriaSlot slotA, MateriaSlot slotB)
    {
        if (slotA.isLinked && slotB.isLinked && slotA.equippedMateria.materiaType == MateriaType.Magic 
            && slotB.equippedMateria.materiaType == MateriaType.Support)
        {
         
            Debug.Log($"{slotB.equippedMateria.materiaName} enhances {slotA.equippedMateria.materiaName}");
         
        }
    }
}

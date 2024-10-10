using UnityEngine;
using UnityEngine.UIElements;

public class MateriaSlotHandler : MonoBehaviour
{
   public MateriaSlot slotData;
   public VisualElement slotElement;

   public void AssignMateria(Materia materia)
   {
      if (slotData.IsCompatibleMateria(materia))
      {
         slotData.EquipMateria(materia);
         UpdateSlotUI(materia);
      }
   }

   private void UpdateSlotUI(Materia materia)
   {
      if (slotElement != null)
      {
         slotElement.Clear();
         var label = new Label(materia.materiaName);
         slotElement.Add(label);
      }
   }
}

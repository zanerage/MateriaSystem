using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentUIHandler : MonoBehaviour
{
    public Weapon Weapon;
    private VisualElement root;
    private List<VisualElement> weaponSlots;

    private void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        weaponSlots = root.Query<VisualElement>(className: "materia-slot").ToList();
        InitializeEquipmentSlots();
    }

    private void InitializeEquipmentSlots()
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            var slotElement = weaponSlots[i];

            if (i < Weapon.materiaSlots.Count)
            {
                var slotData = Weapon.materiaSlots[i];
                slotElement.userData = slotData;

                if (slotData.equippedMateria != null)
                {
                    var label = new Label(slotData.equippedMateria.materiaName);
                    slotElement.Add(label);
                }
                else
                {
                    var label = new Label("Empty Slot") { style = { opacity = 0.5f } };
                    slotElement.Add(label);
                }
            }
        }
    }


    public bool TryEquipMateriaToSlot(Materia materia, VisualElement slot)
    {
        if (slot.userData is MateriaSlot slotData && slotData.IsCompatibleMateria(materia))
        {
            slotData.EquipMateria(materia);

            
            slot.Clear();
            slot.Add(new Label(materia.materiaName));

            // Update the Weapon ScriptableObject
            int slotIndex = Weapon.materiaSlots.IndexOf(slotData);
            if (slotIndex >= 0)
            {
                Weapon.materiaSlots[slotIndex].equippedMateria = materia;
            }

            return true;
        }
        return false;
    }

    public List<VisualElement> GetAllSlots()
    {
        List<VisualElement> allSlots = new List<VisualElement>();
        allSlots.AddRange(weaponSlots);
      //  allSlots.AddRange(ArmorSlots);
        return allSlots;
    }
    public void UpdateWeaponSlot(VisualElement slot, Materia materia)
    {
        
        int slotIndex = weaponSlots.IndexOf(slot);
        if (slotIndex >= 0 && slotIndex < Weapon.materiaSlots.Count)
        {
            Weapon.EquipMateria(slotIndex, materia);
        }
    }
}
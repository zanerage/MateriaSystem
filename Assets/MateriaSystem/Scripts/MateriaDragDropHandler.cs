using UnityEngine;
using UnityEngine.UIElements;

public class MateriaDragDropHandler : MonoBehaviour
{
  private VisualElement root;
    private VisualElement draggedElement;
    private Materia draggedMateria;
    private VisualElement originalSlot;

    private void OnEnable()
    {
        UIDocument uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found in the scene.");
            return;
        }

        root = uiDocument.rootVisualElement;

        // Register callbacks for inventory slots to initiate drag
        var inventorySlots = root.Query<VisualElement>(className: "inventory-slot").ToList();
        foreach (var slot in inventorySlots)
        {
            slot.RegisterCallback<PointerDownEvent>(evt => StartDrag(evt, slot));
        }
    }

    private void StartDrag(PointerDownEvent evt, VisualElement slot)
    {
        originalSlot = slot;

        // Get Materia from the slot's userData
        if (slot.userData is MateriaSlot materiaSlot && materiaSlot.equippedMateria != null)
        {
            draggedMateria = materiaSlot.equippedMateria; // Assign the dragged Materia
        }
        else
        {
         //   Debug.LogError("No Materia equipped in the selected slot.");
            return;
        }

     
        draggedElement = new VisualElement();
        draggedElement.style.width = slot.resolvedStyle.width;
        draggedElement.style.height = slot.resolvedStyle.height;
        draggedElement.style.position = Position.Absolute;
        draggedElement.style.left = evt.position.x - slot.resolvedStyle.width / 2;
        draggedElement.style.top = evt.position.y - slot.resolvedStyle.height / 2;

        // Add an Image to represent the Materia icon
        var icon = new Image();
        icon.sprite = draggedMateria.icon; // Set the icon sprite
        icon.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        icon.style.height = new StyleLength(new Length(100, LengthUnit.Percent));

        draggedElement.Add(icon);

        // Add the dragged element to the root element
        root.Add(draggedElement);
        draggedElement.BringToFront(); // Make sure it's visible above other elements

        // Register callbacks to handle dragging and dropping
        root.RegisterCallback<PointerMoveEvent>(Drag);
        root.RegisterCallback<PointerUpEvent>(EndDrag);
    }
    
    private void Drag(PointerMoveEvent evt)
    {
        if (draggedElement != null)
        {
            draggedElement.style.left = evt.position.x - draggedElement.resolvedStyle.width / 2;
            draggedElement.style.top = evt.position.y - draggedElement.resolvedStyle.height / 2;
        }
    }

    private void OnDrag(PointerMoveEvent evt)
    {
        if (draggedElement != null)
        {
            draggedElement.style.left = evt.position.x - draggedElement.resolvedStyle.width / 2;
            draggedElement.style.top = evt.position.y - draggedElement.resolvedStyle.height / 2;
        }
    }

    private void EndDrag(PointerUpEvent evt)
    {
        if (draggedElement == null) return;

        Vector2 mousePosition = evt.position;
        VisualElement dropTarget = root.panel.Pick(mousePosition);

        if (dropTarget != null && dropTarget.ClassListContains("materia-slot"))
        {
            Debug.Log($"Dropped on slot: {dropTarget.name}");

            if (dropTarget.userData is MateriaSlot targetSlot)
            {
                if (targetSlot.IsEmpty() && targetSlot.IsCompatibleMateria(draggedMateria))
                {
                   

                  
                    targetSlot.EquipMateria(draggedMateria);

                    
                    UpdateSlotUI(dropTarget, draggedMateria);
                    UpdateWeaponData(targetSlot);

                    Debug.Log($"Weapon data updated with Materia: {draggedMateria.materiaName}");
                }
                else
                {
                    Debug.LogWarning("The slot is either already occupied or incompatible with the dragged Materia.");
                }
            }
            else
            {
                Debug.LogWarning("Drop target does not contain valid MateriaSlot data.");
            }
        }
        else
        {
            Debug.LogWarning("No valid drop target found or incompatible slot class.");
        }

        draggedElement.RemoveFromHierarchy();
        draggedElement = null;
        draggedMateria = null;

        root.UnregisterCallback<PointerMoveEvent>(OnDrag);
        root.UnregisterCallback<PointerUpEvent>(EndDrag);
    }

    private void UpdateSlotUI(VisualElement slot, Materia materia)
    {
        slot.Clear();
        var label = new Label(materia.materiaName);
        slot.Add(label);
    }

    private void UpdateWeaponData(MateriaSlot slotData)
    {
        EquipmentUIHandler equipmentHandler = FindObjectOfType<EquipmentUIHandler>();
        if (equipmentHandler != null && equipmentHandler.Weapon != null)
        {
            int slotIndex = equipmentHandler.Weapon.materiaSlots.IndexOf(slotData);
            if (slotIndex >= 0)
            {
                equipmentHandler.Weapon.materiaSlots[slotIndex] = slotData;
                Debug.Log($"Weapon slot updated at index: {slotIndex} with Materia: {slotData.equippedMateria.materiaName}");
            }
            else
            {
                Debug.LogWarning("Failed to find the appropriate weapon slot for the Materia.");
            }
        }
        else
        {
            Debug.LogWarning("EquipmentHandler or Weapon is null.");
        }
    }
}

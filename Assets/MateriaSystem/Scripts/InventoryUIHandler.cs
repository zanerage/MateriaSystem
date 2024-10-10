using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIHandler : MonoBehaviour
{
 public List<Materia> availableMateria;
    private VisualElement root;
    private List<VisualElement> inventorySlots = new List<VisualElement>();

    private VisualElement draggedElement;
    private Materia draggedMateria;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        inventorySlots = root.Query<VisualElement>(className: "inventory-slot").ToList();
        InitializeInventorySlots();
    }

    private void InitializeInventorySlots()
    {
        for (int i = 0; i < inventorySlots.Count && i < availableMateria.Count; i++)
        {
            var slotElement = inventorySlots[i];
            Materia materia = availableMateria[i];

        
            var materiaLabel = new Label(materia.materiaName);
            slotElement.Add(materiaLabel);

           
            slotElement.userData = materia;

          
            slotElement.RegisterCallback<PointerDownEvent>(evt => StartDrag(evt, slotElement));
        }
    }

    private void StartDrag(PointerDownEvent evt, VisualElement slot)
    {
        draggedMateria = slot.userData as Materia;
        if (draggedMateria == null) return;

        draggedElement = new VisualElement
        {
            style =
            {
                width = slot.resolvedStyle.width,
                height = slot.resolvedStyle.height,
                position = Position.Absolute,
                left = evt.position.x - slot.resolvedStyle.width / 2,
                top = evt.position.y - slot.resolvedStyle.height / 2,
                backgroundColor = new StyleColor(Color.cyan)
            }
        };

        var label = new Label(draggedMateria.materiaName);
        draggedElement.Add(label);
        root.Add(draggedElement);

        draggedElement.CapturePointer(evt.pointerId);
        draggedElement.RegisterCallback<PointerMoveEvent>(OnDrag);
        draggedElement.RegisterCallback<PointerUpEvent>(EndDrag);
    }

    private void OnDrag(PointerMoveEvent evt)
    {
        if (draggedElement == null) return;

        draggedElement.style.left = evt.position.x - draggedElement.resolvedStyle.width / 2;
        draggedElement.style.top = evt.position.y - draggedElement.resolvedStyle.height / 2;
    }

    private void EndDrag(PointerUpEvent evt)
    {
        if (draggedElement == null) return;

        EquipmentUIHandler equipmentUI = FindObjectOfType<EquipmentUIHandler>();
        if (equipmentUI != null)
        {
            foreach (var slot in equipmentUI.GetAllSlots())
            {
                if (IsPointerOverSlot(evt, slot))
                {
                    bool equipped = equipmentUI.TryEquipMateriaToSlot(draggedMateria, slot);
                    if (equipped)
                    {
                        break;
                    }
                }
            }
        }

        draggedElement.ReleasePointer(evt.pointerId);
        root.Remove(draggedElement);
        draggedElement = null;
        draggedMateria = null;
    }

    private bool IsPointerOverSlot(PointerUpEvent evt, VisualElement slot)
    {
        return slot.worldBound.Contains(evt.position);
    }
    }

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentUI : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/EquipmentUI")]
    public static void ShowExample()
    {
        EquipmentUI wnd = GetWindow<EquipmentUI>();
        wnd.titleContent = new GUIContent("EquipmentUI");
    }

    public void CreateGUI()
    {
     
        VisualElement root = rootVisualElement;

     
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

     
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}

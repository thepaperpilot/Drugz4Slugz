using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Procedure))]
public class ProcedureEditor : Editor {

    private GenericMenu drugsMenu;
    private ReorderableList firstList;
    private ReorderableList secondList;
    private ReorderableList thirdList;
    private ReorderableList fourthList;
    
    private string prop;
    private int index;

    private void OnEnable() {
        CreateDrugsMenu();

        // Create our various lists
        firstList = CreateConditionLists("firstSlug");
        secondList = CreateConditionLists("secondSlug");
        thirdList = CreateConditionLists("thirdSlug");
        fourthList = CreateConditionLists("fourthSlug");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        // Draw description
        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));

        // Create our info box
        EditorGUILayout.HelpBox("Note: Slug order does not matter", MessageType.Info);

        // Draw our slugs
        RenderSlug("firstSlug", firstList);
        RenderSlug("secondSlug", secondList);
        RenderSlug("thirdSlug", thirdList);
        RenderSlug("fourthSlug", fourthList);

        serializedObject.ApplyModifiedProperties();
    }

    void RenderSlug(string prop, ReorderableList list) {
        SerializedProperty property = serializedObject.FindProperty(prop);
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName + "[" + property.arraySize + "]", true);

        if (property.isExpanded)
            list.DoLayoutList();
    }

    void CreateDrugsMenu() {
        // Create our triggers menu
        drugsMenu = new GenericMenu();
        foreach (string drug in DrugManager.GetDrugs())
            drugsMenu.AddItem(new GUIContent(drug), false, clickHandler, drug);
    }

    ReorderableList CreateConditionLists(string prop) {
        // Create our list of conditions
        ReorderableList conditionList = new ReorderableList(serializedObject,
                serializedObject.FindProperty(prop),
                true, false, true, true);
        conditionList.headerHeight = 0;

        // Draw each comment
        conditionList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = conditionList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 40, EditorGUIUtility.singleLineHeight), "Drug");
            if (EditorGUI.DropdownButton(new Rect(rect.x + 40, rect.y, rect.width / 2f - 42, EditorGUIUtility.singleLineHeight), 
                new GUIContent(element.FindPropertyRelative("drug").stringValue), FocusType.Keyboard)) {
                this.prop = prop;
                this.index = index;
                drugsMenu.DropDown(new Rect(rect.x + 40, rect.y, rect.width / 2f - 42, EditorGUIUtility.singleLineHeight));
            }
            EditorGUI.LabelField(new Rect(rect.x + rect.width / 2f + 2, rect.y, 50, EditorGUIUtility.singleLineHeight), "Dosage");
            SerializedProperty dosage = element.FindPropertyRelative("dosage");
            dosage.intValue = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 52, rect.y, rect.width / 2f - 52, EditorGUIUtility.singleLineHeight), dosage.intValue);
        };

        return conditionList;
    }

    void clickHandler(object target) {
        serializedObject.FindProperty(prop).GetArrayElementAtIndex(index).FindPropertyRelative("drug").stringValue = (string)target;
        serializedObject.ApplyModifiedProperties();
    }
}
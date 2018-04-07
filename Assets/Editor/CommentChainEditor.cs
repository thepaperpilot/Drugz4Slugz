using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(CommentChain))]
public class CommentChainEditor : Editor {

    private GenericMenu triggers;
    private ReorderableList commentList;
    private string trigger;
    private Rect menuRect;

    private Dictionary<string, Trigger> triggersDict;

    private void OnEnable() {
        CreateTriggersMenu();
        CreateCommentList();
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        // Draw our trigger dropdown
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Trigger");
        if (EditorGUILayout.DropdownButton(new GUIContent(trigger), FocusType.Keyboard)) {
            triggers.DropDown(menuRect);
        }
        if (Event.current.type == EventType.Repaint) {
            menuRect = GUILayoutUtility.GetLastRect();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("excitementDelta"));

        EditorGUILayout.Space();

        // Draw our comment list
        commentList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void CreateTriggersMenu() {
        // Create our triggers menu
        triggersDict = new Dictionary<string, Trigger>();
        triggers = new GenericMenu();
        AddTrigger("test/test", ScriptableObject.CreateInstance<Trigger>());
    }

    void AddTrigger(string path, Trigger trigger) {
        if (this.trigger == null && serializedObject.FindProperty("trigger").objectReferenceValue == trigger)
            this.trigger = path;
        triggers.AddItem(new GUIContent(path), false, clickHandler, trigger);
        triggersDict.Add(path, trigger);
    }

    void CreateCommentList() {
        // Create our list of dice
        commentList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("comments"),
                true, true, true, true);

        // Set the list's header
        commentList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Comments");
        };

        // Draw each comment
        commentList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = commentList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty commenterNumber = element.FindPropertyRelative("commenterNumber");
            SerializedProperty delay = element.FindPropertyRelative("delay");
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight), "Viewer #");
            commenterNumber.intValue = EditorGUI.IntField(new Rect(rect.x + 70, rect.y, rect.width / 2f - 72, EditorGUIUtility.singleLineHeight), commenterNumber.intValue);
            EditorGUI.LabelField(new Rect(rect.x + rect.width / 2f + 2, rect.y, 50, EditorGUIUtility.singleLineHeight), "Delay");
            delay.floatValue = EditorGUI.FloatField(new Rect(rect.x + rect.width / 2f + 52, rect.y, rect.width / 2f - 52, EditorGUIUtility.singleLineHeight), delay.floatValue);

            rect.y += EditorGUIUtility.singleLineHeight + 2;

            SerializedProperty comment = element.FindPropertyRelative("comment");
            comment.stringValue = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width, 2 * EditorGUIUtility.singleLineHeight), comment.stringValue);
        };

        commentList.elementHeight = EditorGUIUtility.singleLineHeight * 3 + 6;
    }

    void clickHandler(object target) {
        trigger = triggersDict.Where(pair => pair.Value == (Object)target).First().Key;
        serializedObject.FindProperty("trigger").objectReferenceValue = (Object)target;
        serializedObject.ApplyModifiedProperties();
    }
}

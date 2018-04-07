﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
[CanEditMultipleObjects]
public class CustomEditorBase : Editor {
    private Dictionary<string, ReorderableListProperty> reorderableLists;

    protected virtual void OnEnable() {
        this.reorderableLists = new Dictionary<string, ReorderableListProperty>(10);
    }

    ~CustomEditorBase() {
        this.reorderableLists.Clear();
        this.reorderableLists = null;
    }

    public override void OnInspectorGUI() {
        //EditorGUILayout.LabelField("Custom Editor", EditorStyles.centeredGreyMiniLabel);
        Color cachedGuiColor = GUI.color;
        serializedObject.Update();
        var property = serializedObject.GetIterator();
        var next = property.NextVisible(true);
        if (next)
            do {
                GUI.color = cachedGuiColor;
                this.HandleProperty(property);
            } while (property.NextVisible(false));
        serializedObject.ApplyModifiedProperties();
    }

    protected void HandleProperty(SerializedProperty property) {
        //Debug.LogFormat("name: {0}, displayName: {1}, type: {2}, propertyType: {3}, path: {4}", property.name, property.displayName, property.type, property.propertyType, property.propertyPath);
        bool isdefaultScriptProperty = property.name.Equals("m_Script") && property.type.Equals("PPtr<MonoScript>") && property.propertyType == SerializedPropertyType.ObjectReference && property.propertyPath.Equals("m_Script");
        bool cachedGUIEnabled = GUI.enabled;
        if (isdefaultScriptProperty)
            GUI.enabled = false;
        //var attr = this.GetPropertyAttributes(property);
        if (property.isArray && property.propertyType != SerializedPropertyType.String)
            this.HandleArray(property);
        else
            EditorGUILayout.PropertyField(property, property.isExpanded);
        if (isdefaultScriptProperty)
            GUI.enabled = cachedGUIEnabled;
    }

    protected void HandleArray(SerializedProperty property) {
        //this.GetReorderableList(property).List.DoLayoutList();
        var listData = this.GetReorderableList(property);
        EditorGUILayout.BeginHorizontal();
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName + "[" + property.arraySize + "]", true);
        EditorGUILayout.EndHorizontal();

        if (property.isExpanded)
            listData.List.DoLayoutList();
    }

    protected object[] GetPropertyAttributes(SerializedProperty property) {
        return this.GetPropertyAttributes<PropertyAttribute>(property);
    }

    protected object[] GetPropertyAttributes<T>(SerializedProperty property) where T : System.Attribute {
        System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.GetField
            | System.Reflection.BindingFlags.GetProperty
            | System.Reflection.BindingFlags.IgnoreCase
            | System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic
            | System.Reflection.BindingFlags.Public;
        if (property.serializedObject.targetObject == null)
            return null;
        var targetType = property.serializedObject.targetObject.GetType();
        var field = targetType.GetField(property.name, bindingFlags);
        if (field != null)
            return field.GetCustomAttributes(typeof(T), true);
        return null;
    }

    private ReorderableListProperty GetReorderableList(SerializedProperty property) {
        ReorderableListProperty ret = null;
        if (this.reorderableLists.TryGetValue(property.name, out ret)) {
            ret.Property = property;
            return ret;
        }
        ret = new ReorderableListProperty(property);
        this.reorderableLists.Add(property.name, ret);
        return ret;
    }

    #region Inner-class ReorderableListProperty
    private class ReorderableListProperty {

        /// <summary>
        /// ref http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
        /// </summary>
        public ReorderableList List { get; private set; }

        private SerializedProperty _property;
        public SerializedProperty Property {
            get { return this._property; }
            set {
                this._property = value;
                this.List.serializedProperty = this._property;
            }
        }

        public ReorderableListProperty(SerializedProperty property) {
            this._property = property;
            this.CreateList();
        }

        ~ReorderableListProperty() {
            this._property = null;
            this.List = null;
        }

        private void CreateList() {
            bool dragable = true, header = false, add = true, remove = true;
            this.List = new ReorderableList(this.Property.serializedObject, this.Property, dragable, header, add, remove);
            this.List.headerHeight = 0;
            this.List.onCanRemoveCallback += (list) => { return this.List.count > 0; };
            this.List.drawElementCallback += this.drawElement;
            this.List.elementHeightCallback += (idx) => { return Mathf.Max(EditorGUIUtility.singleLineHeight, EditorGUI.GetPropertyHeight(this._property.GetArrayElementAtIndex(idx), GUIContent.none, true)) + 4.0f; };
        }

        private void drawElement(Rect rect, int index, bool active, bool focused) {
            if (this._property.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Generic) {
                rect.x += 12;
                rect.width -= 12;
                EditorGUI.LabelField(rect, this._property.GetArrayElementAtIndex(index).displayName);
            }
            //rect.height = 16;
            rect.height = EditorGUI.GetPropertyHeight(this._property.GetArrayElementAtIndex(index), GUIContent.none, true);
            rect.y += 1;
            EditorGUI.PropertyField(rect, this._property.GetArrayElementAtIndex(index), GUIContent.none, true);
            this.List.elementHeight = rect.height + 4.0f;
        }
    }
    #endregion
}

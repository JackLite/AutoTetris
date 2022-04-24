using System;
using UnityEditor;
using UnityEngine;

namespace Global.Settings.Localization.Editor
{
    [CustomPropertyDrawer(typeof(LocalizationSettings))]
    public class LocalizationSettingsEditor : PropertyDrawer
    {
        private static bool fold;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            var baseHeight = GetBaseHeight(property, label);
            DrawFoldout(rect, label, baseHeight);
            if (!fold)
            {
                EditorGUI.EndProperty();
                return;
            }
            var rawRect = DrawRawLocalization(property, rect, baseHeight);
            var prevRect = DrawArrayProperty(rawRect, property.FindPropertyRelative("langKeyToEnumMap"));
            var buttonRect = DrawParseButton(rect, property, prevRect, baseHeight);
            DrawArrayProperty(buttonRect, property.FindPropertyRelative("localizations"));
            EditorGUI.EndProperty();
        }

        private static void DrawFoldout(Rect rect, GUIContent label, float baseHeight)
        {
            var foldRect = new Rect(rect.position.x, rect.position.y, rect.width, baseHeight);
            fold = EditorGUI.Foldout(foldRect, fold, label);
        }

        private static Rect DrawParseButton(Rect rect, SerializedProperty property, Rect prevRect, float baseHeight)
        {
            var buttonRect = new Rect(prevRect.position.x,
                prevRect.position.y + prevRect.height,
                rect.width,
                baseHeight);
            if (GUI.Button(buttonRect, "Parse"))
                LocalizationSettingEditorParser.Parse(property);
            return buttonRect;
        }

        private Rect DrawRawLocalization(SerializedProperty baseProperty, Rect rect, float baseHeight)
        {
            var rawProperty = baseProperty.FindPropertyRelative("rawLocalizationJson");
            var rawRect = new Rect(rect.position.x, rect.position.y + baseHeight, rect.width, baseHeight);
            EditorGUI.ObjectField(rawRect, rawProperty);
            return rawRect;
        }

        private Rect DrawArrayProperty(Rect prevRect, SerializedProperty property)
        {
            var h = GetExpandablePropertyHeight(property);
            var newRect = new Rect(prevRect.position.x, prevRect.position.y + prevRect.height, prevRect.width, h);
            EditorGUI.PropertyField(newRect, property, new GUIContent(property.displayName), true);
            return newRect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!fold)
                return base.GetPropertyHeight(property, label);
            var height = 60f;
            var propertyHeight = GetExpandablePropertyHeight(property.FindPropertyRelative("langKeyToEnumMap"));
            height += propertyHeight;
            var expandablePropertyHeight = GetExpandablePropertyHeight(property.FindPropertyRelative("localizations"));
            height += expandablePropertyHeight;
            return height;
        }

        private static float GetExpandablePropertyHeight(SerializedProperty property)
        {
            var size = property.arraySize;
            var height = property.isExpanded ? 60 + Math.Max(1, size) * 20 : 20;
            for (var i = 0; i < size; ++i)
                height += property.GetArrayElementAtIndex(i).isExpanded ? 40 : 0;
            return height;
        }

        private float GetBaseHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
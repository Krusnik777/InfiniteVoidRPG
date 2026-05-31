using UnityEditor;
using UnityEngine;

namespace UI.Buttons
{
    [CustomPropertyDrawer(typeof(SelectableTransition))]
    public class SelectableTransitionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty typeProperty = property.FindPropertyRelative("Type");

            SerializedProperty indicatorProperty = property.FindPropertyRelative("SelectedIndicator");

            SerializedProperty targetGraphicProperty = property.FindPropertyRelative("TargetGraphic");
            SerializedProperty selectedColorProperty = property.FindPropertyRelative("SelectedColor");
            SerializedProperty unselectedColorProperty = property.FindPropertyRelative("UnselectedColor");

            SerializedProperty animationScaleProperty = property.FindPropertyRelative("TargetScale");
            SerializedProperty animationTimeProperty = property.FindPropertyRelative("AnimationTime");

            float y = position.y;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect typeRect = new Rect(position.x, y, position.width, lineHeight);
            EditorGUI.PropertyField(typeRect, typeProperty);
            y += lineHeight + spacing;

            var selectType = (SelectableTransition.SelectType)typeProperty.enumValueIndex;

            switch (selectType)
            {
                case SelectableTransition.SelectType.Indicator:
                    Rect indicatorRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(indicatorRect, indicatorProperty);
                    y += lineHeight + spacing;
                    break;

                case SelectableTransition.SelectType.Color:
                    Rect targetRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(targetRect, targetGraphicProperty);
                    y += lineHeight + spacing;

                    Rect selectedColorRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(selectedColorRect, selectedColorProperty);
                    y += lineHeight + spacing;

                    Rect unselectedColorRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(unselectedColorRect, unselectedColorProperty);
                    y += lineHeight + spacing;
                    break;

                case SelectableTransition.SelectType.Animation:
                    Rect scaleRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(scaleRect, animationScaleProperty);
                    y += lineHeight + spacing;

                    Rect timeRect = new Rect(position.x, y, position.width, lineHeight);
                    EditorGUI.PropertyField(timeRect, animationTimeProperty);
                    y += lineHeight + spacing;
                    break;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProperty = property.FindPropertyRelative("Type");

            var selectType = (SelectableTransition.SelectType)typeProperty.enumValueIndex;

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            switch (selectType)
            {
                case SelectableTransition.SelectType.Indicator:
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    break;
                case SelectableTransition.SelectType.Color:
                    height += 3 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                    break;
                case SelectableTransition.SelectType.Animation:
                    height += 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                    break;
            }

            return height;
        }
    }
}

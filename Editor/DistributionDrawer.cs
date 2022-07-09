using UnityEditor;
using UnityEngine;


namespace RandomToolbox
{/*
    [CustomPropertyDrawer(typeof(Distribution), true)]
    public class DistributionDrawer : PropertyDrawer
    {
        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return base.CanCacheInspectorGUI(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight + 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty curve = property.FindPropertyRelative(nameof(Distribution.function));
            SerializedProperty area = property.FindPropertyRelative(nameof(Distribution.area));
            SerializedProperty xmin = property.FindPropertyRelative(nameof(Distribution.xmin));
            SerializedProperty xmax = property.FindPropertyRelative(nameof(Distribution.xmax));


            Rect curveRect = new Rect(position);
            curveRect.yMin = curveRect.yMax - EditorGUIUtility.singleLineHeight;

            Color curveColor = area.floatValue > 0.999 && area.floatValue < 1.001 ? Color.green : Color.red;

            Rect curveRanges = new Rect(xmin.floatValue, 0, (xmax.floatValue - xmin.floatValue), 1);

            EditorGUI.CurveField(curveRect, curve, curveColor, curveRanges);

            Rect multiRect = new Rect(position);
            multiRect.height -= EditorGUIUtility.singleLineHeight + 2;
            GUIContent[] contents = new GUIContent[3]
            {
                new GUIContent("min"),
                new GUIContent("max"),
                new GUIContent("area")
            };
            EditorGUI.MultiPropertyField(multiRect, contents, xmin);

        }
    }*/
}

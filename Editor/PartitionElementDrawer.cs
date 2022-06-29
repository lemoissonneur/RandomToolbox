using UnityEngine;
using UnityEditor;


namespace RandomToolbox
{
    /// <summary>
    /// drawer for elements in the list of a partition
    /// </summary>
    [CustomPropertyDrawer(typeof(PartitionBase.ElementBase), true)]
    public class PartitionElementDrawer : PropertyDrawer
    {
        /// <summary>
        /// Process Rect to draw index number of the element
        /// </summary>
        private Rect IndexLabelRect(Rect position) => new Rect(position)
        {
            height = EditorGUIUtility.singleLineHeight,
            width = EditorGUIUtility.singleLineHeight,
        };

        /// <summary>
        /// Process Rect to draw color selector
        /// </summary>
        private Rect ColorRect(Rect position) => new Rect(position)
        {
            height = EditorGUIUtility.singleLineHeight,
            x = IndexLabelRect(position).xMax + EditorGUIUtility.standardVerticalSpacing,
            width = EditorGUIUtility.singleLineHeight * 3,
        };

        /// <summary>
        /// Process Rect to draw value field
        /// </summary>
        private Rect ValueRect(Rect position) => new Rect(position)
        {
            height = EditorGUIUtility.singleLineHeight,
            x = ColorRect(position).xMax + EditorGUIUtility.standardVerticalSpacing,
            width = EditorGUIUtility.singleLineHeight * 5,
        };

        /// <summary>
        /// Process Rect to draw the object associated to the partition element if any
        /// </summary>
        private Rect DataRect(Rect position)
        {
            // if we were given more than a single line height,
            // then we draw the element data below the (index + color + value) line
            // else the data rect use the remaining length in the (index + color + value) line
            if (position.height > (EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing + 1))
            {
                return new Rect(position)
                {
                    yMin = position.yMin + 1 + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight,
                    yMax = position.yMax - 1,
                };
            }
            else
            {
                return new Rect(position)
                {
                    height = EditorGUIUtility.singleLineHeight,
                    xMin = ValueRect(position).xMax + EditorGUIUtility.singleLineHeight,
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty dataProperty = property.FindPropertyRelative("Object");

            if (dataProperty != null)
            {
                float dataHeight = EditorGUI.GetPropertyHeight(dataProperty);

                if (dataHeight > EditorGUIUtility.singleLineHeight)
                {
                    // if data height is more than one line, we need one line for (index + color + value)
                    // and draw the data below it.
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + dataHeight;
                }
            }

            // if there is no data or if we can fit in one line, we draw it on the same line as index, color and value
            // so we only need one line
            return EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // get properties
            SerializedProperty colorProperty = property.FindPropertyRelative("Color");
            SerializedProperty valueProperty = property.FindPropertyRelative("Value");
            SerializedProperty dataProperty = property.FindPropertyRelative("Object");
            int index = GetIndexFromPath(property.propertyPath);

            // draw index, color and value
            EditorGUI.LabelField(IndexLabelRect(position), index.ToString());

            EditorGUI.PropertyField(ColorRect(position), colorProperty, GUIContent.none);

            EditorGUI.PropertyField(ValueRect(position), valueProperty, GUIContent.none);

            // draw data field if needed
            if (dataProperty != null)
            {
                EditorGUI.PropertyField(DataRect(position), dataProperty, GUIContent.none, true);
            }
        }

        /// <summary>
        /// Get the index of a serialized property in a serialized array
        /// </summary>
        /// <param name="path">SerializedProperty.propertyPath</param>
        /// <returns>index or -1 if not found</returns>
        public static int GetIndexFromPath(string path)
        {
            // please unity devs, make a SerializedProperty.index field for array type :(

            int startIndex = path.LastIndexOf('[') + 1;
            int length = path.LastIndexOf(']') - startIndex;

            if (startIndex < 0 || length < 1)
                return -1;

            string indexStr = path.Substring(startIndex, length);

            if (int.TryParse(indexStr, out int index))
                return index;
            else return -1;
        }
    }
}

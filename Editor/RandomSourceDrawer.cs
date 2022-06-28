using UnityEditor;
using UnityEngine;


namespace RandomToolbox
{
    /// <summary>
    /// Property drawer for derived class of RandomSourceBase.
    /// Allow to edit seed in inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(RandomSourceBase), true)]
    class RandomSourceDrawer : PropertyDrawer
    {
        private float OneThird(float value) => ( value / 3 ) - 2 * EditorGUIUtility.standardVerticalSpacing;

        /// <summary>
        /// Process Rect for seed field (int)
        /// </summary>
        private Rect SeedRect(Rect fieldPosition) => new Rect(fieldPosition)
        {
            width = OneThird(fieldPosition.width),
        };

        /// <summary>
        /// Process Rect for 'Reseed' button
        /// </summary>
        private Rect ButtonRect(Rect fieldPosition) => new Rect(fieldPosition)
        {
            x = fieldPosition.x + OneThird(fieldPosition.width) + EditorGUIUtility.standardVerticalSpacing,
            width = OneThird(fieldPosition.width),
        };

        /// <summary>
        /// Process Rect for Seed generator dropdown selection (enum)
        /// </summary>
        private Rect GeneratorRect(Rect fieldPosition) => new Rect(fieldPosition)
        {
            x = fieldPosition.x + 2 * ( OneThird(fieldPosition.width) + EditorGUIUtility.standardVerticalSpacing),
            width = OneThird(fieldPosition.width),
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            // draw label (field name)
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty seedProperty = property.FindPropertyRelative("m_seed");
            SerializedProperty generatorProperty = property.FindPropertyRelative(nameof(RandomSourceBase.SeedGenerator));

            // draw seed field (int)
            EditorGUI.PropertyField(SeedRect(position), seedProperty, GUIContent.none);

            // draw 'Reseed' button and generate a new seed if pressed
            if (GUI.Button(ButtonRect(position), "re seed"))
            {
                // retrieve seed generator selected
                SeedGenerators.Generator generator = (SeedGenerators.Generator)generatorProperty.enumValueIndex;

                // generate new seed
                seedProperty.intValue = SeedGenerators.GetSeed(generator);
            }

            // draw seed generator selection dropdown
            EditorGUI.PropertyField(GeneratorRect(position), generatorProperty, GUIContent.none);

            // apply edit if any
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
    }
}

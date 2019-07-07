using UnityEditor;

namespace BrunoMikoski.TextJuicer.Modifiers
{
    [CustomEditor(typeof(TextJuicerTransformModifier))]
    public sealed class TextJuicerTransformModifierInspector : Editor
    {
        private SerializedProperty animatePositionProperty;
        private SerializedProperty animateRotationProperty;
        private SerializedProperty animateScaleProperty;
        private SerializedProperty applyOnXProperty;
        private SerializedProperty applyOnYProperty;
        private SerializedProperty applyOnZProperty;
        private SerializedProperty positionCurveProperty;
        private SerializedProperty positionMultiplierProperty;
        private SerializedProperty rotationCurveProperty;
        private SerializedProperty rotationMultiplierProperty;
        private SerializedProperty scaleCurveProperty;

        private void OnEnable()
        {
            animateScaleProperty = serializedObject.FindProperty("animateScale");
            scaleCurveProperty = serializedObject.FindProperty("scaleCurve");
            applyOnXProperty = serializedObject.FindProperty("applyOnX");
            applyOnYProperty = serializedObject.FindProperty("applyOnY");
            applyOnZProperty = serializedObject.FindProperty("applyOnZ");

            animatePositionProperty = serializedObject.FindProperty("animatePosition");
            positionMultiplierProperty = serializedObject.FindProperty("positionMultiplier");
            positionCurveProperty = serializedObject.FindProperty("positionCurve");

            animateRotationProperty = serializedObject.FindProperty("animateRotation");
            rotationCurveProperty = serializedObject.FindProperty("rotationCurve");
            rotationMultiplierProperty = serializedObject.FindProperty("rotationMultiplier");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(animateScaleProperty);
            if (animateScaleProperty.boolValue)
            {
                EditorGUILayout.PropertyField(scaleCurveProperty);


                EditorGUILayout.PropertyField(applyOnXProperty);

                EditorGUILayout.PropertyField(applyOnYProperty);

                EditorGUILayout.PropertyField(applyOnZProperty);
            }

            EditorGUILayout.PropertyField(animatePositionProperty);
            if (animatePositionProperty.boolValue)
            {
                EditorGUILayout.PropertyField(positionMultiplierProperty);

                EditorGUILayout.PropertyField(positionCurveProperty);
            }

            EditorGUILayout.PropertyField(animateRotationProperty);
            if (animateRotationProperty.boolValue)
            {
                EditorGUILayout.PropertyField(rotationCurveProperty);

                EditorGUILayout.PropertyField(rotationMultiplierProperty);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}

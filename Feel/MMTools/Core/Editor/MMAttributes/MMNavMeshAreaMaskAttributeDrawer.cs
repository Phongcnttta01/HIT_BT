#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MoreMountains.Tools
{
    [CustomPropertyDrawer(typeof(MMNavMeshAreaMaskAttribute))]
    public class MMNavMeshAreaMaskAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            // Lấy danh sách tên khu vực NavMesh
            string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames(); // Thay thế API cũ

            if (navMeshAreaNames == null || navMeshAreaNames.Length == 0)
            {
                EditorGUI.LabelField(position, label, new GUIContent("No NavMesh areas found"));
                return;
            }

            float positionWidth = position.width;
            int maskValue = serializedProperty.intValue;

            position.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(position, label);

            position.x += EditorGUIUtility.labelWidth;
            position.width = positionWidth - EditorGUIUtility.labelWidth;

            EditorGUI.BeginChangeCheck();
            maskValue = EditorGUI.MaskField(position, maskValue, navMeshAreaNames);

            if (EditorGUI.EndChangeCheck())
            {
                serializedProperty.intValue = maskValue;
            }
        }
    }
}
#endif
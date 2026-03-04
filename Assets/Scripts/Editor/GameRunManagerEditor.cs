#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameRunManager))]
public class GameRunManagerEditor : Editor
{
    private SerializedProperty testBelongingsEntitiesProp;
    private SerializedProperty testCardEntitiesProp;
    private bool showDebugTools = false; // 폴드아웃(열고 닫기) 상태 저장

    private void OnEnable()
    {
        // Debug 파트의 리스트 프로퍼티를 찾아옵니다.
        testBelongingsEntitiesProp = serializedObject.FindProperty("testBelongingsEntities");
        testCardEntitiesProp = serializedObject.FindProperty("testCardEntities");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 1. 메인 로직의 변수들을 기본적으로 그려줍니다.
        // ("m_Script"를 제외하여 스크립트 참조 필드 중복을 막고, testBelongingsEntities는 아래에서 따로 그립니다.)
        DrawPropertiesExcluding(serializedObject, "m_Script", "testBelongingsEntities", "testCardEntities");

        EditorGUILayout.Space(10);

        // 2. 디버그 툴 폴드아웃(열고 닫기) 시작
        GUIStyle foldoutStyle = EditorStyles.foldoutHeader;
        foldoutStyle.fontStyle = FontStyle.Bold;
        
        showDebugTools = EditorGUILayout.Foldout(showDebugTools, "🛠️ Debug / Test Tools", true, foldoutStyle);

        if (showDebugTools)
        {
            // 폴드아웃 내부에 약간의 들여쓰기 적용
            EditorGUI.indentLevel++;

            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox("테스트 시에만 사용하는 데이터 및 기능입니다.", MessageType.Info);

            // 리스트 그리기
            EditorGUILayout.PropertyField(testBelongingsEntitiesProp, true);
            EditorGUILayout.PropertyField(testCardEntitiesProp, true);

            EditorGUILayout.Space(5);

            // 버튼 그리기
            GameRunManager manager = (GameRunManager)target;
            if (GUILayout.Button("Add Belongings", GUILayout.Height(30)))
            {
                manager.TestAddBelongings();
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Add Card", GUILayout.Height(30)))
            {
                manager.TestAddCard();
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
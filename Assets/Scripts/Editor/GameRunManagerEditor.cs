#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameRunManager))]
public class GameRunManagerEditor : Editor
{
    private SerializedProperty testBelongingsEntitiesProp;
    private SerializedProperty testCardEntitiesProp;
    private SerializedProperty testHurtDamage;
    private SerializedProperty isOverflowable;
    
    private SerializedProperty testHealAmount;
    private SerializedProperty isHealOverflowable;

    private bool showDebugTools = false; 

    private void OnEnable()
    {
        testBelongingsEntitiesProp = serializedObject.FindProperty("testBelongingsEntities");
        testCardEntitiesProp = serializedObject.FindProperty("testCardEntities");
        
        testHurtDamage = serializedObject.FindProperty("testHurtDamage");
        isOverflowable = serializedObject.FindProperty("isOverflowable");
        
        testHealAmount = serializedObject.FindProperty("testHealAmount");
        isHealOverflowable = serializedObject.FindProperty("isHealOverflowable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", "testBelongingsEntities", "testCardEntities", 
            "testHurtDamage", "isOverflowable", "testHealAmount", "isHealOverflowable");

        EditorGUILayout.Space(10);

        GUIStyle foldoutStyle = EditorStyles.foldoutHeader;
        foldoutStyle.fontStyle = FontStyle.Bold;
        
        showDebugTools = EditorGUILayout.Foldout(showDebugTools, "🛠️ Debug / Test Tools", true, foldoutStyle);

        if (showDebugTools)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox("테스트 시에만 사용하는 데이터 및 기능입니다.", MessageType.Info);

            EditorGUILayout.PropertyField(testBelongingsEntitiesProp, true);
            EditorGUILayout.PropertyField(testCardEntitiesProp, true);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("💥 Hurt Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(testHurtDamage, true);
            EditorGUILayout.PropertyField(isOverflowable, true);
            
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("💖 Heal Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(testHealAmount, true);
            EditorGUILayout.PropertyField(isHealOverflowable, true);

            EditorGUILayout.Space(10);

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

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Hurt Player (Battle Health)", GUILayout.Height(30)))
            {
                manager.TestHurtPlayer();
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Heal Mentality", GUILayout.Height(30)))
            {
                manager.TestHealMentality();
            }
            
            EditorGUILayout.Space(2);
            
            if (GUILayout.Button("Heal Battle Health", GUILayout.Height(30)))
            {
                manager.TestHealBattleHealth();
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
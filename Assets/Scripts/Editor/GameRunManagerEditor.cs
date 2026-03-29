#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using View.BattleView;

[CustomEditor(typeof(GameRunManager))]
public class GameRunManagerEditor : Editor
{
    // Inventory
    private SerializedProperty testBelongingsEntitiesProp;
    private SerializedProperty testCardEntitiesProp;
    private SerializedProperty testRemoveCardEntitiesProp; 
    
    // Player
    private SerializedProperty testHurtDamage;
    private SerializedProperty isOverflowable;
    private SerializedProperty testHealAmount;
    private SerializedProperty isHealOverflowable;

    // Enemy
    private SerializedProperty enemyProp;
    private SerializedProperty enemyHurtAmountProp;
    private SerializedProperty enemyHealAmountProp;

    // Status Effect
    private SerializedProperty statusEffectTargetViewObjectProp;
    private SerializedProperty statusEffectDataProp;
    private SerializedProperty effectStackProp;
    private SerializedProperty effectDurationProp;
    private SerializedProperty isEffectEternalProp;
    private SerializedProperty statusEffectIconToRemoveProp;

    private bool showDebugTools = false; 

    private void OnEnable()
    {
        testBelongingsEntitiesProp = serializedObject.FindProperty("testBelongingsEntities");
        testCardEntitiesProp = serializedObject.FindProperty("testCardEntities");
        testRemoveCardEntitiesProp = serializedObject.FindProperty("testRemoveCardEntities"); 
        
        testHurtDamage = serializedObject.FindProperty("testHurtDamage");
        isOverflowable = serializedObject.FindProperty("isOverflowable");
        
        testHealAmount = serializedObject.FindProperty("testHealAmount");
        isHealOverflowable = serializedObject.FindProperty("isHealOverflowable");

        enemyProp = serializedObject.FindProperty("enemy");
        enemyHurtAmountProp = serializedObject.FindProperty("enemyHurtAmount");
        enemyHealAmountProp = serializedObject.FindProperty("enemyHealAmount");

        // 💡 GameObject 변수명 매핑으로 수정
        statusEffectTargetViewObjectProp = serializedObject.FindProperty("statusEffectTargetViewObject");
        statusEffectDataProp = serializedObject.FindProperty("statusEffectData");
        effectStackProp = serializedObject.FindProperty("effectStack");
        effectDurationProp = serializedObject.FindProperty("effectDuration");
        isEffectEternalProp = serializedObject.FindProperty("isEffectEternal");
        statusEffectIconToRemoveProp = serializedObject.FindProperty("statusEffectIconToRemove");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", 
            "testBelongingsEntities", "testCardEntities", "testRemoveCardEntities", 
            "testHurtDamage", "isOverflowable", "testHealAmount", "isHealOverflowable", 
            "enemy", "enemyHurtAmount", "enemyHealAmount",
            "statusEffectTargetViewObject", "statusEffectData", "effectStack", "effectDuration", "isEffectEternal", "statusEffectIconToRemove");

        EditorGUILayout.Space(10);

        GUIStyle foldoutStyle = EditorStyles.foldoutHeader;
        foldoutStyle.fontStyle = FontStyle.Bold;
        
        showDebugTools = EditorGUILayout.Foldout(showDebugTools, "🛠️ Debug / Test Tools", true, foldoutStyle);

        if (showDebugTools)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox("테스트 시에만 사용하는 데이터 및 기능입니다.", MessageType.Info);

            // 1. Inventory & Deck Settings
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("🎒 Inventory & Deck", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(testBelongingsEntitiesProp, true);
            EditorGUILayout.PropertyField(testCardEntitiesProp, true);
            EditorGUILayout.PropertyField(testRemoveCardEntitiesProp, true);

            // 2. Player Status Settings
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("🧍 Player Status", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(testHurtDamage, true);
            EditorGUILayout.PropertyField(isOverflowable, true);
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(testHealAmount, true);
            EditorGUILayout.PropertyField(isHealOverflowable, true);

            // 3. Enemy Target Settings
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("👾 Target Enemy Settings", EditorStyles.boldLabel);
            enemyProp.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Target Enemy View"), enemyProp.objectReferenceValue, typeof(BattleEnemyView), true);
            EditorGUILayout.PropertyField(enemyHurtAmountProp, true);
            EditorGUILayout.PropertyField(enemyHealAmountProp, true);

            // 4. Status Effect Settings
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("✨ Status Effect Settings", EditorStyles.boldLabel);
            
            statusEffectTargetViewObjectProp.objectReferenceValue = EditorGUILayout.ObjectField(
                new GUIContent("Target Entity View (Object)"), 
                statusEffectTargetViewObjectProp.objectReferenceValue, 
                typeof(GameObject), 
                true
            );
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("└ Apply Settings", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(statusEffectDataProp, new GUIContent("Effect Entity"), true);
            EditorGUILayout.PropertyField(effectStackProp, new GUIContent("Stack"), true);
            EditorGUILayout.PropertyField(effectDurationProp, new GUIContent("Duration"), true);
            EditorGUILayout.PropertyField(isEffectEternalProp, new GUIContent("Is Eternal"), true);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("└ Remove Settings", EditorStyles.miniBoldLabel);
            
            statusEffectIconToRemoveProp.objectReferenceValue = EditorGUILayout.ObjectField(
                new GUIContent("Target Icon to Remove"), 
                statusEffectIconToRemoveProp.objectReferenceValue, 
                typeof(BattleStatusEffectIcon), 
                true
            );

            EditorGUILayout.Space(15);

            // 🚀 테스트 실행 버튼 패널
            GameRunManager manager = (GameRunManager)target;

            GUILayout.BeginVertical("box");
            
            GUIStyle centerBoldStyle = new GUIStyle(EditorStyles.boldLabel) 
            { 
                alignment = TextAnchor.MiddleCenter 
            };
            EditorGUILayout.LabelField("▶️ Execute Tests", centerBoldStyle);
            
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Belongings", GUILayout.Height(25))) manager.TestAddBelongings();
            if (GUILayout.Button("Add Card", GUILayout.Height(25))) manager.TestAddCard();
            if (GUILayout.Button("Remove Card", GUILayout.Height(25))) manager.TestRemoveCard();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Hurt Player (HP)", GUILayout.Height(25))) manager.TestHurtPlayer();
            if (GUILayout.Button("Heal Player (HP)", GUILayout.Height(25))) manager.TestHealBattleHealth();
            if (GUILayout.Button("Heal Mentality", GUILayout.Height(25))) manager.TestHealMentality();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Hurt Target Enemy", GUILayout.Height(25))) manager.TestHurtEnemy();
            if (GUILayout.Button("Heal Target Enemy", GUILayout.Height(25))) manager.TestHealEnemy();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("✨ Apply Status Effect", GUILayout.Height(30))) manager.TestApplyBattleStatusEffect();
            if (GUILayout.Button("🗑️ Remove Status Effect", GUILayout.Height(30))) manager.TestRemoveBattleStatusEffect();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class ViewScriptGenerator : EditorWindow
{
    // 💡 1. 여기에 씬별 프리셋 데이터를 정의합니다. (나중에 씬이 추가되면 여기에 추가만 하세요!)
    private enum ScenePreset { None, Global, StartMenu, ScheduleSelecting, ScheduleView }

    private ScenePreset currentPreset = ScenePreset.None;
    
    // 현재 선택된 프리셋의 고정 데이터
    private string targetNamespace = "";
    private string eventNamespace = "";
    private string eventType = "";
    private string commanderType = "";

    // 사용자가 수정할 데이터
    private string scriptName = "NewViewScript";
    private bool isInteractable = false;

    [MenuItem("Tools/Architecture/Generate View Script")]
    public static void ShowWindow()
    {
        GetWindow<ViewScriptGenerator>("View Generator");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("1. 씬(Scene) 프리셋 선택", EditorStyles.boldLabel);
        
        // 💡 2. 프리셋 버튼들을 가로로 예쁘게 배치합니다.
        if (GUILayout.Button("전역 (Global)", GUILayout.Height(30))) ApplyPreset(ScenePreset.Global);
        GUILayout.Space(5);
        if (GUILayout.Button("시작 메뉴 (StartMenu)", GUILayout.Height(30))) ApplyPreset(ScenePreset.StartMenu);
        GUILayout.Space(5);
        if (GUILayout.Button("일정 선택 (Selecting)", GUILayout.Height(30))) ApplyPreset(ScenePreset.ScheduleSelecting);
        GUILayout.Space(5);
        if (GUILayout.Button("일정 진행 (Schedule)", GUILayout.Height(30))) ApplyPreset(ScenePreset.ScheduleView);

        // 프리셋이 선택되지 않았다면 아래 내용은 숨깁니다.
        if (currentPreset == ScenePreset.None)
        {
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("위에서 씬 프리셋을 먼저 선택해 주세요.", MessageType.Info);
            return;
        }

        // 선택된 프리셋 정보 보여주기 (읽기 전용 상태로 표시하여 실수 방지)
        GUILayout.Space(15);
        GUILayout.Label($"선택된 씬: {currentPreset}", EditorStyles.boldLabel);
        GUI.enabled = false; // 아래 필드들은 수정 불가 처리
        EditorGUILayout.TextField("Namespace", targetNamespace);
        EditorGUILayout.TextField("Event Type", eventType);
        if (isInteractable) EditorGUILayout.TextField("Commander Type", commanderType);
        GUI.enabled = true;  // 다시 수정 가능 처리

        GUILayout.Space(15);
        GUILayout.Label("2. 스크립트 설정", EditorStyles.boldLabel);
        
        // 💡 3. 여기서 사용자가 스크립트 이름과 상호작용 여부를 결정합니다.
        scriptName = EditorGUILayout.TextField("Script Name", scriptName);

        isInteractable = EditorGUILayout.Toggle("Is Interactable (조작 가능)?", isInteractable);
        GUILayout.Space(20);

        // 생성 버튼
        GUI.backgroundColor = new Color(0.2f, 0.8f, 0.2f); // 초록색 버튼으로 강조!
        if (GUILayout.Button($"{scriptName}.cs 생성하기", GUILayout.Height(40)))
        {
            GenerateScript();
        }
        GUI.backgroundColor = Color.white;
    }

    // 💡 4. 프리셋 버튼을 누르면 알맞은 데이터가 자동으로 채워집니다.
    private void ApplyPreset(ScenePreset preset)
    {
        currentPreset = preset;
        switch (preset)
        {
            case ScenePreset.Global:
                targetNamespace = "View.Global";
                eventNamespace = "ViewEvent.Global"; // 예시
                eventType = "IViewEvent";
                commanderType = "IViewCommander";
                break;

            case ScenePreset.StartMenu:
                targetNamespace = "View.StartMenu";
                eventNamespace = "ViewEvent.StartMenu";
                eventType = "IStartMenuViewEvent";
                commanderType = "IStartMenuViewCommander";
                break;

            case ScenePreset.ScheduleSelecting:
                targetNamespace = "View.ScheduleSelecting";
                eventNamespace = "ViewEvent.ScheduleSelecting";
                eventType = "IScheduleSelectingEvent";
                commanderType = "ISelectingScheduleViewCommander";
                break;

            case ScenePreset.ScheduleView:
                targetNamespace = "View.ScheduleView";
                eventNamespace = "ViewEvent.ScheduleView";
                eventType = "IScheduleViewEvent";
                commanderType = "IScheduleViewCommander";
                break;
        }
    }

    private void GenerateScript()
    {
        if (string.IsNullOrEmpty(scriptName))
        {
            EditorUtility.DisplayDialog("오류", "스크립트 이름을 입력해주세요.", "확인");
            return;
        }

        string path = GetSelectedPathOrFallback();
        string fullPath = Path.Combine(path, scriptName + ".cs");

        if (File.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("오류", "이미 같은 이름의 파일이 존재합니다!", "확인");
            return;
        }

        string template = CreateTemplate();
        File.WriteAllText(fullPath, template, Encoding.UTF8);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("성공", $"{scriptName}.cs 파일이 생성되었습니다!\n경로: {path}", "확인");
    }

    private string CreateTemplate()
    {
        StringBuilder sb = new StringBuilder();

        // 기본 using문
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using View.Core;");
        sb.AppendLine("using ViewEvent.Core;");
        
        // 프리셋에 맞는 이벤트 네임스페이스 추가
        if (!string.IsNullOrEmpty(eventNamespace))
        {
            sb.AppendLine($"using {eventNamespace};");
        }

        sb.AppendLine();
        sb.AppendLine($"namespace {targetNamespace}");
        sb.AppendLine("{");

        // 클래스 선언부 (Interactable 여부에 따라 분기)
        if (isInteractable)
        {
            sb.AppendLine($"    public class {scriptName} : InteractableViewBehaviour<{eventType}, {commanderType}>");
        }
        else
        {
            sb.AppendLine($"    public class {scriptName} : ViewBehaviour<{eventType}>");
        }
        
        sb.AppendLine("    {");
        
        // OnInitialized
        sb.AppendLine("        public override void OnInitialized()");
        sb.AppendLine("        {");
        sb.AppendLine("            // TODO: 이벤트 구독 (예: eventBus.Subscribe<T>(Method);)");
        sb.AppendLine("        }");
        sb.AppendLine();
        
        // OnDestroy
        sb.AppendLine("        public override void OnDestroy()");
        sb.AppendLine("        {");
        sb.AppendLine("            // TODO: 이벤트 구독 해제 (예: eventBus.Unsubscribe<T>(Method);)");
        sb.AppendLine("        }");
        
        // Interactable일 경우 버튼 클릭 예시 메서드 추가
        if (isInteractable)
        {
            sb.AppendLine();
            sb.AppendLine("        public void OnInteract()");
            sb.AppendLine("        {");
            sb.AppendLine("            // TODO: commander를 통해 Model에 명령 전달");
            sb.AppendLine("            // commander.DoSomething();");
            sb.AppendLine("        }");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
#endif
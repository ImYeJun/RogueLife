using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DiaryTextCurver : MonoBehaviour
{
    [Header("Anchor Settings (다중 앵커 무제한 지원)")]
    [Tooltip("순서에 상관없이 씬에 배치한 앵커들을 넣어주세요. (최소 2개 이상 필요)")]
    public Transform[] curveAnchors;

    [Header("Curve Options")]
    [Tooltip("휘어지는 강도 (Y축, Z축 모두 적용됨)")]
    public float curveStrength = 1f;
    
    [Header("3D Rotation Options")]
    [Tooltip("글자가 곡선의 경사를 따라 자연스럽게 기울어질지 여부")]
    public bool alignToCurve = true;
    
    [Tooltip("💡 텍스트가 뒤로 눕는 각도 (단위: 도(Degree))\n(양수: 뒤로 눕는 오목한 느낌, 음수: 앞으로 쏟아지는 느낌)")]
    [Range(-90f, 90f)]
    public float backwardTiltAngle = 15f; 

    [Header("Jitter Options (손글씨 감성 랜덤 틀어짐)")]
    [Tooltip("글자마다 위치가 무작위로 틀어질 최대 범위 (X, Y, Z)")]
    public Vector3 randomPositionOffset = new Vector3(1f, 1f, 0f);
    
    [Tooltip("글자마다 각도가 무작위로 틀어질 최대 범위 (X, Y, Z)")]
    public Vector3 randomRotationOffset = new Vector3(0f, 0f, 2f);

    public void ApplyCurve(TMP_Text textComponent)
    {
        if (textComponent == null || curveAnchors == null || curveAnchors.Length < 2) return;

        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;

        if (textInfo.characterCount == 0) return;

        List<Vector3> localPts = new List<Vector3>();
        foreach (var anchor in curveAnchors)
        {
            if (anchor != null)
                localPts.Add(textComponent.transform.InverseTransformPoint(anchor.position));
        }
        
        localPts = localPts.OrderBy(p => p.x).ToList();

        for (int i = 1; i < localPts.Count; i++)
        {
            if (localPts[i].x <= localPts[i - 1].x)
                localPts[i] = new Vector3(localPts[i - 1].x + 0.01f, localPts[i].y, localPts[i].z);
        }
        
        AnimationCurve dynamicCurveY = new AnimationCurve();
        AnimationCurve dynamicCurveZ = new AnimationCurve();

        for (int i = 0; i < localPts.Count; i++)
        {
            dynamicCurveY.AddKey(new Keyframe(localPts[i].x, localPts[i].y));
            dynamicCurveZ.AddKey(new Keyframe(localPts[i].x, localPts[i].z));
            
            dynamicCurveY.SmoothTangents(i, 0f);
            dynamicCurveZ.SmoothTangents(i, 0f);
        }

        Vector3 pFirst = localPts[0];
        Vector3 pLast = localPts[localPts.Count - 1];

        // 💡 텍스트가 갱신될 때마다 랜덤값이 너무 요동치지 않도록 시드(Seed)를 고정!
        // (타이핑 효과 중에도 이미 써진 글자는 모양을 유지하게 만듭니다)
        Random.InitState(textComponent.text.GetHashCode());

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            if (materialIndex < 0 || materialIndex >= textInfo.meshInfo.Length) continue;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            if (vertexIndex < 0 || vertexIndex + 3 >= vertices.Length) continue;

            // 글자 중심 좌표 
            Vector3 charCenter = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2f;
            
            float xPos = Mathf.Clamp(charCenter.x, pFirst.x, pLast.x);

            float curveY = dynamicCurveY.Evaluate(xPos);
            float curveZ = dynamicCurveZ.Evaluate(xPos);

            float t_straight = Mathf.InverseLerp(pFirst.x, pLast.x, xPos);
            float straightY = Mathf.Lerp(pFirst.y, pLast.y, t_straight);
            float straightZ = Mathf.Lerp(pFirst.z, pLast.z, t_straight);

            float bendOffsetY = (curveY - straightY) * curveStrength;
            float bendOffsetZ = (curveZ - straightZ) * curveStrength;

            // 💡 [추가] 각 글자마다 랜덤한 오프셋 생성
            float randX = Random.Range(-randomPositionOffset.x, randomPositionOffset.x);
            float randY = Random.Range(-randomPositionOffset.y, randomPositionOffset.y);
            float randZ = Random.Range(-randomPositionOffset.z, randomPositionOffset.z);
            Vector3 jitterOffset = new Vector3(randX, randY, randZ);

            float randRotX = Random.Range(-randomRotationOffset.x, randomRotationOffset.x);
            float randRotY = Random.Range(-randomRotationOffset.y, randomRotationOffset.y);
            float randRotZ = Random.Range(-randomRotationOffset.z, randomRotationOffset.z);

            // --- 회전(Rotation) 계산 ---
            Quaternion charRotation = Quaternion.identity;
            
            if (alignToCurve)
            {
                float delta = 0.5f; 
                float y1 = dynamicCurveY.Evaluate(xPos - delta);
                float y2 = dynamicCurveY.Evaluate(xPos + delta);
                float z1 = dynamicCurveZ.Evaluate(xPos - delta);
                float z2 = dynamicCurveZ.Evaluate(xPos + delta);

                Vector3 tangent = new Vector3(delta * 2f, y2 - y1, z2 - z1).normalized;
                
                float zRot = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
                float yRot = -Mathf.Atan2(tangent.z, tangent.x) * Mathf.Rad2Deg;
                float xRot = backwardTiltAngle;

                // 💡 [수정] 기본 곡선 회전에 랜덤 회전값을 더해줍니다!
                charRotation = Quaternion.Euler(xRot + randRotX, yRot + randRotY, zRot + randRotZ);
            }
            else
            {
                charRotation = Quaternion.Euler(backwardTiltAngle + randRotX, randRotY, randRotZ);
            }

            // 매트릭스 변환 적용 (기존 곡선 위치 + 랜덤 Jitter 위치)
            Vector3 offset = new Vector3(0, bendOffsetY, bendOffsetZ) + jitterOffset;
            Matrix4x4 matrix = Matrix4x4.TRS(offset, charRotation, Vector3.one);

            for (int j = 0; j < 4; j++)
            {
                Vector3 vertex = vertices[vertexIndex + j];
                vertex -= charCenter; // 로컬 중앙으로
                vertex = matrix.MultiplyPoint3x4(vertex); // TRS 적용
                vertex += charCenter; // 다시 복귀
                vertices[vertexIndex + j] = vertex;
            }
        }

        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

#if UNITY_EDITOR
    [Header("Test Mode")]
    public TMP_Text testTextComponent;

    [ContextMenu("Test Apply Curve")]
    public void TestApplyCurve()
    {
        if (testTextComponent != null)
        {
            ApplyCurve(testTextComponent);
            Debug.Log($"[DiaryTextCurver] {curveAnchors.Length}개의 다중 앵커 + 랜덤 Jitter 워핑 완료!");
        }
    }
#endif
}
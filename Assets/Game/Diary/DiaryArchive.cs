using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DiaryArchive
{
    private List<Diary> diaries = new List<Diary>();
    
    public void AddDiary(Diary diary)
    {
        string json = JsonUtility.ToJson(diary);
        string encryptedJson = EncryptDecrypt(json);

        string diaryID = Guid.NewGuid().ToString();
        string savePath = Path.Combine(Constant.DIARY_STORE_PATH, diaryID) + ".txt";

        EnsureDiaryDirectoryExists();
        File.WriteAllText(savePath, encryptedJson);
    }

    public void LoadDiaries()
    {
        EnsureDiaryDirectoryExists();
        string[] diaryPaths = Directory.GetFiles(Constant.DIARY_STORE_PATH, "*.txt");

        foreach (string path in diaryPaths)
        {
            string encryptedJson = File.ReadAllText(path);
            string json = EncryptDecrypt(encryptedJson);
            Diary diary = JsonUtility.FromJson<Diary>(json);

            diaries.Add(diary);
        }
    }

    public bool HasDiary(Diary operand)
    {
        return diaries.Any(diary => diary.Equals(operand));
    }

    private string EncryptDecrypt(string data)
    {
        char[] buffer = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            buffer[i] = (char)(data[i] ^ Constant.ENCODE_KEY[i % Constant.ENCODE_KEY.Length]);
        }

        return new string(buffer);
    }

    private void EnsureDiaryDirectoryExists()
    {
        if (!Directory.Exists(Constant.DIARY_STORE_PATH)) { Directory.CreateDirectory(Constant.DIARY_STORE_PATH); }
    }
}
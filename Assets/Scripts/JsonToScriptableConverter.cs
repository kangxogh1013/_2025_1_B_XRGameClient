#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                        //JSON ���� ��� ���ڿ� ��
    private string outputFolder = "Assets/ScriptableObjects/otems";          //��� SO������ ��� ��
    private bool createDatabase = true;

    [MenuItem("Tools/JSON to Scriptable Objects")]

    public static void ShowWinodw()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Object");
    }

    void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable Object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Select JSON File"))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }

        EditorGUILayout.LabelField("Selected File : ", jsonFilePath);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder :", outputFolder);
        createDatabase = EditorGUILayout.Toggle("Create Database Asset", createDatabase);
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert to Scriptalbe Objects"))
        {
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a JSON file firest!", "OK");
                return;
            }
            ConvertJsonToScriptableObjects();
        }
    }


    private void ConvertJsonToScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        //JSON���� �б�
        string jsonText = File.ReadAllText(jsonFilePath);                 //json ������ �д´�

        try
        {
            //JSON �Ľ�
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText);

            List<ItemSO> createdltems = new List<ItemSO>();               //ItemSO����Ʈ ����

            //�� ������ �����͸� ��ũ���ͺ� ������Ʈ�� ��ȯ
            foreach (var itemData in itemDataList)
            {
                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();  //Item������ ����

                //������ ����
                itemSO.id = itemData.id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                //������ ��ȯ
                if (System.Enum.TryParse(itemData.itemTypeString, out ItemTypes parsedType))
                {
                    itemSO.itemTypes = parsedType;
                }
                else
                {
                    Debug.LogWarning($"������ '{itemData.itemName}'�� �������� ���� Ÿ�� : {itemData.itemTypeString}");
                }

                itemSO.price = itemData.price;
                itemSO.power = itemData.power;
                itemSO.level = itemData.level;
                itemSO.isStackable = itemData.isStackable;

                //������ �ε�(��ΰ� ���� ���)
                if (!string.IsNullOrEmpty(itemData.iconPath))
                {
                    itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Resources/{itemData.iconPath}.png");

                    if (itemSO.icon == null)
                    {
                        Debug.LogWarning($"������ '{itemData.nameEng}'�� �������� ã�� �� �����ϴ�. : {itemData.iconPath}");
                    }
                }
                //��ũ���ͺ� ������Ʈ ���� - ID�� 4�ڸ� ���ڷ� ������
                string assetPath = $"{outputFolder}/Item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                AssetDatabase.CreateAsset(itemSO, assetPath);

                //���� �̸� ����
                itemSO.name = $"{itemData.id.ToString("D4")}+{itemData.nameEng}";
                createdltems.Add(itemSO);

                EditorUtility.SetDirty(itemSO);
            }

            //�����ͺ��̽� ����
            if (createDatabase && createdltems.Count > 0)
            {
                ItemDatabaseSO database = ScriptableObject.CreateInstance<ItemDatabaseSO>();    //ItemDatabaseSO ����
                database.items = createdltems;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/ItemDatabase.asset");
                EditorUtility.SetDirty(database);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdltems.Count} scriptable objects!", "OK");

        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Covert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ���� : {e}");
        }
    }
}


#endif
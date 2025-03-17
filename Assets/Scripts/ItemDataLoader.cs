using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class ItemDataLoader : MonoBehaviour
{
    [SerializeField]
    private string jsonFileName = "items";          //Resources �������� ������ JSON ���� �̸�

    private List<ItemData> itemList;

    void Start()
    {
        LoadltemData();
    }

    // Start is called before the first frame update
    void LoadltemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            // ���� �ؽ�Ʈ���� UTF-8�� ��ȯ ó��
            byte[] bytes = Encoding.Default.GetBytes(jsonFile.text);
            string correntText = Encoding.UTF8.GetString(bytes);

            itemList = JsonConvert.DeserializeObject<List<ItemData>>(correntText);

            Debug.Log($"�ε�� ������ ��: {itemList.Count}");

            foreach (var item in itemList)
            {
                Debug.Log($"������: {EncodeKorean(item.itemName)}, ���� : {EncodeKorean(item.description)}");
            }
        }
        else
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�. : {jsonFileName}");
        }
    }

   private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";           //�ؽ�Ʈ�� NULL ���̸� �Լ��� ������.
        byte[] bytes = Encoding.Default.GetBytes(text);      //string�� Byte�迭�� ��ȯ�� ��
        return Encoding.UTF8.GetString(bytes);               //���ڵ��� UTF8�� �ٲ۴�.
    }
}

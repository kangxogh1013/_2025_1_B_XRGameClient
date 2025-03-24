using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public int id;
    public string itemName;
    public string description;
    public string nameEng;
    public string itemTypeString;
    [NonSerialized]
    public ItemTypes itemType;
    public int price;
    public int power;
    public int level;
    public bool isStackable;
    public string iconPath;

    public void InitalizeEnums()
    {
        if(Enum.TryParse(itemTypeString, out ItemTypes parsedType))
        {
            itemType = parsedType;
        }
        else
        {
            Debug.LogError($"아이템 '{itemName}'에 유효하지 않은 아이템 타입 : {itemTypeString}");
            //기본값 설정
            itemType = ItemTypes.Consumable;
        }
    }
}

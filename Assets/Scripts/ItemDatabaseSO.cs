using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Database")]

public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();         //item ����Ʈ�� ���� �Ǿ� �ִ� ���� ������  Dictionary�� �Է��Ѵ�

    //ĳ���� ���� ����
    private Dictionary<int, ItemSO> itemsByld;              //ID�� ������ ã�� ���� ĳ��
    private Dictionary<string, ItemSO> itemsByName;         //�̸����� ������ ã��

    public void Initalize()                                 //�ʱ⼳�� �Լ�
    {
        itemsByld = new Dictionary<int, ItemSO>();          //���� ���� �߱� ������ Dictionary �Ҵ�
        itemsByName = new Dictionary<string, ItemSO>(); 

        foreach (var item in items)                         //item ����Ʈ�� ���� �Ǿ� �ִ� ���� ������  Dictionary�� �Է��Ѵ�.
        {
            itemsByld[item.id] = item;
            itemsByName[item.itemName] = item;
        }
    }

    //ID�� ������ ã��
    public ItemSO GetItemByld(int id)                       //itemsByld�� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�
    {
        if(itemsByld == null)
        {
            Initalize();
        }

        if (itemsByld.TryGetValue(id, out ItemSO item))     //ID ���� ã�Ƽ� ItemSO�� ���� �Ѵ�
            return item;

        return null;                                        //���� ��� NULL
    }

    //�̸����� ������ ã��
    public ItemSO GetItemByName(string name)
    {
        if(itemsByName == null)                             //itemsByName �� ĳ���� �Ǿ����� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initalize();
        }
        if (itemsByName.TryGetValue(name, out ItemSO item)) //name ���� ã�Ƽ� ItemSO�� ���� �Ѵ�.
            return item;

        return null;
    }

    //Ÿ������ ������ ���͸�
    public List<ItemSO> GetItemByTape(ItemTypes types)
    {
        return items.FindAll(item => item.itemTypes == types);
    }
}

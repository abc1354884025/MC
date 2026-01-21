using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;


public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;
    private List<ItemDetails> itemList=new List<ItemDetails>();
    private VisualTreeAsset itemRowTemplate;

    private ListView itemListView;
    private ScrollView itemDetailsSection;

    private VisualElement iconPreview;

    //选中的物品
    private ItemDetails activeItem;
    //默认图标
    private Sprite defaultIcon;
    [MenuItem("M STUDIO/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        // 获取单元模板
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ItemRowTemplate.uxml");

        // 变量赋值
        itemListView=root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection=root.Q<ScrollView>("ItemDetails");
        iconPreview=itemDetailsSection.Q<VisualElement>("Icon");
        defaultIcon=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //添加物品
        root.Q<Button>("AddItemBtn").clicked+=OnAddItemBtnClicked;
        //删除物品
        root.Q<Button>("DeleteItemBtn").clicked+=OnDeleteItemBtnClicked;

        //加载数据
        LoadDataBase();

        GenerateListView();

        //初始隐藏详情面板
        itemDetailsSection.visible = false;
    }

    private void LoadDataBase()
    {
        var dataArray=AssetDatabase.FindAssets("ItemDataList_SO");
        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase=AssetDatabase.LoadAssetAtPath<ItemDataList_SO>(path) as ItemDataList_SO;
        }
        itemList = dataBase.itemDetailsList;
        EditorUtility.SetDirty(dataBase);

    }
    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        Action<VisualElement,int> bindItem = (e, i) =>
        {
            if (i > itemList.Count) return;
            var item = itemList[i];
            if(item.itemIcon!=null)
                e.Q<VisualElement>("Icon").style.backgroundImage = item.itemIcon.texture;
            e.Q<Label>("Name").text = item ==null?"NO ITEM":item.itemName;
        };

        itemListView.itemsSource=itemList;
        itemListView.makeItem=makeItem;
        itemListView.bindItem=bindItem;
        itemListView.onSelectionChange += OnListSelectionChange;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItems)
    {
        activeItem=selectedItems.First() as ItemDetails;
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value=activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID=evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        var icon = activeItem.itemIcon?activeItem.itemIcon : defaultIcon;
        iconPreview.style.backgroundImage = icon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = icon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon==null?defaultIcon.texture:newIcon.texture;
            itemListView.Rebuild();

        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;

        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;

        });

    }

    private void OnAddItemBtnClicked()
    {
        ItemDetails newItem=new ItemDetails();
        newItem.itemName="NEW ITEM";
        newItem.itemID=1001+itemList.Count;
        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    private void OnDeleteItemBtnClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }
}
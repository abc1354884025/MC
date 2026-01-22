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
                e.Q<VisualElement>("Icon").style.backgroundImage = GetTextureFromSprite(item.itemIcon);
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
        iconPreview.style.backgroundImage = GetTextureFromSprite(icon);
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = icon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon==null?defaultIcon.texture:GetTextureFromSprite(newIcon);
            itemListView.Rebuild();

        });

        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemOnWorldSprite = newIcon;

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

        itemDetailsSection.Q<IntegerField>("UseRadius").value = activeItem.useRadius;
        itemDetailsSection.Q<IntegerField>("UseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.useRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedUp").value = activeItem.canPickedup;
        itemDetailsSection.Q<Toggle>("CanPickedUp").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.price;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.price = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").lowValue = 0;
        itemDetailsSection.Q<Slider>("SellPercentage").highValue = 1;
        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
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
    private Texture2D GetTextureFromSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            return null;
        }

        // 如果Sprite的纹理是独立的（没有使用图集），可以直接使用
        if (sprite.texture != null && sprite.texture.isReadable)
        {
            // 如果Sprite的矩形就是整个纹理，那么可以直接使用
            if (sprite.rect.width == sprite.texture.width && sprite.rect.height == sprite.texture.height)
            {
                return sprite.texture;
            }
            else
            {
                // 否则，我们需要从Sprite中提取出对应的区域
                // 注意：这里假设纹理是可读的，如果没有勾选Read/Write Enabled，需要先处理
                Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
                newTexture.SetPixels(pixels);
                newTexture.Apply();
                return newTexture;
            }
        }
        else
        {
            
            //不能转化
            return sprite.texture;
        }
    }
}
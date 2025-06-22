using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopView : MonoBehaviour
{
    [Header("Shop UI")]
    public Transform shopItemsContainer;
    public GameObject shopItemPrefab;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI shopTitleText;
    public Button closeButton;

    [Header("Selected Item Info")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemCost;
    public Button purchaseButton;

    private LogicManager logicManager;
    private List<GameObject> shopItemObjects = new List<GameObject>();
    private ShopItem selectedItem;

    void Start()
    {
        logicManager = LogicManager.Instance;

        // 닫기 버튼 이벤트
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseShop);
        }

        // 구매 버튼 이벤트
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        }
    }

    public void UpdateView(Game gameData)
    {
        if (gameData == null) return;

        UpdateCurrencyDisplay(gameData);
        UpdateShopItems();
        UpdateSelectedItemInfo();
    }

    private void UpdateCurrencyDisplay(Game gameData)
    {
        if (currencyText != null)
        {
            currencyText.text = $"보유 골드: {gameData.currency}";
        }

        if (shopTitleText != null)
        {
            shopTitleText.text = "마법 상점";
        }
    }

    private void UpdateShopItems()
    {
        if (logicManager == null || shopItemsContainer == null || shopItemPrefab == null)
            return;

        // 기존 아이템 UI들 정리
        ClearShopItems();

        // 상점 아이템들 가져오기
        List<ShopItem> shopItems = logicManager.GetShopItems();

        // 각 아이템에 대한 UI 생성
        foreach (ShopItem item in shopItems)
        {
            GameObject itemObject = Instantiate(shopItemPrefab, shopItemsContainer);
            shopItemObjects.Add(itemObject);

            SetupShopItemUI(itemObject, item);
        }
    }

    private void SetupShopItemUI(GameObject itemObject, ShopItem item)
    {
        // 아이템 정보 설정
        TextMeshProUGUI nameText = itemObject.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = item.name;

        TextMeshProUGUI costText = itemObject.transform.Find("ItemCost")?.GetComponent<TextMeshProUGUI>();
        if (costText != null)
            costText.text = $"{item.cost}G";

        TextMeshProUGUI descText = itemObject.transform.Find("ItemDescription")?.GetComponent<TextMeshProUGUI>();
        if (descText != null)
            descText.text = item.description;

        // 클릭 이벤트 설정
        Button itemButton = itemObject.GetComponent<Button>();
        if (itemButton != null)
        {
            itemButton.onClick.AddListener(() => SelectItem(item));
        }

        // 구매 가능 여부에 따른 UI 업데이트
        Game gameData = logicManager.GetGameData();
        bool canAfford = gameData != null && gameData.currency >= item.cost;

        Image itemImage = itemObject.GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.color = canAfford ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.8f);
        }

        if (itemButton != null)
        {
            itemButton.interactable = canAfford;
        }
    }

    private void SelectItem(ShopItem item)
    {
        selectedItem = item;
        UpdateSelectedItemInfo();
    }

    private void UpdateSelectedItemInfo()
    {
        if (selectedItem == null)
        {
            // 선택된 아이템이 없을 때
            if (selectedItemName != null) selectedItemName.text = "아이템을 선택하세요";
            if (selectedItemDescription != null) selectedItemDescription.text = "";
            if (selectedItemCost != null) selectedItemCost.text = "";
            if (purchaseButton != null) purchaseButton.interactable = false;
            return;
        }

        // 선택된 아이템 정보 표시
        if (selectedItemName != null)
            selectedItemName.text = selectedItem.name;

        if (selectedItemDescription != null)
            selectedItemDescription.text = selectedItem.description;

        if (selectedItemCost != null)
            selectedItemCost.text = $"비용: {selectedItem.cost} 골드";

        // 구매 버튼 활성화/비활성화
        if (purchaseButton != null)
        {
            Game gameData = logicManager?.GetGameData();
            bool canAfford = gameData != null && gameData.currency >= selectedItem.cost;
            purchaseButton.interactable = canAfford;
        }
    }

    private void PurchaseSelectedItem()
    {
        if (selectedItem == null || logicManager == null) return;

        bool success = logicManager.PurchaseShopItem(selectedItem);

        if (success)
        {
            Debug.Log($"아이템 구매 성공: {selectedItem.name}");
            // 구매 후 상점 UI 갱신
            selectedItem = null;
            UpdateShopItems();
        }
        else
        {
            Debug.Log("아이템 구매 실패");
        }
    }

    private void CloseShop()
    {
        if (logicManager != null)
        {
            logicManager.CloseShop();
        }
    }

    private void ClearShopItems()
    {
        foreach (GameObject item in shopItemObjects)
        {
            if (item != null)
                Destroy(item);
        }
        shopItemObjects.Clear();
    }

    void OnDestroy()
    {
        ClearShopItems();
    }
}

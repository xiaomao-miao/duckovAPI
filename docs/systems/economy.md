# 经济系统

## 概述
经济系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的货币、商店、物品解锁等经济相关功能。

## 核心组件

### EconomyManager
经济管理器，管理游戏中的经济系统。

#### 主要功能
- **货币管理** - 添加、移除、获取货币
- **物品解锁** - 解锁物品、检查解锁状态
- **商店管理** - 商店物品管理
- **事件通知** - 货币改变、物品解锁事件

#### 重要属性
- `Instance` - 获取EconomyManager实例
- `ItemUnlockNotificationTextMainFormat` - 物品解锁通知主格式
- `ItemUnlockNotificationTextSubFormat` - 物品解锁通知副格式

#### 重要事件
- `OnEconomyManagerLoaded` - 经济管理器加载事件
- `OnItemUnlocked` - 物品解锁事件
- `OnCurrencyChanged` - 货币改变事件

#### 重要方法
- `UnlockItem(string itemID)` - 解锁物品
- `IsItemUnlocked(string itemID)` - 检查物品是否已解锁
- `GetUnlockedItems()` - 获取已解锁物品
- `AddCurrency(int amount)` - 添加货币
- `RemoveCurrency(int amount)` - 移除货币
- `GetCurrency()` - 获取当前货币

### StockShop
股票商店，管理游戏中的商店系统。

#### 主要功能
- **商店管理** - 开放、关闭商店
- **物品交易** - 购买、出售物品
- **价格管理** - 获取物品价格

#### 重要属性
- `ShopItems` - 商店物品列表
- `IsOpen` - 是否开放

#### 重要方法
- `OpenShop()` - 开放商店
- `CloseShop()` - 关闭商店
- `BuyItem(string itemID, int quantity)` - 购买物品
- `SellItem(string itemID, int quantity)` - 出售物品
- `GetItemPrice(string itemID)` - 获取物品价格

## 使用示例

### 货币管理
```csharp
// 获取当前货币
int currentMoney = EconomyManager.GetCurrency();
Debug.Log($"当前货币: {currentMoney}");

// 添加货币
EconomyManager.AddCurrency(1000);
Debug.Log($"添加1000货币，当前货币: {EconomyManager.GetCurrency()}");

// 移除货币
EconomyManager.RemoveCurrency(500);
Debug.Log($"移除500货币，当前货币: {EconomyManager.GetCurrency()}");
```

### 物品解锁
```csharp
// 解锁物品
EconomyManager.UnlockItem("Weapon_AK47");
Debug.Log("解锁AK47武器");

// 检查物品是否已解锁
if (EconomyManager.IsItemUnlocked("Weapon_AK47"))
{
    Debug.Log("AK47武器已解锁");
}
else
{
    Debug.Log("AK47武器未解锁");
}

// 获取所有已解锁物品
List<string> unlockedItems = EconomyManager.GetUnlockedItems();
Debug.Log($"已解锁物品数量: {unlockedItems.Count}");
```

### 监听经济事件
```csharp
void OnEnable()
{
    EconomyManager.OnItemUnlocked += OnItemUnlocked;
    EconomyManager.OnCurrencyChanged += OnCurrencyChanged;
}

void OnDisable()
{
    EconomyManager.OnItemUnlocked -= OnItemUnlocked;
    EconomyManager.OnCurrencyChanged -= OnCurrencyChanged;
}

private void OnItemUnlocked(string itemID)
{
    Debug.Log($"物品解锁: {itemID}");
    // 显示解锁通知
    ShowUnlockNotification(itemID);
}

private void OnCurrencyChanged(int newAmount)
{
    Debug.Log($"货币改变: {newAmount}");
    // 更新UI显示
    UpdateCurrencyUI(newAmount);
}
```

### 商店系统
```csharp
StockShop shop = GetStockShop();

// 开放商店
shop.OpenShop();
Debug.Log("商店已开放");

// 检查商店状态
if (shop.IsOpen)
{
    Debug.Log("商店正在营业");
}

// 购买物品
bool purchaseSuccess = shop.BuyItem("Weapon_AK47", 1);
if (purchaseSuccess)
{
    Debug.Log("成功购买AK47");
}
else
{
    Debug.Log("购买失败，可能货币不足");
}

// 出售物品
bool sellSuccess = shop.SellItem("Weapon_AK47", 1);
if (sellSuccess)
{
    Debug.Log("成功出售AK47");
}

// 获取物品价格
int itemPrice = shop.GetItemPrice("Weapon_AK47");
Debug.Log($"AK47价格: {itemPrice}");
```

### 经济系统集成
```csharp
public class EconomySystem
{
    private EconomyManager economyManager;
    private StockShop stockShop;
    
    public EconomySystem()
    {
        economyManager = EconomyManager.Instance;
        stockShop = GetStockShop();
    }
    
    public bool TryPurchaseItem(string itemID, int quantity)
    {
        // 检查物品是否已解锁
        if (!economyManager.IsItemUnlocked(itemID))
        {
            Debug.Log($"物品未解锁: {itemID}");
            return false;
        }
        
        // 获取物品价格
        int itemPrice = stockShop.GetItemPrice(itemID);
        int totalCost = itemPrice * quantity;
        
        // 检查货币是否足够
        if (economyManager.GetCurrency() < totalCost)
        {
            Debug.Log($"货币不足，需要 {totalCost}，当前 {economyManager.GetCurrency()}");
            return false;
        }
        
        // 执行购买
        bool purchaseSuccess = stockShop.BuyItem(itemID, quantity);
        if (purchaseSuccess)
        {
            // 扣除货币
            economyManager.RemoveCurrency(totalCost);
            Debug.Log($"成功购买 {quantity} 个 {itemID}，花费 {totalCost} 货币");
            return true;
        }
        
        return false;
    }
    
    public bool TrySellItem(string itemID, int quantity)
    {
        // 执行出售
        bool sellSuccess = stockShop.SellItem(itemID, quantity);
        if (sellSuccess)
        {
            // 获取出售价格
            int itemPrice = stockShop.GetItemPrice(itemID);
            int totalEarned = itemPrice * quantity;
            
            // 添加货币
            economyManager.AddCurrency(totalEarned);
            Debug.Log($"成功出售 {quantity} 个 {itemID}，获得 {totalEarned} 货币");
            return true;
        }
        
        return false;
    }
}
```

### 物品解锁管理
```csharp
public class ItemUnlockManager
{
    private EconomyManager economyManager;
    
    public ItemUnlockManager()
    {
        economyManager = EconomyManager.Instance;
    }
    
    public void UnlockItemsByCategory(string category)
    {
        string[] items = GetItemsByCategory(category);
        foreach (string item in items)
        {
            if (!economyManager.IsItemUnlocked(item))
            {
                economyManager.UnlockItem(item);
                Debug.Log($"解锁物品: {item}");
            }
        }
    }
    
    public void UnlockAllWeapons()
    {
        string[] weapons = {
            "Weapon_AK47",
            "Weapon_M4A1",
            "Weapon_MP5",
            "Weapon_Shotgun",
            "Weapon_Sniper"
        };
        
        foreach (string weapon in weapons)
        {
            economyManager.UnlockItem(weapon);
        }
        
        Debug.Log("解锁所有武器");
    }
    
    public void UnlockAllArmor()
    {
        string[] armor = {
            "Armor_Vest",
            "Armor_Helmet",
            "Armor_FaceMask"
        };
        
        foreach (string item in armor)
        {
            economyManager.UnlockItem(item);
        }
        
        Debug.Log("解锁所有护甲");
    }
    
    private string[] GetItemsByCategory(string category)
    {
        // 根据分类获取物品列表
        // 这里需要根据实际游戏数据实现
        return new string[0];
    }
}
```

### 货币交易系统
```csharp
public class CurrencyTransaction
{
    private EconomyManager economyManager;
    
    public CurrencyTransaction()
    {
        economyManager = EconomyManager.Instance;
    }
    
    public bool TransferCurrency(int amount, string fromPlayer, string toPlayer)
    {
        // 检查发送方是否有足够货币
        if (economyManager.GetCurrency() < amount)
        {
            Debug.Log($"货币不足，无法转账 {amount}");
            return false;
        }
        
        // 扣除发送方货币
        economyManager.RemoveCurrency(amount);
        
        // 添加接收方货币
        economyManager.AddCurrency(amount);
        
        Debug.Log($"成功转账 {amount} 货币从 {fromPlayer} 到 {toPlayer}");
        return true;
    }
    
    public bool ExchangeCurrency(int amount, float exchangeRate)
    {
        int requiredAmount = Mathf.RoundToInt(amount * exchangeRate);
        
        if (economyManager.GetCurrency() < requiredAmount)
        {
            Debug.Log($"货币不足，无法兑换");
            return false;
        }
        
        economyManager.RemoveCurrency(requiredAmount);
        economyManager.AddCurrency(amount);
        
        Debug.Log($"成功兑换 {requiredAmount} 货币为 {amount} 货币");
        return true;
    }
}
```

### 经济统计系统
```csharp
public class EconomyStatistics
{
    private EconomyManager economyManager;
    private Dictionary<string, int> transactionHistory;
    
    public EconomyStatistics()
    {
        economyManager = EconomyManager.Instance;
        transactionHistory = new Dictionary<string, int>();
    }
    
    public void RecordTransaction(string itemID, int amount, bool isPurchase)
    {
        string key = isPurchase ? $"purchase_{itemID}" : $"sell_{itemID}";
        
        if (transactionHistory.ContainsKey(key))
        {
            transactionHistory[key] += amount;
        }
        else
        {
            transactionHistory[key] = amount;
        }
        
        Debug.Log($"记录交易: {key} - {amount}");
    }
    
    public int GetTotalSpent()
    {
        int total = 0;
        foreach (var kvp in transactionHistory)
        {
            if (kvp.Key.StartsWith("purchase_"))
            {
                total += kvp.Value;
            }
        }
        return total;
    }
    
    public int GetTotalEarned()
    {
        int total = 0;
        foreach (var kvp in transactionHistory)
        {
            if (kvp.Key.StartsWith("sell_"))
            {
                total += kvp.Value;
            }
        }
        return total;
    }
}
```

## 注意事项

1. 经济系统管理货币和物品解锁状态
2. 货币改变和物品解锁会触发相应事件
3. 商店系统需要检查货币是否足够
4. 物品解锁状态会保存在存档中
5. 经济系统与物品系统、制作系统紧密相关
6. 货币交易需要考虑安全性和验证
7. 经济统计有助于平衡游戏经济
8. 物品价格应该根据游戏平衡调整
9. 解锁通知应该及时显示给玩家
10. 经济系统的事件监听要正确管理，避免内存泄漏

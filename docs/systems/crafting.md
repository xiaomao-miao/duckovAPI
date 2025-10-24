# 制作系统

## 概述
制作系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的制作配方、材料管理和物品制作功能。

## 核心组件

### CraftingManager
制作管理器，负责管理游戏中的制作配方和制作系统。

#### 主要功能
- **配方管理** - 解锁、检查配方状态
- **物品制作** - 根据配方制作物品
- **材料检查** - 验证制作材料是否足够
- **事件通知** - 制作完成、配方解锁事件

#### 重要属性
- `Instance` - 获取CraftingManager实例
- `UnlockedFormulaIDs` - 已解锁的配方ID集合

#### 重要事件
- `OnItemCrafted` - 物品制作完成事件
- `OnFormulaUnlocked` - 配方解锁事件

#### 重要方法
- `UnlockFormula(string formulaID)` - 解锁配方
- `IsFormulaUnlocked(string value)` - 检查配方是否已解锁
- `GetFormula(string id)` - 获取配方
- `Craft(string id)` - 制作物品
- `Craft(CraftingFormula formula)` - 制作物品（配方）

### CraftingFormula
制作配方结构体，定义制作所需的材料和结果。

#### 重要属性
- `id` - 配方ID
- `result` - 制作结果
- `tags` - 标签
- `cost` - 制作成本
- `unlockByDefault` - 是否默认解锁
- `lockInDemo` - 演示版是否锁定
- `requirePerk` - 需要的技能点
- `hideInIndex` - 是否在索引中隐藏

#### 重要方法
- `IDValid` - 检查ID是否有效

### CraftingFormulaCollection
制作配方集合，管理所有制作配方。

#### 重要属性
- `Instance` - 获取实例
- `Entries` - 配方条目集合

#### 重要方法
- `TryGetFormula(string id, out CraftingFormula formula)` - 尝试获取配方

## 使用示例

### 解锁配方
```csharp
// 解锁单个配方
CraftingManager.UnlockFormula("Weapon_AK47");

// 解锁多个配方
string[] formulas = { "Weapon_AK47", "Armor_Vest", "Ammo_762x39" };
foreach (string formula in formulas)
{
    CraftingManager.UnlockFormula(formula);
}
```

### 检查配方状态
```csharp
// 检查配方是否已解锁
if (CraftingManager.IsFormulaUnlocked("Weapon_AK47"))
{
    Debug.Log("AK47配方已解锁");
}
else
{
    Debug.Log("AK47配方未解锁");
}

// 获取配方信息
CraftingFormula formula = CraftingManager.GetFormula("Weapon_AK47");
if (formula != null)
{
    Debug.Log($"配方ID: {formula.id}");
    Debug.Log($"制作结果: {formula.result}");
    Debug.Log($"制作成本: {formula.cost}");
}
```

### 制作物品
```csharp
// 检查配方是否解锁
if (CraftingManager.IsFormulaUnlocked("Weapon_AK47"))
{
    try
    {
        // 制作物品
        List<Item> craftedItems = await CraftingManager.Instance.Craft("Weapon_AK47");
        
        if (craftedItems != null && craftedItems.Count > 0)
        {
            Debug.Log($"成功制作 {craftedItems.Count} 个物品");
            foreach (Item item in craftedItems)
            {
                Debug.Log($"制作了: {item.DisplayName}");
            }
        }
        else
        {
            Debug.Log("制作失败，可能材料不足");
        }
    }
    catch (Exception e)
    {
        Debug.LogError($"制作过程中发生错误: {e.Message}");
    }
}
```

### 监听制作事件
```csharp
void OnEnable()
{
    CraftingManager.OnItemCrafted += OnItemCrafted;
    CraftingManager.OnFormulaUnlocked += OnFormulaUnlocked;
}

void OnDisable()
{
    CraftingManager.OnItemCrafted -= OnItemCrafted;
    CraftingManager.OnFormulaUnlocked -= OnFormulaUnlocked;
}

private void OnItemCrafted(Item craftedItem)
{
    Debug.Log($"物品制作完成: {craftedItem.DisplayName}");
}

private void OnFormulaUnlocked(string formulaID)
{
    Debug.Log($"配方解锁: {formulaID}");
}
```

### 获取配方信息
```csharp
// 通过配方集合获取配方
if (CraftingFormulaCollection.Instance.TryGetFormula("Weapon_AK47", out CraftingFormula formula))
{
    Debug.Log($"配方ID: {formula.id}");
    Debug.Log($"制作结果: {formula.result}");
    Debug.Log($"默认解锁: {formula.unlockByDefault}");
    Debug.Log($"需要技能点: {formula.requirePerk}");
    Debug.Log($"在索引中隐藏: {formula.hideInIndex}");
    
    // 检查配方ID是否有效
    if (formula.IDValid)
    {
        Debug.Log("配方ID有效");
    }
}
```

### 批量解锁配方
```csharp
public void UnlockAllWeaponFormulas()
{
    string[] weaponFormulas = {
        "Weapon_AK47",
        "Weapon_M4A1",
        "Weapon_MP5",
        "Weapon_Shotgun",
        "Weapon_Sniper"
    };
    
    foreach (string formula in weaponFormulas)
    {
        if (!CraftingManager.IsFormulaUnlocked(formula))
        {
            CraftingManager.UnlockFormula(formula);
            Debug.Log($"解锁配方: {formula}");
        }
    }
}
```

### 检查制作材料
```csharp
public bool CheckCraftingMaterials(string formulaID)
{
    CraftingFormula formula = CraftingManager.GetFormula(formulaID);
    if (formula == null)
    {
        Debug.LogError($"配方不存在: {formulaID}");
        return false;
    }
    
    // 检查背包中是否有足够的材料
    Inventory inventory = GetPlayerInventory();
    foreach (var material in formula.cost)
    {
        if (!inventory.HasItem(material.itemID, material.quantity))
        {
            Debug.Log($"材料不足: {material.itemID} 需要 {material.quantity}");
            return false;
        }
    }
    
    return true;
}
```

### 制作系统集成
```csharp
public class CraftingSystem
{
    private CraftingManager craftingManager;
    
    public CraftingSystem()
    {
        craftingManager = CraftingManager.Instance;
    }
    
    public async Task<bool> TryCraftItem(string formulaID)
    {
        // 检查配方是否解锁
        if (!craftingManager.IsFormulaUnlocked(formulaID))
        {
            Debug.Log($"配方未解锁: {formulaID}");
            return false;
        }
        
        // 检查材料是否足够
        if (!CheckCraftingMaterials(formulaID))
        {
            Debug.Log($"材料不足: {formulaID}");
            return false;
        }
        
        try
        {
            // 执行制作
            List<Item> craftedItems = await craftingManager.Craft(formulaID);
            
            if (craftedItems != null && craftedItems.Count > 0)
            {
                Debug.Log($"成功制作 {craftedItems.Count} 个物品");
                return true;
            }
            else
            {
                Debug.Log("制作失败");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"制作过程中发生错误: {e.Message}");
            return false;
        }
    }
    
    public void UnlockFormulasByCategory(string category)
    {
        // 根据分类解锁配方
        string[] formulas = GetFormulasByCategory(category);
        foreach (string formula in formulas)
        {
            craftingManager.UnlockFormula(formula);
        }
    }
}
```

### 配方搜索和过滤
```csharp
public class RecipeSearch
{
    public List<CraftingFormula> SearchRecipes(string searchTerm)
    {
        List<CraftingFormula> results = new List<CraftingFormula>();
        
        // 搜索所有配方
        foreach (var entry in CraftingFormulaCollection.Instance.Entries)
        {
            if (entry.id.Contains(searchTerm) || 
                entry.result.Contains(searchTerm) ||
                entry.tags.Any(tag => tag.Contains(searchTerm)))
            {
                results.Add(entry);
            }
        }
        
        return results;
    }
    
    public List<CraftingFormula> GetUnlockedRecipes()
    {
        List<CraftingFormula> unlocked = new List<CraftingFormula>();
        
        foreach (var entry in CraftingFormulaCollection.Instance.Entries)
        {
            if (CraftingManager.IsFormulaUnlocked(entry.id))
            {
                unlocked.Add(entry);
            }
        }
        
        return unlocked;
    }
}
```

### 制作进度跟踪
```csharp
public class CraftingProgress
{
    private Dictionary<string, float> progressMap;
    
    public CraftingProgress()
    {
        progressMap = new Dictionary<string, float>();
    }
    
    public void StartCrafting(string formulaID)
    {
        progressMap[formulaID] = 0f;
        Debug.Log($"开始制作: {formulaID}");
    }
    
    public void UpdateProgress(string formulaID, float progress)
    {
        if (progressMap.ContainsKey(formulaID))
        {
            progressMap[formulaID] = Mathf.Clamp01(progress);
            Debug.Log($"制作进度: {formulaID} - {progressMap[formulaID] * 100}%");
        }
    }
    
    public float GetProgress(string formulaID)
    {
        return progressMap.ContainsKey(formulaID) ? progressMap[formulaID] : 0f;
    }
}
```

## 注意事项

1. 制作系统需要先解锁配方才能制作物品
2. 制作配方的解锁状态会保存在存档中
3. 制作过程中需要检查材料是否足够
4. 制作是异步操作，需要使用await等待完成
5. 制作失败时可能抛出异常，需要适当处理
6. 配方ID必须与游戏中的实际配方ID匹配
7. 制作结果可能包含多个物品，需要遍历处理
8. 事件监听后记得在OnDisable中取消订阅
9. 制作系统与背包系统紧密相关，需要检查背包容量
10. 某些配方可能需要特定的技能点或条件才能解锁

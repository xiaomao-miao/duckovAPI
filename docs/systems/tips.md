# 提示系统

## 概述
提示系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的提示信息显示、分类管理、随机显示等功能。

## 核心组件

### TipsDisplay
提示显示系统，管理游戏中的提示信息。

#### 主要功能
- **提示显示** - 显示提示信息
- **随机提示** - 显示随机提示
- **提示管理** - 管理提示生命周期
- **隐藏控制** - 隐藏提示信息

#### 重要方法
- `DisplayRandom()` - 显示随机提示
- `Display(string tipID)` - 显示指定提示
- `Hide()` - 隐藏提示
- `Show()` - 显示提示

### TipEntry
提示条目，表示单个提示信息。

#### 重要属性
- `TipID` - 提示ID
- `Description` - 提示描述
- `Category` - 提示分类

## 使用示例

### 基本提示显示
```csharp
// 显示随机提示
TipsDisplay.DisplayRandom();
Debug.Log("显示随机提示");

// 显示指定提示
TipsDisplay.Display("Tip_Combat_01");
Debug.Log("显示战斗提示01");

// 隐藏提示
TipsDisplay.Hide();
Debug.Log("隐藏提示");

// 显示提示
TipsDisplay.Show();
Debug.Log("显示提示");
```

### 提示系统集成
```csharp
public class TipSystem
{
    private TipsDisplay tipsDisplay;
    private Dictionary<string, TipEntry> tipDatabase;
    private List<string> displayedTips;
    
    public TipSystem()
    {
        tipsDisplay = GetComponent<TipsDisplay>();
        tipDatabase = new Dictionary<string, TipEntry>();
        displayedTips = new List<string>();
        InitializeTipDatabase();
    }
    
    public void ShowRandomTip()
    {
        string randomTipID = GetRandomTipID();
        if (!string.IsNullOrEmpty(randomTipID))
        {
            tipsDisplay.Display(randomTipID);
            displayedTips.Add(randomTipID);
            Debug.Log($"显示随机提示: {randomTipID}");
        }
    }
    
    public void ShowTipByCategory(string category)
    {
        string tipID = GetTipByCategory(category);
        if (!string.IsNullOrEmpty(tipID))
        {
            tipsDisplay.Display(tipID);
            displayedTips.Add(tipID);
            Debug.Log($"显示分类提示: {tipID}");
        }
    }
    
    public void ShowTipByID(string tipID)
    {
        if (tipDatabase.ContainsKey(tipID))
        {
            tipsDisplay.Display(tipID);
            displayedTips.Add(tipID);
            Debug.Log($"显示指定提示: {tipID}");
        }
        else
        {
            Debug.LogWarning($"提示不存在: {tipID}");
        }
    }
    
    public void HideCurrentTip()
    {
        tipsDisplay.Hide();
        Debug.Log("隐藏当前提示");
    }
    
    private void InitializeTipDatabase()
    {
        // 初始化提示数据库
        tipDatabase["Tip_Combat_01"] = new TipEntry
        {
            TipID = "Tip_Combat_01",
            Description = "使用掩体可以避免受到伤害",
            Category = "Combat"
        };
        
        tipDatabase["Tip_Combat_02"] = new TipEntry
        {
            TipID = "Tip_Combat_02",
            Description = "瞄准头部可以造成更多伤害",
            Category = "Combat"
        };
        
        tipDatabase["Tip_Survival_01"] = new TipEntry
        {
            TipID = "Tip_Survival_01",
            Description = "保持水分和食物可以维持生命",
            Category = "Survival"
        };
        
        tipDatabase["Tip_Inventory_01"] = new TipEntry
        {
            TipID = "Tip_Inventory_01",
            Description = "整理背包可以提高效率",
            Category = "Inventory"
        };
    }
    
    private string GetRandomTipID()
    {
        List<string> availableTips = tipDatabase.Keys.ToList();
        availableTips = availableTips.Where(tip => !displayedTips.Contains(tip)).ToList();
        
        if (availableTips.Count > 0)
        {
            return availableTips[UnityEngine.Random.Range(0, availableTips.Count)];
        }
        
        return null;
    }
    
    private string GetTipByCategory(string category)
    {
        List<string> categoryTips = tipDatabase.Values
            .Where(tip => tip.Category == category)
            .Select(tip => tip.TipID)
            .ToList();
        
        categoryTips = categoryTips.Where(tip => !displayedTips.Contains(tip)).ToList();
        
        if (categoryTips.Count > 0)
        {
            return categoryTips[UnityEngine.Random.Range(0, categoryTips.Count)];
        }
        
        return null;
    }
}
```

### 提示分类系统
```csharp
public class TipCategorySystem
{
    private Dictionary<string, List<TipEntry>> categoryTips;
    
    public TipCategorySystem()
    {
        categoryTips = new Dictionary<string, List<TipEntry>>();
        InitializeCategories();
    }
    
    public void ShowTipByCategory(string category)
    {
        if (categoryTips.ContainsKey(category))
        {
            List<TipEntry> tips = categoryTips[category];
            if (tips.Count > 0)
            {
                TipEntry randomTip = tips[UnityEngine.Random.Range(0, tips.Count)];
                ShowTip(randomTip);
            }
        }
    }
    
    public void ShowTipByCategoryAndPriority(string category, int priority)
    {
        if (categoryTips.ContainsKey(category))
        {
            List<TipEntry> tips = categoryTips[category]
                .Where(tip => tip.Priority >= priority)
                .ToList();
            
            if (tips.Count > 0)
            {
                TipEntry randomTip = tips[UnityEngine.Random.Range(0, tips.Count)];
                ShowTip(randomTip);
            }
        }
    }
    
    public List<string> GetAvailableCategories()
    {
        return categoryTips.Keys.ToList();
    }
    
    public int GetCategoryTipCount(string category)
    {
        return categoryTips.ContainsKey(category) ? categoryTips[category].Count : 0;
    }
    
    private void InitializeCategories()
    {
        // 初始化分类提示
        categoryTips["Combat"] = new List<TipEntry>();
        categoryTips["Survival"] = new List<TipEntry>();
        categoryTips["Inventory"] = new List<TipEntry>();
        categoryTips["Crafting"] = new List<TipEntry>();
        categoryTips["Navigation"] = new List<TipEntry>();
    }
    
    private void ShowTip(TipEntry tip)
    {
        // 显示提示
        Debug.Log($"显示提示: {tip.TipID} - {tip.Description}");
    }
}
```

### 提示优先级系统
```csharp
public class TipPrioritySystem
{
    private Dictionary<string, int> tipPriorities;
    private Dictionary<string, int> tipDisplayCount;
    
    public TipPrioritySystem()
    {
        tipPriorities = new Dictionary<string, int>();
        tipDisplayCount = new Dictionary<string, int>();
        InitializePriorities();
    }
    
    public void ShowHighPriorityTip()
    {
        string highPriorityTip = GetHighestPriorityTip();
        if (!string.IsNullOrEmpty(highPriorityTip))
        {
            ShowTip(highPriorityTip);
            IncrementDisplayCount(highPriorityTip);
        }
    }
    
    public void ShowTipByPriority(int minPriority)
    {
        string tip = GetTipByPriority(minPriority);
        if (!string.IsNullOrEmpty(tip))
        {
            ShowTip(tip);
            IncrementDisplayCount(tip);
        }
    }
    
    public void AdjustTipPriority(string tipID, int newPriority)
    {
        if (tipPriorities.ContainsKey(tipID))
        {
            tipPriorities[tipID] = newPriority;
            Debug.Log($"调整提示优先级: {tipID} -> {newPriority}");
        }
    }
    
    public int GetTipDisplayCount(string tipID)
    {
        return tipDisplayCount.ContainsKey(tipID) ? tipDisplayCount[tipID] : 0;
    }
    
    private void InitializePriorities()
    {
        // 初始化提示优先级
        tipPriorities["Tip_Combat_01"] = 5;
        tipPriorities["Tip_Combat_02"] = 4;
        tipPriorities["Tip_Survival_01"] = 3;
        tipPriorities["Tip_Inventory_01"] = 2;
        tipPriorities["Tip_Crafting_01"] = 1;
    }
    
    private string GetHighestPriorityTip()
    {
        int maxPriority = tipPriorities.Values.Max();
        List<string> highPriorityTips = tipPriorities
            .Where(kvp => kvp.Value == maxPriority)
            .Select(kvp => kvp.Key)
            .ToList();
        
        if (highPriorityTips.Count > 0)
        {
            return highPriorityTips[UnityEngine.Random.Range(0, highPriorityTips.Count)];
        }
        
        return null;
    }
    
    private string GetTipByPriority(int minPriority)
    {
        List<string> availableTips = tipPriorities
            .Where(kvp => kvp.Value >= minPriority)
            .Select(kvp => kvp.Key)
            .ToList();
        
        if (availableTips.Count > 0)
        {
            return availableTips[UnityEngine.Random.Range(0, availableTips.Count)];
        }
        
        return null;
    }
    
    private void ShowTip(string tipID)
    {
        // 显示提示
        Debug.Log($"显示提示: {tipID}");
    }
    
    private void IncrementDisplayCount(string tipID)
    {
        if (tipDisplayCount.ContainsKey(tipID))
        {
            tipDisplayCount[tipID]++;
        }
        else
        {
            tipDisplayCount[tipID] = 1;
        }
    }
}
```

### 提示条件系统
```csharp
public class TipConditionSystem
{
    private Dictionary<string, System.Func<bool>> tipConditions;
    
    public TipConditionSystem()
    {
        tipConditions = new Dictionary<string, System.Func<bool>>();
        InitializeConditions();
    }
    
    public void ShowConditionalTip(string tipID)
    {
        if (tipConditions.ContainsKey(tipID))
        {
            if (tipConditions[tipID].Invoke())
            {
                ShowTip(tipID);
            }
            else
            {
                Debug.Log($"提示条件不满足: {tipID}");
            }
        }
        else
        {
            ShowTip(tipID);
        }
    }
    
    public void ShowTipsByCondition(string conditionType)
    {
        List<string> conditionalTips = tipConditions
            .Where(kvp => kvp.Key.Contains(conditionType))
            .Where(kvp => kvp.Value.Invoke())
            .Select(kvp => kvp.Key)
            .ToList();
        
        if (conditionalTips.Count > 0)
        {
            string randomTip = conditionalTips[UnityEngine.Random.Range(0, conditionalTips.Count)];
            ShowTip(randomTip);
        }
    }
    
    private void InitializeConditions()
    {
        // 初始化提示条件
        tipConditions["Tip_Combat_01"] = () => IsInCombat();
        tipConditions["Tip_Survival_01"] = () => IsLowHealth();
        tipConditions["Tip_Inventory_01"] = () => IsInventoryFull();
        tipConditions["Tip_Crafting_01"] = () => HasCraftingMaterials();
    }
    
    private bool IsInCombat()
    {
        // 检查是否在战斗中
        return false; // 示例
    }
    
    private bool IsLowHealth()
    {
        // 检查生命值是否较低
        CharacterMainControl character = CharacterMainControl.Main;
        if (character != null)
        {
            return character.Health.CurrentHealth < character.Health.MaxHealth * 0.3f;
        }
        return false;
    }
    
    private bool IsInventoryFull()
    {
        // 检查背包是否已满
        return false; // 示例
    }
    
    private bool HasCraftingMaterials()
    {
        // 检查是否有制作材料
        return false; // 示例
    }
    
    private void ShowTip(string tipID)
    {
        // 显示提示
        Debug.Log($"显示条件提示: {tipID}");
    }
}
```

### 提示统计系统
```csharp
public class TipStatisticsSystem
{
    private Dictionary<string, int> tipDisplayStats;
    private Dictionary<string, float> tipEffectiveness;
    
    public TipStatisticsSystem()
    {
        tipDisplayStats = new Dictionary<string, int>();
        tipEffectiveness = new Dictionary<string, float>();
    }
    
    public void RecordTipDisplay(string tipID)
    {
        if (tipDisplayStats.ContainsKey(tipID))
        {
            tipDisplayStats[tipID]++;
        }
        else
        {
            tipDisplayStats[tipID] = 1;
        }
        
        Debug.Log($"记录提示显示: {tipID} - {tipDisplayStats[tipID]}");
    }
    
    public void RecordTipEffectiveness(string tipID, float effectiveness)
    {
        tipEffectiveness[tipID] = effectiveness;
        Debug.Log($"记录提示效果: {tipID} - {effectiveness}");
    }
    
    public int GetTipDisplayCount(string tipID)
    {
        return tipDisplayStats.ContainsKey(tipID) ? tipDisplayStats[tipID] : 0;
    }
    
    public float GetTipEffectiveness(string tipID)
    {
        return tipEffectiveness.ContainsKey(tipID) ? tipEffectiveness[tipID] : 0f;
    }
    
    public string GetMostDisplayedTip()
    {
        if (tipDisplayStats.Count > 0)
        {
            return tipDisplayStats.OrderByDescending(kvp => kvp.Value).First().Key;
        }
        return null;
    }
    
    public string GetMostEffectiveTip()
    {
        if (tipEffectiveness.Count > 0)
        {
            return tipEffectiveness.OrderByDescending(kvp => kvp.Value).First().Key;
        }
        return null;
    }
    
    public void SaveTipStatistics()
    {
        SavesSystem.Save("TipDisplayStats", tipDisplayStats);
        SavesSystem.Save("TipEffectiveness", tipEffectiveness);
        Debug.Log("提示统计已保存");
    }
    
    public void LoadTipStatistics()
    {
        tipDisplayStats = SavesSystem.Load("TipDisplayStats", new Dictionary<string, int>());
        tipEffectiveness = SavesSystem.Load("TipEffectiveness", new Dictionary<string, float>());
        Debug.Log("提示统计已加载");
    }
}
```

## 注意事项

1. 提示系统支持分类和随机显示
2. 提示优先级系统确保重要提示优先显示
3. 提示条件系统根据游戏状态显示相关提示
4. 提示统计系统帮助优化提示效果
5. 提示系统与游戏状态紧密相关
6. 提示显示需要考虑用户体验和时机
7. 提示内容应该简洁明了，易于理解
8. 提示系统需要支持多语言
9. 提示统计有助于改进游戏设计
10. 提示系统应该避免重复显示相同内容

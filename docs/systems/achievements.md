# 成就系统

## 概述
成就系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的成就解锁、统计管理和进度跟踪等功能。

## 核心组件

### AchievementManager
成就管理器，管理游戏中的成就解锁和统计。

#### 主要功能
- **成就管理** - 解锁成就、检查解锁状态
- **进度跟踪** - 成就进度监控
- **统计管理** - 游戏统计数据
- **事件通知** - 成就解锁、数据加载事件

#### 重要属性
- `Instance` - 获取AchievementManager实例
- `CanUnlockAchievement` - 是否可以解锁成就
- `UnlockedAchievements` - 已解锁的成就列表

#### 重要事件
- `OnAchievementUnlocked` - 成就解锁事件
- `OnAchievementDataLoaded` - 成就数据加载事件

#### 重要方法
- `UnlockAchievement(string id)` - 解锁成就
- `IsAchievementUnlocked(string id)` - 检查成就是否已解锁
- `GetAchievementProgress(string id)` - 获取成就进度

### StatisticsManager
统计管理器，管理游戏统计数据。

#### 主要功能
- **统计管理** - 增加、设置、获取统计值
- **数据重置** - 重置所有统计
- **进度跟踪** - 统计进度监控

#### 重要属性
- `Instance` - 获取StatisticsManager实例

#### 重要方法
- `IncrementStat(string statName, int value)` - 增加统计值
- `SetStat(string statName, int value)` - 设置统计值
- `GetStat(string statName)` - 获取统计值
- `ResetAllStats()` - 重置所有统计

## 使用示例

### 成就管理
```csharp
// 解锁成就
AchievementManager.UnlockAchievement("Kill_100_Enemies");
Debug.Log("解锁成就: 击杀100个敌人");

// 检查成就是否已解锁
if (AchievementManager.IsAchievementUnlocked("Kill_100_Enemies"))
{
    Debug.Log("击杀100个敌人成就已解锁");
}
else
{
    Debug.Log("击杀100个敌人成就未解锁");
}

// 获取成就进度
float progress = AchievementManager.GetAchievementProgress("Kill_100_Enemies");
Debug.Log($"成就进度: {progress * 100}%");
```

### 统计管理
```csharp
// 增加统计值
StatisticsManager.IncrementStat("EnemiesKilled", 1);
Debug.Log("敌人击杀数 +1");

// 设置统计值
StatisticsManager.SetStat("PlayerLevel", 5);
Debug.Log("设置玩家等级为5");

// 获取统计值
int enemiesKilled = StatisticsManager.GetStat("EnemiesKilled");
int playerLevel = StatisticsManager.GetStat("PlayerLevel");
Debug.Log($"击杀敌人数: {enemiesKilled}");
Debug.Log($"玩家等级: {playerLevel}");

// 重置所有统计
StatisticsManager.ResetAllStats();
Debug.Log("重置所有统计");
```

### 监听成就事件
```csharp
void OnEnable()
{
    AchievementManager.OnAchievementUnlocked += OnAchievementUnlocked;
    AchievementManager.OnAchievementDataLoaded += OnAchievementDataLoaded;
}

void OnDisable()
{
    AchievementManager.OnAchievementUnlocked -= OnAchievementUnlocked;
    AchievementManager.OnAchievementDataLoaded -= OnAchievementDataLoaded;
}

private void OnAchievementUnlocked(string achievementID)
{
    Debug.Log($"成就解锁: {achievementID}");
    // 显示成就解锁通知
    ShowAchievementNotification(achievementID);
}

private void OnAchievementDataLoaded()
{
    Debug.Log("成就数据加载完成");
    // 更新成就UI
    UpdateAchievementUI();
}
```

### 成就系统集成
```csharp
public class AchievementSystem
{
    private AchievementManager achievementManager;
    private StatisticsManager statisticsManager;
    
    public AchievementSystem()
    {
        achievementManager = AchievementManager.Instance;
        statisticsManager = StatisticsManager.Instance;
    }
    
    public void CheckKillAchievements()
    {
        int enemiesKilled = statisticsManager.GetStat("EnemiesKilled");
        
        // 检查击杀成就
        if (enemiesKilled >= 10 && !achievementManager.IsAchievementUnlocked("Kill_10_Enemies"))
        {
            achievementManager.UnlockAchievement("Kill_10_Enemies");
        }
        
        if (enemiesKilled >= 100 && !achievementManager.IsAchievementUnlocked("Kill_100_Enemies"))
        {
            achievementManager.UnlockAchievement("Kill_100_Enemies");
        }
        
        if (enemiesKilled >= 1000 && !achievementManager.IsAchievementUnlocked("Kill_1000_Enemies"))
        {
            achievementManager.UnlockAchievement("Kill_1000_Enemies");
        }
    }
    
    public void CheckLevelAchievements()
    {
        int playerLevel = statisticsManager.GetStat("PlayerLevel");
        
        // 检查等级成就
        if (playerLevel >= 5 && !achievementManager.IsAchievementUnlocked("Reach_Level_5"))
        {
            achievementManager.UnlockAchievement("Reach_Level_5");
        }
        
        if (playerLevel >= 10 && !achievementManager.IsAchievementUnlocked("Reach_Level_10"))
        {
            achievementManager.UnlockAchievement("Reach_Level_10");
        }
    }
    
    public void CheckItemAchievements()
    {
        int itemsCollected = statisticsManager.GetStat("ItemsCollected");
        
        // 检查收集成就
        if (itemsCollected >= 50 && !achievementManager.IsAchievementUnlocked("Collect_50_Items"))
        {
            achievementManager.UnlockAchievement("Collect_50_Items");
        }
    }
}
```

### 统计跟踪系统
```csharp
public class StatisticsTracker
{
    private StatisticsManager statisticsManager;
    private Dictionary<string, int> statHistory;
    
    public StatisticsTracker()
    {
        statisticsManager = StatisticsManager.Instance;
        statHistory = new Dictionary<string, int>();
    }
    
    public void TrackEnemyKill()
    {
        statisticsManager.IncrementStat("EnemiesKilled", 1);
        UpdateStatHistory("EnemiesKilled", 1);
        Debug.Log("记录敌人击杀");
    }
    
    public void TrackItemCollected()
    {
        statisticsManager.IncrementStat("ItemsCollected", 1);
        UpdateStatHistory("ItemsCollected", 1);
        Debug.Log("记录物品收集");
    }
    
    public void TrackLevelUp(int newLevel)
    {
        statisticsManager.SetStat("PlayerLevel", newLevel);
        UpdateStatHistory("PlayerLevel", newLevel);
        Debug.Log($"记录等级提升: {newLevel}");
    }
    
    private void UpdateStatHistory(string statName, int value)
    {
        if (statHistory.ContainsKey(statName))
        {
            statHistory[statName] += value;
        }
        else
        {
            statHistory[statName] = value;
        }
    }
    
    public int GetStatHistory(string statName)
    {
        return statHistory.ContainsKey(statName) ? statHistory[statName] : 0;
    }
}
```

### 成就进度系统
```csharp
public class AchievementProgressSystem
{
    private AchievementManager achievementManager;
    private StatisticsManager statisticsManager;
    
    public AchievementProgressSystem()
    {
        achievementManager = AchievementManager.Instance;
        statisticsManager = StatisticsManager.Instance;
    }
    
    public float GetAchievementProgress(string achievementID)
    {
        switch (achievementID)
        {
            case "Kill_100_Enemies":
                return GetKillProgress(100);
            case "Collect_50_Items":
                return GetCollectProgress(50);
            case "Reach_Level_10":
                return GetLevelProgress(10);
            default:
                return 0f;
        }
    }
    
    private float GetKillProgress(int target)
    {
        int current = statisticsManager.GetStat("EnemiesKilled");
        return Mathf.Clamp01((float)current / target);
    }
    
    private float GetCollectProgress(int target)
    {
        int current = statisticsManager.GetStat("ItemsCollected");
        return Mathf.Clamp01((float)current / target);
    }
    
    private float GetLevelProgress(int target)
    {
        int current = statisticsManager.GetStat("PlayerLevel");
        return Mathf.Clamp01((float)current / target);
    }
    
    public void CheckAllAchievements()
    {
        string[] achievements = {
            "Kill_10_Enemies",
            "Kill_100_Enemies",
            "Kill_1000_Enemies",
            "Collect_10_Items",
            "Collect_50_Items",
            "Collect_100_Items",
            "Reach_Level_5",
            "Reach_Level_10",
            "Reach_Level_20"
        };
        
        foreach (string achievement in achievements)
        {
            if (!achievementManager.IsAchievementUnlocked(achievement))
            {
                float progress = GetAchievementProgress(achievement);
                if (progress >= 1f)
                {
                    achievementManager.UnlockAchievement(achievement);
                }
            }
        }
    }
}
```

### 成就通知系统
```csharp
public class AchievementNotificationSystem
{
    private AchievementManager achievementManager;
    
    public AchievementNotificationSystem()
    {
        achievementManager = AchievementManager.Instance;
    }
    
    public void ShowAchievementNotification(string achievementID)
    {
        string title = GetAchievementTitle(achievementID);
        string description = GetAchievementDescription(achievementID);
        
        // 显示成就通知
        Debug.Log($"成就解锁: {title}");
        Debug.Log($"描述: {description}");
        
        // 播放音效
        AudioManager.PlaySound("AchievementUnlocked");
        
        // 显示UI通知
        ShowAchievementUI(title, description);
    }
    
    private string GetAchievementTitle(string achievementID)
    {
        switch (achievementID)
        {
            case "Kill_100_Enemies":
                return "敌人杀手";
            case "Collect_50_Items":
                return "收集家";
            case "Reach_Level_10":
                return "等级大师";
            default:
                return "未知成就";
        }
    }
    
    private string GetAchievementDescription(string achievementID)
    {
        switch (achievementID)
        {
            case "Kill_100_Enemies":
                return "击杀100个敌人";
            case "Collect_50_Items":
                return "收集50个物品";
            case "Reach_Level_10":
                return "达到10级";
            default:
                return "未知描述";
        }
    }
    
    private void ShowAchievementUI(string title, string description)
    {
        // 显示成就UI
        // 这里需要根据实际UI系统实现
    }
}
```

### 成就数据管理
```csharp
public class AchievementDataManager
{
    private AchievementManager achievementManager;
    private StatisticsManager statisticsManager;
    
    public AchievementDataManager()
    {
        achievementManager = AchievementManager.Instance;
        statisticsManager = StatisticsManager.Instance;
    }
    
    public void SaveAchievementData()
    {
        // 保存成就数据到存档
        List<string> unlockedAchievements = achievementManager.GetUnlockedAchievements();
        SavesSystem.Save("UnlockedAchievements", unlockedAchievements);
        
        // 保存统计数据
        Dictionary<string, int> stats = GetAllStats();
        SavesSystem.Save("Statistics", stats);
        
        Debug.Log("成就数据已保存");
    }
    
    public void LoadAchievementData()
    {
        // 加载成就数据
        List<string> unlockedAchievements = SavesSystem.Load("UnlockedAchievements", new List<string>());
        foreach (string achievement in unlockedAchievements)
        {
            if (!achievementManager.IsAchievementUnlocked(achievement))
            {
                achievementManager.UnlockAchievement(achievement);
            }
        }
        
        // 加载统计数据
        Dictionary<string, int> stats = SavesSystem.Load("Statistics", new Dictionary<string, int>());
        foreach (var kvp in stats)
        {
            statisticsManager.SetStat(kvp.Key, kvp.Value);
        }
        
        Debug.Log("成就数据已加载");
    }
    
    private Dictionary<string, int> GetAllStats()
    {
        // 获取所有统计数据
        return new Dictionary<string, int>();
    }
}
```

## 注意事项

1. 成就系统需要用户认证才能解锁
2. 成就解锁会触发相应事件和通知
3. 统计数据会实时更新，影响成就进度
4. 成就数据会保存在存档中
5. 成就系统与统计系统紧密相关
6. 事件监听后记得在OnDisable中取消订阅
7. 成就通知应该及时显示给玩家
8. 成就进度需要实时计算和更新
9. 成就系统与任务系统、经济系统相关
10. 成就数据管理需要正确处理保存和加载

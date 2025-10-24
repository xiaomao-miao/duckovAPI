# 任务系统

## 概述
任务系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的任务、进度跟踪、奖励发放等功能。

## 核心组件

### QuestManager
任务管理器，管理游戏中的任务系统。

#### 主要功能
- **任务管理** - 激活、完成任务
- **进度跟踪** - 任务进度监控
- **状态检查** - 任务状态查询
- **事件通知** - 任务激活、完成事件

#### 重要属性
- `Instance` - 获取QuestManager实例
- `AnyQuestNeedsInspection` - 是否有任务需要检查
- `TaskFinishNotificationFormat` - 任务完成通知格式

#### 重要事件
- `OnQuestActivated` - 任务激活事件
- `OnQuestCompleted` - 任务完成事件
- `OnTaskCompleted` - 任务完成事件

#### 重要方法
- `ActivateQuest(string questID)` - 激活任务
- `CompleteQuest(string questID)` - 完成任务
- `GetActiveQuests()` - 获取活动任务
- `GetCompletedQuests()` - 获取已完成任务
- `IsQuestActive(string questID)` - 检查任务是否激活
- `IsQuestCompleted(string questID)` - 检查任务是否完成

### Quest
任务类，表示单个任务。

#### 主要功能
- **任务管理** - 激活、完成任务
- **任务信息** - 标题、描述、奖励
- **状态跟踪** - 激活状态、完成状态

#### 重要属性
- `QuestID` - 任务ID
- `Title` - 任务标题
- `Description` - 任务描述
- `IsActive` - 是否激活
- `IsCompleted` - 是否完成
- `Tasks` - 任务列表
- `Rewards` - 奖励列表

#### 重要方法
- `Activate()` - 激活任务
- `Complete()` - 完成任务
- `AddTask(Task task)` - 添加任务
- `RemoveTask(Task task)` - 移除任务

### Task
任务类，表示任务中的单个任务。

#### 主要功能
- **任务执行** - 开始、完成任务
- **进度管理** - 任务进度跟踪
- **状态检查** - 完成状态查询

#### 重要属性
- `TaskID` - 任务ID
- `Description` - 任务描述
- `IsCompleted` - 是否完成
- `Progress` - 进度

#### 重要方法
- `Complete()` - 完成任务
- `UpdateProgress(int progress)` - 更新进度

## 使用示例

### 任务管理
```csharp
// 激活任务
QuestManager.ActivateQuest("MainQuest_01");
Debug.Log("激活主任务01");

// 检查任务状态
if (QuestManager.IsQuestActive("MainQuest_01"))
{
    Debug.Log("主任务01正在进行中");
}

// 完成任务
QuestManager.CompleteQuest("MainQuest_01");
Debug.Log("完成主任务01");

// 检查任务是否完成
if (QuestManager.IsQuestCompleted("MainQuest_01"))
{
    Debug.Log("主任务01已完成");
}
```

### 获取任务列表
```csharp
// 获取活动任务
List<string> activeQuests = QuestManager.GetActiveQuests();
Debug.Log($"活动任务数量: {activeQuests.Count}");
foreach (string quest in activeQuests)
{
    Debug.Log($"活动任务: {quest}");
}

// 获取已完成任务
List<string> completedQuests = QuestManager.GetCompletedQuests();
Debug.Log($"已完成任务数量: {completedQuests.Count}");
foreach (string quest in completedQuests)
{
    Debug.Log($"已完成任务: {quest}");
}
```

### 监听任务事件
```csharp
void OnEnable()
{
    QuestManager.OnQuestActivated += OnQuestActivated;
    QuestManager.OnQuestCompleted += OnQuestCompleted;
    QuestManager.OnTaskCompleted += OnTaskCompleted;
}

void OnDisable()
{
    QuestManager.OnQuestActivated -= OnQuestActivated;
    QuestManager.OnQuestCompleted -= OnQuestCompleted;
    QuestManager.OnTaskCompleted -= OnTaskCompleted;
}

private void OnQuestActivated(string questID)
{
    Debug.Log($"任务激活: {questID}");
    // 显示任务激活通知
    ShowQuestNotification($"任务激活: {questID}");
}

private void OnQuestCompleted(string questID)
{
    Debug.Log($"任务完成: {questID}");
    // 显示任务完成通知
    ShowQuestNotification($"任务完成: {questID}");
}

private void OnTaskCompleted(string taskID)
{
    Debug.Log($"任务完成: {taskID}");
    // 显示任务完成通知
    ShowTaskNotification($"任务完成: {taskID}");
}
```

### 任务系统集成
```csharp
public class QuestSystem
{
    private QuestManager questManager;
    
    public QuestSystem()
    {
        questManager = QuestManager.Instance;
    }
    
    public bool StartQuest(string questID)
    {
        if (questManager.IsQuestActive(questID))
        {
            Debug.Log($"任务已在进行中: {questID}");
            return false;
        }
        
        if (questManager.IsQuestCompleted(questID))
        {
            Debug.Log($"任务已完成: {questID}");
            return false;
        }
        
        questManager.ActivateQuest(questID);
        Debug.Log($"开始任务: {questID}");
        return true;
    }
    
    public bool CompleteQuest(string questID)
    {
        if (!questManager.IsQuestActive(questID))
        {
            Debug.Log($"任务未激活: {questID}");
            return false;
        }
        
        questManager.CompleteQuest(questID);
        Debug.Log($"完成任务: {questID}");
        return true;
    }
    
    public void CheckQuestProgress(string questID)
    {
        if (questManager.IsQuestActive(questID))
        {
            Debug.Log($"任务进行中: {questID}");
        }
        else if (questManager.IsQuestCompleted(questID))
        {
            Debug.Log($"任务已完成: {questID}");
        }
        else
        {
            Debug.Log($"任务未开始: {questID}");
        }
    }
}
```

### 任务进度跟踪
```csharp
public class QuestProgressTracker
{
    private Dictionary<string, int> questProgress;
    
    public QuestProgressTracker()
    {
        questProgress = new Dictionary<string, int>();
    }
    
    public void UpdateQuestProgress(string questID, int progress)
    {
        if (questProgress.ContainsKey(questID))
        {
            questProgress[questID] = progress;
        }
        else
        {
            questProgress[questID] = progress;
        }
        
        Debug.Log($"任务进度更新: {questID} - {progress}");
        
        // 检查是否完成任务
        if (progress >= 100)
        {
            CompleteQuest(questID);
        }
    }
    
    public int GetQuestProgress(string questID)
    {
        return questProgress.ContainsKey(questID) ? questProgress[questID] : 0;
    }
    
    private void CompleteQuest(string questID)
    {
        QuestManager.CompleteQuest(questID);
        Debug.Log($"任务完成: {questID}");
    }
}
```

### 任务奖励系统
```csharp
public class QuestRewardSystem
{
    private QuestManager questManager;
    private EconomyManager economyManager;
    
    public QuestRewardSystem()
    {
        questManager = QuestManager.Instance;
        economyManager = EconomyManager.Instance;
    }
    
    public void GiveQuestReward(string questID)
    {
        if (!questManager.IsQuestCompleted(questID))
        {
            Debug.Log($"任务未完成，无法给予奖励: {questID}");
            return;
        }
        
        // 根据任务ID给予不同奖励
        switch (questID)
        {
            case "MainQuest_01":
                GiveMainQuestReward();
                break;
            case "SideQuest_01":
                GiveSideQuestReward();
                break;
            default:
                GiveDefaultReward();
                break;
        }
    }
    
    private void GiveMainQuestReward()
    {
        // 给予货币奖励
        economyManager.AddCurrency(1000);
        Debug.Log("获得1000货币奖励");
        
        // 解锁物品
        economyManager.UnlockItem("Weapon_AK47");
        Debug.Log("解锁AK47武器");
    }
    
    private void GiveSideQuestReward()
    {
        // 给予货币奖励
        economyManager.AddCurrency(500);
        Debug.Log("获得500货币奖励");
    }
    
    private void GiveDefaultReward()
    {
        // 给予默认奖励
        economyManager.AddCurrency(100);
        Debug.Log("获得100货币奖励");
    }
}
```

### 任务链系统
```csharp
public class QuestChainSystem
{
    private QuestManager questManager;
    private Dictionary<string, string[]> questChains;
    
    public QuestChainSystem()
    {
        questManager = QuestManager.Instance;
        questChains = new Dictionary<string, string[]>();
        InitializeQuestChains();
    }
    
    private void InitializeQuestChains()
    {
        // 主任务链
        questChains["MainQuest_01"] = new string[] { "MainQuest_02", "MainQuest_03" };
        questChains["MainQuest_02"] = new string[] { "MainQuest_03" };
        
        // 支线任务链
        questChains["SideQuest_01"] = new string[] { "SideQuest_02" };
    }
    
    public void OnQuestCompleted(string questID)
    {
        if (questChains.ContainsKey(questID))
        {
            string[] nextQuests = questChains[questID];
            foreach (string nextQuest in nextQuests)
            {
                if (!questManager.IsQuestCompleted(nextQuest))
                {
                    questManager.ActivateQuest(nextQuest);
                    Debug.Log($"激活后续任务: {nextQuest}");
                }
            }
        }
    }
}
```

### 任务条件检查
```csharp
public class QuestConditionChecker
{
    private QuestManager questManager;
    private CharacterMainControl player;
    
    public QuestConditionChecker()
    {
        questManager = QuestManager.Instance;
        player = CharacterMainControl.Main;
    }
    
    public bool CheckQuestCondition(string questID, string condition)
    {
        switch (condition)
        {
            case "PlayerLevel":
                return CheckPlayerLevelCondition(questID);
            case "ItemCount":
                return CheckItemCountCondition(questID);
            case "EnemyKilled":
                return CheckEnemyKilledCondition(questID);
            default:
                return false;
        }
    }
    
    private bool CheckPlayerLevelCondition(string questID)
    {
        // 检查玩家等级条件
        int requiredLevel = GetRequiredLevel(questID);
        int currentLevel = EXPManager.Level;
        return currentLevel >= requiredLevel;
    }
    
    private bool CheckItemCountCondition(string questID)
    {
        // 检查物品数量条件
        string requiredItem = GetRequiredItem(questID);
        int requiredCount = GetRequiredCount(questID);
        int currentCount = GetItemCount(requiredItem);
        return currentCount >= requiredCount;
    }
    
    private bool CheckEnemyKilledCondition(string questID)
    {
        // 检查击杀敌人条件
        int requiredKills = GetRequiredKills(questID);
        int currentKills = GetEnemyKillCount();
        return currentKills >= requiredKills;
    }
    
    private int GetRequiredLevel(string questID)
    {
        // 根据任务ID获取所需等级
        return 5; // 示例
    }
    
    private string GetRequiredItem(string questID)
    {
        // 根据任务ID获取所需物品
        return "Weapon_AK47"; // 示例
    }
    
    private int GetRequiredCount(string questID)
    {
        // 根据任务ID获取所需数量
        return 1; // 示例
    }
    
    private int GetRequiredKills(string questID)
    {
        // 根据任务ID获取所需击杀数
        return 10; // 示例
    }
    
    private int GetItemCount(string itemID)
    {
        // 获取物品数量
        return 0; // 示例
    }
    
    private int GetEnemyKillCount()
    {
        // 获取击杀敌人数量
        return 0; // 示例
    }
}
```

## 注意事项

1. 任务系统管理任务的激活、完成和进度跟踪
2. 任务状态会保存在存档中
3. 任务完成会触发相应事件和奖励
4. 任务链系统支持任务的依赖关系
5. 任务条件检查确保任务完成的合法性
6. 任务奖励系统与经济系统紧密相关
7. 事件监听后记得在OnDisable中取消订阅
8. 任务进度跟踪需要实时更新
9. 任务系统与成就系统、统计系统相关
10. 任务通知应该及时显示给玩家

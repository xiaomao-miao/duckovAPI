# 事件系统

## 概述
事件系统是《逃离鸭科夫》Mod开发的核心机制之一，它允许Mod监听游戏中的各种事件，并在事件发生时执行相应的逻辑。

## 事件系统基础

### 事件订阅模式
事件系统使用观察者模式，允许Mod订阅感兴趣的事件，当事件发生时自动调用相应的处理函数。

### 事件生命周期
1. **订阅事件**: 在Mod初始化时订阅事件
2. **事件触发**: 游戏系统触发事件
3. **事件处理**: 调用Mod的事件处理函数
4. **取消订阅**: 在Mod停用时取消事件订阅

## 核心事件类型

### 角色事件
```csharp
// 主角色开始使用物品
CharacterMainControl.OnMainCharacterStartUseItem += OnItemUsed;

// 主角色背包改变
CharacterMainControl.OnMainCharacterInventoryChangedEvent += OnInventoryChanged;

// 主角色插槽内容改变
CharacterMainControl.OnMainCharacterSlotContentChangedEvent += OnSlotContentChanged;

// 主角色改变手持物品代理
CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent += OnHoldItemChanged;
```

### 生命值事件
```csharp
// 角色受伤
Health.OnHurt += OnCharacterHurt;

// 角色死亡
Health.OnDead += OnCharacterDead;

// 生命值改变
Health.OnHealthChange += OnHealthChanged;

// 最大生命值改变
Health.OnMaxHealthChange += OnMaxHealthChanged;
```

### 经验值事件
```csharp
// 等级改变
EXPManager.OnLevelChanged += OnLevelChanged;

// 经验值改变
EXPManager.OnExpChanged += OnExpChanged;
```

### 关卡事件
```csharp
// 关卡开始初始化
LevelManager.OnLevelBeginInitializing += OnLevelBeginInitializing;

// 关卡初始化完成
LevelManager.OnLevelInitialized += OnLevelInitialized;

// 关卡初始化后
LevelManager.OnAfterLevelInitialized += OnAfterLevelInitialized;

// 撤离
LevelManager.OnEvacuated += OnEvacuated;

// 主角色死亡
LevelManager.OnMainCharacterDead += OnMainCharacterDead;

// 新游戏报告
LevelManager.OnNewGameReport += OnNewGameReport;
```

### 制作事件
```csharp
// 物品制作完成
CraftingManager.OnItemCrafted += OnItemCrafted;

// 配方解锁
CraftingManager.OnFormulaUnlocked += OnFormulaUnlocked;
```

### 成就事件
```csharp
// 成就解锁
AchievementManager.OnAchievementUnlocked += OnAchievementUnlocked;

// 成就数据加载
AchievementManager.OnAchievementDataLoaded += OnAchievementDataLoaded;
```

### 任务事件
```csharp
// 任务激活
QuestManager.OnQuestActivated += OnQuestActivated;

// 任务完成
QuestManager.OnQuestCompleted += OnQuestCompleted;

// 任务完成
QuestManager.OnTaskCompleted += OnTaskCompleted;
```

### 经济事件
```csharp
// 经济管理器加载
EconomyManager.OnEconomyManagerLoaded += OnEconomyManagerLoaded;

// 物品解锁
EconomyManager.OnItemUnlocked += OnItemUnlocked;

// 货币改变
EconomyManager.OnCurrencyChanged += OnCurrencyChanged;
```

### 难度事件
```csharp
// 规则改变
GameRulesManager.OnRuleChanged += OnRuleChanged;
```

### 存档事件
```csharp
// 收集存档数据
SavesSystem.OnCollectSaveData += OnCollectSaveData;

// 加载存档数据
SavesSystem.OnLoadSaveData += OnLoadSaveData;
```

### 输入事件
```csharp
// 输入设备改变
InputManager.OnInputDeviceChanged += OnInputDeviceChanged;

// 切换子弹类型输入
InputManager.OnSwitchBulletTypeInput += OnSwitchBulletTypeInput;

// 切换武器输入
InputManager.OnSwitchWeaponInput += OnSwitchWeaponInput;

// 交互按钮按下
InputManager.OnInteractButtonDown += OnInteractButtonDown;
```

### UI事件
```csharp
// 活动视图改变
View.OnActiveViewChanged += OnActiveViewChanged;

// 对话状态改变
DialogueUI.OnDialogueStatusChanged += OnDialogueStatusChanged;

// 自定义UI视图改变
CustomFaceUI.OnCustomUIViewChanged += OnCustomUIViewChanged;

// 相机模式改变
CameraMode.OnCameraModeChanged += OnCameraModeChanged;
```

### 富状态事件
```csharp
// 实例改变
RichPresenceManager.OnInstanceChanged += OnInstanceChanged;
```

## 事件处理最佳实践

### 1. 事件订阅管理
```csharp
public class MyMod : ModBehaviour
{
    private bool isInitialized = false;
    
    public override void OnAfterSetup()
    {
        if (!isInitialized)
        {
            RegisterEventListeners();
            isInitialized = true;
        }
    }
    
    public override void OnBeforeDeactivate()
    {
        if (isInitialized)
        {
            UnregisterEventListeners();
            isInitialized = false;
        }
    }
    
    private void RegisterEventListeners()
    {
        // 订阅事件
        CharacterMainControl.OnMainCharacterStartUseItem += OnItemUsed;
        Health.OnHurt += OnCharacterHurt;
        EXPManager.OnLevelChanged += OnLevelChanged;
    }
    
    private void UnregisterEventListeners()
    {
        // 取消订阅事件
        CharacterMainControl.OnMainCharacterStartUseItem -= OnItemUsed;
        Health.OnHurt -= OnCharacterHurt;
        EXPManager.OnLevelChanged -= OnLevelChanged;
    }
}
```

### 2. 事件处理函数
```csharp
private void OnItemUsed(Item item)
{
    try
    {
        // 检查物品是否有效
        if (item == null)
        {
            Debug.LogWarning("物品为空");
            return;
        }
        
        // 处理物品使用逻辑
        Debug.Log($"主角色使用了物品: {item.DisplayName}");
        
        // 记录统计信息
        RecordItemUsage(item);
    }
    catch (System.Exception e)
    {
        Debug.LogError($"处理物品使用事件时发生错误: {e.Message}");
    }
}

private void OnCharacterHurt(DamageInfo damageInfo)
{
    try
    {
        // 检查伤害信息是否有效
        if (damageInfo == null)
        {
            Debug.LogWarning("伤害信息为空");
            return;
        }
        
        // 处理受伤逻辑
        Debug.Log($"角色受到伤害: {damageInfo.damageValue}");
        
        // 更新UI显示
        UpdateHealthUI();
    }
    catch (System.Exception e)
    {
        Debug.LogError($"处理受伤事件时发生错误: {e.Message}");
    }
}

private void OnLevelChanged(int newLevel)
{
    try
    {
        // 处理等级改变逻辑
        Debug.Log($"角色等级提升到: {newLevel}");
        
        // 显示等级提升通知
        ShowLevelUpNotification(newLevel);
        
        // 更新角色属性
        UpdateCharacterAttributes(newLevel);
    }
    catch (System.Exception e)
    {
        Debug.LogError($"处理等级改变事件时发生错误: {e.Message}");
    }
}
```

### 3. 事件过滤和条件检查
```csharp
private void OnItemUsed(Item item)
{
    // 只处理特定类型的物品
    if (item.TypeID == "Weapon_AK47")
    {
        Debug.Log("使用了AK47武器");
        HandleWeaponUsage(item);
    }
    
    // 只处理特定品质的物品
    if (item.Quality >= 3)
    {
        Debug.Log("使用了高品质物品");
        HandleHighQualityItem(item);
    }
}

private void OnCharacterHurt(DamageInfo damageInfo)
{
    // 只处理特定类型的伤害
    if (damageInfo.damageType == DamageTypes.physics)
    {
        Debug.Log("受到物理伤害");
        HandlePhysicalDamage(damageInfo);
    }
    
    // 只处理特定来源的伤害
    if (damageInfo.fromCharacter != null)
    {
        Debug.Log("受到来自其他角色的伤害");
        HandlePlayerDamage(damageInfo);
    }
}
```

### 4. 事件优先级和顺序
```csharp
public class EventPriorityManager
{
    private List<System.Action> highPriorityHandlers;
    private List<System.Action> normalPriorityHandlers;
    private List<System.Action> lowPriorityHandlers;
    
    public EventPriorityManager()
    {
        highPriorityHandlers = new List<System.Action>();
        normalPriorityHandlers = new List<System.Action>();
        lowPriorityHandlers = new List<System.Action>();
    }
    
    public void RegisterHighPriorityHandler(System.Action handler)
    {
        highPriorityHandlers.Add(handler);
    }
    
    public void RegisterNormalPriorityHandler(System.Action handler)
    {
        normalPriorityHandlers.Add(handler);
    }
    
    public void RegisterLowPriorityHandler(System.Action handler)
    {
        lowPriorityHandlers.Add(handler);
    }
    
    public void ExecuteHandlers()
    {
        // 按优先级执行处理器
        foreach (var handler in highPriorityHandlers)
        {
            handler.Invoke();
        }
        
        foreach (var handler in normalPriorityHandlers)
        {
            handler.Invoke();
        }
        
        foreach (var handler in lowPriorityHandlers)
        {
            handler.Invoke();
        }
    }
}
```

### 5. 事件数据传递
```csharp
public class EventData
{
    public string EventType { get; set; }
    public object Data { get; set; }
    public float Timestamp { get; set; }
    
    public EventData(string eventType, object data)
    {
        EventType = eventType;
        Data = data;
        Timestamp = Time.time;
    }
}

public class EventDataManager
{
    private Dictionary<string, List<EventData>> eventHistory;
    
    public EventDataManager()
    {
        eventHistory = new Dictionary<string, List<EventData>>();
    }
    
    public void RecordEvent(string eventType, object data)
    {
        EventData eventData = new EventData(eventType, data);
        
        if (!eventHistory.ContainsKey(eventType))
        {
            eventHistory[eventType] = new List<EventData>();
        }
        
        eventHistory[eventType].Add(eventData);
        
        // 限制历史记录数量
        if (eventHistory[eventType].Count > 100)
        {
            eventHistory[eventType].RemoveAt(0);
        }
    }
    
    public List<EventData> GetEventHistory(string eventType)
    {
        return eventHistory.ContainsKey(eventType) ? eventHistory[eventType] : new List<EventData>();
    }
}
```

## 事件系统高级用法

### 1. 自定义事件
```csharp
public class CustomEventManager
{
    public static event System.Action<string> OnCustomEvent;
    public static event System.Action<CustomEventData> OnCustomEventWithData;
    
    public static void TriggerCustomEvent(string eventName)
    {
        OnCustomEvent?.Invoke(eventName);
    }
    
    public static void TriggerCustomEventWithData(CustomEventData eventData)
    {
        OnCustomEventWithData?.Invoke(eventData);
    }
}

public class CustomEventData
{
    public string EventName { get; set; }
    public object Data { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
    
    public CustomEventData(string eventName, object data)
    {
        EventName = eventName;
        Data = data;
        Parameters = new Dictionary<string, object>();
    }
}
```

### 2. 事件链式处理
```csharp
public class EventChainProcessor
{
    private List<System.Func<object, object>> processors;
    
    public EventChainProcessor()
    {
        processors = new List<System.Func<object, object>>();
    }
    
    public void AddProcessor(System.Func<object, object> processor)
    {
        processors.Add(processor);
    }
    
    public object ProcessEvent(object input)
    {
        object result = input;
        
        foreach (var processor in processors)
        {
            result = processor(result);
        }
        
        return result;
    }
}
```

### 3. 事件异步处理
```csharp
public class AsyncEventProcessor
{
    public static async System.Threading.Tasks.Task ProcessEventAsync(System.Func<System.Threading.Tasks.Task> eventHandler)
    {
        try
        {
            await eventHandler();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"异步事件处理失败: {e.Message}");
        }
    }
    
    public static async System.Threading.Tasks.Task<T> ProcessEventAsync<T>(System.Func<System.Threading.Tasks.Task<T>> eventHandler)
    {
        try
        {
            return await eventHandler();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"异步事件处理失败: {e.Message}");
            return default(T);
        }
    }
}
```

## 事件系统调试

### 1. 事件日志记录
```csharp
public class EventLogger
{
    private static List<string> eventLog;
    
    static EventLogger()
    {
        eventLog = new List<string>();
    }
    
    public static void LogEvent(string eventName, object data = null)
    {
        string logEntry = $"[{Time.time:F2}] {eventName}";
        if (data != null)
        {
            logEntry += $" - {data}";
        }
        
        eventLog.Add(logEntry);
        Debug.Log(logEntry);
        
        // 限制日志数量
        if (eventLog.Count > 1000)
        {
            eventLog.RemoveAt(0);
        }
    }
    
    public static void SaveEventLog()
    {
        string logContent = string.Join("\n", eventLog);
        System.IO.File.WriteAllText("event_log.txt", logContent);
        Debug.Log("事件日志已保存");
    }
}
```

### 2. 事件性能监控
```csharp
public class EventPerformanceMonitor
{
    private static Dictionary<string, float> eventTimings;
    private static Dictionary<string, int> eventCounts;
    
    static EventPerformanceMonitor()
    {
        eventTimings = new Dictionary<string, float>();
        eventCounts = new Dictionary<string, int>();
    }
    
    public static void StartTiming(string eventName)
    {
        eventTimings[eventName] = Time.realtimeSinceStartup;
    }
    
    public static void EndTiming(string eventName)
    {
        if (eventTimings.ContainsKey(eventName))
        {
            float duration = Time.realtimeSinceStartup - eventTimings[eventName];
            Debug.Log($"事件 {eventName} 耗时: {duration * 1000:F2}ms");
            
            if (eventCounts.ContainsKey(eventName))
            {
                eventCounts[eventName]++;
            }
            else
            {
                eventCounts[eventName] = 1;
            }
        }
    }
    
    public static void ShowPerformanceReport()
    {
        Debug.Log("=== 事件性能报告 ===");
        foreach (var kvp in eventCounts)
        {
            Debug.Log($"事件 {kvp.Key}: 触发 {kvp.Value} 次");
        }
    }
}
```

## 注意事项

1. **事件订阅**: 始终在OnAfterSetup中订阅事件
2. **事件取消**: 始终在OnBeforeDeactivate中取消事件订阅
3. **异常处理**: 在事件处理函数中使用try-catch
4. **性能考虑**: 避免在事件处理函数中执行耗时操作
5. **内存管理**: 及时清理事件订阅，避免内存泄漏
6. **事件顺序**: 考虑事件处理的顺序和优先级
7. **数据验证**: 在事件处理函数中验证数据有效性
8. **日志记录**: 记录重要事件的日志信息
9. **性能监控**: 监控事件处理的性能
10. **错误恢复**: 在事件处理失败时提供恢复机制

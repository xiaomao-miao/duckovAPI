# 难度系统

## 概述
难度系统是《逃离鸭科夫》的重要系统之一，负责管理游戏难度、规则设置、难度选择等功能。

## 核心组件

### GameRulesManager
游戏规则管理器，管理游戏难度和规则设置。

#### 主要功能
- **难度管理** - 设置、获取当前难度
- **规则管理** - 管理游戏规则集
- **事件通知** - 规则改变事件
- **存档管理** - 难度设置保存

#### 重要属性
- `Instance` - 获取GameRulesManager实例
- `Current` - 当前规则集
- `SelectedRuleIndex` - 选中的规则索引

#### 重要事件
- `OnRuleChanged` - 规则改变事件

#### 重要方法
- `NotifyRuleChanged()` - 通知规则改变
- `GetRuleIndexOfSaveSlot(int slot)` - 获取存档槽的规则索引
- `GetRuleIndexDisplayNameOfSlot(int slotIndex)` - 获取存档槽的规则显示名称

### Ruleset
规则集类，定义游戏的具体规则设置。

#### 重要属性
- `DisplayName` - 显示名称
- `Description` - 描述
- `SpawnDeadBody` - 是否生成尸体
- `SaveDeadbodyCount` - 保存尸体数量
- `FogOfWar` - 是否启用战争迷雾
- `AdvancedDebuffMode` - 是否启用高级减益模式
- `RecoilMultiplier` - 后坐力倍数
- `DamageFactor_ToPlayer` - 对玩家伤害倍数
- `EnemyHealthFactor` - 敌人生命值倍数
- `EnemyReactionTimeFactor` - 敌人反应时间倍数
- `EnemyAttackTimeSpaceFactor` - 敌人攻击时间间隔倍数
- `EnemyAttackTimeFactor` - 敌人攻击时间倍数

### RuleIndex
规则索引枚举，定义可用的难度等级。

#### 枚举值
- `Standard` - 标准难度
- `Custom` - 自定义难度
- `Easy` - 简单难度
- `ExtraEasy` - 超简单难度
- `Hard` - 困难难度
- `ExtraHard` - 超困难难度
- `Rage` - 愤怒难度
- `StandardChallenge` - 标准挑战难度

### DifficultySelection
难度选择UI系统，管理难度选择界面。

#### 主要功能
- **难度选择** - 选择游戏难度
- **解锁管理** - 解锁特殊难度
- **UI管理** - 难度选择界面

#### 重要属性
- `CustomDifficultyMarker` - 自定义难度标记
- `SelectedRuleIndex` - 选中的规则索引
- `SelectedEntry` - 选中的条目
- `HoveringEntry` - 悬停的条目

#### 重要方法
- `UnlockRage()` - 解锁愤怒难度
- `Execute()` - 执行难度选择
- `GetRageUnlocked(bool isFirstSelect)` - 获取愤怒难度是否解锁
- `NotifySelected(DifficultySelection_Entry entry)` - 通知选中条目
- `NotifyEntryPointerEnter(DifficultySelection_Entry entry)` - 通知条目指针进入
- `NotifyEntryPointerExit(DifficultySelection_Entry entry)` - 通知条目指针退出
- `RefreshDescription()` - 刷新描述
- `SkipHide()` - 跳过隐藏

## 使用示例

### 获取当前难度
```csharp
// 获取当前难度
RuleIndex currentDifficulty = GameRulesManager.SelectedRuleIndex;
Ruleset currentRules = GameRulesManager.Current;

Debug.Log($"当前难度: {currentRules.DisplayName}");
Debug.Log($"难度描述: {currentRules.Description}");
```

### 设置难度
```csharp
// 设置难度
GameRulesManager.SelectedRuleIndex = RuleIndex.Hard;
Debug.Log("设置难度为困难");

// 通知规则改变
GameRulesManager.NotifyRuleChanged();
```

### 监听难度改变
```csharp
void OnEnable()
{
    GameRulesManager.OnRuleChanged += OnDifficultyChanged;
}

void OnDisable()
{
    GameRulesManager.OnRuleChanged -= OnDifficultyChanged;
}

private void OnDifficultyChanged()
{
    Ruleset currentRules = GameRulesManager.Current;
    Debug.Log($"难度改变为: {currentRules.DisplayName}");
    
    // 更新游戏设置
    UpdateGameSettings(currentRules);
}
```

### 检查难度设置
```csharp
Ruleset currentRules = GameRulesManager.Current;

// 检查战争迷雾
if (currentRules.FogOfWar)
{
    Debug.Log("战争迷雾已启用");
}

// 检查高级减益模式
if (currentRules.AdvancedDebuffMode)
{
    Debug.Log("高级减益模式已启用");
}

// 检查尸体生成
if (currentRules.SpawnDeadBody)
{
    Debug.Log("尸体生成已启用");
}
```

### 获取难度属性
```csharp
Ruleset currentRules = GameRulesManager.Current;

// 获取伤害倍数
float damageMultiplier = currentRules.DamageFactor_ToPlayer;
Debug.Log($"对玩家伤害倍数: {damageMultiplier}");

// 获取敌人生命值倍数
float enemyHealthMultiplier = currentRules.EnemyHealthFactor;
Debug.Log($"敌人生命值倍数: {enemyHealthMultiplier}");

// 获取后坐力倍数
float recoilMultiplier = currentRules.RecoilMultiplier;
Debug.Log($"后坐力倍数: {recoilMultiplier}");
```

### 解锁特殊难度
```csharp
// 解锁愤怒难度
DifficultySelection.UnlockRage();
Debug.Log("解锁愤怒难度");

// 检查愤怒难度是否解锁
bool rageUnlocked = DifficultySelection.GetRageUnlocked(true);
if (rageUnlocked)
{
    Debug.Log("愤怒难度已解锁");
}
```

### 难度选择系统
```csharp
public class DifficultySelectionSystem
{
    private DifficultySelection difficultySelection;
    
    public DifficultySelectionSystem()
    {
        difficultySelection = GetComponent<DifficultySelection>();
    }
    
    public async Task<RuleIndex> SelectDifficulty()
    {
        // 执行难度选择
        await difficultySelection.Execute();
        
        // 获取选中的难度
        RuleIndex selectedDifficulty = difficultySelection.SelectedRuleIndex;
        Debug.Log($"选中难度: {selectedDifficulty}");
        
        return selectedDifficulty;
    }
    
    public void ShowDifficultyDescription(RuleIndex difficulty)
    {
        // 显示难度描述
        Ruleset rules = GetRulesetByIndex(difficulty);
        Debug.Log($"难度: {rules.DisplayName}");
        Debug.Log($"描述: {rules.Description}");
    }
    
    private Ruleset GetRulesetByIndex(RuleIndex index)
    {
        // 根据索引获取规则集
        // 这里需要根据实际实现
        return GameRulesManager.Current;
    }
}
```

### 难度系统集成
```csharp
public class DifficultySystem
{
    private GameRulesManager gameRulesManager;
    private DifficultySelection difficultySelection;
    
    public DifficultySystem()
    {
        gameRulesManager = GameRulesManager.Instance;
        difficultySelection = GetComponent<DifficultySelection>();
    }
    
    public void ApplyDifficultySettings(RuleIndex difficulty)
    {
        // 设置难度
        gameRulesManager.SelectedRuleIndex = difficulty;
        
        // 获取规则集
        Ruleset rules = gameRulesManager.Current;
        
        // 应用游戏设置
        ApplyGameSettings(rules);
        
        Debug.Log($"应用难度设置: {rules.DisplayName}");
    }
    
    private void ApplyGameSettings(Ruleset rules)
    {
        // 应用战争迷雾设置
        if (rules.FogOfWar)
        {
            EnableFogOfWar();
        }
        else
        {
            DisableFogOfWar();
        }
        
        // 应用尸体生成设置
        if (rules.SpawnDeadBody)
        {
            EnableDeadBodySpawning();
        }
        else
        {
            DisableDeadBodySpawning();
        }
        
        // 应用伤害倍数设置
        ApplyDamageMultiplier(rules.DamageFactor_ToPlayer);
        
        // 应用敌人设置
        ApplyEnemySettings(rules);
    }
    
    private void EnableFogOfWar()
    {
        // 启用战争迷雾
        Debug.Log("启用战争迷雾");
    }
    
    private void DisableFogOfWar()
    {
        // 禁用战争迷雾
        Debug.Log("禁用战争迷雾");
    }
    
    private void EnableDeadBodySpawning()
    {
        // 启用尸体生成
        Debug.Log("启用尸体生成");
    }
    
    private void DisableDeadBodySpawning()
    {
        // 禁用尸体生成
        Debug.Log("禁用尸体生成");
    }
    
    private void ApplyDamageMultiplier(float multiplier)
    {
        // 应用伤害倍数
        Debug.Log($"应用伤害倍数: {multiplier}");
    }
    
    private void ApplyEnemySettings(Ruleset rules)
    {
        // 应用敌人设置
        Debug.Log($"敌人生命值倍数: {rules.EnemyHealthFactor}");
        Debug.Log($"敌人反应时间倍数: {rules.EnemyReactionTimeFactor}");
        Debug.Log($"敌人攻击时间倍数: {rules.EnemyAttackTimeFactor}");
    }
}
```

### 难度平衡系统
```csharp
public class DifficultyBalanceSystem
{
    private GameRulesManager gameRulesManager;
    
    public DifficultyBalanceSystem()
    {
        gameRulesManager = GameRulesManager.Instance;
    }
    
    public void BalanceDifficulty()
    {
        Ruleset currentRules = gameRulesManager.Current;
        
        // 根据难度调整游戏平衡
        switch (gameRulesManager.SelectedRuleIndex)
        {
            case RuleIndex.Easy:
                ApplyEasyDifficultyBalance();
                break;
            case RuleIndex.Standard:
                ApplyStandardDifficultyBalance();
                break;
            case RuleIndex.Hard:
                ApplyHardDifficultyBalance();
                break;
            case RuleIndex.Rage:
                ApplyRageDifficultyBalance();
                break;
        }
    }
    
    private void ApplyEasyDifficultyBalance()
    {
        Debug.Log("应用简单难度平衡");
        // 降低敌人难度
        // 增加玩家资源
        // 减少惩罚
    }
    
    private void ApplyStandardDifficultyBalance()
    {
        Debug.Log("应用标准难度平衡");
        // 标准游戏平衡
    }
    
    private void ApplyHardDifficultyBalance()
    {
        Debug.Log("应用困难难度平衡");
        // 增加敌人难度
        // 减少玩家资源
        // 增加惩罚
    }
    
    private void ApplyRageDifficultyBalance()
    {
        Debug.Log("应用愤怒难度平衡");
        // 最高难度设置
        // 极限挑战
    }
}
```

### 难度统计系统
```csharp
public class DifficultyStatistics
{
    private GameRulesManager gameRulesManager;
    private Dictionary<RuleIndex, int> difficultyPlayCount;
    
    public DifficultyStatistics()
    {
        gameRulesManager = GameRulesManager.Instance;
        difficultyPlayCount = new Dictionary<RuleIndex, int>();
        LoadDifficultyStatistics();
    }
    
    public void RecordDifficultyPlay(RuleIndex difficulty)
    {
        if (difficultyPlayCount.ContainsKey(difficulty))
        {
            difficultyPlayCount[difficulty]++;
        }
        else
        {
            difficultyPlayCount[difficulty] = 1;
        }
        
        Debug.Log($"记录难度游戏次数: {difficulty} - {difficultyPlayCount[difficulty]}");
        SaveDifficultyStatistics();
    }
    
    public int GetDifficultyPlayCount(RuleIndex difficulty)
    {
        return difficultyPlayCount.ContainsKey(difficulty) ? difficultyPlayCount[difficulty] : 0;
    }
    
    public RuleIndex GetMostPlayedDifficulty()
    {
        RuleIndex mostPlayed = RuleIndex.Standard;
        int maxCount = 0;
        
        foreach (var kvp in difficultyPlayCount)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                mostPlayed = kvp.Key;
            }
        }
        
        return mostPlayed;
    }
    
    private void LoadDifficultyStatistics()
    {
        // 从存档加载难度统计
        difficultyPlayCount = SavesSystem.Load("DifficultyPlayCount", new Dictionary<RuleIndex, int>());
    }
    
    private void SaveDifficultyStatistics()
    {
        // 保存难度统计到存档
        SavesSystem.Save("DifficultyPlayCount", difficultyPlayCount);
    }
}
```

## 注意事项

1. 难度系统管理游戏规则和难度设置
2. 难度改变会触发相应事件
3. 难度设置会保存在存档中
4. 特殊难度需要解锁才能使用
5. 难度系统与游戏平衡紧密相关
6. 事件监听后记得在OnDisable中取消订阅
7. 难度选择UI需要正确处理用户交互
8. 难度统计有助于了解玩家偏好
9. 难度系统与战斗系统、角色系统相关
10. 难度平衡需要根据游戏测试调整

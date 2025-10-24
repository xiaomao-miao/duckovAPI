# 尸体系统

## 概述
尸体系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的死亡记录、尸体管理、死亡信息存储等功能。

## 核心组件

### DeadBodyManager
尸体管理器，管理游戏中的尸体系统。

#### 主要功能
- **死亡记录** - 记录角色死亡信息
- **尸体管理** - 管理尸体生成和清理
- **死亡统计** - 统计死亡次数和信息
- **数据管理** - 管理死亡数据

#### 重要属性
- `Instance` - 获取DeadBodyManager实例

#### 重要方法
- `RecordDeath(CharacterMainControl character)` - 记录死亡
- `GetDeathCount()` - 获取死亡次数
- `ClearDeathRecords()` - 清除死亡记录
- `GetDeathInfo(int index)` - 获取死亡信息

### DeathInfo
死亡信息类，存储死亡相关信息。

#### 重要属性
- `Valid` - 是否有效
- `RaidID` - 突袭ID
- `SubSceneID` - 子场景ID
- `DeathTime` - 死亡时间
- `CharacterName` - 角色名称
- `DeathCause` - 死亡原因

## 使用示例

### 记录死亡
```csharp
CharacterMainControl character = CharacterMainControl.Main;
if (character != null)
{
    DeadBodyManager.RecordDeath(character);
    Debug.Log("记录角色死亡");
}
```

### 获取死亡统计
```csharp
// 获取死亡次数
int deathCount = DeadBodyManager.GetDeathCount();
Debug.Log($"死亡次数: {deathCount}");

// 获取死亡信息
DeathInfo deathInfo = DeadBodyManager.GetDeathInfo(0);
if (deathInfo != null && deathInfo.Valid)
{
    Debug.Log($"死亡时间: {deathInfo.DeathTime}");
    Debug.Log($"角色名称: {deathInfo.CharacterName}");
    Debug.Log($"死亡原因: {deathInfo.DeathCause}");
    Debug.Log($"突袭ID: {deathInfo.RaidID}");
    Debug.Log($"子场景ID: {deathInfo.SubSceneID}");
}
```

### 清除死亡记录
```csharp
// 清除所有死亡记录
DeadBodyManager.ClearDeathRecords();
Debug.Log("清除所有死亡记录");
```

### 尸体系统集成
```csharp
public class DeathSystem
{
    private DeadBodyManager deadBodyManager;
    private List<DeathInfo> deathHistory;
    
    public DeathSystem()
    {
        deadBodyManager = DeadBodyManager.Instance;
        deathHistory = new List<DeathInfo>();
    }
    
    public void OnCharacterDeath(CharacterMainControl character)
    {
        // 记录死亡
        deadBodyManager.RecordDeath(character);
        
        // 创建死亡信息
        DeathInfo deathInfo = CreateDeathInfo(character);
        deathHistory.Add(deathInfo);
        
        // 处理死亡逻辑
        HandleDeathLogic(character, deathInfo);
        
        Debug.Log($"角色死亡: {character.name}");
    }
    
    public void ShowDeathStatistics()
    {
        int totalDeaths = deadBodyManager.GetDeathCount();
        Debug.Log($"总死亡次数: {totalDeaths}");
        
        // 显示死亡历史
        for (int i = 0; i < deathHistory.Count; i++)
        {
            DeathInfo deathInfo = deathHistory[i];
            if (deathInfo.Valid)
            {
                Debug.Log($"死亡记录 {i + 1}: {deathInfo.CharacterName} - {deathInfo.DeathCause}");
            }
        }
    }
    
    public void ClearDeathHistory()
    {
        deadBodyManager.ClearDeathRecords();
        deathHistory.Clear();
        Debug.Log("清除死亡历史");
    }
    
    private DeathInfo CreateDeathInfo(CharacterMainControl character)
    {
        DeathInfo deathInfo = new DeathInfo();
        deathInfo.Valid = true;
        deathInfo.CharacterName = character.name;
        deathInfo.DeathTime = Time.time;
        deathInfo.DeathCause = GetDeathCause(character);
        deathInfo.RaidID = GetCurrentRaidID();
        deathInfo.SubSceneID = GetCurrentSubSceneID();
        
        return deathInfo;
    }
    
    private string GetDeathCause(CharacterMainControl character)
    {
        // 根据角色状态确定死亡原因
        if (character.Health.CurrentHealth <= 0)
        {
            return "生命值归零";
        }
        
        return "未知原因";
    }
    
    private string GetCurrentRaidID()
    {
        // 获取当前突袭ID
        return "Raid_001"; // 示例
    }
    
    private string GetCurrentSubSceneID()
    {
        // 获取当前子场景ID
        return "Scene_001"; // 示例
    }
    
    private void HandleDeathLogic(CharacterMainControl character, DeathInfo deathInfo)
    {
        // 处理死亡逻辑
        // 生成尸体
        GenerateDeadBody(character, deathInfo);
        
        // 掉落物品
        DropItems(character);
        
        // 更新统计
        UpdateDeathStatistics(deathInfo);
    }
    
    private void GenerateDeadBody(CharacterMainControl character, DeathInfo deathInfo)
    {
        // 生成尸体
        Debug.Log($"生成尸体: {character.name}");
    }
    
    private void DropItems(CharacterMainControl character)
    {
        // 掉落物品
        Debug.Log($"掉落物品: {character.name}");
    }
    
    private void UpdateDeathStatistics(DeathInfo deathInfo)
    {
        // 更新死亡统计
        Debug.Log($"更新死亡统计: {deathInfo.CharacterName}");
    }
}
```

### 死亡统计系统
```csharp
public class DeathStatisticsSystem
{
    private DeadBodyManager deadBodyManager;
    private Dictionary<string, int> deathCauseStats;
    private Dictionary<string, int> deathLocationStats;
    private Dictionary<string, int> deathTimeStats;
    
    public DeathStatisticsSystem()
    {
        deadBodyManager = DeadBodyManager.Instance;
        deathCauseStats = new Dictionary<string, int>();
        deathLocationStats = new Dictionary<string, int>();
        deathTimeStats = new Dictionary<string, int>();
    }
    
    public void RecordDeath(DeathInfo deathInfo)
    {
        if (deathInfo.Valid)
        {
            // 记录死亡原因统计
            string deathCause = deathInfo.DeathCause;
            if (deathCauseStats.ContainsKey(deathCause))
            {
                deathCauseStats[deathCause]++;
            }
            else
            {
                deathCauseStats[deathCause] = 1;
            }
            
            // 记录死亡位置统计
            string deathLocation = deathInfo.SubSceneID;
            if (deathLocationStats.ContainsKey(deathLocation))
            {
                deathLocationStats[deathLocation]++;
            }
            else
            {
                deathLocationStats[deathLocation] = 1;
            }
            
            // 记录死亡时间统计
            string deathTime = GetDeathTimeCategory(deathInfo.DeathTime);
            if (deathTimeStats.ContainsKey(deathTime))
            {
                deathTimeStats[deathTime]++;
            }
            else
            {
                deathTimeStats[deathTime] = 1;
            }
            
            Debug.Log($"记录死亡统计: {deathCause} - {deathLocation} - {deathTime}");
        }
    }
    
    public void ShowDeathStatistics()
    {
        Debug.Log("=== 死亡统计 ===");
        
        // 显示死亡原因统计
        Debug.Log("死亡原因统计:");
        foreach (var kvp in deathCauseStats.OrderByDescending(x => x.Value))
        {
            Debug.Log($"  {kvp.Key}: {kvp.Value} 次");
        }
        
        // 显示死亡位置统计
        Debug.Log("死亡位置统计:");
        foreach (var kvp in deathLocationStats.OrderByDescending(x => x.Value))
        {
            Debug.Log($"  {kvp.Key}: {kvp.Value} 次");
        }
        
        // 显示死亡时间统计
        Debug.Log("死亡时间统计:");
        foreach (var kvp in deathTimeStats.OrderByDescending(x => x.Value))
        {
            Debug.Log($"  {kvp.Key}: {kvp.Value} 次");
        }
    }
    
    public string GetMostCommonDeathCause()
    {
        if (deathCauseStats.Count > 0)
        {
            return deathCauseStats.OrderByDescending(x => x.Value).First().Key;
        }
        return "无数据";
    }
    
    public string GetMostCommonDeathLocation()
    {
        if (deathLocationStats.Count > 0)
        {
            return deathLocationStats.OrderByDescending(x => x.Value).First().Key;
        }
        return "无数据";
    }
    
    public string GetMostCommonDeathTime()
    {
        if (deathTimeStats.Count > 0)
        {
            return deathTimeStats.OrderByDescending(x => x.Value).First().Key;
        }
        return "无数据";
    }
    
    private string GetDeathTimeCategory(float deathTime)
    {
        // 根据死亡时间分类
        if (deathTime < 60f)
        {
            return "1分钟内";
        }
        else if (deathTime < 300f)
        {
            return "5分钟内";
        }
        else if (deathTime < 600f)
        {
            return "10分钟内";
        }
        else
        {
            return "10分钟以上";
        }
    }
}
```

### 尸体清理系统
```csharp
public class DeadBodyCleanupSystem
{
    private DeadBodyManager deadBodyManager;
    private List<GameObject> deadBodies;
    private float cleanupInterval;
    private int maxDeadBodies;
    
    public DeadBodyCleanupSystem()
    {
        deadBodyManager = DeadBodyManager.Instance;
        deadBodies = new List<GameObject>();
        cleanupInterval = 300f; // 5分钟
        maxDeadBodies = 10;
    }
    
    public void AddDeadBody(GameObject deadBody)
    {
        deadBodies.Add(deadBody);
        Debug.Log($"添加尸体: {deadBody.name}");
        
        // 检查是否需要清理
        if (deadBodies.Count > maxDeadBodies)
        {
            CleanupOldestDeadBody();
        }
    }
    
    public void RemoveDeadBody(GameObject deadBody)
    {
        if (deadBodies.Contains(deadBody))
        {
            deadBodies.Remove(deadBody);
            Debug.Log($"移除尸体: {deadBody.name}");
        }
    }
    
    public void CleanupAllDeadBodies()
    {
        foreach (GameObject deadBody in deadBodies)
        {
            if (deadBody != null)
            {
                Destroy(deadBody);
            }
        }
        
        deadBodies.Clear();
        Debug.Log("清理所有尸体");
    }
    
    public void CleanupOldestDeadBody()
    {
        if (deadBodies.Count > 0)
        {
            GameObject oldestDeadBody = deadBodies[0];
            deadBodies.RemoveAt(0);
            
            if (oldestDeadBody != null)
            {
                Destroy(oldestDeadBody);
                Debug.Log($"清理最旧的尸体: {oldestDeadBody.name}");
            }
        }
    }
    
    public void StartCleanupTimer()
    {
        StartCoroutine(CleanupTimer());
    }
    
    private IEnumerator CleanupTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(cleanupInterval);
            
            // 清理过期尸体
            CleanupExpiredDeadBodies();
        }
    }
    
    private void CleanupExpiredDeadBodies()
    {
        List<GameObject> expiredBodies = new List<GameObject>();
        
        foreach (GameObject deadBody in deadBodies)
        {
            if (deadBody != null)
            {
                DeadBodyComponent deadBodyComponent = deadBody.GetComponent<DeadBodyComponent>();
                if (deadBodyComponent != null && deadBodyComponent.IsExpired())
                {
                    expiredBodies.Add(deadBody);
                }
            }
        }
        
        foreach (GameObject expiredBody in expiredBodies)
        {
            deadBodies.Remove(expiredBody);
            Destroy(expiredBody);
            Debug.Log($"清理过期尸体: {expiredBody.name}");
        }
    }
}
```

### 死亡回放系统
```csharp
public class DeathReplaySystem
{
    private List<DeathInfo> deathHistory;
    private int currentReplayIndex;
    
    public DeathReplaySystem()
    {
        deathHistory = new List<DeathInfo>();
        currentReplayIndex = 0;
    }
    
    public void AddDeathInfo(DeathInfo deathInfo)
    {
        deathHistory.Add(deathInfo);
        Debug.Log($"添加死亡信息: {deathInfo.CharacterName}");
    }
    
    public void StartDeathReplay()
    {
        if (deathHistory.Count > 0)
        {
            currentReplayIndex = 0;
            PlayDeathReplay(currentReplayIndex);
        }
    }
    
    public void PlayNextDeathReplay()
    {
        if (currentReplayIndex < deathHistory.Count - 1)
        {
            currentReplayIndex++;
            PlayDeathReplay(currentReplayIndex);
        }
    }
    
    public void PlayPreviousDeathReplay()
    {
        if (currentReplayIndex > 0)
        {
            currentReplayIndex--;
            PlayDeathReplay(currentReplayIndex);
        }
    }
    
    public void PlayDeathReplay(int index)
    {
        if (index >= 0 && index < deathHistory.Count)
        {
            DeathInfo deathInfo = deathHistory[index];
            if (deathInfo.Valid)
            {
                Debug.Log($"播放死亡回放 {index + 1}/{deathHistory.Count}");
                Debug.Log($"角色: {deathInfo.CharacterName}");
                Debug.Log($"死亡原因: {deathInfo.DeathCause}");
                Debug.Log($"死亡时间: {deathInfo.DeathTime}");
                Debug.Log($"位置: {deathInfo.SubSceneID}");
            }
        }
    }
    
    public void ClearDeathHistory()
    {
        deathHistory.Clear();
        currentReplayIndex = 0;
        Debug.Log("清除死亡历史");
    }
}
```

## 注意事项

1. 尸体系统记录死亡信息
2. 死亡统计有助于了解游戏难度
3. 尸体清理系统管理内存使用
4. 死亡回放系统提供学习机会
5. 死亡信息包含详细的上下文
6. 尸体系统与战斗系统、角色系统紧密相关
7. 死亡统计需要定期清理，避免内存泄漏
8. 尸体清理需要考虑性能影响
9. 死亡回放系统需要合理的数据结构
10. 死亡系统应该支持数据导出和分析

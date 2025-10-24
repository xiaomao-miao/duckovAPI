# 社交系统

## 概述
社交系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的用户认证、成就同步、排行榜、社交功能等。

## 核心组件

### SocialManager
社交管理器，管理游戏中的社交功能。

#### 主要功能
- **用户认证** - 认证用户身份
- **成就同步** - 同步成就数据
- **进度报告** - 报告游戏进度
- **社交功能** - 显示成就界面、排行榜

#### 重要属性
- `Initialized` - 是否已初始化

#### 重要方法
- `Authenticate()` - 认证用户
- `LoadAchievements()` - 加载成就
- `ReportProgress(string achievementID, double progress)` - 报告进度
- `ShowAchievements()` - 显示成就界面
- `ShowLeaderboard()` - 显示排行榜

### RichPresenceManager
富状态管理器，管理游戏中的富状态显示。

#### 主要功能
- **状态管理** - 管理游戏状态显示
- **状态更新** - 更新富状态信息
- **状态清除** - 清除富状态

#### 重要属性
- `IsPlaying` - 是否在游戏中
- `IsMainMenu` - 是否在主菜单

#### 重要事件
- `OnInstanceChanged` - 实例改变事件

#### 重要方法
- `UpdatePresence(string details, string state)` - 更新富状态
- `ClearPresence()` - 清除富状态
- `SetPlaying(bool playing)` - 设置游戏状态

## 使用示例

### 用户认证
```csharp
SocialManager socialManager = GetComponent<SocialManager>();

// 认证用户
socialManager.Authenticate();
Debug.Log("开始用户认证");

// 检查认证状态
if (socialManager.Initialized)
{
    Debug.Log("用户认证成功");
}
else
{
    Debug.Log("用户认证失败");
}
```

### 成就同步
```csharp
SocialManager socialManager = GetComponent<SocialManager>();

// 加载成就
socialManager.LoadAchievements();
Debug.Log("加载成就数据");

// 报告成就进度
socialManager.ReportProgress("Kill_100_Enemies", 50.0);
Debug.Log("报告成就进度: 击杀100个敌人 - 50%");

// 显示成就界面
socialManager.ShowAchievements();
Debug.Log("显示成就界面");
```

### 排行榜功能
```csharp
SocialManager socialManager = GetComponent<SocialManager>();

// 显示排行榜
socialManager.ShowLeaderboard();
Debug.Log("显示排行榜");
```

### 富状态管理
```csharp
RichPresenceManager richPresenceManager = GetComponent<RichPresenceManager>();

// 更新富状态
richPresenceManager.UpdatePresence("Playing Escape from Duckov", "In Game");
Debug.Log("更新富状态: 游戏中");

// 设置游戏状态
richPresenceManager.SetPlaying(true);
Debug.Log("设置游戏状态: 游戏中");

// 清除富状态
richPresenceManager.ClearPresence();
Debug.Log("清除富状态");
```

### 监听富状态事件
```csharp
void OnEnable()
{
    RichPresenceManager.OnInstanceChanged += OnInstanceChanged;
}

void OnDisable()
{
    RichPresenceManager.OnInstanceChanged -= OnInstanceChanged;
}

private void OnInstanceChanged()
{
    Debug.Log("富状态实例改变");
}
```

### 社交系统集成
```csharp
public class SocialSystem
{
    private SocialManager socialManager;
    private RichPresenceManager richPresenceManager;
    private bool isAuthenticated;
    
    public SocialSystem()
    {
        socialManager = GetComponent<SocialManager>();
        richPresenceManager = GetComponent<RichPresenceManager>();
        isAuthenticated = false;
    }
    
    public async Task<bool> InitializeSocial()
    {
        try
        {
            // 认证用户
            await AuthenticateUser();
            
            // 初始化富状态
            InitializeRichPresence();
            
            // 加载成就
            LoadAchievements();
            
            Debug.Log("社交系统初始化成功");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"社交系统初始化失败: {e.Message}");
            return false;
        }
    }
    
    private async Task AuthenticateUser()
    {
        socialManager.Authenticate();
        
        // 等待认证完成
        while (!socialManager.Initialized)
        {
            await Task.Delay(100);
        }
        
        isAuthenticated = true;
        Debug.Log("用户认证完成");
    }
    
    private void InitializeRichPresence()
    {
        richPresenceManager.SetPlaying(false);
        richPresenceManager.UpdatePresence("Escape from Duckov", "Main Menu");
        Debug.Log("富状态初始化完成");
    }
    
    private void LoadAchievements()
    {
        socialManager.LoadAchievements();
        Debug.Log("成就数据加载完成");
    }
    
    public void UpdateGameStatus(string status)
    {
        if (isAuthenticated)
        {
            richPresenceManager.UpdatePresence("Escape from Duckov", status);
            Debug.Log($"更新游戏状态: {status}");
        }
    }
    
    public void ReportAchievementProgress(string achievementID, double progress)
    {
        if (isAuthenticated)
        {
            socialManager.ReportProgress(achievementID, progress);
            Debug.Log($"报告成就进度: {achievementID} - {progress}%");
        }
    }
    
    public void ShowSocialFeatures()
    {
        if (isAuthenticated)
        {
            socialManager.ShowAchievements();
            Debug.Log("显示社交功能");
        }
    }
}
```

### 成就同步系统
```csharp
public class AchievementSyncSystem
{
    private SocialManager socialManager;
    private AchievementManager achievementManager;
    private Dictionary<string, double> achievementProgress;
    
    public AchievementSyncSystem()
    {
        socialManager = GetComponent<SocialManager>();
        achievementManager = AchievementManager.Instance;
        achievementProgress = new Dictionary<string, double>();
    }
    
    public void SyncAchievements()
    {
        if (socialManager.Initialized)
        {
            // 同步已解锁的成就
            SyncUnlockedAchievements();
            
            // 同步成就进度
            SyncAchievementProgress();
            
            Debug.Log("成就同步完成");
        }
    }
    
    private void SyncUnlockedAchievements()
    {
        List<string> unlockedAchievements = achievementManager.GetUnlockedAchievements();
        
        foreach (string achievementID in unlockedAchievements)
        {
            socialManager.ReportProgress(achievementID, 100.0);
            Debug.Log($"同步已解锁成就: {achievementID}");
        }
    }
    
    private void SyncAchievementProgress()
    {
        foreach (var kvp in achievementProgress)
        {
            string achievementID = kvp.Key;
            double progress = kvp.Value;
            
            socialManager.ReportProgress(achievementID, progress);
            Debug.Log($"同步成就进度: {achievementID} - {progress}%");
        }
    }
    
    public void UpdateAchievementProgress(string achievementID, double progress)
    {
        achievementProgress[achievementID] = progress;
        
        if (socialManager.Initialized)
        {
            socialManager.ReportProgress(achievementID, progress);
            Debug.Log($"更新成就进度: {achievementID} - {progress}%");
        }
    }
    
    public void OnAchievementUnlocked(string achievementID)
    {
        if (socialManager.Initialized)
        {
            socialManager.ReportProgress(achievementID, 100.0);
            Debug.Log($"成就解锁同步: {achievementID}");
        }
    }
}
```

### 富状态系统
```csharp
public class RichPresenceSystem
{
    private RichPresenceManager richPresenceManager;
    private string currentDetails;
    private string currentState;
    
    public RichPresenceSystem()
    {
        richPresenceManager = GetComponent<RichPresenceManager>();
        currentDetails = "Escape from Duckov";
        currentState = "Main Menu";
    }
    
    public void UpdateMainMenuPresence()
    {
        currentDetails = "Escape from Duckov";
        currentState = "Main Menu";
        richPresenceManager.UpdatePresence(currentDetails, currentState);
        richPresenceManager.SetPlaying(false);
        Debug.Log("更新主菜单富状态");
    }
    
    public void UpdateGamePresence(string levelName, string difficulty)
    {
        currentDetails = "Escape from Duckov";
        currentState = $"Playing {levelName} ({difficulty})";
        richPresenceManager.UpdatePresence(currentDetails, currentState);
        richPresenceManager.SetPlaying(true);
        Debug.Log($"更新游戏富状态: {levelName} ({difficulty})");
    }
    
    public void UpdateCombatPresence(string weapon, int enemiesKilled)
    {
        currentDetails = "Escape from Duckov";
        currentState = $"Combat - {weapon} ({enemiesKilled} kills)";
        richPresenceManager.UpdatePresence(currentDetails, currentState);
        richPresenceManager.SetPlaying(true);
        Debug.Log($"更新战斗富状态: {weapon} ({enemiesKilled} kills)");
    }
    
    public void UpdateSurvivalPresence(int health, int level)
    {
        currentDetails = "Escape from Duckov";
        currentState = $"Surviving - Health: {health}%, Level: {level}";
        richPresenceManager.UpdatePresence(currentDetails, currentState);
        richPresenceManager.SetPlaying(true);
        Debug.Log($"更新生存富状态: Health: {health}%, Level: {level}");
    }
    
    public void ClearPresence()
    {
        richPresenceManager.ClearPresence();
        richPresenceManager.SetPlaying(false);
        Debug.Log("清除富状态");
    }
}
```

### 社交统计系统
```csharp
public class SocialStatisticsSystem
{
    private SocialManager socialManager;
    private Dictionary<string, int> socialStats;
    
    public SocialStatisticsSystem()
    {
        socialManager = GetComponent<SocialManager>();
        socialStats = new Dictionary<string, int>();
    }
    
    public void RecordSocialActivity(string activity)
    {
        if (socialStats.ContainsKey(activity))
        {
            socialStats[activity]++;
        }
        else
        {
            socialStats[activity] = 1;
        }
        
        Debug.Log($"记录社交活动: {activity} - {socialStats[activity]}");
    }
    
    public void ShowSocialStatistics()
    {
        Debug.Log("=== 社交统计 ===");
        
        foreach (var kvp in socialStats.OrderByDescending(x => x.Value))
        {
            Debug.Log($"  {kvp.Key}: {kvp.Value} 次");
        }
    }
    
    public int GetSocialActivityCount(string activity)
    {
        return socialStats.ContainsKey(activity) ? socialStats[activity] : 0;
    }
    
    public string GetMostActiveSocialActivity()
    {
        if (socialStats.Count > 0)
        {
            return socialStats.OrderByDescending(x => x.Value).First().Key;
        }
        return "无数据";
    }
    
    public void SaveSocialStatistics()
    {
        SavesSystem.Save("SocialStats", socialStats);
        Debug.Log("社交统计已保存");
    }
    
    public void LoadSocialStatistics()
    {
        socialStats = SavesSystem.Load("SocialStats", new Dictionary<string, int>());
        Debug.Log("社交统计已加载");
    }
}
```

## 注意事项

1. 社交系统需要平台支持
2. 用户认证是社交功能的基础
3. 成就同步需要网络连接
4. 富状态系统用于显示游戏状态
5. 社交系统与成就系统、统计系统紧密相关
6. 社交功能需要用户授权
7. 富状态更新应该及时准确
8. 社交统计有助于了解用户行为
9. 社交系统需要处理网络异常
10. 社交功能应该支持离线模式

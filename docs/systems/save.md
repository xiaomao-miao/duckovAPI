# 存档系统

## 概述
存档系统是《逃离鸭科夫》的重要系统之一，负责管理游戏数据的保存、加载、删除等功能。

## 核心组件

### SavesSystem
存档系统，管理游戏数据的保存和加载。

#### 主要功能
- **数据保存** - 保存游戏数据到文件
- **数据加载** - 从文件加载游戏数据
- **数据管理** - 删除、检查数据存在
- **事件通知** - 收集存档数据、加载存档数据事件

#### 重要属性
- `CurrentSlot` - 当前存档槽
- `CurrentFilePath` - 当前存档文件路径
- `SavesFolder` - 存档文件夹路径

#### 重要事件
- `OnCollectSaveData` - 收集存档数据事件
- `OnLoadSaveData` - 加载存档数据事件

#### 重要方法
- `Save<T>(string key, T value)` - 保存数据
- `Load<T>(string key, T defaultValue)` - 加载数据
- `SaveGlobal<T>(string key, T value)` - 保存全局数据
- `LoadGlobal<T>(string key, T defaultValue)` - 加载全局数据
- `DeleteKey(string key)` - 删除键
- `KeyExists(string key)` - 检查键是否存在
- `GetFilePath(int slot)` - 获取指定槽的文件路径
- `SaveToFile()` - 保存到文件
- `LoadFromFile()` - 从文件加载

## 使用示例

### 基本数据保存和加载
```csharp
// 保存数据
SavesSystem.Save("PlayerName", "Player1");
SavesSystem.Save("PlayerLevel", 5);
SavesSystem.Save("PlayerHealth", 100.0f);
SavesSystem.Save("PlayerPosition", new Vector3(10, 0, 10));

// 加载数据
string playerName = SavesSystem.Load("PlayerName", "DefaultPlayer");
int playerLevel = SavesSystem.Load("PlayerLevel", 1);
float playerHealth = SavesSystem.Load("PlayerHealth", 100.0f);
Vector3 playerPosition = SavesSystem.Load("PlayerPosition", Vector3.zero);

Debug.Log($"玩家名称: {playerName}");
Debug.Log($"玩家等级: {playerLevel}");
Debug.Log($"玩家生命值: {playerHealth}");
Debug.Log($"玩家位置: {playerPosition}");
```

### 全局数据管理
```csharp
// 保存全局数据
SavesSystem.SaveGlobal("GameVersion", "1.0.0");
SavesSystem.SaveGlobal("TotalPlayTime", 3600);
SavesSystem.SaveGlobal("Settings", gameSettings);

// 加载全局数据
string gameVersion = SavesSystem.LoadGlobal("GameVersion", "0.0.0");
int totalPlayTime = SavesSystem.LoadGlobal("TotalPlayTime", 0);
GameSettings settings = SavesSystem.LoadGlobal("Settings", new GameSettings());

Debug.Log($"游戏版本: {gameVersion}");
Debug.Log($"总游戏时间: {totalPlayTime}");
```

### 数据存在检查
```csharp
// 检查数据是否存在
if (SavesSystem.KeyExists("PlayerName"))
{
    string playerName = SavesSystem.Load("PlayerName", "");
    Debug.Log($"玩家名称存在: {playerName}");
}
else
{
    Debug.Log("玩家名称不存在");
}

// 删除数据
SavesSystem.DeleteKey("OldData");
Debug.Log("删除旧数据");
```

### 存档槽管理
```csharp
// 获取存档文件路径
string filePath = SavesSystem.GetFilePath(1);
Debug.Log($"存档槽1的文件路径: {filePath}");

// 设置当前存档槽
SavesSystem.CurrentSlot = 1;
Debug.Log($"当前存档槽: {SavesSystem.CurrentSlot}");

// 获取存档文件夹路径
string savesFolder = SavesSystem.SavesFolder;
Debug.Log($"存档文件夹: {savesFolder}");
```

### 文件操作
```csharp
// 保存到文件
SavesSystem.SaveToFile();
Debug.Log("数据已保存到文件");

// 从文件加载
SavesSystem.LoadFromFile();
Debug.Log("从文件加载数据");
```

### 监听存档事件
```csharp
void OnEnable()
{
    SavesSystem.OnCollectSaveData += OnCollectSaveData;
    SavesSystem.OnLoadSaveData += OnLoadSaveData;
}

void OnDisable()
{
    SavesSystem.OnCollectSaveData -= OnCollectSaveData;
    SavesSystem.OnLoadSaveData -= OnLoadSaveData;
}

private void OnCollectSaveData()
{
    Debug.Log("收集存档数据");
    // 保存自定义数据
    SavesSystem.Save("CustomData", customData);
}

private void OnLoadSaveData()
{
    Debug.Log("加载存档数据");
    // 加载自定义数据
    customData = SavesSystem.Load("CustomData", new CustomData());
}
```

### 存档系统集成
```csharp
public class SaveSystem
{
    private SavesSystem savesSystem;
    
    public SaveSystem()
    {
        savesSystem = SavesSystem.Instance;
    }
    
    public void SaveGameData()
    {
        // 保存玩家数据
        SavePlayerData();
        
        // 保存游戏状态
        SaveGameState();
        
        // 保存设置
        SaveSettings();
        
        // 保存到文件
        savesSystem.SaveToFile();
        
        Debug.Log("游戏数据已保存");
    }
    
    public void LoadGameData()
    {
        // 从文件加载
        savesSystem.LoadFromFile();
        
        // 加载玩家数据
        LoadPlayerData();
        
        // 加载游戏状态
        LoadGameState();
        
        // 加载设置
        LoadSettings();
        
        Debug.Log("游戏数据已加载");
    }
    
    private void SavePlayerData()
    {
        CharacterMainControl player = CharacterMainControl.Main;
        if (player != null)
        {
            savesSystem.Save("PlayerPosition", player.transform.position);
            savesSystem.Save("PlayerRotation", player.transform.rotation);
            savesSystem.Save("PlayerHealth", player.Health.CurrentHealth);
            savesSystem.Save("PlayerLevel", EXPManager.Level);
        }
    }
    
    private void LoadPlayerData()
    {
        CharacterMainControl player = CharacterMainControl.Main;
        if (player != null)
        {
            Vector3 position = savesSystem.Load("PlayerPosition", Vector3.zero);
            Quaternion rotation = savesSystem.Load("PlayerRotation", Quaternion.identity);
            float health = savesSystem.Load("PlayerHealth", 100f);
            int level = savesSystem.Load("PlayerLevel", 1);
            
            player.transform.position = position;
            player.transform.rotation = rotation;
            player.Health.SetHealth(health);
            // 设置等级需要根据实际系统实现
        }
    }
    
    private void SaveGameState()
    {
        savesSystem.Save("GameTime", Time.time);
        savesSystem.Save("CurrentLevel", LevelManager.GetCurrentLevelInfo());
        savesSystem.Save("Inventory", GetInventoryData());
    }
    
    private void LoadGameState()
    {
        float gameTime = savesSystem.Load("GameTime", 0f);
        string currentLevel = savesSystem.Load("CurrentLevel", "");
        InventoryData inventoryData = savesSystem.Load("Inventory", new InventoryData());
        
        // 恢复游戏状态
        RestoreGameState(gameTime, currentLevel, inventoryData);
    }
    
    private void SaveSettings()
    {
        savesSystem.Save("GraphicsSettings", graphicsSettings);
        savesSystem.Save("AudioSettings", audioSettings);
        savesSystem.Save("ControlSettings", controlSettings);
    }
    
    private void LoadSettings()
    {
        graphicsSettings = savesSystem.Load("GraphicsSettings", new GraphicsSettings());
        audioSettings = savesSystem.Load("AudioSettings", new AudioSettings());
        controlSettings = savesSystem.Load("ControlSettings", new ControlSettings());
        
        // 应用设置
        ApplySettings();
    }
}
```

### 存档验证系统
```csharp
public class SaveValidationSystem
{
    private SavesSystem savesSystem;
    
    public SaveValidationSystem()
    {
        savesSystem = SavesSystem.Instance;
    }
    
    public bool ValidateSaveData()
    {
        // 检查必要数据是否存在
        if (!savesSystem.KeyExists("PlayerName"))
        {
            Debug.LogError("存档数据不完整：缺少玩家名称");
            return false;
        }
        
        if (!savesSystem.KeyExists("PlayerLevel"))
        {
            Debug.LogError("存档数据不完整：缺少玩家等级");
            return false;
        }
        
        if (!savesSystem.KeyExists("PlayerHealth"))
        {
            Debug.LogError("存档数据不完整：缺少玩家生命值");
            return false;
        }
        
        // 检查数据有效性
        int playerLevel = savesSystem.Load("PlayerLevel", 0);
        if (playerLevel < 1 || playerLevel > 100)
        {
            Debug.LogError("存档数据无效：玩家等级超出范围");
            return false;
        }
        
        float playerHealth = savesSystem.Load("PlayerHealth", 0f);
        if (playerHealth < 0f || playerHealth > 1000f)
        {
            Debug.LogError("存档数据无效：玩家生命值超出范围");
            return false;
        }
        
        Debug.Log("存档数据验证通过");
        return true;
    }
    
    public void RepairSaveData()
    {
        Debug.Log("开始修复存档数据");
        
        // 修复缺失的数据
        if (!savesSystem.KeyExists("PlayerName"))
        {
            savesSystem.Save("PlayerName", "Player1");
            Debug.Log("修复玩家名称");
        }
        
        if (!savesSystem.KeyExists("PlayerLevel"))
        {
            savesSystem.Save("PlayerLevel", 1);
            Debug.Log("修复玩家等级");
        }
        
        if (!savesSystem.KeyExists("PlayerHealth"))
        {
            savesSystem.Save("PlayerHealth", 100f);
            Debug.Log("修复玩家生命值");
        }
        
        Debug.Log("存档数据修复完成");
    }
}
```

### 存档备份系统
```csharp
public class SaveBackupSystem
{
    private SavesSystem savesSystem;
    private string backupFolder;
    
    public SaveBackupSystem()
    {
        savesSystem = SavesSystem.Instance;
        backupFolder = Path.Combine(Application.persistentDataPath, "Backups");
        
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
        }
    }
    
    public void CreateBackup(int slot)
    {
        string sourceFile = savesSystem.GetFilePath(slot);
        string backupFile = Path.Combine(backupFolder, $"backup_slot_{slot}_{DateTime.Now:yyyyMMdd_HHmmss}.dat");
        
        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, backupFile);
            Debug.Log($"创建存档备份: {backupFile}");
        }
        else
        {
            Debug.LogWarning($"源存档文件不存在: {sourceFile}");
        }
    }
    
    public void RestoreBackup(int slot, string backupFile)
    {
        string targetFile = savesSystem.GetFilePath(slot);
        
        if (File.Exists(backupFile))
        {
            File.Copy(backupFile, targetFile, true);
            Debug.Log($"恢复存档备份: {backupFile} -> {targetFile}");
        }
        else
        {
            Debug.LogError($"备份文件不存在: {backupFile}");
        }
    }
    
    public List<string> GetAvailableBackups(int slot)
    {
        List<string> backups = new List<string>();
        string pattern = $"backup_slot_{slot}_*.dat";
        
        foreach (string file in Directory.GetFiles(backupFolder, pattern))
        {
            backups.Add(Path.GetFileName(file));
        }
        
        return backups;
    }
    
    public void CleanOldBackups(int maxBackups = 10)
    {
        List<string> backups = GetAvailableBackups(0); // 获取所有备份
        
        if (backups.Count > maxBackups)
        {
            // 按时间排序，删除最旧的备份
            backups.Sort();
            int toDelete = backups.Count - maxBackups;
            
            for (int i = 0; i < toDelete; i++)
            {
                string fileToDelete = Path.Combine(backupFolder, backups[i]);
                File.Delete(fileToDelete);
                Debug.Log($"删除旧备份: {backups[i]}");
            }
        }
    }
}
```

## 注意事项

1. 存档系统支持多种数据类型，包括基本类型和复杂对象
2. 全局数据与存档槽数据分开管理
3. 数据存在检查避免加载不存在的键
4. 存档事件监听需要正确管理，避免内存泄漏
5. 存档验证确保数据完整性
6. 存档备份系统提供数据安全保障
7. 存档系统与所有其他系统紧密相关
8. 数据保存和加载是异步操作，需要适当处理
9. 存档文件路径需要正确设置
10. 存档数据格式需要考虑版本兼容性

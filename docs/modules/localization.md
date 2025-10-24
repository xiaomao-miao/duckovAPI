# TeamSoda.MiniLocalizor - 本地化模块

## 概述
TeamSoda.MiniLocalizor 是《逃离鸭科夫》游戏的本地化模块，提供了多语言支持和本地化文本管理功能。

## 核心类

### CSVFileLocalizor
CSV文件本地化器，从CSV文件读取和管理本地化文本。

#### 属性
- `Path` - 本地化文件路径
- `Language` - 当前语言

#### 构造函数
- `CSVFileLocalizor(string path)` - 使用指定路径创建本地化器
- `CSVFileLocalizor(SystemLanguage language)` - 使用系统语言创建本地化器

#### 方法
- `BuildDictionary()` - 构建本地化字典
- `Get(string key)` - 获取本地化文本
- `GetEntry(string key)` - 获取本地化条目
- `HasKey(string key)` - 检查是否存在指定键

#### 静态方法
- `ConvertFromEscapes(string origin)` - 从转义字符转换
- `ConvertToEscapes(string origin)` - 转换为转义字符

### DataEntry
本地化数据条目，存储本地化文本的键值对。

#### 属性
- `key` - 本地化键
- `value` - 本地化值
- `version` - 版本信息
- `sheet` - 工作表名称

#### 方法
- `IsNewerThan(string version)` - 检查版本是否更新

### ILocalizationProvider
本地化提供者接口，定义本地化功能的基本接口。

#### 方法
- `Get(string key)` - 获取本地化文本

## 使用示例

### 创建本地化器
```csharp
// 使用文件路径创建
CSVFileLocalizor localizor = new CSVFileLocalizor("Assets/Localization/Chinese.csv");

// 使用系统语言创建
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.Chinese);
```

### 获取本地化文本
```csharp
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.English);

// 获取本地化文本
string text = localizor.Get("PlayerName");
if (text != null)
{
    Debug.Log($"本地化文本: {text}");
}
else
{
    Debug.LogWarning("未找到本地化文本");
}
```

### 检查本地化键是否存在
```csharp
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.Chinese);

if (localizor.HasKey("PlayerName"))
{
    string text = localizor.Get("PlayerName");
    Debug.Log($"玩家名称: {text}");
}
else
{
    Debug.LogWarning("本地化键不存在");
}
```

### 获取本地化条目
```csharp
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.English);

DataEntry entry = localizor.GetEntry("PlayerName");
if (entry != null)
{
    Debug.Log($"键: {entry.key}");
    Debug.Log($"值: {entry.value}");
    Debug.Log($"版本: {entry.version}");
    Debug.Log($"工作表: {entry.sheet}");
}
```

### 版本比较
```csharp
DataEntry entry = localizor.GetEntry("PlayerName");
if (entry != null)
{
    if (entry.IsNewerThan("1.0"))
    {
        Debug.Log("本地化条目版本更新");
    }
    else
    {
        Debug.Log("本地化条目版本较旧");
    }
}
```

### 重新构建本地化字典
```csharp
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.Chinese);

// 重新构建字典（当文件更新时）
localizor.BuildDictionary();

// 获取更新后的文本
string text = localizor.Get("UpdatedText");
```

### 处理转义字符
```csharp
// 本地化文本中的转义字符会被自动处理
string text = localizor.Get("TextWithNewLine");
// 如果CSV中存储的是 "Hello\\nWorld"，会转换为 "Hello\nWorld"

Debug.Log(text); // 输出: Hello
                 //       World
```

### 使用接口
```csharp
public class LocalizationManager
{
    private ILocalizationProvider provider;
    
    public LocalizationManager(ILocalizationProvider provider)
    {
        this.provider = provider;
    }
    
    public string GetLocalizedText(string key)
    {
        return provider.Get(key);
    }
}

// 使用
CSVFileLocalizor localizor = new CSVFileLocalizor(SystemLanguage.Chinese);
LocalizationManager manager = new LocalizationManager(localizor);
string text = manager.GetLocalizedText("PlayerName");
```

### 多语言支持
```csharp
public class MultiLanguageManager
{
    private Dictionary<SystemLanguage, CSVFileLocalizor> localizors;
    
    public MultiLanguageManager()
    {
        localizors = new Dictionary<SystemLanguage, CSVFileLocalizor>();
        
        // 初始化多种语言
        localizors[SystemLanguage.English] = new CSVFileLocalizor(SystemLanguage.English);
        localizors[SystemLanguage.Chinese] = new CSVFileLocalizor(SystemLanguage.Chinese);
        localizors[SystemLanguage.Japanese] = new CSVFileLocalizor(SystemLanguage.Japanese);
    }
    
    public string GetText(string key, SystemLanguage language)
    {
        if (localizors.TryGetValue(language, out CSVFileLocalizor localizor))
        {
            return localizor.Get(key);
        }
        return null;
    }
    
    public void SwitchLanguage(SystemLanguage language)
    {
        if (localizors.ContainsKey(language))
        {
            // 切换语言逻辑
            Debug.Log($"切换到语言: {language}");
        }
    }
}
```

### 本地化文本缓存
```csharp
public class LocalizationCache
{
    private Dictionary<string, string> cache;
    private CSVFileLocalizor localizor;
    
    public LocalizationCache(CSVFileLocalizor localizor)
    {
        this.localizor = localizor;
        this.cache = new Dictionary<string, string>();
    }
    
    public string GetCachedText(string key)
    {
        if (cache.TryGetValue(key, out string cachedText))
        {
            return cachedText;
        }
        
        string text = localizor.Get(key);
        if (text != null)
        {
            cache[key] = text;
        }
        return text;
    }
    
    public void ClearCache()
    {
        cache.Clear();
    }
}
```

### 本地化文本验证
```csharp
public class LocalizationValidator
{
    public bool ValidateLocalization(CSVFileLocalizor localizor, string[] requiredKeys)
    {
        foreach (string key in requiredKeys)
        {
            if (!localizor.HasKey(key))
            {
                Debug.LogError($"缺少本地化键: {key}");
                return false;
            }
            
            string text = localizor.Get(key);
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning($"本地化文本为空: {key}");
            }
        }
        return true;
    }
}
```

## 注意事项

1. 本地化文件必须放在 `StreamingAssets/Localization/` 目录下
2. 文件名必须与 `SystemLanguage` 枚举值对应
3. CSV文件格式必须正确，包含键值对
4. 本地化文本中的转义字符会被自动处理
5. 版本比较支持数字版本和带#前缀的特殊版本
6. 本地化器在创建时会自动构建字典，无需手动调用
7. 如果本地化文件不存在，会创建空文件
8. 使用哈希值访问比字符串键更快
9. 本地化文本会缓存，提高性能
10. 支持多种语言切换，但需要重新构建字典

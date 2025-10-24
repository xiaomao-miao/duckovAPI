# TeamSoda.Duckov.Utilities - 工具模块

## 概述
TeamSoda.Duckov.Utilities 是《逃离鸭科夫》游戏的工具模块，提供了自定义数据管理、对象池等实用功能。

## 核心类

### CustomData
自定义数据类，用于存储和管理各种类型的数据。

#### 属性
- `Key` - 数据键
- `DataType` - 数据类型
- `Display` - 是否显示
- `DisplayName` - 显示名称

#### 事件
- `OnSetData` - 数据设置事件

#### 方法
- `GetFloat()` - 获取浮点数值
- `SetFloat(float value)` - 设置浮点数值
- `GetInt()` - 获取整数值
- `SetInt(int value)` - 设置整数值
- `GetBool()` - 获取布尔值
- `SetBool(bool value)` - 设置布尔值
- `GetString()` - 获取字符串值
- `SetString(string value)` - 设置字符串值
- `GetRawCopied()` - 获取原始数据副本
- `SetRaw(byte[] value)` - 设置原始数据
- `GetValueDisplayString(string format = "")` - 获取值的显示字符串

#### 构造函数
- `CustomData(string key, CustomDataType dataType, byte[] data)` - 使用键、类型和原始数据创建
- `CustomData(string key, float floatValue)` - 创建浮点数数据
- `CustomData(string key, int intValue)` - 创建整数数据
- `CustomData(string key, bool boolValue)` - 创建布尔数据
- `CustomData(string key, string stringValue)` - 创建字符串数据
- `CustomData()` - 默认构造函数
- `CustomData(CustomData copyFrom)` - 复制构造函数

### CustomDataCollection
自定义数据集合，管理多个CustomData对象。

#### 属性
- `Count` - 数据条目数量
- `IsReadOnly` - 是否只读

#### 方法
- `GetEntry(int hash)` - 通过哈希获取条目
- `GetEntry(string key)` - 通过键获取条目
- `SetRaw(string key, CustomDataType type, byte[] bytes, bool createNewIfNotExist = true)` - 设置原始数据
- `SetRaw(int hash, CustomDataType type, byte[] bytes)` - 通过哈希设置原始数据
- `GetRawCopied(string key, byte[] defaultResult = null)` - 获取原始数据副本
- `GetRawCopied(int hash, byte[] defaultResult = null)` - 通过哈希获取原始数据副本

#### 获取方法
- `GetFloat(string key, float defaultResult = 0f)` - 获取浮点数
- `GetInt(string key, int defaultResult = 0)` - 获取整数
- `GetBool(string key, bool defaultResult = false)` - 获取布尔值
- `GetString(string key, string defaultResult = null)` - 获取字符串
- `GetFloat(int hash, float defaultResult = 0f)` - 通过哈希获取浮点数
- `GetInt(int hash, int defaultResult = 0)` - 通过哈希获取整数
- `GetBool(int hash, bool defaultResult = false)` - 通过哈希获取布尔值
- `GetString(int hash, string defaultResult = null)` - 通过哈希获取字符串

#### 设置方法
- `SetFloat(string key, float value, bool createNewIfNotExist = true)` - 设置浮点数
- `SetInt(string key, int value, bool createNewIfNotExist = true)` - 设置整数
- `SetBool(string key, bool value, bool createNewIfNotExist = true)` - 设置布尔值
- `SetString(string key, string value, bool createNewIfNotExist = true)` - 设置字符串
- `SetFloat(int hash, float value)` - 通过哈希设置浮点数
- `SetInt(int hash, int value)` - 通过哈希设置整数
- `SetBool(int hash, bool value)` - 通过哈希设置布尔值
- `SetString(int hash, string value)` - 通过哈希设置字符串

### PrefabPool<T>
预制体对象池，用于高效管理游戏对象的创建和销毁。

#### 属性
- `Prefab` - 预制体
- `poolParent` - 池父对象
- `CollectionCheck` - 集合检查
- `DefaultCapacity` - 默认容量
- `MaxSize` - 最大大小
- `ActiveEntries` - 活动条目集合

#### 构造函数
- `PrefabPool(T prefab, Transform poolParent = null, Action<T> onGet = null, Action<T> onRelease = null, Action<T> onDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000, Action<T> onCreate = null)` - 创建对象池

#### 方法
- `Get(Transform setParent = null)` - 获取对象
- `Release(T item)` - 释放对象
- `ReleaseAll()` - 释放所有对象
- `Find(Predicate<T> predicate)` - 查找对象
- `ReleaseAll(Predicate<T> predicate)` - 释放符合条件的对象

## 重要枚举

### CustomDataType
自定义数据类型枚举
- `Raw` - 原始数据
- `Float` - 浮点数
- `Int` - 整数
- `Bool` - 布尔值
- `String` - 字符串

## 使用示例

### 创建和使用CustomData
```csharp
// 创建浮点数数据
CustomData floatData = new CustomData("PlayerHealth", 100.0f);

// 创建整数数据
CustomData intData = new CustomData("PlayerLevel", 5);

// 创建布尔数据
CustomData boolData = new CustomData("IsAlive", true);

// 创建字符串数据
CustomData stringData = new CustomData("PlayerName", "Player1");

// 获取和设置值
float health = floatData.GetFloat();
floatData.SetFloat(80.0f);

int level = intData.GetInt();
intData.SetInt(6);

bool isAlive = boolData.GetBool();
boolData.SetBool(false);

string name = stringData.GetString();
stringData.SetString("Player2");
```

### 使用CustomDataCollection
```csharp
CustomDataCollection collection = new CustomDataCollection();

// 添加数据
collection.SetFloat("Health", 100.0f);
collection.SetInt("Level", 5);
collection.SetBool("IsAlive", true);
collection.SetString("Name", "Player1");

// 获取数据
float health = collection.GetFloat("Health", 0f);
int level = collection.GetInt("Level", 0);
bool isAlive = collection.GetBool("IsAlive", false);
string name = collection.GetString("Name", "");

// 检查数据是否存在
CustomData healthData = collection.GetEntry("Health");
if (healthData != null)
{
    Debug.Log($"Health: {healthData.GetFloat()}");
}
```

### 使用PrefabPool
```csharp
// 创建对象池
PrefabPool<GameObject> bulletPool = new PrefabPool<GameObject>(
    bulletPrefab,
    poolParent,
    onGet: (bullet) => bullet.SetActive(true),
    onRelease: (bullet) => bullet.SetActive(false),
    onDestroy: (bullet) => Destroy(bullet),
    defaultCapacity: 50,
    maxSize: 200
);

// 获取对象
GameObject bullet = bulletPool.Get();

// 使用对象
bullet.transform.position = firePoint.position;
bullet.GetComponent<Rigidbody>().velocity = direction * speed;

// 释放对象
bulletPool.Release(bullet);

// 释放所有对象
bulletPool.ReleaseAll();

// 查找特定对象
GameObject foundBullet = bulletPool.Find(b => b.GetComponent<Bullet>().damage > 50);

// 释放符合条件的对象
int releasedCount = bulletPool.ReleaseAll(b => b.GetComponent<Bullet>().isExpired);
```

### 监听CustomData事件
```csharp
CustomData data = new CustomData("TestValue", 10.0f);

// 订阅数据变化事件
data.OnSetData += OnDataChanged;

// 事件处理函数
private void OnDataChanged(CustomData data)
{
    Debug.Log($"数据 {data.Key} 已更新");
}

// 取消订阅
data.OnSetData -= OnDataChanged;
```

### 批量操作CustomDataCollection
```csharp
CustomDataCollection collection = new CustomDataCollection();

// 批量添加数据
collection.SetFloat("Health", 100.0f);
collection.SetFloat("Mana", 50.0f);
collection.SetInt("Level", 5);
collection.SetBool("IsAlive", true);

// 遍历所有数据
foreach (CustomData data in collection)
{
    Debug.Log($"{data.Key}: {data.GetValueDisplayString()}");
}

// 检查数据是否存在
if (collection.Contains(healthData))
{
    Debug.Log("健康数据存在");
}
```

## 注意事项

1. CustomData的键必须唯一，避免冲突
2. 数据类型一旦设置就不能更改，除非重新创建
3. CustomDataCollection会自动管理内部字典，无需手动维护
4. PrefabPool使用对象池模式，提高性能，避免频繁创建销毁对象
5. 对象池中的对象在释放后会被重置，确保下次使用时状态正确
6. 使用IPoolable接口可以实现对象池生命周期管理
7. 对象池的容量设置要合理，避免内存浪费或性能问题
8. 自定义数据会保存在存档中，请谨慎使用
9. 使用哈希值访问数据比字符串键更快，但可读性较差
10. 对象池的父对象设置会影响对象的层级关系

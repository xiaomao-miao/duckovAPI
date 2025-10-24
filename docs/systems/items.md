# 物品系统

## 概述
物品系统是《逃离鸭科夫》的核心系统之一，负责管理游戏中的所有物品、属性、效果、背包等功能。

## 核心组件

### Item
物品类，游戏中最基础的对象，所有物品都继承自此类。

#### 主要功能
- **物品属性** - 名称、描述、图标、价值、品质
- **堆叠系统** - 最大堆叠数量、当前堆叠数量
- **重量系统** - 自身重量、总重量计算
- **耐久度系统** - 最大耐久度、当前耐久度、修复
- **变量系统** - 自定义变量存储
- **统计属性** - 伤害、防御等数值属性
- **效果系统** - 被动效果、触发效果
- **插槽系统** - 装备插槽、连接系统

#### 重要属性
- `TypeID` - 物品类型ID
- `DisplayName` - 显示名称
- `Description` - 描述
- `Icon` - 图标
- `MaxStackCount` - 最大堆叠数量
- `Stackable` - 是否可堆叠
- `Value` - 价值
- `Quality` - 品质
- `TotalWeight` - 总重量
- `Durability` - 当前耐久度
- `MaxDurability` - 最大耐久度
- `Variables` - 变量
- `Stats` - 统计属性
- `Effects` - 效果

#### 重要事件
- `onUse` - 使用事件
- `onUseStatic` - 静态使用事件
- `onDestroy` - 销毁事件
- `onDurabilityChanged` - 耐久度改变事件
- `onItemTreeChanged` - 物品树改变事件

### ItemAssetsCollection
物品资产集合，管理游戏中所有物品的预制体和元数据。

#### 主要功能
- **物品管理** - 预制体管理、元数据管理
- **动态物品** - 添加、移除动态物品
- **物品搜索** - 按条件搜索物品
- **物品实例化** - 异步、同步创建物品

#### 重要方法
- `InstantiateAsync(int typeID)` - 异步实例化物品
- `InstantiateSync(int typeID)` - 同步实例化物品
- `AddDynamicEntry(Item prefab)` - 添加动态条目
- `RemoveDynamicEntry(Item prefab)` - 移除动态条目
- `Search(ItemFilter filter)` - 搜索物品

### Inventory
背包类，管理物品的存储和检索。

#### 主要功能
- **容量管理** - 背包容量、重量限制
- **物品存储** - 添加、移除物品
- **物品合并** - 自动合并相同物品
- **重量计算** - 实时计算总重量

#### 重要属性
- `Capacity` - 容量
- `Content` - 内容列表
- `CachedWeight` - 缓存重量

#### 重要事件
- `onContentChanged` - 内容改变事件

### Slot
插槽类，用于物品的装备和连接。

#### 主要功能
- **物品装备** - 插入、拔出物品
- **连接管理** - 物品之间的连接关系
- **事件通知** - 插槽内容改变通知

#### 重要属性
- `Key` - 键
- `Content` - 内容
- `Master` - 主物品

### Stat
统计属性类，管理物品的各种数值属性。

#### 主要功能
- **属性管理** - 基础值、当前值
- **修饰符系统** - 添加、移除修饰符
- **数值计算** - 自动计算最终值

#### 重要属性
- `Key` - 键
- `BaseValue` - 基础值
- `Value` - 当前值
- `Modifiers` - 修饰符列表

### Modifier
修饰符类，用于修改统计属性的值。

#### 主要功能
- **数值修改** - 加法、乘法、百分比
- **源管理** - 标识修饰符来源
- **类型管理** - 不同类型的修饰符

#### 重要属性
- `Value` - 值
- `Type` - 类型
- `Source` - 源

### Effect
效果类，物品的被动效果。

#### 主要功能
- **效果触发** - 条件触发、动作执行
- **效果管理** - 添加、移除效果
- **效果计算** - 效果数值计算

#### 重要属性
- `Item` - 关联的物品

## 使用示例

### 创建物品实例
```csharp
// 异步创建物品
Item item = await ItemAssetsCollection.InstantiateAsync(typeID);

// 同步创建物品
Item item = ItemAssetsCollection.InstantiateSync(typeID);

// 检查物品是否创建成功
if (item != null)
{
    Debug.Log($"物品创建成功: {item.DisplayName}");
}
```

### 获取物品属性
```csharp
Item item = GetItem();

// 基本属性
string name = item.DisplayName;
string description = item.Description;
Sprite icon = item.Icon;
int value = item.GetTotalRawValue();
float weight = item.TotalWeight;

// 堆叠属性
int stackCount = item.StackCount;
int maxStack = item.MaxStackCount;
bool stackable = item.Stackable;

// 耐久度属性
float durability = item.Durability;
float maxDurability = item.MaxDurability;
bool repairable = item.Repairable;
```

### 设置物品变量
```csharp
Item item = GetItem();

// 设置各种类型的变量
item.SetFloat("CustomValue", 10.5f);
item.SetInt("CustomCount", 5);
item.SetBool("CustomFlag", true);
item.SetString("CustomText", "Hello World");

// 设置变量时创建新条目
item.SetFloat("NewValue", 20.0f, true);
```

### 获取物品变量
```csharp
Item item = GetItem();

// 获取各种类型的变量
float customValue = item.GetFloat("CustomValue", 0f);
int customCount = item.GetInt("CustomCount", 0);
bool customFlag = item.GetBool("CustomFlag", false);
string customText = item.GetString("CustomText", "");

// 检查变量是否存在
if (item.GetVariableEntry("CustomValue") != null)
{
    Debug.Log("变量存在");
}
```

### 监听物品事件
```csharp
void OnEnable()
{
    Item.onUseStatic += OnItemUsed;
    Item.onDestroy += OnItemDestroyed;
}

void OnDisable()
{
    Item.onUseStatic -= OnItemUsed;
    Item.onDestroy -= OnItemDestroyed;
}

private void OnItemUsed(Item item, object user)
{
    Debug.Log($"物品 {item.DisplayName} 被 {user} 使用");
}

private void OnItemDestroyed(Item item)
{
    Debug.Log($"物品 {item.DisplayName} 被销毁");
}
```

### 物品合并和分割
```csharp
Item item1 = GetItem();
Item item2 = GetItem();

// 合并物品
bool combined = item1.Combine(item2);
if (combined)
{
    Debug.Log("物品合并成功");
}

// 分割物品
Item splitItem = await item1.Split(5);
if (splitItem != null)
{
    Debug.Log("物品分割成功");
}
```

### 获取物品统计属性
```csharp
Item item = GetItem();

// 获取统计属性
Stat damageStat = item.GetStat("Damage");
if (damageStat != null)
{
    float damage = damageStat.Value;
    Debug.Log($"伤害值: {damage}");
}

// 直接获取统计属性值
float damage = item.GetStatValue("Damage");
float defense = item.GetStatValue("Defense");
```

### 管理统计属性修饰符
```csharp
Item item = GetItem();
Stat damageStat = item.GetStat("Damage");

if (damageStat != null)
{
    // 添加修饰符
    Modifier damageModifier = new Modifier(10f, ModifierType.Add, this);
    damageStat.AddModifier(damageModifier);
    
    // 获取最终值
    float finalDamage = damageStat.Value;
    
    // 移除修饰符
    damageStat.RemoveModifier(damageModifier);
}
```

### 添加动态物品
```csharp
// 创建自定义物品
Item customItem = CreateCustomItem();

// 添加到游戏中
bool success = ItemAssetsCollection.AddDynamicEntry(customItem);
if (success)
{
    Debug.Log("动态物品添加成功");
}

// 移除动态物品
ItemAssetsCollection.RemoveDynamicEntry(customItem);
```

### 搜索物品
```csharp
// 创建搜索过滤器
ItemFilter filter = new ItemFilter();
filter.requireTags = new string[] { "Weapon" };
filter.minQuality = 1;
filter.maxQuality = 5;
filter.caliber = "7.62";

// 搜索物品
int[] itemIds = ItemAssetsCollection.Search(filter);
foreach (int itemId in itemIds)
{
    Debug.Log($"找到物品ID: {itemId}");
}
```

### 背包管理
```csharp
Inventory inventory = GetInventory();

// 添加物品
Item newItem = GetItem();
inventory.AddItem(newItem);

// 移除物品
inventory.RemoveItem(newItem);

// 设置容量
inventory.SetCapacity(100);

// 重新计算重量
inventory.RecalculateWeight();

// 监听背包变化
inventory.onContentChanged += OnInventoryChanged;

private void OnInventoryChanged(Item item)
{
    Debug.Log($"背包内容变化: {item.DisplayName}");
}
```

### 插槽管理
```csharp
Slot slot = GetSlot();

// 插入物品
Item item = GetItem();
slot.Plug(item);

// 拔出物品
slot.Unplug();

// 检查插槽内容
if (slot.Content != null)
{
    Debug.Log($"插槽中有物品: {slot.Content.DisplayName}");
}
```

### 物品效果系统
```csharp
Item item = GetItem();

// 添加效果
Effect customEffect = CreateCustomEffect();
item.AddEffect(customEffect);

// 检查效果
if (item.Effects != null && item.Effects.Count > 0)
{
    Debug.Log($"物品有 {item.Effects.Count} 个效果");
}
```

## 注意事项

1. 物品的TypeID必须唯一，避免冲突
2. 动态添加的物品在游戏重启后会丢失，需要重新添加
3. 物品的堆叠数量不能超过MaxStackCount
4. 使用物品前请检查IsUsable方法
5. 物品的耐久度为0时，某些效果可能不会生效
6. 物品的标签用于分类和过滤，请合理使用
7. 自定义变量会保存在存档中，请谨慎使用
8. 物品的父级关系会影响其效果和重量计算
9. 统计属性的修饰符按顺序计算，顺序很重要
10. 物品代理的生命周期与物品绑定，物品销毁时代理也会销毁

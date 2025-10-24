# ItemStatsSystem - 物品统计系统

## 概述
ItemStatsSystem 是《逃离鸭科夫》的物品统计系统，负责管理游戏中的所有物品、属性、效果、背包等功能。

## 核心类

### Item
物品类，游戏中最基础的对象，所有物品都继承自此类。

#### 属性
- `TypeID` - 物品类型ID
- `Order` - 排序
- `DisplayName` - 显示名称
- `DisplayNameRaw` - 原始显示名称
- `Description` - 描述
- `DescriptionRaw` - 原始描述
- `Icon` - 图标
- `MaxStackCount` - 最大堆叠数量
- `Stackable` - 是否可堆叠
- `Value` - 价值
- `Quality` - 品质
- `DisplayQuality` - 显示品质
- `UnitSelfWeight` - 单位自身重量
- `SelfWeight` - 自身重量
- `Sticky` - 是否粘性（不可丢弃）
- `CanBeSold` - 是否可出售
- `CanDrop` - 是否可丢弃
- `TotalWeight` - 总重量
- `HasHandHeldAgent` - 是否有手持代理
- `AgentUtilities` - 代理工具
- `ActiveAgent` - 活动代理
- `ItemGraphic` - 物品图形
- `Stats` - 统计属性
- `Modifiers` - 修饰符
- `Slots` - 插槽
- `Inventory` - 背包
- `Effects` - 效果
- `PluggedIntoSlot` - 插入的插槽
- `InInventory` - 所在背包
- `ParentItem` - 父物品
- `ParentObject` - 父对象
- `Tags` - 标签
- `Variables` - 变量
- `Constants` - 常量
- `IsCharacter` - 是否是角色
- `StackCount` - 堆叠数量
- `UseDurability` - 是否使用耐久度
- `MaxDurability` - 最大耐久度
- `MaxDurabilityWithLoss` - 考虑损失的最大耐久度
- `DurabilityLoss` - 耐久度损失
- `Durability` - 当前耐久度
- `Inspected` - 是否已检查
- `Inspecting` - 是否正在检查
- `NeedInspection` - 是否需要检查
- `IsBeingDestroyed` - 是否正在被销毁
- `Repairable` - 是否可修复
- `SoundKey` - 声音键

#### 事件
- `onItemTreeChanged` - 物品树改变事件
- `onDestroy` - 销毁事件
- `onSetStackCount` - 设置堆叠数量事件
- `onDurabilityChanged` - 耐久度改变事件
- `onInspectionStateChanged` - 检查状态改变事件
- `onUse` - 使用事件
- `onUseStatic` - 静态使用事件
- `onChildChanged` - 子物品改变事件
- `onParentChanged` - 父物品改变事件
- `onPluggedIntoSlot` - 插入插槽事件
- `onUnpluggedFromSlot` - 从插槽拔出事件
- `onSlotContentChanged` - 插槽内容改变事件
- `onSlotTreeChanged` - 插槽树改变事件

### ItemAssetsCollection
物品资产集合，管理游戏中所有物品的预制体和元数据。

#### 静态属性
- `Instance` - 获取实例

#### 属性
- `NextTypeID` - 下一个类型ID

#### 静态方法
- `AddDynamicEntry(Item prefab)` - 添加动态条目
- `RemoveDynamicEntry(Item prefab)` - 移除动态条目
- `InstantiateAsync(int typeID)` - 异步实例化物品
- `InstantiateSync(int typeID)` - 同步实例化物品
- `GetMetaData(int typeID)` - 获取元数据
- `GetPrefab(int typeID)` - 获取预制体
- `GetAllTypeIds(ItemFilter filter)` - 获取所有类型ID
- `Search(ItemFilter filter)` - 搜索物品
- `TryGetIDByName(string name)` - 尝试通过名称获取ID

### Inventory
背包类，管理物品的存储和检索。

#### 属性
- `Capacity` - 容量
- `Content` - 内容列表
- `Loading` - 是否正在加载
- `AttachedToItem` - 附加到的物品
- `NeedInspection` - 是否需要检查
- `CachedWeight` - 缓存重量

#### 事件
- `onContentChanged` - 内容改变事件

#### 方法
- `AddItem(Item item)` - 添加物品
- `RemoveItem(Item item)` - 移除物品
- `AddAndMerge(Item item, int index)` - 添加并合并
- `SetCapacity(int capacity)` - 设置容量
- `RecalculateWeight()` - 重新计算重量
- `NotifyContentChanged(Item item)` - 通知内容改变

### Slot
插槽类，用于物品的装备和连接。

#### 属性
- `Key` - 键
- `Content` - 内容
- `Master` - 主物品
- `Hash` - 哈希值

#### 方法
- `Plug(Item item)` - 插入物品
- `Unplug()` - 拔出物品
- `ForceInvokeSlotContentChangedEvent()` - 强制触发插槽内容改变事件

### Stat
统计属性类，管理物品的各种数值属性。

#### 属性
- `Key` - 键
- `BaseValue` - 基础值
- `Value` - 当前值
- `Modifiers` - 修饰符列表

#### 方法
- `AddModifier(Modifier modifier)` - 添加修饰符
- `RemoveModifier(Modifier modifier)` - 移除修饰符
- `RemoveAllModifiersFromSource(object source)` - 移除来自指定源的所有修饰符

### Modifier
修饰符类，用于修改统计属性的值。

#### 属性
- `Value` - 值
- `Type` - 类型
- `Source` - 源

### Effect
效果类，物品的被动效果。

#### 属性
- `Item` - 关联的物品

#### 方法
- `SetItem(Item item)` - 设置关联物品

## 使用示例

### 创建物品实例
```csharp
// 异步创建
Item item = await ItemAssetsCollection.InstantiateAsync(typeID);

// 同步创建
Item item = ItemAssetsCollection.InstantiateSync(typeID);
```

### 获取物品属性
```csharp
Item item = GetItem();
float weight = item.TotalWeight;
int value = item.GetTotalRawValue();
bool canSell = item.CanBeSold;
```

### 设置物品变量
```csharp
Item item = GetItem();
item.SetFloat("CustomValue", 10.5f);
item.SetInt("CustomCount", 5);
item.SetBool("CustomFlag", true);
item.SetString("CustomText", "Hello");
```

### 获取物品变量
```csharp
Item item = GetItem();
float customValue = item.GetFloat("CustomValue", 0f);
int customCount = item.GetInt("CustomCount", 0);
bool customFlag = item.GetBool("CustomFlag", false);
string customText = item.GetString("CustomText", "");
```

### 监听物品事件
```csharp
void OnEnable()
{
    Item.onUseStatic += OnItemUsed;
}

void OnDisable()
{
    Item.onUseStatic -= OnItemUsed;
}

private void OnItemUsed(Item item, object user)
{
    Debug.Log($"物品 {item.DisplayName} 被 {user} 使用");
}
```

### 添加动态物品
```csharp
// 添加自定义物品到游戏中
Item customItem = CreateCustomItem();
bool success = ItemAssetsCollection.AddDynamicEntry(customItem);

// 移除自定义物品
ItemAssetsCollection.RemoveDynamicEntry(customItem);
```

### 搜索物品
```csharp
ItemFilter filter = new ItemFilter();
filter.tags = new string[] { "Weapon" };
filter.minQuality = 1;
filter.maxQuality = 5;

int[] itemIds = ItemAssetsCollection.Search(filter);
```

### 物品合并和分割
```csharp
Item item1 = GetItem();
Item item2 = GetItem();

// 合并物品
item1.Combine(item2);

// 分割物品
Item splitItem = await item1.Split(5);
```

### 获取物品统计属性
```csharp
Item item = GetItem();
Stat damageStat = item.GetStat("Damage");
if (damageStat != null)
{
    float damage = damageStat.Value;
}

// 或者直接获取值
float damage = item.GetStatValue("Damage");
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

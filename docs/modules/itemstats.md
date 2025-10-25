# ItemStatsSystem 模块

> 对应程序集：`ItemStatsSystem.dll`

ItemStatsSystem 承担 Duckov 物品、背包、统计与效果的全部运行时代码。下列内容按照职能对反编译程序集中的公开类型、枚举、事件与主要方法进行归档，帮助 Mod 开发者对照 API。

## 1. 核心物品与容器

### `Item`
- 继承 `MonoBehaviour`、实现 `ISelfValidator`，是所有物品的宿主组件。
- **识别与展示**：`TypeID`（内部设定）、`Order`、`DisplayName`/`DisplayNameRaw`、`Description`/`DescriptionRaw`、`Icon`、`DisplayQuality`、`SoundKey`。
- **重量与堆叠**：`UnitSelfWeight`、`SelfWeight`、`TotalWeight`（包含子物品）、`MaxStackCount`、`StackCount`、`Stackable`，提供 `SetStackCount`、`TryMergeStack`、`Combine`、`CombineInto`、`Split(int count)`（返回 `UniTask<Item>`）等操作。
- **状态管理**：`Sticky`、`CanBeSold`、`CanDrop`、`IsCharacter`、`Repairable`、`UseDurability`、`Durability`/`MaxDurability`/`DurabilityLoss`、`NeedInspection`、`Inspected`、`Inspecting`、`IsBeingDestroyed` 等。
- **组件引用**：
  - `AgentUtilities` / `ActiveAgent`（代理系统）。
  - `ItemGraphic`（`ItemGraphicInfo`）。
  - `Stats` (`StatCollection`)、`Modifiers` (`ModifierDescriptionCollection`)、`Slots` (`SlotCollection`)、`Inventory`、`Effects` (`List<Effect>`)、`UsageUtilities`。
  - 自定义数据：`Variables`、`Constants` (`List<CustomData>`)、`Tags` (`TagCollection`)。
- **层级关系**：`ParentObject`、`ParentItem`、`PluggedIntoSlot`、`InInventory`、`IsAttached`、`GetAllChildren(includeInactive, includeSelf)`、`GetRootParent()`。
- **交互方法**：`Use(object user)`（委托给 `UsageUtilities`）、`Detach()`、`NotifyAddedToInventory`/`NotifyRemovedFromInventory`、`PlugInto(Slot slot)`、`Unplug()`、`RecalculateTotalWeight()`、`RebuildAllComponents()`、`Inspect()`、`RequestInspection()`、`Repair(float delta)`、`NotifyUsed()`、`DestroySelf()` 等。
- **事件**：`onItemTreeChanged`、`onDestroy`、`onSetStackCount`、`onDurabilityChanged`、`onInspectionStateChanged`、`onUse`、`onUseStatic`、`onChildChanged`、`onParentChanged`、`onPluggedIntoSlot`、`onUnpluggedFromSlot`、`onSlotContentChanged`、`onSlotTreeChanged`。

### `Slot`
- 表示装备/插件位，包含 `Key`、`DisplayName`/`Raw`、`Icon`、`Locked`、`Hidden`、`AutoEquip`、`CanDragOut`、`AllowedTypes` (`ItemFilter`) 等属性。
- `Content` 指向当前物品，支持 `SetContent`、`ClearContent`、`CanInsert(Item candidate)`、`Initialize(SlotCollection owner)`。
- 事件：`onSlotContentChanged`、`onSlotTreeChanged`。

### `SlotCollection`
- `ItemComponent` 实现，维护 `Slot` 列表并实现 `ICollection<Slot>`。
- 通过索引器或 `GetSlot(string key)` / `GetSlot(int hash)` 快速查找。
- `OnInitialize` 会为所有插槽绑定宿主。

### `Inventory`
- `MonoBehaviour` 容器，实现 `ISelfValidator` 与 `IEnumerable<Item>`。
- **属性**：`DisplayNameKey`、`DisplayName`、`Capacity`、`Content`、`AttachedToItem`、`NeedInspection`、`AcceptSticky`、`Loading`、`CachedWeight`、`lockedIndexes`。
- **事件**：`onContentChanged`、`onInventorySorted`、`onCapacityChanged`、`onSetIndexLock`。
- **操作**：`AddItem`、`AddAt`、`RemoveItem`、`RemoveAt`、`GetItemAt`、`GetIndex`、`GetFirstEmptyPosition`、`GetLastItemPosition`、`Find(int typeID)`、`Sort()`（按标签/类型/堆叠自动整理）、`LockIndex`/`UnlockIndex`/`ToggleLockIndex`、`IsEmpty()`、`RecalculateWeight()`。

### `ItemGraphicInfo`
- 存放展示资源：`DisplayPrefab`、`PreviewCameraOffset`、`PreviewRotation`、`PreviewScale` 等。
- 提供 `GetRenderTexture`、`BakePreview()` 等生成缩略图的工具。

### 其他基础组件
- `ItemComponent`：所有子组件基类，持有 `Master` (`Item`)、`Display`、`DisplayName`，在 `Validate` 中确保与宿主 Item 同对象。
- `ItemAgent`：代理实体，`Initialize(Item, AgentTypes)` 绑定宿主并监听 `onUnpluggedFromSlot`；代理类型包括 `normal`、`pickUp`、`handheld`、`equipment`。
- `ItemAgentUtilities`：序列化的代理库，依据 key/hash 创建代理（`CreateAgent`）、`ReleaseActiveAgent`，并通过 `onCreateAgent` 事件暴露创建回调。
- `ItemTreeExtensions`：扩展方法集合（如 `IsInCharacterSlot`、`GetRootParent`、`GetAllChildren`）。

## 2. 统计与修饰

### `StatCollection`
- 附加于 Item 的属性集，实现 `IEnumerable<Stat>`。
- 方法：`Initialize(Item master)`、`Add(Stat stat)`、`Remove(string key)`、`GetStat(string key)`、`Contains(string key)`、`RecalculateAll()`。
- 事件：`onSetDirty`（内部 `Stat` 触发时级联）。

### `Stat`
- 单个数值条目，公开 `Key`、`DisplayNameKey`、`DisplayName`、`BaseValue`、`Value`、`Display`、`Modifiers`。
- 方法：`AddModifier(Modifier)`、`RemoveModifier(Modifier)`、`ClearModifiers()`、`Recalculate()`。
- `OnSetDirty` 事件在属性需要刷新时触发。

### `ModifierDescriptionCollection`
- `ItemComponent`，序列化 `ModifierDescription` 列表并在 `OnInitialize` 中应用。
- 提供 `ReapplyModifiers()`、`Add(ModifierDescription)`、`Remove(ModifierDescription)`、`Clear()`、`GetDescription(string key)`。

### `ModifierDescription`
- 定义单个修饰：`Key`、`Type` (`ModifierType`)、`Value`、`Order`、`ModifierTarget`、`Display`、`Polarity`、`enableInInventory`、`overrideOrder`。
- 方法：`CreateModifier(object source)`、`ReapplyModifier(ModifierDescriptionCollection)`、`RemoveModifier()`、`GetTargetItem()`、`GetDescription()`。

### 辅助枚举与结构
- `ModifierTarget`（`Self`、`Parent`、`Children`、`Character`）。
- `ModifierType`（定义在 `ItemStatsSystem.Stats` 命名空间）。
- `Polarity`（`Negative`、`Neutral`、`Positive`）。
- `ModifierDescriptionCollection.ModifierMeta`：在编辑器中描述修饰顺序与显示开关。

## 3. 效果系统

### `Effect`
- 聚合效果逻辑的核心组件，持有 `Item`、`Display`、`Description`。
- 公开只读集合 `Triggers`、`Filters`、`Actions`。
- 事件：`onSetTargetItem`、`onItemTreeChanged`。
- 方法：`AddEffectComponent(EffectComponent)`、`RemoveEffectComponent`、`SetItem(Item target)`、`Trigger(EffectTriggerEventContext context)`。

### `EffectComponent`
- `Effect` 子元素基类，提供 `Master`、`DisplayName`、`LabelColor`、`Validate(SelfValidationResult)`。
- `Effect` 在运行时会为每个子组件调用 `OnAttachToMaster`/`OnDetachFromMaster`（在派生类实现）。

### 触发器
- `EffectTrigger`：通过 `Trigger(bool positive)`/`TriggerPositive()`/`TriggerNegative()` 将事件发送给 `Effect`，在 `OnDisable` 时默认触发负向。
- `UpdateTrigger`：按帧或固定间隔触发（`useFixedUpdate`、`interval`）。
- `TickTrigger`：基于 `tickRate` 定时，支持 `useUnscaledTime`。
- `ItemUsedTrigger`：监听 `Item.onUse`。
- `TriggerOnSetItem`：当 `Effect` 的目标 Item 被设置时触发。

### 过滤器
- `EffectFilter`：实现 `Evaluate(EffectTriggerEventContext)`，并可忽略负向触发 (`IgnoreNegativeTrigger`)。
- `BoolFilter`：内部布尔值控制效果是否通过。
- `ItemInCharacterSlotFilter`：判定 `Effect.Item` 是否处于角色槽位。

### 行为
- `EffectAction`：接收触发通知并调用 `OnTriggered(bool positive)`、`OnTriggeredPositive()`、`OnTriggeredNegative()`。
- `LogItemNameAction`、`LogWhenUsed`：调试用实现，输出触发信息。

### 使用行为
- `UsageBehavior`：抽象类，定义 `CanBeUsed(Item item, object user)` 与 `Use(Item item, object user)` 流程，`DisplaySettings` 可在 UI 中展示操作说明。
- `UsageUtilities`：`ItemComponent`，维护 `behaviors` 列表、`UseTime`、耐久消耗、音效键，并暴露 `IsUsable(Item item, object user)`、`Use(Item item, object user)`；触发静态事件 `OnItemUsedStaticEvent`。

### 触发上下文
- `EffectTriggerEventContext`：结构体，包含触发器引用 `source` 与正负向标记 `positive`。

## 4. 过滤与查询

### `ItemFilter`
- 支持嵌套、按类型 ID、标签、品质、极性、布尔开关等条件过滤。
- 方法：`Evaluate(Item item)`、`Match(ItemMetaData meta)`、`CheckItemType(int typeId)`、`AddChild(ItemFilter child)`、`RemoveChild`。
- 属性：`includeChildren`、`typeIDs`、`displayQualityRequirement`、`requiredTags`、`polarityRequirement` 等。

### `BoolFilter`
- 简单布尔开关，可单独使用或作为 `ItemFilter`、`EffectFilter` 的一部分。

### `ItemInCharacterSlotFilter`
- 检测指定 Item 是否装备在角色身上（或其父级是角色装备），常与效果系统结合。

## 5. 序列化与数据交换

### `ItemTreeData`
- 用于序列化物品树。
- 静态成员：`OnItemLoaded` 事件、`FromItem(Item item)`、`InstantiateAsync(ItemTreeData data)`、`InstantiateSync(ItemTreeData data)`。
- 实例成员：`entries`、`rootInstanceID`、`RootData`、`RootTypeID`、`GetEntry(int instanceID)`、`ToString()`（打印树形结构）。
- 内部类型：
  - `DataEntry`（`instanceID`、`typeID`、`variables`、`slotContents`、`inventory`、`inventorySortLocks`、`TypeName`、`StackCount`）。
  - `InventoryDataEntry`（索引 + 子实例 ID）。
  - `SlotInstanceIDPair`（槽键 + 子实例 ID）。

### `InventoryData`
- 线性背包存档：`capacity`、`entries`、`lockedIndexes`。
- 静态方法：`FromInventory(Inventory inventory)`、`LoadIntoInventory(InventoryData data, Inventory inventoryInstance)`（返回 `UniTask`）。
- 内部 `Entry` 存储 `inventoryPosition` 与子树数据。

## 6. 属性、标签与工具

- `DisplayQuality`：枚举 `None`、`White`、`Green`、`Blue`、`Purple`、`Orange`、`Red`、`Q7`、`Q8`。
- `ItemTypeIDAttribute` / `MenuPathAttribute`：Odin Inspector 支持，用于自定义菜单与类型标识。
- `ModifierDescriptionCollection.ModifierMeta`、`ItemAgentUtilities.AgentKeyPair`、`StringLists`（来自 Utilities 模块）为编辑器下拉与键值提供支持。
- `UnitySourceGeneratedAssemblyMonoScriptTypes_v1`：Unity 生成的脚本索引，无需在运行时代码中直接使用。

## 7. 使用示例

### 7.1 注册自定义使用行为
```csharp
public class HealUsage : UsageBehavior
{
    public float healAmount = 25f;

    public override bool CanBeUsed(Item item, object user)
    {
        return item.Durability > 0 && user is Health health && !health.IsDead;
    }

    protected override void OnUse(Item item, object user)
    {
        if (user is Health health)
        {
            health.Restore(healAmount);
        }
    }
}

// 安装
var usage = item.UsageUtilities;
usage.behaviors.Add(item.gameObject.AddComponent<HealUsage>());
```

### 7.2 保存与恢复背包
```csharp
InventoryData snapshot = InventoryData.FromInventory(player.Backpack);
await InventoryData.LoadIntoInventory(snapshot, player.Backpack);
```

### 7.3 按条件触发效果
```csharp
public class CriticalHitTrigger : EffectTrigger
{
    public void OnCriticalHit()
    {
        TriggerPositive();
    }
}

public class CriticalBonusAction : EffectAction
{
    protected override void OnTriggeredPositive()
    {
        Master.Item.Stats.GetStat("CritBonus")?.AddModifier(new Modifier(ModifierType.Add, 0.1f, source: this));
    }

    protected override void OnTriggeredNegative()
    {
        // 触发器关闭时清除
    }
}
```

借助上述归档，Mod 作者可以快速对照 ItemStatsSystem 的运行时代码，覆盖物品、插槽、背包、统计、修饰、效果、使用逻辑与序列化的全部 API。

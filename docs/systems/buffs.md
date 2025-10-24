# Buff系统

## 概述
Buff系统是《逃离鸭科夫》的重要系统之一，负责管理角色身上的增益/减益效果、持续时间、效果叠加等功能。

## 核心组件

### Buff
Buff类，表示角色身上的增益/减益效果。

#### 主要功能
- **效果管理** - 激活、停用Buff
- **持续时间** - 管理Buff持续时间
- **排他性** - 处理Buff之间的冲突
- **优先级** - 管理Buff优先级

#### 重要属性
- `ExclusiveTag` - 排他标签
- `ExclusiveTagPriority` - 排他标签优先级
- `Hide` - 是否隐藏
- `Character` - 所属角色
- `Duration` - 持续时间
- `IsActive` - 是否激活

#### 重要枚举
- `BuffExclusiveTags` - Buff排他标签枚举

#### 重要方法
- `Activate()` - 激活Buff
- `Deactivate()` - 停用Buff
- `ExtendDuration(float time)` - 延长持续时间
- `Remove()` - 移除Buff

### CharacterBuffManager
角色Buff管理器，管理角色身上的Buff效果。

#### 主要功能
- **Buff管理** - 添加、移除Buff
- **状态检查** - 检查Buff状态
- **清理管理** - 清除所有Buff

#### 重要属性
- `Character` - 所属角色
- `ActiveBuffs` - 活动Buff列表

#### 重要方法
- `AddBuff(Buff buff)` - 添加Buff
- `RemoveBuff(Buff buff)` - 移除Buff
- `RemoveBuffByID(int buffID)` - 通过ID移除Buff
- `HasBuff(int buffID)` - 检查是否有指定Buff
- `GetBuff(int buffID)` - 获取指定Buff
- `ClearAllBuffs()` - 清除所有Buff

## 使用示例

### 添加Buff
```csharp
CharacterMainControl character = CharacterMainControl.Main;
Buff healthBuff = GetHealthBuff();

// 添加Buff
character.GetBuffManager().AddBuff(healthBuff, character, 0);
Debug.Log("添加健康Buff");

// 检查Buff是否存在
if (character.GetBuffManager().HasBuff(healthBuffID))
{
    Debug.Log("角色有健康Buff");
}
```

### 移除Buff
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 移除指定Buff
character.GetBuffManager().RemoveBuff(healthBuffID);
Debug.Log("移除健康Buff");

// 通过ID移除Buff
character.GetBuffManager().RemoveBuffByID(healthBuffID);

// 清除所有Buff
character.GetBuffManager().ClearAllBuffs();
Debug.Log("清除所有Buff");
```

### 获取Buff信息
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 获取指定Buff
Buff buff = character.GetBuffManager().GetBuff(healthBuffID);
if (buff != null)
{
    Debug.Log($"Buff名称: {buff.name}");
    Debug.Log($"是否激活: {buff.IsActive}");
    Debug.Log($"持续时间: {buff.Duration}");
    Debug.Log($"排他标签: {buff.ExclusiveTag}");
}

// 获取所有活动Buff
List<Buff> activeBuffs = character.GetBuffManager().ActiveBuffs;
Debug.Log($"活动Buff数量: {activeBuffs.Count}");
```

### Buff系统集成
```csharp
public class BuffSystem
{
    private CharacterMainControl character;
    private CharacterBuffManager buffManager;
    
    public BuffSystem(CharacterMainControl character)
    {
        this.character = character;
        this.buffManager = character.GetBuffManager();
    }
    
    public void ApplyHealthBuff(float duration)
    {
        Buff healthBuff = CreateHealthBuff(duration);
        buffManager.AddBuff(healthBuff, character, 0);
        Debug.Log($"应用健康Buff，持续时间: {duration}");
    }
    
    public void ApplySpeedBuff(float duration, float speedMultiplier)
    {
        Buff speedBuff = CreateSpeedBuff(duration, speedMultiplier);
        buffManager.AddBuff(speedBuff, character, 0);
        Debug.Log($"应用速度Buff，持续时间: {duration}，倍数: {speedMultiplier}");
    }
    
    public void ApplyDamageBuff(float duration, float damageMultiplier)
    {
        Buff damageBuff = CreateDamageBuff(duration, damageMultiplier);
        buffManager.AddBuff(damageBuff, character, 0);
        Debug.Log($"应用伤害Buff，持续时间: {duration}，倍数: {damageMultiplier}");
    }
    
    public void RemoveBuffByTag(Buff.BuffExclusiveTags tag)
    {
        buffManager.RemoveBuffsByTag(tag, false);
        Debug.Log($"移除标签为 {tag} 的Buff");
    }
    
    private Buff CreateHealthBuff(float duration)
    {
        // 创建健康Buff
        Buff buff = new Buff();
        buff.Duration = duration;
        buff.ExclusiveTag = Buff.BuffExclusiveTags.Health;
        return buff;
    }
    
    private Buff CreateSpeedBuff(float duration, float multiplier)
    {
        // 创建速度Buff
        Buff buff = new Buff();
        buff.Duration = duration;
        buff.ExclusiveTag = Buff.BuffExclusiveTags.Speed;
        return buff;
    }
    
    private Buff CreateDamageBuff(float duration, float multiplier)
    {
        // 创建伤害Buff
        Buff buff = new Buff();
        buff.Duration = duration;
        buff.ExclusiveTag = Buff.BuffExclusiveTags.Damage;
        return buff;
    }
}
```

### Buff持续时间管理
```csharp
public class BuffDurationManager
{
    private CharacterMainControl character;
    private CharacterBuffManager buffManager;
    private Dictionary<int, float> buffTimers;
    
    public BuffDurationManager(CharacterMainControl character)
    {
        this.character = character;
        this.buffManager = character.GetBuffManager();
        this.buffTimers = new Dictionary<int, float>();
    }
    
    public void UpdateBuffDurations()
    {
        List<Buff> activeBuffs = buffManager.ActiveBuffs;
        
        foreach (Buff buff in activeBuffs)
        {
            if (buff.IsActive)
            {
                // 更新Buff计时器
                if (buffTimers.ContainsKey(buff.GetInstanceID()))
                {
                    buffTimers[buff.GetInstanceID()] -= Time.deltaTime;
                    
                    if (buffTimers[buff.GetInstanceID()] <= 0)
                    {
                        // Buff时间到期，移除Buff
                        buffManager.RemoveBuff(buff);
                        buffTimers.Remove(buff.GetInstanceID());
                        Debug.Log($"Buff {buff.name} 时间到期");
                    }
                }
                else
                {
                    // 初始化Buff计时器
                    buffTimers[buff.GetInstanceID()] = buff.Duration;
                }
            }
        }
    }
    
    public void ExtendBuffDuration(int buffID, float additionalTime)
    {
        Buff buff = buffManager.GetBuff(buffID);
        if (buff != null)
        {
            buff.ExtendDuration(additionalTime);
            if (buffTimers.ContainsKey(buff.GetInstanceID()))
            {
                buffTimers[buff.GetInstanceID()] += additionalTime;
            }
            Debug.Log($"延长Buff {buff.name} 持续时间 {additionalTime} 秒");
        }
    }
    
    public float GetBuffRemainingTime(int buffID)
    {
        Buff buff = buffManager.GetBuff(buffID);
        if (buff != null && buffTimers.ContainsKey(buff.GetInstanceID()))
        {
            return buffTimers[buff.GetInstanceID()];
        }
        return 0f;
    }
}
```

### Buff效果系统
```csharp
public class BuffEffectSystem
{
    private CharacterMainControl character;
    private CharacterBuffManager buffManager;
    
    public BuffEffectSystem(CharacterMainControl character)
    {
        this.character = character;
        this.buffManager = character.GetBuffManager();
    }
    
    public void ApplyBuffEffects()
    {
        List<Buff> activeBuffs = buffManager.ActiveBuffs;
        
        foreach (Buff buff in activeBuffs)
        {
            if (buff.IsActive)
            {
                ApplyBuffEffect(buff);
            }
        }
    }
    
    private void ApplyBuffEffect(Buff buff)
    {
        switch (buff.ExclusiveTag)
        {
            case Buff.BuffExclusiveTags.Health:
                ApplyHealthEffect(buff);
                break;
            case Buff.BuffExclusiveTags.Speed:
                ApplySpeedEffect(buff);
                break;
            case Buff.BuffExclusiveTags.Damage:
                ApplyDamageEffect(buff);
                break;
            case Buff.BuffExclusiveTags.Armor:
                ApplyArmorEffect(buff);
                break;
        }
    }
    
    private void ApplyHealthEffect(Buff buff)
    {
        // 应用健康效果
        character.Health.AddHealth(10f * Time.deltaTime);
    }
    
    private void ApplySpeedEffect(Buff buff)
    {
        // 应用速度效果
        float speedMultiplier = GetBuffMultiplier(buff);
        // 这里需要根据实际的速度系统实现
    }
    
    private void ApplyDamageEffect(Buff buff)
    {
        // 应用伤害效果
        float damageMultiplier = GetBuffMultiplier(buff);
        // 这里需要根据实际的伤害系统实现
    }
    
    private void ApplyArmorEffect(Buff buff)
    {
        // 应用护甲效果
        float armorBonus = GetBuffValue(buff);
        // 这里需要根据实际的护甲系统实现
    }
    
    private float GetBuffMultiplier(Buff buff)
    {
        // 获取Buff倍数
        return 1.5f; // 示例
    }
    
    private float GetBuffValue(Buff buff)
    {
        // 获取Buff数值
        return 10f; // 示例
    }
}
```

### Buff冲突处理
```csharp
public class BuffConflictResolver
{
    private CharacterMainControl character;
    private CharacterBuffManager buffManager;
    
    public BuffConflictResolver(CharacterMainControl character)
    {
        this.character = character;
        this.buffManager = character.GetBuffManager();
    }
    
    public void ResolveBuffConflicts(Buff newBuff)
    {
        List<Buff> activeBuffs = buffManager.ActiveBuffs;
        
        foreach (Buff existingBuff in activeBuffs)
        {
            if (existingBuff.ExclusiveTag == newBuff.ExclusiveTag)
            {
                // 处理排他性冲突
                if (newBuff.ExclusiveTagPriority > existingBuff.ExclusiveTagPriority)
                {
                    // 新Buff优先级更高，移除旧Buff
                    buffManager.RemoveBuff(existingBuff);
                    Debug.Log($"移除低优先级Buff: {existingBuff.name}");
                }
                else if (newBuff.ExclusiveTagPriority < existingBuff.ExclusiveTagPriority)
                {
                    // 旧Buff优先级更高，拒绝新Buff
                    Debug.Log($"拒绝低优先级Buff: {newBuff.name}");
                    return;
                }
                else
                {
                    // 优先级相同，延长现有Buff时间
                    existingBuff.ExtendDuration(newBuff.Duration);
                    Debug.Log($"延长现有Buff时间: {existingBuff.name}");
                    return;
                }
            }
        }
        
        // 没有冲突，添加新Buff
        buffManager.AddBuff(newBuff, character, 0);
        Debug.Log($"添加新Buff: {newBuff.name}");
    }
}
```

### Buff UI系统
```csharp
public class BuffUISystem
{
    private CharacterMainControl character;
    private CharacterBuffManager buffManager;
    private Dictionary<int, GameObject> buffUIElements;
    
    public BuffUISystem(CharacterMainControl character)
    {
        this.character = character;
        this.buffManager = character.GetBuffManager();
        this.buffUIElements = new Dictionary<int, GameObject>();
    }
    
    public void UpdateBuffUI()
    {
        List<Buff> activeBuffs = buffManager.ActiveBuffs;
        
        // 更新现有UI元素
        foreach (var kvp in buffUIElements.ToList())
        {
            Buff buff = buffManager.GetBuff(kvp.Key);
            if (buff == null || !buff.IsActive)
            {
                // Buff已移除，销毁UI元素
                Destroy(kvp.Value);
                buffUIElements.Remove(kvp.Key);
            }
            else
            {
                // 更新UI元素
                UpdateBuffUIElement(kvp.Value, buff);
            }
        }
        
        // 添加新Buff的UI元素
        foreach (Buff buff in activeBuffs)
        {
            if (!buffUIElements.ContainsKey(buff.GetInstanceID()))
            {
                GameObject buffUI = CreateBuffUIElement(buff);
                buffUIElements[buff.GetInstanceID()] = buffUI;
            }
        }
    }
    
    private GameObject CreateBuffUIElement(Buff buff)
    {
        // 创建Buff UI元素
        GameObject buffUI = new GameObject($"BuffUI_{buff.name}");
        // 这里需要根据实际UI系统实现
        return buffUI;
    }
    
    private void UpdateBuffUIElement(GameObject buffUI, Buff buff)
    {
        // 更新Buff UI元素
        // 这里需要根据实际UI系统实现
    }
}
```

## 注意事项

1. Buff系统支持排他性和优先级管理
2. Buff持续时间需要实时更新和管理
3. Buff效果需要根据实际游戏系统实现
4. Buff冲突处理确保游戏平衡
5. Buff UI系统需要实时更新显示
6. Buff数据会保存在存档中
7. Buff系统与角色系统、战斗系统紧密相关
8. Buff效果叠加需要合理设计
9. Buff持续时间到期需要自动移除
10. Buff系统的事件监听要正确管理，避免内存泄漏

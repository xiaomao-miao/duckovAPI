# 战斗系统

## 概述
战斗系统是《逃离鸭科夫》的核心系统之一，负责管理武器、伤害、生命值、护甲等战斗相关功能。

## 核心组件

### Health
生命值系统，管理角色的生命值和伤害计算。

#### 主要功能
- **生命值管理** - 当前生命值、最大生命值
- **伤害计算** - 护甲、暴击、元素抗性
- **状态效果** - 无敌、隐藏
- **死亡处理** - 死亡检测、死亡事件

#### 重要属性
- `MaxHealth` - 最大生命值
- `CurrentHealth` - 当前生命值
- `IsDead` - 是否死亡
- `Invincible` - 是否无敌
- `BodyArmor` - 身体护甲
- `HeadArmor` - 头部护甲

#### 重要事件
- `OnHurt` - 受伤事件
- `OnDead` - 死亡事件
- `OnHealthChange` - 生命值改变事件
- `OnMaxHealthChange` - 最大生命值改变事件

### DamageInfo
伤害信息结构体，包含伤害的详细信息。

#### 重要属性
- `damageValue` - 伤害值
- `damageType` - 伤害类型
- `fromCharacter` - 伤害来源角色
- `hitPoint` - 命中点
- `isCritical` - 是否暴击
- `elementType` - 元素类型

### ElementTypes
元素类型枚举，定义不同的伤害元素。

#### 枚举值
- `physics` - 物理伤害
- `fire` - 火焰伤害
- `poison` - 毒素伤害
- `electricity` - 电击伤害
- `space` - 空间伤害

### DamageTypes
伤害类型枚举，定义不同的伤害类型。

#### 枚举值
- `realDamage` - 真实伤害
- 其他伤害类型

## 使用示例

### 生命值管理
```csharp
Health health = GetComponent<Health>();

// 检查生命值状态
if (health.IsDead)
{
    Debug.Log("角色已死亡");
    return;
}

// 获取生命值信息
float currentHealth = health.CurrentHealth;
float maxHealth = health.MaxHealth;
float healthPercentage = currentHealth / maxHealth;

Debug.Log($"生命值: {currentHealth}/{maxHealth} ({healthPercentage * 100}%)");
```

### 添加和设置生命值
```csharp
Health health = GetComponent<Health>();

// 添加生命值
health.AddHealth(50f);
Debug.Log($"添加50点生命值，当前生命值: {health.CurrentHealth}");

// 设置生命值
health.SetHealth(100f);
Debug.Log($"设置生命值为100，当前生命值: {health.CurrentHealth}");

// 设置无敌状态
health.SetInvincible(true);
Debug.Log("角色进入无敌状态");
```

### 造成伤害
```csharp
Health targetHealth = GetComponent<Health>();

// 创建伤害信息
DamageInfo damageInfo = new DamageInfo();
damageInfo.damageValue = 100f;
damageInfo.damageType = DamageTypes.physics;
damageInfo.fromCharacter = attacker;
damageInfo.hitPoint = hitPoint;
damageInfo.isCritical = false;
damageInfo.elementType = ElementTypes.physics;

// 造成伤害
bool wasHurt = targetHealth.Hurt(damageInfo);
if (wasHurt)
{
    Debug.Log($"造成 {damageInfo.damageValue} 点伤害");
}
```

### 监听生命值事件
```csharp
void OnEnable()
{
    Health.OnHurt += OnCharacterHurt;
    Health.OnDead += OnCharacterDead;
    Health.OnHealthChange += OnHealthChanged;
    Health.OnMaxHealthChange += OnMaxHealthChanged;
}

void OnDisable()
{
    Health.OnHurt -= OnCharacterHurt;
    Health.OnDead -= OnCharacterDead;
    Health.OnHealthChange -= OnHealthChanged;
    Health.OnMaxHealthChanged -= OnMaxHealthChanged;
}

private void OnCharacterHurt(DamageInfo damageInfo)
{
    Debug.Log($"角色受到伤害: {damageInfo.damageValue}");
    Debug.Log($"伤害类型: {damageInfo.damageType}");
    Debug.Log($"伤害来源: {damageInfo.fromCharacter}");
    
    if (damageInfo.isCritical)
    {
        Debug.Log("暴击！");
    }
}

private void OnCharacterDead()
{
    Debug.Log("角色死亡");
    // 处理死亡逻辑
}

private void OnHealthChanged(float newHealth)
{
    Debug.Log($"生命值改变: {newHealth}");
}

private void OnMaxHealthChanged(float newMaxHealth)
{
    Debug.Log($"最大生命值改变: {newMaxHealth}");
}
```

### 护甲系统
```csharp
Health health = GetComponent<Health>();

// 获取护甲信息
float bodyArmor = health.BodyArmor;
float headArmor = health.HeadArmor;

Debug.Log($"身体护甲: {bodyArmor}");
Debug.Log($"头部护甲: {headArmor}");

// 检查护甲是否有效
if (bodyArmor > 0)
{
    Debug.Log("角色有身体护甲保护");
}

if (headArmor > 0)
{
    Debug.Log("角色有头部护甲保护");
}
```

### 元素抗性
```csharp
Health health = GetComponent<Health>();

// 获取不同元素的抗性
float physicsResistance = health.ElementFactor(ElementTypes.physics);
float fireResistance = health.ElementFactor(ElementTypes.fire);
float poisonResistance = health.ElementFactor(ElementTypes.poison);
float electricityResistance = health.ElementFactor(ElementTypes.electricity);
float spaceResistance = health.ElementFactor(ElementTypes.space);

Debug.Log($"物理抗性: {physicsResistance}");
Debug.Log($"火焰抗性: {fireResistance}");
Debug.Log($"毒素抗性: {poisonResistance}");
Debug.Log($"电击抗性: {electricityResistance}");
Debug.Log($"空间抗性: {spaceResistance}");
```

### 战斗系统集成
```csharp
public class CombatSystem
{
    private Health playerHealth;
    private CharacterMainControl player;
    
    public CombatSystem(Health health, CharacterMainControl character)
    {
        playerHealth = health;
        player = character;
    }
    
    public void AttackTarget(Health targetHealth, float damage, ElementTypes elementType)
    {
        if (targetHealth == null || targetHealth.IsDead)
        {
            Debug.Log("目标无效或已死亡");
            return;
        }
        
        // 创建伤害信息
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damageValue = damage;
        damageInfo.damageType = DamageTypes.physics;
        damageInfo.fromCharacter = player;
        damageInfo.hitPoint = targetHealth.transform.position;
        damageInfo.isCritical = CalculateCriticalHit();
        damageInfo.elementType = elementType;
        
        // 造成伤害
        bool wasHurt = targetHealth.Hurt(damageInfo);
        
        if (wasHurt)
        {
            Debug.Log($"成功造成 {damage} 点 {elementType} 伤害");
            
            // 检查目标是否死亡
            if (targetHealth.IsDead)
            {
                Debug.Log("目标死亡");
                OnTargetKilled(targetHealth);
            }
        }
    }
    
    private bool CalculateCriticalHit()
    {
        // 计算暴击概率
        float critChance = GetCriticalChance();
        return UnityEngine.Random.Range(0f, 1f) < critChance;
    }
    
    private float GetCriticalChance()
    {
        // 根据角色属性计算暴击概率
        return 0.1f; // 10% 暴击率
    }
    
    private void OnTargetKilled(Health targetHealth)
    {
        Debug.Log($"目标 {targetHealth.name} 被击杀");
        // 处理击杀逻辑
    }
}
```

### 伤害计算系统
```csharp
public class DamageCalculator
{
    public static float CalculateDamage(DamageInfo damageInfo, Health targetHealth)
    {
        float baseDamage = damageInfo.damageValue;
        
        // 应用护甲减伤
        float armorReduction = CalculateArmorReduction(damageInfo, targetHealth);
        baseDamage *= (1f - armorReduction);
        
        // 应用元素抗性
        float elementResistance = targetHealth.ElementFactor(damageInfo.elementType);
        baseDamage *= (1f - elementResistance);
        
        // 应用暴击倍率
        if (damageInfo.isCritical)
        {
            baseDamage *= GetCriticalMultiplier();
        }
        
        return Mathf.Max(0f, baseDamage);
    }
    
    private static float CalculateArmorReduction(DamageInfo damageInfo, Health targetHealth)
    {
        float armor = 0f;
        
        // 根据命中部位选择护甲
        if (IsHeadshot(damageInfo.hitPoint, targetHealth))
        {
            armor = targetHealth.HeadArmor;
        }
        else
        {
            armor = targetHealth.BodyArmor;
        }
        
        // 护甲减伤公式
        return armor / (armor + 100f);
    }
    
    private static bool IsHeadshot(Vector3 hitPoint, Health targetHealth)
    {
        // 简单的头部命中检测
        float headHeight = 1.8f; // 假设角色高度
        return hitPoint.y > targetHealth.transform.position.y + headHeight * 0.8f;
    }
    
    private static float GetCriticalMultiplier()
    {
        return 2.0f; // 暴击造成2倍伤害
    }
}
```

### 战斗状态管理
```csharp
public class CombatStateManager
{
    private bool inCombat;
    private float combatStartTime;
    private List<Health> enemiesInCombat;
    
    public bool InCombat => inCombat;
    public float CombatDuration => Time.time - combatStartTime;
    
    public void StartCombat(Health enemy)
    {
        if (!inCombat)
        {
            inCombat = true;
            combatStartTime = Time.time;
            enemiesInCombat = new List<Health>();
            Debug.Log("进入战斗状态");
        }
        
        if (!enemiesInCombat.Contains(enemy))
        {
            enemiesInCombat.Add(enemy);
        }
    }
    
    public void EndCombat()
    {
        if (inCombat)
        {
            inCombat = false;
            enemiesInCombat.Clear();
            Debug.Log($"战斗结束，持续了 {CombatDuration} 秒");
        }
    }
    
    public void RemoveEnemy(Health enemy)
    {
        if (enemiesInCombat.Contains(enemy))
        {
            enemiesInCombat.Remove(enemy);
            
            if (enemiesInCombat.Count == 0)
            {
                EndCombat();
            }
        }
    }
}
```

## 注意事项

1. 生命值系统的伤害计算包含护甲、暴击、元素抗性等复杂因素
2. 伤害信息必须正确设置，包括伤害值、类型、来源等
3. 事件监听后记得在OnDisable中取消订阅，避免内存泄漏
4. 护甲减伤和元素抗性会显著影响最终伤害
5. 暴击系统需要根据角色属性计算概率
6. 死亡检测要及时，避免对已死亡目标造成伤害
7. 战斗状态管理有助于优化性能和游戏体验
8. 伤害计算应该考虑命中部位（头部、身体）
9. 元素伤害和物理伤害有不同的计算方式
10. 战斗系统与角色系统、物品系统紧密相关

# 角色系统

## 概述
角色系统是《逃离鸭科夫》的核心系统之一，负责管理角色的移动、战斗、技能、装备等功能。

## 核心组件

### CharacterMainControl
角色主控制器，管理角色的所有核心功能。

#### 主要功能
- **移动控制** - 角色移动、奔跑、冲刺
- **战斗系统** - 攻击、瞄准、射击
- **物品使用** - 手持物品、装备管理
- **技能系统** - 角色技能、物品技能
- **状态管理** - 生命值、体力、能量

#### 重要属性
- `Main` - 获取主角色实例
- `Health` - 生命值系统
- `Team` - 角色队伍
- `CurrentHoldItemAgent` - 当前手持物品代理
- `Running` - 是否在奔跑
- `IsOnGround` - 是否在地面上
- `Velocity` - 速度
- `ThermalOn` - 热成像是否开启
- `IsInAdsInput` - 是否在瞄准输入
- `CurrentAction` - 当前动作

#### 重要事件
- `OnMainCharacterStartUseItem` - 主角色开始使用物品
- `OnMainCharacterInventoryChangedEvent` - 主角色背包改变
- `OnMainCharacterSlotContentChangedEvent` - 主角色插槽内容改变
- `OnMainCharacterChangeHoldItemAgentEvent` - 主角色改变手持物品代理
- `OnShootEvent` - 射击事件
- `OnAttackEvent` - 攻击事件
- `OnSkillStartReleaseEvent` - 技能开始释放事件

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

## 使用示例

### 获取主角色
```csharp
CharacterMainControl mainCharacter = CharacterMainControl.Main;
if (mainCharacter != null)
{
    // 使用主角色
    Debug.Log($"主角色生命值: {mainCharacter.Health.CurrentHealth}");
}
```

### 监听角色事件
```csharp
void OnEnable()
{
    CharacterMainControl.OnMainCharacterStartUseItem += OnMainCharacterStartUseItem;
    CharacterMainControl.OnMainCharacterInventoryChangedEvent += OnMainCharacterInventoryChanged;
}

void OnDisable()
{
    CharacterMainControl.OnMainCharacterStartUseItem -= OnMainCharacterStartUseItem;
    CharacterMainControl.OnMainCharacterInventoryChangedEvent -= OnMainCharacterInventoryChanged;
}

private void OnMainCharacterStartUseItem(Item item)
{
    Debug.Log($"主角色开始使用物品: {item.DisplayName}");
}

private void OnMainCharacterInventoryChanged()
{
    Debug.Log("主角色背包发生变化");
}
```

### 角色移动控制
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 检查角色状态
if (character.CanMove())
{
    // 角色可以移动
    if (character.Running)
    {
        Debug.Log("角色正在奔跑");
    }
    
    if (character.IsOnGround)
    {
        Debug.Log("角色在地面上");
    }
}

// 设置位置
character.SetPosition(new Vector3(0, 0, 0));

// 设置强制移动速度
character.SetForceMoveVelocity(new Vector3(1, 0, 0));
```

### 角色战斗系统
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 检查瞄准状态
if (character.IsInAdsInput)
{
    Debug.Log("角色正在瞄准");
}

// 获取瞄准点
Vector3 aimPoint = character.GetCurrentAimPoint();

// 攻击
character.Attack();

// 切换武器
character.SwitchWeapon(1);

// 尝试重新装弹
Item bullet = GetBullet();
character.TryToReload(bullet);
```

### 角色技能系统
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 设置技能
SkillBase skill = GetSkill();
character.SetSkill(SkillTypes.CharacterSkill, skill, gameObject);

// 开始技能瞄准
character.StartSkillAim(SkillTypes.CharacterSkill);

// 释放技能
character.ReleaseSkill(SkillTypes.CharacterSkill);

// 取消技能
character.CancleSkill();

// 获取当前运行的技能
SkillBase currentSkill = character.GetCurrentRunningSkill();
```

### 生命值管理
```csharp
Health health = GetComponent<Health>();

// 检查生命值状态
if (health.IsDead)
{
    Debug.Log("角色已死亡");
    return;
}

// 添加生命值
health.AddHealth(50f);

// 设置生命值
health.SetHealth(100f);

// 设置无敌
health.SetInvincible(true);

// 监听生命值事件
health.OnHurt += OnCharacterHurt;
health.OnDead += OnCharacterDead;
health.OnHealthChange += OnHealthChanged;

private void OnCharacterHurt(DamageInfo damageInfo)
{
    Debug.Log($"角色受到伤害: {damageInfo.damageValue}");
}

private void OnCharacterDead()
{
    Debug.Log("角色死亡");
}

private void OnHealthChanged(float newHealth)
{
    Debug.Log($"生命值改变: {newHealth}");
}
```

### 角色装备系统
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 获取装备
Item armor = character.GetArmorItem();
Item helmet = character.GetHelmatItem();
Item faceMask = character.GetFaceMaskItem();

// 改变手持物品
Item newItem = GetItem();
character.ChangeHoldItem(newItem);

// 在插槽中切换手持代理
character.SwitchHoldAgentInSlot(slotHash);
```

### 角色状态检查
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 检查各种状态
if (character.CanMove())
{
    Debug.Log("角色可以移动");
}

if (character.CanRun())
{
    Debug.Log("角色可以奔跑");
}

if (character.CanUseHand())
{
    Debug.Log("角色可以使用手");
}

if (character.CanControlAim())
{
    Debug.Log("角色可以控制瞄准");
}

// 检查是否在冲刺
if (character.Dashing)
{
    Debug.Log("角色正在冲刺");
}
```

### 角色特殊功能
```csharp
CharacterMainControl character = CharacterMainControl.Main;

// 切换夜视
character.ToggleNightVision();

// 冲刺
character.Dash();

// 尝试抓鱼
character.TryCatchFishInput();

// 切换视角
InputManager.Instance.ToggleView();

// 切换夜视
InputManager.Instance.ToggleNightVision();
```

## 注意事项

1. 角色相关操作需要在关卡初始化完成后进行
2. 事件订阅后记得在OnDisable中取消订阅，避免内存泄漏
3. 生命值系统的伤害计算包含护甲、暴击、元素抗性等复杂因素
4. 角色状态检查要全面，避免在错误状态下执行操作
5. 技能系统支持多种技能类型，需要正确设置
6. 装备系统会影响角色的各种属性
7. 角色移动需要考虑地形和障碍物
8. 战斗系统需要处理瞄准、射击、伤害等复杂逻辑
9. 角色死亡时会触发相应事件，需要正确处理
10. 所有角色操作都应该检查角色是否存在和有效

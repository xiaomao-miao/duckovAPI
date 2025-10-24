# TeamSoda.Duckov.Core - 核心模块

## 概述
TeamSoda.Duckov.Core 是《逃离鸭科夫》游戏的核心模块，包含了游戏的主要系统、角色控制、UI管理、关卡管理等核心功能。

## 核心类

### GameManager
游戏管理器，负责管理游戏的整体状态和各个子系统。

#### 静态属性
- `Instance` - 获取GameManager单例实例
- `Paused` - 游戏是否暂停
- `AudioManager` - 音频管理器
- `UiInputManager` - UI输入管理器
- `PauseMenu` - 暂停菜单
- `DifficultyManager` - 难度管理器
- `SceneLoader` - 场景加载器
- `BlackScreen` - 黑屏控制器
- `EventSystem` - 事件系统
- `NightVision` - 夜视系统
- `BloodFxOn` - 血液特效开关
- `MainPlayerInput` - 主玩家输入
- `ModManager` - Mod管理器
- `NoteIndex` - 笔记索引
- `AchievementManager` - 成就管理器

#### 方法
- `TimeTravelDetected()` - 检测到时间穿越

### CharacterMainControl
角色主控制器，管理角色的移动、战斗、物品使用等核心功能。

#### 公有静态属性
- `Main` - 获取主角色实例 (public static)

#### 公有属性
- `AudioVoiceType` - 音频声音类型 (public get/set)
- `FootStepMaterialType` - 脚步声材质类型 (public get/set)
- `Team` - 角色队伍 (public get)
- `CharacterItem` - 角色物品 (public get)
- `CurrentHoldItemAgent` - 当前手持物品代理 (public get)
- `Hidden` - 是否隐藏 (public get)
- `CurrentUsingAimSocket` - 当前使用的瞄准插槽 (public get)
- `RightHandSocket` - 右手插槽 (public get)
- `CurrentAimDirection` - 当前瞄准方向 (public get)
- `CurrentMoveDirection` - 当前移动方向 (public get)
- `AnimationMoveSpeedValue` - 动画移动速度值 (public get)
- `AnimationLocalMoveDirectionValue` - 动画本地移动方向值 (public get)
- `Running` - 是否在奔跑 (public get)
- `IsOnGround` - 是否在地面上 (public get)
- `Velocity` - 速度 (public get)
- `ThermalOn` - 热成像是否开启 (public get)
- `IsInAdsInput` - 是否在瞄准输入 (public get)
- `AdsValue` - 瞄准值 (public get)
- `AimType` - 瞄准类型 (public get)
- `CurrentAction` - 当前动作 (public get)
- `Health` - 生命值 (public get)
- `MoveInput` - 移动输入 (public get)
- `EquipmentController` - 装备控制器 (public get)
- `Dashing` - 是否在冲刺 (public get)

#### 公有事件
- `OnTeamChanged` - 队伍改变事件 (public event)
- `OnSetPositionEvent` - 设置位置事件 (public event)
- `BeforeCharacterSpawnLootOnDead` - 角色死亡前生成战利品事件 (public event)
- `OnActionStartEvent` - 动作开始事件 (public event)
- `OnActionProgressFinishEvent` - 动作进度完成事件 (public event)
- `OnHoldAgentChanged` - 手持代理改变事件 (public event)
- `OnShootEvent` - 射击事件 (public event)
- `TryCatchFishInputEvent` - 尝试抓鱼输入事件 (public event)
- `OnAttackEvent` - 攻击事件 (public event)
- `OnSkillStartReleaseEvent` - 技能开始释放事件 (public event)

#### 公有静态事件
- `OnMainCharacterStartUseItem` - 主角色开始使用物品事件 (public static event)
- `OnMainCharacterInventoryChangedEvent` - 主角色背包改变事件 (public static event)
- `OnMainCharacterSlotContentChangedEvent` - 主角色插槽内容改变事件 (public static event)
- `OnMainCharacterChangeHoldItemAgentEvent` - 主角色改变手持物品代理事件 (public static event)

### LevelManager
关卡管理器，负责管理游戏关卡的加载、初始化和状态。

#### 静态属性
- `Instance` - 获取LevelManager实例
- `LootBoxInventoriesParent` - 战利品箱背包父对象
- `LootBoxInventories` - 战利品箱背包字典
- `LevelInitializing` - 关卡是否正在初始化
- `AfterInit` - 是否在初始化后
- `LevelInited` - 关卡是否已初始化
- `LevelInitializingComment` - 关卡初始化注释
- `OnLevelInitializingCommentChanged` - 关卡初始化注释改变事件
- `OnLevelBeginInitializing` - 关卡开始初始化事件
- `OnLevelInitialized` - 关卡初始化完成事件
- `OnAfterLevelInitialized` - 关卡初始化后事件
- `OnEvacuated` - 撤离事件
- `OnMainCharacterDead` - 主角色死亡事件
- `OnNewGameReport` - 新游戏报告事件
- `Rule` - 规则集

### HUDManager
HUD管理器，负责管理游戏界面的显示和隐藏。

#### 静态方法
- `RegisterHideToken(UnityEngine.Object obj)` - 注册隐藏令牌
- `UnregisterHideToken(UnityEngine.Object obj)` - 注销隐藏令牌

### CraftingManager
制作管理器，负责管理游戏中的制作配方和制作系统。

#### 静态属性
- `Instance` - 获取CraftingManager实例
- `UnlockedFormulaIDs` - 已解锁的配方ID集合

#### 事件
- `OnItemCrafted` - 物品制作完成事件
- `OnFormulaUnlocked` - 配方解锁事件

#### 静态方法
- `UnlockFormula(string formulaID)` - 解锁配方
- `IsFormulaUnlocked(string value)` - 检查配方是否已解锁
- `GetFormula(string id)` - 获取配方

#### 方法
- `Craft(string id)` - 制作物品
- `Craft(CraftingFormula formula)` - 制作物品（配方）

### InputManager
输入管理器，负责处理玩家输入和输入设备管理。

#### 静态属性
- `InputDevice` - 当前输入设备
- `InputActived` - 输入是否激活

#### 属性
- `WorldMoveInput` - 世界移动输入
- `AimTarget` - 瞄准目标
- `MoveAxisInput` - 移动轴输入
- `AimScreenPoint` - 瞄准屏幕点
- `InputAimPoint` - 输入瞄准点
- `MousePos` - 鼠标位置
- `TriggerInput` - 触发输入
- `AimingEnemyHead` - 是否瞄准敌人头部

#### 事件
- `OnInputDeviceChanged` - 输入设备改变事件
- `OnSwitchBulletTypeInput` - 切换子弹类型输入事件
- `OnSwitchWeaponInput` - 切换武器输入事件
- `OnInteractButtonDown` - 交互按钮按下事件

### Health
生命值系统，管理角色的生命值和伤害计算。

#### 属性
- `showHealthBar` - 是否显示血条
- `Hidden` - 是否隐藏
- `MaxHealth` - 最大生命值
- `IsMainCharacterHealth` - 是否是主角色生命值
- `CurrentHealth` - 当前生命值
- `IsDead` - 是否死亡
- `Invincible` - 是否无敌
- `BodyArmor` - 身体护甲
- `HeadArmor` - 头部护甲

#### 事件
- `OnHurt` - 受伤事件
- `OnDead` - 死亡事件
- `OnRequestHealthBar` - 请求血条事件
- `OnHealthChange` - 生命值改变事件
- `OnMaxHealthChange` - 最大生命值改变事件

### ModBehaviour
Mod行为基类，所有Mod必须继承此类。

#### 属性
- `master` - Mod管理器
- `info` - Mod信息

#### 方法
- `Setup(ModManager master, ModInfo info)` - 设置Mod
- `NotifyBeforeDeactivate()` - 通知停用前
- `OnAfterSetup()` - 设置后回调（可重写）
- `OnBeforeDeactivate()` - 停用前回调（可重写）

## 使用示例

### 获取主角色
```csharp
CharacterMainControl mainCharacter = CharacterMainControl.Main;
if (mainCharacter != null)
{
    // 使用主角色
}
```

### 监听角色事件
```csharp
void OnEnable()
{
    CharacterMainControl.OnMainCharacterStartUseItem += OnMainCharacterStartUseItem;
}

void OnDisable()
{
    CharacterMainControl.OnMainCharacterStartUseItem -= OnMainCharacterStartUseItem;
}

private void OnMainCharacterStartUseItem(Item item)
{
    Debug.Log($"主角色开始使用物品: {item.DisplayName}");
}
```

### 制作系统
```csharp
// 解锁配方
CraftingManager.UnlockFormula("Weapon_AK47");

// 检查配方是否解锁
if (CraftingManager.IsFormulaUnlocked("Weapon_AK47"))
{
    // 制作物品
    List<Item> craftedItems = await CraftingManager.Instance.Craft("Weapon_AK47");
}
```

### 生命值系统
```csharp
Health health = GetComponent<Health>();

// 检查是否死亡
if (health.IsDead)
{
    // 处理死亡逻辑
}

// 添加生命值
health.AddHealth(50f);

// 设置无敌
health.SetInvincible(true);

// 监听生命值事件
Health.OnHurt += OnHurt;
Health.OnDead += OnDead;
```

## 注意事项

1. 所有静态属性在游戏未初始化时可能返回null，使用前请检查
2. 事件订阅后记得在OnDisable中取消订阅，避免内存泄漏
3. 角色相关操作需要在关卡初始化完成后进行
4. Mod开发时请继承ModBehaviour基类并实现必要的方法
5. 制作系统需要先解锁配方才能制作物品
6. 输入管理器支持多种输入设备，需要根据设备类型处理输入
7. 生命值系统的伤害计算包含护甲、暴击、元素抗性等复杂因素
8. Buff系统与生命值系统紧密相关，受伤时可能触发Buff
9. 制作配方的解锁状态会保存在存档中
10. 输入设备切换时会触发相应事件，需要更新UI显示

# TeamSoda.Duckov.Core - 核心模块

## 概述
TeamSoda.Duckov.Core 是《逃离鸭科夫》游戏的核心模块，包含了游戏的主要系统、角色控制、UI 管理、关卡管理等核心功能。

## 核心类

### GameManager
游戏管理器，负责管理游戏的整体状态和各个子系统。

#### 静态属性
- `Instance` - 获取 GameManager 单例实例。
- `Paused` - 游戏是否暂停。
- `AudioManager` - 当前音频管理器实例。
- `UiInputManager` - UI 输入管理器。
- `PauseMenu` - 暂停菜单。
- `DifficultyManager` - 难度/规则管理器。
- `SceneLoader` - 场景加载器。
- `BlackScreen` - 黑屏控制器。
- `EventSystem` - Unity 事件系统。
- `NightVision` - 夜视后处理控制。
- `BloodFxOn` - 是否启用流血特效（来自 GameMetaData）。
- `MainPlayerInput` - 当前主玩家输入组件。
- `ModManager` - Mod 管理器实例。
- `NoteIndex` - 笔记索引管理器。
- `AchievementManager` - 成就管理器。

#### 静态字段
- `newBoot` - 标记当前是否为新启动流程。

#### 静态方法
- `TimeTravelDetected()` - 在检测到时间错误/回档时触发处理流程。

---

### CharacterMainControl
角色主控制器，管理角色移动、战斗、技能、交互、物品与 Buff 等核心功能。

#### 公有静态属性
- `Main` - 获取当前关卡主角色实例，若关卡尚未初始化则返回 `null`。

#### 公有静态事件与回调
- `OnMainCharacterStartUseItem` - 主角色开始使用物品时触发（事件）。
- `OnMainCharacterInventoryChangedEvent` - 主角色背包内容变化时触发，提供角色、背包与槽位哈希（委托类型：`Action<CharacterMainControl, Inventory, int>`）。
- `OnMainCharacterSlotContentChangedEvent` - 主角色具体槽位内容变化时触发，提供角色与槽位引用（委托类型：`Action<CharacterMainControl, Slot>`）。
- `OnMainCharacterChangeHoldItemAgentEvent` - 主角色手持物品代理发生变化时触发（`Action<CharacterMainControl, DuckovItemAgent>`）。

#### 公有事件
- `OnTeamChanged` - 队伍变更回调。
- `OnSetPositionEvent` - 角色位置被外部设置时触发。
- `BeforeCharacterSpawnLootOnDead` - 角色生成掉落物前触发，可修改伤害数据。
- `OnActionStartEvent` - 新动作开始执行时通知。
- `OnActionProgressFinishEvent` - 动作进度完成时通知。
- `OnHoldAgentChanged` - 当前手持物品代理变化时通知。
- `OnShootEvent` - 触发射击动作时回调。
- `TryCatchFishInputEvent` - 钓鱼尝试收杆输入时回调。
- `OnAttackEvent` - 近战攻击触发事件。
- `OnSkillStartReleaseEvent` - 技能开始释放时通知。

#### 公有字段
- `characterPreset` - 角色随机预设数据。
- `movementControl` - 角色移动控制组件。
- `agentHolder` - 物品代理持有器。
- `carryAction` - 搬运动作处理组件。
- `characterModel` - 角色模型引用。
- `deadLootBoxPrefab` - 角色死亡后生成的战利品箱预制体。
- `modelRoot` - 模型根节点变换。
- `buffResist` - 角色具备免疫的 Buff 标签列表。
- `reloadAction` - 装填动作控制。
- `skillAction` - 角色技能动作控制。
- `useItemAction` - 使用物品动作控制。
- `interactAction` - 交互动作控制。
- `dashAction` - 冲刺动作控制。
- `attackAction` - 攻击动作控制。
- `mainDamageReceiver` - 主体伤害接收组件。
- `weightThreshold_Light` - 轻负重阈值 (0.25)。
- `weightThreshold_Heavy` - 重负重阈值 (0.5)。
- `weightThreshold_superWeight` - 超重阈值 (0.75)。

#### 公有属性（基础引用与状态）
- `AudioVoiceType` - 当前语音类型（设置时会同步给 AudioManager）。
- `FootStepMaterialType` - 当前脚步声材质类型。
- `Team` - 当前所属队伍。
- `CharacterItem` - 绑定的角色物品条目。
- `CurrentHoldItemAgent` - 当前手持物品代理。
- `Hidden` - 是否处于隐藏状态。
- `CurrentUsingAimSocket` - 当前用于瞄准的插槽变换。
- `RightHandSocket` - 角色右手插槽变换。
- `CurrentAimDirection` - 当前角色朝向/瞄准方向。
- `CurrentMoveDirection` - 当前移动方向（XZ 平面）。
- `AnimationMoveSpeedValue` - 动画驱动的移动速度参数。
- `AnimationLocalMoveDirectionValue` - 动画驱动的本地移动方向参数。
- `Running` - 是否处于奔跑状态。
- `IsOnGround` - 是否在地面上。
- `Velocity` - 当前刚体速度。
- `ThermalOn` - 是否启用热成像。
- `IsInAdsInput` - 是否接收到瞄准（ADS）输入。
- `AdsValue` - 当前瞄准插值数值。
- `AimType` - 当前瞄准类型。
- `NeedToSearchTarget` - 触屏操作下是否需要自动寻找目标。
- `CurrentAction` - 当前执行的角色动作。
- `Health` - 角色生命组件。
- `MoveInput` - 移动输入向量。
- `EquipmentController` - 装备控制器。
- `Dashing` - 是否正在冲刺。
- `IsMainCharacter` - 是否为 LevelManager 主角色。

#### 公有属性（数值与能力）
- `CharacterWalkSpeed` - 当前步行速度。
- `AdsWalkSpeedMultiplier` - 瞄准时移动速度倍率。
- `CharacterOriginWalkSpeed` - 角色基础移动速度。
- `CharacterRunSpeed` - 实际奔跑速度（包含装备影响）。
- `StormProtection` - 风暴伤害防护。
- `WaterEnergyRecoverMultiplier` - 饮水体力恢复倍率。
- `GunDistanceMultiplier` - 枪械射程倍率。
- `CharacterMoveability` - 移动能力系数。
- `CharacterRunAcc` - 奔跑加速度。
- `CharacterTurnSpeed` - 转向速度。
- `CharacterAimTurnSpeed` - 瞄准时转向速度。
- `DashSpeed` - 冲刺速度。
- `PetCapcity` - 宠物数量上限。
- `DashCanControl` - 冲刺时是否可操作方向。
- `MaxStamina` - 最大耐力值。
- `CurrentStamina` - 当前耐力值。
- `StaminaDrainRate` - 耐力消耗速率。
- `StaminaRecoverRate` - 耐力恢复速率。
- `StaminaRecoverTime` - 恢复耐力需要的时间。
- `CharacterWalkAcc` - 步行加速度。
- `VisableDistanceFactor` - 可视距离系数。
- `MaxWeight` - 最大负重。
- `FoodGain` - 食物收益。
- `HealGain` - 治疗收益。
- `MaxEnergy` - 最大精力。
- `EnergyCostPerMin` - 每分钟精力消耗。
- `CurrentEnergy` - 当前精力值。
- `MaxWater` - 最大水分值。
- `WaterCostPerMin` - 每分钟水分消耗。
- `CurrentWater` - 当前水分值。
- `NightVisionAbility` - 夜视能力。
- `NightVisionType` - 夜视类型。
- `HearingAbility` - 听力能力值。
- `SoundVisable` - 声音可视化强度。
- `ViewAngle` - 视野角度。
- `ViewDistance` - 视距。
- `SenseRange` - 感知范围。
- `MeleeDamageMultiplier` - 近战伤害倍率。
- `MeleeCritRateGain` - 近战暴击率增益。
- `MeleeCritDamageGain` - 近战暴击伤害增益。
- `GunDamageMultiplier` - 枪械伤害倍率。
- `ReloadSpeedGain` - 装填速度增益。
- `GunCritRateGain` - 枪械暴击率增益。
- `GunCritDamageGain` - 枪械暴击伤害增益。
- `GunBulletSpeedMultiplier` - 子弹速度倍率。
- `RecoilControl` - 后坐力控制。
- `GunScatterMultiplier` - 枪械散布倍率。
- `InventoryCapacity` - 背包容量加成。
- `HasGasMask` - 是否装备防毒面具。
- `WalkSoundRange` - 步行声音范围。
- `RunSoundRange` - 奔跑声音范围。
- `FlashLight` - 是否启用手电。
- `SoundKey` - 当前音效 Key。

#### 主要方法
- `GetAimRange()` - 依据瞄准类型与装备计算射程。
- `GetCurrentAimPoint()` - 获取当前输入瞄准点。
- `GetCurrentSkillAimPoint()` - 获取当前技能瞄准点。
- `SwitchWeapon(int dir)` - 按方向切换武器（-1 近战，0 主武器，1 副武器）。
- `CanEditInventory()` - 当前状态是否允许打开背包。
- `SetMoveInput(Vector3 moveInput)` - 设置移动输入。
- `MeleeWeaponSlot()` / `PrimWeaponSlot()` / `SecWeaponSlot()` - 获取对应武器槽位。
- `GetSlot(int hash)` - 通过槽位哈希获取槽位。
- `SwitchToFirstAvailableWeapon()` - 切换到首个可用武器。
- `SwitchToWeapon(int index)` - 切换到指定索引的武器。
- `ToggleNightVision()` - 切换夜视。
- `Dash()` - 执行冲刺动作。
- `TryCatchFishInput()` - 钓鱼时尝试收杆。
- `HasNearByHalfObsticle()` - 检查附近是否有半掩体。
- `SwitchToWeaponBeforeUse()` - 恢复使用物品前的武器。
- `SetForceMoveVelocity(Vector3 velocity)` - 设置强制移动速度。
- `SetAimPoint(Vector3 aimPoint)` - 设置瞄准点。
- `Attack()` - 执行近战攻击，返回是否成功。
- `SetAimType(AimTypes aimType)` - 设置瞄准类型。
- `SetRunInput(bool run)` / `SetAdsInput(bool ads)` - 设置奔跑/瞄准输入状态。
- `TryToReload(Item preferedBulletToLoad = null)` - 尝试装填弹药，可传入优先子弹。
- `SetSkill(SkillTypes skillType, SkillBase skill, GameObject bindingObject)` - 绑定技能。
- `StartSkillAim(SkillTypes skillType)` - 开始技能瞄准。
- `ReleaseSkill(SkillTypes skillType)` - 释放技能。
- `CancleSkill()` - 取消技能。
- `GetCurrentRunningSkill()` - 获取当前执行的技能。
- `GetGunReloadable()` - 当前枪械是否可装填。
- `CanUseHand()` - 是否可以进行手部操作。
- `CanControlAim()` - 是否可以控制瞄准。
- `StartAction(CharacterActionBase newAction)` - 尝试启动新的角色动作。
- `SwitchHoldAgentInSlot(int slotHash)` - 切换指定槽位的持有物。
- `SwitchInteractSelection(int dir)` - 在可交互对象中切换选择。
- `SetTeam(Teams team)` - 设置队伍。
- `GetGun()` / `GetMeleeWeapon()` - 获取当前枪械或近战代理。
- `ChangeHoldItem(Item item)` - 切换持有物品。
- `SetItem(Item item)` - 直接设置角色物品。
- `Trigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)` - 处理触发器输入。
- `CanMove()` - 是否允许移动。
- `PopText(string text, float speed = -1f)` - 在角色上弹出文本。
- `CanRun()` - 当前状态是否允许奔跑。
- `IsAiming()` - 是否正在瞄准。
- `DestroyCharacter()` - 销毁角色实体。
- `TriggerShootEvent(DuckovItemAgent shootByAgent)` - 手动触发射击事件。
- `SetCharacterModel(CharacterModel characterModel)` - 绑定角色模型。
- `TickVariables(float deltaTime, float tickTime)` - 更新角色缓存参数。
- `UpdateThirstyAndStarve()` - 更新饥渴状态。
- `UpdateWeightState()` - 更新负重状态。
- `PickupItem(Item item)` - 拾取物品。
- `GetInteractableTargetToInteract()` - 获取当前可交互目标。
- `Interact(InteractableBase target)` / `Interact()` - 执行交互。
- `AddHealth(float healthValue)` - 增加生命值。
- `SetRelatedScene(int relatedScene, bool setActiveByPlayerDistance = true)` - 绑定相关场景并根据距离激活。
- `Carry(Carriable target)` - 搬运可携带物体。
- `AddEnergy(float energyValue)` - 增加精力。
- `AddWater(float waterValue)` - 增加水分。
- `DropAllItems()` - 丢弃所有物品。
- `DestroyAllItem()` - 销毁所有物品。
- `DestroyItemsThatNeededToBeDestriedInBase()` - 在基地销毁需要移除的物品。
- `AddSubVisuals(CharacterSubVisuals subVisuals)` / `RemoveVisual(CharacterSubVisuals subVisuals)` - 添加或移除子视觉。
- `Hide()` / `Show()` - 隐藏或显示角色。
- `IsNearByHalfObsticle(GameObject target)` - 判断指定对象是否半掩体。
- `GetNearByHalfObsticles()` - 获取附近半掩体列表。
- `AddnearByHalfObsticles(List<GameObject> objs)` / `RemoveNearByHalfObsticles(List<GameObject> objs)` - 管理附近半掩体列表。
- `UseItem(Item item)` - 使用物品。
- `GetBuffManager()` - 获取 Buff 管理器。
- `AddBuff(Buff buffPrefab, CharacterMainControl fromWho = null, int overrideWeaponID = 0)` - 添加 Buff。
- `RemoveBuff(int buffID, bool removeOneLayer)` - 移除指定 Buff。
- `RemoveBuffsByTag(Buff.BuffExclusiveTags tag, bool removeOneLayer)` - 按标签移除 Buff。
- `HasBuff(int buffID)` - 检查是否拥有 Buff。
- `SetPosition(Vector3 pos)` - 设置角色位置并触发事件。
- `GetArmorItem()` / `GetHelmatItem()` / `GetFaceMaskItem()` - 获取护甲相关物品。
- `WeaponRepairLossFactor()` / `EquipmentRepairLossFactor()` - 静态方法，返回维修损耗系数。
- `UseStamina(float value)` - 消耗耐力。

---

### LevelManager
关卡管理器，负责关卡加载、初始化与核心系统引用。

#### 静态属性
- `Instance` - 获取 LevelManager 单例。
- `LootBoxInventoriesParent` - 战利品箱背包的父节点。
- `LootBoxInventories` - 战利品箱背包缓存字典。
- `LevelInitializing` - 是否处于初始化流程。
- `AfterInit` - 是否已进入初始化后阶段。
- `LevelInitializingComment` - 当前初始化状态描述。
- `LevelInited` - 关卡是否完成初始化。
- `Rule` - 当前应用的规则集。

#### 公有属性
- `IsRaidMap` - 是否为突袭关卡。
- `IsBaseLevel` - 是否为基地场景。
- `InputManager` - 输入管理器实例。
- `CharacterCreator` - 角色生成器。
- `ExitCreator` - 撤离点创建器。
- `ExplosionManager` - 爆炸效果管理器。
- `MainCharacter` - 主角色控制器。
- `PetCharacter` - 宠物角色控制器。
- `GameCamera` - 游戏相机。
- `FogOfWarManager` - 战争迷雾管理器。
- `TimeOfDayController` - 昼夜控制器。
- `AIMainBrain` - AI 主脑。
- `PetProxy` - 宠物代理。
- `BulletPool` - 子弹对象池。
- `CustomFaceManager` - 捏脸管理器。
- `LevelTime` - 关卡已运行时间（秒）。

#### 公有字段
- `defaultSkill` - 默认技能实例。
- `loadLevelBeaconIndex` - 加载关卡时的信标索引。
- `MainCharacterItemSaveKey` - 主角物品存档键名。
- `MainCharacterHealthSaveKey` - 主角生命存档键名。

#### 静态事件
- `OnLevelBeginInitializing` - 关卡开始初始化时触发。
- `OnLevelInitialized` - 关卡初始化完成时触发。
- `OnAfterLevelInitialized` - 初始化后回调。
- `OnLevelInitializingCommentChanged` - 初始化状态描述更新时回调。
- `OnEvacuated` - 撤离事件。
- `OnMainCharacterDead` - 主角死亡事件。
- `OnNewGameReport` - 新游戏汇报事件。

#### 主要方法
- `RegisterWaitForInitialization<T>(T toWait)` - 注册在初始化完成前需要等待的对象（实现 `IInitializedQueryHandler`）。
- `UnregisterWaitForInitialization<T>(T obj)` - 取消注册等待对象。
- `RefreshMainCharacterFace()` - 刷新主角外观。
- `NotifyEvacuated(EvacuationInfo info)` - 通知撤离并触发事件。
- `NotifySaveBeforeLoadScene(bool saveToFile)` - 在加载场景前保存数据。
- `TestTeleport()` - 调试用传送函数。
- `GetCurrentLevelInfo()` - 获取当前关卡信息结构体。

#### 结构体
- `LevelInfo` - 描述关卡信息，包含 `isBaseLevel`、`sceneName`、`activeSubSceneID` 字段。

---

### HUDManager
HUD 管理器，负责处理 UI HUD 的显示开关。

#### 静态方法
- `RegisterHideToken(UnityEngine.Object obj)` - 注册隐藏 HUD 的令牌。
- `UnregisterHideToken(UnityEngine.Object obj)` - 释放隐藏令牌。

---

### CraftingManager
制作管理器，负责制作配方解锁与制造流程。

#### 静态属性
- `Instance` - 当前 CraftingManager 单例（仅内部设置）。
- `UnlockedFormulaIDs` - 当前已解锁的配方 ID 枚举。

#### 静态事件
- `OnItemCrafted` - 物品制作完成事件，提供配方与产出物品。
- `OnFormulaUnlocked` - 配方被解锁时触发。

#### 静态方法
- `UnlockFormula(string formulaID)` - 解锁指定配方。
- `IsFormulaUnlocked(string value)` *(internal)* - 检查配方是否解锁。
- `GetFormula(string id)` *(internal)* - 获取配方定义。

#### 方法
- `Craft(string id)` - 根据配方 ID 制作物品，返回异步的物品列表。

---

### InputManager
输入管理器，负责处理玩家输入、设备切换与角色交互。

#### 静态属性
- `InputDevice` - 当前使用的输入设备枚举。
- `InputActived` - 是否允许输入。

#### 公有属性
- `WorldMoveInput` - 世界坐标系下的移动输入。
- `AimTarget` - 当前瞄准目标。
- `MoveAxisInput` - 原始移动轴输入。
- `AimScreenPoint` - 屏幕坐标瞄准点。
- `InputAimPoint` - 计算后的世界瞄准点。
- `MousePos` - 鼠标位置。
- `TriggerInput` - 是否触发开火输入。
- `AimingEnemyHead` - 是否瞄准敌人头部。

#### 公有字段
- `characterMainControl` - 关联的主角色控制器。
- `aimTargetFinder` - 瞄准目标搜索器。
- `runThreshold` - 摇杆判定奔跑的阈值。
- `OnInteractButtonDown` - 静态事件，交互键按下时触发。
- `PrimaryWeaponSlotHash` / `SecondaryWeaponSlotHash` / `MeleeWeaponSlotHash` - 槽位哈希常量。
- `useRunInputBuffer` - 静态标记，是否启用奔跑输入缓冲。

#### 静态事件
- `OnInputDeviceChanged` - 输入设备切换时触发。
- `OnSwitchBulletTypeInput` - 请求切换子弹类型时触发，参数为方向。
- `OnSwitchWeaponInput` - 请求切换武器时触发，参数为方向。

#### 静态方法
- `DisableInput(GameObject source)` - 禁用输入（记录来源）。
- `ActiveInput(GameObject source)` - 启用输入。
- `SetInputDevice(InputManager.InputDevices inputDevice)` - 切换输入设备。

#### 方法
- `SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)` - 设置触发器输入状态。
- `SetSwitchBulletTypeInput(int dir)` - 触发切换子弹类型输入。
- `SetSwitchWeaponInput(int dir)` - 触发切换武器输入。
- `SetSwitchInteractInput(int dir)` - 触发交互对象切换。
- `SetMoveInput(Vector2 axisInput)` - 设置移动轴输入。
- `SetRunInput(bool run)` - 设置奔跑输入。
- `SetAdsInput(bool ads)` - 设置瞄准输入。
- `ToggleView()` - 切换视角。
- `ToggleNightVision()` - 切换夜视。
- `SetAimInputUsingJoystick(Vector2 joystickAxisInput)` - 通过摇杆设置瞄准输入。
- `SetAimType(AimTypes aimType)` - 设置瞄准类型。
- `SetMousePosition(Vector2 mousePosition)` - 记录鼠标位置。
- `SetAimInputUsingMouse(Vector2 mouseDelta)` - 根据鼠标增量调整瞄准。
- `Interact()` - 请求交互。
- `PutAway()` - 收起当前物品。
- `SwitchItemAgent(int index)` - 切换手持物品代理。
- `StopAction()` - 停止当前动作。
- `ReleaseItemSkill()` - 释放物品技能。
- `ReleaseCharacterSkill()` - 释放角色技能。
- `CancleSkill()` - 取消技能并返回是否成功。
- `Dash()` - 请求冲刺。
- `StartCharacterSkillAim()` - 开始角色技能瞄准。
- `StartItemSkillAim()` - 开始物品技能瞄准。
- `AddRecoil(ItemAgent_Gun gun)` - 根据枪械添加后坐力。

---

### Health
生命值系统组件，负责记录生命、护甲、伤害计算与 Buff 应用。

#### 公有属性
- `showHealthBar` - 是否显示血条。
- `Hidden` - 是否隐藏血条显示。
- `MaxHealth` - 最大生命值。
- `IsMainCharacterHealth` - 是否为主角生命组件。
- `CurrentHealth` - 当前生命值。
- `IsDead` - 是否死亡。
- `Invincible` - 是否无敌。
- `BodyArmor` - 身体护甲值。
- `HeadArmor` - 头部护甲值。

#### 公有字段
- `team` - 所属队伍。
- `hasSoul` - 是否在死亡后保留灵魂。
- `OnHealthChange` - 生命值变化 UnityEvent。
- `OnMaxHealthChange` - 最大生命值变化 UnityEvent。
- `healthBarHeight` - 血条高度。
- `autoInit` - 是否在 Awake 时自动初始化。

#### 静态事件
- `OnHurt` - 受伤时触发，提供 Health 与伤害信息。
- `OnDead` - 死亡时触发。
- `OnRequestHealthBar` - 请求显示血条事件。

#### 主要方法
- `TryGetCharacter()` - 获取关联的角色控制器。
- `ElementFactor(ElementTypes type)` - 获取对应元素伤害修正。
- `SetItemAndCharacter(Item item, CharacterMainControl character)` - 绑定物品与角色。
- `Init()` - 初始化生命组件。
- `AddBuff(Buff buffPrefab, CharacterMainControl fromWho, int overrideFromWeaponID = 0)` - 为生命组件添加 Buff。
- `Hurt(DamageInfo damageInfo)` - 处理伤害，返回是否成功命中。
- `RequestHealthBar()` - 主动请求显示血条。
- `DestroyOnDelay()` - 延迟销毁（返回 `UniTask`）。
- `AddHealth(float healthValue)` - 增加生命值。
- `SetHealth(float healthValue)` - 设置生命值。
- `SetInvincible(bool value)` - 设置无敌状态。

---

### ModBehaviour
Mod 行为抽象基类，所有 Mod 需继承此类。

#### 公有属性
- `master` - Mod 管理器实例（仅内部赋值）。
- `info` - Mod 元信息（仅内部赋值）。

#### 公有方法
- `Setup(ModManager master, ModInfo info)` - 初始化 Mod，并调用 `OnAfterSetup`。
- `NotifyBeforeDeactivate()` - 模块停用前通知，调用 `OnBeforeDeactivate`。

#### 可重写方法
- `OnAfterSetup()` - Setup 完成后回调，可重写执行初始化逻辑。
- `OnBeforeDeactivate()` - 停用前回调，可重写清理逻辑。

---

### CharacterInputControl
玩家输入控制器，负责将 `PlayerInput` 动作映射到 `InputManager`，并在角色初始化后持续推送输入状态。

#### 静态属性
- `Instance` *(static)* - 当前激活的输入控制器单例。

#### 公有字段
- `inputManager` - 绑定的 `InputManager` 实例。
- `MoveAxis` / `Run` / `ADS` - 对应移动、奔跑与瞄准的输入动作。
- `MousePos` / `MouseDelta` - 鼠标位置与增量输入。
- `Trigger` - 武器触发输入动作。
- `ScrollWheel` - 鼠标滚轮输入（用于切换武器/交互目标）。
- `SwitchWeapon` / `SwitchInteractAndBulletType` / `SwitchBulletType` - 武器与子弹切换输入。
- `ItemShortcut1` ~ `ItemShortcut8` / `ItemShortcut_Melee` - 快捷物品槽输入动作。
- `Skill_1_StartAim` - 角色技能瞄准输入。
- `Reload` / `StopAction` / `PutAway` / `Dash` / `CancelSkill` 等用于动作控制的输入引用。
- `UI_Inventory` / `UI_Map` / `UI_Quest` - UI 打开/关闭输入。

#### 主要方法
- `OnPlayerMoveInput(InputAction.CallbackContext context)` - 更新角色移动输入。
- `OnPlayerRunInput(InputAction.CallbackContext context)` - 切换奔跑状态。
- `OnPlayerAdsInput(InputAction.CallbackContext context)` - 设置瞄准输入标记。
- `OnPlayerMouseMove(InputAction.CallbackContext context)` / `OnPlayerMouseDelta(InputAction.CallbackContext context)` - 推送鼠标指针与增量。
- `OnPlayerTriggerInputUsingMouseKeyboard(InputAction.CallbackContext context)` - 处理鼠标左键触发器输入，并记录本帧状态。
- `OnToggleViewInput(InputAction.CallbackContext context)` / `OnToggleNightVisionInput(InputAction.CallbackContext context)` - 切换视角或夜视。
- `OnReloadInput(InputAction.CallbackContext context)` / `OnPlayerStopAction(InputAction.CallbackContext context)` / `OnPutAwayInput(InputAction.CallbackContext context)` - 映射常用操作至 `InputManager`。
- `OnDashInput(InputAction.CallbackContext context)` - 请求冲刺。
- `OnInteractInput(InputAction.CallbackContext context)` / `OnSwitchInteractAndBulletTypeInput(InputAction.CallbackContext context)` - 执行交互与交互目标切换。
- `OnSwitchWeaponInput(InputAction.CallbackContext context)` / `OnMouseScollerInput(InputAction.CallbackContext context)` - 响应武器切换与滚轮输入。
- `OnPlayerSwitchItemAgent1/2/.../Melee(InputAction.CallbackContext context)` 与 `OnShortCutInput3~8(InputAction.CallbackContext context)` - 响应快捷物品槽切换。
- `OnUIInventoryInput(InputAction.CallbackContext context)` / `OnUIMapInput(InputAction.CallbackContext context)` / `OnUIQuestViewInput(InputAction.CallbackContext context)` - 打开或关闭界面。
- `OnStartCharacterSkillAim(InputAction.CallbackContext context)` / `OnCharacterSkillRelease(InputAction.CallbackContext context)` - 控制角色技能瞄准与释放。
- `OnCancelSkillInput(InputAction.CallbackContext context)` - 请求取消技能。
- `GetChangeBulletTypeWasPressed()` - 查询本帧是否触发子弹类型切换。

---

### CharacterActionBase 及派生动作
角色动作体系的基类与具体实现，驱动角色在执行互动、攻击、技能时的进度状态。

#### CharacterActionBase
- `Running` - 当前动作是否执行中。
- `ActionTimer` - 经过时间。
- `ActionPriorities` - 动作优先级枚举。
- `UpdateAction(float deltaTime)` - 每帧更新动作。
- `StartActionByCharacter(CharacterMainControl character)` - 绑定角色并启动动作。
- `StopAction()` - 强制停止动作并重置状态。

#### CA_Attack
- `DamageDealed` - 最近一次造成的伤害值。
- `GetProgress()` - 当前攻击动作的进度。

#### CA_Carry
- `GetWeight()` - 返回被搬运目标的重量信息。
- `GetProgress()` - 当前搬运动作进度。

#### CA_Interact
- `MasterInteractableAround` - 最近检测到的交互体集合。
- `InteractTarget` - 当前锁定的交互对象。
- `InteractIndexInGroup` - 当前组内索引。
- `InteractingTarget` - 正在执行交互的对象引用。
- `SearchInteractableAround(bool includeMultiInteraction)` - 搜索周围可交互目标。
- `SwitchInteractable(int dir)` - 按方向切换交互对象。
- `SetInteractableTarget(InteractableBase interactable)` - 指定交互目标。
- `GetProgress()` - 获取当前交互进度。

#### CA_Skill
- `CurrentRunningSkill` - 当前执行中的技能引用。
- `GetSkillKeeper()` - 返回技能保持器。
- `IsSkillHasEnoughStaminaAndCD(SkillTypes type)` - 检测技能冷却与体力。
- `SetNextSkillType(SkillTypes type)` - 指定下一次技能类型。
- `SetSkillOfType(SkillTypes type, SkillBase skill)` - 绑定技能实例。
- `ReleaseSkill(SkillTypes type)` - 请求释放技能。
- `GetProgress()` - 返回技能动作进度。

---

### CharacterEquipmentController
装备控制器，负责维护角色各个装备槽位并监听装备变化。

#### 静态字段
- `equipmentModelHash` / `armorHash` / `helmatHash` / `faceMaskHash` / `backpackHash` / `headsetHash` - 不同装备槽位的哈希常量。

#### 公有事件
- `OnHelmatSlotContentChanged` - 头盔槽内容更新时回调。
- `OnFaceMaskSlotContentChanged` - 面罩槽内容更新时回调。

#### 主要方法
- `SetItem(Slot slot, Item item)` - 为指定槽位设置装备，并触发对应事件。

---

### CharacterRandomPreset
角色随机预设数据，定义 AI 或 NPC 的初始配置、属性与掉落。

#### 公有字段（节选）
- `nameKey` - 名称本地化键。
- `voiceType` / `footstepMaterialType` - 语音与脚步音效类型。
- `lootBoxPrefab` - 对应的战利品箱预制体。
- `team` - 预设所属队伍。
- `showName` / `showHealthBar` / `hasSoul` - 外观表现与灵魂配置。
- `health` / `exp` / `moveSpeedFactor` 等基础数值。
- `specialAttachmentBases` / `buffs` / `buffResist` - 附加特殊附件、初始 Buff 与抗性列表。
- `sightDistance` / `sightAngle` / `reactionTime` / `forgetTime` - AI 感知与反应参数。
- `shootDelay` / `shootTimeRange` / `shootTimeSpaceRange` / `combatMoveRange` - 战斗行为配置。
- `dashCoolTimeRange` / `skillCoolTimeRange` / `skillSuccessChance` 等技能与冲刺参数。
- `wantItem` / `hasCashChance` / `cashRange` - 战利品偏好。

#### 主要方法
- `GetCharacterIcon()` - 根据预设生成角色图标。
- `CreateCharacterAsync(CharacterCreator creator, Transform parent)` - 异步创建角色实例并返回 `UniTask`。

---

### CharacterModel 与 CharacterSubVisuals
角色模型与子视觉系统，负责角色外观、挂载点与特效管理。

#### CharacterModel
- 关键字段：`characterMainControl`、`modelRoot`、`RightHandSocket`、`damageReceiverRadius` 等。
- 事件：`OnDestroyEvent`、`OnCharacterSetEvent`、`OnAttackOrShootEvent`。
- 主要方法：
  - `OnMainCharacterSetted(CharacterMainControl character)` - 绑定主角引用。
  - `AddSubVisuals(CharacterSubVisuals visuals)` / `RemoveVisual(CharacterSubVisuals visuals)` - 管理附属视觉。
  - `SyncHiddenToMainCharacter()` - 同步隐藏状态。
  - `SetFaceFromPreset(CharacterRandomPreset preset)` / `SetFaceFromData(CustomFacePreset presetData)` - 设置自定义脸部。
  - `ForcePlayAttackAnimation()` - 强制播放攻击动画。

#### CharacterSubVisuals
- 字段：`renderers`、`particles`、`lights`、`sodaPointLights`、`mainModel` 等。
- 方法：`AddRenderer(Renderer renderer)`、`SetRenderersHidden(bool hidden)`、`SetCharacter(CharacterModel model)`。

---

### Movement
角色运动组件，封装移动输入与强制运动逻辑。

#### 公有字段
- `characterController` - 关联的 `CharacterMainControl`。
- `targetAimDirection` - 目标朝向。
- `forceMove` / `forceMoveVelocity` - 强制移动标记与速度。

#### 主要方法
- `SetMoveInput(Vector3 moveInput)` - 设置移动输入向量。
- `SetForceMoveVelocity(Vector3 velocity)` - 施加强制移动。
- `SetAimDirection(Vector3 aimDirection)` / `SetAimDirectionToTarget(Transform target)` - 调整瞄准方向。
- `ForceTurnTo(Vector3 direction)` / `ForceSetAimDirectionToAimPoint(Vector3 aimPoint)` - 瞬间旋转至目标。
- `GetMoveAnimationValue()` / `GetLocalMoveDirectionAnimationValue()` - 获取动画驱动的速度与方向值。

---

### ItemAgentHolder 与物品代理
负责角色持有的武器/物品代理管理。

#### ItemAgentHolder
- 属性：`CurrentHoldItemAgent`、`CurrentUsingSocket`、`CurrentHoldGun`、`CurrentHoldMeleeWeapon`、`Skill`。
- 事件：`OnHoldAgentChanged` - 持有代理变更时触发。
- 方法：`ChangeHoldItemAgent(DuckovItemAgent agent)`、`SwitchHoldAgentInSlot(int slotHash)`。

#### ItemAgent_Gun
- 关键属性：`BulletItem`、`ShootSpeed`、`ReloadTime`、`Capacity`、`Durability` / `MaxDurability`、`Damage`、`BurstCount`、`BulletSpeed`、`BulletDistance`、`Penetrate`、`ExplosionDamageMultiplier`、`CritRate`、`CritDamageFactor`、`SoundRange`、`Silenced`、`ArmorPiercing`、`ArmorBreak`、`ShotCount`。
- 事件：`OnMainCharacterShootEvent`、`OnShootEvent`、`OnLoadedEvent`。
- 主要方法：`GetReloadProgress()`、`GetBulletCount()`、`TryShoot()`、`Shoot(Vector3 aimPoint)`、`Reload(Item preferedBullet)`、`ForceSetBullet(Item bullet)`、`UnloadBullet()`、`PlayTriggerSound()`、`AddRecoil(float amount)`、`GetAmmoDescription()`。

#### ItemAgent_MeleeWeapon
- 属性：`Damage`、`CritRate`、`CritDamageFactor`、`ArmorPiercing`、`AttackSpeed`、`AttackRange`、`DealDamageTime`、`StaminaCost`、`BleedChance`、`MoveSpeedMultiplier`、`CharacterDamageMultiplier`、`CharacterCritRateGain`、`CharacterCritDamageGain`、`SoundKey`。
- 方法：`GetAttackProgress()`、`RequestAttack()`。

#### AccessoryBase / Accessory_Lazer
- `AccessoryBase` 提供 `OnItemSet(Item item)` 等初始化钩子供子类实现。
- `Accessory_Lazer` 派生类，开启/关闭激光瞄具并更新可视特效。

---

### 角色创建与自定义外观

#### CharacterCreator
- 字段：`characterPfb` - 角色预制体。
- 方法：`CreateCharacter(CharacterRandomPreset preset, Transform parent)`、`LoadOrCreateCharacterItemInstance(Item item)`。

#### CharacterSpawnerGroupSelector
- 字段：`spawnerRoot`、`groups`、`spawnGroupCountRange`。
- 方法：`Collect()` - 从子节点收集分组；`RandomSpawn()` - 根据范围随机生成角色。

#### CustomFace 系统
- `CustomFaceUI` - 管理自定义外观界面，包含 `tabs`、`skinColorPicker`、各类滑杆与颜色选择器，事件 `OnCustomUIViewChanged`。主要方法：`OpenTab(int index)`、`ApplyPreset(CustomFacePreset preset)`、`SetCanControl(bool canControl)`、`Randomize()`、`ResetToDefault()`、`Apply()`。
- `CustomFaceInstance` - 实时应用外观数据，事件 `OnLoadFaceData`，方法：`ApplyPreset(CustomFacePreset preset)`、`ApplyData(CustomFacePreset preset)`、`SetCustomColor(string key, Color color)`、`GetSocket(CustomFacePartTypes partType)`、`TryGetPartUtility(CustomFacePartTypes partType, out CustomFacePartUtility utility)`。
- `CustomFacePart` / `CustomFacePartInfo` / `CustomFacePartMeta` / `CustomFacePartTypes` - 定义可替换部件、元数据与枚举。
- `CustomFacePartUtility` - 管理部件实例，方法：`SetInstance(CustomFaceInstance instance)`、`ApplyPart(CustomFacePreset preset)`、`ApplyColor(Color color)`、`Reset()`。
- `CustomFaceUIColorPicker` / `CustomFaceUIColorPickerButton` - 颜色选择 UI，事件 `OnSetColor`，方法：`Init(CustomFaceUI master)`、`SetColor(Color color)`、`SetSelected(bool selected)`。

---

### 输入与 UI 系统

#### InputManager
- 字段：`characterMainControl`、`aimTargetFinder`、`runThreshold`。
- 静态字段：`PrimaryWeaponSlotHash`、`SecondaryWeaponSlotHash`、`MeleeWeaponSlotHash`、`useRunInputBuffer`。
- 静态事件：`OnInputDeviceChanged`、`OnSwitchBulletTypeInput`、`OnSwitchWeaponInput`、`OnInteractButtonDown`。
- 主要方法：`SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)`、`SetMoveInput(Vector2 axis)`、`SetRunInput(bool run)`、`SetAdsInput(bool ads)`、`ToggleView()`、`ToggleNightVision()`、`SetAimInputUsingJoystick(Vector2 axis)`、`SetAimInputUsingMouse(Vector2 delta)`、`Interact()`、`PutAway()`、`SwitchItemAgent(int index)`、`StopAction()`、`ReleaseItemSkill()`、`ReleaseCharacterSkill()`、`CancleSkill()`、`Dash()`、`StartCharacterSkillAim()`、`StartItemSkillAim()`、`AddRecoil(ItemAgent_Gun gun)`。

#### UIInputManager
- 静态属性：`Instance`。
- 属性：`Ctrl`、`Alt`、`Shift`、`Point`、`MouseDelta`、`WasClickedThisFrame`。
- 静态事件：`OnNavigate`、`OnConfirm`、`OnToggleIndicatorHUD`、`OnCancelEarly`、`OnCancel`、`OnFastPick`、`OnDropItem`、`OnUseItem`、`OnToggleCameraMode`、`OnWishlistHoveringItem`、`OnNextPage`、`OnPreviousPage`、`OnLockInventoryIndex`、`OnInteractInputContext`。
- 方法：`GetPointRay(Camera camera)` - 根据当前指针位置获取射线。

#### InputRebinder 与 CharacterTouchInputControl
- `InputRebinder` 暴露事件：`OnRebindBegin`、`OnRebindComplete`、`OnBindingChanged`，方法：`RebindAction(string actionName)`、`ResetBinding(string actionName)`、`RestoreDefaults()`、`GetDisplayName(InputBinding binding)`、`SaveBindingOverrides()`。
- `InputRebinderIndicator` - UI 组件，用于展示当前按键绑定。
- `CharacterTouchInputControl` - 面向触控的输入代理，方法：`SetMoveInput(Vector2 axis)`、`SetRunInput(bool run)`、`SetAdsInput(bool ads)`、`SetGunAimInput(Vector2 aim)`、`SetCharacterSkillAimInput(Vector2 aim)`、`StartCharacterSkillAim()`、`CharacterSkillRelease()`、`SetItemSkillAimInput(Vector2 aim)`、`StartItemSkillAim()`、`ItemSkillRelease()`。

#### MultiInteraction 与 HUD
- `MultiInteraction` - 聚合多个 `InteractableBase`，属性 `Interactables` 用于 UI 呈现。
- `MultiInteractionMenuButton` - HUD 按钮组件，提供 `Setup(MultiInteraction multiInteraction, int index)`、`OnClick()` 等回调。
- `InteractSelectionHUD` - 管理交互目标选择，属性 `InteractTarget`，方法：`SetInteractable(MultiInteraction multi, int selection)`、`SetSelection(int index)`。
- `InteractHUD` - 单一交互提示 UI。
- `NotificationProxy` - 方法：`Notify(string text, NotificationProxy.Types type, Sprite icon = null)`。
- `DialogueBubbleProxy` - 方法：`Pop(string text)`、`Pop(string text, DuckovDialogueActor actor)`。
- `StaminaHUD` / `SkillHUD` - UI 控件，分别负责耐力与技能状态显示（主要通过序列化字段驱动）。

---

### 场景与相机系统

#### GameCamera
- 静态属性：`Instance`。
- 属性：`CameraAimingTypes` - 当前瞄准模式。
- 方法：`UpdateFov(float fov)`、`ForceSyncPos()`、`SetTarget(Transform target)`、`UpdatePosition(float deltaTime)`、`IsOffScreen(Vector3 worldPos)`。

#### SceneLoader
- 静态属性：`Instance`、`IsSceneLoading`、`LoadingComment`、`HideTips`。
- 事件：`onStartedLoadingScene`、`onFinishedLoadingScene`、`onBeforeSetSceneActive`、`onAfterSceneInitialize`、`OnSetLoadingComment`。
- 方法：`LoadScene(string sceneName, bool additive = false, bool showTips = true)`、`LoadScene(Scenes sceneEnum)`、`LoadScene(SceneLoadingContext context)`、`LoadTarget(SceneLoadingContext context)`、`LoadBaseScene()`、`LoadMainMenu()`、`NotifyPointerClick()`。

#### SceneLoadingContext 与 SceneLoadingEventsReceiver
- `SceneLoadingContext` - 静态属性 `SceneLoadingContext`，存储当前加载上下文信息。
- `SceneLoadingEventsReceiver` - 接收场景加载事件的挂件组件，通过 UnityEvent 配置回调（无额外公开方法）。

#### GamePrepareProcess
- 负责关卡启动前的准备流程，核心逻辑通过 Inspector 配置的步骤执行。

#### GameClock
- 静态属性：`Instance`、`TimeOfDay`、`Day`、`Now`、`Hour`、`Minut`、`Seconds`、`Milliseconds`、`RealTimePlayed`。
- 事件：`OnGameClockStep` - 时间步进时回调。
- 方法：`GetRealTimePlayedOfSaveSlot(int slotIndex)`、`StepTimeTil(TimeSpan targetTime)`。

---

### 环境与天气
- `FogOfWarManager` - 提供战争迷雾渲染控制，公开字段用于配置渲染器、半径等（通过 Inspector 设置）。
- `TimeOfDayController` - 静态属性 `Instance`，属性 `AtNight`、`CurrentPhase`、`CurrentWeather`、`Time`，方法：`GetTimePhaseNameByPhaseTag(string tag)`、`GetWeatherNameByWeather(TimeOfDayController.Weather weather)`。
- `StormWeather` - 控制风暴天气特效与触发器，包含 `StartStorm()`、`StopStorm()` 等公开方法。

---

### AI、角色生成与声音

#### AIMainBrain
- 属性：`SearchTaskContext`、`CheckObsticleTaskContext`。
- 静态事件：`OnSoundSpawned`、`OnPlayerHearSound`。
- 方法：`MakeSound(AISound sound)`、`AddSearchTask(AIPathSearchTask task)`、`AddCheckObsticleTask(AIPathCheckTask task)`。

#### AICharacterController
- 属性：`CharacterMainControl`、`NoticeFromPos`、`NoticeFromDirection`、`NoticeFromCharacter`。
- 方法：`Init(CharacterMainControl character)`、`NightReactionTimeMultiplier()`、`AddItemSkill(ItemAgent skill)`、`GetItemSkill(ItemAgent skill)`、`CheckAndAddDrugItem(Item item)`、`GetDrugItem()`、`SetNoticedToTarget(CharacterMainControl target)`、`IsHurt()`、`isNoticing()`、`MoveToPos(Vector3 position)`、`HasPath()`、`WaitingForPathResult()`、`StopMove()`、`IsMoving()`、`ReachedEndOfPath()`、`SetTarget(Transform target)`、`SetAimInput(Vector2 aim)`、`PutBackWeapon()`、`TakeOutWeapon()`。

#### CharacterSpawner 体系
- `CharacterSpawnerRoot` - 属性 `RelatedScene`，方法 `AddCreatedCharacter(CharacterMainControl character)`。
- `CharacterSpawnerGroupSelector` - 负责随机选择分组生成，方法 `Collect()`、`RandomSpawn()`。
- `CharacterSpawnerComponentBase` - 可扩展的基类（具体生成逻辑由子类实现）。

---

### 存档与玩家存储

#### SavesSystem
- 静态属性：`SavesSystem`、`CurrentSlot`、`CurrentFilePath`、`IsSaving`、`SavesFolder`、`RestoreFailureMarker`、`GlobalSaveDataFilePath`、`GlobalSaveDataFileName`、`BackupInfo`、`TimeValid`、`Time`。
- 静态事件：`OnSetFile`、`OnSaveDeleted`、`OnCollectSaveData`、`OnRestoreFailureDetected`。
- 主要方法：`GetFullPathToSavesFolder()`、`GetFilePath(int slot)`、`GetSaveFileName(int slot)`、`IsOldSave(string filePath)`、`SetFile(int slot)`、`GetBackupList(int slot)` / `GetBackupList(string path)` / `GetBackupList(DirectoryInfo dir)`、`UpgradeSaveFileAssemblyInfo()`、`RestoreIndexedBackup(int index)`、`GetSaveTimeUTC(string filePath)`、`GetSaveTimeLocal(string filePath)`、`SaveFile(int slot)`、`KeyExisits(string key)`（及重载）、`CollectSaveData()`、`IsOldGame(int slot)`（及重载）、`DeleteCurrentSave()`。

#### PlayerStorage 与 Buffer
- `PlayerStorage` 属性：`Instance`、`Inventory`、`IncomingItemBuffer`、`InteractableLootBox`、`DefaultCapacity`、`Loading`、`TakingItem`、`StorageCapacityCalculationHolder`。
- 事件：`OnRecalculateStorageCapacity`、`OnTakeBufferItem`、`OnItemAddedToBuffer`、`OnLoadingFinished`。
- 方法：`IsAccessableAndNotFull()`、`NotifyCapacityDirty()`、`Push(Item item)`、`RecalculateStorageCapacity()`、`TakeBufferItem()`、`HasInitialized()`。
- `PlayerStorageBuffer` 属性：`Instance`、`Buffer`；方法：`SaveBuffer()`、`LoadBuffer()`。

#### PlayerPositionBackupManager
- 方法：`BackupCurrentPos()`、`StartRecover(float duration)`、`SetPlayerToBackupPos()`。

---

### 其他系统
- `MultiInteraction`、`NotificationProxy`、`DialogueBubbleProxy` 等辅助组件用于交互提示与文本反馈。
- `GameCamera`、`HUDManager` 与 `InputManager` 协同提供 UI 与操作体验。
- `ModBehaviour` / `ModManager`（参见 Mod 模块）提供 Mod 生命周期入口。

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

### 输入控制
```csharp
InputManager.SetInputDevice(InputManager.InputDevices.Gamepad);
InputManager.OnSwitchWeaponInput += dir => Debug.Log($"切换武器方向: {dir}");
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

1. 静态属性在游戏未初始化时可能返回 `null`，使用前请判空。
2. 事件订阅后记得在 `OnDisable` 中取消订阅，避免内存泄漏。
3. 角色相关操作需要在关卡初始化完成后进行。
4. Mod 开发时请继承 `ModBehaviour` 基类并实现必要的重写方法。
5. 制作系统需要先解锁配方才能制作物品，内部将数据持久化到存档。
6. 输入管理器支持多种输入设备，切换时会触发 `OnInputDeviceChanged` 以便更新 UI。
7. 生命值系统与 Buff、护甲等多个组件耦合，伤害计算需考虑这些因素。
8. Buff 系统与生命值系统紧密相关，受伤或死亡时可能触发 Buff。
9. 制作配方的解锁状态会保存在存档中，`UnlockedFormulaIDs` 仅枚举已解锁条目。
10. 关闭或启用输入时请成对调用 `DisableInput` 与 `ActiveInput`，否则可能导致输入被锁定。

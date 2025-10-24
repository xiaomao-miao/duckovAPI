# 快速开始指南

欢迎来到《逃离鸭科夫》Mod开发！本指南将帮助您从零开始创建您的第一个Mod。

## 🎯 概述

《逃离鸭科夫》的Mod系统通过扫描并读取 `Duckov_Data/Mods` 文件夹中的各个子文件夹，以及Steam创意工坊已订阅物品的各个文件夹来加载Mod。每个Mod需要包含dll文件、info.ini和preview.png文件。

## 📁 Mod文件结构

将准备好的Mod文件夹放到《逃离鸭科夫》本体的 `Duckov_Data/Mods` 中，即可在游戏主界面的Mods界面加载该Mod。

假设Mod的名字为"MyMod"，发布的文件结构应该如下：

```
MyMod (文件夹)
├── MyMod.dll          # 主要的Mod代码文件
├── info.ini           # Mod信息配置文件
└── preview.png        # 预览图 (建议256x256分辨率)
```

### info.ini 配置

info.ini 应包含以下参数：

```ini
name=MyMod                    # Mod名称，主要用于加载dll文件
displayName=我的Mod           # 显示的名称
description=这是我的第一个Mod # 显示的描述
```

可选参数：
```ini
publishedFileId=123456789     # Steam创意工坊的ID
```

!!! warning "注意事项"
    在上传Steam Workshop的时候，会复写info.ini。info.ini中原有的信息可能会因此丢失。所以不建议在info.ini中存储除以上项目之外的其他信息。

## 🛠️ 开发环境配置

### 1. 准备游戏本体

在电脑上准备好《逃离鸭科夫》本体，确保游戏已正确安装。

### 2. 创建C#项目

1. 创建一个 .NET Class Library 工程
2. 配置工程参数：
   - **Target Framework**: 建议设置为 `netstandard2.1`
   - 注意删除TargetFramework不支持的功能，比如`<ImplicitUsings>`

### 3. 添加引用

将《逃离鸭科夫》的 `\Duckov_Data\Managed\*.dll` 添加到引用中：

```xml
<ItemGroup>
  <Reference Include="$(DuckovPath)\Duckov_Data\Managed\TeamSoda.*" />
  <Reference Include="$(DuckovPath)\Duckov_Data\Managed\ItemStatsSystem.dll" />
  <Reference Include="$(DuckovPath)\Duckov_Data\Managed\Unity*" />
</ItemGroup>
```

### 4. 完整的csproj示例

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <AssemblyName>MyMod</AssemblyName>
    <RootNamespace>MyMod</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="$(DuckovPath)\Duckov_Data\Managed\TeamSoda.*" />
    <Reference Include="$(DuckovPath)\Duckov_Data\Managed\ItemStatsSystem.dll" />
    <Reference Include="$(DuckovPath)\Duckov_Data\Managed\Unity*" />
  </ItemGroup>

</Project>
```

## 🚀 创建您的第一个Mod

### 基础Mod结构

ModBehaviour 应继承自 `Duckov.Modding.ModBehaviour`。Duckov.Modding.ModBehaviour 是一个继承自 MonoBehaviour 的类，其中包含了一些Mod系统中需要使用的额外功能。

在加载Mod时，《逃离鸭科夫》会创建一个该Mod的GameObject并通过调用 `GameObject.AddComponent(Type)` 的方式创建一个ModBehaviour的实例。

### 示例代码

```csharp
using Duckov.Modding;
using UnityEngine;

namespace MyMod
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        void Start()
        {
            // 监听角色使用物品事件
            CharacterMainControl.OnMainCharacterStartUseItem += OnItemUsed;
            
            Debug.Log("我的Mod已加载！");
        }

        private void OnItemUsed(Item item)
        {
            Debug.Log($"使用了物品: {item.DisplayName}");
        }

        void OnDestroy()
        {
            // 取消事件监听
            CharacterMainControl.OnMainCharacterStartUseItem -= OnItemUsed;
        }
    }
}
```

## 🎮 基础功能示例

### 获取主角色

```csharp
// 获取主角色
CharacterMainControl mainCharacter = CharacterMainControl.Main;

if (mainCharacter != null)
{
    // 获取角色生命值
    Health health = mainCharacter.Health;
    Debug.Log($"当前生命值: {health.CurrentHealth}");
}
```

### 创建自定义物品

```csharp
// 创建自定义物品
Item customItem = new Item();
customItem.SetString("DisplayName", "我的自定义物品");
customItem.SetFloat("Damage", 100f);
customItem.SetInt("Ammo", 30);

// 添加到物品集合
ItemAssetsCollection.AddDynamicEntry("MyCustomItem", customItem);
```

### 本地化支持

```csharp
// 设置本地化文本
SodaCraft.Localizations.LocalizationManager.SetOverrideText("MyMod_ItemName", "我的物品");

// 监听语言切换
SodaCraft.Localizations.LocalizationManager.OnSetLanguage += OnLanguageChanged;

private void OnLanguageChanged(SystemLanguage language)
{
    Debug.Log($"语言切换为: {language}");
}
```

## 🔧 高级功能

### Unity Package支持

使用Unity进行开发时，可以参考本仓库附带的 [manifest.json文件](UnityFiles/manifest.json) 来选择package。

### 自定义游戏物品

- 可调用 `ItemStatsSystem.ItemAssetsCollection.AddDynamicEntry(Item prefab)` 添加自定义物品
- 可调用 `ItemStatsSystem.ItemAssetsCollection.RemoveDynamicEntry(Item prefab)` 将该Mod物品移除
- 自定义物品的prefab上需要配置好TypeID，避免与游戏本体和其他MOD冲突
- 进入游戏时如果未加载对应MOD，存档中的自定义物品会直接消失

### 事件系统

《逃离鸭科夫》提供了丰富的事件系统，您可以监听各种游戏事件：

```csharp
void Start()
{
    // 监听角色事件
    CharacterMainControl.OnMainCharacterStartUseItem += OnItemUsed;
    CharacterMainControl.OnMainCharacterInventoryChangedEvent += OnInventoryChanged;
    
    // 监听生命值事件
    Health.OnHurt += OnCharacterHurt;
    Health.OnHeal += OnCharacterHeal;
    
    // 监听制作事件
    CraftingManager.OnCraftStart += OnCraftStart;
    CraftingManager.OnCraftComplete += OnCraftComplete;
}

private void OnItemUsed(Item item)
{
    Debug.Log($"使用了物品: {item.DisplayName}");
}

private void OnInventoryChanged()
{
    Debug.Log("背包发生变化");
}

private void OnCharacterHurt(float damage)
{
    Debug.Log($"角色受到 {damage} 点伤害");
}

private void OnCharacterHeal(float healAmount)
{
    Debug.Log($"角色恢复了 {healAmount} 点生命值");
}

private void OnCraftStart(string recipeID)
{
    Debug.Log($"开始制作: {recipeID}");
}

private void OnCraftComplete(string recipeID, List<Item> result)
{
    Debug.Log($"制作完成: {recipeID}，获得了 {result.Count} 个物品");
}
```

## 📦 构建和部署

### 1. 构建项目

在Visual Studio或命令行中构建您的项目：

```bash
dotnet build --configuration Release
```

### 2. 准备发布文件

将构建好的dll文件、info.ini和preview.png放入Mod文件夹中。

### 3. 安装Mod

将Mod文件夹复制到《逃离鸭科夫》的 `Duckov_Data/Mods` 目录中。

### 4. 测试Mod

启动游戏，在主界面的Mods界面中应该能看到您的Mod。

## 🐛 调试技巧

### 使用Debug.Log

```csharp
void Start()
{
    Debug.Log("Mod启动");
    Debug.LogWarning("这是一个警告");
    Debug.LogError("这是一个错误");
}
```

### 检查对象状态

```csharp
void Update()
{
    if (CharacterMainControl.Main != null)
    {
        // 主角色存在时的逻辑
    }
    else
    {
        // 主角色不存在时的逻辑
    }
}
```

### 异常处理

```csharp
void Start()
{
    try
    {
        // 可能出错的代码
        CharacterMainControl.OnMainCharacterStartUseItem += OnItemUsed;
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Mod初始化失败: {e.Message}");
    }
}
```

## 📚 下一步

现在您已经了解了Mod开发的基础知识，可以继续探索：

- [角色系统](systems/character.md) - 深入了解角色控制API
- [物品系统](systems/items.md) - 学习物品管理
- [事件系统](guides/events.md) - 掌握事件监听和处理
- [性能优化](guides/performance.md) - 开发高性能Mod

## 🆘 获取帮助

- 📖 查看[常见问题](faq.md)获取常见问题的解答
- 💬 加入开发者社区讨论
- 🐛 报告问题或建议改进
- 📝 贡献文档和示例

---

**开始您的Mod开发之旅吧！** 🎮✨

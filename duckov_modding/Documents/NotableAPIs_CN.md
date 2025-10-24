# 值得注意的API和实现细节

## 物品相关

### 物品生成

使用 ItemAssetsCollection 类里的函数来生成物品

```
//notable functions
public static async UniTask<Item> InstantiateAsync(int typeID)
public static Item InstantiateSync(int typeID) 
```

```
using ItemStatsSystem;

...
//生成一个 a glick (Item #254)
Item glick = ItemAssetsCollection.InstantiateAsync(254);

//对它做一些事，比如把它送给玩家
ItemUtilities.SendToPlayer(glick);
...

```

### Item Utilities

你可以使用 Item Utilities 类里的一些函数来操作物品。比如送到玩家身上之类的。以及其他一些操作。


```
//notable functions

//send to player and storage
public static void SendToPlayer(Item item, bool dontMerge = false, bool sendToStorage = true)
public static bool SendToPlayerCharacter(Item item, bool dontMerge = false)
public static bool SendToPlayerCharacterInventory(Item item, bool dontMerge = false)

//check item's relationship
public static bool IsInPlayerCharacter(this Item item)
public static bool IsInPlayerStorage(this Item item)

//Try plug one item to another's slot
public static bool TryPlug(this Item main, Item part, bool emptyOnly = false, Inventory backupInventory = null, int preferredFirstIndex = 0)
```

### Item

物品类定义在 ItemStatsSystem 命名空间下。


```
//notable function definitions

//使item脱离当前的东西，比如从槽位中移除，从 Inventory 中移除等。
public void Detach()


```

## 角色相关

### CharacterMainControl
CharacterMainControl 是所有角色的核心组件。

```
//notable function definitions

//设置角色的阵营
public void SetTeam(Teams _team)
```

### 敌人生成

(to be written)

## 对话相关

### 大对话

DoSubtitle这个函数原来是私有的，我把它改成了公有(1.0.29之后的版本)，这样大家可以调用它来展示对话。
因为是异步函数，所以需要小心一些，因为多次调用会影响彼此。

```
//function in DialogueUI
public async UniTask DoSubtitle(SubtitlesRequestInfo info)
```

```
using Dialogues;

...
NodeCanvas.DialogueTrees.SubtitlesRequestInfo content = new(...);
...
DialogueUI.instance.DoSubtitle(content);
...

```
### 气泡对话

```
//class
Duckov.UI.DialogueBubbles.DialogueBubblesManager

//function definition
public static async UniTask Show(string text, Transform target, float yOffset = -1, bool needInteraction = false, bool skippable = false,float speed=-1, float duration = 2f)
```

```
using Duckov.UI.DialogueBubbles;

...
DialogueBubblesManager.Show("Hello world!", someGameObject.transform);
...
```

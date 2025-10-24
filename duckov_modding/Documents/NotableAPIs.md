# Notable APIs and Implementation Details

## Items Related

### Item Generation

Use ItemAssetsCollection class to generate item instances.

```
//notable functions
public static async UniTask<Item> InstantiateAsync(int typeID)
public static Item InstantiateSync(int typeID) 
```

```
using ItemStatsSystem;

...
//generate a glick (Item #254)
Item glick = ItemAssetsCollection.InstantiateAsync(254);

//Do something it with it. for example send it to player:
ItemUtilities.SendToPlayer(glick);
...

```

### Item Utilities

You can call functions in ItemUtilities to send item to players' storage.
And do other stuff.

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

Item class is defined in ItemStatsSystem.


```
//notable function definitions

//make the item loose. unplug the item, or move it out of the inventory.
public void Detach()


```

## Character Related

### CharacterMainControl
CharacterMainControl is the core of a character.

```
//notable function definitions

//set the team of a character
public void SetTeam(Teams _team)
```

### Enemy Generation

(to be written)

## Dialogues

### Screen Bottom Dialogue

The main subtitle function is previously only called by the events listeners. I will change it to public after release 1.0.29 so you can call it freely.
But be careful with it since it's an async function, and instances of the calls will interfere themselves.

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
### Dialogue Bubble
Use this to do the bubble.

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

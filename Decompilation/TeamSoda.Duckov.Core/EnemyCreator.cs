using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class EnemyCreator : MonoBehaviour
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0002AC94 File Offset: 0x00028E94
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0002ACA0 File Offset: 0x00028EA0
	private void Start()
	{
		Debug.LogError("This scripts shouldn't exist!", this);
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0002ACCC File Offset: 0x00028ECC
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0002ACE0 File Offset: 0x00028EE0
	private void StartCreate()
	{
		int creatorID = this.GetCreatorID();
		if (MultiSceneCore.Instance != null)
		{
			if (MultiSceneCore.Instance.usedCreatorIds.Contains(creatorID))
			{
				return;
			}
			MultiSceneCore.Instance.usedCreatorIds.Add(creatorID);
		}
		this.CreateCharacterAsync();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0002AD2C File Offset: 0x00028F2C
	private UniTaskVoid CreateCharacterAsync()
	{
		EnemyCreator.<CreateCharacterAsync>d__11 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<EnemyCreator.<CreateCharacterAsync>d__11>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0002AD70 File Offset: 0x00028F70
	private void PlugAccessories()
	{
		Slot slot = this.character.PrimWeaponSlot();
		Item item = (slot != null) ? slot.Content : null;
		if (item == null)
		{
			return;
		}
		CharacterMainControl characterMainControl = this.character;
		Inventory inventory;
		if (characterMainControl == null)
		{
			inventory = null;
		}
		else
		{
			Item characterItem = characterMainControl.CharacterItem;
			inventory = ((characterItem != null) ? characterItem.Inventory : null);
		}
		Inventory inventory2 = inventory;
		if (inventory2 == null)
		{
			return;
		}
		foreach (Item item2 in inventory2)
		{
			if (!(item2 == null))
			{
				item.TryPlug(item2, true, null, 0);
			}
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0002AE10 File Offset: 0x00029010
	private UniTask AddBullet()
	{
		EnemyCreator.<AddBullet>d__13 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<EnemyCreator.<AddBullet>d__13>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0002AE54 File Offset: 0x00029054
	private UniTask<List<Item>> GenerateItems()
	{
		EnemyCreator.<GenerateItems>d__14 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<EnemyCreator.<GenerateItems>d__14>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0002AE98 File Offset: 0x00029098
	private UniTask<Item> LoadOrCreateCharacterItemInstance()
	{
		EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.<>4__this = this;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0002AEDC File Offset: 0x000290DC
	private int GetCreatorID()
	{
		Transform parent = base.transform.parent;
		string text = base.transform.GetSiblingIndex().ToString();
		while (parent != null)
		{
			text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
			parent = parent.parent;
		}
		text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
		return text.GetHashCode();
	}

	// Token: 0x040008BE RID: 2238
	private CharacterMainControl character;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	private AudioManager.VoiceType voiceType;

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private AICharacterController aiController;
}

using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class CharacterCreator : MonoBehaviour
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002583C File Offset: 0x00023A3C
	public CharacterMainControl characterPfb
	{
		get
		{
			return GameplayDataSettings.Prefabs.CharacterPrefab;
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00025848 File Offset: 0x00023A48
	public UniTask<CharacterMainControl> CreateCharacter(Item itemInstance, CharacterModel modelPrefab, Vector3 pos, Quaternion rotation)
	{
		CharacterCreator.<CreateCharacter>d__2 <CreateCharacter>d__;
		<CreateCharacter>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacter>d__.<>4__this = this;
		<CreateCharacter>d__.itemInstance = itemInstance;
		<CreateCharacter>d__.modelPrefab = modelPrefab;
		<CreateCharacter>d__.pos = pos;
		<CreateCharacter>d__.rotation = rotation;
		<CreateCharacter>d__.<>1__state = -1;
		<CreateCharacter>d__.<>t__builder.Start<CharacterCreator.<CreateCharacter>d__2>(ref <CreateCharacter>d__);
		return <CreateCharacter>d__.<>t__builder.Task;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000258AC File Offset: 0x00023AAC
	public UniTask<Item> LoadOrCreateCharacterItemInstance(int itemTypeID)
	{
		CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.itemTypeID = itemTypeID;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}
}

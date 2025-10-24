using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class AISpecialAttachment_SpawnItemOnCritKill : AISpecialAttachmentBase
{
	// Token: 0x060004EA RID: 1258 RVA: 0x000161AC File Offset: 0x000143AC
	protected override void OnInited()
	{
		this.character.BeforeCharacterSpawnLootOnDead += this.BeforeCharacterSpawnLootOnDead;
		this.SpawnItem().Forget();
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000161E0 File Offset: 0x000143E0
	private UniTaskVoid SpawnItem()
	{
		AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5 <SpawnItem>d__;
		<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnItem>d__.<>4__this = this;
		<SpawnItem>d__.<>1__state = -1;
		<SpawnItem>d__.<>t__builder.Start<AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5>(ref <SpawnItem>d__);
		return <SpawnItem>d__.<>t__builder.Task;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00016223 File Offset: 0x00014423
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.BeforeCharacterSpawnLootOnDead -= this.BeforeCharacterSpawnLootOnDead;
		}
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001624C File Offset: 0x0001444C
	private void BeforeCharacterSpawnLootOnDead(DamageInfo dmgInfo)
	{
		this.hasDead = true;
		Debug.Log(string.Format("Die crit:{0}", dmgInfo.crit));
		bool flag = dmgInfo.crit > 0;
		if (this.inverse == flag || this.character == null)
		{
			if (this.itemInstance != null)
			{
				UnityEngine.Object.Destroy(this.itemInstance.gameObject);
			}
			return;
		}
		Debug.Log("pick up on crit");
		if (this.itemInstance != null)
		{
			this.character.CharacterItem.Inventory.AddAndMerge(this.itemInstance, 0);
		}
	}

	// Token: 0x0400041D RID: 1053
	[ItemTypeID]
	public int itemToSpawn;

	// Token: 0x0400041E RID: 1054
	private Item itemInstance;

	// Token: 0x0400041F RID: 1055
	private bool hasDead;

	// Token: 0x04000420 RID: 1056
	public bool inverse;
}

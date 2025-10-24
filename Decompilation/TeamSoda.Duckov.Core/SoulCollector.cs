using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class SoulCollector : MonoBehaviour
{
	// Token: 0x060005D3 RID: 1491 RVA: 0x00019F2E File Offset: 0x0001812E
	private void Awake()
	{
		Health.OnDead += this.OnCharacterDie;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00019F41 File Offset: 0x00018141
	private void OnDestroy()
	{
		Health.OnDead -= this.OnCharacterDie;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00019F54 File Offset: 0x00018154
	private void Update()
	{
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00019F58 File Offset: 0x00018158
	private void OnCharacterDie(Health health, DamageInfo dmgInfo)
	{
		if (!health)
		{
			return;
		}
		if (!health.hasSoul)
		{
			return;
		}
		if (!this.selfCharacter && this.selfAgent.Item)
		{
			this.selfCharacter = this.selfAgent.Item.GetCharacterMainControl();
		}
		if (!this.selfCharacter)
		{
			return;
		}
		if (Vector3.Distance(health.transform.position, this.selfCharacter.transform.position) > 40f)
		{
			return;
		}
		int num = Mathf.RoundToInt(health.MaxHealth / 15f);
		if (num < 1)
		{
			num = 1;
		}
		if (LevelManager.Rule.AdvancedDebuffMode)
		{
			num *= 3;
		}
		this.SpawnCubes(health.transform.position + Vector3.up * 0.75f, num).Forget();
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0001A038 File Offset: 0x00018238
	private UniTaskVoid SpawnCubes(Vector3 startPoint, int times)
	{
		SoulCollector.<SpawnCubes>d__10 <SpawnCubes>d__;
		<SpawnCubes>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnCubes>d__.<>4__this = this;
		<SpawnCubes>d__.startPoint = startPoint;
		<SpawnCubes>d__.times = times;
		<SpawnCubes>d__.<>1__state = -1;
		<SpawnCubes>d__.<>t__builder.Start<SoulCollector.<SpawnCubes>d__10>(ref <SpawnCubes>d__);
		return <SpawnCubes>d__.<>t__builder.Task;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001A08C File Offset: 0x0001828C
	public void AddCube()
	{
		this.AddCubeAsync().Forget();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001A0A8 File Offset: 0x000182A8
	private UniTaskVoid AddCubeAsync()
	{
		SoulCollector.<AddCubeAsync>d__12 <AddCubeAsync>d__;
		<AddCubeAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<AddCubeAsync>d__.<>4__this = this;
		<AddCubeAsync>d__.<>1__state = -1;
		<AddCubeAsync>d__.<>t__builder.Start<SoulCollector.<AddCubeAsync>d__12>(ref <AddCubeAsync>d__);
		return <AddCubeAsync>d__.<>t__builder.Task;
	}

	// Token: 0x04000550 RID: 1360
	public DuckovItemAgent selfAgent;

	// Token: 0x04000551 RID: 1361
	private CharacterMainControl selfCharacter;

	// Token: 0x04000552 RID: 1362
	[ItemTypeID]
	public int soulCubeID = 1165;

	// Token: 0x04000553 RID: 1363
	private Slot cubeSlot;

	// Token: 0x04000554 RID: 1364
	public GameObject addFx;

	// Token: 0x04000555 RID: 1365
	public SoulCube cubePfb;
}

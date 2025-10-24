using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000095 RID: 149
[RequireComponent(typeof(Points))]
public class RandomCharacterSpawner : CharacterSpawnerComponentBase
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600051A RID: 1306 RVA: 0x00017110 File Offset: 0x00015310
	private float minDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.minDistanceToPlayer;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001711D File Offset: 0x0001531D
	private int scene
	{
		get
		{
			return this.spawnerRoot.RelatedScene;
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001712A File Offset: 0x0001532A
	private void ShowGizmo()
	{
		RandomCharacterSpawner.currentGizmosTag = this.gizmosTag;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00017137 File Offset: 0x00015337
	public override void Init(CharacterSpawnerRoot root)
	{
		this.spawnerRoot = root;
		if (this.spawnPoints == null)
		{
			this.spawnPoints = base.GetComponent<Points>();
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001715A File Offset: 0x0001535A
	private void OnDestroy()
	{
		this.destroied = true;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00017164 File Offset: 0x00015364
	private CharacterRandomPresetInfo GetAPresetByWeight()
	{
		if (this.totalWeight < 0f)
		{
			this.totalWeight = 0f;
			for (int i = 0; i < this.randomPresetInfos.Count; i++)
			{
				if (this.randomPresetInfos[i].randomPreset == null)
				{
					this.randomPresetInfos.RemoveAt(i);
					i--;
					Debug.Log("Null preset");
				}
				else
				{
					this.totalWeight += this.randomPresetInfos[i].weight;
				}
			}
		}
		float num = UnityEngine.Random.Range(0f, this.totalWeight);
		float num2 = 0f;
		for (int j = 0; j < this.randomPresetInfos.Count; j++)
		{
			num2 += this.randomPresetInfos[j].weight;
			if (num < num2)
			{
				return this.randomPresetInfos[j];
			}
		}
		Debug.LogError("权重计算错误", base.gameObject);
		return this.randomPresetInfos[this.randomPresetInfos.Count - 1];
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001726C File Offset: 0x0001546C
	public override void StartSpawn()
	{
		this.CreateAsync().Forget();
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00017288 File Offset: 0x00015488
	private UniTaskVoid CreateAsync()
	{
		RandomCharacterSpawner.<CreateAsync>d__25 <CreateAsync>d__;
		<CreateAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateAsync>d__.<>4__this = this;
		<CreateAsync>d__.<>1__state = -1;
		<CreateAsync>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAsync>d__25>(ref <CreateAsync>d__);
		return <CreateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000172CC File Offset: 0x000154CC
	private UniTask<CharacterMainControl> CreateAt(Vector3 point, int scene, CharacterSpawnerGroup group, bool isLeader)
	{
		RandomCharacterSpawner.<CreateAt>d__26 <CreateAt>d__;
		<CreateAt>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateAt>d__.<>4__this = this;
		<CreateAt>d__.point = point;
		<CreateAt>d__.scene = scene;
		<CreateAt>d__.group = group;
		<CreateAt>d__.isLeader = isLeader;
		<CreateAt>d__.<>1__state = -1;
		<CreateAt>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAt>d__26>(ref <CreateAt>d__);
		return <CreateAt>d__.<>t__builder.Task;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00017330 File Offset: 0x00015530
	private void OnDrawGizmos()
	{
		if (RandomCharacterSpawner.currentGizmosTag != this.gizmosTag)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		if (this.spawnPoints && this.spawnPoints.points.Count > 0)
		{
			Vector3 point = this.spawnPoints.GetPoint(0);
			Vector3 vector = point + Vector3.up * 20f;
			Gizmos.DrawWireSphere(point, 10f);
			Gizmos.DrawLine(point, vector);
			Gizmos.DrawSphere(vector, 3f);
		}
	}

	// Token: 0x04000495 RID: 1173
	public Points spawnPoints;

	// Token: 0x04000496 RID: 1174
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x04000497 RID: 1175
	public CharacterSpawnerGroup masterGroup;

	// Token: 0x04000498 RID: 1176
	public List<CharacterRandomPresetInfo> randomPresetInfos;

	// Token: 0x04000499 RID: 1177
	private float delayTime = 1f;

	// Token: 0x0400049A RID: 1178
	public Vector2Int spawnCountRange;

	// Token: 0x0400049B RID: 1179
	private float totalWeight = -1f;

	// Token: 0x0400049C RID: 1180
	public bool isStaticTarget;

	// Token: 0x0400049D RID: 1181
	public static string currentGizmosTag;

	// Token: 0x0400049E RID: 1182
	public bool firstIsLeader;

	// Token: 0x0400049F RID: 1183
	private bool firstCreateStarted;

	// Token: 0x040004A0 RID: 1184
	public UnityEvent OnStartCreateEvent;

	// Token: 0x040004A1 RID: 1185
	private int targetSpawnCount;

	// Token: 0x040004A2 RID: 1186
	private int currentSpawnedCount;

	// Token: 0x040004A3 RID: 1187
	private bool destroied;

	// Token: 0x040004A4 RID: 1188
	public string gizmosTag;
}

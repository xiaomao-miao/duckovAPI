using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class CharacterSpawnerGroup : CharacterSpawnerComponentBase
{
	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001671D File Offset: 0x0001491D
	public AICharacterController LeaderAI
	{
		get
		{
			return this.leaderAI;
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00016725 File Offset: 0x00014925
	public void Collect()
	{
		this.spawners = base.GetComponentsInChildren<RandomCharacterSpawner>().ToList<RandomCharacterSpawner>();
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00016738 File Offset: 0x00014938
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (randomCharacterSpawner == null)
			{
				Debug.LogError("生成器引用为空：" + base.gameObject.name);
			}
			else
			{
				randomCharacterSpawner.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x000167B8 File Offset: 0x000149B8
	public void Awake()
	{
		this.characters = new List<AICharacterController>();
		if (this.hasLeader && UnityEngine.Random.Range(0f, 1f) > this.hasLeaderChance)
		{
			this.hasLeader = false;
		}
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000167EC File Offset: 0x000149EC
	private void Update()
	{
		if (this.hasLeader && this.leaderAI == null && this.characters.Count > 0)
		{
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] == null)
				{
					this.characters.RemoveAt(i);
					i--;
				}
				else
				{
					this.leaderAI = this.characters[i];
				}
			}
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001686B File Offset: 0x00014A6B
	public void AddCharacterSpawned(AICharacterController _character, bool isLeader)
	{
		_character.group = this;
		if (isLeader)
		{
			this.leaderAI = _character;
		}
		else if (this.hasLeader && !this.leaderAI)
		{
			this.leaderAI = _character;
		}
		this.characters.Add(_character);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000168A8 File Offset: 0x00014AA8
	public override void StartSpawn()
	{
		bool flag = true;
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (!(randomCharacterSpawner == null))
			{
				randomCharacterSpawner.masterGroup = this;
				if (flag && this.hasLeader)
				{
					randomCharacterSpawner.firstIsLeader = true;
				}
				flag = false;
				randomCharacterSpawner.StartSpawn();
			}
		}
	}

	// Token: 0x04000471 RID: 1137
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x04000472 RID: 1138
	public bool hasLeader;

	// Token: 0x04000473 RID: 1139
	[Range(0f, 1f)]
	public float hasLeaderChance = 1f;

	// Token: 0x04000474 RID: 1140
	public List<RandomCharacterSpawner> spawners;

	// Token: 0x04000475 RID: 1141
	private List<AICharacterController> characters;

	// Token: 0x04000476 RID: 1142
	private AICharacterController leaderAI;
}

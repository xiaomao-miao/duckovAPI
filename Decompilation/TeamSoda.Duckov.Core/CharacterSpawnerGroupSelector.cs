using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class CharacterSpawnerGroupSelector : CharacterSpawnerComponentBase
{
	// Token: 0x06000507 RID: 1287 RVA: 0x00016934 File Offset: 0x00014B34
	public void Collect()
	{
		this.groups = base.GetComponentsInChildren<CharacterSpawnerGroup>().ToList<CharacterSpawnerGroup>();
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			characterSpawnerGroup.Collect();
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00016998 File Offset: 0x00014B98
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			if (characterSpawnerGroup == null)
			{
				Debug.LogError("生成器引用为空");
			}
			else
			{
				characterSpawnerGroup.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00016A08 File Offset: 0x00014C08
	public override void StartSpawn()
	{
		if (this.spawnGroupCountRange.y > this.groups.Count)
		{
			this.spawnGroupCountRange.y = this.groups.Count;
		}
		if (this.spawnGroupCountRange.x > this.groups.Count)
		{
			this.spawnGroupCountRange.x = this.groups.Count;
		}
		int count = UnityEngine.Random.Range(this.spawnGroupCountRange.x, this.spawnGroupCountRange.y);
		this.finalCount = count;
		this.RandomSpawn(count);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00016A9B File Offset: 0x00014C9B
	private void OnValidate()
	{
		if (this.groups.Count < 0)
		{
			return;
		}
		if (this.spawnGroupCountRange.x > this.spawnGroupCountRange.y)
		{
			this.spawnGroupCountRange.y = this.spawnGroupCountRange.x;
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00016ADC File Offset: 0x00014CDC
	public void RandomSpawn(int count)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.groups.Count; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < count; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int index2 = list[index];
			list.RemoveAt(index);
			CharacterSpawnerGroup characterSpawnerGroup = this.groups[index2];
			if (characterSpawnerGroup)
			{
				characterSpawnerGroup.StartSpawn();
			}
		}
	}

	// Token: 0x04000477 RID: 1143
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x04000478 RID: 1144
	public List<CharacterSpawnerGroup> groups;

	// Token: 0x04000479 RID: 1145
	public Vector2Int spawnGroupCountRange = new Vector2Int(1, 1);

	// Token: 0x0400047A RID: 1146
	private int finalCount;
}

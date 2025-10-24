using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public abstract class CharacterSpawnerComponentBase : MonoBehaviour
{
	// Token: 0x060004FC RID: 1276
	public abstract void Init(CharacterSpawnerRoot root);

	// Token: 0x060004FD RID: 1277
	public abstract void StartSpawn();
}

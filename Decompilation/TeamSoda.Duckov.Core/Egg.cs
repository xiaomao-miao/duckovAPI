using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class Egg : MonoBehaviour
{
	// Token: 0x0600054D RID: 1357 RVA: 0x00017B92 File Offset: 0x00015D92
	private void Start()
	{
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00017B94 File Offset: 0x00015D94
	public void Init(Vector3 spawnPosition, Vector3 spawnVelocity, CharacterMainControl _fromCharacter, CharacterRandomPreset preset, float _life)
	{
		this.characterPreset = preset;
		base.transform.position = spawnPosition;
		if (this.rb)
		{
			this.rb.position = spawnPosition;
			this.rb.velocity = spawnVelocity;
		}
		this.fromCharacter = _fromCharacter;
		this.life = _life;
		this.inited = true;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00017BF0 File Offset: 0x00015DF0
	private UniTaskVoid Spawn()
	{
		Egg.<Spawn>d__10 <Spawn>d__;
		<Spawn>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Spawn>d__.<>4__this = this;
		<Spawn>d__.<>1__state = -1;
		<Spawn>d__.<>t__builder.Start<Egg.<Spawn>d__10>(ref <Spawn>d__);
		return <Spawn>d__.<>t__builder.Task;
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00017C34 File Offset: 0x00015E34
	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.life && !this.spawned)
		{
			this.spawned = true;
			this.Spawn().Forget();
		}
	}

	// Token: 0x040004BD RID: 1213
	public GameObject spawnFx;

	// Token: 0x040004BE RID: 1214
	public CharacterMainControl fromCharacter;

	// Token: 0x040004BF RID: 1215
	public Rigidbody rb;

	// Token: 0x040004C0 RID: 1216
	private float life;

	// Token: 0x040004C1 RID: 1217
	private CharacterRandomPreset characterPreset;

	// Token: 0x040004C2 RID: 1218
	private bool inited;

	// Token: 0x040004C3 RID: 1219
	private float timer;

	// Token: 0x040004C4 RID: 1220
	private bool spawned;
}

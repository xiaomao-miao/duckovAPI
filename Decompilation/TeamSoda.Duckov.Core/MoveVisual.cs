using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class MoveVisual : MonoBehaviour
{
	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000B97 RID: 2967 RVA: 0x000310C5 File Offset: 0x0002F2C5
	private CharacterMainControl Character
	{
		get
		{
			if (!this.characterModel)
			{
				return null;
			}
			return this.characterModel.characterMainControl;
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x000310E4 File Offset: 0x0002F2E4
	private void Awake()
	{
		foreach (ParticleSystem particleSystem in this.runParticles)
		{
			particleSystem.emission.enabled = this.running;
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00031144 File Offset: 0x0002F344
	private void Update()
	{
		if (!this.Character)
		{
			return;
		}
		if (this.Character.Running != this.running)
		{
			this.running = this.Character.Running;
			foreach (ParticleSystem particleSystem in this.runParticles)
			{
				particleSystem.emission.enabled = this.running;
			}
		}
	}

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x040009E4 RID: 2532
	public List<ParticleSystem> runParticles;

	// Token: 0x040009E5 RID: 2533
	private bool running;
}

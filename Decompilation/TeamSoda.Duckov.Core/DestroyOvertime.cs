using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class DestroyOvertime : MonoBehaviour
{
	// Token: 0x06000A23 RID: 2595 RVA: 0x0002B76D File Offset: 0x0002996D
	private void Awake()
	{
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0002B787 File Offset: 0x00029987
	private void Update()
	{
		this.life -= Time.deltaTime;
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002B7B3 File Offset: 0x000299B3
	private void OnValidate()
	{
		this.ProcessParticleSystem();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002B7BC File Offset: 0x000299BC
	private void ProcessParticleSystem()
	{
		float num = 0f;
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		if (!component)
		{
			return;
		}
		if (component != null)
		{
			ParticleSystem.MainModule main = component.main;
			main.stopAction = ParticleSystemStopAction.None;
			if (main.startLifetime.constant > num)
			{
				num = main.startLifetime.constant;
			}
		}
		ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem.MainModule main2 = componentsInChildren[i].main;
			main2.stopAction = ParticleSystemStopAction.None;
			if (main2.startLifetime.constant > num)
			{
				num = main2.startLifetime.constant;
			}
		}
		this.life = num + 0.2f;
	}

	// Token: 0x040008DC RID: 2268
	public float life = 1f;
}

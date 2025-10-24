using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000175 RID: 373
public class BuffVFX : MonoBehaviour
{
	// Token: 0x06000B58 RID: 2904 RVA: 0x000301A3 File Offset: 0x0002E3A3
	private void Awake()
	{
		if (!this.buff)
		{
			this.buff = base.GetComponent<Buff>();
		}
		this.buff.OnSetupEvent.AddListener(new UnityAction(this.OnSetup));
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x000301DC File Offset: 0x0002E3DC
	private void OnSetup()
	{
		if (this.shockFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.shockFxInstance);
		}
		if (!this.buff || !this.buff.Character || !this.shockFxPfb)
		{
			return;
		}
		this.shockFxInstance = UnityEngine.Object.Instantiate<GameObject>(this.shockFxPfb, this.buff.Character.transform);
		this.shockFxInstance.transform.localPosition = this.offsetFromCharacter;
		this.shockFxInstance.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0003027B File Offset: 0x0002E47B
	private void OnDestroy()
	{
		if (this.shockFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.shockFxInstance);
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00030296 File Offset: 0x0002E496
	public void AutoSetup()
	{
		this.buff = base.GetComponent<Buff>();
	}

	// Token: 0x040009A8 RID: 2472
	public Buff buff;

	// Token: 0x040009A9 RID: 2473
	public GameObject shockFxPfb;

	// Token: 0x040009AA RID: 2474
	private GameObject shockFxInstance;

	// Token: 0x040009AB RID: 2475
	public Vector3 offsetFromCharacter;
}

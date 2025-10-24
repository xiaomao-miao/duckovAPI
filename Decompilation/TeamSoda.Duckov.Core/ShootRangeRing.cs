using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class ShootRangeRing : MonoBehaviour
{
	// Token: 0x06000BC8 RID: 3016 RVA: 0x00031F1A File Offset: 0x0003011A
	private void Awake()
	{
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00031F1C File Offset: 0x0003011C
	private void Update()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			this.character.OnHoldAgentChanged += this.OnAgentChanged;
			this.OnAgentChanged(this.character.CurrentHoldItemAgent);
			return;
		}
		if (this.ringRenderer.gameObject.activeInHierarchy && !this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00031FA0 File Offset: 0x000301A0
	private void LateUpdate()
	{
		if (!this.character)
		{
			return;
		}
		base.transform.rotation = Quaternion.LookRotation(this.character.CurrentAimDirection, Vector3.up);
		base.transform.position = this.character.transform.position;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00031FF6 File Offset: 0x000301F6
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnHoldAgentChanged -= this.OnAgentChanged;
		}
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0003201C File Offset: 0x0003021C
	private void OnAgentChanged(DuckovItemAgent agent)
	{
		if (agent == null)
		{
			return;
		}
		this.gunAgent = this.character.GetGun();
		if (this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(true);
			this.ringRenderer.transform.localScale = Vector3.one * this.character.GetAimRange() * 0.5f;
			return;
		}
		this.ringRenderer.gameObject.SetActive(false);
	}

	// Token: 0x04000A1E RID: 2590
	private CharacterMainControl character;

	// Token: 0x04000A1F RID: 2591
	public MeshRenderer ringRenderer;

	// Token: 0x04000A20 RID: 2592
	private ItemAgent_Gun gunAgent;
}

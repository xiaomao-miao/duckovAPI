using System;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class HalfObsticle : MonoBehaviour
{
	// Token: 0x06000398 RID: 920 RVA: 0x0000FD44 File Offset: 0x0000DF44
	private void Awake()
	{
		this.outline.enabled = false;
		this.defaultVisuals.SetActive(true);
		this.deadVisuals.SetActive(false);
		this.health.OnDeadEvent += this.Dead;
		if (this.airWallCollider)
		{
			this.airWallCollider.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0000FDAA File Offset: 0x0000DFAA
	private void OnValidate()
	{
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000FDAC File Offset: 0x0000DFAC
	public void Dead(DamageInfo dmgInfo)
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.defaultVisuals.SetActive(false);
		this.deadVisuals.SetActive(true);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
	public void OnTriggerEnter(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.AddnearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = true;
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0000FE18 File Offset: 0x0000E018
	public void OnTriggerExit(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.RemoveNearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = false;
		}
	}

	// Token: 0x040002B9 RID: 697
	public Outlinable outline;

	// Token: 0x040002BA RID: 698
	public HealthSimpleBase health;

	// Token: 0x040002BB RID: 699
	public List<GameObject> parts;

	// Token: 0x040002BC RID: 700
	public GameObject defaultVisuals;

	// Token: 0x040002BD RID: 701
	public GameObject deadVisuals;

	// Token: 0x040002BE RID: 702
	public Collider airWallCollider;

	// Token: 0x040002BF RID: 703
	private bool dead;
}

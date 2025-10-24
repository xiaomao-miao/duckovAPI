using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B6 RID: 182
[RequireComponent(typeof(Rigidbody))]
public class Zone : MonoBehaviour
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x0001A56C File Offset: 0x0001876C
	public HashSet<Health> Healths
	{
		get
		{
			return this.healths;
		}
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001A574 File Offset: 0x00018774
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.healths = new HashSet<Health>();
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		this.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Register(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0001A5DC File Offset: 0x000187DC
	private void OnDestroy()
	{
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Unregister(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001A5F8 File Offset: 0x000187F8
	private void OnTriggerEnter(Collider other)
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (!this.healths.Contains(component))
		{
			this.healths.Add(component);
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001A664 File Offset: 0x00018864
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (this.healths.Contains(component))
		{
			this.healths.Remove(component);
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0001A6C6 File Offset: 0x000188C6
	private void OnDisable()
	{
		this.healths.Clear();
	}

	// Token: 0x04000571 RID: 1393
	public bool onlyPlayerTeam;

	// Token: 0x04000572 RID: 1394
	private HashSet<Health> healths;

	// Token: 0x04000573 RID: 1395
	public bool setActiveByDistance = true;

	// Token: 0x04000574 RID: 1396
	private Rigidbody rb;

	// Token: 0x04000575 RID: 1397
	private int sceneBuildIndex = -1;
}

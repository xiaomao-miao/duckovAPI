using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000DF RID: 223
public class OnTriggerEnterEvent : MonoBehaviour
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001FE9B File Offset: 0x0001E09B
	private bool hideLayerMask
	{
		get
		{
			return this.onlyMainCharacter || this.filterByTeam;
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0001FEAD File Offset: 0x0001E0AD
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
	public void Init()
	{
		Collider component = base.GetComponent<Collider>();
		if (component)
		{
			component.isTrigger = true;
		}
		if (this.filterByTeam)
		{
			this.layerMask = 1 << LayerMask.NameToLayer("Character");
		}
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0001FEFD File Offset: 0x0001E0FD
	private void OnCollisionEnter(Collision collision)
	{
		this.OnEvent(collision.gameObject, true);
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0001FF0C File Offset: 0x0001E10C
	private void OnCollisionExit(Collision collision)
	{
		this.OnEvent(collision.gameObject, false);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0001FF1B File Offset: 0x0001E11B
	private void OnTriggerEnter(Collider other)
	{
		this.OnEvent(other.gameObject, true);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0001FF2A File Offset: 0x0001E12A
	private void OnTriggerExit(Collider other)
	{
		this.OnEvent(other.gameObject, false);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0001FF3C File Offset: 0x0001E13C
	private void OnEvent(GameObject other, bool enter)
	{
		if (this.triggerOnce && this.triggered)
		{
			return;
		}
		if (this.onlyMainCharacter)
		{
			if (CharacterMainControl.Main == null || other != CharacterMainControl.Main.gameObject)
			{
				return;
			}
		}
		else
		{
			if ((1 << other.layer | this.layerMask) != this.layerMask)
			{
				return;
			}
			if (this.filterByTeam)
			{
				CharacterMainControl component = other.GetComponent<CharacterMainControl>();
				if (!component)
				{
					return;
				}
				Teams team = component.Team;
				if (!Team.IsEnemy(this.selfTeam, team))
				{
					return;
				}
			}
		}
		this.triggered = true;
		if (enter)
		{
			UnityEvent doOnTriggerEnter = this.DoOnTriggerEnter;
			if (doOnTriggerEnter == null)
			{
				return;
			}
			doOnTriggerEnter.Invoke();
			return;
		}
		else
		{
			UnityEvent doOnTriggerExit = this.DoOnTriggerExit;
			if (doOnTriggerExit == null)
			{
				return;
			}
			doOnTriggerExit.Invoke();
			return;
		}
	}

	// Token: 0x040006BB RID: 1723
	public bool onlyMainCharacter;

	// Token: 0x040006BC RID: 1724
	public bool filterByTeam;

	// Token: 0x040006BD RID: 1725
	public Teams selfTeam;

	// Token: 0x040006BE RID: 1726
	public LayerMask layerMask;

	// Token: 0x040006BF RID: 1727
	public bool triggerOnce;

	// Token: 0x040006C0 RID: 1728
	public UnityEvent DoOnTriggerEnter = new UnityEvent();

	// Token: 0x040006C1 RID: 1729
	public UnityEvent DoOnTriggerExit = new UnityEvent();

	// Token: 0x040006C2 RID: 1730
	private bool triggered;

	// Token: 0x040006C3 RID: 1731
	private bool mainCharacterIn;
}

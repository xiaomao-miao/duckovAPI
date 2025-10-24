using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class ItemAgentHolder : MonoBehaviour
{
	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x000111D1 File Offset: 0x0000F3D1
	public DuckovItemAgent CurrentHoldItemAgent
	{
		get
		{
			return this.currentHoldItemAgent;
		}
	}

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060003E1 RID: 993 RVA: 0x000111DC File Offset: 0x0000F3DC
	// (remove) Token: 0x060003E2 RID: 994 RVA: 0x00011214 File Offset: 0x0000F414
	public event Action<DuckovItemAgent> OnHoldAgentChanged;

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x00011249 File Offset: 0x0000F449
	public Transform CurrentUsingSocket
	{
		get
		{
			if (!this.currentHoldItemAgent)
			{
				this._currentUsingSocketCache = null;
			}
			return this._currentUsingSocketCache;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x00011268 File Offset: 0x0000F468
	public ItemAgent_Gun CurrentHoldGun
	{
		get
		{
			if (this._gunRef && this.currentHoldItemAgent && this._gunRef.gameObject == this.currentHoldItemAgent.gameObject)
			{
				return this._gunRef;
			}
			this._gunRef = null;
			return null;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x000112BC File Offset: 0x0000F4BC
	public ItemAgent_MeleeWeapon CurrentHoldMeleeWeapon
	{
		get
		{
			if (this._meleeRef && this.currentHoldItemAgent && this._meleeRef.gameObject == this.currentHoldItemAgent.gameObject)
			{
				return this._meleeRef;
			}
			this._meleeRef = null;
			return null;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001130F File Offset: 0x0000F50F
	public ItemSetting_Skill Skill
	{
		get
		{
			return this._skillRef;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00011318 File Offset: 0x0000F518
	public DuckovItemAgent ChangeHoldItem(Item item)
	{
		this.DestroyCurrentItemAgent();
		if (item == null)
		{
			Action<DuckovItemAgent> onHoldAgentChanged = this.OnHoldAgentChanged;
			if (onHoldAgentChanged != null)
			{
				onHoldAgentChanged(null);
			}
			return null;
		}
		ItemAgent itemAgent = item.CreateHandheldAgent();
		if (itemAgent == null)
		{
			Action<DuckovItemAgent> onHoldAgentChanged2 = this.OnHoldAgentChanged;
			if (onHoldAgentChanged2 != null)
			{
				onHoldAgentChanged2(null);
			}
			return null;
		}
		this.currentHoldItemAgent = (itemAgent as DuckovItemAgent);
		if (this.currentHoldItemAgent == null)
		{
			UnityEngine.Object.Destroy(itemAgent.gameObject);
			Action<DuckovItemAgent> onHoldAgentChanged3 = this.OnHoldAgentChanged;
			if (onHoldAgentChanged3 != null)
			{
				onHoldAgentChanged3(null);
			}
			return null;
		}
		this.currentHoldItemAgent.SetHolder(this.characterController);
		Transform transform;
		switch (this.currentHoldItemAgent.handheldSocket)
		{
		case HandheldSocketTypes.normalHandheld:
			transform = this.characterController.characterModel.RightHandSocket;
			break;
		case HandheldSocketTypes.meleeWeapon:
			transform = this.characterController.characterModel.MeleeWeaponSocket;
			break;
		case HandheldSocketTypes.leftHandSocket:
			transform = this.characterController.characterModel.LefthandSocket;
			if (transform == null)
			{
				transform = this.characterController.characterModel.RightHandSocket;
			}
			break;
		default:
			transform = this.characterController.characterModel.RightHandSocket;
			break;
		}
		this.currentHoldItemAgent.transform.SetParent(transform, false);
		this._currentUsingSocketCache = transform;
		this.currentHoldItemAgent.transform.localPosition = Vector3.zero;
		this.currentHoldItemAgent.transform.localRotation = Quaternion.identity;
		this.currentHoldItemAgent.Item.onItemTreeChanged += this.OnAgentItemTreeChanged;
		this._gunRef = (this.currentHoldItemAgent as ItemAgent_Gun);
		this._meleeRef = (this.currentHoldItemAgent as ItemAgent_MeleeWeapon);
		if (!this.IsSkillItem(item))
		{
			this._skillRef = null;
		}
		else
		{
			this._skillRef = item.GetComponent<ItemSetting_Skill>();
		}
		if (this._skillRef)
		{
			this.characterController.SetSkill(SkillTypes.itemSkill, this._skillRef.Skill, itemAgent.gameObject);
		}
		else
		{
			this.characterController.SetSkill(SkillTypes.itemSkill, null, null);
		}
		this.holdStadyTimer = 0f;
		this.holdStady = false;
		itemAgent.gameObject.SetActive(false);
		Action<DuckovItemAgent> onHoldAgentChanged4 = this.OnHoldAgentChanged;
		if (onHoldAgentChanged4 != null)
		{
			onHoldAgentChanged4(this.currentHoldItemAgent);
		}
		return this.currentHoldItemAgent;
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00011552 File Offset: 0x0000F752
	public void SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		if (!this.currentHoldItemAgent)
		{
			return;
		}
		if (!this.characterController.CanUseHand())
		{
			return;
		}
		if (this.CurrentHoldGun != null)
		{
			this.CurrentHoldGun.SetTrigger(trigger, triggerThisFrame, releaseThisFrame);
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001158C File Offset: 0x0000F78C
	private void OnDestroy()
	{
		if (this.currentHoldItemAgent)
		{
			this.currentHoldItemAgent.Item.onItemTreeChanged -= this.OnAgentItemTreeChanged;
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x000115B8 File Offset: 0x0000F7B8
	private void DestroyCurrentItemAgent()
	{
		this._skillRef = null;
		if (this.currentHoldItemAgent == null)
		{
			return;
		}
		if (this.currentHoldItemAgent.Item != null)
		{
			this.currentHoldItemAgent.Item.onItemTreeChanged -= this.OnAgentItemTreeChanged;
			this.currentHoldItemAgent.Item.AgentUtilities.ReleaseActiveAgent();
		}
		this.currentHoldItemAgent = null;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00011628 File Offset: 0x0000F828
	private void OnAgentItemTreeChanged(Item item)
	{
		if (item == null || this.currentHoldItemAgent == null || this.currentHoldItemAgent.Item != item || item.GetCharacterItem() != this.characterController.CharacterItem)
		{
			this.DestroyCurrentItemAgent();
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001167D File Offset: 0x0000F87D
	private bool IsSkillItem(Item item)
	{
		return !(item == null) && item.GetBool("IsSkill", false);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00011698 File Offset: 0x0000F898
	private void Update()
	{
		if (this.currentHoldItemAgent != null && !this.holdStady)
		{
			this.holdStadyTimer += Time.deltaTime;
			if (this.holdStadyTimer > this.holdStadyTime)
			{
				this.holdStady = true;
				this.currentHoldItemAgent.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x040002F9 RID: 761
	public CharacterMainControl characterController;

	// Token: 0x040002FA RID: 762
	private DuckovItemAgent currentHoldItemAgent;

	// Token: 0x040002FC RID: 764
	private Transform _currentUsingSocketCache;

	// Token: 0x040002FD RID: 765
	private static int handheldHash = "Handheld".GetHashCode();

	// Token: 0x040002FE RID: 766
	private ItemAgent_Gun _gunRef;

	// Token: 0x040002FF RID: 767
	private ItemAgent_MeleeWeapon _meleeRef;

	// Token: 0x04000300 RID: 768
	private ItemSetting_Skill _skillRef;

	// Token: 0x04000301 RID: 769
	private bool holdStady;

	// Token: 0x04000302 RID: 770
	private float holdStadyTime = 0.15f;

	// Token: 0x04000303 RID: 771
	private float holdStadyTimer;
}

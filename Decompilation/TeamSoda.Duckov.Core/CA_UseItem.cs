using System;
using Duckov;
using FMOD.Studio;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class CA_UseItem : CharacterActionBase, IProgress
{
	// Token: 0x0600024C RID: 588 RVA: 0x0000A7B8 File Offset: 0x000089B8
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.usingItem;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000A7BB File Offset: 0x000089BB
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000A7BE File Offset: 0x000089BE
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000A7C1 File Offset: 0x000089C1
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000A7C4 File Offset: 0x000089C4
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000A7C7 File Offset: 0x000089C7
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000A7CC File Offset: 0x000089CC
	protected override bool OnStart()
	{
		this.agentUsable = null;
		bool flag = false;
		if (this.item.AgentUtilities.ActiveAgent == null)
		{
			if (this.characterController.ChangeHoldItem(this.item) && this.characterController.CurrentHoldItemAgent != null)
			{
				this.agentUsable = (this.characterController.CurrentHoldItemAgent as IAgentUsable);
				flag = true;
			}
		}
		else if (this.item.AgentUtilities.ActiveAgent == this.characterController.CurrentHoldItemAgent)
		{
			flag = true;
		}
		if (flag)
		{
			this.PostActionSound();
		}
		return flag;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000A868 File Offset: 0x00008A68
	protected override void OnStop()
	{
		this.StopSound();
		this.characterController.SwitchToWeaponBeforeUse();
		if (this.item != null && !this.item.IsBeingDestroyed && this.item.GetRoot() != this.characterController.CharacterItem && !this.characterController.PickupItem(this.item))
		{
			this.item.Drop(this.characterController, true);
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000A8E4 File Offset: 0x00008AE4
	public void SetUseItem(Item _item)
	{
		this.item = _item;
		this.hasSound = false;
		UsageUtilities component = this.item.GetComponent<UsageUtilities>();
		if (component)
		{
			this.hasSound = component.hasSound;
			this.actionSound = component.actionSound;
			this.useSound = component.useSound;
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000A938 File Offset: 0x00008B38
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.item == null)
		{
			base.StopAction();
			return;
		}
		if (this.characterController.CurrentHoldItemAgent == null || this.characterController.CurrentHoldItemAgent.Item == null || this.characterController.CurrentHoldItemAgent.Item != this.item)
		{
			Debug.Log("拿的不统一");
			base.StopAction();
			return;
		}
		if (base.ActionTimer > this.characterController.CurrentHoldItemAgent.Item.UseTime)
		{
			this.OnFinish();
			Debug.Log("Use Finished");
			base.StopAction();
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000A9EC File Offset: 0x00008BEC
	private void OnFinish()
	{
		this.item.Use(this.characterController);
		this.PostUseSound();
		if (this.item.Stackable)
		{
			this.item.StackCount = this.item.StackCount - 1;
			return;
		}
		if (this.item.UseDurability)
		{
			if (this.item.Durability <= 0f && !this.item.IsBeingDestroyed)
			{
				this.item.DestroyTree();
				return;
			}
		}
		else
		{
			this.item.DestroyTree();
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000AA7C File Offset: 0x00008C7C
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (this.item != null && base.Running)
		{
			result.inProgress = true;
			result.total = this.item.UseTime;
			result.current = this.actionTimer;
			return result;
		}
		result.inProgress = false;
		return result;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000AAD9 File Offset: 0x00008CD9
	private void OnDestroy()
	{
		this.StopSound();
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000AAE1 File Offset: 0x00008CE1
	private void OnDisable()
	{
		this.StopSound();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000AAE9 File Offset: 0x00008CE9
	private void PostActionSound()
	{
		if (!this.hasSound)
		{
			return;
		}
		this.soundInstance = AudioManager.Post(this.actionSound, base.gameObject);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000AB0B File Offset: 0x00008D0B
	private void PostUseSound()
	{
		if (!this.hasSound)
		{
			return;
		}
		AudioManager.Post(this.useSound, base.gameObject);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000AB28 File Offset: 0x00008D28
	private void StopSound()
	{
		if (this.soundInstance != null)
		{
			this.soundInstance.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x040001D8 RID: 472
	private Item item;

	// Token: 0x040001D9 RID: 473
	public IAgentUsable agentUsable;

	// Token: 0x040001DA RID: 474
	public bool hasSound;

	// Token: 0x040001DB RID: 475
	public string actionSound;

	// Token: 0x040001DC RID: 476
	public string useSound;

	// Token: 0x040001DD RID: 477
	private EventInstance? soundInstance;
}

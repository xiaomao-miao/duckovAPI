using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public abstract class CharacterActionBase : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600025E RID: 606 RVA: 0x0000AB5F File Offset: 0x00008D5F
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x0600025F RID: 607 RVA: 0x0000AB67 File Offset: 0x00008D67
	public float ActionTimer
	{
		get
		{
			return this.actionTimer;
		}
	}

	// Token: 0x06000260 RID: 608
	public abstract CharacterActionBase.ActionPriorities ActionPriority();

	// Token: 0x06000261 RID: 609
	public abstract bool CanMove();

	// Token: 0x06000262 RID: 610
	public abstract bool CanRun();

	// Token: 0x06000263 RID: 611
	public abstract bool CanUseHand();

	// Token: 0x06000264 RID: 612
	public abstract bool CanControlAim();

	// Token: 0x06000265 RID: 613 RVA: 0x0000AB6F File Offset: 0x00008D6F
	public virtual bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000AB72 File Offset: 0x00008D72
	public void UpdateAction(float deltaTime)
	{
		this.actionTimer += deltaTime;
		this.OnUpdateAction(deltaTime);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000AB89 File Offset: 0x00008D89
	protected virtual void OnUpdateAction(float deltaTime)
	{
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000AB8B File Offset: 0x00008D8B
	protected virtual bool OnStart()
	{
		return true;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000AB8E File Offset: 0x00008D8E
	public virtual bool IsStopable()
	{
		return true;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000AB91 File Offset: 0x00008D91
	protected virtual void OnStop()
	{
	}

	// Token: 0x0600026B RID: 619
	public abstract bool IsReady();

	// Token: 0x0600026C RID: 620 RVA: 0x0000AB93 File Offset: 0x00008D93
	public bool StartActionByCharacter(CharacterMainControl _character)
	{
		if (!this.IsReady())
		{
			return false;
		}
		this.characterController = _character;
		if (this.OnStart())
		{
			this.actionTimer = 0f;
			this.running = true;
			return true;
		}
		return false;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000ABC3 File Offset: 0x00008DC3
	public bool StopAction()
	{
		if (!this.running)
		{
			return true;
		}
		if (this.IsStopable())
		{
			this.running = false;
			this.OnStop();
			return true;
		}
		return false;
	}

	// Token: 0x040001DE RID: 478
	private bool running;

	// Token: 0x040001DF RID: 479
	protected float actionTimer;

	// Token: 0x040001E0 RID: 480
	public bool progressHUD = true;

	// Token: 0x040001E1 RID: 481
	public CharacterMainControl characterController;

	// Token: 0x0200042E RID: 1070
	public enum ActionPriorities
	{
		// Token: 0x04001A0E RID: 6670
		Whatever,
		// Token: 0x04001A0F RID: 6671
		Reload,
		// Token: 0x04001A10 RID: 6672
		Attack,
		// Token: 0x04001A11 RID: 6673
		usingItem,
		// Token: 0x04001A12 RID: 6674
		Dash,
		// Token: 0x04001A13 RID: 6675
		Skills,
		// Token: 0x04001A14 RID: 6676
		Fishing,
		// Token: 0x04001A15 RID: 6677
		Interact
	}
}

using System;
using Duckov;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class CA_Carry : CharacterActionBase, IProgress
{
	// Token: 0x06000202 RID: 514 RVA: 0x00009C6B File Offset: 0x00007E6B
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Whatever;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00009C6E File Offset: 0x00007E6E
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00009C71 File Offset: 0x00007E71
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00009C74 File Offset: 0x00007E74
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00009C77 File Offset: 0x00007E77
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00009C7A File Offset: 0x00007E7A
	public override bool IsReady()
	{
		return this.carryTarget != null;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00009C88 File Offset: 0x00007E88
	public float GetWeight()
	{
		if (!base.Running)
		{
			return 0f;
		}
		if (!this.carringTarget)
		{
			return 0f;
		}
		return this.carringTarget.GetWeight();
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00009CB8 File Offset: 0x00007EB8
	public Progress GetProgress()
	{
		return new Progress
		{
			inProgress = false,
			total = 1f,
			current = 1f
		};
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00009CEE File Offset: 0x00007EEE
	protected override bool OnStart()
	{
		this.characterController.ChangeHoldItem(null);
		this.carryTarget.Take(this);
		this.carringTarget = this.carryTarget;
		return true;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00009D16 File Offset: 0x00007F16
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.characterController.CurrentHoldItemAgent != null)
		{
			base.StopAction();
		}
		if (this.carryTarget)
		{
			this.carryTarget.OnCarriableUpdate(deltaTime);
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00009D4B File Offset: 0x00007F4B
	protected override void OnStop()
	{
		this.carryTarget.Drop();
		this.carringTarget = null;
	}

	// Token: 0x040001BC RID: 444
	[HideInInspector]
	public Carriable carryTarget;

	// Token: 0x040001BD RID: 445
	private Carriable carringTarget;

	// Token: 0x040001BE RID: 446
	public Vector3 carryPoint = new Vector3(0f, 1f, 0.8f);
}

using System;
using Duckov;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class CA_Dash : CharacterActionBase, IProgress
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600020E RID: 526 RVA: 0x00009D81 File Offset: 0x00007F81
	private string sfx
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this.overrideSFX))
			{
				return "Char/Footstep/dash";
			}
			return this.overrideSFX;
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00009D9C File Offset: 0x00007F9C
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Dash;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00009D9F File Offset: 0x00007F9F
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00009DA2 File Offset: 0x00007FA2
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00009DA5 File Offset: 0x00007FA5
	public override bool CanUseHand()
	{
		return this.dashCanControl;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00009DAD File Offset: 0x00007FAD
	public override bool CanControlAim()
	{
		return this.dashCanControl;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00009DB8 File Offset: 0x00007FB8
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (base.Running)
		{
			result.inProgress = true;
			result.total = this.dashTime;
			result.current = this.actionTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00009E02 File Offset: 0x00008002
	public override bool IsReady()
	{
		return Time.time - this.lastEndTime >= this.coolTime && !base.Running;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00009E24 File Offset: 0x00008024
	protected override bool OnStart()
	{
		if (this.characterController.CurrentStamina < this.staminaCost)
		{
			return false;
		}
		this.characterController.UseStamina(this.staminaCost);
		this.dashSpeed = this.characterController.DashSpeed;
		this.dashCanControl = this.characterController.DashCanControl;
		if (this.characterController.MoveInput.magnitude > 0f)
		{
			this.dashDirection = this.characterController.MoveInput.normalized;
		}
		else
		{
			this.dashDirection = this.characterController.CurrentAimDirection;
		}
		this.characterController.SetForceMoveVelocity(this.dashSpeed * this.speedCurve.Evaluate(0f) * this.dashDirection);
		if (!this.dashCanControl)
		{
			this.characterController.movementControl.ForceTurnTo(this.dashDirection);
		}
		AudioManager.Post(this.sfx, base.gameObject);
		return true;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00009F1D File Offset: 0x0000811D
	protected override void OnStop()
	{
		this.characterController.SetForceMoveVelocity(this.characterController.CharacterRunSpeed * this.dashDirection);
		this.lastEndTime = Time.time;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00009F4C File Offset: 0x0000814C
	protected override void OnUpdateAction(float deltaTime)
	{
		if ((this.actionTimer > this.dashTime || !base.Running) && base.StopAction())
		{
			return;
		}
		this.characterController.SetForceMoveVelocity(this.dashSpeed * this.speedCurve.Evaluate(Mathf.Clamp01(this.actionTimer / this.dashTime)) * this.dashDirection);
	}

	// Token: 0x040001BF RID: 447
	private float dashSpeed;

	// Token: 0x040001C0 RID: 448
	private bool dashCanControl;

	// Token: 0x040001C1 RID: 449
	public AnimationCurve speedCurve;

	// Token: 0x040001C2 RID: 450
	public float dashTime;

	// Token: 0x040001C3 RID: 451
	public float coolTime = 0.5f;

	// Token: 0x040001C4 RID: 452
	private Vector3 dashDirection;

	// Token: 0x040001C5 RID: 453
	public float staminaCost = 10f;

	// Token: 0x040001C6 RID: 454
	private float lastEndTime = -999f;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private string overrideSFX;
}

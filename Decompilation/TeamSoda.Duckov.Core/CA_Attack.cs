using System;
using Duckov;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class CA_Attack : CharacterActionBase, IProgress
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060001F3 RID: 499 RVA: 0x00009904 File Offset: 0x00007B04
	// (remove) Token: 0x060001F4 RID: 500 RVA: 0x0000993C File Offset: 0x00007B3C
	public event Action OnAttack;

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060001F5 RID: 501 RVA: 0x00009971 File Offset: 0x00007B71
	public bool DamageDealed
	{
		get
		{
			return this.damageDealed;
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00009979 File Offset: 0x00007B79
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Attack;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000997C File Offset: 0x00007B7C
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000997F File Offset: 0x00007B7F
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00009982 File Offset: 0x00007B82
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00009985 File Offset: 0x00007B85
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00009988 File Offset: 0x00007B88
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (base.Running)
		{
			result.inProgress = true;
			result.total = this.attackActionTime;
			result.current = this.actionTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x000099D4 File Offset: 0x00007BD4
	public override bool IsReady()
	{
		if (Time.time - this.lastAttackTime < this.cd)
		{
			return false;
		}
		this.meleeWeapon = this.characterController.GetMeleeWeapon();
		return !(this.meleeWeapon == null) && this.meleeWeapon.StaminaCost <= this.characterController.CurrentStamina && !base.Running;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00009A3C File Offset: 0x00007C3C
	protected override bool OnStart()
	{
		if (!this.characterController.CurrentHoldItemAgent)
		{
			return false;
		}
		this.meleeWeapon = this.characterController.GetMeleeWeapon();
		if (!this.meleeWeapon)
		{
			return false;
		}
		this.characterController.UseStamina(this.meleeWeapon.StaminaCost);
		this.dealDamageTime = this.meleeWeapon.DealDamageTime;
		this.damageDealed = false;
		Action onAttack = this.OnAttack;
		if (onAttack != null)
		{
			onAttack();
		}
		this.CreateAttackSound();
		this.lastAttackTime = Time.time;
		this.cd = 1f / this.meleeWeapon.AttackSpeed;
		this.slashFxDelayTime = this.meleeWeapon.slashFxDelayTime;
		this.slashFxSpawned = false;
		return true;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00009AFD File Offset: 0x00007CFD
	private void CreateAttackSound()
	{
		AudioManager.Post("SFX/Combat/Melee/attack_" + this.meleeWeapon.SoundKey.ToLower(), base.gameObject);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00009B25 File Offset: 0x00007D25
	protected override void OnStop()
	{
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00009B28 File Offset: 0x00007D28
	protected override void OnUpdateAction(float deltaTime)
	{
		if ((this.actionTimer > this.attackActionTime || !base.Running || this.meleeWeapon == null) && base.StopAction())
		{
			return;
		}
		if (!this.slashFxSpawned && base.ActionTimer > this.slashFxDelayTime && this.meleeWeapon && this.meleeWeapon.slashFx)
		{
			this.slashFxSpawned = true;
			Vector3 position = this.characterController.transform.position;
			position.y = this.meleeWeapon.transform.position.y;
			UnityEngine.Object.Instantiate<GameObject>(this.meleeWeapon.slashFx, position, Quaternion.LookRotation(this.characterController.modelRoot.forward, Vector3.up)).transform.SetParent(base.transform);
		}
		if (!this.damageDealed && base.ActionTimer > this.dealDamageTime)
		{
			this.damageDealed = true;
			this.meleeWeapon.CheckAndDealDamage();
		}
	}

	// Token: 0x040001B3 RID: 435
	private float attackActionTime = 0.25f;

	// Token: 0x040001B4 RID: 436
	private ItemAgent_MeleeWeapon meleeWeapon;

	// Token: 0x040001B6 RID: 438
	private float dealDamageTime = 0.1f;

	// Token: 0x040001B7 RID: 439
	private bool damageDealed;

	// Token: 0x040001B8 RID: 440
	private float lastAttackTime = -999f;

	// Token: 0x040001B9 RID: 441
	private float cd = -1f;

	// Token: 0x040001BA RID: 442
	private bool slashFxSpawned;

	// Token: 0x040001BB RID: 443
	private float slashFxDelayTime;
}

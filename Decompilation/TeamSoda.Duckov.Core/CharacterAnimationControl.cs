using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class CharacterAnimationControl : MonoBehaviour
{
	// Token: 0x0600026F RID: 623 RVA: 0x0000ABF8 File Offset: 0x00008DF8
	private void InitHash()
	{
		foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
		{
			this.animatorHashes.Add(animatorControllerParameter.nameHash);
		}
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000AC35 File Offset: 0x00008E35
	private void SetAnimatorBool(int hash, bool value)
	{
		if (!this.animatorHashes.Contains(hash))
		{
			return;
		}
		this.animator.SetBool(hash, value);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000AC53 File Offset: 0x00008E53
	private void SetAnimatorFloat(int hash, float value)
	{
		if (!this.animatorHashes.Contains(hash))
		{
			return;
		}
		this.animator.SetFloat(hash, value);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000AC71 File Offset: 0x00008E71
	private void SetAnimatorInteger(int hash, int value)
	{
		if (!this.animatorHashes.Contains(hash))
		{
			return;
		}
		this.animator.SetInteger(hash, value);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000AC8F File Offset: 0x00008E8F
	private void SetAnimatorTrigger(int hash)
	{
		if (!this.animatorHashes.Contains(hash))
		{
			return;
		}
		this.animator.SetTrigger(hash);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000ACAC File Offset: 0x00008EAC
	private void Awake()
	{
		if (!this.characterModel)
		{
			this.characterModel = base.GetComponent<CharacterModel>();
		}
		this.characterModel.OnCharacterSetEvent += this.OnCharacterSet;
		if (this.characterModel.characterMainControl)
		{
			this.characterMainControl = this.characterModel.characterMainControl;
		}
		this.characterModel.OnAttackOrShootEvent += this.OnAttack;
		this.InitHash();
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000AD29 File Offset: 0x00008F29
	private void OnDestroy()
	{
		if (this.characterModel)
		{
			this.characterModel.OnCharacterSetEvent -= this.OnCharacterSet;
			this.characterModel.OnAttackOrShootEvent -= this.OnAttack;
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000AD66 File Offset: 0x00008F66
	private void OnCharacterSet()
	{
		this.characterMainControl = this.characterModel.characterMainControl;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000AD79 File Offset: 0x00008F79
	private void Start()
	{
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000AD9A File Offset: 0x00008F9A
	private void SetAttackLayerWeight(float weight)
	{
		if (this.attackLayer < 0)
		{
			return;
		}
		this.animator.SetLayerWeight(this.attackLayer, weight);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000ADB8 File Offset: 0x00008FB8
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		this.SetAnimatorFloat(this.hash_MoveSpeed, this.characterMainControl.AnimationMoveSpeedValue);
		Vector2 animationLocalMoveDirectionValue = this.characterMainControl.AnimationLocalMoveDirectionValue;
		this.SetAnimatorFloat(this.hash_MoveDirX, animationLocalMoveDirectionValue.x);
		this.SetAnimatorFloat(this.hash_MoveDirY, animationLocalMoveDirectionValue.y);
		bool value = true;
		if (this.characterMainControl.CurrentHoldItemAgent == null)
		{
			value = false;
		}
		else if (!this.characterMainControl.CurrentHoldItemAgent.gameObject.activeSelf)
		{
			value = false;
		}
		else if (this.characterMainControl.reloadAction.Running)
		{
			value = false;
		}
		this.SetAnimatorBool(this.hash_RightHandOut, value);
		bool flag = this.characterMainControl.Dashing;
		if (flag && !this.hasAnimationIfDashCanControl && this.characterMainControl.DashCanControl)
		{
			flag = false;
		}
		this.SetAnimatorBool(this.hash_Dashing, flag);
		int value2 = 0;
		if (!this.holdAgent)
		{
			this.holdAgent = this.characterMainControl.CurrentHoldItemAgent;
		}
		if (this.holdAgent != null)
		{
			value2 = (int)this.holdAgent.handAnimationType;
		}
		this.SetAnimatorInteger(this.hash_HandState, value2);
		this.UpdateAttackLayerWeight();
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000AEF0 File Offset: 0x000090F0
	private void UpdateAttackLayerWeight()
	{
		if (!this.attacking)
		{
			if (this.weight > 0f)
			{
				this.weight = 0f;
				this.SetAttackLayerWeight(this.weight);
			}
			return;
		}
		this.attackTimer += Time.deltaTime;
		this.weight = this.attackLayerWeightCurve.Evaluate(this.attackTimer / this.attackTime);
		if (this.attackTimer >= this.attackTime)
		{
			this.attacking = false;
			this.weight = 0f;
		}
		this.SetAttackLayerWeight(this.weight);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000AF88 File Offset: 0x00009188
	public void OnAttack()
	{
		if (!this.characterMainControl || !this.holdAgent || this.holdAgent.handAnimationType != HandheldAnimationType.meleeWeapon)
		{
			this.attacking = false;
			return;
		}
		this.attacking = true;
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
		this.SetAnimatorTrigger(this.hash_Attack);
		this.attackTimer = 0f;
	}

	// Token: 0x040001E2 RID: 482
	public CharacterMainControl characterMainControl;

	// Token: 0x040001E3 RID: 483
	public CharacterModel characterModel;

	// Token: 0x040001E4 RID: 484
	public Animator animator;

	// Token: 0x040001E5 RID: 485
	public float attackTime = 0.3f;

	// Token: 0x040001E6 RID: 486
	private int attackLayer = -1;

	// Token: 0x040001E7 RID: 487
	private bool attacking;

	// Token: 0x040001E8 RID: 488
	private float attackTimer;

	// Token: 0x040001E9 RID: 489
	private bool hasAnimationIfDashCanControl;

	// Token: 0x040001EA RID: 490
	public AnimationCurve attackLayerWeightCurve;

	// Token: 0x040001EB RID: 491
	private int hash_MoveSpeed = Animator.StringToHash("MoveSpeed");

	// Token: 0x040001EC RID: 492
	private int hash_MoveDirX = Animator.StringToHash("MoveDirX");

	// Token: 0x040001ED RID: 493
	private int hash_MoveDirY = Animator.StringToHash("MoveDirY");

	// Token: 0x040001EE RID: 494
	private int hash_RightHandOut = Animator.StringToHash("RightHandOut");

	// Token: 0x040001EF RID: 495
	private int hash_HandState = Animator.StringToHash("HandState");

	// Token: 0x040001F0 RID: 496
	private int hash_Dashing = Animator.StringToHash("Dashing");

	// Token: 0x040001F1 RID: 497
	private int hash_Attack = Animator.StringToHash("Attack");

	// Token: 0x040001F2 RID: 498
	private HashSet<int> animatorHashes = new HashSet<int>();

	// Token: 0x040001F3 RID: 499
	private float weight;

	// Token: 0x040001F4 RID: 500
	private DuckovItemAgent holdAgent;
}

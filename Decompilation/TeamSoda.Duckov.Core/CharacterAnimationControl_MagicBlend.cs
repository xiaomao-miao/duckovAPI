using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class CharacterAnimationControl_MagicBlend : MonoBehaviour
{
	// Token: 0x0600027D RID: 637 RVA: 0x0000B0A4 File Offset: 0x000092A4
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
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000B11B File Offset: 0x0000931B
	private void OnDestroy()
	{
		if (this.characterModel)
		{
			this.characterModel.OnCharacterSetEvent -= this.OnCharacterSet;
			this.characterModel.OnAttackOrShootEvent -= this.OnAttack;
		}
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000B158 File Offset: 0x00009358
	private void OnCharacterSet()
	{
		this.characterMainControl = this.characterModel.characterMainControl;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000B16B File Offset: 0x0000936B
	private void Start()
	{
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
		this.animator.SetLayerWeight(this.attackLayer, 0f);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000B1A4 File Offset: 0x000093A4
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		this.animator.SetFloat(this.hash_MoveSpeed, this.characterMainControl.AnimationMoveSpeedValue);
		Vector2 animationLocalMoveDirectionValue = this.characterMainControl.AnimationLocalMoveDirectionValue;
		this.animator.SetFloat(this.hash_MoveDirX, animationLocalMoveDirectionValue.x);
		this.animator.SetFloat(this.hash_MoveDirY, animationLocalMoveDirectionValue.y);
		int value = 0;
		if (!this.holdAgent || !this.holdAgent.isActiveAndEnabled)
		{
			this.holdAgent = this.characterMainControl.CurrentHoldItemAgent;
		}
		else
		{
			value = (int)this.holdAgent.handAnimationType;
		}
		if (this.characterMainControl.carryAction.Running)
		{
			value = -1;
		}
		this.animator.SetInteger(this.hash_HandState, value);
		if (this.holdAgent != null && this.gunAgent == null)
		{
			this.gunAgent = (this.holdAgent as ItemAgent_Gun);
		}
		bool value2 = false;
		if (this.gunAgent != null)
		{
			value2 = true;
			if (this.gunAgent.IsReloading() || this.gunAgent.BulletCount <= 0)
			{
				value2 = false;
			}
		}
		this.animator.SetBool(this.hash_GunReady, value2);
		bool dashing = this.characterMainControl.Dashing;
		this.animator.SetBool(this.hash_Dashing, dashing);
		this.UpdateAttackLayerWeight();
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000B308 File Offset: 0x00009508
	private void UpdateAttackLayerWeight()
	{
		if (!this.attacking)
		{
			if (this.weight > 0f)
			{
				this.weight = 0f;
				this.animator.SetLayerWeight(this.attackLayer, this.weight);
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
		this.animator.SetLayerWeight(this.attackLayer, this.weight);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000B3B4 File Offset: 0x000095B4
	public void OnAttack()
	{
		this.attacking = true;
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
		this.animator.SetTrigger(this.hash_Attack);
		this.attackTimer = 0f;
	}

	// Token: 0x040001F5 RID: 501
	public CharacterMainControl characterMainControl;

	// Token: 0x040001F6 RID: 502
	public CharacterModel characterModel;

	// Token: 0x040001F7 RID: 503
	public Animator animator;

	// Token: 0x040001F8 RID: 504
	public float attackTime = 0.3f;

	// Token: 0x040001F9 RID: 505
	private int attackLayer = -1;

	// Token: 0x040001FA RID: 506
	private bool attacking;

	// Token: 0x040001FB RID: 507
	private float attackTimer;

	// Token: 0x040001FC RID: 508
	private DuckovItemAgent holdAgent;

	// Token: 0x040001FD RID: 509
	private ItemAgent_Gun gunAgent;

	// Token: 0x040001FE RID: 510
	public AnimationCurve attackLayerWeightCurve;

	// Token: 0x040001FF RID: 511
	private int hash_MoveSpeed = Animator.StringToHash("MoveSpeed");

	// Token: 0x04000200 RID: 512
	private int hash_MoveDirX = Animator.StringToHash("MoveDirX");

	// Token: 0x04000201 RID: 513
	private int hash_MoveDirY = Animator.StringToHash("MoveDirY");

	// Token: 0x04000202 RID: 514
	private int hash_Dashing = Animator.StringToHash("Dashing");

	// Token: 0x04000203 RID: 515
	private int hash_Attack = Animator.StringToHash("Attack");

	// Token: 0x04000204 RID: 516
	private int hash_HandState = Animator.StringToHash("HandState");

	// Token: 0x04000205 RID: 517
	private int hash_GunReady = Animator.StringToHash("GunReady");

	// Token: 0x04000206 RID: 518
	private float weight;
}

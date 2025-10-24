using System;
using ECM2;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Movement : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00011717 File Offset: 0x0000F917
	public float walkSpeed
	{
		get
		{
			return this.characterController.CharacterWalkSpeed * (this.characterController.IsInAdsInput ? this.characterController.AdsWalkSpeedMultiplier : 1f);
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00011744 File Offset: 0x0000F944
	public float originWalkSpeed
	{
		get
		{
			return this.characterController.CharacterOriginWalkSpeed;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00011751 File Offset: 0x0000F951
	public float runSpeed
	{
		get
		{
			return this.characterController.CharacterRunSpeed;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0001175E File Offset: 0x0000F95E
	public float walkAcc
	{
		get
		{
			return this.characterController.CharacterWalkAcc;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001176B File Offset: 0x0000F96B
	public float runAcc
	{
		get
		{
			return this.characterController.CharacterRunAcc;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00011778 File Offset: 0x0000F978
	public float turnSpeed
	{
		get
		{
			return this.characterController.CharacterTurnSpeed;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00011785 File Offset: 0x0000F985
	public float aimTurnSpeed
	{
		get
		{
			return this.characterController.CharacterAimTurnSpeed;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00011792 File Offset: 0x0000F992
	public Vector3 MoveInput
	{
		get
		{
			return this.moveInput;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001179A File Offset: 0x0000F99A
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000117A2 File Offset: 0x0000F9A2
	public bool Moving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x000117AA File Offset: 0x0000F9AA
	public bool IsOnGround
	{
		get
		{
			return this.characterMovement.isOnGround;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x000117B7 File Offset: 0x0000F9B7
	public bool StandStill
	{
		get
		{
			return !this.moving && this.characterMovement.velocity.magnitude < 0.1f;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x000117DA File Offset: 0x0000F9DA
	private bool checkCanMove
	{
		get
		{
			return this.characterController.CanMove();
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x000117E7 File Offset: 0x0000F9E7
	private bool checkCanRun
	{
		get
		{
			return this.characterController.CanRun();
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x000117F4 File Offset: 0x0000F9F4
	public Vector3 CurrentMoveDirectionXZ
	{
		get
		{
			return this.currentMoveDirectionXZ;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x000117FC File Offset: 0x0000F9FC
	public Transform rotationRoot
	{
		get
		{
			return this.characterController.modelRoot;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x00011809 File Offset: 0x0000FA09
	public unsafe Vector3 Velocity
	{
		get
		{
			return *this.characterMovement.velocity;
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001181B File Offset: 0x0000FA1B
	private void Awake()
	{
		this.characterMovement.constrainToGround = true;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00011829 File Offset: 0x0000FA29
	public void SetMoveInput(Vector3 _moveInput)
	{
		_moveInput.y = 0f;
		this.moveInput = _moveInput;
		this.moving = false;
		if (this.checkCanMove && this.moveInput.magnitude > 0.02f)
		{
			this.moving = true;
		}
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00011866 File Offset: 0x0000FA66
	public void SetForceMoveVelocity(Vector3 _forceMoveVelocity)
	{
		this.forceMove = true;
		this.forceMoveVelocity = _forceMoveVelocity;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00011876 File Offset: 0x0000FA76
	public void SetAimDirection(Vector3 _aimDirection)
	{
		this.targetAimDirection = _aimDirection;
		this.targetAimDirection.y = 0f;
		this.targetAimDirection.Normalize();
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001189C File Offset: 0x0000FA9C
	public void SetAimDirectionToTarget(Vector3 targetPoint, Transform aimHandler)
	{
		Vector3 position = base.transform.position;
		position.y = 0f;
		Vector3 position2 = aimHandler.position;
		position2.y = 0f;
		targetPoint.y = 0f;
		float num = Vector3.Distance(position, targetPoint);
		float num2 = Vector3.Distance(position, position2);
		if (num < num2 + 0.25f)
		{
			return;
		}
		float num3 = Mathf.Asin(num2 / num) * 57.29578f;
		this.targetAimDirection = Quaternion.Euler(0f, -num3, 0f) * (targetPoint - position).normalized;
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00011938 File Offset: 0x0000FB38
	private void UpdateAiming()
	{
		Vector3 currentAimPoint = this.characterController.GetCurrentAimPoint();
		currentAimPoint.y = base.transform.position.y;
		if (Vector3.Distance(currentAimPoint, base.transform.position) > 0.6f && this.characterController.IsAiming() && this.characterController.CanControlAim())
		{
			this.SetAimDirectionToTarget(currentAimPoint, this.characterController.CurrentUsingAimSocket);
			return;
		}
		if (this.Moving)
		{
			this.SetAimDirection(this.CurrentMoveDirectionXZ);
		}
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x000119C4 File Offset: 0x0000FBC4
	public unsafe void UpdateMovement()
	{
		bool checkCanRun = this.checkCanRun;
		bool checkCanMove = this.checkCanMove;
		if (this.moveInput.magnitude <= 0.02f || !checkCanMove)
		{
			this.moving = false;
			this.running = false;
		}
		else
		{
			this.moving = true;
		}
		if (!checkCanRun)
		{
			this.running = false;
		}
		if (this.moving && checkCanRun)
		{
			this.running = true;
		}
		if (!this.forceMove)
		{
			this.UpdateNormalMove();
		}
		else
		{
			this.UpdateForceMove();
			this.forceMove = false;
		}
		this.UpdateAiming();
		this.UpdateRotation(Time.deltaTime);
		*this.characterMovement.velocity += Physics.gravity * Time.deltaTime;
		this.characterMovement.Move(*this.characterMovement.velocity, Time.deltaTime);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00011A9F File Offset: 0x0000FC9F
	private void Update()
	{
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00011AA1 File Offset: 0x0000FCA1
	public unsafe void ForceSetPosition(Vector3 Pos)
	{
		this.characterMovement.PauseGroundConstraint(1f);
		this.characterMovement.SetPosition(Pos, false);
		*this.characterMovement.velocity = Vector3.zero;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00011AD8 File Offset: 0x0000FCD8
	private unsafe void UpdateNormalMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 target = Vector3.zero;
		float num = this.walkAcc;
		if (this.moving)
		{
			target = this.moveInput * (this.running ? this.runSpeed : this.walkSpeed);
			num = (this.running ? this.runAcc : this.walkAcc);
		}
		target.y = vector.y;
		vector = Vector3.MoveTowards(vector, target, num * Time.deltaTime);
		Vector3 vector2 = vector;
		vector2.y = 0f;
		if (vector2.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector2.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00011B9C File Offset: 0x0000FD9C
	private unsafe void UpdateForceMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 vector2 = this.forceMoveVelocity;
		float walkAcc = this.walkAcc;
		vector2.y = vector.y;
		vector = vector2;
		Vector3 vector3 = vector;
		vector3.y = 0f;
		if (vector3.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector3.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00011C14 File Offset: 0x0000FE14
	public void ForceTurnTo(Vector3 direction)
	{
		this.targetAimDirection = direction.normalized;
		Quaternion rotation = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
		this.rotationRoot.rotation = rotation;
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00011C68 File Offset: 0x0000FE68
	private void UpdateRotation(float deltaTime)
	{
		if (this.targetAimDirection.magnitude < 0.1f)
		{
			this.targetAimDirection = this.rotationRoot.forward;
		}
		float num = this.turnSpeed;
		if (this.characterController.IsAiming() && this.characterController.IsMainCharacter)
		{
			num = this.aimTurnSpeed;
		}
		if (this.targetAimDirection.magnitude > 0.1f)
		{
			Quaternion to = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
			this.rotationRoot.rotation = Quaternion.RotateTowards(this.rotationRoot.rotation, to, num * deltaTime);
		}
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00011D1E File Offset: 0x0000FF1E
	public void ForceSetAimDirectionToAimPoint()
	{
		this.UpdateRotation(99999f);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00011D2C File Offset: 0x0000FF2C
	public float GetMoveAnimationValue()
	{
		float magnitude = this.characterMovement.velocity.magnitude;
		float num;
		if (this.moving && this.running)
		{
			num = Mathf.InverseLerp(this.walkSpeed, this.runSpeed, magnitude) + 1f;
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		else
		{
			num = Mathf.Clamp01(magnitude / this.walkSpeed);
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		if (this.walkSpeed <= 0f)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00011DC0 File Offset: 0x0000FFC0
	public Vector2 GetLocalMoveDirectionAnimationValue()
	{
		Vector2 up = Vector2.up;
		if (!this.StandStill)
		{
			Vector3 direction = this.currentMoveDirectionXZ;
			Vector3 vector = this.rotationRoot.InverseTransformDirection(direction);
			up.x = vector.x;
			up.y = vector.z;
		}
		return up;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00011E0A File Offset: 0x0001000A
	private void FixedUpdate()
	{
	}

	// Token: 0x04000304 RID: 772
	public CharacterMainControl characterController;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private CharacterMovement characterMovement;

	// Token: 0x04000306 RID: 774
	public Vector3 targetAimDirection;

	// Token: 0x04000307 RID: 775
	private Vector3 moveInput;

	// Token: 0x04000308 RID: 776
	private bool running;

	// Token: 0x04000309 RID: 777
	private bool moving;

	// Token: 0x0400030A RID: 778
	private Vector3 currentMoveDirectionXZ;

	// Token: 0x0400030B RID: 779
	public bool forceMove;

	// Token: 0x0400030C RID: 780
	public Vector3 forceMoveVelocity;

	// Token: 0x0400030D RID: 781
	private const float movingInputThreshold = 0.02f;
}

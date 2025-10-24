using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000293 RID: 659
	public class Hook : MiniGameBehaviour
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0004FC57 File Offset: 0x0004DE57
		public Transform Axis
		{
			get
			{
				return this.hookAxis;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x0004FC5F File Offset: 0x0004DE5F
		public Hook.HookStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x0004FC67 File Offset: 0x0004DE67
		private float RopeDistance
		{
			get
			{
				return Mathf.Lerp(this.minDist, this.maxDist, this.ropeControl);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0004FC80 File Offset: 0x0004DE80
		private float AxisAngle
		{
			get
			{
				return Mathf.Lerp(-this.maxAngle, this.maxAngle, (this.axisControl + 1f) / 2f);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x0004FCA8 File Offset: 0x0004DEA8
		private bool RopeOutOfBound
		{
			get
			{
				Vector3 point = Quaternion.Euler(0f, 0f, this.AxisAngle) * Vector2.down * this.RopeDistance;
				return !this.bounds.Contains(point);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x0004FCFC File Offset: 0x0004DEFC
		public GoldMinerEntity GrabbingTarget
		{
			get
			{
				return this._grabbingTarget;
			}
			private set
			{
				this._grabbingTarget = value;
			}
		}

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06001594 RID: 5524 RVA: 0x0004FD08 File Offset: 0x0004DF08
		// (remove) Token: 0x06001595 RID: 5525 RVA: 0x0004FD40 File Offset: 0x0004DF40
		public event Action<Hook, GoldMinerEntity> OnResolveTarget;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06001596 RID: 5526 RVA: 0x0004FD78 File Offset: 0x0004DF78
		// (remove) Token: 0x06001597 RID: 5527 RVA: 0x0004FDB0 File Offset: 0x0004DFB0
		public event Action<Hook> OnLaunch;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06001598 RID: 5528 RVA: 0x0004FDE8 File Offset: 0x0004DFE8
		// (remove) Token: 0x06001599 RID: 5529 RVA: 0x0004FE20 File Offset: 0x0004E020
		public event Action<Hook> OnBeginRetrieve;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x0600159A RID: 5530 RVA: 0x0004FE58 File Offset: 0x0004E058
		// (remove) Token: 0x0600159B RID: 5531 RVA: 0x0004FE90 File Offset: 0x0004E090
		public event Action<Hook, GoldMinerEntity> OnAttach;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x0600159C RID: 5532 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		// (remove) Token: 0x0600159D RID: 5533 RVA: 0x0004FF00 File Offset: 0x0004E100
		public event Action<Hook> OnEndRetrieve;

		// Token: 0x0600159E RID: 5534 RVA: 0x0004FF35 File Offset: 0x0004E135
		public void SetParameters(float swingFreqFactor, float emptySpeed, float strength)
		{
			this.swingFreqFactor = swingFreqFactor;
			this.emptySpeed = emptySpeed;
			this.strength = strength;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0004FF4C File Offset: 0x0004E14C
		public void Tick(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
			this.ropeLineRenderer.SetPositions(new Vector3[]
			{
				this.hookAxis.transform.position,
				this.hookHead.transform.position
			});
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0004FFAB File Offset: 0x0004E1AB
		private void UpdateHookHeadPosition()
		{
			this.hookHead.transform.localPosition = this.GetHookHeadPosition(this.RopeDistance);
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0004FFC9 File Offset: 0x0004E1C9
		private Vector3 GetHookHeadPosition(float ropeDistance)
		{
			return -Vector3.up * this.RopeDistance;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0004FFE0 File Offset: 0x0004E1E0
		private void UpdateAxis()
		{
			this.hookAxis.transform.localRotation = Quaternion.Euler(0f, 0f, this.AxisAngle);
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00050007 File Offset: 0x0004E207
		private void OnValidate()
		{
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00050018 File Offset: 0x0004E218
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case Hook.HookStatus.Idle:
				break;
			case Hook.HookStatus.Swinging:
				this.UpdateSwinging(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Launching:
				this.UpdateClaw();
				this.UpdateLaunching(deltaTime);
				return;
			case Hook.HookStatus.Attaching:
				this.UpdateAttaching(deltaTime);
				return;
			case Hook.HookStatus.Retrieving:
				this.UpdateRetreving(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Retrieved:
				this.UpdateRetrieved();
				break;
			default:
				return;
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00050083 File Offset: 0x0004E283
		public void Launch()
		{
			if (this.status != Hook.HookStatus.Swinging)
			{
				return;
			}
			this.EnterStatus(Hook.HookStatus.Launching);
			Action<Hook> onLaunch = this.OnLaunch;
			if (onLaunch == null)
			{
				return;
			}
			onLaunch(this);
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x000500A7 File Offset: 0x0004E2A7
		public void Reset()
		{
			this.ropeControl = 0f;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x000500B4 File Offset: 0x0004E2B4
		private void UpdateClaw()
		{
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			if (!this.GrabbingTarget)
			{
				this.claw.localRotation = Quaternion.Euler(0f, 0f, -180f);
				this.claw.localPosition = Vector3.zero;
				return;
			}
			Vector2 to = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			this.claw.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, to));
			this.claw.position = this.hookHead.transform.position + to.normalized * this.clawOffset;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x000501A0 File Offset: 0x0004E3A0
		private void UpdateSwinging(float deltaTime)
		{
			this.t += deltaTime * 90f * this.swingFreqFactor * 0.017453292f;
			this.axisControl = Mathf.Sin(this.t);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x000501D4 File Offset: 0x0004E3D4
		private void UpdateLaunching(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float maxDelta = num * deltaTime / (this.maxDist - this.minDist);
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 1f, maxDelta);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.RopeOutOfBound || this.ropeControl >= 1f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
			this.CheckGrab(oldWorldPos, newWorldPos);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x000502B0 File Offset: 0x0004E4B0
		private void CheckGrab(Vector3 oldWorldPos, Vector3 newWorldPos)
		{
			if (this.GrabbingTarget)
			{
				return;
			}
			Vector3 vector = newWorldPos - oldWorldPos;
			foreach (RaycastHit2D raycastHit2D in Physics2D.CircleCastAll(oldWorldPos, 8f, vector.normalized, vector.magnitude))
			{
				if (!(raycastHit2D.collider == null))
				{
					GoldMinerEntity component = raycastHit2D.collider.gameObject.GetComponent<GoldMinerEntity>();
					if (!(component == null))
					{
						this.Grab(component);
						return;
					}
				}
			}
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00050344 File Offset: 0x0004E544
		private void Grab(GoldMinerEntity target)
		{
			this.GrabbingTarget = target;
			this.EnterStatus(Hook.HookStatus.Attaching);
			this.relativePos = target.transform.position - this.hookHead.transform.position;
			this.targetDist = this.relativePos.magnitude;
			this.targetRelativeRotation = Quaternion.FromToRotation(this.relativePos, this.GrabbingTarget.transform.up);
			this.retrieveETA = this.grabAnimationTime;
			Vector2 to = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			Vector3 endValue = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, to));
			Vector3 endValue2 = this.hookHead.transform.position + to.normalized * this.clawOffset;
			this.claw.DORotate(endValue, this.retrieveETA, RotateMode.Fast).SetEase(this.grabAnimationEase);
			this.claw.DOMove(endValue2, this.retrieveETA, false).SetEase(this.grabAnimationEase);
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			this.GrabbingTarget.NotifyAttached(this);
			Action<Hook, GoldMinerEntity> onAttach = this.OnAttach;
			if (onAttach == null)
			{
				return;
			}
			onAttach(this, target);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000504B8 File Offset: 0x0004E6B8
		private void UpdateAttaching(float deltaTime)
		{
			if (this.GrabbingTarget == null)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
				return;
			}
			this.retrieveETA -= deltaTime;
			if (this.retrieveETA <= 0f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x000504F4 File Offset: 0x0004E6F4
		private void UpdateRetreving(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float maxDelta = num * deltaTime / (this.maxDist - this.minDist);
			this.maxDeltaWatch = maxDelta;
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 0f, maxDelta);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.ropeControl <= 0f)
			{
				this.ropeControl = 0f;
				this.EnterStatus(Hook.HookStatus.Retrieved);
			}
			if (this.GrabbingTarget)
			{
				Vector3 point = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
				if (point.magnitude > this.targetDist)
				{
					this.GrabbingTarget.transform.position = this.hookHead.transform.position + point.normalized * this.targetDist;
					Vector3 toDirection = this.targetRelativeRotation * point;
					this.GrabbingTarget.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
					return;
				}
			}
			else
			{
				this.CheckGrab(oldWorldPos, newWorldPos);
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00050683 File Offset: 0x0004E883
		private void UpdateRetrieved()
		{
			if (this.GrabbingTarget)
			{
				this.ResolveRetrievedObject(this.GrabbingTarget);
				this.GrabbingTarget = null;
			}
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x000506AC File Offset: 0x0004E8AC
		private void ResolveRetrievedObject(GoldMinerEntity grabingTarget)
		{
			Action<Hook, GoldMinerEntity> onResolveTarget = this.OnResolveTarget;
			if (onResolveTarget == null)
			{
				return;
			}
			onResolveTarget(this, grabingTarget);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x000506C0 File Offset: 0x0004E8C0
		private void OnExitStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Swinging:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Retrieving:
			{
				Action<Hook> onEndRetrieve = this.OnEndRetrieve;
				if (onEndRetrieve == null)
				{
					return;
				}
				onEndRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000506F2 File Offset: 0x0004E8F2
		private void EnterStatus(Hook.HookStatus status)
		{
			this.OnExitStatus(this.status);
			this.status = status;
			this.OnEnterStatus(this.status);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00050714 File Offset: 0x0004E914
		private void OnEnterStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Swinging:
				this.ropeControl = 0f;
				return;
			case Hook.HookStatus.Retrieving:
			{
				if (this.GrabbingTarget)
				{
					this.GrabbingTarget.NotifyBeginRetrieving();
				}
				Action<Hook> onBeginRetrieve = this.OnBeginRetrieve;
				if (onBeginRetrieve == null)
				{
					return;
				}
				onBeginRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060015B3 RID: 5555 RVA: 0x00050775 File Offset: 0x0004E975
		internal Vector3 Direction
		{
			get
			{
				return -this.hookAxis.transform.up;
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0005078C File Offset: 0x0004E98C
		internal void ReleaseClaw()
		{
			this.GrabbingTarget = null;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00050795 File Offset: 0x0004E995
		internal void BeginSwing()
		{
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x04000FEA RID: 4074
		public float emptySpeed = 1000f;

		// Token: 0x04000FEB RID: 4075
		public float strength;

		// Token: 0x04000FEC RID: 4076
		public float swingFreqFactor = 1f;

		// Token: 0x04000FED RID: 4077
		[SerializeField]
		private Transform hookAxis;

		// Token: 0x04000FEE RID: 4078
		[SerializeField]
		private HookHead hookHead;

		// Token: 0x04000FEF RID: 4079
		[SerializeField]
		private Transform claw;

		// Token: 0x04000FF0 RID: 4080
		[SerializeField]
		private float clawOffset = 4f;

		// Token: 0x04000FF1 RID: 4081
		[SerializeField]
		private Animator clawAnimator;

		// Token: 0x04000FF2 RID: 4082
		[SerializeField]
		private LineRenderer ropeLineRenderer;

		// Token: 0x04000FF3 RID: 4083
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04000FF4 RID: 4084
		[SerializeField]
		private float grabAnimationTime = 0.5f;

		// Token: 0x04000FF5 RID: 4085
		[SerializeField]
		private Ease grabAnimationEase = Ease.OutBounce;

		// Token: 0x04000FF6 RID: 4086
		[SerializeField]
		private float maxAngle;

		// Token: 0x04000FF7 RID: 4087
		[SerializeField]
		private float minDist;

		// Token: 0x04000FF8 RID: 4088
		[SerializeField]
		private float maxDist;

		// Token: 0x04000FF9 RID: 4089
		[Range(0f, 1f)]
		private float ropeControl;

		// Token: 0x04000FFA RID: 4090
		[Range(-1f, 1f)]
		private float axisControl;

		// Token: 0x04000FFB RID: 4091
		private Hook.HookStatus status;

		// Token: 0x04000FFC RID: 4092
		private float t;

		// Token: 0x04000FFD RID: 4093
		private GoldMinerEntity _grabbingTarget;

		// Token: 0x04000FFE RID: 4094
		private Vector2 relativePos;

		// Token: 0x04000FFF RID: 4095
		private Quaternion targetRelativeRotation;

		// Token: 0x04001000 RID: 4096
		private float targetDist;

		// Token: 0x04001001 RID: 4097
		private float retrieveETA;

		// Token: 0x04001007 RID: 4103
		public float forceModification;

		// Token: 0x04001008 RID: 4104
		private float maxDeltaWatch;

		// Token: 0x0200056E RID: 1390
		public enum HookStatus
		{
			// Token: 0x04001F65 RID: 8037
			Idle,
			// Token: 0x04001F66 RID: 8038
			Swinging,
			// Token: 0x04001F67 RID: 8039
			Launching,
			// Token: 0x04001F68 RID: 8040
			Attaching,
			// Token: 0x04001F69 RID: 8041
			Retrieving,
			// Token: 0x04001F6A RID: 8042
			Retrieved
		}
	}
}

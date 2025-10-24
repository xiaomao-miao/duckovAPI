using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D5 RID: 725
	public class FPSGun : MiniGameBehaviour
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x0005357C File Offset: 0x0005177C
		public float ScatterAngle
		{
			get
			{
				return Mathf.Lerp(this.minScatterAngle, this.maxScatterAngle, this.scatterStatus);
			}
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00053598 File Offset: 0x00051798
		private void Fire()
		{
			this.coolDown = 1f / this.fireRate;
			this.DoCast();
			this.muzzleFlash.Play();
			this.DoFireAnimation();
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 1f, this.scatterIncrementPerShot);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x000535EC File Offset: 0x000517EC
		private void DoFireAnimation()
		{
			this.graphicsTransform.DOKill(true);
			this.graphicsTransform.localPosition = Vector3.zero;
			this.graphicsTransform.localRotation = Quaternion.identity;
			this.graphicsTransform.DOPunchPosition(Vector3.back * 0.2f, 0.2f, 10, 1f, false);
			this.graphicsTransform.DOShakeRotation(0.5f, -Vector3.right * 10f, 10, 90f, true);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x0005367C File Offset: 0x0005187C
		private void DoCast()
		{
			Ray ray = this.mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.ScatterAngle / 2f;
			Vector3 vector2 = Quaternion.Euler(vector.y, vector.x, 0f) * Vector3.forward;
			Vector3 direction = this.mainCamera.transform.localToWorldMatrix.MultiplyVector(vector2);
			ray.direction = direction;
			RaycastHit castInfo;
			Physics.Raycast(ray, out castInfo, 100f, this.castLayers);
			this.HandleBulletTracer(castInfo);
			if (castInfo.collider == null)
			{
				return;
			}
			FPSDamageInfo fpsdamageInfo = new FPSDamageInfo
			{
				source = this,
				amount = 1f,
				point = castInfo.point,
				normal = castInfo.normal
			};
			FPSDamageReceiver component = castInfo.collider.GetComponent<FPSDamageReceiver>();
			if (component)
			{
				component.CastDamage(fpsdamageInfo);
				return;
			}
			this.HandleNormalHit(fpsdamageInfo);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x0005379C File Offset: 0x0005199C
		private void HandleBulletTracer(RaycastHit castInfo)
		{
			if (this.bulletTracer == null)
			{
				return;
			}
			if (!true)
			{
				return;
			}
			Vector3 position = this.muzzle.transform.position;
			Vector3 vector = this.muzzle.transform.forward;
			if (castInfo.collider != null)
			{
				vector = castInfo.point - position;
				if ((castInfo.point - position).magnitude < 5f)
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -vector);
					this.bulletTracer.transform.position = castInfo.point;
				}
				else
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
					this.bulletTracer.transform.position = this.muzzle.position;
				}
			}
			else
			{
				this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
				this.bulletTracer.transform.position = this.muzzle.position;
			}
			this.bulletTracer.Emit(1);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000538CD File Offset: 0x00051ACD
		private void HandleNormalHit(FPSDamageInfo info)
		{
			FXPool.Play(this.normalHitFXPrefab, info.point, Quaternion.FromToRotation(Vector3.forward, info.normal));
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x000538F1 File Offset: 0x00051AF1
		internal void SetTrigger(bool value)
		{
			this.trigger = value;
			if (value)
			{
				this.justPressedTrigger = true;
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00053904 File Offset: 0x00051B04
		internal void Setup(Camera mainCamera, Transform gunParent)
		{
			base.transform.SetParent(gunParent, false);
			base.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			this.mainCamera = mainCamera;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00053930 File Offset: 0x00051B30
		protected override void OnUpdate(float deltaTime)
		{
			if (this.coolDown > 0f)
			{
				this.coolDown -= deltaTime;
				this.coolDown = Mathf.Max(0f, this.coolDown);
			}
			if (this.coolDown <= 0f && this.trigger && (this.auto || this.justPressedTrigger))
			{
				this.Fire();
			}
			this.justPressedTrigger = false;
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 0f, this.scatterDecayRate * deltaTime);
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x000539C5 File Offset: 0x00051BC5
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x040010A5 RID: 4261
		[SerializeField]
		private float fireRate = 1f;

		// Token: 0x040010A6 RID: 4262
		[SerializeField]
		private bool auto;

		// Token: 0x040010A7 RID: 4263
		[SerializeField]
		private Transform muzzle;

		// Token: 0x040010A8 RID: 4264
		[SerializeField]
		private ParticleSystem muzzleFlash;

		// Token: 0x040010A9 RID: 4265
		[SerializeField]
		private ParticleSystem bulletTracer;

		// Token: 0x040010AA RID: 4266
		[SerializeField]
		private LayerMask castLayers = -1;

		// Token: 0x040010AB RID: 4267
		[SerializeField]
		private ParticleSystem normalHitFXPrefab;

		// Token: 0x040010AC RID: 4268
		[SerializeField]
		private float minScatterAngle;

		// Token: 0x040010AD RID: 4269
		[SerializeField]
		private float maxScatterAngle;

		// Token: 0x040010AE RID: 4270
		[SerializeField]
		private float scatterIncrementPerShot;

		// Token: 0x040010AF RID: 4271
		[SerializeField]
		private float scatterDecayRate;

		// Token: 0x040010B0 RID: 4272
		[SerializeField]
		private Transform graphicsTransform;

		// Token: 0x040010B1 RID: 4273
		[SerializeField]
		private FPSGun.Pose idlePose;

		// Token: 0x040010B2 RID: 4274
		[SerializeField]
		private FPSGun.Pose recoilPose;

		// Token: 0x040010B3 RID: 4275
		private float scatterStatus;

		// Token: 0x040010B4 RID: 4276
		private float coolDown;

		// Token: 0x040010B5 RID: 4277
		private Camera mainCamera;

		// Token: 0x040010B6 RID: 4278
		private bool trigger;

		// Token: 0x040010B7 RID: 4279
		private bool justPressedTrigger;

		// Token: 0x02000571 RID: 1393
		[Serializable]
		public struct Pose
		{
			// Token: 0x0600284C RID: 10316 RVA: 0x00094B24 File Offset: 0x00092D24
			public static FPSGun.Pose Extraterpolate(FPSGun.Pose poseA, FPSGun.Pose poseB, float t)
			{
				return new FPSGun.Pose
				{
					localPosition = Vector3.LerpUnclamped(poseA.localPosition, poseB.localPosition, t),
					localRotation = Quaternion.LerpUnclamped(poseA.localRotation, poseB.localRotation, t)
				};
			}

			// Token: 0x0600284D RID: 10317 RVA: 0x00094B6C File Offset: 0x00092D6C
			public Pose(Transform fromTransform)
			{
				this.localPosition = fromTransform.localPosition;
				this.localRotation = fromTransform.localRotation;
			}

			// Token: 0x04001F71 RID: 8049
			[SerializeField]
			private Vector3 localPosition;

			// Token: 0x04001F72 RID: 8050
			[SerializeField]
			private Quaternion localRotation;
		}
	}
}

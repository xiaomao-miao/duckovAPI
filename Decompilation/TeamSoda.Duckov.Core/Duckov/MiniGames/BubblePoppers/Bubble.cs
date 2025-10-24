using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002D9 RID: 729
	public class Bubble : MiniGameBehaviour
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x00053E31 File Offset: 0x00052031
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x00053E39 File Offset: 0x00052039
		public BubblePopper Master { get; private set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x00053E42 File Offset: 0x00052042
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x00053E4A File Offset: 0x0005204A
		public int ColorIndex
		{
			get
			{
				return this.colorIndex;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x00053E52 File Offset: 0x00052052
		public Color DisplayColor
		{
			get
			{
				if (this.Master == null)
				{
					return Color.white;
				}
				return this.Master.GetDisplayColor(this.ColorIndex);
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00053E79 File Offset: 0x00052079
		// (set) Token: 0x060016F9 RID: 5881 RVA: 0x00053E81 File Offset: 0x00052081
		public Vector2Int Coord { get; internal set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00053E8A File Offset: 0x0005208A
		// (set) Token: 0x060016FB RID: 5883 RVA: 0x00053E92 File Offset: 0x00052092
		public Vector2 MoveDirection { get; internal set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x00053E9B File Offset: 0x0005209B
		// (set) Token: 0x060016FD RID: 5885 RVA: 0x00053EA3 File Offset: 0x000520A3
		public Vector2 Velocity { get; internal set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x00053EAC File Offset: 0x000520AC
		// (set) Token: 0x060016FF RID: 5887 RVA: 0x00053EB4 File Offset: 0x000520B4
		public Bubble.Status status { get; private set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x00053EBD File Offset: 0x000520BD
		// (set) Token: 0x06001701 RID: 5889 RVA: 0x00053ECF File Offset: 0x000520CF
		private Vector2 gPos
		{
			get
			{
				return this.graphicsRoot.localPosition;
			}
			set
			{
				this.graphicsRoot.localPosition = value;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x00053EE4 File Offset: 0x000520E4
		private Vector2 gForce
		{
			get
			{
				return (new Vector2(Mathf.PerlinNoise(7.3f, Time.time * this.vibrationFrequency) * 2f - 1f, Mathf.PerlinNoise(0.3f, Time.time * this.vibrationFrequency) * 2f - 1f) * this.vibrationAmp - this.gPos) * this.gSpring;
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00053F5B File Offset: 0x0005215B
		internal void Setup(BubblePopper master, int colorIndex)
		{
			this.Master = master;
			this.colorIndex = colorIndex;
			this.image.color = this.DisplayColor;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00053F7C File Offset: 0x0005217C
		internal void Launch(Vector2 direction)
		{
			this.MoveDirection = direction;
			this.status = Bubble.Status.Moving;
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00053F8C File Offset: 0x0005218C
		internal void NotifyExplode(Vector2Int origin)
		{
			this.status = Bubble.Status.Explode;
			Vector2Int v = this.Coord - origin;
			float magnitude = v.magnitude;
			this.explodeETA = magnitude * 0.025f;
			this.Impact(v.normalized * 5f);
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00053FE0 File Offset: 0x000521E0
		internal void NotifyAttached(Vector2Int coord)
		{
			Vector2 v = this.Master.Layout.CoordToLocalPosition(coord);
			base.transform.position = this.Master.Layout.transform.localToWorldMatrix.MultiplyPoint(v);
			this.status = Bubble.Status.Attached;
			this.Coord = coord;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0005403B File Offset: 0x0005223B
		public void NotifyDetached()
		{
			this.status = Bubble.Status.Detached;
			this.Velocity = Vector2.zero;
			this.explodeCountDown = this.explodeAfterDetachedFor;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0005405B File Offset: 0x0005225B
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.UpdateGraphics(deltaTime);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0005406B File Offset: 0x0005226B
		private void UpdateLogic(float deltaTime)
		{
			if (this.Master == null)
			{
				return;
			}
			if (this.Master.Busy)
			{
				return;
			}
			if (this.status == Bubble.Status.Moving)
			{
				this.Master.MoveBubble(this, deltaTime);
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000540A0 File Offset: 0x000522A0
		private void UpdateGraphics(float deltaTime)
		{
			if (this.status == Bubble.Status.Explode)
			{
				this.explodeETA -= deltaTime;
				if (this.explodeETA <= 0f)
				{
					FXPool.Play(this.explodeFXrefab, base.transform.position, base.transform.rotation, this.DisplayColor);
					this.Master.Release(this);
				}
			}
			if (this.status == Bubble.Status.Detached)
			{
				base.transform.localPosition += this.Velocity * deltaTime;
				this.Velocity += -Vector2.up * this.gravity;
				this.explodeCountDown -= deltaTime;
				if (this.explodeCountDown <= 0f)
				{
					this.NotifyExplode(this.Coord);
				}
			}
			this.UpdateElasticMovement(deltaTime);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0005418C File Offset: 0x0005238C
		private void UpdateElasticMovement(float deltaTime)
		{
			float num = (Vector2.Dot(this.gVelocity, this.gForce.normalized) < 0f) ? this.gDamping : 1f;
			this.gVelocity += this.gForce * deltaTime;
			this.gVelocity = Vector2.MoveTowards(this.gVelocity, Vector2.zero, num * this.gVelocity.magnitude * deltaTime);
			this.gPos += this.gVelocity;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00054220 File Offset: 0x00052420
		public void Impact(Vector2 velocity)
		{
			this.gVelocity = velocity;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00054229 File Offset: 0x00052429
		internal void Rest()
		{
			this.gPos = Vector2.zero;
			this.gVelocity = Vector2.zero;
		}

		// Token: 0x040010C7 RID: 4295
		[SerializeField]
		private float radius;

		// Token: 0x040010C8 RID: 4296
		[SerializeField]
		private int colorIndex;

		// Token: 0x040010C9 RID: 4297
		[SerializeField]
		private float gravity;

		// Token: 0x040010CA RID: 4298
		[SerializeField]
		private float explodeAfterDetachedFor = 1f;

		// Token: 0x040010CB RID: 4299
		[SerializeField]
		private ParticleSystem explodeFXrefab;

		// Token: 0x040010CC RID: 4300
		[SerializeField]
		private Image image;

		// Token: 0x040010CD RID: 4301
		[SerializeField]
		private RectTransform graphicsRoot;

		// Token: 0x040010CE RID: 4302
		[SerializeField]
		private float gSpring = 1f;

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private float gDamping = 10f;

		// Token: 0x040010D0 RID: 4304
		[SerializeField]
		private float vibrationFrequency = 10f;

		// Token: 0x040010D1 RID: 4305
		[SerializeField]
		private float vibrationAmp = 4f;

		// Token: 0x040010D6 RID: 4310
		private float explodeETA;

		// Token: 0x040010D7 RID: 4311
		private float explodeCountDown;

		// Token: 0x040010D8 RID: 4312
		private Vector2 gVelocity;

		// Token: 0x02000572 RID: 1394
		public enum Status
		{
			// Token: 0x04001F74 RID: 8052
			Idle,
			// Token: 0x04001F75 RID: 8053
			Moving,
			// Token: 0x04001F76 RID: 8054
			Attached,
			// Token: 0x04001F77 RID: 8055
			Detached,
			// Token: 0x04001F78 RID: 8056
			Explode
		}
	}
}

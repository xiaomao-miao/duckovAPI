using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x0200028A RID: 650
	public class SnakePartDisplay : MiniGameBehaviour
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0004DF85 File Offset: 0x0004C185
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x0004DF8D File Offset: 0x0004C18D
		public SnakeDisplay Master { get; private set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x0004DF96 File Offset: 0x0004C196
		// (set) Token: 0x06001505 RID: 5381 RVA: 0x0004DF9E File Offset: 0x0004C19E
		public SnakeForce.Part Target { get; private set; }

		// Token: 0x06001506 RID: 5382 RVA: 0x0004DFA8 File Offset: 0x0004C1A8
		internal void Setup(SnakeDisplay master, SnakeForce.Part part)
		{
			if (this.Target != null)
			{
				this.Target.OnMove -= this.OnTargetMove;
			}
			this.Master = master;
			this.Target = part;
			this.cachedCoord = this.Target.coord;
			base.transform.localPosition = this.Master.GetPosition(this.cachedCoord);
			this.Target.OnMove += this.OnTargetMove;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0004E028 File Offset: 0x0004C228
		private void OnTargetMove(SnakeForce.Part part)
		{
			if (!base.enabled)
			{
				return;
			}
			int sqrMagnitude = (this.Target.coord - this.cachedCoord).sqrMagnitude;
			this.cachedCoord = this.Target.coord;
			Vector3 position = this.Master.GetPosition(this.cachedCoord);
			this.DoMove(position);
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0004E087 File Offset: 0x0004C287
		private void DoMove(Vector3 vector3)
		{
			base.transform.localPosition = vector3;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0004E098 File Offset: 0x0004C298
		internal void Punch()
		{
			base.transform.DOKill(true);
			base.transform.localScale = Vector3.one;
			base.transform.DOPunchScale(Vector3.one * 1.1f, 0.2f, 4, 1f);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0004E0E8 File Offset: 0x0004C2E8
		internal void PunchColor(Color color)
		{
			this.image.DOKill(false);
			this.image.color = color;
			this.image.DOColor(Color.white, 0.4f);
		}

		// Token: 0x04000F6F RID: 3951
		[SerializeField]
		private Image image;

		// Token: 0x04000F70 RID: 3952
		private Vector2Int cachedCoord;
	}
}

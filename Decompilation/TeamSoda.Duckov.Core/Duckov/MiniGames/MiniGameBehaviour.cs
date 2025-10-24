using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000283 RID: 643
	public class MiniGameBehaviour : MonoBehaviour
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0004C2C8 File Offset: 0x0004A4C8
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0004C2D0 File Offset: 0x0004A4D0
		public void SetGame(MiniGame game = null)
		{
			if (game == null)
			{
				this.game = base.GetComponentInParent<MiniGame>();
				return;
			}
			this.game = game;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004C2EF File Offset: 0x0004A4EF
		private void OnUpdateLogic(MiniGame game, float deltaTime)
		{
			if (this == null)
			{
				return;
			}
			if (!base.enabled)
			{
				return;
			}
			if (game == null)
			{
				return;
			}
			if (game != this.game)
			{
				return;
			}
			this.OnUpdate(deltaTime);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0004C324 File Offset: 0x0004A524
		protected virtual void OnEnable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Combine(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0004C346 File Offset: 0x0004A546
		protected virtual void OnDisable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004C368 File Offset: 0x0004A568
		private void OnDestroy()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0004C38A File Offset: 0x0004A58A
		protected virtual void Start()
		{
			if (this.game == null)
			{
				this.SetGame(null);
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004C3A1 File Offset: 0x0004A5A1
		protected virtual void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x04000F0F RID: 3855
		[SerializeField]
		private MiniGame game;
	}
}

using System;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003BE RID: 958
	public class PlayerStatsView : View
	{
		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x0007A287 File Offset: 0x00078487
		public static PlayerStatsView Instance
		{
			get
			{
				return View.GetViewInstance<PlayerStatsView>();
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0007A28E File Offset: 0x0007848E
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x0007A296 File Offset: 0x00078496
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x0007A2A9 File Offset: 0x000784A9
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0007A2BC File Offset: 0x000784BC
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x0007A2C4 File Offset: 0x000784C4
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0007A2CC File Offset: 0x000784CC
		private void RegisterEvents()
		{
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0007A2CE File Offset: 0x000784CE
		private void UnregisterEvents()
		{
		}

		// Token: 0x040017B6 RID: 6070
		[SerializeField]
		private FadeGroup fadeGroup;
	}
}

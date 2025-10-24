using System;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000287 RID: 647
	public class GamingConsoleGraphics : MonoBehaviour
	{
		// Token: 0x060014BF RID: 5311 RVA: 0x0004CD60 File Offset: 0x0004AF60
		private void Awake()
		{
			this.master.onContentChanged += this.OnContentChanged;
			this.master.OnAfterAnimateIn += this.OnAfterAnimateIn;
			this.master.OnBeforeAnimateOut += this.OnBeforeAnimateOut;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004CDB2 File Offset: 0x0004AFB2
		private void Start()
		{
			this.dirty = true;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004CDBC File Offset: 0x0004AFBC
		private void OnContentChanged(GamingConsole console)
		{
			if (console.Monitor != this._cachedMonitor)
			{
				this.OnMonitorChanged();
			}
			if (console.Console != this._cachedConsole)
			{
				this.OnConsoleChanged();
			}
			if (console.Cartridge != this._cachedCartridge)
			{
				this.OnCatridgeChanged();
			}
			this.dirty = true;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004CE1B File Offset: 0x0004B01B
		private void Update()
		{
			if (this.dirty)
			{
				this.RefreshDisplays();
				this.dirty = false;
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004CE34 File Offset: 0x0004B034
		private void RefreshDisplays()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this._cachedMonitor = this.master.Monitor;
			this._cachedConsole = this.master.Console;
			this._cachedCartridge = this.master.Cartridge;
			if (this.monitorGraphic)
			{
				UnityEngine.Object.Destroy(this.monitorGraphic.gameObject);
			}
			if (this.consoleGraphic)
			{
				UnityEngine.Object.Destroy(this.consoleGraphic.gameObject);
			}
			if (this._cachedMonitor && !this._cachedMonitor.IsBeingDestroyed)
			{
				this.monitorGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedMonitor, this.monitorRoot);
			}
			if (this._cachedConsole && !this._cachedConsole.IsBeingDestroyed)
			{
				this.consoleGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedConsole, this.consoleRoot);
				if (this.consoleGraphic != null)
				{
					this.pickupAnimation = this.consoleGraphic.GetComponent<ControllerPickupAnimation>();
					this.controllerAnimator = this.consoleGraphic.GetComponentInChildren<ControllerAnimator>();
				}
				else
				{
					this.pickupAnimation = null;
					this.controllerAnimator = null;
				}
				if (this.controllerAnimator != null)
				{
					this.controllerAnimator.SetConsole(this.master);
				}
			}
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004CF7B File Offset: 0x0004B17B
		private void OnCatridgeChanged()
		{
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004CF7D File Offset: 0x0004B17D
		private void OnConsoleChanged()
		{
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004CF7F File Offset: 0x0004B17F
		private void OnMonitorChanged()
		{
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004CF81 File Offset: 0x0004B181
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004CF8A File Offset: 0x0004B18A
		private void OnBeforeAnimateOut(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PutDown().Forget();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004CFAB File Offset: 0x0004B1AB
		private void OnAfterAnimateIn(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PickUp(this.playingControllerPosition).Forget();
		}

		// Token: 0x04000F31 RID: 3889
		[SerializeField]
		private GamingConsole master;

		// Token: 0x04000F32 RID: 3890
		[SerializeField]
		private Transform monitorRoot;

		// Token: 0x04000F33 RID: 3891
		[SerializeField]
		private Transform consoleRoot;

		// Token: 0x04000F34 RID: 3892
		[SerializeField]
		private Transform playingControllerPosition;

		// Token: 0x04000F35 RID: 3893
		private Transform cartridgeRoot;

		// Token: 0x04000F36 RID: 3894
		private Item _cachedMonitor;

		// Token: 0x04000F37 RID: 3895
		private Item _cachedConsole;

		// Token: 0x04000F38 RID: 3896
		private Item _cachedCartridge;

		// Token: 0x04000F39 RID: 3897
		private ItemGraphicInfo monitorGraphic;

		// Token: 0x04000F3A RID: 3898
		private ItemGraphicInfo consoleGraphic;

		// Token: 0x04000F3B RID: 3899
		private ControllerPickupAnimation pickupAnimation;

		// Token: 0x04000F3C RID: 3900
		private ControllerAnimator controllerAnimator;

		// Token: 0x04000F3D RID: 3901
		private bool dirty;

		// Token: 0x04000F3E RID: 3902
		private bool isBeingDestroyed;
	}
}

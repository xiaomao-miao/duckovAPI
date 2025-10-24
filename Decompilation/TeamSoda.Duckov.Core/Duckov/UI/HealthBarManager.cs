using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A4 RID: 932
	public class HealthBarManager : MonoBehaviour
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x00074646 File Offset: 0x00072846
		public static HealthBarManager Instance
		{
			get
			{
				return HealthBarManager._instance;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x00074650 File Offset: 0x00072850
		private PrefabPool<HealthBar> PrefabPool
		{
			get
			{
				if (this._prefabPool == null)
				{
					this._prefabPool = new PrefabPool<HealthBar>(this.healthBarPrefab, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this._prefabPool;
			}
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0007468E File Offset: 0x0007288E
		private void Awake()
		{
			if (HealthBarManager._instance == null)
			{
				HealthBarManager._instance = this;
			}
			this.UnregisterStaticEvents();
			this.RegisterStaticEvents();
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000746AF File Offset: 0x000728AF
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000746B7 File Offset: 0x000728B7
		private void RegisterStaticEvents()
		{
			Health.OnRequestHealthBar += this.Health_OnRequestHealthBar;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000746CA File Offset: 0x000728CA
		private void UnregisterStaticEvents()
		{
			Health.OnRequestHealthBar -= this.Health_OnRequestHealthBar;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000746E0 File Offset: 0x000728E0
		private HealthBar GetActiveHealthBar(Health health)
		{
			if (health == null)
			{
				return null;
			}
			return this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00074728 File Offset: 0x00072928
		private HealthBar CreateHealthBarFor(Health health, DamageInfo? damage = null)
		{
			if (health == null)
			{
				return null;
			}
			if (this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health))
			{
				Debug.Log("Health bar for " + health.name + " already exists");
				return null;
			}
			HealthBar newBar = this.PrefabPool.Get(null);
			newBar.Setup(health, damage, delegate
			{
				this.PrefabPool.Release(newBar);
			});
			return newBar;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000747D4 File Offset: 0x000729D4
		private void Health_OnRequestHealthBar(Health health)
		{
			HealthBar activeHealthBar = this.GetActiveHealthBar(health);
			if (activeHealthBar != null)
			{
				activeHealthBar.RefreshOffset();
				return;
			}
			this.CreateHealthBarFor(health, null);
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x0007480A File Offset: 0x00072A0A
		public static void RequestHealthBar(Health health, DamageInfo? damage = null)
		{
			if (HealthBarManager.Instance == null)
			{
				return;
			}
			HealthBarManager.Instance.CreateHealthBarFor(health, damage);
		}

		// Token: 0x040016A2 RID: 5794
		private static HealthBarManager _instance;

		// Token: 0x040016A3 RID: 5795
		[SerializeField]
		public HealthBar healthBarPrefab;

		// Token: 0x040016A4 RID: 5796
		private PrefabPool<HealthBar> _prefabPool;
	}
}

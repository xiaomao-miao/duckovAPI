using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Buffs
{
	// Token: 0x020003FC RID: 1020
	public class Buff : MonoBehaviour
	{
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x0007FD10 File Offset: 0x0007DF10
		public Buff.BuffExclusiveTags ExclusiveTag
		{
			get
			{
				return this.exclusiveTag;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x0007FD18 File Offset: 0x0007DF18
		public int ExclusiveTagPriority
		{
			get
			{
				return this.exclusiveTagPriority;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x0007FD20 File Offset: 0x0007DF20
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x0007FD28 File Offset: 0x0007DF28
		public CharacterMainControl Character
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				return characterBuffManager.Master;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x0007FD3B File Offset: 0x0007DF3B
		private Item CharacterItem
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				CharacterMainControl characterMainControl = characterBuffManager.Master;
				if (characterMainControl == null)
				{
					return null;
				}
				return characterMainControl.CharacterItem;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x0007FD59 File Offset: 0x0007DF59
		// (set) Token: 0x06002503 RID: 9475 RVA: 0x0007FD61 File Offset: 0x0007DF61
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0007FD6A File Offset: 0x0007DF6A
		// (set) Token: 0x06002505 RID: 9477 RVA: 0x0007FD72 File Offset: 0x0007DF72
		public int CurrentLayers
		{
			get
			{
				return this.currentLayers;
			}
			set
			{
				this.currentLayers = value;
				Action onLayerChangedEvent = this.OnLayerChangedEvent;
				if (onLayerChangedEvent == null)
				{
					return;
				}
				onLayerChangedEvent();
			}
		}

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06002506 RID: 9478 RVA: 0x0007FD8C File Offset: 0x0007DF8C
		// (remove) Token: 0x06002507 RID: 9479 RVA: 0x0007FDC4 File Offset: 0x0007DFC4
		public event Action OnLayerChangedEvent;

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x0007FDF9 File Offset: 0x0007DFF9
		public int MaxLayers
		{
			get
			{
				return this.maxLayers;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x0007FE01 File Offset: 0x0007E001
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x0007FE0E File Offset: 0x0007E00E
		public string DisplayNameKey
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x0007FE16 File Offset: 0x0007E016
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x0007FE23 File Offset: 0x0007E023
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0007FE2B File Offset: 0x0007E02B
		public bool LimitedLifeTime
		{
			get
			{
				return this.limitedLifeTime;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x0007FE33 File Offset: 0x0007E033
		public float TotalLifeTime
		{
			get
			{
				return this.totalLifeTime;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x0007FE3B File Offset: 0x0007E03B
		public float CurrentLifeTime
		{
			get
			{
				return Time.time - this.timeWhenStarted;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x0007FE49 File Offset: 0x0007E049
		public float RemainingTime
		{
			get
			{
				if (!this.limitedLifeTime)
				{
					return float.PositiveInfinity;
				}
				return this.totalLifeTime - this.CurrentLifeTime;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x0007FE66 File Offset: 0x0007E066
		public bool IsOutOfTime
		{
			get
			{
				return this.limitedLifeTime && this.CurrentLifeTime >= this.totalLifeTime;
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0007FE84 File Offset: 0x0007E084
		internal void Setup(CharacterBuffManager manager)
		{
			this.master = manager;
			this.timeWhenStarted = Time.time;
			base.transform.SetParent(this.CharacterItem.transform, false);
			if (this.buffFxInstance)
			{
				UnityEngine.Object.Destroy(this.buffFxInstance.gameObject);
			}
			if (this.buffFxPfb && manager.Master && manager.Master.characterModel)
			{
				this.buffFxInstance = UnityEngine.Object.Instantiate<GameObject>(this.buffFxPfb);
				Transform transform = manager.Master.characterModel.ArmorSocket;
				if (transform == null)
				{
					transform = manager.Master.transform;
				}
				this.buffFxInstance.transform.SetParent(transform);
				this.buffFxInstance.transform.position = transform.position;
				this.buffFxInstance.transform.localRotation = Quaternion.identity;
			}
			foreach (Effect effect in this.effects)
			{
				effect.SetItem(this.CharacterItem);
			}
			this.OnSetup();
			UnityEvent onSetupEvent = this.OnSetupEvent;
			if (onSetupEvent == null)
			{
				return;
			}
			onSetupEvent.Invoke();
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0007FFDC File Offset: 0x0007E1DC
		internal void NotifyUpdate()
		{
			this.OnUpdate();
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x0007FFE4 File Offset: 0x0007E1E4
		internal void NotifyOutOfTime()
		{
			this.OnNotifiedOutOfTime();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x0007FFF7 File Offset: 0x0007E1F7
		internal virtual void NotifyIncomingBuffWithSameID(Buff incomingPrefab)
		{
			this.timeWhenStarted = Time.time;
			if (this.CurrentLayers < this.maxLayers)
			{
				this.CurrentLayers += incomingPrefab.CurrentLayers;
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00080025 File Offset: 0x0007E225
		protected virtual void OnSetup()
		{
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00080027 File Offset: 0x0007E227
		protected virtual void OnUpdate()
		{
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00080029 File Offset: 0x0007E229
		protected virtual void OnNotifiedOutOfTime()
		{
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0008002B File Offset: 0x0007E22B
		private void OnDestroy()
		{
			if (this.buffFxInstance)
			{
				UnityEngine.Object.Destroy(this.buffFxInstance.gameObject);
			}
		}

		// Token: 0x04001936 RID: 6454
		[SerializeField]
		private int id;

		// Token: 0x04001937 RID: 6455
		[SerializeField]
		private int maxLayers = 1;

		// Token: 0x04001938 RID: 6456
		[SerializeField]
		private Buff.BuffExclusiveTags exclusiveTag;

		// Token: 0x04001939 RID: 6457
		[Tooltip("优先级高的代替优先级低的。同优先级，选剩余时间长的。如果一方不限制时长，选后来的")]
		[SerializeField]
		private int exclusiveTagPriority;

		// Token: 0x0400193A RID: 6458
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string displayName;

		// Token: 0x0400193B RID: 6459
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string description;

		// Token: 0x0400193C RID: 6460
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private bool limitedLifeTime;

		// Token: 0x0400193E RID: 6462
		[SerializeField]
		private float totalLifeTime;

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private List<Effect> effects = new List<Effect>();

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private bool hide;

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private int currentLayers = 1;

		// Token: 0x04001942 RID: 6466
		private CharacterBuffManager master;

		// Token: 0x04001943 RID: 6467
		public UnityEvent OnSetupEvent;

		// Token: 0x04001945 RID: 6469
		[SerializeField]
		private GameObject buffFxPfb;

		// Token: 0x04001946 RID: 6470
		private GameObject buffFxInstance;

		// Token: 0x04001947 RID: 6471
		[HideInInspector]
		public CharacterMainControl fromWho;

		// Token: 0x04001948 RID: 6472
		public int fromWeaponID;

		// Token: 0x04001949 RID: 6473
		private float timeWhenStarted;

		// Token: 0x02000667 RID: 1639
		public enum BuffExclusiveTags
		{
			// Token: 0x04002312 RID: 8978
			NotExclusive,
			// Token: 0x04002313 RID: 8979
			Bleeding,
			// Token: 0x04002314 RID: 8980
			Starve,
			// Token: 0x04002315 RID: 8981
			Thirsty,
			// Token: 0x04002316 RID: 8982
			Weight,
			// Token: 0x04002317 RID: 8983
			Poison,
			// Token: 0x04002318 RID: 8984
			Pain,
			// Token: 0x04002319 RID: 8985
			Electric,
			// Token: 0x0400231A RID: 8986
			Burning,
			// Token: 0x0400231B RID: 8987
			Space,
			// Token: 0x0400231C RID: 8988
			StormProtection,
			// Token: 0x0400231D RID: 8989
			Nauseous,
			// Token: 0x0400231E RID: 8990
			Stun
		}
	}
}

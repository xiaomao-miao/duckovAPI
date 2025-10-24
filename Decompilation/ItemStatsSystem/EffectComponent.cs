using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000F RID: 15
	public class EffectComponent : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000031BA File Offset: 0x000013BA
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000031C2 File Offset: 0x000013C2
		public Effect Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000031CB File Offset: 0x000013CB
		public virtual string DisplayName
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000031D8 File Offset: 0x000013D8
		internal Color LabelColor
		{
			get
			{
				if (!base.enabled)
				{
					return Color.gray;
				}
				return this.ActiveLabelColor;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000031EE File Offset: 0x000013EE
		protected virtual Color ActiveLabelColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000031F8 File Offset: 0x000013F8
		public virtual void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddError("需要一个Master。").WithFix("将Master设为本物体上的Effect。", delegate()
				{
					this.master = base.GetComponent<Effect>();
				}, true);
				return;
			}
			if (this.master.gameObject != base.gameObject)
			{
				result.AddError("Master必须处于同一Game Object上。").WithFix("将Master设为本物体上的Effect。", delegate()
				{
					this.master = base.GetComponent<Effect>();
				}, true);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003272 File Offset: 0x00001472
		protected virtual void Awake()
		{
			if (this.Master == null)
			{
				this.master = base.GetComponent<Effect>();
			}
			if (this.Master == null)
			{
				Debug.LogWarning("No Effect component on current game object.");
				return;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000032A7 File Offset: 0x000014A7
		private void Start()
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000032A9 File Offset: 0x000014A9
		private void RemoveThisComponent()
		{
		}

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private Effect master;

		// Token: 0x0400002B RID: 43
		private Empty label;
	}
}

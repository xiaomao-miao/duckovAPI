using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000011 RID: 17
	[RequireComponent(typeof(Effect))]
	public class EffectTrigger : EffectComponent, ISelfValidator
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000033A5 File Offset: 0x000015A5
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectTrigger;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000033B1 File Offset: 0x000015B1
		public override string DisplayName
		{
			get
			{
				return "未命名触发器(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000033CD File Offset: 0x000015CD
		protected void Trigger(bool positive = true)
		{
			base.Master.Trigger(new EffectTriggerEventContext(this, positive));
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000033E1 File Offset: 0x000015E1
		protected void TriggerPositive()
		{
			this.Trigger(true);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000033EA File Offset: 0x000015EA
		protected void TriggerNegative()
		{
			this.Trigger(false);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000033F4 File Offset: 0x000015F4
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Triggers.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000344C File Offset: 0x0000164C
		protected virtual void OnDisable()
		{
			this.Trigger(false);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003455 File Offset: 0x00001655
		protected virtual void OnMasterSetTargetItem(Effect effect, Item item)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003457 File Offset: 0x00001657
		internal void NotifySetItem(Effect effect, Item targetItem)
		{
			this.OnMasterSetTargetItem(effect, targetItem);
		}
	}
}

using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000E RID: 14
	[RequireComponent(typeof(Effect))]
	public class EffectAction : EffectComponent, ISelfValidator
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000030EA File Offset: 0x000012EA
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectAction;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000030F6 File Offset: 0x000012F6
		public override string DisplayName
		{
			get
			{
				return "未命名动作(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003112 File Offset: 0x00001312
		internal void NotifyTriggered(EffectTriggerEventContext context)
		{
			if (!base.enabled)
			{
				return;
			}
			this.OnTriggered(context.positive);
			if (context.positive)
			{
				this.OnTriggeredPositive();
				return;
			}
			this.OnTriggeredNegative();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000313E File Offset: 0x0000133E
		protected virtual void OnTriggered(bool positive)
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003140 File Offset: 0x00001340
		protected virtual void OnTriggeredPositive()
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003142 File Offset: 0x00001342
		protected virtual void OnTriggeredNegative()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003144 File Offset: 0x00001344
		private void OnDisable()
		{
			this.OnTriggeredNegative();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000314C File Offset: 0x0000134C
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Actions.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}
	}
}

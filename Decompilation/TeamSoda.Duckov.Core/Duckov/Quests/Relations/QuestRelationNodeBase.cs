using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.Quests.Relations
{
	// Token: 0x0200035E RID: 862
	public class QuestRelationNodeBase : Node
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x0006A852 File Offset: 0x00068A52
		public override int maxInConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x0006A856 File Offset: 0x00068A56
		public override int maxOutConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0006A85A File Offset: 0x00068A5A
		public override Type outConnectionType
		{
			get
			{
				return typeof(QuestRelationConnection);
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0006A866 File Offset: 0x00068A66
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0006A869 File Offset: 0x00068A69
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0006A86C File Offset: 0x00068A6C
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0006A86F File Offset: 0x00068A6F
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}
	}
}

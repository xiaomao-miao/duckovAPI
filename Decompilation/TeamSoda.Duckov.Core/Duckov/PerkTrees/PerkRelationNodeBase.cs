using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000255 RID: 597
	public class PerkRelationNodeBase : Node
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x00046348 File Offset: 0x00044548
		public override int maxInConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0004634C File Offset: 0x0004454C
		public override int maxOutConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x00046350 File Offset: 0x00044550
		public override Type outConnectionType
		{
			get
			{
				return typeof(PerkRelationConnection);
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0004635C File Offset: 0x0004455C
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0004635F File Offset: 0x0004455F
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x00046362 File Offset: 0x00044562
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00046365 File Offset: 0x00044565
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}
	}
}

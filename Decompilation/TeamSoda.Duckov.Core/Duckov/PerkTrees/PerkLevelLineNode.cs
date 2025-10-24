using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000251 RID: 593
	public class PerkLevelLineNode : PerkRelationNodeBase
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00046127 File Offset: 0x00044327
		public string DisplayName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0004612F File Offset: 0x0004432F
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00046132 File Offset: 0x00044332
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04000E18 RID: 3608
		public Vector2 cachedPosition;
	}
}

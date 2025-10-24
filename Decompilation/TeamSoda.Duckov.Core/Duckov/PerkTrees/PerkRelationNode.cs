using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000254 RID: 596
	public class PerkRelationNode : PerkRelationNodeBase
	{
		// Token: 0x0600129B RID: 4763 RVA: 0x0004627C File Offset: 0x0004447C
		internal void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00046288 File Offset: 0x00044488
		public override void OnDestroy()
		{
			if (this.relatedNode == null)
			{
				return;
			}
			IEnumerable<Node> enumerable = from e in base.graph.allNodes
			where (e as PerkRelationNode).relatedNode == this.relatedNode
			select e;
			if (enumerable.Count<Node>() <= 2)
			{
				foreach (Node node in enumerable)
				{
					PerkRelationNode perkRelationNode = node as PerkRelationNode;
					if (perkRelationNode != null)
					{
						perkRelationNode.isDuplicate = false;
						perkRelationNode.SetDirty();
					}
				}
			}
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00046314 File Offset: 0x00044514
		internal void NotifyIncomingStateChanged()
		{
			this.relatedNode.NotifyParentStateChanged();
		}

		// Token: 0x04000E19 RID: 3609
		public Perk relatedNode;

		// Token: 0x04000E1A RID: 3610
		public Vector2 cachedPosition;

		// Token: 0x04000E1B RID: 3611
		private bool dirty = true;

		// Token: 0x04000E1C RID: 3612
		internal bool isDuplicate;

		// Token: 0x04000E1D RID: 3613
		internal bool isInvalid;
	}
}

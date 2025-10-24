using System;
using Duckov.Utilities;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000360 RID: 864
	public class QuestRelationProxyNode : QuestRelationNodeBase
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0006A9A0 File Offset: 0x00068BA0
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0006A9A3 File Offset: 0x00068BA3
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationProxyNode._questCollection == null)
				{
					QuestRelationProxyNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationProxyNode._questCollection;
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0006A9C1 File Offset: 0x00068BC1
		private void SelectQuest()
		{
		}

		// Token: 0x0400149F RID: 5279
		private static QuestCollection _questCollection;

		// Token: 0x040014A0 RID: 5280
		public int questID;
	}
}

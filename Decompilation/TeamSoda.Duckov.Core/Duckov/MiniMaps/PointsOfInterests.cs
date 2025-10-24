using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000274 RID: 628
	public static class PointsOfInterests
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00049645 File Offset: 0x00047845
		public static ReadOnlyCollection<MonoBehaviour> Points
		{
			get
			{
				if (PointsOfInterests.points_ReadOnly == null)
				{
					PointsOfInterests.points_ReadOnly = new ReadOnlyCollection<MonoBehaviour>(PointsOfInterests.points);
				}
				return PointsOfInterests.points_ReadOnly;
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060013C5 RID: 5061 RVA: 0x00049664 File Offset: 0x00047864
		// (remove) Token: 0x060013C6 RID: 5062 RVA: 0x00049698 File Offset: 0x00047898
		public static event Action<MonoBehaviour> OnPointRegistered;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060013C7 RID: 5063 RVA: 0x000496CC File Offset: 0x000478CC
		// (remove) Token: 0x060013C8 RID: 5064 RVA: 0x00049700 File Offset: 0x00047900
		public static event Action<MonoBehaviour> OnPointUnregistered;

		// Token: 0x060013C9 RID: 5065 RVA: 0x00049733 File Offset: 0x00047933
		public static void Register(MonoBehaviour point)
		{
			PointsOfInterests.points.Add(point);
			Action<MonoBehaviour> onPointRegistered = PointsOfInterests.OnPointRegistered;
			if (onPointRegistered != null)
			{
				onPointRegistered(point);
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00049756 File Offset: 0x00047956
		public static void Unregister(MonoBehaviour point)
		{
			if (PointsOfInterests.points.Remove(point))
			{
				Action<MonoBehaviour> onPointUnregistered = PointsOfInterests.OnPointUnregistered;
				if (onPointUnregistered != null)
				{
					onPointUnregistered(point);
				}
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004977B File Offset: 0x0004797B
		private static void CleanUp()
		{
			PointsOfInterests.points.RemoveAll((MonoBehaviour e) => e == null);
		}

		// Token: 0x04000E9A RID: 3738
		private static List<MonoBehaviour> points = new List<MonoBehaviour>();

		// Token: 0x04000E9B RID: 3739
		private static ReadOnlyCollection<MonoBehaviour> points_ReadOnly;
	}
}

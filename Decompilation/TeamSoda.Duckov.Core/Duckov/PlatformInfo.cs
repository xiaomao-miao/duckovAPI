using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000239 RID: 569
	public static class PlatformInfo
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00044140 File Offset: 0x00042340
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x00044155 File Offset: 0x00042355
		public static Platform Platform
		{
			get
			{
				if (Application.isEditor)
				{
					return Platform.UnityEditor;
				}
				return GameMetaData.Instance.Platform;
			}
			set
			{
				GameMetaData.Instance.Platform = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x00044162 File Offset: 0x00042362
		// (set) Token: 0x060011AC RID: 4524 RVA: 0x00044169 File Offset: 0x00042369
		public static Func<string> GetIDFunc
		{
			get
			{
				return PlatformInfo._getIDFunc;
			}
			set
			{
				PlatformInfo._getIDFunc = value;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x00044171 File Offset: 0x00042371
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x00044178 File Offset: 0x00042378
		public static Func<string> GetDisplayNameFunc
		{
			get
			{
				return PlatformInfo._getDisplayNameFunc;
			}
			set
			{
				PlatformInfo._getDisplayNameFunc = value;
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00044180 File Offset: 0x00042380
		public static string GetID()
		{
			string text = null;
			if (PlatformInfo.GetIDFunc != null)
			{
				text = PlatformInfo.GetIDFunc();
			}
			if (text == null)
			{
				text = Environment.MachineName;
			}
			return text;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000441AB File Offset: 0x000423AB
		public static string GetDisplayName()
		{
			if (PlatformInfo.GetDisplayNameFunc != null)
			{
				return PlatformInfo.GetDisplayNameFunc();
			}
			return "UNKOWN";
		}

		// Token: 0x04000DA3 RID: 3491
		private static Func<string> _getIDFunc;

		// Token: 0x04000DA4 RID: 3492
		private static Func<string> _getDisplayNameFunc;
	}
}

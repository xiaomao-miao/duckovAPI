using System;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000007 RID: 7
	public class DuckovUtilitiesSettings : ScriptableObject
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002C93 File Offset: 0x00000E93
		private static DuckovUtilitiesSettings Instance
		{
			get
			{
				if (DuckovUtilitiesSettings._instance == null)
				{
					DuckovUtilitiesSettings._instance = DuckovUtilitiesSettings.Load();
				}
				return DuckovUtilitiesSettings._instance;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CB4 File Offset: 0x00000EB4
		private static DuckovUtilitiesSettings Load()
		{
			DuckovUtilitiesSettings duckovUtilitiesSettings = Resources.Load("DuckovUtilitiesSettings") as DuckovUtilitiesSettings;
			if (duckovUtilitiesSettings != null)
			{
				return duckovUtilitiesSettings;
			}
			return null;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CDD File Offset: 0x00000EDD
		private static void CreateAsset()
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CDF File Offset: 0x00000EDF
		private static void LoadOrCreate()
		{
			DuckovUtilitiesSettings._instance = DuckovUtilitiesSettings.Load();
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002CEB File Offset: 0x00000EEB
		public static DuckovUtilitiesSettings.ColorsData Colors
		{
			get
			{
				return DuckovUtilitiesSettings.Instance.colors;
			}
		}

		// Token: 0x04000016 RID: 22
		private static DuckovUtilitiesSettings _instance;

		// Token: 0x04000017 RID: 23
		private const string fileName = "DuckovUtilitiesSettings";

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private DuckovUtilitiesSettings.ColorsData colors;

		// Token: 0x02000017 RID: 23
		[Serializable]
		public class ColorsData
		{
			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003F16 File Offset: 0x00002116
			public Color EffectTrigger
			{
				get
				{
					return this.effectTrigger;
				}
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003F1E File Offset: 0x0000211E
			public Color EffectFilter
			{
				get
				{
					return this.effectFilter;
				}
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003F26 File Offset: 0x00002126
			public Color EffectAction
			{
				get
				{
					return this.effectAction;
				}
			}

			// Token: 0x0400003B RID: 59
			[SerializeField]
			private Color effectTrigger = Color.cyan;

			// Token: 0x0400003C RID: 60
			[SerializeField]
			private Color effectFilter = Color.yellow;

			// Token: 0x0400003D RID: 61
			[SerializeField]
			private Color effectAction = Color.green;
		}
	}
}

using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000236 RID: 566
	[CreateAssetMenu(menuName = "Settings/MetaData")]
	public class GameMetaData : ScriptableObject
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00044050 File Offset: 0x00042250
		public VersionData Version
		{
			get
			{
				if (GameMetaData.Instance == null)
				{
					return default(VersionData);
				}
				return GameMetaData.Instance.versionData.versionData;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x00044083 File Offset: 0x00042283
		public bool IsDemo
		{
			get
			{
				return this.isDemo;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x0004408B File Offset: 0x0004228B
		public bool IsTestVersion
		{
			get
			{
				return this.isTestVersion;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00044098 File Offset: 0x00042298
		public static GameMetaData Instance
		{
			get
			{
				if (GameMetaData._instance == null)
				{
					GameMetaData._instance = Resources.Load<GameMetaData>("MetaData");
				}
				return GameMetaData._instance;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x000440BB File Offset: 0x000422BB
		public static bool BloodFxOn
		{
			get
			{
				return GameMetaData.Instance.bloodFxOn;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x000440C7 File Offset: 0x000422C7
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x000440CF File Offset: 0x000422CF
		public Platform Platform
		{
			get
			{
				return this.platform;
			}
			set
			{
				this.platform = value;
			}
		}

		// Token: 0x04000D98 RID: 3480
		[SerializeField]
		private GameVersionData versionData;

		// Token: 0x04000D99 RID: 3481
		[SerializeField]
		private bool isTestVersion;

		// Token: 0x04000D9A RID: 3482
		[SerializeField]
		private bool isDemo;

		// Token: 0x04000D9B RID: 3483
		[SerializeField]
		private Platform platform;

		// Token: 0x04000D9C RID: 3484
		[SerializeField]
		private bool bloodFxOn = true;

		// Token: 0x04000D9D RID: 3485
		private static GameMetaData _instance;
	}
}

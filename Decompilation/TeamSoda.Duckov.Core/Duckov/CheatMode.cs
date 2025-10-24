using System;
using System.IO;
using Saves;

namespace Duckov
{
	// Token: 0x0200023D RID: 573
	public class CheatMode
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00044284 File Offset: 0x00042484
		// (set) Token: 0x060011BB RID: 4539 RVA: 0x0004428B File Offset: 0x0004248B
		public static bool Active
		{
			get
			{
				return CheatMode._acitive;
			}
			private set
			{
				CheatMode._acitive = value;
				Action<bool> onCheatModeStatusChanged = CheatMode.OnCheatModeStatusChanged;
				if (onCheatModeStatusChanged == null)
				{
					return;
				}
				onCheatModeStatusChanged(value);
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x060011BC RID: 4540 RVA: 0x000442A4 File Offset: 0x000424A4
		// (remove) Token: 0x060011BD RID: 4541 RVA: 0x000442D8 File Offset: 0x000424D8
		public static event Action<bool> OnCheatModeStatusChanged;

		// Token: 0x060011BE RID: 4542 RVA: 0x0004430B File Offset: 0x0004250B
		public static void Activate()
		{
			if (!CheatMode.CheatFileExists())
			{
				return;
			}
			CheatMode.Active = true;
			SavesSystem.Save<bool>("Cheated", true);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00044326 File Offset: 0x00042526
		public static void Deactivate()
		{
			CheatMode.Active = false;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x0004432E File Offset: 0x0004252E
		private bool Cheated
		{
			get
			{
				return SavesSystem.Load<bool>("Cheated");
			}
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004433A File Offset: 0x0004253A
		private static bool CheatFileExists()
		{
			return File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WWSSADADBA"));
		}

		// Token: 0x04000DB0 RID: 3504
		private static bool _acitive;
	}
}

using System;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003F2 RID: 1010
	[CreateAssetMenu(menuName = "Duckov/Ruleset")]
	public class RulesetFile : ScriptableObject
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x0007EA3A File Offset: 0x0007CC3A
		public Ruleset Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x040018D4 RID: 6356
		[SerializeField]
		private Ruleset data;
	}
}

using System;

namespace Duckov.CreditsUtility
{
	// Token: 0x020002FB RID: 763
	public struct Token
	{
		// Token: 0x060018DD RID: 6365 RVA: 0x0005A730 File Offset: 0x00058930
		public Token(TokenType type, string text = null)
		{
			this.type = type;
			this.text = text;
		}

		// Token: 0x04001214 RID: 4628
		public TokenType type;

		// Token: 0x04001215 RID: 4629
		public string text;
	}
}

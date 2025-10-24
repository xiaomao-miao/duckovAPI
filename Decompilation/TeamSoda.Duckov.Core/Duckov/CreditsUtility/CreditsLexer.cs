using System;
using System.Collections;
using System.Collections.Generic;

namespace Duckov.CreditsUtility
{
	// Token: 0x020002FC RID: 764
	public class CreditsLexer : IEnumerable<Token>, IEnumerable
	{
		// Token: 0x060018DE RID: 6366 RVA: 0x0005A740 File Offset: 0x00058940
		public CreditsLexer(string content)
		{
			this.content = content;
			this.cursor = 0;
			this.lineBegin = 0;
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0005A75D File Offset: 0x0005895D
		public void Reset()
		{
			this.cursor = 0;
			this.lineBegin = 0;
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0005A770 File Offset: 0x00058970
		private void TrimLeft()
		{
			while ((int)this.cursor < this.content.Length)
			{
				char c = this.content[(int)this.cursor];
				if (!char.IsWhiteSpace(c))
				{
					return;
				}
				if (c == '\n')
				{
					return;
				}
				this.cursor += 1;
			}
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0005A7C4 File Offset: 0x000589C4
		public Token Next()
		{
			this.TrimLeft();
			if ((int)this.cursor >= this.content.Length)
			{
				this.cursor += 1;
				return new Token(TokenType.End, null);
			}
			char c = this.content[(int)this.cursor];
			if (c == '\n')
			{
				this.cursor += 1;
				return new Token(TokenType.EmptyLine, null);
			}
			if (c == '#')
			{
				this.cursor += 1;
				int startIndex = (int)this.cursor;
				while ((int)this.cursor < this.content.Length && this.content[(int)this.cursor] != '\n')
				{
					this.cursor += 1;
				}
				this.cursor += 1;
				return new Token(TokenType.Comment, this.content.Substring(startIndex, (int)this.cursor));
			}
			if (c == '[')
			{
				this.cursor += 1;
				int num = (int)this.cursor;
				while ((int)this.cursor < this.content.Length)
				{
					if (this.content[(int)this.cursor] == ']')
					{
						string text = this.content.Substring(num, (int)this.cursor - num);
						while ((int)this.cursor < this.content.Length)
						{
							this.cursor += 1;
							if ((int)this.cursor >= this.content.Length)
							{
								break;
							}
							c = this.content[(int)this.cursor];
							if (c == '\n')
							{
								this.cursor += 1;
								break;
							}
							if (!char.IsWhiteSpace(c))
							{
								break;
							}
						}
						return new Token(TokenType.Instructor, text);
					}
					if (this.content[(int)this.cursor] == '\n')
					{
						this.cursor += 1;
						return new Token(TokenType.Invalid, this.content.Substring(num, (int)this.cursor - num));
					}
					this.cursor += 1;
				}
				return new Token(TokenType.Invalid, this.content.Substring(num - 1));
			}
			int num2 = (int)this.cursor;
			string raw;
			while ((int)this.cursor < this.content.Length)
			{
				c = this.content[(int)this.cursor];
				if (c == '\n')
				{
					raw = this.content.Substring(num2, (int)this.cursor - num2);
					this.cursor += 1;
					return new Token(TokenType.String, this.ConvertEscapes(raw));
				}
				if (c == '#')
				{
					raw = this.content.Substring(num2, (int)this.cursor - num2);
					return new Token(TokenType.String, this.ConvertEscapes(raw));
				}
				this.cursor += 1;
			}
			raw = this.content.Substring(num2, (int)this.cursor - num2);
			return new Token(TokenType.String, this.ConvertEscapes(raw));
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0005AAAF File Offset: 0x00058CAF
		private string ConvertEscapes(string raw)
		{
			return raw.Replace("\\n", "\n");
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0005AAC1 File Offset: 0x00058CC1
		public IEnumerator<Token> GetEnumerator()
		{
			while ((int)this.cursor < this.content.Length)
			{
				Token token = this.Next();
				yield return token;
			}
			yield break;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0005AAD0 File Offset: 0x00058CD0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04001216 RID: 4630
		private readonly string content;

		// Token: 0x04001217 RID: 4631
		private ushort cursor;

		// Token: 0x04001218 RID: 4632
		private ushort lineBegin;
	}
}

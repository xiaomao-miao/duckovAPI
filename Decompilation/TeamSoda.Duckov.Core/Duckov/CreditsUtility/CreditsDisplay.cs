using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x020002F9 RID: 761
	public class CreditsDisplay : MonoBehaviour
	{
		// Token: 0x060018CB RID: 6347 RVA: 0x0005A140 File Offset: 0x00058340
		private void ParseAndDisplay()
		{
			this.Reset();
			CreditsLexer creditsLexer = new CreditsLexer(this.content.text);
			this.BeginVerticalLayout(Array.Empty<string>());
			foreach (Token token in creditsLexer)
			{
				if (this.status.records.Count > 0)
				{
					Token token2 = this.status.records[this.status.records.Count - 1];
				}
				this.status.records.Add(token);
				switch (token.type)
				{
				case TokenType.Invalid:
					Debug.LogError("Invalid Token: " + token.text);
					break;
				case TokenType.End:
					goto IL_F4;
				case TokenType.String:
					this.DoText(token.text);
					break;
				case TokenType.Instructor:
					this.DoInstructor(token.text);
					break;
				case TokenType.EmptyLine:
					this.EndItem();
					break;
				}
			}
			IL_F4:
			this.EndLayout(Array.Empty<string>());
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0005A25C File Offset: 0x0005845C
		private void EndItem()
		{
			if (this.status.activeItem)
			{
				this.status.activeItem = null;
				this.EndLayout(Array.Empty<string>());
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0005A288 File Offset: 0x00058488
		private void BeginItem()
		{
			this.status.activeItem = this.BeginVerticalLayout(Array.Empty<string>());
			this.status.activeItem.SetLayoutSpacing(this.internalItemSpacing);
			this.status.activeItem.SetPreferredWidth(this.itemWidth);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0005A2D7 File Offset: 0x000584D7
		private void DoEmpty(params string[] elements)
		{
			UnityEngine.Object.Instantiate<EmptyEntry>(this.emptyPrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0005A2F0 File Offset: 0x000584F0
		private void DoInstructor(string text)
		{
			string[] array = text.Split(' ', StringSplitOptions.None);
			if (array.Length < 1)
			{
				return;
			}
			string text2 = array[0];
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 3008443898U)
			{
				if (num <= 1811125385U)
				{
					if (num != 1031692888U)
					{
						if (num != 1811125385U)
						{
							return;
						}
						if (!(text2 == "Horizontal"))
						{
							return;
						}
						this.BeginHorizontalLayout(array);
						return;
					}
					else
					{
						if (!(text2 == "color"))
						{
							return;
						}
						this.DoColor(array);
						return;
					}
				}
				else if (num != 2163944795U)
				{
					if (num != 3008443898U)
					{
						return;
					}
					if (!(text2 == "image"))
					{
						return;
					}
					this.DoImage(array);
					return;
				}
				else
				{
					if (!(text2 == "Vertical"))
					{
						return;
					}
					this.BeginVerticalLayout(array);
					return;
				}
			}
			else if (num <= 3482547786U)
			{
				if (num != 3250860581U)
				{
					if (num != 3482547786U)
					{
						return;
					}
					if (!(text2 == "End"))
					{
						return;
					}
					this.EndLayout(Array.Empty<string>());
					return;
				}
				else
				{
					if (!(text2 == "Space"))
					{
						return;
					}
					this.DoEmpty(array);
					return;
				}
			}
			else if (num != 3876335077U)
			{
				if (num != 3909890315U)
				{
					if (num != 4127999362U)
					{
						return;
					}
					if (!(text2 == "s"))
					{
						return;
					}
					this.status.s = true;
					return;
				}
				else
				{
					if (!(text2 == "l"))
					{
						return;
					}
					this.status.l = true;
					return;
				}
			}
			else
			{
				if (!(text2 == "b"))
				{
					return;
				}
				this.status.b = true;
				return;
			}
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0005A460 File Offset: 0x00058660
		private void DoImage(string[] elements)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			UnityEngine.Object.Instantiate<ImageEntry>(this.imagePrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0005A494 File Offset: 0x00058694
		private void DoColor(string[] elements)
		{
			if (elements.Length < 2)
			{
				return;
			}
			Color color;
			ColorUtility.TryParseHtmlString(elements[1], out color);
			this.status.color = color;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0005A4C0 File Offset: 0x000586C0
		private void DoText(string text)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			TextEntry textEntry = UnityEngine.Object.Instantiate<TextEntry>(this.textPrefab, this.CurrentTransform);
			int size = 30;
			if (this.status.s)
			{
				size = 20;
			}
			if (this.status.l)
			{
				size = 40;
			}
			bool b = this.status.b;
			textEntry.Setup(text, this.status.color, size, b);
			this.status.Flush();
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0005A544 File Offset: 0x00058744
		private Transform GetCurrentTransform()
		{
			if (this.status == null)
			{
				return this.rootContentTransform;
			}
			if (this.status.transforms.Count == 0)
			{
				return this.rootContentTransform;
			}
			return this.status.transforms.Peek();
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0005A57E File Offset: 0x0005877E
		private Transform CurrentTransform
		{
			get
			{
				return this.GetCurrentTransform();
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0005A586 File Offset: 0x00058786
		public void PushTransform(Transform trans)
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return;
			}
			this.status.transforms.Push(trans);
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0005A5B0 File Offset: 0x000587B0
		public Transform PopTransform()
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return null;
			}
			if (this.status.transforms.Count == 0)
			{
				Debug.LogError("Nothing to pop. Makesure to match push and pop.", this);
				return null;
			}
			return this.status.transforms.Pop();
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0005A601 File Offset: 0x00058801
		private void Awake()
		{
			if (this.setupOnAwake)
			{
				this.ParseAndDisplay();
			}
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0005A614 File Offset: 0x00058814
		private void Reset()
		{
			while (base.transform.childCount > 0)
			{
				Transform child = base.transform.GetChild(0);
				child.SetParent(null);
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
			this.status = new CreditsDisplay.GenerationStatus();
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0005A670 File Offset: 0x00058870
		private VerticalEntry BeginVerticalLayout(params string[] args)
		{
			VerticalEntry verticalEntry = UnityEngine.Object.Instantiate<VerticalEntry>(this.verticalPrefab, this.CurrentTransform);
			verticalEntry.Setup(args);
			verticalEntry.SetLayoutSpacing(this.mainSpacing);
			this.PushTransform(verticalEntry.transform);
			return verticalEntry;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0005A6AF File Offset: 0x000588AF
		private void EndLayout(params string[] args)
		{
			if (this.status.activeItem != null)
			{
				this.EndItem();
			}
			this.PopTransform();
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0005A6D4 File Offset: 0x000588D4
		private HorizontalEntry BeginHorizontalLayout(params string[] args)
		{
			HorizontalEntry horizontalEntry = UnityEngine.Object.Instantiate<HorizontalEntry>(this.horizontalPrefab, this.CurrentTransform);
			horizontalEntry.Setup(args);
			this.PushTransform(horizontalEntry.transform);
			return horizontalEntry;
		}

		// Token: 0x04001201 RID: 4609
		[SerializeField]
		private bool setupOnAwake;

		// Token: 0x04001202 RID: 4610
		[SerializeField]
		private TextAsset content;

		// Token: 0x04001203 RID: 4611
		[SerializeField]
		private Transform rootContentTransform;

		// Token: 0x04001204 RID: 4612
		[SerializeField]
		private float internalItemSpacing = 8f;

		// Token: 0x04001205 RID: 4613
		[SerializeField]
		private float mainSpacing = 16f;

		// Token: 0x04001206 RID: 4614
		[SerializeField]
		private float itemWidth = 350f;

		// Token: 0x04001207 RID: 4615
		[Header("Prefabs")]
		[SerializeField]
		private HorizontalEntry horizontalPrefab;

		// Token: 0x04001208 RID: 4616
		[SerializeField]
		private VerticalEntry verticalPrefab;

		// Token: 0x04001209 RID: 4617
		[SerializeField]
		private EmptyEntry emptyPrefab;

		// Token: 0x0400120A RID: 4618
		[SerializeField]
		private TextEntry textPrefab;

		// Token: 0x0400120B RID: 4619
		[SerializeField]
		private ImageEntry imagePrefab;

		// Token: 0x0400120C RID: 4620
		private CreditsDisplay.GenerationStatus status;

		// Token: 0x0200058C RID: 1420
		private class GenerationStatus
		{
			// Token: 0x06002878 RID: 10360 RVA: 0x0009554E File Offset: 0x0009374E
			public void Flush()
			{
				this.s = false;
				this.l = false;
				this.b = false;
				this.color = Color.white;
			}

			// Token: 0x04001FCD RID: 8141
			public List<Token> records = new List<Token>();

			// Token: 0x04001FCE RID: 8142
			public Stack<Transform> transforms = new Stack<Transform>();

			// Token: 0x04001FCF RID: 8143
			public bool s;

			// Token: 0x04001FD0 RID: 8144
			public bool l;

			// Token: 0x04001FD1 RID: 8145
			public bool b;

			// Token: 0x04001FD2 RID: 8146
			public Color color = Color.white;

			// Token: 0x04001FD3 RID: 8147
			public VerticalEntry activeItem;
		}
	}
}

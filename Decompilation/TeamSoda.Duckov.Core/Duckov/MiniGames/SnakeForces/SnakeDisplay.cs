using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x02000288 RID: 648
	public class SnakeDisplay : MiniGameBehaviour
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0004CFDC File Offset: 0x0004B1DC
		private PrefabPool<SnakePartDisplay> PartPool
		{
			get
			{
				if (this._partPool == null)
				{
					this._partPool = new PrefabPool<SnakePartDisplay>(this.partDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._partPool;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0004D018 File Offset: 0x0004B218
		private PrefabPool<Transform> FoodPool
		{
			get
			{
				if (this._foodPool == null)
				{
					this._foodPool = new PrefabPool<Transform>(this.foodDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._foodPool;
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004D054 File Offset: 0x0004B254
		private void Awake()
		{
			this.master.OnAddPart += this.OnAddPart;
			this.master.OnGameStart += this.OnGameStart;
			this.master.OnRemovePart += this.OnRemovePart;
			this.master.OnAfterTick += this.OnAfterTick;
			this.master.OnFoodEaten += this.OnFoodEaten;
			this.partDisplayTemplate.gameObject.SetActive(false);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D0E5 File Offset: 0x0004B2E5
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.HandlePunchColor();
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004D0F4 File Offset: 0x0004B2F4
		private void HandlePunchColor()
		{
			if (!this.punchingColor)
			{
				return;
			}
			if (this.punchColorIndex >= this.master.Snake.Count)
			{
				this.punchingColor = false;
				return;
			}
			SnakePartDisplay snakePartDisplay = this.PartPool.ActiveEntries.First((SnakePartDisplay e) => e.Target == this.master.Snake[this.punchColorIndex]);
			if (snakePartDisplay)
			{
				snakePartDisplay.PunchColor(Color.HSVToRGB((float)this.punchColorIndex % 12f / 12f, 1f, 1f));
			}
			this.punchColorIndex++;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D185 File Offset: 0x0004B385
		private void OnGameStart(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004D190 File Offset: 0x0004B390
		private void OnFoodEaten(SnakeForce force, Vector2Int coord)
		{
			FXPool.Play(this.eatFXPrefab, this.GetWorldPosition(coord), Quaternion.LookRotation((Vector3Int)this.master.Head.direction, Vector3.forward));
			foreach (SnakePartDisplay snakePartDisplay in this.PartPool.ActiveEntries)
			{
				snakePartDisplay.Punch();
			}
			this.StartPunchingColor();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004D21C File Offset: 0x0004B41C
		private void StartPunchingColor()
		{
			this.punchingColor = true;
			this.punchColorIndex = 0;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004D22C File Offset: 0x0004B42C
		private void OnAfterTick(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004D234 File Offset: 0x0004B434
		private void RefreshFood()
		{
			this.FoodPool.ReleaseAll();
			foreach (Vector2Int coord in this.master.Foods)
			{
				this.FoodPool.Get(null).localPosition = this.GetPosition(coord);
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004D2A8 File Offset: 0x0004B4A8
		private void OnRemovePart(SnakeForce.Part part)
		{
			this.PartPool.ReleaseAll((SnakePartDisplay e) => e.Target == part);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004D2DA File Offset: 0x0004B4DA
		private void OnAddPart(SnakeForce.Part part)
		{
			this.PartPool.Get(null).Setup(this, part);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004D2EF File Offset: 0x0004B4EF
		internal Vector3 GetPosition(Vector2Int coord)
		{
			return coord * this.gridSize;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0004D308 File Offset: 0x0004B508
		internal Vector3 GetWorldPosition(Vector2Int coord)
		{
			Vector3 position = this.GetPosition(coord);
			return base.transform.TransformPoint(position);
		}

		// Token: 0x04000F3F RID: 3903
		[SerializeField]
		private SnakeForce master;

		// Token: 0x04000F40 RID: 3904
		[SerializeField]
		private SnakePartDisplay partDisplayTemplate;

		// Token: 0x04000F41 RID: 3905
		[SerializeField]
		private Transform foodDisplayTemplate;

		// Token: 0x04000F42 RID: 3906
		[SerializeField]
		private Transform exitDisplayTemplte;

		// Token: 0x04000F43 RID: 3907
		[SerializeField]
		private ParticleSystem eatFXPrefab;

		// Token: 0x04000F44 RID: 3908
		[SerializeField]
		private int gridSize = 8;

		// Token: 0x04000F45 RID: 3909
		private PrefabPool<SnakePartDisplay> _partPool;

		// Token: 0x04000F46 RID: 3910
		private PrefabPool<Transform> _foodPool;

		// Token: 0x04000F47 RID: 3911
		private bool punchingColor;

		// Token: 0x04000F48 RID: 3912
		private int punchColorIndex;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002DA RID: 730
	public class BubblePopper : MiniGameBehaviour
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00054280 File Offset: 0x00052480
		public int AvaliableColorCount
		{
			get
			{
				return this.colorPallette.Length;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x0005428A File Offset: 0x0005248A
		public BubblePopperLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00054292 File Offset: 0x00052492
		public float BubbleRadius
		{
			get
			{
				if (this.bubbleTemplate == null)
				{
					return 8f;
				}
				return this.bubbleTemplate.Radius;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x000542B3 File Offset: 0x000524B3
		public Bubble BubbleTemplate
		{
			get
			{
				return this.bubbleTemplate;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x000542BC File Offset: 0x000524BC
		private PrefabPool<Bubble> BubblePool
		{
			get
			{
				if (this._bubblePool == null)
				{
					this._bubblePool = new PrefabPool<Bubble>(this.bubbleTemplate, null, new Action<Bubble>(this.OnGetBubble), null, null, true, 10, 10000, null);
				}
				return this._bubblePool;
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00054300 File Offset: 0x00052500
		private void OnGetBubble(Bubble bubble)
		{
			bubble.Rest();
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00054308 File Offset: 0x00052508
		// (set) Token: 0x06001716 RID: 5910 RVA: 0x00054310 File Offset: 0x00052510
		public BubblePopper.Status status { get; private set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x00054319 File Offset: 0x00052519
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x00054321 File Offset: 0x00052521
		public int FloorStepETA { get; private set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x0005432A File Offset: 0x0005252A
		// (set) Token: 0x0600171A RID: 5914 RVA: 0x00054332 File Offset: 0x00052532
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				this.RefreshScoreText();
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00054341 File Offset: 0x00052541
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x0005434D File Offset: 0x0005254D
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighScore");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighScore", value);
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x0005435A File Offset: 0x0005255A
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00054366 File Offset: 0x00052566
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighLevel", value);
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00054373 File Offset: 0x00052573
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x0005437B File Offset: 0x0005257B
		public bool Busy { get; private set; }

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06001721 RID: 5921 RVA: 0x00054384 File Offset: 0x00052584
		// (remove) Token: 0x06001722 RID: 5922 RVA: 0x000543B8 File Offset: 0x000525B8
		public static event Action<int> OnLevelClear;

		// Token: 0x06001723 RID: 5923 RVA: 0x000543EB File Offset: 0x000525EB
		protected override void Start()
		{
			base.Start();
			this.RefreshScoreText();
			this.RefreshLevelText();
			this.HideEndScreen();
			this.ShowStartScreen();
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0005440C File Offset: 0x0005260C
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
			this.highScoreText.text = string.Format("{0}", BubblePopper.HighScore);
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00054458 File Offset: 0x00052658
		private void RefreshLevelText()
		{
			this.levelText.text = string.Format("{0}", this.levelIndex);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0005447A File Offset: 0x0005267A
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.HandleInput(deltaTime);
			this.UpdateAimingLine();
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00054490 File Offset: 0x00052690
		private void ShowStartScreen()
		{
			this.startScreen.SetActive(true);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0005449E File Offset: 0x0005269E
		private void HideStartScreen()
		{
			this.startScreen.SetActive(false);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000544AC File Offset: 0x000526AC
		private void ShowEndScreen()
		{
			this.endScreen.SetActive(true);
			this.endScreenLevelText.text = string.Format("LEVEL {0}", this.levelIndex);
			this.endScreenScoreText.text = string.Format("{0}", this.Score);
			this.failIndicator.SetActive(this.fail);
			this.clearIndicator.SetActive(this.clear);
			this.newRecordIndicator.SetActive(this.isHighScore);
			this.allLevelsClearIndicator.SetActive(this.allLevelsClear);
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00054549 File Offset: 0x00052749
		private void HideEndScreen()
		{
			this.endScreen.SetActive(false);
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00054558 File Offset: 0x00052758
		private void NewGame()
		{
			this.playing = true;
			this.levelIndex = 0;
			this.Score = 0;
			this.isHighScore = false;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000545A8 File Offset: 0x000527A8
		private void NextLevel()
		{
			this.levelIndex++;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x000545E9 File Offset: 0x000527E9
		private int[] LoadLevelData(int levelIndex)
		{
			return this.levelDataProvider.GetData(levelIndex);
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000545F8 File Offset: 0x000527F8
		private Vector2Int LevelDataIndexToCoord(int index)
		{
			int num = this.layout.XCoordBorder.y - this.layout.XCoordBorder.x + 1;
			int num2 = index / num;
			return new Vector2Int(index % num, -num2);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00054638 File Offset: 0x00052838
		private void StartNewLevel(int[] levelData)
		{
			this.clear = false;
			this.fail = false;
			this.FloorStepETA = this.floorStepAfterShots;
			this.BubblePool.ReleaseAll();
			this.attachedBubbles.Clear();
			this.ResetFloor();
			for (int i = 0; i < levelData.Length; i++)
			{
				int num = levelData[i];
				if (num >= 0)
				{
					Vector2Int coord = this.LevelDataIndexToCoord(i);
					Bubble bubble = this.BubblePool.Get(null);
					bubble.Setup(this, num);
					this.Set(bubble, coord);
				}
			}
			this.PushRandomColor();
			this.PushRandomColor();
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x000546C9 File Offset: 0x000528C9
		private void ResetFloor()
		{
			this.floorYCoord = this.initialFloorYCoord;
			this.RefreshLayoutPosition();
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x000546DD File Offset: 0x000528DD
		private void StepFloor()
		{
			this.floorYCoord++;
			this.BeginMovingCeiling();
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000546F4 File Offset: 0x000528F4
		private void RefreshLayoutPosition()
		{
			Vector3 localPosition = this.layout.transform.localPosition;
			localPosition.y = (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor;
			this.layout.transform.localPosition = localPosition;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00054748 File Offset: 0x00052948
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.GameOver:
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					if (!this.playing || this.fail || this.allLevelsClear)
					{
						this.NewGame();
						return;
					}
					this.NextLevel();
					return;
				}
				break;
			case BubblePopper.Status.Loaded:
				break;
			case BubblePopper.Status.Launched:
				this.UpdateLaunched(deltaTime);
				return;
			case BubblePopper.Status.Settled:
				this.UpdateSettled(deltaTime);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000547BA File Offset: 0x000529BA
		private void BeginMovingCeiling()
		{
			this.movingCeiling = true;
			this.moveCeilingT = 0f;
			this.originalCeilingPos = this.layout.transform.localPosition;
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x000547EC File Offset: 0x000529EC
		private void UpdateMoveCeiling(float deltaTime)
		{
			this.moveCeilingT += deltaTime;
			if (this.moveCeilingT >= this.moveCeilingTime)
			{
				this.movingCeiling = false;
				this.RefreshLayoutPosition();
				return;
			}
			Vector3 vector = this.layout.transform.localPosition;
			Vector2 b = new Vector2(vector.x, (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor);
			float t = this.moveCeilingCurve.Evaluate(this.moveCeilingT / this.moveCeilingTime);
			vector = Vector2.LerpUnclamped(this.originalCeilingPos, b, t);
			this.layout.transform.localPosition = vector;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0005489A File Offset: 0x00052A9A
		private void UpdateSettled(float deltaTime)
		{
			if (this.movingCeiling)
			{
				this.UpdateMoveCeiling(deltaTime);
				return;
			}
			if (this.CheckGameOver())
			{
				this.SetStatus(BubblePopper.Status.GameOver);
				return;
			}
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000548C4 File Offset: 0x00052AC4
		private void HandleFloorStep()
		{
			int floorStepETA = this.FloorStepETA;
			this.FloorStepETA = floorStepETA - 1;
			if (this.FloorStepETA <= 0)
			{
				this.StepFloor();
				this.FloorStepETA = this.floorStepAfterShots;
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000548FC File Offset: 0x00052AFC
		private bool CheckGameOver()
		{
			if (this.attachedBubbles.Count == 0)
			{
				this.clear = true;
				this.allLevelsClear = (this.levelIndex >= this.levelDataProvider.TotalLevels);
				if (this.clear)
				{
					if (this.levelIndex > BubblePopper.HighLevel)
					{
						BubblePopper.HighLevel = this.levelIndex;
					}
					Action<int> onLevelClear = BubblePopper.OnLevelClear;
					if (onLevelClear != null)
					{
						onLevelClear(this.levelIndex);
					}
				}
				return true;
			}
			if (this.attachedBubbles.Keys.Any((Vector2Int e) => e.y <= this.floorYCoord))
			{
				this.fail = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0005499C File Offset: 0x00052B9C
		private void SetStatus(BubblePopper.Status newStatus)
		{
			this.OnExitStatus(this.status);
			this.status = newStatus;
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.Loaded:
			case BubblePopper.Status.Launched:
				break;
			case BubblePopper.Status.Settled:
				this.PushRandomColor();
				this.HandleFloorStep();
				return;
			case BubblePopper.Status.GameOver:
				if (this.Score > BubblePopper.HighScore)
				{
					BubblePopper.HighScore = this.Score;
					this.isHighScore = true;
				}
				this.ShowGameOverScreen();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00054A10 File Offset: 0x00052C10
		private void ShowGameOverScreen()
		{
			this.ShowEndScreen();
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00054A18 File Offset: 0x00052C18
		private void OnExitStatus(BubblePopper.Status status)
		{
			switch (status)
			{
			default:
				return;
			}
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00054A30 File Offset: 0x00052C30
		private void Set(Bubble bubble, Vector2Int coord)
		{
			this.attachedBubbles[coord] = bubble;
			bubble.NotifyAttached(coord);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00054A48 File Offset: 0x00052C48
		private void Attach(Bubble bubble, Vector2Int coord)
		{
			Bubble bubble2;
			if (this.attachedBubbles.TryGetValue(coord, out bubble2))
			{
				Debug.LogError("Target coord is occupied!");
				return;
			}
			this.Set(bubble, coord);
			List<Vector2Int> continousCoords = this.GetContinousCoords(coord);
			if (continousCoords.Count >= 3)
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				int num = 0;
				foreach (Vector2Int vector2Int in continousCoords)
				{
					hashSet.AddRange(this.layout.GetAllNeighbourCoords(vector2Int, false));
					this.Explode(vector2Int, coord);
					num++;
				}
				this.PunchCamera();
				HashSet<Vector2Int> looseCoords = this.GetLooseCoords(hashSet);
				foreach (Vector2Int coord2 in looseCoords)
				{
					this.Detach(coord2);
				}
				this.CalculateAndAddScore(looseCoords, continousCoords);
			}
			this.Shockwave(coord, this.shockwaveStrength).Forget();
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00054B5C File Offset: 0x00052D5C
		private void CalculateAndAddScore(HashSet<Vector2Int> detached, List<Vector2Int> exploded)
		{
			float count = (float)exploded.Count;
			int count2 = detached.Count;
			int num = Mathf.FloorToInt(Mathf.Pow(count, 2f)) * (1 + count2);
			this.Score += num;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00054B9C File Offset: 0x00052D9C
		private void Explode(Vector2Int coord, Vector2Int origin)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			if (bubble == null)
			{
				return;
			}
			bubble.NotifyExplode(origin);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00054BD8 File Offset: 0x00052DD8
		private List<Vector2Int> GetContinousCoords(Vector2Int root)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(root, out bubble))
			{
				return list;
			}
			if (bubble == null)
			{
				return list;
			}
			int colorIndex = bubble.ColorIndex;
			BubblePopper.<>c__DisplayClass117_0 CS$<>8__locals1;
			CS$<>8__locals1.visitedCoords = new HashSet<Vector2Int>();
			CS$<>8__locals1.coords = new Stack<Vector2Int>();
			BubblePopper.<GetContinousCoords>g__Push|117_0(root, ref CS$<>8__locals1);
			while (CS$<>8__locals1.coords.Count > 0)
			{
				Vector2Int vector2Int = CS$<>8__locals1.coords.Pop();
				Bubble bubble2;
				if (this.attachedBubbles.TryGetValue(vector2Int, out bubble2) && !(bubble2 == null) && bubble2.ColorIndex == colorIndex)
				{
					list.Add(vector2Int);
					foreach (Vector2Int vector2Int2 in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!CS$<>8__locals1.visitedCoords.Contains(vector2Int2))
						{
							BubblePopper.<GetContinousCoords>g__Push|117_0(vector2Int2, ref CS$<>8__locals1);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00054CC8 File Offset: 0x00052EC8
		private HashSet<Vector2Int> GetLooseCoords(HashSet<Vector2Int> roots)
		{
			BubblePopper.<>c__DisplayClass118_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pendingRoots = roots.ToList<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			while (CS$<>8__locals1.pendingRoots.Count > 0)
			{
				Vector2Int root = this.<GetLooseCoords>g__PopRoot|118_0(ref CS$<>8__locals1);
				List<Vector2Int> range;
				if (this.<GetLooseCoords>g__CheckConnectedLoose|118_1(root, out range, ref CS$<>8__locals1))
				{
					hashSet.AddRange(range);
				}
			}
			return hashSet;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00054D20 File Offset: 0x00052F20
		private void Detach(Vector2Int coord)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			bubble.NotifyDetached();
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x00054D54 File Offset: 0x00052F54
		private void UpdateAimingLine()
		{
			this.aimingLine.gameObject.SetActive(this.status == BubblePopper.Status.Loaded);
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 vector2 = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			Vector3 v = vector2 * this.aimingDistance;
			BubblePopper.CastResult castResult = this.SlideCast(vector, v);
			vector.z = 0f;
			this.aimlinePoints[0] = vector;
			this.aimlinePoints[1] = castResult.endPosition;
			if (castResult.touchWall)
			{
				float d = Mathf.Max(this.aimingDistance - (castResult.endPosition - vector).magnitude, 0f);
				Vector2 a = vector2;
				a.x *= -1f;
				this.aimlinePoints[2] = castResult.endPosition + a * d;
			}
			else
			{
				this.aimlinePoints[2] = castResult.endPosition;
			}
			this.aimingLine.SetPositions(this.aimlinePoints);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00054EA3 File Offset: 0x000530A3
		private void UpdateLaunched(float deltaTime)
		{
			if (this.activeBubble == null || this.activeBubble.status != Bubble.Status.Moving)
			{
				this.activeBubble = null;
				this.SetStatus(BubblePopper.Status.Settled);
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00054ED0 File Offset: 0x000530D0
		private void HandleInput(float deltaTime)
		{
			float x = base.Game.GetAxis(0).x;
			this.cannonAngle = Mathf.Clamp(this.cannonAngle - x * this.cannonRotateSpeed * deltaTime, this.cannonAngleRange.x, this.cannonAngleRange.y);
			this.cannon.rotation = Quaternion.Euler(0f, 0f, this.cannonAngle);
			this.duckAnimator.SetInteger("MovementDirection", (x > 0.01f) ? 1 : ((x < -0.01f) ? -1 : 0));
			this.gear.Rotate(0f, 0f, x * this.cannonRotateSpeed * deltaTime);
			if (base.Game.GetButtonDown(MiniGame.Button.A))
			{
				this.LaunchBubble();
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00054F9C File Offset: 0x0005319C
		public void MoveBubble(Bubble bubble, float deltaTime)
		{
			if (bubble == null)
			{
				return;
			}
			Vector2 moveDirection = bubble.MoveDirection;
			float d = deltaTime * this.bubbleMoveSpeed;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Matrix4x4 localToWorldMatrix = this.layout.transform.localToWorldMatrix;
			Vector2 normalized = moveDirection.normalized;
			Vector2 origin = worldToLocalMatrix.MultiplyPoint(bubble.transform.position);
			Vector2 delta = worldToLocalMatrix.MultiplyVector(moveDirection.normalized) * d;
			BubblePopper.CastResult castResult = this.SlideCast(origin, delta);
			bubble.transform.position = localToWorldMatrix.MultiplyPoint(castResult.endPosition);
			if (!castResult.Collide)
			{
				return;
			}
			if (castResult.touchWall && (float)castResult.touchWallDirection * normalized.x > 0f)
			{
				moveDirection.x *= -1f;
				bubble.MoveDirection = moveDirection;
			}
			if (castResult.touchingBubble || castResult.touchCeiling)
			{
				this.Attach(bubble, castResult.endCoord);
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000550B8 File Offset: 0x000532B8
		private Bubble LaunchBubble(Vector2 origin, Vector2 direction, int colorIndex)
		{
			Bubble bubble = this.BubblePool.Get(null);
			bubble.transform.position = this.layout.transform.localToWorldMatrix.MultiplyPoint(origin);
			bubble.MoveDirection = direction;
			bubble.Setup(this, colorIndex);
			bubble.Launch(direction);
			return bubble;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00055110 File Offset: 0x00053310
		private void LaunchBubble()
		{
			if (this.status != BubblePopper.Status.Loaded)
			{
				return;
			}
			this.activeBubble = this.LaunchBubble(this.layout.transform.worldToLocalMatrix.MultiplyPoint(this.cannon.transform.position), this.layout.transform.worldToLocalMatrix.MultiplyVector(this.cannon.transform.up), this.loadedColor);
			this.loadedColor = -1;
			this.RefreshColorIndicators();
			this.SetStatus(BubblePopper.Status.Launched);
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x000551A8 File Offset: 0x000533A8
		private void PunchLoadedIndicator()
		{
			this.loadedColorIndicator.transform.DOKill(true);
			this.loadedColorIndicator.transform.localPosition = Vector2.left * 15f;
			this.loadedColorIndicator.transform.DOLocalMove(Vector3.zero, 0.1f, true);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00055208 File Offset: 0x00053408
		private void PunchWaitingIndicator()
		{
			this.waitingColorIndicator.transform.localPosition = Vector2.zero;
			this.waitingColorIndicator.transform.DOKill(true);
			this.waitingColorIndicator.transform.DOPunchPosition(Vector3.down * 5f, 0.5f, 10, 1f, true);
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00055270 File Offset: 0x00053470
		private void PushRandomColor()
		{
			this.loadedColor = this.waitingColor;
			this.waitingColor = UnityEngine.Random.Range(0, this.AvaliableColorCount);
			if (this.attachedBubbles.Count <= 0)
			{
				this.waitingColor = UnityEngine.Random.Range(0, this.AvaliableColorCount);
			}
			List<int> list = (from e in this.attachedBubbles.Values
			group e by e.ColorIndex into g
			select g.Key).ToList<int>();
			this.waitingColor = list.GetRandom<int>();
			this.RefreshColorIndicators();
			this.PunchLoadedIndicator();
			this.PunchWaitingIndicator();
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00055332 File Offset: 0x00053532
		private void RefreshColorIndicators()
		{
			this.loadedColorIndicator.color = this.GetDisplayColor(this.loadedColor);
			this.waitingColorIndicator.color = this.GetDisplayColor(this.waitingColor);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00055362 File Offset: 0x00053562
		private bool IsCoordOccupied(Vector2Int coord, out Bubble touchingBubble, out bool ceiling)
		{
			ceiling = false;
			if (this.attachedBubbles.TryGetValue(coord, out touchingBubble))
			{
				return true;
			}
			if (coord.y > this.ceilingYCoord)
			{
				ceiling = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00055390 File Offset: 0x00053590
		public BubblePopper.CastResult SlideCast(Vector2 origin, Vector2 delta)
		{
			float magnitude = delta.magnitude;
			Vector2 normalized = delta.normalized;
			float bubbleRadius = this.BubbleRadius;
			BubblePopper.CastResult castResult = default(BubblePopper.CastResult);
			castResult.origin = origin;
			castResult.castDirection = normalized;
			castResult.castDistance = magnitude;
			Vector2 vector = origin + delta;
			float d = 1f;
			float num = this.layout.XPositionBorder.x + bubbleRadius;
			float num2 = this.layout.XPositionBorder.y - bubbleRadius;
			if (origin.x < num || origin.x > num2)
			{
				Vector2 vector2 = origin;
				vector2.x = Mathf.Clamp(vector2.x, num + 0.001f, num2 - 0.001f);
				castResult.endPosition = vector2;
				castResult.clipWall = true;
				castResult.collide = true;
			}
			else
			{
				if (vector.x < num)
				{
					castResult.touchWall = true;
					d = Mathf.Abs(origin.x - num) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = -1;
				}
				else if (vector.x > num2)
				{
					castResult.touchWall = true;
					d = Mathf.Abs(num2 - origin.x) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = 1;
				}
				delta *= d;
				magnitude = delta.magnitude;
				castResult.endPosition = origin + delta;
				List<Vector2Int> allPassingCoords = this.layout.GetAllPassingCoords(origin, normalized, delta.magnitude);
				float num3 = magnitude;
				foreach (Vector2Int vector2Int in allPassingCoords)
				{
					Bubble touchingBubble;
					bool touchCeiling;
					Vector2 vector3;
					if (this.IsCoordOccupied(vector2Int, out touchingBubble, out touchCeiling) && this.BubbleCast(this.layout.CoordToLocalPosition(vector2Int), origin, normalized, magnitude, out vector3))
					{
						float magnitude2 = (vector3 - origin).magnitude;
						if (magnitude2 < num3)
						{
							castResult.collide = true;
							castResult.touchingBubble = touchingBubble;
							castResult.touchBubbleCoord = vector2Int;
							castResult.endPosition = vector3;
							castResult.touchCeiling = touchCeiling;
							num3 = magnitude2;
							castResult.touchWall = false;
						}
					}
				}
			}
			castResult.endCoord = this.layout.LocalPositionToCoord(castResult.endPosition);
			return castResult;
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x000555E0 File Offset: 0x000537E0
		private bool BubbleCast(Vector2 pos, Vector2 origin, Vector2 direction, float distance, out Vector2 hitCircleCenter)
		{
			float bubbleRadius = this.BubbleRadius;
			hitCircleCenter = origin;
			Vector2 vector = pos - origin;
			float sqrMagnitude = vector.sqrMagnitude;
			float magnitude = vector.magnitude;
			if (magnitude > distance + 2f * bubbleRadius)
			{
				return false;
			}
			if (magnitude <= bubbleRadius * 2f)
			{
				hitCircleCenter = pos - 2f * vector.normalized * bubbleRadius;
				return true;
			}
			if (Vector2.Dot(vector, direction) < 0f)
			{
				return false;
			}
			float f = 0.017453292f * Vector2.Angle(vector, direction);
			float num = vector.magnitude * Mathf.Sin(f);
			if (num > 2f * bubbleRadius)
			{
				return false;
			}
			float num2 = num * num;
			float num3 = bubbleRadius * bubbleRadius * 2f * 2f;
			float num4 = Mathf.Sqrt(sqrMagnitude - num2) - Mathf.Sqrt(num3 - num2);
			if (num4 > distance)
			{
				return false;
			}
			hitCircleCenter = origin + direction * num4;
			return true;
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000556DC File Offset: 0x000538DC
		private void OnDrawGizmos()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			float bubbleRadius = this.BubbleRadius;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 a = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			BubblePopper.CastResult castResult = this.SlideCast(vector, a * this.distance);
			Gizmos.matrix = this.layout.transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
			for (int i = this.layout.XCoordBorder.x; i <= this.layout.XCoordBorder.y; i++)
			{
				for (int j = this.floorYCoord; j <= this.ceilingYCoord; j++)
				{
					new Vector2Int(i, j);
					this.layout.GizmosDrawCoord(new Vector2Int(i, j), 0.25f);
				}
			}
			Gizmos.color = (castResult.Collide ? Color.red : Color.green);
			Gizmos.DrawWireSphere(vector, bubbleRadius);
			Gizmos.DrawWireSphere(castResult.endPosition, bubbleRadius);
			Gizmos.DrawLine(vector, castResult.endPosition);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.endCoord), bubbleRadius * 0.8f);
			if (castResult.collide)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.touchBubbleCoord), bubbleRadius * 0.5f);
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00055895 File Offset: 0x00053A95
		internal void Release(Bubble bubble)
		{
			this.BubblePool.Release(bubble);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000558A3 File Offset: 0x00053AA3
		internal Color GetDisplayColor(int colorIndex)
		{
			if (colorIndex < 0)
			{
				return Color.clear;
			}
			if (colorIndex >= this.colorPallette.Length)
			{
				return Color.white;
			}
			return this.colorPallette[colorIndex];
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000558CC File Offset: 0x00053ACC
		private UniTask Shockwave(Vector2Int origin, float amplitude)
		{
			BubblePopper.<Shockwave>d__140 <Shockwave>d__;
			<Shockwave>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Shockwave>d__.<>4__this = this;
			<Shockwave>d__.origin = origin;
			<Shockwave>d__.amplitude = amplitude;
			<Shockwave>d__.<>1__state = -1;
			<Shockwave>d__.<>t__builder.Start<BubblePopper.<Shockwave>d__140>(ref <Shockwave>d__);
			return <Shockwave>d__.<>t__builder.Task;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00055920 File Offset: 0x00053B20
		private void PunchCamera()
		{
			this.cameraParent.DOKill(true);
			this.cameraParent.DOShakePosition(0.4f, 1f, 10, 90f, false, true);
			this.cameraParent.DOShakeRotation(0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00055A18 File Offset: 0x00053C18
		[CompilerGenerated]
		internal static void <GetContinousCoords>g__Push|117_0(Vector2Int coord, ref BubblePopper.<>c__DisplayClass117_0 A_1)
		{
			A_1.coords.Push(coord);
			A_1.visitedCoords.Add(coord);
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00055A33 File Offset: 0x00053C33
		[CompilerGenerated]
		private Vector2Int <GetLooseCoords>g__PopRoot|118_0(ref BubblePopper.<>c__DisplayClass118_0 A_1)
		{
			Vector2Int result = A_1.pendingRoots[0];
			A_1.pendingRoots.RemoveAt(0);
			return result;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00055A50 File Offset: 0x00053C50
		[CompilerGenerated]
		private bool <GetLooseCoords>g__CheckConnectedLoose|118_1(Vector2Int root, out List<Vector2Int> connected, ref BubblePopper.<>c__DisplayClass118_0 A_3)
		{
			connected = new List<Vector2Int>();
			bool result = true;
			Stack<Vector2Int> stack = new Stack<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			stack.Push(root);
			hashSet.Add(root);
			while (stack.Count > 0)
			{
				Vector2Int vector2Int = stack.Pop();
				A_3.pendingRoots.Remove(vector2Int);
				if (this.attachedBubbles.ContainsKey(vector2Int))
				{
					if (vector2Int.y >= this.ceilingYCoord)
					{
						result = false;
					}
					connected.Add(vector2Int);
					foreach (Vector2Int item in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!hashSet.Contains(item))
						{
							stack.Push(item);
							hashSet.Add(item);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040010D9 RID: 4313
		[SerializeField]
		private Bubble bubbleTemplate;

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private BubblePopperLayout layout;

		// Token: 0x040010DB RID: 4315
		[SerializeField]
		private Image waitingColorIndicator;

		// Token: 0x040010DC RID: 4316
		[SerializeField]
		private Image loadedColorIndicator;

		// Token: 0x040010DD RID: 4317
		[SerializeField]
		private Transform cannon;

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private LineRenderer aimingLine;

		// Token: 0x040010DF RID: 4319
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private Animator duckAnimator;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private Transform gear;

		// Token: 0x040010E2 RID: 4322
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x040010E3 RID: 4323
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x040010E4 RID: 4324
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x040010E5 RID: 4325
		[SerializeField]
		private GameObject startScreen;

		// Token: 0x040010E6 RID: 4326
		[SerializeField]
		private GameObject endScreen;

		// Token: 0x040010E7 RID: 4327
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x040010E8 RID: 4328
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x040010E9 RID: 4329
		[SerializeField]
		private GameObject newRecordIndicator;

		// Token: 0x040010EA RID: 4330
		[SerializeField]
		private GameObject allLevelsClearIndicator;

		// Token: 0x040010EB RID: 4331
		[SerializeField]
		private TextMeshProUGUI endScreenLevelText;

		// Token: 0x040010EC RID: 4332
		[SerializeField]
		private TextMeshProUGUI endScreenScoreText;

		// Token: 0x040010ED RID: 4333
		[SerializeField]
		private BubblePopperLevelDataProvider levelDataProvider;

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private Color[] colorPallette;

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private float aimingDistance = 100f;

		// Token: 0x040010F0 RID: 4336
		[SerializeField]
		private Vector2 cannonAngleRange = new Vector2(-45f, 45f);

		// Token: 0x040010F1 RID: 4337
		[SerializeField]
		private float cannonRotateSpeed = 20f;

		// Token: 0x040010F2 RID: 4338
		[SerializeField]
		private int ceilingYCoord;

		// Token: 0x040010F3 RID: 4339
		[SerializeField]
		private int initialFloorYCoord = -18;

		// Token: 0x040010F4 RID: 4340
		[SerializeField]
		private int floorStepAfterShots = 4;

		// Token: 0x040010F5 RID: 4341
		[SerializeField]
		private float bubbleMoveSpeed = 100f;

		// Token: 0x040010F6 RID: 4342
		private float shockwaveStrength = 2f;

		// Token: 0x040010F7 RID: 4343
		[SerializeField]
		private float moveCeilingTime = 1f;

		// Token: 0x040010F8 RID: 4344
		[SerializeField]
		private AnimationCurve moveCeilingCurve;

		// Token: 0x040010F9 RID: 4345
		private PrefabPool<Bubble> _bubblePool;

		// Token: 0x040010FA RID: 4346
		private Dictionary<Vector2Int, Bubble> attachedBubbles = new Dictionary<Vector2Int, Bubble>();

		// Token: 0x040010FB RID: 4347
		private float cannonAngle;

		// Token: 0x040010FC RID: 4348
		private int waitingColor;

		// Token: 0x040010FD RID: 4349
		private int loadedColor;

		// Token: 0x040010FE RID: 4350
		private Bubble activeBubble;

		// Token: 0x04001100 RID: 4352
		private bool clear;

		// Token: 0x04001101 RID: 4353
		private bool fail;

		// Token: 0x04001102 RID: 4354
		private bool allLevelsClear;

		// Token: 0x04001103 RID: 4355
		private bool playing;

		// Token: 0x04001104 RID: 4356
		[SerializeField]
		private int floorYCoord;

		// Token: 0x04001106 RID: 4358
		private int levelIndex;

		// Token: 0x04001107 RID: 4359
		private int _score;

		// Token: 0x04001108 RID: 4360
		private bool isHighScore;

		// Token: 0x04001109 RID: 4361
		private const string HighScoreSaveKey = "MiniGame/BubblePopper/HighScore";

		// Token: 0x0400110A RID: 4362
		private const string HighLevelSaveKey = "MiniGame/BubblePopper/HighLevel";

		// Token: 0x0400110C RID: 4364
		private const int CriticalCount = 3;

		// Token: 0x0400110E RID: 4366
		private bool movingCeiling;

		// Token: 0x0400110F RID: 4367
		private float moveCeilingT;

		// Token: 0x04001110 RID: 4368
		private Vector2 originalCeilingPos;

		// Token: 0x04001111 RID: 4369
		private Vector3[] aimlinePoints = new Vector3[3];

		// Token: 0x04001112 RID: 4370
		[SerializeField]
		private bool drawGizmos = true;

		// Token: 0x04001113 RID: 4371
		[SerializeField]
		private float distance;

		// Token: 0x02000573 RID: 1395
		public enum Status
		{
			// Token: 0x04001F7A RID: 8058
			Idle,
			// Token: 0x04001F7B RID: 8059
			Loaded,
			// Token: 0x04001F7C RID: 8060
			Launched,
			// Token: 0x04001F7D RID: 8061
			Settled,
			// Token: 0x04001F7E RID: 8062
			GameOver
		}

		// Token: 0x02000574 RID: 1396
		public struct CastResult
		{
			// Token: 0x17000765 RID: 1893
			// (get) Token: 0x0600284E RID: 10318 RVA: 0x00094B86 File Offset: 0x00092D86
			public bool Collide
			{
				get
				{
					return this.collide || this.clipWall || this.touchWall || this.touchingBubble;
				}
			}

			// Token: 0x04001F7F RID: 8063
			public Vector2 origin;

			// Token: 0x04001F80 RID: 8064
			public Vector2 castDirection;

			// Token: 0x04001F81 RID: 8065
			public float castDistance;

			// Token: 0x04001F82 RID: 8066
			public bool clipWall;

			// Token: 0x04001F83 RID: 8067
			public bool touchWall;

			// Token: 0x04001F84 RID: 8068
			public int touchWallDirection;

			// Token: 0x04001F85 RID: 8069
			public bool collide;

			// Token: 0x04001F86 RID: 8070
			public Bubble touchingBubble;

			// Token: 0x04001F87 RID: 8071
			public Vector2Int touchBubbleCoord;

			// Token: 0x04001F88 RID: 8072
			public bool touchCeiling;

			// Token: 0x04001F89 RID: 8073
			public Vector2 endPosition;

			// Token: 0x04001F8A RID: 8074
			public Vector2Int endCoord;
		}
	}
}

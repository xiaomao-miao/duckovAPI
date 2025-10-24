using System;
using System.Collections.Generic;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x02000289 RID: 649
	public class SnakeForce : MiniGameBehaviour
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0004D358 File Offset: 0x0004B558
		public List<SnakeForce.Part> Snake
		{
			get
			{
				return this.snake;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0004D360 File Offset: 0x0004B560
		public List<Vector2Int> Foods
		{
			get
			{
				return this.foods;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0004D368 File Offset: 0x0004B568
		// (set) Token: 0x060014DE RID: 5342 RVA: 0x0004D370 File Offset: 0x0004B570
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				Action<SnakeForce> onScoreChanged = this.OnScoreChanged;
				if (onScoreChanged == null)
				{
					return;
				}
				onScoreChanged(this);
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x0004D38A File Offset: 0x0004B58A
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0004D396 File Offset: 0x0004B596
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/Snake/HighScore");
			}
			private set
			{
				SavesSystem.Save<int>("MiniGame/Snake/HighScore", value);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x0004D3A3 File Offset: 0x0004B5A3
		public SnakeForce.Part Head
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				return this.snake[0];
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0004D3C1 File Offset: 0x0004B5C1
		public SnakeForce.Part Tail
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				List<SnakeForce.Part> list = this.snake;
				return list[list.Count - 1];
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x060014E3 RID: 5347 RVA: 0x0004D3E8 File Offset: 0x0004B5E8
		// (remove) Token: 0x060014E4 RID: 5348 RVA: 0x0004D420 File Offset: 0x0004B620
		public event Action<SnakeForce.Part> OnAddPart;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x060014E5 RID: 5349 RVA: 0x0004D458 File Offset: 0x0004B658
		// (remove) Token: 0x060014E6 RID: 5350 RVA: 0x0004D490 File Offset: 0x0004B690
		public event Action<SnakeForce.Part> OnRemovePart;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x060014E7 RID: 5351 RVA: 0x0004D4C8 File Offset: 0x0004B6C8
		// (remove) Token: 0x060014E8 RID: 5352 RVA: 0x0004D500 File Offset: 0x0004B700
		public event Action<SnakeForce> OnAfterTick;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x060014E9 RID: 5353 RVA: 0x0004D538 File Offset: 0x0004B738
		// (remove) Token: 0x060014EA RID: 5354 RVA: 0x0004D570 File Offset: 0x0004B770
		public event Action<SnakeForce> OnScoreChanged;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x060014EB RID: 5355 RVA: 0x0004D5A8 File Offset: 0x0004B7A8
		// (remove) Token: 0x060014EC RID: 5356 RVA: 0x0004D5E0 File Offset: 0x0004B7E0
		public event Action<SnakeForce> OnGameStart;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060014ED RID: 5357 RVA: 0x0004D618 File Offset: 0x0004B818
		// (remove) Token: 0x060014EE RID: 5358 RVA: 0x0004D650 File Offset: 0x0004B850
		public event Action<SnakeForce> OnGameOver;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060014EF RID: 5359 RVA: 0x0004D688 File Offset: 0x0004B888
		// (remove) Token: 0x060014F0 RID: 5360 RVA: 0x0004D6C0 File Offset: 0x0004B8C0
		public event Action<SnakeForce, Vector2Int> OnFoodEaten;

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004D6F5 File Offset: 0x0004B8F5
		protected override void Start()
		{
			base.Start();
			this.titleScreen.SetActive(true);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0004D70C File Offset: 0x0004B90C
		private void Restart()
		{
			this.Clear();
			this.gameOverScreen.SetActive(false);
			for (int i = this.borderXMin; i <= this.borderXMax; i++)
			{
				for (int j = this.borderYMin; j <= this.borderYMax; j++)
				{
					this.allCoords.Add(new Vector2Int(i, j));
				}
			}
			this.AddPart(new Vector2Int((this.borderXMax + this.borderXMin) / 2, (this.borderYMax + this.borderYMin) / 2), Vector2Int.up);
			this.Grow();
			this.Grow();
			this.AddFood(3);
			this.PunchCamera();
			this.playing = true;
			this.RefreshScoreText();
			this.highScoreText.text = string.Format("{0}", SnakeForce.HighScore);
			Action<SnakeForce> onGameStart = this.OnGameStart;
			if (onGameStart == null)
			{
				return;
			}
			onGameStart(this);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
		private void AddFood(int count = 3)
		{
			List<Vector2Int> list = new List<Vector2Int>(this.allCoords);
			foreach (SnakeForce.Part part in this.snake)
			{
				list.Remove(part.coord);
			}
			if (list.Count <= 0)
			{
				this.Win();
				return;
			}
			foreach (Vector2Int item in list.GetRandomSubSet(count))
			{
				this.foods.Add(item);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0004D898 File Offset: 0x0004BA98
		private void GameOver()
		{
			Action<SnakeForce> onGameOver = this.OnGameOver;
			if (onGameOver != null)
			{
				onGameOver(this);
			}
			bool active = this.Score > SnakeForce.HighScore;
			if (this.Score > SnakeForce.HighScore)
			{
				SnakeForce.HighScore = this.Score;
			}
			this.highScoreIndicator.SetActive(active);
			this.winIndicator.SetActive(this.won);
			this.scoreTextGameOver.text = string.Format("{0}", this.Score);
			this.gameOverScreen.SetActive(true);
			this.PunchCamera();
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0004D92C File Offset: 0x0004BB2C
		private void Win()
		{
			this.won = true;
			this.GameOver();
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0004D93C File Offset: 0x0004BB3C
		protected override void OnUpdate(float deltaTime)
		{
			Vector2 axis = base.Game.GetAxis(0);
			if (axis.sqrMagnitude > 0.1f)
			{
				Vector2Int rhs = default(Vector2Int);
				if (axis.x > 0f)
				{
					rhs = Vector2Int.right;
				}
				else if (axis.x < 0f)
				{
					rhs = Vector2Int.left;
				}
				else if (axis.y > 0f)
				{
					rhs = Vector2Int.up;
				}
				else if (axis.y < 0f)
				{
					rhs = Vector2Int.down;
				}
				if (this.lastFrameAxis != rhs)
				{
					this.axisInput = true;
				}
				this.lastFrameAxis = rhs;
			}
			else
			{
				this.lastFrameAxis = Vector2Int.zero;
			}
			if (this.freezeCountDown > 0.0)
			{
				this.freezeCountDown -= (double)Time.unscaledDeltaTime;
				return;
			}
			if (this.dead || this.won || !this.playing)
			{
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					this.Restart();
				}
				return;
			}
			this.RefreshScoreText();
			bool flag = base.Game.GetButton(MiniGame.Button.B) || base.Game.GetButton(MiniGame.Button.A);
			this.tickETA -= deltaTime * (flag ? 10f : 1f);
			float time = (this.playTick < (ulong)((long)this.maxSpeedTick)) ? (this.playTick / (float)this.maxSpeedTick) : 1f;
			float num = Mathf.Lerp(this.tickIntervalFrom, this.tickIntervalTo, this.speedCurve.Evaluate(time));
			if (this.tickETA <= 0f || this.axisInput)
			{
				this.Tick();
				this.tickETA = num;
				this.axisInput = false;
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0004DAEF File Offset: 0x0004BCEF
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0004DB11 File Offset: 0x0004BD11
		private void Tick()
		{
			this.playTick += 1UL;
			if (this.Head == null)
			{
				return;
			}
			this.HandleMovement();
			this.DetectDeath();
			this.HandleEatAndGrow();
			Action<SnakeForce> onAfterTick = this.OnAfterTick;
			if (onAfterTick == null)
			{
				return;
			}
			onAfterTick(this);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004DB50 File Offset: 0x0004BD50
		private void HandleMovement()
		{
			Vector2Int vector2Int = this.lastFrameAxis;
			if ((!(vector2Int == -this.Head.direction) || this.snake.Count <= 1) && vector2Int != Vector2Int.zero)
			{
				this.Head.direction = vector2Int;
			}
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				Vector2Int coord = (i > 0) ? this.snake[i - 1].coord : (part.coord + part.direction);
				if (i > 0)
				{
					part.direction = this.snake[i - 1].direction;
				}
				if (coord.x > this.borderXMax)
				{
					coord.x = this.borderXMin;
				}
				if (coord.y > this.borderYMax)
				{
					coord.y = this.borderYMin;
				}
				if (coord.x < this.borderXMin)
				{
					coord.x = this.borderXMax;
				}
				if (coord.y < this.borderYMin)
				{
					coord.y = this.borderYMax;
				}
				part.MoveTo(coord);
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004DC90 File Offset: 0x0004BE90
		private void HandleEatAndGrow()
		{
			Vector2Int coord = this.Head.coord;
			if (this.foods.Remove(coord))
			{
				this.Grow();
				int score = this.Score;
				this.Score = score + 1;
				int num = 3 + Mathf.FloorToInt(Mathf.Log((float)this.Score, 2f));
				int count = Mathf.Max(0, num - this.foods.Count);
				this.AddFood(count);
				Action<SnakeForce, Vector2Int> onFoodEaten = this.OnFoodEaten;
				if (onFoodEaten != null)
				{
					onFoodEaten(this, coord);
				}
				this.PunchCamera();
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004DD1C File Offset: 0x0004BF1C
		private void DetectDeath()
		{
			Vector2Int coord = this.Head.coord;
			for (int i = 1; i < this.snake.Count; i++)
			{
				if (this.snake[i].coord == coord)
				{
					this.dead = true;
					this.GameOver();
					return;
				}
			}
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004DD74 File Offset: 0x0004BF74
		private SnakeForce.Part Grow()
		{
			if (this.snake.Count == 0)
			{
				Debug.LogError("Cannot grow the snake! It haven't been created yet.");
				return null;
			}
			SnakeForce.Part tail = this.Tail;
			Vector2Int coord = tail.coord - tail.direction;
			return this.AddPart(coord, tail.direction);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004DDC0 File Offset: 0x0004BFC0
		private SnakeForce.Part AddPart(Vector2Int coord, Vector2Int direction)
		{
			SnakeForce.Part part = new SnakeForce.Part(this, coord, direction);
			this.snake.Add(part);
			Action<SnakeForce.Part> onAddPart = this.OnAddPart;
			if (onAddPart != null)
			{
				onAddPart(part);
			}
			return part;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004DDF5 File Offset: 0x0004BFF5
		private bool RemovePart(SnakeForce.Part part)
		{
			if (!this.snake.Remove(part))
			{
				return false;
			}
			Action<SnakeForce.Part> onRemovePart = this.OnRemovePart;
			if (onRemovePart != null)
			{
				onRemovePart(part);
			}
			return true;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004DE1C File Offset: 0x0004C01C
		private void Clear()
		{
			this.titleScreen.SetActive(false);
			this.won = false;
			this.dead = false;
			this.Score = 0;
			this.playTick = 0UL;
			this.allCoords.Clear();
			this.foods.Clear();
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				if (part == null)
				{
					this.snake.RemoveAt(i);
				}
				else
				{
					this.RemovePart(part);
				}
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0004DEA8 File Offset: 0x0004C0A8
		private void PunchCamera()
		{
			this.freezeCountDown = 0.10000000149011612;
			this.cameraParent.DOKill(true);
			this.cameraParent.DOShakePosition(0.4f, 1f, 10, 90f, false, true);
			this.cameraParent.DOShakeRotation(0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x04000F49 RID: 3913
		[SerializeField]
		private GameObject gameOverScreen;

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04000F4B RID: 3915
		[SerializeField]
		private GameObject winIndicator;

		// Token: 0x04000F4C RID: 3916
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04000F4D RID: 3917
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x04000F4E RID: 3918
		[SerializeField]
		private GameObject highScoreIndicator;

		// Token: 0x04000F4F RID: 3919
		[SerializeField]
		private TextMeshProUGUI scoreTextGameOver;

		// Token: 0x04000F50 RID: 3920
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x04000F51 RID: 3921
		[SerializeField]
		private float tickIntervalFrom = 0.5f;

		// Token: 0x04000F52 RID: 3922
		[SerializeField]
		private float tickIntervalTo = 0.01f;

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private int maxSpeedTick = 4096;

		// Token: 0x04000F54 RID: 3924
		[SerializeField]
		private AnimationCurve speedCurve;

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		private int borderXMin = -10;

		// Token: 0x04000F56 RID: 3926
		[SerializeField]
		private int borderXMax = 10;

		// Token: 0x04000F57 RID: 3927
		[SerializeField]
		private int borderYMin = -10;

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		private int borderYMax = 10;

		// Token: 0x04000F59 RID: 3929
		private bool playing;

		// Token: 0x04000F5A RID: 3930
		private bool dead;

		// Token: 0x04000F5B RID: 3931
		private bool won;

		// Token: 0x04000F5C RID: 3932
		private List<SnakeForce.Part> snake = new List<SnakeForce.Part>();

		// Token: 0x04000F5D RID: 3933
		private List<Vector2Int> foods = new List<Vector2Int>();

		// Token: 0x04000F5E RID: 3934
		private int _score;

		// Token: 0x04000F5F RID: 3935
		public const string HighScoreKey = "MiniGame/Snake/HighScore";

		// Token: 0x04000F67 RID: 3943
		private float tickETA;

		// Token: 0x04000F68 RID: 3944
		private List<Vector2Int> allCoords = new List<Vector2Int>();

		// Token: 0x04000F69 RID: 3945
		private ulong playTick;

		// Token: 0x04000F6A RID: 3946
		private Vector2Int lastFrameAxis;

		// Token: 0x04000F6B RID: 3947
		private double freezeCountDown;

		// Token: 0x04000F6C RID: 3948
		private bool axisInput;

		// Token: 0x0200055D RID: 1373
		public class Part
		{
			// Token: 0x06002818 RID: 10264 RVA: 0x000935CE File Offset: 0x000917CE
			public Part(SnakeForce master, Vector2Int coord, Vector2Int direction)
			{
				this.Master = master;
				this.coord = coord;
				this.direction = direction;
			}

			// Token: 0x17000761 RID: 1889
			// (get) Token: 0x06002819 RID: 10265 RVA: 0x000935EB File Offset: 0x000917EB
			public bool IsHead
			{
				get
				{
					return this == this.Master.Head;
				}
			}

			// Token: 0x17000762 RID: 1890
			// (get) Token: 0x0600281A RID: 10266 RVA: 0x000935FB File Offset: 0x000917FB
			public bool IsTail
			{
				get
				{
					return this == this.Master.Tail;
				}
			}

			// Token: 0x0600281B RID: 10267 RVA: 0x0009360B File Offset: 0x0009180B
			internal void MoveTo(Vector2Int coord)
			{
				this.coord = coord;
				Action<SnakeForce.Part> onMove = this.OnMove;
				if (onMove == null)
				{
					return;
				}
				onMove(this);
			}

			// Token: 0x140000F7 RID: 247
			// (add) Token: 0x0600281C RID: 10268 RVA: 0x00093628 File Offset: 0x00091828
			// (remove) Token: 0x0600281D RID: 10269 RVA: 0x00093660 File Offset: 0x00091860
			public event Action<SnakeForce.Part> OnMove;

			// Token: 0x04001F14 RID: 7956
			public Vector2Int coord;

			// Token: 0x04001F15 RID: 7957
			public Vector2Int direction;

			// Token: 0x04001F16 RID: 7958
			public readonly SnakeForce Master;
		}
	}
}

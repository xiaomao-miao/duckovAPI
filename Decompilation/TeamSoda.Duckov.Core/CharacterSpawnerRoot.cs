using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Duckov.Weathers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// Token: 0x02000094 RID: 148
public class CharacterSpawnerRoot : MonoBehaviour
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x00016B69 File Offset: 0x00014D69
	public int RelatedScene
	{
		get
		{
			return this.relatedScene;
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00016B74 File Offset: 0x00014D74
	private void Awake()
	{
		if (this.createdCharacters == null)
		{
			this.createdCharacters = new List<CharacterMainControl>();
		}
		if (this.despawningCharacters == null)
		{
			this.despawningCharacters = new List<CharacterMainControl>();
		}
		if (!this.useTimeOfDay && !this.checkWeather)
		{
			this.despawnIfTimingWrong = false;
		}
		if (this.needTrigger && this.trigger)
		{
			this.trigger.triggerOnce = false;
			this.trigger.onlyMainCharacter = true;
			this.trigger.DoOnTriggerEnter.AddListener(new UnityAction(this.DoOnTriggerEnter));
			this.trigger.DoOnTriggerExit.AddListener(new UnityAction(this.DoOnTriggerLeave));
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00016C24 File Offset: 0x00014E24
	private void OnDestroy()
	{
		if (this.needTrigger && this.trigger)
		{
			this.trigger.DoOnTriggerEnter.RemoveListener(new UnityAction(this.DoOnTriggerEnter));
			this.trigger.DoOnTriggerExit.RemoveListener(new UnityAction(this.DoOnTriggerLeave));
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00016C7E File Offset: 0x00014E7E
	private void Start()
	{
		if (LevelManager.Instance && LevelManager.Instance.IsBaseLevel)
		{
			this.minDistanceToPlayer = 0f;
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00016CA4 File Offset: 0x00014EA4
	private void Update()
	{
		if (!this.inited && LevelManager.LevelInited)
		{
			this.Init();
		}
		bool flag = this.CheckTiming();
		if (this.inited && !this.created && flag)
		{
			this.StartSpawn();
		}
		if (this.created && !flag && this.despawnIfTimingWrong)
		{
			this.despawningCharacters.AddRange(this.createdCharacters);
			this.createdCharacters.Clear();
			this.created = false;
		}
		this.despawnTickTimer -= Time.deltaTime;
		if (this.despawnTickTimer < 0f && this.despawnIfTimingWrong && this.despawningCharacters.Count > 0)
		{
			this.CheckDespawn();
		}
		if (this.despawnTickTimer < 0f && !this.allDead && this.stillhasAliveCharacters && !this.allDeadEventInvoked)
		{
			if (this.createdCharacters.Count <= 0)
			{
				this.allDead = true;
			}
			else
			{
				this.allDead = true;
				foreach (CharacterMainControl characterMainControl in this.createdCharacters)
				{
					if (characterMainControl != null && characterMainControl.Health && !characterMainControl.Health.IsDead)
					{
						this.allDead = false;
						break;
					}
				}
			}
			if (this.allDead)
			{
				this.stillhasAliveCharacters = false;
				UnityEvent onAllDeadEvent = this.OnAllDeadEvent;
				if (onAllDeadEvent != null)
				{
					onAllDeadEvent.Invoke();
				}
				this.allDeadEventInvoked = true;
			}
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00016E40 File Offset: 0x00015040
	private void CheckDespawn()
	{
		for (int i = 0; i < this.despawningCharacters.Count; i++)
		{
			CharacterMainControl characterMainControl = this.despawningCharacters[i];
			if (!characterMainControl)
			{
				this.despawningCharacters.RemoveAt(i);
				i--;
			}
			else if (!characterMainControl.gameObject.activeInHierarchy)
			{
				UnityEngine.Object.Destroy(characterMainControl.gameObject);
				this.despawningCharacters.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00016EB4 File Offset: 0x000150B4
	private bool CheckTiming()
	{
		if (LevelManager.Instance == null)
		{
			return false;
		}
		if (this.needTrigger && !this.playerInTrigger)
		{
			return false;
		}
		bool flag;
		if (this.useTimeOfDay)
		{
			float num = (float)GameClock.TimeOfDay.TotalHours % 24f;
			flag = ((num >= this.spawnTimeRangeFrom && num <= this.spawnTimeRangeTo) || (this.spawnTimeRangeTo < this.spawnTimeRangeFrom && (num >= this.spawnTimeRangeFrom || num <= this.spawnTimeRangeTo)));
		}
		else
		{
			flag = (LevelManager.Instance.LevelTime >= this.whenToSpawn);
		}
		bool flag2 = true;
		if (this.checkWeather && !this.targetWeathers.Contains(TimeOfDayController.Instance.CurrentWeather))
		{
			flag2 = false;
		}
		return flag && flag2;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00016F80 File Offset: 0x00015180
	private void Init()
	{
		this.inited = true;
		this.spawnerComponent.Init(this);
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		bool flag = true;
		if (MultiSceneCore.Instance != null)
		{
			flag = MultiSceneCore.Instance.usedCreatorIds.Contains(this.SpawnerGuid);
		}
		if (flag)
		{
			Debug.Log("Contain this spawner");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.relatedScene = SceneManager.GetActiveScene().buildIndex;
		base.transform.SetParent(null);
		MultiSceneCore.MoveToMainScene(base.gameObject);
		MultiSceneCore.Instance.usedCreatorIds.Add(this.SpawnerGuid);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00017030 File Offset: 0x00015230
	private void StartSpawn()
	{
		if (this.created)
		{
			return;
		}
		this.created = true;
		if (UnityEngine.Random.Range(0f, 1f) > this.spawnChance)
		{
			return;
		}
		UnityEvent onStartEvent = this.OnStartEvent;
		if (onStartEvent != null)
		{
			onStartEvent.Invoke();
		}
		if (this.spawnerComponent)
		{
			this.spawnerComponent.StartSpawn();
		}
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0001708E File Offset: 0x0001528E
	private void DoOnTriggerEnter()
	{
		this.playerInTrigger = true;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00017097 File Offset: 0x00015297
	private void DoOnTriggerLeave()
	{
		this.playerInTrigger = false;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000170A0 File Offset: 0x000152A0
	public void AddCreatedCharacter(CharacterMainControl c)
	{
		this.createdCharacters.Add(c);
		this.stillhasAliveCharacters = true;
	}

	// Token: 0x0400047B RID: 1147
	public bool needTrigger;

	// Token: 0x0400047C RID: 1148
	public OnTriggerEnterEvent trigger;

	// Token: 0x0400047D RID: 1149
	private bool playerInTrigger;

	// Token: 0x0400047E RID: 1150
	private bool created;

	// Token: 0x0400047F RID: 1151
	private bool inited;

	// Token: 0x04000480 RID: 1152
	[Range(0f, 1f)]
	public float spawnChance = 1f;

	// Token: 0x04000481 RID: 1153
	public float minDistanceToPlayer = 25f;

	// Token: 0x04000482 RID: 1154
	public bool useTimeOfDay;

	// Token: 0x04000483 RID: 1155
	public float whenToSpawn;

	// Token: 0x04000484 RID: 1156
	[Range(0f, 24f)]
	public float spawnTimeRangeFrom;

	// Token: 0x04000485 RID: 1157
	[Range(0f, 24f)]
	public float spawnTimeRangeTo;

	// Token: 0x04000486 RID: 1158
	[FormerlySerializedAs("despawnIfOutOfTime")]
	public bool despawnIfTimingWrong;

	// Token: 0x04000487 RID: 1159
	public bool checkWeather;

	// Token: 0x04000488 RID: 1160
	public List<Weather> targetWeathers;

	// Token: 0x04000489 RID: 1161
	private int relatedScene = -1;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private CharacterSpawnerComponentBase spawnerComponent;

	// Token: 0x0400048B RID: 1163
	public bool autoRefreshGuid = true;

	// Token: 0x0400048C RID: 1164
	public int SpawnerGuid;

	// Token: 0x0400048D RID: 1165
	private List<CharacterMainControl> createdCharacters = new List<CharacterMainControl>();

	// Token: 0x0400048E RID: 1166
	private List<CharacterMainControl> despawningCharacters = new List<CharacterMainControl>();

	// Token: 0x0400048F RID: 1167
	private float despawnTickTimer = 1f;

	// Token: 0x04000490 RID: 1168
	public UnityEvent OnStartEvent;

	// Token: 0x04000491 RID: 1169
	public UnityEvent OnAllDeadEvent;

	// Token: 0x04000492 RID: 1170
	private bool allDeadEventInvoked;

	// Token: 0x04000493 RID: 1171
	private bool stillhasAliveCharacters;

	// Token: 0x04000494 RID: 1172
	private bool allDead;
}

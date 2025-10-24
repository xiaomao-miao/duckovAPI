using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ParadoxNotion;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class AIMainBrain : MonoBehaviour
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600085C RID: 2140 RVA: 0x0002519C File Offset: 0x0002339C
	private static CharacterMainControl mainCharacter
	{
		get
		{
			if (AIMainBrain._mc == null)
			{
				AIMainBrain._mc = CharacterMainControl.Main;
			}
			return AIMainBrain._mc;
		}
	}

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600085D RID: 2141 RVA: 0x000251BC File Offset: 0x000233BC
	// (remove) Token: 0x0600085E RID: 2142 RVA: 0x000251F0 File Offset: 0x000233F0
	public static event Action<AISound> OnSoundSpawned;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x0600085F RID: 2143 RVA: 0x00025224 File Offset: 0x00023424
	// (remove) Token: 0x06000860 RID: 2144 RVA: 0x00025258 File Offset: 0x00023458
	public static event Action<AISound> OnPlayerHearSound;

	// Token: 0x06000861 RID: 2145 RVA: 0x0002528B File Offset: 0x0002348B
	public static void MakeSound(AISound sound)
	{
		Action<AISound> onSoundSpawned = AIMainBrain.OnSoundSpawned;
		if (onSoundSpawned != null)
		{
			onSoundSpawned(sound);
		}
		AIMainBrain.FilterPlayerHearSound(sound);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x000252A4 File Offset: 0x000234A4
	private static void FilterPlayerHearSound(AISound sound)
	{
		if (!AIMainBrain.mainCharacter)
		{
			return;
		}
		if (!Team.IsEnemy(Teams.player, sound.fromTeam))
		{
			return;
		}
		if (sound.fromCharacter && sound.fromCharacter.characterModel && !sound.fromCharacter.characterModel.Hidden && !GameCamera.Instance.IsOffScreen(sound.pos))
		{
			return;
		}
		float num = Vector3.Distance(sound.pos, AIMainBrain.mainCharacter.transform.position);
		if (AIMainBrain.mainCharacter.SoundVisable < 0.2f)
		{
			return;
		}
		float hearingAbility = AIMainBrain.mainCharacter.HearingAbility;
		if (num > sound.radius * hearingAbility)
		{
			return;
		}
		Action<AISound> onPlayerHearSound = AIMainBrain.OnPlayerHearSound;
		if (onPlayerHearSound == null)
		{
			return;
		}
		onPlayerHearSound(sound);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00025365 File Offset: 0x00023565
	public void Awake()
	{
		this.searchTasks = new Queue<AIMainBrain.SearchTaskContext>();
		this.checkObsticleTasks = new Queue<AIMainBrain.CheckObsticleTaskContext>();
		this.fowBlockLayer = LayerMask.NameToLayer("FowBlock");
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00025390 File Offset: 0x00023590
	private void Start()
	{
		this.dmgReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		this.interactLayers = 1 << LayerMask.NameToLayer("Interactable");
		this.obsticleLayers = GameplayDataSettings.Layers.fowBlockLayers;
		this.obsticleLayersWithThermal = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
		this.cols = new Collider[15];
		this.ObsHits = new RaycastHit[15];
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00025404 File Offset: 0x00023604
	private void Update()
	{
		int num = 0;
		while (num < this.maxSeachCount && this.searchTasks.Count > 0)
		{
			this.DoSearch(this.searchTasks.Dequeue());
			num++;
		}
		int num2 = 0;
		while (num2 < this.maxCheckObsticleCount && this.checkObsticleTasks.Count > 0)
		{
			this.DoCheckObsticle(this.checkObsticleTasks.Dequeue());
			num2++;
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00025474 File Offset: 0x00023674
	private void DoSearch(AIMainBrain.SearchTaskContext context)
	{
		int num = Physics.OverlapSphereNonAlloc(context.searchCenter, context.searchDistance, this.cols, (context.searchPickupID > 0) ? (this.dmgReceiverLayers | this.interactLayers) : this.dmgReceiverLayers, QueryTriggerInteraction.Collide);
		if (num <= 0)
		{
			context.onSearchFinishedCallback(null, null);
			return;
		}
		float num2 = 9999f;
		DamageReceiver arg = null;
		float num3 = 9999f;
		InteractablePickup arg2 = null;
		float num4 = 1.5f;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.cols[i];
			if (Mathf.Abs(context.searchCenter.y - collider.transform.position.y) <= 4f)
			{
				float num5 = Vector3.Distance(context.searchCenter, collider.transform.position);
				if (Vector3.Angle(context.searchDirection.normalized, (collider.transform.position - context.searchCenter).normalized) <= context.searchAngle * 0.5f || num5 <= num4)
				{
					this.dmgReceiverTemp = null;
					float num6 = 1f;
					if (collider.gameObject.IsInLayerMask(this.dmgReceiverLayers))
					{
						this.dmgReceiverTemp = collider.GetComponent<DamageReceiver>();
						if (this.dmgReceiverTemp != null && this.dmgReceiverTemp.health)
						{
							CharacterMainControl characterMainControl = this.dmgReceiverTemp.health.TryGetCharacter();
							if (characterMainControl)
							{
								num6 = characterMainControl.VisableDistanceFactor;
							}
						}
					}
					if (num5 <= context.searchDistance * num6 && (num5 < num2 || num5 < num3) && (!context.checkObsticle || num5 <= num4 || !this.CheckObsticle(context.searchCenter, collider.transform.position + Vector3.up * 1.5f, context.thermalOn, context.ignoreFowBlockLayer)))
					{
						if (this.dmgReceiverTemp)
						{
							if (!(this.dmgReceiverTemp.health == null) && Team.IsEnemy(context.selfTeam, this.dmgReceiverTemp.Team))
							{
								num2 = num5;
								arg = this.dmgReceiverTemp;
							}
						}
						else if (context.searchPickupID > 0)
						{
							InteractablePickup component = collider.GetComponent<InteractablePickup>();
							if (component && component.ItemAgent && component.ItemAgent.Item && component.ItemAgent.Item.TypeID == context.searchPickupID)
							{
								num3 = num5;
								arg2 = component;
							}
						}
					}
				}
			}
		}
		context.onSearchFinishedCallback(arg, arg2);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0002572C File Offset: 0x0002392C
	public void AddSearchTask(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
	{
		AIMainBrain.SearchTaskContext item = new AIMainBrain.SearchTaskContext(center, dir, searchAngle, searchDistance, selfTeam, checkObsticle, thermalOn, ignoreFowBlockLayer, searchPickupID, callback);
		this.searchTasks.Enqueue(item);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00025760 File Offset: 0x00023960
	private void DoCheckObsticle(AIMainBrain.CheckObsticleTaskContext context)
	{
		bool obj = this.CheckObsticle(context.start, context.end, context.thermalOn, context.ignoreFowBlockLayer);
		context.onCheckFinishCallback(obj);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00025798 File Offset: 0x00023998
	public void AddCheckObsticleTask(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> callback)
	{
		AIMainBrain.CheckObsticleTaskContext item = new AIMainBrain.CheckObsticleTaskContext(start, end, thermalOn, ignoreFowBlockLayer, callback);
		this.checkObsticleTasks.Enqueue(item);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x000257C0 File Offset: 0x000239C0
	private bool CheckObsticle(Vector3 startPoint, Vector3 endPoint, bool thermalOn, bool ignoreFowBlockLayer)
	{
		Ray ray = new Ray(startPoint, (endPoint - startPoint).normalized);
		LayerMask mask = thermalOn ? this.obsticleLayersWithThermal : this.obsticleLayers;
		if (ignoreFowBlockLayer)
		{
			mask &= ~(1 << this.fowBlockLayer);
		}
		return Physics.RaycastNonAlloc(ray, this.ObsHits, (endPoint - startPoint).magnitude, mask) > 0;
	}

	// Token: 0x04000790 RID: 1936
	private Queue<AIMainBrain.SearchTaskContext> searchTasks;

	// Token: 0x04000791 RID: 1937
	private Queue<AIMainBrain.CheckObsticleTaskContext> checkObsticleTasks;

	// Token: 0x04000792 RID: 1938
	private LayerMask dmgReceiverLayers;

	// Token: 0x04000793 RID: 1939
	private LayerMask interactLayers;

	// Token: 0x04000794 RID: 1940
	private LayerMask obsticleLayers;

	// Token: 0x04000795 RID: 1941
	private LayerMask obsticleLayersWithThermal;

	// Token: 0x04000796 RID: 1942
	private Collider[] cols;

	// Token: 0x04000797 RID: 1943
	private RaycastHit[] ObsHits;

	// Token: 0x04000798 RID: 1944
	public int maxSeachCount;

	// Token: 0x04000799 RID: 1945
	public int maxCheckObsticleCount;

	// Token: 0x0400079A RID: 1946
	private static CharacterMainControl _mc;

	// Token: 0x0400079D RID: 1949
	private int fowBlockLayer;

	// Token: 0x0400079E RID: 1950
	private DamageReceiver dmgReceiverTemp;

	// Token: 0x02000483 RID: 1155
	public struct SearchTaskContext
	{
		// Token: 0x06002688 RID: 9864 RVA: 0x000887D0 File Offset: 0x000869D0
		public SearchTaskContext(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
		{
			this.searchCenter = center;
			this.searchDirection = dir;
			this.searchAngle = searchAngle;
			this.searchDistance = searchDistance;
			this.selfTeam = selfTeam;
			this.thermalOn = thermalOn;
			this.checkObsticle = checkObsticle;
			this.searchPickupID = searchPickupID;
			this.onSearchFinishedCallback = callback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001B88 RID: 7048
		public Vector3 searchCenter;

		// Token: 0x04001B89 RID: 7049
		public Vector3 searchDirection;

		// Token: 0x04001B8A RID: 7050
		public float searchAngle;

		// Token: 0x04001B8B RID: 7051
		public float searchDistance;

		// Token: 0x04001B8C RID: 7052
		public Teams selfTeam;

		// Token: 0x04001B8D RID: 7053
		public bool checkObsticle;

		// Token: 0x04001B8E RID: 7054
		public bool thermalOn;

		// Token: 0x04001B8F RID: 7055
		public bool ignoreFowBlockLayer;

		// Token: 0x04001B90 RID: 7056
		public int searchPickupID;

		// Token: 0x04001B91 RID: 7057
		public Action<DamageReceiver, InteractablePickup> onSearchFinishedCallback;
	}

	// Token: 0x02000484 RID: 1156
	public struct CheckObsticleTaskContext
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x0008882A File Offset: 0x00086A2A
		public CheckObsticleTaskContext(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> onCheckFinishCallback)
		{
			this.start = start;
			this.end = end;
			this.thermalOn = thermalOn;
			this.onCheckFinishCallback = onCheckFinishCallback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001B92 RID: 7058
		public Vector3 start;

		// Token: 0x04001B93 RID: 7059
		public Vector3 end;

		// Token: 0x04001B94 RID: 7060
		public bool thermalOn;

		// Token: 0x04001B95 RID: 7061
		public bool ignoreFowBlockLayer;

		// Token: 0x04001B96 RID: 7062
		public Action<bool> onCheckFinishCallback;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

// Token: 0x02000071 RID: 113
public class Projectile : MonoBehaviour
{
	// Token: 0x06000439 RID: 1081 RVA: 0x00012CDD File Offset: 0x00010EDD
	public void SetPool(ObjectPool<Projectile> _pool)
	{
		this.pool = _pool;
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00012CE6 File Offset: 0x00010EE6
	private void Release()
	{
		if (this.pool == null)
		{
			Debug.Log("Destroy");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.pool.Release(this);
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00012D12 File Offset: 0x00010F12
	private void Awake()
	{
		if (!this.inited)
		{
			this.inited = true;
			this.Init();
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00012D2C File Offset: 0x00010F2C
	public void Init()
	{
		this.inited = true;
		this.damagedObjects = new List<GameObject>();
		this.damagedObjects.Clear();
		this.traveledDistance = 0f;
		this.dead = false;
		this.overMaxDistance = false;
		this.flyThroughCharacterSoundPlayed = false;
		this.firstFrame = true;
		this.hitLayers = (GameplayDataSettings.Layers.damageReceiverLayerMask | GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask);
		if (this.trail)
		{
			this.trail.Clear();
		}
		if (this.randomRotate)
		{
			this.randomRotate.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		}
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00012E08 File Offset: 0x00011008
	private void Update()
	{
		if (this.dead)
		{
			this.Release();
			return;
		}
		this.UpdateMoveAndCheck();
		if (this.dead)
		{
			if (this.firstFrame && this.trail)
			{
				this.trail.Clear();
			}
			if (this.context.explosionRange > 0f)
			{
				DamageInfo dmgInfo = new DamageInfo(this.context.fromCharacter);
				dmgInfo.damageValue = this.context.explosionDamage;
				dmgInfo.fromWeaponItemID = this.context.fromWeaponItemID;
				dmgInfo.armorPiercing = this.context.armorPiercing;
				LevelManager.Instance.ExplosionManager.CreateExplosion(base.transform.position, this.context.explosionRange, dmgInfo, ExplosionFxTypes.normal, 1f, true);
			}
			this.Release();
		}
		this.UpdateFlyThroughSound();
		this.firstFrame = false;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00012EF0 File Offset: 0x000110F0
	private void UpdateFlyThroughSound()
	{
		if (this.dead)
		{
			return;
		}
		if (this.context.team == Teams.player)
		{
			return;
		}
		if (this.flyThroughCharacterSoundPlayed)
		{
			return;
		}
		if (CharacterMainControl.Main == null)
		{
			return;
		}
		if (this.velocity.magnitude < 9f)
		{
			return;
		}
		Vector3 lhs = CharacterMainControl.Main.transform.position - base.transform.position;
		lhs.y = 0f;
		if (lhs.magnitude > 5f)
		{
			return;
		}
		lhs.Normalize();
		if (Vector3.Dot(lhs, this.velocity) > 0f)
		{
			return;
		}
		this.flyThroughCharacterSoundPlayed = true;
		Action<Vector3> onBulletFlyByCharacter = Projectile.OnBulletFlyByCharacter;
		if (onBulletFlyByCharacter == null)
		{
			return;
		}
		onBulletFlyByCharacter(base.transform.position);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00012FB8 File Offset: 0x000111B8
	public void Init(ProjectileContext _context)
	{
		this.Init();
		this.context = _context;
		this.direction = this.context.direction;
		this.velocity = this.context.speed * this.direction;
		this.gravity = Mathf.Abs(this.context.gravity);
		this.UpdateAimDirection();
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0001301C File Offset: 0x0001121C
	private void UpdateMoveAndCheck()
	{
		if (this.firstFrame)
		{
			this.startPoint = base.transform.position;
		}
		float num = Time.deltaTime;
		if (num > 0.04f)
		{
			num = 0.04f;
		}
		this.velocity.y = this.velocity.y - num * this.gravity;
		this.direction = this.velocity.normalized;
		this.UpdateAimDirection();
		this._distanceThisFrame = this.velocity.magnitude * num;
		if (this._distanceThisFrame + this.traveledDistance > this.context.distance)
		{
			this._distanceThisFrame = this.context.distance - this.traveledDistance;
			this.overMaxDistance = true;
		}
		Vector3 origin = base.transform.position - base.transform.forward * 0.1f;
		if (this.firstFrame && this.context.firstFrameCheck)
		{
			origin = this.context.firstFrameCheckStartPoint;
		}
		this.hits = Physics.SphereCastAll(origin, this.radius, this.direction, this._distanceThisFrame + 0.3f, this.hitLayers, QueryTriggerInteraction.Ignore).ToList<RaycastHit>();
		int count = this.hits.Count;
		if (count > 0)
		{
			this.hits.Sort(delegate(RaycastHit a, RaycastHit b)
			{
				if (a.distance > b.distance)
				{
					return 1;
				}
				return 0;
			});
			for (int i = 0; i < count; i++)
			{
				RaycastHit raycastHit = this.hits[i];
				this.hitPoint = raycastHit.point;
				if (raycastHit.distance <= 0f)
				{
					this.hitPoint = raycastHit.collider.transform.position;
				}
				if (!this.damagedObjects.Contains(this.hits[i].collider.gameObject) && (!this.context.ignoreHalfObsticle || !GameplayDataSettings.LayersData.IsLayerInLayerMask(this.hits[i].collider.gameObject.layer, GameplayDataSettings.Layers.halfObsticleLayer)))
				{
					this.damagedObjects.Add(this.hits[i].collider.gameObject);
					if ((GameplayDataSettings.Layers.damageReceiverLayerMask & 1 << this.hits[i].collider.gameObject.layer) != 0)
					{
						this._dmgReceiverTemp = this.hits[i].collider.GetComponent<DamageReceiver>();
						if (this._dmgReceiverTemp.Team == this.context.team)
						{
							goto IL_5FA;
						}
						if (this._dmgReceiverTemp.isHalfObsticle && this.context.ignoreHalfObsticle)
						{
							goto IL_5FA;
						}
					}
					else
					{
						this._dmgReceiverTemp = null;
					}
					if (this._dmgReceiverTemp)
					{
						bool flag = true;
						if (this._dmgReceiverTemp.Team == this.context.team)
						{
							flag = false;
						}
						else if (this._dmgReceiverTemp.health)
						{
							CharacterMainControl characterMainControl = this._dmgReceiverTemp.health.TryGetCharacter();
							if (characterMainControl && this._dmgReceiverTemp.health.TryGetCharacter().Dashing)
							{
								flag = false;
							}
							else if (characterMainControl && characterMainControl == this.context.fromCharacter)
							{
								flag = false;
							}
						}
						if (flag)
						{
							DamageInfo damageInfo = new DamageInfo(this.context.fromCharacter);
							damageInfo.damageValue = this.context.damage;
							if (this.context.halfDamageDistance > 0f && Vector3.Distance(this.startPoint, this.hitPoint) > this.context.halfDamageDistance)
							{
								damageInfo.damageValue *= 0.5f;
							}
							damageInfo.critDamageFactor = this.context.critDamageFactor;
							damageInfo.critRate = this.context.critRate;
							damageInfo.armorPiercing = this.context.armorPiercing;
							damageInfo.armorBreak = this.context.armorBreak;
							damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.physics, this.context.element_Physics));
							damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.fire, this.context.element_Fire));
							damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.poison, this.context.element_Poison));
							damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.electricity, this.context.element_Electricity));
							damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.space, this.context.element_Space));
							damageInfo.damagePoint = this.hitPoint;
							damageInfo.buffChance = this.context.buffChance;
							damageInfo.buff = this.context.buff;
							damageInfo.bleedChance = this.context.bleedChance;
							damageInfo.damageType = DamageTypes.normal;
							damageInfo.fromWeaponItemID = this.context.fromWeaponItemID;
							damageInfo.damageNormal = raycastHit.normal.normalized;
							this._dmgReceiverTemp.Hurt(damageInfo);
							this._dmgReceiverTemp.AddBuff(GameplayDataSettings.Buffs.Pain, this.context.fromCharacter);
							this.context.penetrate = this.context.penetrate - 1;
							if (this.context.penetrate < 0)
							{
								base.transform.position = this.hitPoint;
								this.dead = true;
								break;
							}
						}
					}
					else
					{
						this.dead = true;
						base.transform.position = this.hitPoint;
						Vector3 normal = raycastHit.normal;
						if (this.hitFx)
						{
							UnityEngine.Object.Instantiate<GameObject>(this.hitFx, this.hitPoint, Quaternion.LookRotation(normal, Vector3.up));
							break;
						}
						UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.BulletHitObsticleFx, this.hitPoint, Quaternion.LookRotation(normal, Vector3.up));
						break;
					}
				}
				IL_5FA:;
			}
		}
		if (this.overMaxDistance)
		{
			this.dead = true;
		}
		if (!this.dead)
		{
			base.transform.position += this.direction * this._distanceThisFrame;
			this.traveledDistance += this._distanceThisFrame;
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0001367F File Offset: 0x0001187F
	private void UpdateAimDirection()
	{
		base.transform.rotation = Quaternion.LookRotation(this.direction, Vector3.up);
	}

	// Token: 0x04000358 RID: 856
	public ProjectileContext context;

	// Token: 0x04000359 RID: 857
	public float radius;

	// Token: 0x0400035A RID: 858
	private float traveledDistance;

	// Token: 0x0400035B RID: 859
	private List<RaycastHit> hits;

	// Token: 0x0400035C RID: 860
	private LayerMask hitLayers;

	// Token: 0x0400035D RID: 861
	private Vector3 hitPoint;

	// Token: 0x0400035E RID: 862
	private Vector3 hitNormal;

	// Token: 0x0400035F RID: 863
	private bool dead;

	// Token: 0x04000360 RID: 864
	private bool overMaxDistance;

	// Token: 0x04000361 RID: 865
	[SerializeField]
	private GameObject hitFx;

	// Token: 0x04000362 RID: 866
	private Vector3 direction;

	// Token: 0x04000363 RID: 867
	private Vector3 velocity;

	// Token: 0x04000364 RID: 868
	private float gravity;

	// Token: 0x04000365 RID: 869
	[HideInInspector]
	public List<GameObject> damagedObjects;

	// Token: 0x04000366 RID: 870
	public static Action<Vector3> OnBulletFlyByCharacter;

	// Token: 0x04000367 RID: 871
	private bool flyThroughCharacterSoundPlayed;

	// Token: 0x04000368 RID: 872
	private bool firstFrame = true;

	// Token: 0x04000369 RID: 873
	private Vector3 startPoint;

	// Token: 0x0400036A RID: 874
	private ObjectPool<Projectile> pool;

	// Token: 0x0400036B RID: 875
	[SerializeField]
	private TrailRenderer trail;

	// Token: 0x0400036C RID: 876
	[FormerlySerializedAs("spin")]
	public Transform randomRotate;

	// Token: 0x0400036D RID: 877
	private bool inited;

	// Token: 0x0400036E RID: 878
	private DamageReceiver _dmgReceiverTemp;

	// Token: 0x0400036F RID: 879
	private float _distanceThisFrame;

	// Token: 0x04000370 RID: 880
	private int _hitCount;
}

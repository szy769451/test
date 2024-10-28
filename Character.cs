using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件監聽")]
    public VoidEventSO newGameEvent;
    //private Recharge recharge;

    [Header("基本屬性")]
    public float maxHealth;
    public float currentHealth;
    public float maxRecharge;
    public float currentRecharge;

    [Header("受傷無敵")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    [Header("治療CD")]
    public float treatDuration;
    private float invulnerableCD;
    public bool treatCD;

    public UnityEvent<Character> OnRechargeChange;
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    //public UnityEvent<Transform> OnTakeTreat;
    public UnityEvent UnL;

    private void Start()
    {
        Recharge.Instance.OnRechargeChange += TakeRecharge;
    }
    private void NewGame()
    {
        if (!CompareTag("Player"))
        {
        currentHealth = maxHealth;
        }
        currentRecharge = 0;
        OnHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
        if (treatCD)
        {
            invulnerableCD -= Time.deltaTime;
            if (invulnerableCD <= 0)
            {
                treatCD = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if (currentHealth - attacker.damage > 0)
        {
            //attacker.currentRecharge + 1;
            //Debug.Log(attacker.damage);
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //受傷
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            //觸發死亡
            currentHealth = 0;
            UnL?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    /// <summary>
    /// 觸發受傷無敵
    /// </summary>
    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public void TakeTreat(Treat treater)
    {
        if (treatCD)
            return;
        if (currentHealth - treater.addHealth > 0 && currentHealth < maxHealth)
        {
            currentHealth += treater.addHealth;
            TriggerTreat();
            //受傷
            //OnTakeTreat?.Invoke(treater.transform);
        }
        OnHealthChange?.Invoke(this);
    }
    private void TriggerTreat()
    {
        if (!treatCD)
        {
            treatCD = true;
            invulnerableCD = treatDuration;
        }
    }
    public void TakeRecharge(int newRecharge)
    {
        if (currentRecharge < maxRecharge)
        {
            currentRecharge += newRecharge;
        }
        OnRechargeChange?.Invoke(this);
    }

    public DataDefinition GatDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GatDataID().ID))
        {
            data.characterPosDict[GatDataID().ID] = transform.position;
            data.floatSaveData[GatDataID().ID + "health"] = this.currentHealth;
            data.floatSaveData[GatDataID().ID + "recharge"] = this.currentRecharge;
        }
        else
        {
            data.characterPosDict.Add(GatDataID().ID, transform.position);
            data.floatSaveData.Add(GatDataID().ID + "health", this.currentHealth);
            data.floatSaveData.Add(GatDataID().ID + "recharge", this.currentRecharge);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GatDataID().ID))
        {
            transform.position = data.characterPosDict[GatDataID().ID];
            this.currentHealth = data.floatSaveData[GatDataID().ID + "health"];
            this.currentRecharge = data.floatSaveData[GatDataID().ID + "recharge"];

            OnHealthChange?.Invoke(this);
        }
    }
}

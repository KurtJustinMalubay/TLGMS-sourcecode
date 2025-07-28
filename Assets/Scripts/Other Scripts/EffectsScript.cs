using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EffectsScript : MonoBehaviour
{
    public Slider slider;
    private AudioSource[] audioSource;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    public playerScript playerS;
    public GameObject explosion;
    public static EffectsScript instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int poolSize = 10;
    private float counter = 4f;
    private bool slowTime = false;
    private bool coolDown = true;
    private float coolDownTime = 12f;
    private float startFixedDeltaTime;
    private float slowMotionTS = 0.5f;
    [NonSerialized]public bool isSlow = false;
    [NonSerialized] public bool slowButtonHeld = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(explosion);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        startFixedDeltaTime = Time.fixedDeltaTime;
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out vignette);
        slider.maxValue = 12f;
    }
    private void FixedUpdate()
    {
        BasicLogic();
    }
    private void BasicLogic()
    {
        slider.value = coolDownTime;
        if ((Input.GetKey(KeyCode.E) || slowButtonHeld) && !coolDown && counter == 4f)
        {
            slowTime = true;
            coolDown = true;
            isSlow = true;
        }
        if (slowTime)
        {
            CounterFunct();
            sloMo();
        }
        if (coolDown && !slowTime)
        {
            coolDownTime -= Time.fixedDeltaTime;
            if (coolDownTime <= 0)
            {
                coolDown = false;
            }
        }
    }
    public void SlowButton()
    {
        slowButtonHeld = true;
        StartCoroutine(buttonStop());
    }
    IEnumerator buttonStop()
    {
        yield return null;
        slowButtonHeld = false;
    }
    private void CounterFunct()
    {
        counter -= Time.unscaledDeltaTime;
        if (counter <= 0)
        {
            counter = 4f;
            slowTime = false;
        }
    }

    private void normal()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = startFixedDeltaTime;
        foreach (AudioSource source in audioSource){
            source.pitch = 1f;
        }
        isSlow = false;
        colorAdjustments.saturation.value = 0f;
        vignette.intensity.value = 0f;
    }
    private void sloMo()
    {
        Time.timeScale = slowMotionTS;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTS;
        foreach (AudioSource source in audioSource)
        {
            source.pitch = slowMotionTS;
        }
        colorAdjustments.saturation.value = -67f;
        vignette.intensity.value = 0.429f;
        if (!slowTime)
        {
            normal();
            coolDownTime = 12f;
        }
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }
    public void SpawnExplosion(Vector2 pos)
    {
        GameObject explosion = GetPooledObject();
        if (explosion != null)
        {
            explosion.transform.position = pos;
            explosion.SetActive(true);
        }
    }
}
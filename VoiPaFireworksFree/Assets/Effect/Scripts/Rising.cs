﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Rising : MonoBehaviour{
    /* Properties */
    // GameObjects
    public GameObject StarObjects;
    // Normal Stars
    public GameObject BotanStar;
    public GameObject KikuStar;
    public GameObject YashiStar;
    public GameObject KamuroStar;
    public GameObject YnaagiStar;
    // Sparse Stars
    public GameObject BotanSparseStar;
    public GameObject KikuSparseStar;
    public GameObject YashiSparseStar;
    public GameObject KamuroSparseStar;
    public GameObject YanagiSparseStar;
    // Senrin Stars
    public GameObject BotanSenrinStar;
    public GameObject KikuSenrinStar;
    public GameObject YashiSenrinStar;
    public GameObject KamuroSenrinStar;
    public GameObject YanagiSenrinStar;
    public GameObject HachiSenrinStar;
    // Poka Stars
    public GameObject BotanPokaStar;
    public GameObject KikuPokaStar;
    public GameObject YashiPokaStar;
    public GameObject KamuroPokaStar;
    public GameObject YanagiPokaStar;

    // Public
    // Common Setting Parameter
    public int OpenVelocityThreshold = 5;
    public float DestroyDelay = 0.5f;
    public float SpeedCoefficient = 120.0f;
    public float SpeedMin = 20.0f;
    public float SpeedMax = 50.0f;
    public float ColorBlending = 0.33f;
    // Individual Setting Parameter
    public float PokaStarSpeedCoefficient = 0.25f;
    // Externally Accesable Parameter
    public List<Tone> ToneList = new List<Tone>();
    public float ToneCountValue = 0f;
    public string Type = Constant.FIREWORKS_RISING_TYPE_NORMAL;

    // Private
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;
    private bool isOpend = false;

    // Start is called before the first frame update
    void Start(){
        _particleSystem = this.gameObject.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[_particleSystem.emission.burstCount];
    }

    // Update is called once per frame
    void Update(){
        if (_particleSystem.particleCount > 0) {
            // Get Particle
            _particleSystem.GetParticles(particles);
            // Velocity
            float velocity_y = particles[0].velocity.y;
            // Particle Position
            Vector3 position = particles[0].position;
            // Open Fireworks !!!
            if (!isOpend && ToneList.Any() && velocity_y < 0) {
                // Ref Speed from Volume
                float volume = ToneList.Select(x => x.volume).Max();
                float speedRef = SpeedCoefficient * volume;
                // Mean Count
                int countMean = (int)ToneList.Select(x => x.count).Average();
                // This Tone's Stars
                for (int i = 0; i < ToneList.Count; i++) {
                    float speed = speedRef * (float)(i + 1) / (float)(ToneList.Count);
                    speed = Mathf.Clamp(speed, SpeedMin, SpeedMax);
                    // Open by Type
                    switch (Type) {
                        // Normal
                        case Constant.FIREWORKS_RISING_TYPE_NORMAL:
                            Open(position, ToneList[i], speed);
                            break;
                        // Sparese
                        case Constant.FIREWORKS_RISING_TYPE_SPARSE:
                            OpenSparse(position, ToneList[i], speed);
                            break;
                        // Senrin
                        case Constant.FIREWORKS_RISING_TYPE_SENRIN:
                            OpenSenrin(position, ToneList[i], speed, ToneCountValue);
                            break;
                        // Poka
                        case Constant.FIREWORKS_RISING_TYPE_POKA:
                            OpenPoka(position, ToneList[i], speed);
                            break;
                        default:
                            Debug.Log("Error : No Fireworks type in Constant.FIREWORKS_TYPE:" + Type);
                            break;
                    }
                }
            }
        }
    }

    public void Init(){
        // Open init
        isOpend = false;
        // Type init
        Type = Constant.FIREWORKS_RISING_TYPE_NORMAL;
        // Tone init
        ToneList.Clear();
        ToneCountValue = 0f;
        // Unity Start init
        Start();
    }

    // Open FireWorks
    void Open(Vector3 position, Tone tone, float startSpeed) {
        // Instantiate Star
        GameObject obj = null;
        // Star :  by Tone Count
       if (tone.count < 10){
            // First Search Stanby Star
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_BOTAN_STAR_STANBY);
            if (obj == null) {
                // Instantiate New Star
                obj = GameObject.Instantiate(BotanStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 20){
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKU_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 30){
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHI_STAR_STANBY);
            if (obj == null){
                obj = GameObject.Instantiate(YashiStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if(tone.count < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMURO_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGI_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YnaagiStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        // Change Tag to Updating
        obj.tag = Constant.TAG_FIREWORKS_UPDATING;

        // Particle System Transform
        var transform = obj.GetComponent<Transform>();
        transform.position = position;
        transform.rotation = UnityEngine.Random.rotation;

        // Particle System of Star
        var particleSystem = obj.GetComponent<ParticleSystem>();

        // Particle Sysytem Main Module
        var main = particleSystem.main;
        // Start Color
        Color myColor = Colors.getToneColor(tone.number);
        Color refColor = Colors.getToneColor(tone.chordRef);
        main.startColor = (1.0f - ColorBlending) * myColor + ColorBlending * refColor;
        // Start Speed
        main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed, startSpeed * 1.2f);
        // Start Size
        float startSize = Mathf.Sqrt(tone.volume * 3f);
        main.startSize = new ParticleSystem.MinMaxCurve(startSize, startSize * 1.2f);

        // Saki : Star Material by Chord
        var textureSheetAnimation = particleSystem.textureSheetAnimation;
        // Minor => Sazanami
        if (tone.chordID == Constant.CHORD_ID_MINOR) {
            textureSheetAnimation.rowIndex = 1;
        }else if(tone.chordID == Constant.CHORD_ID_DIMINISHED || tone.chordID == Constant.CHORD_ID_AUGMENTED){
        }
        // Other => Normal Star
        else {
            textureSheetAnimation.rowIndex = 0;
        }

        // Emit
        var count = particleSystem.emission.GetBurst(0).count.constant;
        particleSystem.Emit((int)count);

        // Opend
        isOpend = true;

        // Stop this Rising
        _particleSystem.Clear();
        _particleSystem.Stop();
        _particleSystem.tag = Constant.TAG_RISING_STANBY;
    }

    // Open Sparse Star
    void OpenSparse(Vector3 position, Tone tone, float startSpeed) {
        // Instantiate Star
        GameObject obj = null;
        // Star :  by Tone Count
        if (tone.count < 10) {
            // First Search Stanby Star
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_BOTAN_SPARSE_STAR_STANBY);
            if (obj == null) {
                // Instantiate New Star
                obj = GameObject.Instantiate(BotanSparseStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 20) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKU_SPARSE_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuSparseStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 30) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHI_SPARSE_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YashiSparseStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMURO_SPARSE_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroSparseStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGI_SPARSE_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YanagiSparseStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        // Change Tag to Updating
        obj.tag = Constant.TAG_FIREWORKS_UPDATING;

        // Particle System Transform
        var transform = obj.GetComponent<Transform>();
        transform.position = position;
        transform.rotation = UnityEngine.Random.rotation;

        // Particle System of Star
        var particleSystem = obj.GetComponent<ParticleSystem>();

        // Particle Sysytem Main Module
        var main = particleSystem.main;
        // Start Color
        Color myColor = Colors.getToneColor(tone.number);
        Color refColor = Colors.getToneColor(tone.chordRef);
        main.startColor = (1.0f - ColorBlending) * myColor + ColorBlending * refColor;
        // Start Speed
        main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed, startSpeed * 1.2f);
        // Start Size
        float startSize = 1.33f * Mathf.Sqrt(tone.volume * 3f);
        main.startSize = new ParticleSystem.MinMaxCurve(startSize, startSize * 1.2f);

        // Saki : Star Material by Chord
        var textureSheetAnimation = particleSystem.textureSheetAnimation;
        // Minor => Sazanami
        if (tone.chordID == Constant.CHORD_ID_MINOR) {
            textureSheetAnimation.rowIndex = 1;
        }
        else if (tone.chordID == Constant.CHORD_ID_DIMINISHED || tone.chordID == Constant.CHORD_ID_AUGMENTED) {
        }
        // Other => Normal Star
        else {
            textureSheetAnimation.rowIndex = 0;
        }

        // Emit
        var count = particleSystem.emission.GetBurst(0).count.constant;
        particleSystem.Emit((int)count);

        // Opend
        isOpend = true;

        // Stop this Rising
        _particleSystem.Clear();
        _particleSystem.Stop();
        _particleSystem.tag = Constant.TAG_RISING_STANBY;
    }

    // Open Senrin FireWorks
    void OpenSenrin(Vector3 position, Tone tone, float startSpeed, float count_value = 0f) {
        // count
        float countValue = count_value;
        // Instantiate Star
        GameObject obj = null;
        // Star :  by Tone Count
        if (countValue < 24) {
            // First Search Stanby Star
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_BOTANSENRIN_STANBY);
            if (obj == null) {
                // Instantiate New Star
                obj = GameObject.Instantiate(BotanSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (countValue < 28) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKUSENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (countValue < 32) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_HACHISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(HachiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (countValue < 36) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YashiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (countValue < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMUROSENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YanagiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        // Change Tag to Updationg
        obj.tag = Constant.TAG_FIREWORKS_UPDATING;

        // Particle System Transform
        var transform = obj.GetComponent<Transform>();
        transform.position = position;
        transform.rotation = UnityEngine.Random.rotation;

        // Particle System of Star
        var particleSystem = obj.GetComponent<ParticleSystem>();

        // Particle Sysytem Main Module
        var main = particleSystem.main;
        // Start Color
        Color myColor = Colors.getToneColor(tone.number);
        Color refColor = Colors.getToneColor(tone.chordRef);
        main.startColor = (1.0f - ColorBlending) * myColor + ColorBlending * refColor;
        // Start Speed
        main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed, startSpeed * 1.2f);

        // Emit
        var count = particleSystem.emission.GetBurst(0).count.constant;
        particleSystem.Emit((int)count);

        // Opend
        isOpend = true;

        // Stop this Rising
        _particleSystem.Clear();
        _particleSystem.Stop();
        _particleSystem.tag = Constant.TAG_RISING_STANBY;
    }

    // Open Poka FireWorks
    void OpenPoka(Vector3 position, Tone tone, float start_speed) {
        // Instantiate Star
        GameObject obj = null;
        // Star :  by Tone Count
        if (tone.count < 10) {
            // First Search Stanby Star
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_BOTAN_POKA_STAR_STANBY);
            if (obj == null) {
                // Instantiate New Star
                obj = GameObject.Instantiate(BotanPokaStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 20) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKU_POKA_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuPokaStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 30) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHI_POKA_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YashiPokaStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else if (tone.count < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMURO_POKA_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroPokaStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGI_POKA_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YanagiPokaStar, position, Quaternion.identity, StarObjects.transform);
            }
        }
        // Change Tag to Updating
        obj.tag = Constant.TAG_FIREWORKS_UPDATING;

        // Particle System Transform
        var transform = obj.GetComponent<Transform>();
        transform.position = position;
        transform.rotation = UnityEngine.Random.rotation;

        // Particle System of Star
        var particleSystem = obj.GetComponent<ParticleSystem>();

        // Particle Sysytem Main Module
        var main = particleSystem.main;
        // Start Color
        Color myColor = Colors.getToneColor(tone.number);
        Color refColor = Colors.getToneColor(tone.chordRef);
        main.startColor = (1.0f - ColorBlending) * myColor + ColorBlending * refColor;
        // Start Speed
        float startSpeed = start_speed * PokaStarSpeedCoefficient;
        main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed, startSpeed * 1.2f);
        // Start Size
        float startSize = Mathf.Sqrt(tone.volume * 3f);
        main.startSize = new ParticleSystem.MinMaxCurve(startSize, startSize * 1.2f);

        // Saki : Star Material by Chord
        var textureSheetAnimation = particleSystem.textureSheetAnimation;
        // Minor => Sazanami
        if (tone.chordID == Constant.CHORD_ID_MINOR) {
            textureSheetAnimation.rowIndex = 1;
        }
        else if (tone.chordID == Constant.CHORD_ID_DIMINISHED || tone.chordID == Constant.CHORD_ID_AUGMENTED) {
        }
        // Other => Normal Star
        else {
            textureSheetAnimation.rowIndex = 0;
        }

        // Emit
        var count = particleSystem.emission.GetBurst(0).count.constant;
        particleSystem.Emit((int)count);

        // Opend
        isOpend = true;

        // Stop this Rising
        _particleSystem.Clear();
        _particleSystem.Stop();
        _particleSystem.tag = Constant.TAG_RISING_STANBY;
    }
}

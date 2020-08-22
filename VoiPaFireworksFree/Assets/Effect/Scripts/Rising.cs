using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Rising : MonoBehaviour{
    /* Properties */
    // GameObjects
    public GameObject StarObjects;
    // Stars
    // Dim 1
    public GameObject BotanStar1;
    public GameObject KikuStar1;
    public GameObject YashiStar1;
    public GameObject KamuroStar1;
    public GameObject YanagiStar1;
    // Dim 2
    public GameObject BotanStar2;
    public GameObject KikuStar2;
    public GameObject YashiStar2;
    public GameObject KamuroStar2;
    public GameObject YanagiStar2;
    // Dim 3
    public GameObject BotanStar3;
    public GameObject KikuStar3;
    public GameObject YashiStar3;
    public GameObject KamuroStar3;
    public GameObject YanagiStar3;
    // Dim 4
    public GameObject BotanStar4;
    public GameObject KikuStar4;
    public GameObject YashiStar4;
    public GameObject KamuroStar4;
    public GameObject YanagiStar4;
    // Dim 5
    public GameObject BotanStar5;
    public GameObject KikuStar5;
    public GameObject YashiStar5;
    public GameObject KamuroStar5;
    public GameObject YanagiStar5;
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
    public float StartSizeCoefficient = 3;
    public float ColorBlending = 0.33f;
    // Individual Setting Parameter
    public float PokaStarSpeedCoefficient = 0.25f;
    // Tone Number Range : MIDI Note Number 
    public int ToneNumberRange1Max = 47;
    public int ToneNumberRange2Max = 59;
    public int ToneNumberRange3Max = 71;
    public int ToneNumberRange4Max = 83;
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
        // Get Particle
        _particleSystem = this.gameObject.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[_particleSystem.emission.burstCount];
        // Change Rising Parameter
        if (ToneList.Any()) {
            // Size
            float volumeMax = ToneList.Select(x => x.volume).Max();
            float startSize = volumeMax * StartSizeCoefficient;
            var main = _particleSystem.main;
            main.startSize = new ParticleSystem.MinMaxCurve(startSize, startSize * 1.2f);
        }
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
                float volumeMax = ToneList.Select(x => x.volume).Max();
                float speedRef = SpeedCoefficient * volumeMax;
                // Mean Count
                int countMean = (int)ToneList.Select(x => x.count).Average();
                // Open This Tone's Stars
                for (int i = 0; i < ToneList.Count; i++) {
                    // Speed
                    float speed = speedRef * (float)(i + 1) / (float)(ToneList.Count);
                    speed = Mathf.Clamp(speed, SpeedMin, SpeedMax);
                    // Open by Type
                    switch (Type) {
                        // Normal
                        case Constant.FIREWORKS_RISING_TYPE_NORMAL:
                            Open(position, ToneList[i], speed);
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
                // Opend
                isOpend = true;

                // Stop this Rising
                _particleSystem.Clear();
                _particleSystem.Stop();
                gameObject.tag = Constant.TAG_RISING_STANBY;
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
        // Star Type by Tone Count and Number
        // Botan Star
       if (tone.count < 10){
            // Tone Number Range 1
            if (tone.number <= ToneNumberRange1Max) {
                // First Search Stanby Star
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_BOTAN_1);
                if (obj == null) {
                    // Instantiate New Star
                    obj = GameObject.Instantiate(BotanStar1, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 2
            else if (tone.number <= ToneNumberRange2Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_BOTAN_2);
                if (obj == null) {
                    obj = GameObject.Instantiate(BotanStar2, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 3
            else if (tone.number <= ToneNumberRange3Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_BOTAN_3);
                if (obj == null) {
                    obj = GameObject.Instantiate(BotanStar3, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 4
            else if (tone.number <= ToneNumberRange4Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_BOTAN_4);
                if (obj == null) {
                    obj = GameObject.Instantiate(BotanStar4, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 5
            else {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_BOTAN_5);
                if (obj == null) {
                    obj = GameObject.Instantiate(BotanStar5, position, Quaternion.identity, StarObjects.transform);
                }
            }
        }
       // Kiku Star
        else if (tone.count < 20){
            // Tone Number Range 1
            if (tone.number <= ToneNumberRange1Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KIKU_1);
                if (obj == null) {
                    obj = GameObject.Instantiate(KikuStar1, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 2
            else if (tone.number <= ToneNumberRange2Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KIKU_2);
                if (obj == null) {
                    obj = GameObject.Instantiate(KikuStar2, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 3
            else if (tone.number <= ToneNumberRange3Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KIKU_3);
                if (obj == null) {
                    obj = GameObject.Instantiate(KikuStar3, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 4
            else if (tone.number <= ToneNumberRange4Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KIKU_4);
                if (obj == null) {
                    obj = GameObject.Instantiate(KikuStar4, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 5
            else {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KIKU_5);
                if (obj == null) {
                    obj = GameObject.Instantiate(KikuStar5, position, Quaternion.identity, StarObjects.transform);
                }
            }
        }
       // Yashi Star
        else if (tone.count < 30){
            // Tone Number Range 1
            if (tone.number <= ToneNumberRange1Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YASHI_1);
                if (obj == null) {
                    obj = GameObject.Instantiate(YashiStar1, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 2
            else if (tone.number <= ToneNumberRange2Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YASHI_2);
                if (obj == null) {
                    obj = GameObject.Instantiate(YashiStar2, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 3
            else if (tone.number <= ToneNumberRange3Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YASHI_3);
                if (obj == null) {
                    obj = GameObject.Instantiate(YashiStar3, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 4
            else if (tone.number <= ToneNumberRange4Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YASHI_4);
                if (obj == null) {
                    obj = GameObject.Instantiate(YashiStar4, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 5
            else {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YASHI_5);
                if (obj == null) {
                    obj = GameObject.Instantiate(YashiStar5, position, Quaternion.identity, StarObjects.transform);
                }
            }
        }
       // Kamuro Star
        else if(tone.count < 40) {
            // Tone Number Range 1
            if (tone.number <= ToneNumberRange1Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KAMURO_1);
                if (obj == null) {
                    obj = GameObject.Instantiate(KamuroStar1, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 2
            else if (tone.number <= ToneNumberRange2Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KAMURO_2);
                if (obj == null) {
                    obj = GameObject.Instantiate(KamuroStar2, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 3
            else if (tone.number <= ToneNumberRange3Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KAMURO_3);
                if (obj == null) {
                    obj = GameObject.Instantiate(KamuroStar3, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 4
            else if (tone.number <= ToneNumberRange4Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KAMURO_4);
                if (obj == null) {
                    obj = GameObject.Instantiate(KamuroStar4, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 5
            else {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_KAMURO_5);
                if (obj == null) {
                    obj = GameObject.Instantiate(KamuroStar5, position, Quaternion.identity, StarObjects.transform);
                }
            }
        }
       // Yanagi Star
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGI_STAR_STANBY);
            // Tone Number Range 1
            if (tone.number <= ToneNumberRange1Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YANAGI_1);
                if (obj == null) {
                    obj = GameObject.Instantiate(YanagiStar1, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 2
            else if (tone.number <= ToneNumberRange2Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YANAGI_2);
                if (obj == null) {
                    obj = GameObject.Instantiate(YanagiStar2, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 3
            else if (tone.number <= ToneNumberRange3Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YANAGI_3);
                if (obj == null) {
                    obj = GameObject.Instantiate(YanagiStar3, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 4
            else if (tone.number <= ToneNumberRange4Max) {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YANAGI_4);
                if (obj == null) {
                    obj = GameObject.Instantiate(YanagiStar4, position, Quaternion.identity, StarObjects.transform);
                }
            }
            // Tone Number Range 5
            else {
                obj = GameObject.FindGameObjectWithTag(Constant.FIREWORKS_STAR_ID_YANAGI_5);
                if (obj == null) {
                    obj = GameObject.Instantiate(YanagiStar5, position, Quaternion.identity, StarObjects.transform);
                }
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
        }
        // Other => Normal Star
        else {
            textureSheetAnimation.rowIndex = 0;
        }

        // Emit Particle
        var count = particleSystem.emission.GetBurst(0).count.constant;
        particleSystem.Emit((int)count);
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
    }
}

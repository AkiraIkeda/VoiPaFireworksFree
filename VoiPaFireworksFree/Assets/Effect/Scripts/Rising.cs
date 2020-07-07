using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Rising : MonoBehaviour{
    /* Properties */
    // MonoBehavior
    public Material materialStar;
    public Material materialSazanami;

    // GameObjects
    public GameObject StarObjects;
    // Normal Stars
    public GameObject BotanStar;
    public GameObject KikuStar;
    public GameObject YashiStar;
    public GameObject KamuroStar;
    public GameObject YnaagiStar;
    // Senrin Stars
    public GameObject BotanSenrinStar;
    public GameObject KikuSenrinStar;
    public GameObject YashiSenrinStar;
    public GameObject KamuroSenrinStar;
    public GameObject YanagiSenrinStar;
    public GameObject HachiSenrinStar;

    // Public
    public int OpenVelocityThreshold = 5;
    public float DestroyDelay = 0.5f;
    public float SpeedCoefficient = 120.0f;
    public float SpeedMin = 20.0f;
    public float SpeedMax = 50.0f;
    public float ColorBlending = 0.33f;
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
                        case Constant.FIREWORKS_RISING_TYPE_NORMAL:
                            Open(position, ToneList[i], speed);
                            break;
                        case Constant.FIREWORKS_RISING_TYPE_SENRIN:
                            OpenSenrin(position, ToneList[i], speed, ToneCountValue);
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
            // Change Tag
            obj.tag = Constant.TAG_BOTAN_STAR_UPDATING;
        }
        else if (tone.count < 20){
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKU_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_KIKU_STAR_UPDATING;
        }
        else if (tone.count < 30){
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHI_STAR_STANBY);
            if (obj == null){
                obj = GameObject.Instantiate(YashiStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_YASHI_STAR_UPDATING;
        }
        else if(tone.count < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMURO_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_KAMURO_STAR_UPDATING;
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGI_STAR_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YnaagiStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_YANAGI_STAR_UPDATING;
        }

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
            // Change Tag
            obj.tag = Constant.TAG_BOTANSENRIN_UPDATING;
        }
        else if (countValue < 28) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKUSENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KikuSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_KIKUSENRIN_UPDATING;
        }
        else if (countValue < 32) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_HACHISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(HachiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_HACHISENRIN_UPDATING;
        }
        else if (countValue < 36) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YashiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_YASHISENRIN_UPDATING;
        }
        else if (countValue < 40) {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_KAMUROSENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(KamuroSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_KAMUROSENRIN_UPDATING;
        }
        else {
            obj = GameObject.FindGameObjectWithTag(Constant.TAG_YANAGISENRIN_STANBY);
            if (obj == null) {
                obj = GameObject.Instantiate(YanagiSenrinStar, position, Quaternion.identity, StarObjects.transform);
            }
            obj.tag = Constant.TAG_YANAGISENRIN_UPDATING;
        }

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
}

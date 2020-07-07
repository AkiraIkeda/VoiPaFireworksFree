using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Fireworks : MonoBehaviour{
    /* Properties */
    // Prefab
    // Rising Prefab
    public GameObject RisingPrefab;
    // Ground Effect Prefabs
    public GameObject ToraStar;
    public GameObject KikuToraStar;
    public GameObject VToraStar;
    public GameObject YashiToraStar;
    public GameObject SazanamiStar;
    public GameObject HiyuStar;
    public GameObject SenrinStar;
    public GameObject RandamaStar;
    // Parent Object
    public GameObject RisingObjects;
    public GameObject GroundEffectObjects;

    // Public
    public float RisingSpeedOffset = 20.0f;
    public float RisingSpeedCoefficient = 4.0f;
    public float ShootingWidth = 300.0f;
    public float SenrinStarVolumeThreshold = 0.5f;
    public float GroundStarVolumeThreshold = 0.5f;

    // Private
    private GameObject obj;
    private List<Tone> toneList;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        
    }

    // Rising Fireworks
    public void shootRising(List<Tone> tone_list, float rms = 0.0f) {
        // Copy tone
        toneList = new List<Tone>();
        foreach( Tone tone in tone_list) {
            toneList.Add(tone);
        }
        // Chord List : Grouped by Chord
        var chordlist = toneList.GroupBy(x => x.chordRef).ToList();

        // Pitch List : Grouped by Pitch
        var pitchList = toneList.GroupBy(x => (int)(x.number / 12) - 1).ToList();

        // Pitch Chord List : Grouped by Pitch and Chord
        var pitchChordList = chordlist.GroupBy(x => (int)(x.Key / 12) - 1).ToList();

        // Shoot the Rising
        for (int i = 0; i < pitchChordList.Count; i++) {
            // Rising Chords by Pitch
            var chords = pitchChordList[i].ToList();

            // Tone Count Mean by Pitch
            float toneCountValue = (float)pitchList[i].ToList().Select(x => x.count).Max();

            // Volume by Pitch
            int countCurrent = pitchList[i].ToList().Count();
            float volumeCurrent = countCurrent * rms;

            // Type
            // Normal
            string type = Constant.FIREWORKS_RISING_TYPE_NORMAL;
            // Senrin
            if (volumeCurrent > SenrinStarVolumeThreshold) {
                type = Constant.FIREWORKS_RISING_TYPE_SENRIN;
            }

            // Rising by Chord
            for (int j = 0; j < chords.Count; j++){
                var tones = chords[j].ToList();

                // Main Tone & Pitch
                var main_tone = tones[0];
                int pitch = (int)(main_tone.number / 12) - 1;

                // Position X
                float pos_x = ((j + 1) * ShootingWidth / (chords.Count + 1)) - (ShootingWidth / 2);
                // Position Vector
                Vector3 position = new Vector3(pos_x, 0, 0);

                // Rising
                obj = GameObject.FindGameObjectWithTag(Constant.TAG_RISING_STANBY);
                if (obj == null){
                    obj = Instantiate(RisingPrefab, position, Quaternion.identity, RisingObjects.transform);
                }
                else{
                    var transform = obj.GetComponent<Transform>();
                    transform.position = position;
                    transform.rotation = Quaternion.identity;
                }

                // Change Tag Updating
                obj.tag = Constant.TAG_RISING_UPDATING;

                // Rising Init
                // Get Rising Component
                Rising rising = obj.GetComponent<Rising>();
                // Init
                rising.Init();

                // Set Type
                rising.Type = type;

                // Add ToneList
                foreach (Tone tone in tones){
                    rising.ToneList.Add(tone);
                }

                // Tone Count Mean
                rising.ToneCountValue = toneCountValue;

                // Particle Sysytem Main Module
                var main = obj.GetComponent<ParticleSystem>().main;
                // StartSpeed : from Tone Pitch
                main.startSpeed = RisingSpeedOffset + RisingSpeedCoefficient * (pitch + 1);

                // Play Particle
                var particleSystem = obj.GetComponent<ParticleSystem>();
                particleSystem.Play();
            }
        }
    }

    // Ground Effect Fireworks
    public void shootGroundEffect(List<Tone> tone_list,float rms = 0.0f){
        // Tone Volume
        int toneCountCurrent = tone_list.Count();
        float toneVolume = (float)toneCountCurrent * rms;

        // Check Shooting Threshold
        if (toneVolume < GroundStarVolumeThreshold) {
            return;
        }

        // Get Main Chord
        string[] mainChordID = MyAudioAnalyzer.GetMainChords(tone_list);

        // Get Main Tone
        int[] mainToneNumbers = MyAudioAnalyzer.GetMainToneNumbers(tone_list);

        // Chord List : Grouped by Chord
        var chord_list = tone_list.GroupBy(x => x.chordRef).ToList();
        // Pitch Chord List : Grouped by Pitch and Chord
        var pitch_chord_list = chord_list.GroupBy(x => (int)(x.Key / 12) - 1).ToList();

        // Count of Main Pitch Tones
        int[] countArray = new int[pitch_chord_list.Count];
        for (int i = 0; i < pitch_chord_list.Count; i++){
            countArray[i] = pitch_chord_list[i].ToList().Count();
        }
        int countMax = countArray.Max();

        // Ground Effect
        foreach (string chordID in mainChordID){
            // Ground Effect
            for (int i = 0; i < countMax; i++){
                GameObject obj = null;

                // Position X
                float pos_x = ((i + 1) * ShootingWidth / (countMax + 1)) - (ShootingWidth / 2);
                // Position Vector
                Vector3 position = new Vector3(pos_x, 0, 0);

                // Select Effect Star
                // Major => Tora Star
                if (chordID == Constant.CHORD_ID_MAJOR){
                    // First Search Stanby Star
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_TORA_STAR_STANBY);
                    if (obj == null){
                        // Instantiate New Star
                        obj = Instantiate(ToraStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    // Change Tag
                    obj.tag = Constant.TAG_TORA_STAR_UPDATING;
                }
                // Minor => Sazanami Star
                else if (chordID == Constant.CHORD_ID_MINOR) {
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_SAZANAMI_STAR_STANBY);
                    if (obj == null) {
                        obj = Instantiate(SazanamiStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_SAZANAMI_STAR_UPDATING;
                }
                // Diminished => KikuTora Star
                else if (chordID == Constant.CHORD_ID_DIMINISHED){
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_KIKUTORA_STAR_STANBY);
                    if (obj == null){
                        obj = Instantiate(KikuToraStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_KIKUTORA_STAR_UPDATING;
                }
                // Augmented => YashiToraStar
                else if (chordID == Constant.CHORD_ID_AUGMENTED){
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_YASHITORA_STAR_STANBY);
                    if (obj == null){
                        obj = Instantiate(YashiToraStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_YASHITORA_STAR_UPDATING;
                }
                // MinorMajor => VTora Star
                else if (chordID == Constant.CHORD_ID_MINORMAJOR){
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_VTORA_STAR_STANBY);
                    if (obj == null){
                        obj = Instantiate(VToraStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_VTORA_STAR_UPDATING;
                }
                // Half Diminished => YuseiStar
                else if(chordID == Constant.CHORD_ID_HALFDIMINISHED){
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_HIYU_STAR_STANBY);
                    if (obj == null){
                        obj = Instantiate(HiyuStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_HIYU_STAR_UPDATING;
                }
                // Dominat => Senrin Star
                else if (chordID == Constant.CHORD_ID_DOMINANT) {
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_SENRIN_STAR_STANBY);
                    if (obj == null) {
                        obj = Instantiate(SenrinStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_SENRIN_STAR_UPDATING;
                }
                // Other(None) => Randama Star
                else {
                    obj = GameObject.FindGameObjectWithTag(Constant.TAG_RANDAMA_STAR_STANBY);
                    if (obj == null){    
                        obj = Instantiate(RandamaStar, position, Quaternion.identity, GroundEffectObjects.transform);
                    }
                    obj.tag = Constant.TAG_RANDAMA_STAR_UPDATING;
                }

                // Particle System Transform
                var transform = obj.GetComponent<Transform>();
                transform.position = position;

                // Particle System of Star
                var particleSystem = obj.GetComponent<ParticleSystem>();

                // Particle Sysytem Main Module
                var main = particleSystem.main;

                // Start Color
                Color color = Colors.getToneColor(mainToneNumbers[0]);
                main.startColor = color;

                // Emit
                // float count = particleSystem.emission.GetBurst(0).count.constant;
                // int cycle = particleSystem.emission.GetBurst(0).cycleCount;
                particleSystem.Play();
            }
        }
    }
}

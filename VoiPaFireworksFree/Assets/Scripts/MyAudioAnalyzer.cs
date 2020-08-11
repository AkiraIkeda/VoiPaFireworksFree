using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class MyAudioAnalyzer : MonoBehaviour{
    /* Properties */
    public Constant Constant;

    // Convert Tone Number to Frequency
    public float ToneNumber2Frequency(int tone_number, float a4_hz = 442.0f) {
        float mult = (tone_number - 69) / 12;
        float freq = a4_hz * Mathf.Pow(2.0f, mult);
        return freq;
    }

    // Get Chord Tuple in Tone Arrays
    public Tuple<int, int, string>[] GetChordArray(int[] tone_array) {
        // No Tone Array
        if (!tone_array.Any()) {
            return new Tuple<int, int, string>[0];
        }
        // DST
        Tuple<int, int, string>[] chord_array = new Tuple<int, int, string>[tone_array.Length];

        // Search List
        List<int> tone_list = tone_array.ToList();
        
        // Search Chord
        foreach(int tone in tone_array) {
            // No Any Tone in Tone List
            if (!tone_list.Contains(tone)) {
                continue;
            }
            // Chord ID Default
            string chord_id = Constant.CHORD_ID_NONE;

            // Chord List Temp
            List<int> list_temp = new List<int>();
            
            // Octave List
            List<int> octave = tone_list.FindAll(x => x - tone < 12);

            // Difference in Octave
            int[] diff_in_octave = new int[octave.Count];
            for (int i = 0; i < diff_in_octave.Length; i++) {
                diff_in_octave[i] = octave[i] - tone;
            }

            // Search Chord in Octave
            // Minor 3rd
            if (diff_in_octave.Contains(3)) {
                list_temp.Add(octave[Array.IndexOf(diff_in_octave, 3)]);
                // Diminished Triad
                if (diff_in_octave.Contains(6)) {
                    list_temp.Add(octave[Array.IndexOf(diff_in_octave, 6)]);
                    // Diminished Seventh
                    if (diff_in_octave.Contains(9)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 9)]);
                        chord_id = Constant.CHORD_ID_DIMINISHED;
                    }
                    // Half Diminished Seventh
                    else if (diff_in_octave.Contains(10)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 10)]);
                        chord_id = Constant.CHORD_ID_HALFDIMINISHED;
                    }
                    // Diminished
                    else {
                        chord_id = Constant.CHORD_ID_DIMINISHED;
                    }
                }// Minor Triad
                else if (diff_in_octave.Contains(7)) {
                    list_temp.Add(octave[Array.IndexOf(diff_in_octave, 7)]);
                    // Minor Seventh
                    if (diff_in_octave.Contains(10)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 10)]);
                        chord_id = Constant.CHORD_ID_MINOR;
                    }
                    // Minor Major Seventh
                    else if (diff_in_octave.Contains(11)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 11)]);
                        chord_id = Constant.CHORD_ID_MINORMAJOR;
                    }
                    else {
                        chord_id = Constant.CHORD_ID_MINOR;
                    }
                }
                else {
                    chord_id = Constant.CHORD_ID_MINOR;
                }
            }
            // Major 3rd
            else if (diff_in_octave.Contains(4)) {
                list_temp.Add(octave[Array.IndexOf(diff_in_octave, 4)]);
                // Major Triad
                if (diff_in_octave.Contains(7)) {
                    list_temp.Add(octave[Array.IndexOf(diff_in_octave, 7)]);
                    // Dominat Seventh
                    if (diff_in_octave.Contains(10)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 10)]);
                        chord_id = Constant.CHORD_ID_DOMINANT;
                    }
                    // Major Seventh 
                    else if (diff_in_octave.Contains(11)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 11)]);
                        chord_id = Constant.CHORD_ID_MAJOR;
                    }
                    else {
                        chord_id = Constant.CHORD_ID_MAJOR;
                    }
                }
                // Augmented Triad
                else if (diff_in_octave.Contains(8)) {
                    list_temp.Add(octave[Array.IndexOf(diff_in_octave, 8)]);
                    // Augmented Major Seventh
                    if (diff_in_octave.Contains(11)) {
                        list_temp.Add(octave[Array.IndexOf(diff_in_octave, 11)]);
                        chord_id = Constant.CHORD_ID_AUGMENTED;
                    }
                    else {
                        chord_id = Constant.CHORD_ID_AUGMENTED;
                    }
                }
                else {
                    chord_id = Constant.CHORD_ID_MAJOR;
                }
            }

            // Chord Check
            // Chord
            if (list_temp.Count() > 0) {
                // Chord
                foreach (int tone_temp in list_temp) {
                    // Register Chord Array
                    // main tone
                    chord_array[Array.IndexOf(tone_array, tone)] = Tuple.Create(tone, tone, chord_id);
                    // chord tone
                    chord_array[Array.IndexOf(tone_array, tone_temp)] = Tuple.Create(tone_temp, tone, chord_id);
                    // Remove
                    tone_list.Remove(tone_temp);
                }
            }
            // Not Chord
            else {
                chord_array[Array.IndexOf(tone_array, tone)] = Tuple.Create(tone, tone, chord_id);
            }

        }return chord_array;
    }

    // Get Main Chord in Tone List
    public static string[] GetMainChords(List<Tone> tone_list){
        var toneChordCountList = tone_list
            .GroupBy(x => x.chordID)
            .Select(x => new { id = x.Key, count = x.Count() }).ToList();
        int countMax = toneChordCountList.Select(x => x.count).Max();
        var toneChordArray = toneChordCountList.Where(x => x.count == countMax).Select(x => x.id).ToArray();
        return toneChordArray;
    }

    // Get Main Chord Tone
    public static Tone GetMainChordTone(List<Tone> tone_list) {
        // Main Tone
        Tone mainTone = null;
        // Tone List that chord ID is not None
        var toneList = tone_list.Where(x => x.chordID != Constant.CHORD_ID_NONE).ToList();
        // If No Tone use Default tone list 
        if (toneList.Count() == 0) {
            return mainTone;
        }

        // ToneList Grouped by Count
        var toneChordCountList = toneList
            .GroupBy(x => x.chordID)
            .Select(x => new { id = x.Key, count = x.Count() }).ToList();
        // Count Max of Tones
        int countMax = toneChordCountList.Select(x => x.count).Max();
        var toneChordArray = toneChordCountList.Where(x => x.count == countMax).Select(x => x.id).ToArray();

        // Main Tone List
        List<Tone> mainToneList = new List<Tone>(); 
        foreach (string chordID in toneChordArray){
            // Search the Tone of Main Chord
            var chordToneList = toneList.Where(x => x.chordID == chordID);
            // The Lowest Tone is Main Chord Tone
            int minNumber = chordToneList.Select(x => x.number).Min();
            var mainToneTemp = toneList.Where(x => x.number == minNumber).ToArray()[0];
            // Add Main tone
            mainToneList.Add(mainToneTemp);
        }

        // If there are seberal main tones, The Volume Higher is the Main tone
        float maxVolume = mainToneList.Select(x => x.volume).Max();
        mainTone = mainToneList.Where(x => x.volume == maxVolume).ToArray()[0];

        return mainTone;
    }

    // Get Chord Progression
    public static string GetChordProgression(List<Tone> chord_tone_list) {
        // Chord Progression ID
        string chordProgressionID = Constant.CHORD_PROGRESSION_ID_NONE;
        // Check ChordToneList Length = > Return None
        if (chord_tone_list.Count() < 4) {
            return chordProgressionID;
        }
        // Create Tone Difference List
        List<int> toneDiffList = new List<int>();
        for (int i = 0; i < chord_tone_list.Count; i++) {
            // Difference number with first tone
            int diff = (chord_tone_list[i].number - chord_tone_list[0].number) % 12;
            if (diff < 0) {
                diff += 12;
            }
            // Add difference List
            toneDiffList.Add(diff);
        }
        // Search Chord Progression
        // Second Tone
        switch (toneDiffList[1]) {
            // II
            case 2:
                switch (toneDiffList[2]) {
                    // III
                    case 4:
                        switch (toneDiffList[3]) {
                            // IV
                            case 5:
                                // I-II-III-IV
                                chordProgressionID = Constant.CHORD_PROGRESSION_ID_I_IIM_IIIM_IV;
                                break;
                            default:
                                break;
                        }
                        break;
                    // VII
                    case 11:
                        switch (toneDiffList[3]) {
                            // I
                            case 0:
                                // I-II-VI-I (IV-V-III-IV)
                                chordProgressionID = Constant.CHORD_PROGRESSION_ID_IV_V_IIIM_VIM;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            // V
            case 7:
                switch (toneDiffList[2]) {
                    // II
                    case 2:
                        switch (toneDiffList[3]) {
                            // III
                            case 4:
                                // I-V-II-III (IV-I-V-VI)
                                chordProgressionID = Constant.CHORD_PROGRESSION_ID_IV_I_V_VIM;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            // VI
            case 9:
                switch (toneDiffList[2]) {
                    // II
                    case 2:
                        switch (toneDiffList[3]) {
                            // V
                            case 7:
                                // I-VI-II-V
                                chordProgressionID = Constant.CHORD_PROGRESSION_ID_I_VIM_IIM_V;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        return chordProgressionID;
    }

    // Get Main Tone Number in Tone List
    public static int[] GetMainToneNumbers(List<Tone> tone_list){
        var toneNumberCountList = tone_list
            .GroupBy(x => x.number % 12)
            .Select(x => new { number = x.Key, count = x.Count() }).ToList();
        int countMax = toneNumberCountList.Select(x => x.count).Max();
        var toneNumberArray = toneNumberCountList.Where(x => x.count == countMax).Select(x => x.number).ToArray();
        return toneNumberArray;
    }
}

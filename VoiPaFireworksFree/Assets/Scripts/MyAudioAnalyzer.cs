using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyAudioAnalyzer : MonoBehaviour{
    /* Properties */
    public Constant Constant;

    // Get Chord Method
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

    // Get Main Chord
    public static string[] GetMainChords(List<Tone> tone_list){
        var toneChordCountList = tone_list
            .GroupBy(x => x.chordID)
            .Select(x => new { id = x.Key, count = x.Count() }).ToList();
        int countMax = toneChordCountList.Select(x => x.count).Max();
        var toneChordArray = toneChordCountList.Where(x => x.count == countMax).Select(x => x.id).ToArray();
        return toneChordArray;
    }

    // Get Main Tone Number
    public static int[] GetMainToneNumbers(List<Tone> tone_list){
        var toneNumberCountList = tone_list
            .GroupBy(x => x.number % 12)
            .Select(x => new { number = x.Key, count = x.Count() }).ToList();
        int countMax = toneNumberCountList.Select(x => x.count).Max();
        var toneNumberArray = toneNumberCountList.Where(x => x.count == countMax).Select(x => x.number).ToArray();
        return toneNumberArray;
    }
}

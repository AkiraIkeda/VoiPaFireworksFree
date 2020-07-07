using UnityEngine;

public class Tone{
    // Properties
    public int number;
    public int count;
    public int chordRef;
    public string chordID;
    public float volume;

    // Constructor
    public Tone(int _number = 0, int _count = 0, float _volume = 0.0f, int _chordRef = 0, string _chordID = Constant.CHORD_ID_NONE) {
        number = _number;
        count = _count;
        volume = _volume;
        chordRef = _chordRef;
        chordID = _chordID;
    }
}

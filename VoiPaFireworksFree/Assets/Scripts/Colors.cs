using UnityEngine;

public class Colors{
    // Tone Colors
    public static readonly Color C = new Color(255f / 255f, 102f / 255f, 255f/ 255f, 255f / 255f);
    public static readonly Color CS = new Color(255f / 255f, 102f / 255f, 179f/ 255f, 255f / 255f);
    public static readonly Color D = new Color(255f / 255f, 102f / 255f, 102f / 255f, 255f / 255f);
    public static readonly Color DS = new Color(255f / 255f, 179f / 255f, 102f / 255f, 255f / 255f);
    public static readonly Color E = new Color(255f / 255f, 255f / 255f, 102f / 255f, 255f / 255f);
    public static readonly Color F = new Color(179f / 255f, 255f / 255f, 102f / 255f, 255f / 255f);
    public static readonly Color FS = new Color(102f / 255f, 255f / 255f, 102f / 255f, 255f / 255f);
    public static readonly Color G = new Color(102f / 255f, 255f / 255f, 179f / 255f, 255f / 255f);
    public static readonly Color GS = new Color(102f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    public static readonly Color A = new Color(102f / 255f, 179f / 255f, 255f / 255f, 255f / 255f);
    public static readonly Color AS = new Color(102f / 255f, 102f / 255f, 255f / 255f, 255f / 255f);
    public static readonly Color B = new Color(179f / 255f, 102f / 255f, 255f / 255f, 255f / 255f);

    // Get Tone Color form Tone number
    public static Color getToneColor(int tone_number) {
        Color color = new Color(255, 255, 255, 255);
        // Color
        int colorID = tone_number % 12;
        switch (colorID) {
            case 0:
                color = C;
                break;
            case 1:
                color = CS;
                break;
            case 2:
                color = D;
                break;
            case 3:
                color = DS;
                break;
            case 4:
                color = E;
                break;
            case 5:
                color = F;
                break;
            case 6:
                color = FS;
                break;
            case 7:
                color = G;
                break;
            case 8:
                color = GS;
                break;
            case 9:
                color = A;
                break;
            case 10:
                color = AS;
                break;
            case 11:
                color = B;
                break;
        }
        return color;
    }
}

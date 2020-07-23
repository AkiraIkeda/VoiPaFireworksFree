using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs{
    public float cameraAngle;
    public float cameraDepth;
    public float cameraHeight;
    public float livelyEffect;
    public float shootingWidth;

    public Prefs(float camera_angle, float camera_depth, float camera_height, float lively_effect, float shooting_width) {
        cameraAngle = camera_angle;
        cameraDepth = camera_depth;
        cameraHeight = camera_height;
        livelyEffect = lively_effect;
        shootingWidth = shooting_width;
    }
}

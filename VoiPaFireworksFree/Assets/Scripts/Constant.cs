using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant{
    /* Preferences */
    public static Prefs PREFS_01 = new Prefs(-25f, -170f, 12.5f, 0.5f, 300f);
    public static Prefs PREFS_02 = new Prefs(-12.5f, -190f, 55f, 0.5f, 400f);

    /* Phisics */
    public const float PHYSICS_GRAVITY_ACC = 9.8f;

    /* MUSIC */
    // CHORD ID
    public const string CHORD_ID_NONE = "non";
    public const string CHORD_ID_MAJOR = "maj"; 
    public const string CHORD_ID_MINOR = "min";
    public const string CHORD_ID_DIMINISHED = "dim";
    public const string CHORD_ID_AUGMENTED = "aug";
    public const string CHORD_ID_DOMINANT = "dom";
    public const string CHORD_ID_MINORMAJOR = "mim";
    public const string CHORD_ID_HALFDIMINISHED = "hal";

    // CHORD PROGRESSION ID
    public const string CHORD_PROGRESSION_ID_NONE = "non";
    public const string CHORD_PROGRESSION_ID_I_IIM_IIIM_IV = "I_IIm_IIIm_IV";
    public const string CHORD_PROGRESSION_ID_I_VIM_IIM_V = "I_VIm_IIm_V";
    public const string CHORD_PROGRESSION_ID_IV_V_IIIM_VIM = "IV_V_IIIm_VIm";
    public const string CHORD_PROGRESSION_ID_IV_I_V_VIM = "IV_I_V_VIm";

    /* Fireworks */
    // Fireworks Type
    public const string FIREWORKS_RISING_TYPE_NORMAL = "Normal";
    public const string FIREWORKS_RISING_TYPE_SPARSE = "Sparse";
    public const string FIREWORKS_RISING_TYPE_SENRIN = "Senrin";
    public const string FIREWORKS_RISING_TYPE_POKA = "Poka";

    // Fireworks Star ID
    // Normal Stars
    public const string FIREWORKS_STAR_ID_BOTAN = "Botan";
    public const string FIREWORKS_STAR_ID_KIKU = "Kiku";
    public const string FIREWORKS_STAR_ID_YASHI = "Yashi";
    public const string FIREWORKS_STAR_ID_KAMURO = "Kamuro";
    public const string FIREWORKS_STAR_ID_YANAGI = "Yanagi";
    // Sparse Stars
    public const string FIREWORKS_STAR_ID_BOTAN_SPARSE = "BotanSparse";
    public const string FIREWORKS_STAR_ID_KIKU_SPARSE = "KikuSparse";
    public const string FIREWORKS_STAR_ID_YASHI_SPARSE = "YashiSparse";
    public const string FIREWORKS_STAR_ID_KAMURO_SPARSE = "KamuroSparse";
    public const string FIREWORKS_STAR_ID_YANAGI_SPARSE = "YanagiSparse";
    // Senrin Stars
    public const string FIREWORKS_STAR_ID_BOTAN_SENRIN = "BotanSenrin";
    public const string FIREWORKS_STAR_ID_KIKU_SENRIN = "KikuSenrin";
    public const string FIREWORKS_STAR_ID_YASHI_SENRIN = "YashiSenrin";
    public const string FIREWORKS_STAR_ID_HACHI_SENRIN = "HachiSenrin";
    public const string FIREWORKS_STAR_ID_KAMURO_SENRIN = "KamuroSenrin";
    public const string FIREWORKS_STAR_ID_YANAGI_SENRIN = "YanagiSenrin";
    // Poka Stars
    public const string FIREWORKS_STAR_ID_BOTAN_POKA = "BotanPoka";
    public const string FIREWORKS_STAR_ID_KIKU_POKA = "KikuPoka";
    public const string FIREWORKS_STAR_ID_YASHI_POKA = "YashiPoka";
    public const string FIREWORKS_STAR_ID_KAMURO_POKA = "KamuroPoka";
    public const string FIREWORKS_STAR_ID_YANAGI_POKA = "YanagiPoka";
    // Shaped Stars
    public const string FIREWORKS_STAR_ID_MIRAI = "Mirai";
    // Ground Stars
    public const string FIREWORKS_STAR_ID_TORA = "Tora";
    public const string FIREWORKS_STAR_ID_V_TORA = "VTora";
    public const string FIREWORKS_STAR_ID_KIKU_TORA = "KikuTora";
    public const string FIREWORKS_STAR_ID_YASHI_TORA = "YashiTora";
    public const string FIREWORKS_STAR_ID_SAZANAMI = "Sazanami";
    public const string FIREWORKS_STAR_ID_HIYU = "Hiyu";
    public const string FIREWORKS_STAR_ID_SENRIN = "Senrin";
    public const string FIREWORKS_STAR_ID_RANDAMA = "Randama";

    /* TAG */
    // Updating
    public const string TAG_FIREWORKS_UPDATING = "FireworksUpdating";
    // Rising Stanby Tag
    public const string TAG_RISING_STANBY = "RisingStanby";
    // Normal Star Stanby Tag
    public const string TAG_BOTAN_STAR_STANBY = "BotanStarStanby";
    public const string TAG_KIKU_STAR_STANBY = "KikuStarStanby";
    public const string TAG_YASHI_STAR_STANBY = "YashiStarStanby";
    public const string TAG_KAMURO_STAR_STANBY = "KamuroStarStanby";
    public const string TAG_YANAGI_STAR_STANBY = "YanagiStarStanby";
    // Sparse Star Stanby Tag
    public const string TAG_BOTAN_SPARSE_STAR_STANBY = "BotanSparseStarStanby";
    public const string TAG_KIKU_SPARSE_STAR_STANBY = "KikuSparseStarStanby";
    public const string TAG_YASHI_SPARSE_STAR_STANBY = "YashiSparseStarStanby";
    public const string TAG_KAMURO_SPARSE_STAR_STANBY = "KamuroSparseStarStanby";
    public const string TAG_YANAGI_SPARSE_STAR_STANBY = "YanagiSparseStarStanby";
    // Senrin Star Stanby Tag
    public const string TAG_BOTANSENRIN_STANBY = "BotanSenrinStanby";
    public const string TAG_KIKUSENRIN_STANBY = "KikuSenrinStanby";
    public const string TAG_YASHISENRIN_STANBY = "YashiSenrinStanby";
    public const string TAG_KAMUROSENRIN_STANBY = "KamuroSenrinStanby";
    public const string TAG_YANAGISENRIN_STANBY = "YanagiSenrinStanby";
    public const string TAG_HACHISENRIN_STANBY = "HachiSenrinStanby";
    // Poka Star Stanby Tag
    public const string TAG_BOTAN_POKA_STAR_STANBY = "BotanPokaStarStanby";
    public const string TAG_KIKU_POKA_STAR_STANBY = "KikuPokaStarStanby";
    public const string TAG_YASHI_POKA_STAR_STANBY = "YashiPokaStarStanby";
    public const string TAG_KAMURO_POKA_STAR_STANBY = "KamuroPokaStarStanby";
    public const string TAG_YANAGI_POKA_STAR_STANBY = "YanagiPokaStarStanby";
    // Shaped Star Stanby Tag
    public const string TAG_MIRAI_STAR_STANBY = "MiraiStarStanby";
    // Ground Star Stanby Tag
    public const string TAG_TORA_STAR_STANBY = "ToraStarStanby";
    public const string TAG_VTORA_STAR_STANBY = "VToraStarStanby";
    public const string TAG_KIKUTORA_STAR_STANBY = "KikuToraStarStanby";
    public const string TAG_YASHITORA_STAR_STANBY = "YashiToraStarStanby";
    public const string TAG_SAZANAMI_STAR_STANBY = "SazanamiStarStanby";
    public const string TAG_HIYU_STAR_STANBY = "HiyuStarStanby";
    public const string TAG_SENRIN_STAR_STANBY = "SenrinStarStanby";
    public const string TAG_RANDAMA_STAR_STANBY = "RandamaStarStanby";
}

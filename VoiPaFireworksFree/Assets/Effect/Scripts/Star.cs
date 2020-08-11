using UnityEngine;

public class Star : MonoBehaviour{
    public string ID = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleSystemStopped() {
        // Change Tag => Stanby
        switch (ID) {
            // Normal Stars
            case Constant.FIREWORKS_STAR_ID_BOTAN:
                this.gameObject.tag = Constant.TAG_BOTAN_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KIKU:
                this.gameObject.tag = Constant.TAG_KIKU_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YASHI:
                this.gameObject.tag = Constant.TAG_YASHI_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KAMURO:
                this.gameObject.tag = Constant.TAG_KAMURO_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YANAGI:
                this.gameObject.tag = Constant.TAG_YANAGI_STAR_STANBY;
                break;
            // Sparse Stars
            case Constant.FIREWORKS_STAR_ID_BOTAN_SPARSE:
                this.gameObject.tag = Constant.TAG_BOTAN_SPARSE_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KIKU_SPARSE:
                this.gameObject.tag = Constant.TAG_KIKU_SPARSE_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YASHI_SPARSE:
                this.gameObject.tag = Constant.TAG_YASHI_SPARSE_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KAMURO_SPARSE:
                this.gameObject.tag = Constant.TAG_KAMURO_SPARSE_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YANAGI_SPARSE:
                this.gameObject.tag = Constant.TAG_YANAGI_SPARSE_STAR_STANBY;
                break;
            // Senrin Stars
            case Constant.FIREWORKS_STAR_ID_BOTAN_SENRIN:
                this.gameObject.tag = Constant.TAG_BOTANSENRIN_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KIKU_SENRIN:
                this.gameObject.tag = Constant.TAG_KIKUSENRIN_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YASHI_SENRIN:
                this.gameObject.tag = Constant.TAG_YASHISENRIN_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KAMURO_SENRIN:
                this.gameObject.tag = Constant.TAG_KAMUROSENRIN_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YANAGI_SENRIN:
                this.gameObject.tag = Constant.TAG_YANAGISENRIN_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_HACHI_SENRIN:
                this.gameObject.tag = Constant.TAG_HACHISENRIN_STANBY;
                break;
            // Poka Stars
            case Constant.FIREWORKS_STAR_ID_BOTAN_POKA:
                this.gameObject.tag = Constant.TAG_BOTAN_POKA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KIKU_POKA:
                this.gameObject.tag = Constant.TAG_KIKU_POKA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YASHI_POKA:
                this.gameObject.tag = Constant.TAG_YASHI_POKA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KAMURO_POKA:
                this.gameObject.tag = Constant.TAG_KAMURO_POKA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YANAGI_POKA:
                this.gameObject.tag = Constant.TAG_YANAGI_POKA_STAR_STANBY;
                break;
            // Shaped Stars
            case Constant.FIREWORKS_STAR_ID_MIRAI:
                this.gameObject.tag = Constant.TAG_MIRAI_STAR_STANBY;
                break;
            // Ground Effect Stars
            case Constant.FIREWORKS_STAR_ID_TORA:
                this.gameObject.tag = Constant.TAG_TORA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_KIKU_TORA:
                this.gameObject.tag = Constant.TAG_KIKUTORA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_V_TORA:
                this.gameObject.tag = Constant.TAG_VTORA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_YASHI_TORA:
                this.gameObject.tag = Constant.TAG_YASHITORA_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_SAZANAMI:
                this.gameObject.tag = Constant.TAG_SAZANAMI_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_HIYU:
                this.gameObject.tag = Constant.TAG_HIYU_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_SENRIN:
                this.gameObject.tag = Constant.TAG_SENRIN_STAR_STANBY;
                break;
            case Constant.FIREWORKS_STAR_ID_RANDAMA:
                this.gameObject.tag = Constant.TAG_RANDAMA_STAR_STANBY;
                break;
            default:
                Debug.Log("Error : This ID is not in the Constant.FIREWORKS_ID :" + ID);
                break;
        }
    }
}

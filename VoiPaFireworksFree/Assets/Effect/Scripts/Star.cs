using UnityEngine;

public class Star : MonoBehaviour{
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
        switch (gameObject.tag) {
            // Normal Stars
            case Constant.TAG_BOTAN_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_BOTAN_STAR_STANBY;
                break;
            case Constant.TAG_KIKU_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_KIKU_STAR_STANBY;
                break;
            case Constant.TAG_YASHI_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_YASHI_STAR_STANBY;
                break;
            case Constant.TAG_KAMURO_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_KAMURO_STAR_STANBY;
                break;
            case Constant.TAG_YANAGI_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_YANAGI_STAR_STANBY;
                break;
            // Senrin Stars
            case Constant.TAG_BOTANSENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_BOTANSENRIN_STANBY;
                break;
            case Constant.TAG_KIKUSENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_KIKUSENRIN_STANBY;
                break;
            case Constant.TAG_YASHISENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_YASHISENRIN_STANBY;
                break;
            case Constant.TAG_KAMUROSENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_KAMUROSENRIN_STANBY;
                break;
            case Constant.TAG_YANAGISENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_YANAGISENRIN_STANBY;
                break;
            case Constant.TAG_HACHISENRIN_UPDATING:
                this.gameObject.tag = Constant.TAG_HACHISENRIN_STANBY;
                break;
            // Ground Effect Stars
            case Constant.TAG_TORA_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_TORA_STAR_STANBY;
                break;
            case Constant.TAG_KIKUTORA_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_KIKUTORA_STAR_STANBY;
                break;
            case Constant.TAG_VTORA_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_VTORA_STAR_STANBY;
                break;
            case Constant.TAG_YASHITORA_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_YASHITORA_STAR_STANBY;
                break;
            case Constant.TAG_SAZANAMI_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_SAZANAMI_STAR_STANBY;
                break;
            case Constant.TAG_HIYU_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_HIYU_STAR_STANBY;
                break;
            case Constant.TAG_SENRIN_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_SENRIN_STAR_STANBY;
                break;
            case Constant.TAG_RANDAMA_STAR_UPDATING:
                this.gameObject.tag = Constant.TAG_RANDAMA_STAR_STANBY;
                break;
        }
    }
}

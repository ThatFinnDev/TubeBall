using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterial : MonoBehaviour
{
    public SelectMaterialType type;
    public Material material;
    public TextMeshProUGUI buyText;
    public long cost = 1000;
    private bool bought => PlayerPrefs.GetInt("shop." + material.name + "."+(type.ToString()).ToLower(), 0) == 1;

    public void SetBought(bool state) { PlayerPrefs.SetInt("shop." + material.name + "."+(type.ToString()).ToLower(), state?1:0); }
    
    void Start()
    {
        if (material.mainTexture != null)
            GetComponent<Image>().sprite = ConvertToSprite(material.mainTexture as Texture2D);
        else 
            GetComponent<Image>().color = material.color;
        buyText.SetText(cost.ToString());
        if (bought)
        {
            buyText.gameObject.SetActive(false);
        }
    }
    public static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f), 1f);
    }
    public void OnPress()
    {
        if (bought)
        {
            switch (type)
            {
                case SelectMaterialType.Obstacle: CosmeticsManager.instance.ChangeObstacleColor(material); break;
                case SelectMaterialType.Dot: CosmeticsManager.instance.ChangeDotColor(material); break;
                case SelectMaterialType.Stripe: CosmeticsManager.instance.ChangeStripeColor(material); break;
                case SelectMaterialType.Tube: CosmeticsManager.instance.ChangeTubeColor(material); break;
                case SelectMaterialType.Ball: CosmeticsManager.instance.ChangeBallColor(material); break;
            }
        }
        else if (GameController.coins >= cost || PreferenceManager.freeShopping)
        {
            if(!PreferenceManager.freeShopping)
                GameController.coins -= cost;
            SetBought(true);
            buyText.gameObject.SetActive(false);
            OnPress();
        }
        else
        {
            switch (type)
            {
                case SelectMaterialType.Obstacle: CosmeticsManager.instance.ApplyPreviewObstacleMaterial(material); break;
                case SelectMaterialType.Dot: CosmeticsManager.instance.ApplyPreviewDotMaterial(material); break;
                case SelectMaterialType.Stripe: CosmeticsManager.instance.ApplyPreviewStripeMaterial(material); break;
                case SelectMaterialType.Tube: CosmeticsManager.instance.ApplyPreviewTubeMaterial(material); break;
                case SelectMaterialType.Ball: CosmeticsManager.instance.ApplyPreviewBallMaterial(material); break;
            }
        }
        
    }
    
}

public enum SelectMaterialType
{
    Obstacle, Dot, Stripe, Tube, Ball
}
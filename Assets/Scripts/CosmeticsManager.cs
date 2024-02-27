using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsManager : MonoBehaviour
{
    public List<BuyableMaterial> buyables;
    [Header("Other")] [SerializeField] Transform obstacles;
    [SerializeField] Transform stripes;
    [SerializeField] Transform dots;
    [SerializeField] TextMeshProUGUI shopSurTitle;
    [SerializeField] Transform buyButtonHolder;
    [SerializeField] GameObject buyButtonPrefab;
    [SerializeField] RenderTexture dotRenderTexture;
    [SerializeField] RenderTexture stripeRenderTexture;
    [SerializeField] RenderTexture obstacleRenderTexture;
    [SerializeField] Renderer dotPreview;
    [SerializeField] Renderer stripePreview;
    [SerializeField] Renderer obstaclePreview;
    [SerializeField] private Image dotTexture;
    [SerializeField] private Image stripeTexture;
    [SerializeField] private Image obstacleTexture;

    public static CosmeticsManager instance;
    private bool refreshPreviews = false;
    private void Update()
    {
        if(refreshPreviews)
        {
            stripeTexture.sprite = ConvertToSprite(toTexture2D(stripeRenderTexture));
            dotTexture.sprite = ConvertToSprite(toTexture2D(dotRenderTexture));
            obstacleTexture.sprite = ConvertToSprite(toTexture2D(obstacleRenderTexture));
        }
    }

    private void Awake()
    {
        instance = this;
        ReGenerateBuyButton(SelectMaterialType.Obstacle);
        ApplyObstacleColor();
        ApplyStripeColor();
        ApplyDotColor();
    }
    public static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
    
    public static Sprite ConvertToSprite(Texture2D texture)
    {
        if(texture==null)
            Debug.Log("E");
        return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f), 1f);
    }
    public void ReObstacles()
    {
        ReGenerateBuyButton(SelectMaterialType.Obstacle);
    }

    public void ReDots()
    {
        ReGenerateBuyButton(SelectMaterialType.Dot);
    }

    public void ReStripes()
    {
        ReGenerateBuyButton(SelectMaterialType.Stripe);
    }

    void ApplyObstacleColor()
    {
        var _index = Mathf.Clamp(PlayerPrefs.GetInt("color.obsticals", 0), 0, buyables.Count - 1);
        Material material = new Material(buyables[_index].material);
        foreach (Transform child in obstacles)
            child.GetComponent<Renderer>().material = material;
        obstaclePreview.material = material;
        refreshPreviews = true;

    }

    void ApplyStripeColor()
    {
        var _index = Mathf.Clamp(PlayerPrefs.GetInt("color.stripes", 0), 0, buyables.Count - 1);
        Material material = new Material(buyables[_index].material);
        foreach (Transform child in stripes)
            child.GetComponent<Renderer>().material = material;
        stripePreview.material = material;
        refreshPreviews = true;
    }
    void ApplyDotColor()
    {
        var _index = Mathf.Clamp(PlayerPrefs.GetInt("color.dots", 0), 0, buyables.Count - 1);
        Material material = new Material(buyables[_index].material);
        foreach (Transform child in dots)
            child.GetComponent<Renderer>().material = material;
        dotPreview.material = material;
        refreshPreviews = true;
    }

    public void ChangeDotColor(Material material)
    {
        int i = -1;
        foreach (BuyableMaterial buyable in buyables)
        {
            i++;
            if (buyable.material == material)
            {
                PlayerPrefs.SetInt("color.dots", i);
                ApplyDotColor();
                return;
            }
        }
    }

    public void ChangeStripeColor(Material material)
    {
        int i = -1;
        foreach (BuyableMaterial buyable in buyables)
        {
            i++;
            if (buyable.material == material)
            {
                PlayerPrefs.SetInt("color.stripes", i);
                ApplyStripeColor();
                return;
            }
        }
    }


    public void ApplyPreviewObstacleMaterial(Material material)
    {
        obstaclePreview.material = new Material(material);
        refreshPreviews = true;
    }
    
    public void ApplyPreviewDotMaterial(Material material)
    {
        stripePreview.material = new Material(material);
        refreshPreviews = true;
    }
    
    public void ApplyPreviewStripeMaterial(Material material)
    {
        dotPreview.material = new Material(material);
        refreshPreviews = true;
    }
    
    public void ChangeObstacleColor(Material material)
    {
        int i = -1;
        foreach (BuyableMaterial buyable in buyables)
        {
            i++;
            if (buyable.material == material)
            {
                PlayerPrefs.SetInt("color.obsticals", i);
                ApplyObstacleColor();
                return;
            }
        }
    }


    public void ReGenerateBuyButton(SelectMaterialType selectedBuyType)
    {
        shopSurTitle.SetText(selectedBuyType.ToString() + "s");
        foreach (Transform child in buyButtonHolder)
            Destroy(child.gameObject);
        foreach (BuyableMaterial buyable in buyables)
        {
            GameObject buttonInstance = Instantiate(buyButtonPrefab, buyButtonHolder);
            SelectMaterial selectMaterial = buttonInstance.GetComponent<SelectMaterial>();
            selectMaterial.material = buyable.material;
            selectMaterial.cost = buyable.cost;
            selectMaterial.type = selectedBuyType;
            if (buyable.boughBuyDefault)
                selectMaterial.SetBought(true);
        }
    }

}

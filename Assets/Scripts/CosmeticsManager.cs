using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsManager : MonoBehaviour
{
    public List<BuyableMaterial> buyables;
    public List<BuyableMaterial> tubeBuyable;
    [Header("Other")] [SerializeField] Transform obstacles;
    [SerializeField] Transform stripes;
    [SerializeField] Transform dots;
    [SerializeField] TextMeshProUGUI shopSurTitle;
    [SerializeField] Transform buyButtonHolder;
    [SerializeField] GameObject buyButtonPrefab;
    [SerializeField] RenderTexture dotRenderTexture;
    [SerializeField] RenderTexture stripeRenderTexture;
    [SerializeField] RenderTexture obstacleRenderTexture;
    [SerializeField] RenderTexture tubeRenderTexture;
    [SerializeField] Renderer dotPreview;
    [SerializeField] Renderer stripePreview;
    [SerializeField] Renderer obstaclePreview;
    [SerializeField] Renderer tubePreview;
    [SerializeField] private List<Renderer> tubes;
    [SerializeField] private Image dotTexture;
    [SerializeField] private Image stripeTexture;
    [SerializeField] private Image obstacleTexture;
    [SerializeField] private Image tubeTexture;
    [SerializeField] private List<Camera> previewCameras;
    public static CosmeticsManager instance;

    private bool refreshPreviews
    {
        get { return _refreshPreviews; }
        set
        {
            if (value) foreach (Camera cam in previewCameras) cam.gameObject.SetActive(true);
            else deActiveCamera = true;
            _refreshPreviews = value;
        }
    }

    private bool deActiveCamera = false;
    private bool _refreshPreviews = false;

    private void Update()
    {
        if(GameController.instance.inGame)
            foreach (BuyableMaterial buyableMaterial in tubeBuyable)
                buyableMaterial.material.mainTextureOffset+= new Vector2(0,Time.timeScale*Time.deltaTime*(GameController.difficulty.speed/5));
        
    }

    private void FixedUpdate()
    {
        if(refreshPreviews)
        {
            stripeTexture.sprite = ConvertToSprite(toTexture2D(stripeRenderTexture));
            dotTexture.sprite = ConvertToSprite(toTexture2D(dotRenderTexture));
            obstacleTexture.sprite = ConvertToSprite(toTexture2D(obstacleRenderTexture));
            tubeTexture.sprite = ConvertToSprite(toTexture2D(tubeRenderTexture));
            refreshPreviews = false;
        }
        else if (deActiveCamera)
        {
            foreach (Camera cam in previewCameras) cam.gameObject.SetActive(false);
            deActiveCamera = false;
        }
    }

    private void Awake()
    {
        instance = this;
        ReGenerateBuyButton(SelectMaterialType.Obstacle);
        ApplyObstacleColor();
        ApplyStripeColor();
        ApplyDotColor();
        ApplyTubeColor();
    }

    private void Start()
    {
        refreshPreviews = true;
    }

    public static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D( rTex.width,  rTex.height, TextureFormat.RGBA32, false);
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

    public void ReTube()
    {
        ReGenerateBuyButton(SelectMaterialType.Tube);
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

    void ApplyTubeColor()
    {
        var _index = Mathf.Clamp(PlayerPrefs.GetInt("color.tubes", 0), 0, tubeBuyable.Count - 1);
        foreach (Renderer child in tubes)
            child.material = tubeBuyable[_index].material;
        tubePreview.material = new Material(tubeBuyable[_index].material);
        refreshPreviews = true;
    }
    public void ChangeTubeColor(Material material)
    {
        int i = -1;
        foreach (BuyableMaterial buyable in tubeBuyable)
        {
            i++;
            if (buyable.material == material)
            {
                PlayerPrefs.SetInt("color.tubes", i);
                ApplyTubeColor();
                return;
            }
        }
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
    
    public void ApplyPreviewTubeMaterial(Material material)
    {
        tubePreview.material = material;
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

    public long GetSpendCoins()
    {
        long totalSpendCoins = 0;
        List<SelectMaterialType> allTypes = new List<SelectMaterialType>(){SelectMaterialType.Dot, SelectMaterialType.Obstacle, SelectMaterialType.Stripe, SelectMaterialType.Tube};
        foreach (SelectMaterialType type in allTypes)
        {
            List<BuyableMaterial> buyableMaterials = buyables;
            if (type == SelectMaterialType.Tube) buyableMaterials = tubeBuyable;
            foreach (BuyableMaterial buyable in buyableMaterials)
            {
                if (buyable.notBuyable) continue;
                bool bought = PlayerPrefs.GetInt("shop." + buyable.material.name + "."+(type.ToString()).ToLower(), 0) == 1;
                if (buyable.boughBuyDefault) bought = false;
                if (bought) totalSpendCoins += buyable.cost;
            }
        }
        return totalSpendCoins;
    }

    public void ReGenerateBuyButton(SelectMaterialType selectedBuyType)
    {
        refreshPreviews = true;
        shopSurTitle.SetText(selectedBuyType.ToString() + "s");
        foreach (Transform child in buyButtonHolder)
            Destroy(child.gameObject);
        List<BuyableMaterial> buyableMaterials = buyables;
        if (selectedBuyType == SelectMaterialType.Tube)
            buyableMaterials = tubeBuyable;
        foreach (BuyableMaterial buyable in buyableMaterials)
        {
            if (buyable.notBuyable) continue;
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

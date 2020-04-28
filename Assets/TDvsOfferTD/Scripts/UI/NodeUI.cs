using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellAmount;
    private Node target;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = ui.GetComponent<RectTransform>();
    }

    public void SetTarget(Node target)
    {
        this.target = target;
        Vector2 position = Camera.main.WorldToScreenPoint(target.transform.position);
        position.x *= (1233f + (1f / 3f)) / Screen.width;
        position.y *= 600f / Screen.height;
        rectTransform.anchoredPosition = position;
        if (!target.isUpgraded)
        {
            upgradeCost.text = "1";
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }
        sellAmount.text = "1";
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}

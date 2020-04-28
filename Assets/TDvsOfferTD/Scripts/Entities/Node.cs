using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    [HideInInspector]
    public GameObject turret;
    public Color notEnoughMoneyColor;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;
    private Renderer rend;
    private Color startColor;
    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    void OnMouseEnter()
    {
        if (!buildManager.canBuild) return;
        if (buildManager.hasElement)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (turret != null)
        {
            buildManager.SelectNode(this);
        }
        if (!buildManager.canBuild) return;
        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (!PlayerStats.HasElement(blueprint.type)) return;
        PlayerStats.DecreaseElement(blueprint.type);
        GameObject turret = Instantiate(blueprint.prefab, transform.position, Quaternion.identity);
        this.turret = turret;
        turretBlueprint = blueprint;
        rend.enabled = false;
    }

    public void UpgradeTurret()
    {
        if (!PlayerStats.HasElement(turretBlueprint.type)) return;
        PlayerStats.DecreaseElement(turretBlueprint.type);
        Destroy(this.turret);
        GameObject turret = Instantiate(turretBlueprint.upgradedPrefab, transform.position, Quaternion.identity);
        this.turret = turret;
        isUpgraded = true;
    }

    public void SellTurret()
    {
        PlayerStats.IncreaseElement(turretBlueprint.type);
        Destroy(turret);
        isUpgraded = false;
        turretBlueprint = null;
        rend.enabled = true;
    }
}

using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint darkness;
    public TurretBlueprint slime;
    public TurretBlueprint spores;
    public TurretBlueprint tentacles;
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectDarkness()
    {
        buildManager.SelectTurretToBuild(darkness);
    }

    public void SelectSlime()
    {
        buildManager.SelectTurretToBuild(slime);
    }

    public void SelectSpores()
    {
        buildManager.SelectTurretToBuild(spores);
    }

    public void SelectTentacles()
    {
        buildManager.SelectTurretToBuild(tentacles);
    }
}

using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public bool canBuild
    {
        get
        {
            return turretToBuild != null;
        }
    }
    public bool hasElement
    {
        get
        {
            return PlayerStats.HasElement(turretToBuild.type);
        }
    }
    public NodeUI nodeUI;
    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}

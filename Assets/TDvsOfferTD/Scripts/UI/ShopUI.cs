using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text darkText;
    public Text slimeText;
    public Text sporeText;
    public Text tentacleText;

    void Update()
    {
        darkText.text = PlayerStats.elements[TurretType.DARK].ToString();
        slimeText.text = PlayerStats.elements[TurretType.SLIME].ToString();
        sporeText.text = PlayerStats.elements[TurretType.SPORE].ToString();
        tentacleText.text = PlayerStats.elements[TurretType.TENTACLE].ToString();
    }
}
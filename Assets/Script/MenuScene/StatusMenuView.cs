using UnityEngine;
using TMPro;
using Data;

public class StatusMenuView : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text elementText;

    [Header("HP / MP")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text mpText;

    [Header("Battle Stats")]
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text defText;
    [SerializeField] private TMP_Text matkText;
    [SerializeField] private TMP_Text mdefText;
    [SerializeField] private TMP_Text speedText;

    public void Refresh(Player player)
    {
        var baseStatus = player.GetStatusBase();
        var calc = player.GetStatusCalculated();
        var run = player.GetStatusRuntime();

        nameText.text = player.GetName();
        levelText.text = $"Lv {player.GetLevel()}";
        elementText.text = $"ElM : {baseStatus.element}";

        hpText.text = $"HP  {run.hp} / {calc.maxHp}";
        mpText.text = $"MP  {run.mp} / {calc.maxMp}";

        atkText.text = $"ATK   {calc.atk}";
        defText.text = $"DEF   {calc.def}";
        matkText.text = $"MATK  {calc.matk}";
        mdefText.text = $"MDEF  {calc.mdef}";
        speedText.text = $"SPD   {calc.speed}";
    }
}

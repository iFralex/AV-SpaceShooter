using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgrade : MonoBehaviour
{
    public enum upgrades { speed, power, maxHealth, healing}
    public static int[] livelli = new int[] { 0, 0, 0, 0 };
    public static UpgradeData upgradeSel;

    [Header("Window")]
    public Text titoloT;
    public Text valoreCorrT;
    public Text valoreNuoT;
    public Text valoreCentT;
    public Text prezzoT;

    public void SelezionaUpgrade(int index)
    {
        upgradeSel = UpgradeData.Get((upgrades)index, livelli[index]);
        titoloT.text = upgradeSel.titolo;
        valoreCorrT.text = upgradeSel.valoreCorr.ToString();
        valoreNuoT.text = upgradeSel.valoreNuovo.ToString();
        valoreCentT.text = upgradeSel.valoreCorr.ToString();
        prezzoT.text = "Pay " + upgradeSel.prezzo.ToString();
        valoreCorrT.transform.parent.gameObject.SetActive(livelli[index] + 1 < 4);
        valoreNuoT.transform.parent.gameObject.SetActive(livelli[index] + 1 < 4);
        valoreCentT.transform.parent.gameObject.SetActive(livelli[index] + 1 >= 4);
        prezzoT.transform.parent.gameObject.SetActive(livelli[index] + 1 < 4);
        titoloT.gameObject.SetActive(true);
    }

    public void Upgrade()
    {
        livelli[(int)upgradeSel.type] += 1;
        //partitaManager.punti -= upgradeSel.prezzo;
        switch (upgradeSel.type)
        {
            case upgrades.speed:
                movimento.velocita = upgradeSel.valoreNuovo;
                break;
            case upgrades.power:
                spara.danno = upgradeSel.valoreNuovo;
                break;
            case upgrades.maxHealth:
                Vita.vitaMax = upgradeSel.valoreNuovo;
                break;
            case upgrades.healing:
                Vita.guarireVel = upgradeSel.valoreNuovo;
                break;
        }
        SelezionaUpgrade((int)upgradeSel.type);
    }

    public class UpgradeData
    {
        public string titolo;
        public upgrades type;
        public int valoreCorr;
        public int valoreNuovo;
        public int prezzo;

        public static UpgradeData Get(upgrades _type, int _livello)
        {
            switch (_type)
            {
                case upgrades.speed:
                    return new UpgradeData()
                    {
                        titolo = "Speed",
                        type = _type,
                        valoreCorr = new int[] { 10, 13, 17, 25 }[_livello],
                        valoreNuovo = _livello + 1 < 4 ? new int[] { 10, 13, 17, 25 }[_livello + 1] : 0,
                        prezzo = _livello + 1 >= 4 ? 0 : new int[] { 0, 3, 6, 10 }[_livello + 1]
                    };
                case upgrades.power:
                    return new UpgradeData()
                    {
                        titolo = "Power",
                        type = _type,
                        valoreCorr = new int[] { 10, 13, 16, 20}[_livello],
                        valoreNuovo = _livello + 1 < 4 ? new int[] { 10, 13, 16, 20 }[_livello + 1] : 0,
                        prezzo = _livello + 1 >= 4 ? 0 : new int[] { 0, 2, 4, 7 }[_livello + 1]
                    };
                case upgrades.maxHealth:
                    return new UpgradeData()
                    {
                        titolo = "Max health",
                        type = _type,
                        valoreCorr = new int[] { 100, 125, 175, 225 }[_livello],
                        valoreNuovo = _livello + 1 < 4 ? new int[] { 100, 125, 175, 225 }[_livello + 1] : 0,
                        prezzo = _livello + 1 >= 4 ? 0 : new int[] { 0, 1, 6, 9 }[_livello + 1]
                    };
                case upgrades.healing:
                    return new UpgradeData()
                    {
                        titolo = "healing velocity",
                        type = _type,
                        valoreCorr = new int[] { 1, 2, 3, 4 }[_livello],
                        valoreNuovo = _livello + 1 < 4 ? new int[] { 1, 2, 3, 4 }[_livello + 1] : 0,
                        prezzo = _livello + 1 >= 4 ? 0 : new int[] { 0, 5, 10, 15 }[_livello + 1]
                    };
                default:
                    return null;
            }
        }
    }
}
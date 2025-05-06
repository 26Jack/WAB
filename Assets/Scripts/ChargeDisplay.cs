using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargeDisplay : MonoBehaviour
{
    private PlayerMovement player;
    private TextMeshProUGUI textMeshPro;

    public string variable = "Charge";
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = variable + ": " + player.chargeAmount;

    }
}

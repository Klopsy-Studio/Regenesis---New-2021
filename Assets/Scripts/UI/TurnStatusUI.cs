using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnStatusUI : MonoBehaviour
{

    [Header("Turn Status")]
    [SerializeField] Image turnStatus;
    // [SerializeField] Text turnUser;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Animator turnStatusAnim;

    public Image turn;
    public GameObject gear;
    [SerializeField] Animator turnTypeAnim;

    [Header("Turn Images")]
    public Sprite playerTurn;
    public Sprite eventTurn;
    public Sprite monsterTurn;

    public Sprite winTurn;
    public Sprite loseTurn;
    public void ActivateTurn(string turn)
    {
        turnStatusAnim.SetBool("inScreen", true);
        // turnUser.text = turn;
        text.text = turn;
    }

    public void DeactivateTurn()
    {
        turnStatusAnim.SetBool("inScreen", false);
    }

    public void IndicateTurnStatus(Sprite sprite)
    {
        turnTypeAnim.SetTrigger("appear");
        turn.sprite = sprite;
    }
    
    public void StopTurnStatus()
    {
        turnTypeAnim.SetTrigger("disappear");
        turnTypeAnim.SetTrigger("idle");
    }
}

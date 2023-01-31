using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStatusUI : MonoBehaviour
{

    [Header("Turn Status")]
    [SerializeField] Image turnStatus;
    [SerializeField] Text turnUser;

    [SerializeField] Animator turnStatusAnim;

    [SerializeField] Image turn;
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
        turnUser.text = turn;
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

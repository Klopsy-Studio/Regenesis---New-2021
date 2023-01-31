using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionSelection : MonoBehaviour
{
  
    //Offset to set the image in the right place
    public float offset = 20f;
    
    
    //Reference to children in scene
    public RectTransform[] options;
    public RectTransform[] itemAmountText;
    public Image[] itemImage;
    public RectTransform selector;

    [SerializeField] Color defaultColor;
    [SerializeField] Color disabledColor;
    [Space]

    [Header("Action Options")]
    [SerializeField] Text moveText;
    [SerializeField] Text abilityText;
    [SerializeField] Text itemText;
    [SerializeField] Text waitText;
    [SerializeField] Text statusText;

    [Space]

    [SerializeField] Button buttonMove;
    [SerializeField] Button buttonAbility;
    [SerializeField] Button buttonItem;
    [SerializeField] Button buttonWait;
    [SerializeField] Button buttonStatus;
    [Space]

    [Header("Ability Options")]
    [SerializeField] Text ability1;
    [SerializeField] Text ability2;
    [SerializeField] Text ability3;
    [SerializeField] Text ability4;
    public int currentSelection = 0;

    [Space]

    [SerializeField] Button buttonSelectAbility1;
    [SerializeField] Button buttonSelectAbility2;
    [SerializeField] Button buttonSelectAbility3;
    [SerializeField] Button buttonSelectAbility4;

    [Space]

    [Header("Item Options")]
    [SerializeField] Text item1;
    [SerializeField] Text item2;
    [SerializeField] Text item3;
    [SerializeField] Text item4;

    [Space]
    [SerializeField] Button buttonSelectItem1;
    [SerializeField] Button buttonSelectItem2;
    [SerializeField] Button buttonSelectItem3;
    [SerializeField] Button buttonSelectItem4;





    public bool onOption;
    private void Start()
    {
        //ACTION BUTTON
        if (buttonMove != null)
        {
            buttonMove.onClick.AddListener(() =>
            {
                //Debug.Log("SELECT MOVE EVENT");
                UIController.instance.ClickConfirm(0);
            });
        }

        if(buttonAbility != null)
        {
            buttonAbility.onClick.AddListener(() =>
            {
                Debug.Log("SELECT ABILITY EVENT");
                UIController.instance.ClickConfirm(1);
            });
        }


        if (buttonItem != null)
        {
            buttonItem.onClick.AddListener(() =>
            {
                Debug.Log("SELECT ITEM EVENT");
                UIController.instance.ClickConfirm(2);
            });
        }

        if (buttonWait != null)
        {
            buttonWait.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(4);
            });
        }

        if(buttonStatus != null)
        {
            buttonStatus.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(5);
            });
        }
        //ABILITIES BUTTON
        if (buttonSelectAbility1 != null)
        {
            buttonSelectAbility1.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(0);
                UIController.instance.ClickCancel(0);
            });
        }

        if (buttonSelectAbility2 != null)
        {
            buttonSelectAbility2.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(1);
                UIController.instance.ClickCancel(1);

            });

        }

        if (buttonSelectAbility3 != null)
        {
            buttonSelectAbility3.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(2);
                UIController.instance.ClickCancel(2);

            });

            
        }

        if (buttonSelectAbility4 != null)
        {
            buttonSelectAbility4.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(3);
                UIController.instance.ClickCancel(3);

            });
        }

        if (buttonSelectItem1 != null)
        {
            buttonSelectItem1.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(0);
            });
        }

        if (buttonSelectItem2 != null)
        {
            buttonSelectItem2.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(1);
            });
        }

        if (buttonSelectItem3 != null)
        {
            buttonSelectItem3.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(2);
            });
        }

        if (buttonSelectItem4 != null)
        {
            buttonSelectItem4.onClick.AddListener(() =>
            {
                UIController.instance.ClickConfirm(3);
            });
        }



    }

    public void MouseOverEnter(SelectorMovement s)
    {
        selector = UIController.instance.ReturnSelector();
        onOption = true;

        if(selector != null && s.canBeSelected)
        {
            selector.position = new Vector3(selector.position.x, s.transform.position.y, selector.position.z);
        }
     
    }

    public void EnableActionSelection()
    {
        buttonMove.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonAbility.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonItem.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonWait.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonStatus.GetComponent<SelectorMovement>().canBeSelected = true;
    }

    public void DisableActionSelection()
    {
        buttonMove.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonAbility.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonItem.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonWait.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonStatus.GetComponent<SelectorMovement>().canBeSelected = false;
    }

    public void EnableAbilitySelection()
    {
        buttonSelectAbility1.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonSelectAbility2.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonSelectAbility3.GetComponent<SelectorMovement>().canBeSelected = true;
        buttonSelectAbility4.GetComponent<SelectorMovement>().canBeSelected = true;
    }

    public void DisableAbilitySelection()
    {
        buttonSelectAbility1.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonSelectAbility2.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonSelectAbility3.GetComponent<SelectorMovement>().canBeSelected = false;
        buttonSelectAbility4.GetComponent<SelectorMovement>().canBeSelected = false;
    }

    public void DeactivateAllAbilitySelection()
    {
        buttonSelectAbility1.gameObject.SetActive(false);
        buttonSelectAbility2.gameObject.SetActive(false);
        buttonSelectAbility3.gameObject.SetActive(false);
        buttonSelectAbility4.gameObject.SetActive(false);
    }
    public void MouseOverExit(SelectorMovement s)
    {
        onOption = false;    
    }

    //Move selector forward/downwards
    public void MoveForward()
    {
        if(currentSelection < options.Length - 1)
        {
            currentSelection++;
        }
        else
        {
            currentSelection = 0;
        }

        MoveSelector(options[currentSelection].rect.position.y);
    }
    
    //public void OnHover(int index)
    //{
    //    selector.position = new Vector3(selector.position.x, newPosition + offset, selector.position.z);
    //}

    public void MoveBackwards()
    {
        if(currentSelection > 0)
        {
            currentSelection--;
        }
        else
        {
            currentSelection = options.Length-1;
        }

        MoveSelector(options[currentSelection].rect.position.y);
    }

    public void ResetSelector()
    {
        currentSelection = 0;
        MoveSelector(options[currentSelection].position.y);
    }
    void MoveSelector(float newPosition)
    {
        //selector.position = new Vector3(selector.position.x, newPosition + +offset, selector.position.z);
    }


    void DisableOption(Text option)
    {
        option.color = disabledColor;
        option.GetComponent<SelectorMovement>().canBeSelected = false;
    }

    void EnableOption(Text option)
    {
        option.color = defaultColor;
        option.GetComponent<SelectorMovement>().canBeSelected = true;
    }

    public void DisableSelectOption(typeOfAction action)
    {
        switch (action)
        {
            case typeOfAction.Move:
                DisableOption(moveText);
                break;
            case typeOfAction.Ability:
                DisableOption(abilityText);
                break;
            case typeOfAction.Item:
                DisableOption(itemText);
                break;
            case typeOfAction.Wait:
                DisableOption(waitText);
                break;
            default:
                break;
        }
    }

    public void EnableSelectOption(typeOfAction action)
    {
        switch (action)
        {
            case typeOfAction.Move:
                EnableOption(moveText);
                break;
            case typeOfAction.Ability:
                EnableOption(abilityText);
                break;
            case typeOfAction.Item:
                EnableOption(itemText);
                break;
            case typeOfAction.Wait:
                EnableOption(waitText);
                break;
            default:
                break;
        }
    }


    public void EnableSelectAbilty(int ability)
    {
        switch (ability)
        {
            case 0:
                ability1.color = defaultColor;
                ability1.GetComponent<SelectorMovement>().canBeSelected = true;
                break;
            case 1:
                ability2.color = defaultColor;
                ability2.GetComponent<SelectorMovement>().canBeSelected = true;

                break;
            case 2:
                ability3.color = defaultColor;
                ability3.GetComponent<SelectorMovement>().canBeSelected = true;

                break;
            case 3:
                ability4.color = defaultColor;
                ability4.GetComponent<SelectorMovement>().canBeSelected = true;

                break;
        }
    }

    public void DisableSelectAbilty(int ability)
    {
        switch (ability)
        {
            case 0:
                ability1.color = disabledColor;
                ability1.GetComponent<SelectorMovement>().canBeSelected = false;
                break;
            case 1:
                ability2.color = disabledColor;
                ability2.GetComponent<SelectorMovement>().canBeSelected = false;

                break;
            case 2:
                ability3.color = disabledColor;
                ability3.GetComponent<SelectorMovement>().canBeSelected = false;

                break;
            case 3:
                ability4.color = disabledColor;
                ability4.GetComponent<SelectorMovement>().canBeSelected = true;
                break;
        }
    }


    public void ChangeAllActionsToDefault()
    {
        buttonMove.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonAbility.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonItem.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonWait.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonStatus.GetComponent<SelectorMovement>().ChangeToDefault();
    }

    public void ChangeAllAbilitiesToDefault()
    {
        buttonSelectAbility1.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonSelectAbility2.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonSelectAbility3.GetComponent<SelectorMovement>().ChangeToDefault();
        buttonSelectAbility4.GetComponent<SelectorMovement>().ChangeToDefault();
    }

    public void ChangeAllItemsToDefault()
    {
        item1.GetComponent<SelectorMovement>().ChangeToDefault();
        item2.GetComponent<SelectorMovement>().ChangeToDefault();
        item3.GetComponent<SelectorMovement>().ChangeToDefault();
        item4.GetComponent<SelectorMovement>().ChangeToDefault();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTrigger : MonoBehaviour
{

    public Tooltip tooltip;

    private void OnMouseOver()
    {
        tooltip.Show();
    }

    private void OnMouseExit()
    {
        tooltip.Hide();
    }
}

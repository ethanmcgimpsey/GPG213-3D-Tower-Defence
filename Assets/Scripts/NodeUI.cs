﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    
    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = target.GetBuildPosition();
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Sell()
    {
        target.SellTurret();
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }
}

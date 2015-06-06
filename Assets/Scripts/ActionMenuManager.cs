﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models.Combat;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour {
    public delegate void ActionSelectedHandler(BattleAction action);

    public event ActionSelectedHandler OnActionSelected;

    [Tooltip("For when there are enemies nearby, but no friendlies")]
    public GameObject MoveFightFull;

    [Tooltip("For when there are no enemies or friendly units around and the unit can still move")]
    public GameObject MoveBraceItemWait;

    [Tooltip("For when there are no enemies nearby, nor friendlies and the unit cannot move")]
    public GameObject BraceItem;

    [Tooltip("For when you've used an action in place, but can still move.")]
    public GameObject MoveWait;

    [Tooltip("For when you've moved as far as you can, but can still act")] 
    public GameObject FightWaitItem;

    [Tooltip("For when you've chosen to fight and all you can do is attack or brace.")]
    public GameObject AttackBrace;
    

    private GameObject _openMenu;
    private readonly Dictionary<CombatAction, GameObject> _prefabsByActions = new Dictionary<CombatAction, GameObject>();

    public void Start() {
        _prefabsByActions.Add(
            CombatAction.Fight | CombatAction.Wait | CombatAction.Move | CombatAction.Item,
            MoveFightFull
        );

        _prefabsByActions.Add(
            CombatAction.Move | CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
            MoveBraceItemWait
        );

        _prefabsByActions.Add(
            CombatAction.Fight | CombatAction.Wait | CombatAction.Item,
            FightWaitItem
        );

        _prefabsByActions.Add(
            CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
            BraceItem
        );


        _prefabsByActions.Add(
            CombatAction.Wait | CombatAction.Move,
            MoveWait
        );
    }

    public void ShowActionMenu(Grid.Unit unit) {
        var battle = CombatObjects.GetBattleState().Model;

        var availableActionEnums = battle.GetAvailableActions(unit.model);

        var availableActions = availableActionEnums
            .Aggregate((value, next) => value | next);

        var menuPrefab = MoveBraceItemWait;
        if (_prefabsByActions.ContainsKey(availableActions)) {
            menuPrefab = _prefabsByActions[availableActions];
        } else {
            Debug.LogWarning("Could not match menu item.");
        }

        var menu = Instantiate(menuPrefab);
        menu.transform.SetParent(unit.transform, true);
        menu.transform.localPosition = new Vector3(-16, 35, 0);

        _openMenu = menu;
    }

    public void HideCurrentMenu() {
        if (_openMenu != null) {
            Destroy(_openMenu);
            _openMenu = null;
        }
    }

    public void SelectAction(string name) {
        var action = (BattleAction) Enum.Parse(typeof (BattleAction), name);
        if (action == BattleAction.FIGHT) {
            ShowFightSubMenu();
        } else {
            if (OnActionSelected != null) {
                OnActionSelected(action);
            }
        }
    }

    public void ShowFightSubMenu() {
       HideCurrentMenu(); 
    }
}
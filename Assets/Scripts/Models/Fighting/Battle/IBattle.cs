﻿using System.Collections.Generic;
using Contexts.Battle.Models;
using Models.Fighting.AI;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Maps.Triggers;
using Models.Fighting.Skills;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Models.Fighting.Battle {
    public interface IBattle {
        bool ShouldTurnEnd();

        int GetRemainingMoves(ICombatant combatant);

        int TurnNumber { get; }

        IMap Map { get; }

        Signal<string> EventTileWalkedSignal { get; }

        List<IObjective> Objectives { get; set; }

        IActionPlan GetActionPlan(ArmyType army);

        void SubmitAction(ICombatAction action);

        List<string> GetCurrentTurnEvents();

        void ScheduleTurnEvent(int turn, string eventName);

        void EndTurn();

        bool CanMove(ICombatant combatant);

        bool CanAct(ICombatant combatant);

        List<ICombatant> GetAliveByArmy(ArmyType army);
        
        ICombatant GetById(string id);

        void SpawnCombatant(ICombatant combatant);

        BattlePhase NextPhase { get; }

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);

        FinalizedFight FinalizeFight(FightForecast forecast);

        int GetMaxWeaponAttackRange(ICombatant combatant);

        SkillType GetWeaponSkillForRange(ICombatant combatant, int range);

        bool IsWon();

        bool IsLost();

        void AddEventTile(EventTile eventTile);

        EventTile GetEventTile(Vector2 location);

        void RemoveEventTile(Vector2 location);
    }
}
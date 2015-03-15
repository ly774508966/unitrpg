﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DefenderFighting : StateMachineBehaviour {
    private Animator Animator;
    private BattleState State;
	private int numAttacks;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        State = CombatObjects.GetBattleState();

        Grid.Unit defender = State.AttackTarget;

        defender.OnAttackComplete += OnAttackComplete;
        defender.Attack();
    }

    void OnAttackComplete() {
		FightResult result = State.FightResult;
		State.SelectedUnit.TakeDamage(result.CounterAttack.AttackerHits[numAttacks].Damage);
		numAttacks++;

		if (!State.SelectedUnit.IsAlive() || numAttacks >= result.CounterAttack.AttackerHits.Count) {
			State.AttackTarget.ReturnToRest();
			State.SelectedUnit.ReturnToRest();
			Animator.SetTrigger("fight_completed");
		} else {
			State.AttackTarget.Attack();
		}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.AttackTarget.OnAttackComplete -= OnAttackComplete;
    }
}
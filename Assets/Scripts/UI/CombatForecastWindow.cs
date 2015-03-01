﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatForecastWindow : MonoBehaviour {
	private Models.Unit attacker;
	private Models.Unit defender;

	private GameObject attackerHealthText;
	private GameObject attackerDamageText;
	private GameObject attackerHitPctText;
	private GameObject attackerCritPctText;

	private GameObject defenderHealthText;
	private GameObject defenderDamageText;
	private GameObject defenderHitPctText;
	private GameObject defenderCritPctText;

	public void SetUnits(Models.Unit attacker, Models.Unit defender) {

		// TODO: Externalize
		attackerHealthText.GetComponent<Text>().text = attacker.Health.ToString();
		attackerDamageText.GetComponent<Text>().text = (attacker.Character.Strength - defender.Character.Defense).ToString();
		attackerHitPctText.GetComponent<Text>().text = ((attacker.Character.Skill + 70) - defender.Character.Speed).ToString();
		attackerCritPctText.GetComponent<Text>().text = (attacker.Character.Skill - defender.Character.Speed).ToString();

		ChangeText(defenderHealthText, defender.Health.ToString());
		ChangeText(defenderDamageText, (defender.Character.Strength - attacker.Character.Defense).ToString());
		ChangeText(defenderCritPctText, (defender.Character.Skill - attacker.Character.Speed).ToString());
		ChangeText(defenderHitPctText, ((defender.Character.Skill + 70) - attacker.Character.Speed).ToString());
	}

	void Start() {
		attackerHealthText = FindChild("atk_healthValue");
		attackerDamageText = FindChild("atk_damageValue");
	    attackerHitPctText = FindChild("atk_hitPctValue");
		attackerCritPctText = FindChild("atk_critPctText");

		defenderHealthText = FindChild("def_healthValue");
		defenderDamageText = FindChild("def_damageValue");
		defenderHitPctText = FindChild("def_hitPctValue");
		defenderCritPctText = FindChild("def_critPctText");
	}

	private GameObject FindChild(string name) {
		return transform.FindChild(name).gameObject;
	}

	private static void ChangeText(GameObject obj, string text) {
		obj.GetComponent<Text>().text = text;
	}
}

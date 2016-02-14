﻿using System.Collections.Generic;
using Models.Fighting.Buffs;
using Models.Fighting.Equip;
using Models.Fighting.Skills;
using UnityEngine;

namespace Models.Fighting {
    public interface ICombatant {
        int Health { get; }
        void TakeDamage(int amount);
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        Attribute GetAttribute(Attribute.AttributeType type);
        Stat GetStat(StatType type);
        List<IBuff> Buffs { get; }
        void AddBuff(IBuff buff);
        void RemoveBuff(string name);
        HashSet<Weapon> EquippedWeapons { get; } 
        string PrimaryWeapon { get; set; }
        void AddTemporaryBuff(IBuff temporaryBuff);
        void RemoveTemporaryBuff(IBuff temporaryBuff);
    }
}
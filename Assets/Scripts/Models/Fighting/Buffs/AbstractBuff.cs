﻿using System;
using System.Collections.Generic;
using Models.Combat;

namespace Models.Fighting.Buffs {
    public abstract class AbstractBuff : IBuff {
        public Unit Host { get; set; }
        public string Name { get; private set; }

        public IDictionary<StatType, StatMod> StatMods { get; protected set; }

        protected AbstractBuff(string name) {
            StatMods = new Dictionary<StatType, StatMod>();
            Name = name;
        }

        protected StatMod CreateMod(Func<int, int> modifierFuntion) {
            return new StatMod(Name, modifierFuntion);            
        }

        public virtual bool CanApply(IBattle battle) {
            return true;
        }

        public virtual IEffect Modify(IEffect effect) {
            return effect;
        }

        public virtual Attribute Apply(Attribute attribute) {
            return attribute;
        }

        public virtual Attribute UnApply(Attribute attribute) {
            return attribute;
        }
    }
}
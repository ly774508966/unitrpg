﻿

using Assets.Contexts.Chapters.Chapter2.Commands;
using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Signals;
using Assets.Contexts.Chapters.Chapter2.Views;
using Contexts.Battle;
using Contexts.Battle.Signals;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2 {
    public class Chapter2Context : BattleContext {
        public Chapter2Context(MonoBehaviour view) : base(view) {
        }

        protected override void mapBindings() {
            base.mapBindings();

            injectionBinder.Bind<MarkHouseVisitedSignal>().ToSingleton();
            injectionBinder.Bind<HouseLightTransitionCompleteSignal>().ToSingleton();
            injectionBinder.Bind<HouseLightDisableSignal>().ToSingleton();
            injectionBinder.Bind<HouseLightEnableSignal>().ToSingleton();

            injectionBinder.Bind<EastmerePlazaState>().ToSingleton();

            mediationBinder.Bind<Chapter2View>().To<Chapter2ViewMediator>();
            mediationBinder.Bind<Chapter2HouseView>().To<Chapter2HouseMediator>();

            commandBinder.Bind<MarkHouseVisitedSignal>().To<MarkHouseVisitedCommand>();
            commandBinder.GetBinding<BattleStartSignal>().To<BindChapter2BattleEventsCommand>().InParallel();
        }
    }
}
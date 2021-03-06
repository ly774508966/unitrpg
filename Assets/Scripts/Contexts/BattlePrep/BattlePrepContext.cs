﻿using Assets.Contexts.Base;
using Contexts.BattlePrep.Commands;
using Contexts.BattlePrep.Signals;
using Contexts.BattlePrep.Signals.Public;
using Contexts.BattlePrep.Views;
using Models.SaveGames;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.BattlePrep {
    public class BattlePrepContext : BaseContext {
        public BattlePrepContext(MonoBehaviour view) : base(view) {
        }

        public override void Launch() {
            base.Launch();
            var startSignal = injectionBinder.GetInstance<ShowBattlePrepSignal>();
            startSignal.Dispatch();
        }

        protected override void mapBindings() {
            base.mapBindings();

            if (this == Context.firstContext) {
                injectionBinder.Unbind<ISaveGameRepository>();
                injectionBinder.Bind<ISaveGameRepository>().ToValue(new TestingSaveGameRepository());
            }

            injectionBinder.Bind<HidePrepSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ClosePrepSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ShowBattlePrepSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<ShowBattlePrepSignal>()
                .To<BattlePrepStartCommand>()
                .To<ShowPrepCommand>().InSequence();

            injectionBinder.Bind<TransitionOutSignal>().ToSingleton();
            injectionBinder.Bind<TransitionInSignal>().ToSingleton();
            injectionBinder.Bind<TransitionCompleteSignal>().ToSingleton();
            injectionBinder.Bind<ActionSelectedSignal>().ToSingleton();

            commandBinder.Bind<ActionSelectedSignal>()
                .To<ActionSelectedCommand>();

            commandBinder.Bind<ClosePrepSignal>()
                .To<ClosePrepCommand>();

            commandBinder.Bind<HidePrepSignal>()
                .To<HidePrepCommand>();

            mediationBinder.Bind<BattlePrepView>().To<BattlePrepViewMediator>();    
        }

    }
}
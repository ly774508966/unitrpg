﻿
using System;
using Assets.Contexts.Common.Services;
using C5;
using Contexts.Common.Model;
using Contexts.Global.Signals;

namespace Assets.Scripts.Contexts.Global.Models {
    public class Storyboard {
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        public ApplicationState State { get; set; }


        private readonly LinkedList<IStoryboardScene> _scenes;

        private int _storyboardIndex = 0;

        public int StorboardIndex {
            get { return _storyboardIndex; }
            set {
                if (value >= _scenes.Count) {
                    throw new ArgumentException("Cannot skip to scene index " + value + ". Too high.");
                }

                _storyboardIndex = value;
            }
        }

        [Construct]
        public Storyboard(ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            ChangeSceneSignal = changeSceneSignal;
            State = state;

            _scenes = new LinkedList<IStoryboardScene> {
               new StoryboardScene("male_soldier_report"),
               new StoryboardScene("female_soldier_report"),
               new StoryboardScene("liat_janek_prep"),
               new StoryboardScene("liat_audric_h2h"),
               new StoryboardScene("liat_audric_balcony"),
               new StoryboardScene("liat_audric_overlook"),
               new StoryboardScene("chapter_1_battle"),
               new StoryboardScene("chapter_2_battle")
            };
        }

        public IStoryboardScene GetAndIncrementNextScene() {
            var result = _scenes[_storyboardIndex];
            _storyboardIndex++;
            return result;
        }
    }
}
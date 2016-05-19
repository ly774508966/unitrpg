﻿using System;
using System.Collections.Generic;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Stateless;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class BubbleMenuView : View {
        private readonly float _showStaggerStepSeconds = 0.1f;
        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly Vector3 _buttonScaleFactor = new Vector3(.25f, .25f, .25f);
        private BubbleMenuUtils.MenuStateMachine _stateMachine;

        public Signal<string> ItemSelectedSignal = new Signal<string>();

        // Contains a list of degree offsets for each number of concurrently-visible buttons.
        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 40f, -40f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } }
        };

        public void Show(HashSet<BubbleMenuItem> config) {
            _stateMachine = BubbleMenuUtils.CreateStateMachine(config);
            _stateMachine.SelectSignal.AddListener(Select);
            _stateMachine.ChangeLevelSignal.AddListener(ShowLevel);
            var prefabNames = GetAllNames(config);

            foreach (var buttonName in prefabNames) {
                var path = "MenuBubbles/" + buttonName;
                var child = Instantiate(Resources.Load(path)) as GameObject;
                if (child == null) {
                    throw new ArgumentException("Could not find menu item by name " + buttonName);
                }
                child.transform.SetParent(transform);
            }
        }

        private List<string> GetAllNames(HashSet<BubbleMenuItem> items) {
            return GetAllNames(new List<string>(), items);
        }

        private List<string> GetAllNames(List<string> names, HashSet<BubbleMenuItem> items) {
            foreach (var item in items) {
                names = GetAllNames(names, item);
            }

            return names;
        }

        private List<string> GetAllNames(List<string> names, BubbleMenuItem item) {
            names.Add(item.Name);
            if (item.IsLeaf()) {
                return names;
            }

            return GetAllNames(names, item.Children);
        }

        public void Hide() {
            // transition closed
            _stateMachine.SelectSignal.RemoveListener(Select);
            _stateMachine.ChangeLevelSignal.RemoveListener(ShowLevel);
        }

        private void ShowLevel(string level) {
            // transition between levels
        }

        private void Select(string item) {
            // Just relay up
            ItemSelectedSignal.Dispatch(item);
        }
    }
}
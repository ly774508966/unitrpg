﻿using System.Collections.Generic;
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

        public Signal<string> ItemSelectedSignal = new Signal<string>();

        // Contains a list of degree offsets for each number of concurrently-visible buttons.
        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 40f, -40f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } }
        };
    }
}
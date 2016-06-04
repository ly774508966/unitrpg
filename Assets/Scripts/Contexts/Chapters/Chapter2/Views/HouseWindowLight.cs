﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    [RequireComponent(typeof(tk2dSprite))]
    public class HouseWindowLight : MonoBehaviour {
        private tk2dSprite _sprite;

        public Tween GetTurnOutTween() {
            return _sprite.DOScale(Vector3.zero, 0.3f);
        } 

        void Awake() {
            _sprite = GetComponent<tk2dSprite>();
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Combat.ScriptedEvents;
using DG.Tweening;
using Grid;
using UnityEngine;
using Unit = Grid.Unit;

public class BattleManager : SceneEntryPoint {
    public bool StartImmediately;
    public List<GameObject> TriggerObjects;

    private Animator _battleStateMachine;
    private Camera _camera;
    private UnitManager _unitManager;
    private MapGrid _grid;

    private List<SpawnableUnit> _scheduledReinforcements = new List<SpawnableUnit>(); 
    private readonly Dictionary<Vector2, List<CombatEvent>> _triggersByGridPosition = new Dictionary<Vector2, List<CombatEvent>>(); 

    public void Start() {
        _battleStateMachine = GetComponent<Animator>();
        _camera = CombatObjects.GetCamera().GetComponent<Camera>();
        _unitManager = CombatObjects.GetUnitManager();

        var triggers = FindObjectsOfType<CombatEvent>();
        _grid = CombatObjects.GetMap();
        foreach (var trigger in triggers) {
            var triggerPosition = trigger.gameObject.transform.position;
            var position = _grid.GridPositionForWorldPosition(triggerPosition);

            if (!_triggersByGridPosition.ContainsKey(position)) {
                _triggersByGridPosition[position] = new List<CombatEvent>();
            }

            _triggersByGridPosition[position].Add(trigger);
        }


        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public IEnumerator RunTriggeredEvents(Vector2 destination) {
        if (!_triggersByGridPosition.ContainsKey(destination)) {
            yield return null;
        } else {
            var matchingTriggers = _triggersByGridPosition[destination];

            foreach (var trigger in matchingTriggers) {
                yield return StartCoroutine(trigger.Play());
            }
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }

    public void ScheduleReinforcements(List<SpawnableUnit> units) {
        _scheduledReinforcements.AddRange(units);
        _battleStateMachine.SetBool("reinforcements_triggered", true);
    }

    public IEnumerator SpawnReinforcements() {
        var originalCameraPosition = _camera.transform.position;

        foreach (var unit in _scheduledReinforcements) {
            var spawn = unit.SpawnPoint;
            var worldSpaceSpawnPoint = _grid.GetWorldPosForGridPos(spawn);

            // Maintain the camera's existing z position.
            var newCameraPos = new Vector3(worldSpaceSpawnPoint.x, worldSpaceSpawnPoint.y, _camera.transform.position.z);

            yield return _camera.transform.DOMove(newCameraPos, 0.7f)
                .SetEase(Ease.OutCubic)
                .WaitForCompletion();

            var unitGameObject = Instantiate(unit.Prefab);
            unitGameObject.transform.SetParent(_unitManager.transform);

            var unitComp = unitGameObject.GetComponent<Grid.Unit>();
            unitComp.gridPosition = unitComp.model.GridPosition = spawn;

            yield return StartCoroutine(_unitManager.SpawnUnit(unitGameObject));
        }

        yield return _camera.transform.DOMove(originalCameraPosition, 0.7f)
            .SetEase(Ease.OutCubic)
            .WaitForCompletion();

        _scheduledReinforcements = new List<SpawnableUnit>();
    }
}

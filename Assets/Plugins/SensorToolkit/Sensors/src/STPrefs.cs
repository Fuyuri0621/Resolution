﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Micosmo.SensorToolkit {
    public class STPrefs : ScriptableObject {

        public static Color defaultCyan = new Color(51 / 255f, 1f, 1f);
        static Color[] defaultVisibilityGradientColours = new Color[] {
            new Color(.2f, 1f, 1f),
            new Color(.21f, 1f, .74f),
            new Color(.21f, 1f, .47f),
            new Color(.22f, 1f, .22f),
            new Color(.48f, 1f, .23f),
            new Color(.75f, 1f, .23f),
            new Color(1f, 1f, .24f),
            new Color(1f, .75f, .25f),
            new Color(1f, .5f, .25f),
            new Color(1f, .26f, .26f)
        };

        public static Color RedEditorTextColour => Instance?.redEditorTextColour ?? new Color(1f, .2f, .2f);
        public static Color ActiveSensorEditorColour => Instance?.activeSensorEditorColour ?? new Color(51 / 255f, 1f, 1f, .4f);
        public static Color SignalBoundsColour => Instance?.signalBoundsColour ?? defaultCyan;
        public static bool ShowEyeIconInSignal => Instance?.showEyeIconInSignal ?? true;
        public static Color RangeColour => Instance?.rangeColour ?? defaultCyan;
        public static Color CastingRayColour => Instance?.castingRayColour ?? defaultCyan;
        public static Color CastingBlockedRayColour => Instance?.castingBlockedRayColour ?? Color.red;
        public static Color CastingShapeColour => Instance?.castingShapeColour ?? Color.green;
        public static Color RayHitNormalColour => Instance?.rayHitNormalColour ?? Color.yellow;
        public static Color LOSFovColour => Instance?.losFovColour ?? Color.yellow;
        public static Color[] RayVisibilityGradient => Instance?.rayVisibilityGradient ?? defaultVisibilityGradientColours;
        public static Color LOSRayBlockedColour => Instance?.losRayBlockedColour ?? Color.red;
        public static Color SteeringVectorColour => Instance?.steeringVectorColour ?? Color.cyan;
        public static Color InterestColour => Instance?.interestColour ?? Color.yellow;
        public static Color DangerColour => Instance?.dangerColour ?? Color.red;
        public static Color LowSpeedColour => Instance?.lowSpeedColour ?? Color.blue;
        public static Color CollisionSpeedColour => Instance?.collisionSpeedColour ?? new Color(69f / 255, 6f / 255, 46f / 255);
        public static Color HighSpeedColour => Instance?.highSpeedColour ?? new Color(0.8f, 1f, 1f);
        public static Color DecisionColour => Instance?.decisionColour ?? Color.green;

        [Header("Sensor Editors")]
        [SerializeField] Color redEditorTextColour = new Color(1f, .2f, .2f);
        [SerializeField] Color activeSensorEditorColour = new Color(51 / 255f, 1f, 1f, .4f);

        [Header("Detected Signal Widgets")]
        [SerializeField] Color signalBoundsColour = defaultCyan;
        [SerializeField] bool showEyeIconInSignal = true;

        [Header("Range Sensor Widgets")]
        [SerializeField] Color rangeColour = defaultCyan;

        [Header("Casting Sensor Widgets")]
        [SerializeField] Color castingRayColour = defaultCyan;
        [SerializeField] Color castingBlockedRayColour = Color.red;
        [SerializeField] Color castingShapeColour = Color.green;
        [SerializeField] Color rayHitNormalColour = Color.yellow;

        [Header("LOS Sensor Widgets")]
        [SerializeField] Color losFovColour = Color.yellow;
        [SerializeField] Color[] rayVisibilityGradient = defaultVisibilityGradientColours;
        [SerializeField] Color losRayBlockedColour = Color.red;

        [Header("Steering Sensor Widgets")]
        [SerializeField] Color steeringVectorColour = Color.cyan;
        [SerializeField] Color dangerColour = Color.red;
        [SerializeField] Color interestColour = Color.yellow;
        [SerializeField] Color lowSpeedColour = Color.blue;
        [SerializeField] Color collisionSpeedColour = new Color(69f / 255, 6f / 255, 46f / 255);
        [SerializeField] Color highSpeedColour = new Color(0.8f, 1f, 1f);
        [SerializeField] Color decisionColour = Color.green;

        static STPrefs instance;
        static STPrefs Instance {
            get {
#if UNITY_EDITOR
                if (instance == null) {
                    string[] guids = AssetDatabase.FindAssets("t:STPrefs");
                    if (guids.Length > 1) {
                        for (var i = 1; i < guids.Length; i++) {
                            Debug.LogError("Duplicate SensorToolkit preferences: " + guids[i]);
                        }
                    }
                    if (guids.Length > 0) {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<STPrefs>(path);
                    }
                }
                return instance;
#else
                return null;
#endif
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class NPCDebuggerWindow : EditorWindow
    {
        private GameObject selectedNPC;
        private List<PieChartData> emotionData;
        private List<float> debugEmotionValues;

        private Vector2 scrollPosition;
        private bool showHistory;

        [MenuItem("Tools/RaafOritme/Smart NPCs/NPC Debugger")]
        public static void ShowWindow()
        {
            GetWindow<NPCDebuggerWindow>("NPC Debugger");
        }

        private void OnEnable()
        {
            emotionData = new();
            debugEmotionValues = new();
            for (int i = 0; i < Enum.GetValues(typeof(EmotionType)).Length; i++)
            {
                debugEmotionValues.Add(0);
            }
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            selectedNPC = Selection.activeGameObject;
            Repaint();
        }

        private void OnGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                GUILayout.Label("Please enter play mode for debugging...", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.BeginVertical();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            if (selectedNPC != null)
            {
                AgentController npcAI = selectedNPC.GetComponent<AgentController>();
                if (npcAI != null)
                {
                    if (npcAI.mainBrain.GetType() == typeof(BrainGOAP))
                    {
                        GUILayout.Label("Debugging NPC GOAP", EditorStyles.boldLabel);
                        BrainGOAP brainType = (BrainGOAP)npcAI.mainBrain;

                        GUILayout.Space(10);
                        GUILayout.Label("Actions", EditorStyles.boldLabel);
                        GUILayout.Label($"Current Action: {brainType.currentAction}");
                        showHistory = EditorGUILayout.Foldout(showHistory, "Last actions");
                        if (showHistory)
                        {
                            for (int i = 0; i < brainType.lastActions.Count; i++)
                            {
                                EditorGUILayout.LabelField(brainType.lastActions[i]);
                            }
                        }

                        GUILayout.Space(10);
                        GUILayout.Label("Utilities", EditorStyles.boldLabel);
                        DrawProgressBar(brainType.decisionSystem.utilities.hunger / 100f, "Hunger");
                        DrawProgressBar(brainType.decisionSystem.utilities.fatigue / 100f, "Fatigue");

                        GUILayout.Space(10);
                        UpdateEmotionData(brainType);
                        DrawPieChart(emotionData);
                        GUILayout.Label($"Emotional State: {brainType.decisionSystem.emotionSystem.emotionalState.GetLeadingEmotion()}");

                        GUILayout.Space(5);
                        EditorGUILayout.LabelField("Debug Values", EditorStyles.boldLabel);
                        for (int i = 0; i < emotionData.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(emotionData[i].categoryName, GUILayout.Width(100));
                            debugEmotionValues[i] = EditorGUILayout.FloatField(debugEmotionValues[i]);
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Apply Values"))
                        {
                            ApplyDebugValues(brainType);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Debugging NPC FSM", EditorStyles.boldLabel);
                        BrainFSM brainType = (BrainFSM)npcAI.mainBrain;

                        GUILayout.Space(10);
                        GUILayout.Label("Actions", EditorStyles.boldLabel);
                        GUILayout.Label($"Current Action: {brainType.currentAction}");
                        showHistory = EditorGUILayout.Foldout(showHistory, "Last actions");
                        if (showHistory)
                        {
                            for (int i = 0; i < brainType.lastActions.Count; i++)
                            {
                                EditorGUILayout.LabelField(brainType.lastActions[i]);
                            }
                        }
                        GUILayout.Label($"Next Action: {brainType.stateRoutine.Peek().GetType().Name}");
                    }
                }
                else
                {
                    GUILayout.Label("Select a npc for the debugging features with an agent controller component.", EditorStyles.boldLabel);
                }
            }
            else
            {
                GUILayout.Label("Select a npc for the debugging features.", EditorStyles.boldLabel);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawProgressBar(float value, string label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(75));
            Rect rect = GUILayoutUtility.GetRect(50, 20);
            EditorGUI.ProgressBar(rect, value, (value * 100f).ToString() + "%");
            GUILayout.EndHorizontal();
        }

        private void DrawPieChart(List<PieChartData> _data)
        {
            EditorGUILayout.LabelField("NPC Emotions", EditorStyles.boldLabel);

            Rect pieChartRect = EditorGUILayout.GetControlRect(false, 250); // Reserve space for pie chart
            float inspectorWidth = EditorGUIUtility.currentViewWidth;
            Vector2 center = new Vector2(inspectorWidth * 0.5f, pieChartRect.y + pieChartRect.height * 0.5f);

            DrawPieChart(center, _data);

            foreach (PieChartData item in _data)
            {
                EditorGUILayout.LabelField($"{item.categoryName}: {item.value}%");
            }
        }

        private void DrawPieChart(Vector2 _center, List<PieChartData> _data)
        {
            List<Color> colors = GeneratePieColors(Color.green, _data.Count);
            float total = 0f;
            foreach (PieChartData item in _data)
            {
                total += item.value;
            }

            if (total == 0)
            {
                // No need to continue with the logic if there is no data.
                return;
            }

            float startAngle = 0f;
            for (int i = 0; i < _data.Count; i++)
            {
                float angle = 360f * (_data[i].value / total);
                DrawSegment(_center, 100, startAngle, angle, colors[i]);

                if (_data[i].value > 0)
                {
                    float midAngle = startAngle + angle / 2f;
                    Vector2 labelPosition = new Vector3(_center.x, _center.y) + Quaternion.Euler(0, 0, midAngle) * Vector3.up * position.width * 0.1f;
                    Handles.Label(labelPosition, _data[i].categoryName, new GUIStyle
                    {
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter,
                        normal = new GUIStyleState { textColor = Color.black }
                    });
                }

                startAngle += angle;
            }
        }

        private void DrawSegment(Vector2 _center, float _radius, float _startAngle, float _angle, Color _color)
        {
            Handles.color = _color;
            Vector3 from = Quaternion.Euler(0, 0, _startAngle) * Vector3.up;
            Handles.DrawSolidArc(_center, Vector3.forward, from, _angle, _radius);
        }

        private List<Color> GeneratePieColors(Color _baseColor, int _sliceCount)
        {
            Color.RGBToHSV(_baseColor, out float baseHue, out float baseSaturation, out float baseValue);
            List<Color> colors = new List<Color>();

            for (int i = 0; i < _sliceCount; i++)
            {
                float hueOffset = (i / (float)_sliceCount) * 0.9f;
                float newHue = (baseHue + hueOffset) % 1f;
                colors.Add(Color.HSVToRGB(newHue, baseSaturation, baseValue));
            }

            return colors;
        }

        private void UpdateEmotionData(BrainGOAP _brain)
        {
            emotionData.Clear();

            foreach(EmotionType emotion in Enum.GetValues(typeof(EmotionType)))
            {
                PieChartData data = new(emotion.ToString(), _brain.decisionSystem.emotionSystem.emotionalState.GetEmotionValue(emotion));
                emotionData.Add(data);
            }
        }

        private void ApplyDebugValues(BrainGOAP _brain)
        {
            for (int i = 0; i < emotionData.Count; i++)
            {
                _brain.decisionSystem.emotionSystem.emotionalState.SetType(emotionData[i].categoryName, debugEmotionValues[i]);
            }
        }
    }
}

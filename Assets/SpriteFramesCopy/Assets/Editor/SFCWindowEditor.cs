using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Yaml.Serialization;
using System.IO;
using System.Yaml;
using System.Collections.Generic;

public class SFCWindowEditor : EditorWindow
{
    GUIStyle guiTitleStyle;

    ScriptableObject targetObject;
    SerializedObject target;

    SerializedProperty textureToCopyFromSerialized;
    SerializedProperty texturesToPasteFramesSerialized;

    public Texture2D textureToCopyFrom;
    public List<Texture2D> texturesToPasteFrames = new List<Texture2D>();
    static Vector2 scrollViewPos;

    static bool showTexturesToPasteFrames = false;

    [MenuItem("Tools/SpriteFrames Copy")]
    public static void ShowSFCWindow()
    {
        EditorWindow window = GetWindow(typeof(SFCWindowEditor));
        window.titleContent = new GUIContent("SpriteFrames Copy");
    }

    private void OnEnable()
    {
        guiTitleStyle = new GUIStyle();
        guiTitleStyle.fontSize = 14;
        guiTitleStyle.fontStyle = FontStyle.Bold;
        guiTitleStyle.margin = new RectOffset(5, 0, 10, 0);

        targetObject = this;
        target = new SerializedObject(targetObject);

        textureToCopyFromSerialized = target.FindProperty("textureToCopyFrom");
        texturesToPasteFramesSerialized = target.FindProperty("texturesToPasteFrames");
    }

    void OnGUI()
    {
        target.Update();


        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            GUILayout.Label("SpriteFrames Copy", guiTitleStyle);

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(textureToCopyFromSerialized, new GUIContent("Copy Frames from:"), true);
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Paste Frames to: ");

            if (DropAreaTextures()) return;

            if (texturesToPasteFrames.Count > 0)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Textures");
                    if (GUILayout.Button(showTexturesToPasteFrames ? "Hide" : "Show", GUILayout.MaxWidth(100))) showTexturesToPasteFrames = !showTexturesToPasteFrames;
                }
                EditorGUILayout.EndHorizontal();

                if (showTexturesToPasteFrames)
                {
                    GUILayout.Space(5);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        GUILayout.Space(5);

                        scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos, false, false);
                        {
                            for (int i = 0; i < texturesToPasteFrames.Count; i++)
                            {
                                SerializedProperty textureToPasteFramesItemSerialized = texturesToPasteFramesSerialized.GetArrayElementAtIndex(i);

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.PropertyField(textureToPasteFramesItemSerialized, new GUIContent(""), GUILayout.MaxWidth(150));
                                    if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                                    {
                                        texturesToPasteFrames.RemoveAt(i);
                                        break;
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                GUILayout.Space(5);
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            else EditorGUILayout.HelpBox("No Texture2D to show yet. Drag and drop some textures!", MessageType.Warning);

            GUILayout.Space(10);

            if(GUILayout.Button("Apply", GUILayout.MaxWidth(100)))
            {
                if(textureToCopyFrom == null)
                {
                    Debug.Log("[SFC] There is no Texture2D to copy the frames from!");
                    return;
                }
                if(texturesToPasteFrames.Count == 0)
                {
                    Debug.Log("[SFC] There is no Texture2D to paste the frames to! Drag and drop some textures to the list through the inspector.");
                    return;
                }
                string path = AssetDatabase.GetAssetPath(textureToCopyFrom);
                path += ".meta";

                YamlSequence spritesNode = null;
                YamlNode spriteMode = null;
                YamlNode[] yamlNodes = YamlNode.FromYamlFile(path);
                {
                    YamlMapping rootMap = (YamlMapping)yamlNodes[0];
                    YamlMapping textureImporter = (YamlMapping)rootMap["TextureImporter"];
                    YamlMapping spriteSheet = (YamlMapping)textureImporter["spriteSheet"];
                    spriteMode = textureImporter["spriteMode"];
                    spritesNode = (YamlSequence)spriteSheet["sprites"];
                }
             
                for(int s = 0; s < texturesToPasteFrames.Count; s++)
                {
                    path = AssetDatabase.GetAssetPath(texturesToPasteFrames[s]);
                    path += ".meta";

                    YamlSequence copyToSpritesNode = null;
                    YamlNode[] currYamlNodes = YamlNode.FromYamlFile(path);

                    YamlMapping rootMap = (YamlMapping)currYamlNodes[0];
                    {
                        YamlMapping textureImporter = (YamlMapping)rootMap["TextureImporter"];
                        YamlMapping spriteSheet = (YamlMapping)textureImporter["spriteSheet"];
                        copyToSpritesNode = (YamlSequence)spriteSheet["sprites"];

                        //Force the spriteMode to be 2, 
                        // 0 - is none selected
                        // 1 - is Single
                        // 2 - is Multiple (for multiple frames)
                        textureImporter["spriteMode"] = 2;
                    }
                    
                    copyToSpritesNode.Clear();

                    string srcTextN = textureToCopyFrom.name;
                    for (int frames = 0; frames < spritesNode.Count; frames++)
                    {
                        YamlNode sframe = spritesNode[frames];

                        YamlMapping frame = (YamlMapping)sframe;

                        
                        string currSpriteN = texturesToPasteFrames[s].name.Replace("\"", "");
                        string currentFrameName = frame["name"].ToString().Replace("\"", "").Replace(srcTextN, currSpriteN);

                        frame["name"] = currentFrameName;// currSpriteN + "_" + frames.ToString();

                        Debug.Log(frame["name"]);
                        copyToSpritesNode.Add(spritesNode[frames]);
                    }

                    // Remove the first two lines that contains YAML and ---
                    string yamlMetafile = rootMap.ToYaml();
                    int lineCount = 0;
                    int y = 0;
                    for(y = 0; y < yamlMetafile.Length; y++)
                    {
                        if(yamlMetafile[y] == '\n')
                        {
                            lineCount++;
                            if (lineCount >= 2) break;
                        }
                    }

                    yamlMetafile = yamlMetafile.Remove(0, y);

                    // Write all the text to the path we loaded it
                    File.WriteAllText(path, yamlMetafile);
                }

                // Refresh the assets
                AssetDatabase.Refresh();
            }

            GUILayout.Space(10);
        }
        EditorGUILayout.EndVertical();

        target.ApplyModifiedProperties();
    }

    bool DropAreaTextures()
    {
        GUILayout.Space(5);

        bool droped = false;

        Event currentEvent = Event.current;

        Rect dragAndDropRect = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dragAndDropRect, "Drag and Drop Texture2D here");

        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dragAndDropRect.Contains(currentEvent.mousePosition)) break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (currentEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object obj in DragAndDrop.objectReferences)
                    {
                        Texture2D objDroped = obj as Texture2D;
                        if (!objDroped || objDroped.GetType() != typeof(Texture2D)) continue;

                        if(texturesToPasteFrames.Contains(objDroped))
                        {
                            Debug.Log("[SFC] The texture " + objDroped.name + " is already on the list.");
                            continue;
                        }

                        texturesToPasteFrames.Add(objDroped);
                        droped = true;
                    }
                }
                Event.current.Use();
                break;
        }

        GUILayout.Space(10);
        return droped;
    }
}

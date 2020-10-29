using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public static DebugController _instance;
    
    private bool showConsole;
    private bool showHelp;

    private string input;

    private PlayerControls _playerControls;

    public static DebugCommand RESTART_LEVEL;
    public static DebugCommand SHOW_HELP;

    public List<object> commandList;

    public bool IsInConsole => showConsole;

    private void Awake()
    {
        _instance = this;
        
        _playerControls = new PlayerControls();

        _playerControls.Player.ToggleDebug.performed += context => OnToggleDebug();

        _playerControls.Player.ReturnDebug.performed += context => OnReturn(); 
        
        RESTART_LEVEL = new DebugCommand("restart_lvl", "Restarts the current level", "restart_lvl", () =>
        {
            var currentLevel = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(currentLevel.name);
        });
        
        SHOW_HELP = new DebugCommand("help", "Shows all commands", "help", () =>
        {
            showHelp = true;
        });
        
        commandList = new List<object>
        {
            RESTART_LEVEL,
            SHOW_HELP
        };
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void OnReturn()
    {
        if (!showConsole) return;
        
        HandleInput();
        input = "";
    }

    private void OnToggleDebug()
    {
        showConsole = !showConsole;

        Cursor.lockState = showConsole ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private Vector2 scroll;

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0,y, Screen.width,100), "" );
            
            Rect viewport = new Rect(0,0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.CommandFormat} - {command.CommandDescription}";
                
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100,  20);
                
                GUI.Label(labelRect, label);
            }
            
            GUI.EndScrollView();
            
            y += 100;
        }
        
        GUI.Box(new Rect(0,y, Screen.width,30), "" );
        GUI.backgroundColor = new Color(0f, 0.98f, 1f);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput()
    {
        if (input == null)
        {
            return;
        }
        
        string[] properties = input.Split(' ');
        
        foreach (var command in commandList)
        {

            var baseCommand = command as DebugCommandBase;

            if (input.Contains(baseCommand?.CommandID ?? string.Empty))
            {
                switch (command)
                {
                    case DebugCommand debugCommand:
                    {
                        debugCommand.Invoke();

                        break;
                    }
                    case DebugCommand<int> debugCommand:
                    {
                        debugCommand.Invoke(int.Parse(properties[1]));
                    
                        break;
                    }
                    
                }    
            }
            
        }   
    }
}

using UnityEngine;
using BepInEx;

public class ConsoleUnlocker : MonoBehaviour
{
    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private void Start()
    {
        ConsoleMain.active = true;
        LoadKeybinds();
    }

    void Update()
    {
        foreach (var keybind in keybinds)
        {
            if (ConsoleMain.liteVersion)
            {
                ConsoleMain.liteVersion = false;
            }
            if (UnityEngine.Input.GetKeyDown(keybind.Value))
            {
                if (keybind.Key == "TogglePause")
                {
                    Debug.Log("TogglePause");
                    if (Time.timeScale != 0.0f) 
                    { 
                        Time.timeScale = 0.0f;
                    }
                    else 
                    {
                        Time.timeScale = 1.0f;
                    }
                }
                else if (keybind.Key == "CameraFly")
                {
                    Debug.Log("CameraFly");
                    GameObject.Find("Game/ConsoleCall").GetComponent<ConsoleCall>().CameraFly(true);
                }
                else if (keybind.Key == "CameraLight")
                {
                    Debug.Log("CameraLight");
                    GameObject.Find("Game/ConsoleCall").GetComponent<ConsoleCall>().CameraLight();
                }
                else
                {
                    Debug.Log(keybind.Key);
                    ConsoleCommandsGame.Command(keybind.Key);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        ConsoleMain.active = false;
        ConsoleMain.liteVersion = true;
    }

    private void LoadKeybinds()
    {
        string path = Path.Combine(Paths.PluginPath + "\\" + PluginInfo.PLUGIN_GUID, "keybinds.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            try
            {
                // Normalize JSON to handle unexpected newlines or whitespace
                json = json.Replace("\n", "").Replace("\r", "").Trim(new char[] { '{', '}' }); // Remove outer braces and newlines

                Dictionary<string, string> rawKeybinds = new Dictionary<string, string>();
                string[] entries = json.Split(',');

                foreach (var entry in entries)
                {
                    string[] kv = entry.Split(':');
                    if (kv.Length == 2)
                    {
                        string command = kv[0].Trim(new char[] { '\"', ' ' });
                        string keyString = kv[1].Trim(new char[] { '\"', ' ' });

                        if (System.Enum.TryParse(keyString, out KeyCode key))
                        {
                            rawKeybinds[command] = keyString;
                            keybinds[command] = key;
                        }
                        else
                        {
                            Debug.LogWarning($"Invalid keybind: {command} -> {keyString}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Malformed keybind entry: {entry}");
                    }
                }

                // Debug log the parsed keybind configuration
                foreach (var keybind in rawKeybinds)
                {
                    Debug.Log($"Keybind loaded: {keybind.Key} -> {keybind.Value}");
                }

                // Ensure F9 and Slash are bound to default commands if not in the JSON
                if (!keybinds.ContainsKey("TogglePause"))
                {
                    keybinds["TogglePause"] = KeyCode.F9;
                    Debug.Log("Default keybind set: TogglePause -> F9");
                }
                if (!keybinds.ContainsKey("CameraFly"))
                {
                    keybinds["CameraFly"] = KeyCode.Slash;
                    Debug.Log("Default keybind set: CameraFly -> Slash");
                }
                if (!keybinds.ContainsKey("CameraLight"))
                {
                    keybinds["CameraLight"] = KeyCode.L;
                    Debug.Log("Default keybind set: CameraLight -> L");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing keybinds.json: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError($"Keybind configuration file not found at {path}");

            // Set default keybinds if the configuration file is not found
            keybinds["TogglePause"] = KeyCode.F9;
            Debug.Log("Default keybind set: TogglePause -> F9");

            keybinds["CameraFly"] = KeyCode.Slash;
            Debug.Log("Default keybind set: CameraFly -> Slash");

            keybinds["CameraLight"] = KeyCode.L;
            Debug.Log("Default keybind set: CameraLight -> L");
        }
    }

}

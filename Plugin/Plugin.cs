using UnityEditor;
using UnityEngine;

public class ConsoleUnlocker : MonoBehaviour
{

    static CapsuleCollider showerCapsule = null;
    static GameObject blur = null;

    private void Start()
	{
        ConsoleMain.active = true;
    }

    private static int unlocked = 0;
    void Update()
	{
        if (unlocked == 1)
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
        }
        else if (unlocked == 2)
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            unlocked = 0;
        }

        if (ConsoleMain.liteVersion)
		{
			ConsoleMain.liteVersion = false;
		}
        if (UnityEngine.Input.GetKeyDown(KeyCode.U))
        {
            if (unlocked == 1)
            {
                unlocked = 2;
            }
            else
            {
                unlocked = 1;
            }
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.F9)){
			if (Time.timeScale != 0.0f)
				Time.timeScale = 0.0f;
			else
				Time.timeScale = 1.0f;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Slash))
        {
            if (Reflection.FindObjectsOfType<ConsoleCall>(true).Length > 0)
            {
                ConsoleCall camerafly = Reflection.FindObjectsOfType<ConsoleCall>(true)[0];
                if (camerafly)
                {
                    camerafly.CameraFly();
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        ConsoleMain.active = false;
        ConsoleMain.liteVersion = true;
    }
}

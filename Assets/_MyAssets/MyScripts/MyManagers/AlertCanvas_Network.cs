using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertCanvas_Network : MonoBehaviour
{
    [SerializeField]
    private UIContainer _NoConnectionPanel;
    [SerializeField]
    private UIContainer _UpdateRequiredPanel;

    public static bool isConnected = true;

    static bool isNotConnectedPanelDisplayedOnceAtLeast = false;
    static bool neverShowAgainThePanel = false;

    bool isNotConnectedPanelDisplayed = false;

    float timer = 0;
    float nextTargetTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (!isConnected && isNotConnectedPanelDisplayed && isNotConnectedPanelDisplayedOnceAtLeast && !neverShowAgainThePanel)
        {
            isNotConnectedPanelDisplayed = false;
            _NoConnectionPanel.Show();
        }

        Check_Connection();
    }

    void Check_Connection()
    {
        timer += Time.deltaTime;

        if (timer >= nextTargetTimer)
        {
            nextTargetTimer += UserSettings.PING_RATE;
            Try_To_Connect(false);
        }
    }

    public void Try_To_Connect(bool canDisplayDisconnectedPanel)
    {
        if (canDisplayDisconnectedPanel)
        {
            isNotConnectedPanelDisplayedOnceAtLeast = false;
            nextTargetTimer += UserSettings.PING_RATE;
            _NoConnectionPanel.Hide();
        }
        Ping();
    }

    void Ping()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isConnected = false;
            isNotConnectedPanelDisplayed = true;
            if (!isNotConnectedPanelDisplayedOnceAtLeast)
            {
                isNotConnectedPanelDisplayedOnceAtLeast = true;
            }
            _NoConnectionPanel.Show();
        }
        else
        {
            isNotConnectedPanelDisplayed = false;
            isNotConnectedPanelDisplayedOnceAtLeast = false;
            isConnected = true;
            neverShowAgainThePanel = false;
            _NoConnectionPanel.Hide();
        }
    }


}

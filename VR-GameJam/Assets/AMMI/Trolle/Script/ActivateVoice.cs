using Meta.WitAi;
using UnityEngine;
using UnityEngine.XR;

public class ActivateVoice : MonoBehaviour
{
    [SerializeField] private Wit wit;
    [SerializeField] private FPIShooter shooter;

    private void Start()
    {
        if (shooter != null)
        {
            shooter.OnMagazineEmpty += EnableVoiceRecognition;
        }
    }

    private void OnDestroy()
    {
        if (shooter != null)
        {
            shooter.OnMagazineEmpty -= EnableVoiceRecognition;
        }
    }

    private void EnableVoiceRecognition()
    {
        if (wit != null)
        {
            wit.Activate();
        }
    }
}

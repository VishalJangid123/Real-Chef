using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject counter;
    [SerializeField] private Image progressBar;

    private IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = counter.GetComponent<IHasProgress>();
        if(hasProgress == null)
        {
            Debug.LogError("Has progress is null");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        progressBar.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        progressBar.fillAmount = e.progressedNormalized;

        if(e.progressedNormalized == 0 || e.progressedNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

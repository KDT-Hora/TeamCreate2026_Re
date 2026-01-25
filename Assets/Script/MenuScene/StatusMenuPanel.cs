using UnityEngine;
using System.Collections.Generic;
using Data;

public class StatusMenuPanel : MonoBehaviour
{
    [SerializeField] private StatusMenuView viewPrefab;
    [SerializeField] private Transform contentRoot;

    private List<StatusMenuView> views = new();

    public void ShowParty(List<Player> members)
    {
        Clear();

        foreach (var player in members)
        {
            if (player == null) continue;

            var view = Instantiate(viewPrefab, contentRoot);
            view.Refresh(player);
            views.Add(view);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Clear();
        gameObject.SetActive(false);
    }

    private void Clear()
    {
        foreach (var v in views)
        {
            if (v != null) Destroy(v.gameObject);
        }
        views.Clear();
    }
}

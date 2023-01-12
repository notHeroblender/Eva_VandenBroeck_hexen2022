using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class PositionView : MonoBehaviour
{
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeActivate;

    private BoardView _parent;
    public Position HexPosition => PositionHelper.WorldToHexPosition(transform.position);

    private void Start()
    {
        _parent = GetComponentInParent<BoardView>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _parent.ChildClicked(this);
    }
    internal void DeActivate()
    {
        OnDeActivate?.Invoke();
    }

    internal void Activate()
    {
        OnActivate?.Invoke();
    }

}

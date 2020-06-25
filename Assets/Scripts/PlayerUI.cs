
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFeulAmount;

    private PlayerController controller;

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }
    void SetFeulAmount(float _amount)
    {
        thrusterFeulAmount.localScale = new Vector3(1f, _amount, 1f);
    }

    private void Update()
    {
        SetFeulAmount(controller.GetThrusterFeulAmount());
    }
}

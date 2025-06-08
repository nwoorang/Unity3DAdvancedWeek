using UnityEngine;

public class PlayerMediator : MonoSingleton<PlayerMediator>
{
    public PlayerController controller;
    public Camera camera;
    public PlayerStatus status;
    protected override void Awake()
    {
        base.Awake();
        camera = Camera.main;
        controller = GetComponent<PlayerController>();
                status = GetComponent<PlayerStatus>();
    }
}

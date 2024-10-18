using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{

    private IA_Controller controls;

    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }  

        controls = new IA_Controller();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveVector2 = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        bool shoot = controls.Player.Shoot.IsPressed();

        SystemAPI.SetSingleton(new InputComponent
        {
            Movement = moveVector2,
            MousePosition = mousePosition,
            Shoot = shoot

        });
    }
}

using Service.Controllers;
using UnityEngine;

namespace Gameplay.Orders
{
    [RequireComponent(typeof(OrdersManager))]
    public class OrdersManagerHandler : MonoBehaviour
    {
        private OrdersManager _ordersManager;

        private void Awake()
        {
            _ordersManager = GetComponent<OrdersManager>();
        }

        private void OnEnable()
        {
            Order.GenerateNewOrder += _ordersManager.Generate;
            GameController.SetActiveOrder += _ordersManager.SetActiveOrder;
            GameController.LevelStarted += _ordersManager.OnStartLevel;
        }

        private void OnDisable()
        {
            Order.GenerateNewOrder -= _ordersManager.Generate;
            GameController.SetActiveOrder -= _ordersManager.SetActiveOrder;
            GameController.LevelStarted -= _ordersManager.OnStartLevel;
        }
    }
}
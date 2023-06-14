using Gameplay.FuelTanks;
using Gameplay.Orders;
using UnityEngine;

namespace Gameplay.UI.FastMessages
{
    [RequireComponent(typeof(FastMessage))]
    public class FastMessageHandler : MonoBehaviour
    {
        private const string _noFreeTanks = "мер ондундъыху йюмхярп";
        private const string _tankAlreadyEmpty = "сфе носярньемю";

        private FastMessage _fastMessage;

        private void Awake()
        {
            _fastMessage = GetComponent<FastMessage>();
        }

        private void OnEnable()
        {
            FuelTanksController.NoFreeTanks += ShowNoFreeTanks;
            FuelTank.TankAlreadyEmpty += ShowTankAlreadyEmpty;
            Order.IncomeGetted += ShowIncome;
        }

        private void OnDisable()
        {
            FuelTanksController.NoFreeTanks -= ShowNoFreeTanks;
            FuelTank.TankAlreadyEmpty -= ShowTankAlreadyEmpty;
            Order.IncomeGetted -= ShowIncome;
        }

        private void ShowNoFreeTanks() => _fastMessage.Show(_noFreeTanks);

        private void ShowTankAlreadyEmpty() => _fastMessage.Show(_tankAlreadyEmpty);

        private void ShowIncome(int income) => _fastMessage.Show($"{income} $");
    }
}
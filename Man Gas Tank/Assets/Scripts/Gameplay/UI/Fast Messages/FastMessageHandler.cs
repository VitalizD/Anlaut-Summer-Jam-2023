using Gameplay.FuelTanks;
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
        }

        private void OnDisable()
        {
            FuelTanksController.NoFreeTanks -= ShowNoFreeTanks;
            FuelTank.TankAlreadyEmpty -= ShowTankAlreadyEmpty;
        }

        private void ShowNoFreeTanks() => _fastMessage.Show(_noFreeTanks);

        private void ShowTankAlreadyEmpty() => _fastMessage.Show(_tankAlreadyEmpty);
    }
}
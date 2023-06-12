using System;

namespace Service.Controllers
{
    [Serializable]
    public class MatchTransportCount
    {
        public int Level;
        public TransportType Transport;
        public int Count;
    }
}
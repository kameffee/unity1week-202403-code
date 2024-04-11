using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class PlayerStatus
    {
        public CostStatus CostStatus { get; }

        public PlayerStatus(Cost initialCost)
        {
            CostStatus = new CostStatus(initialCost);
        }
    }
}
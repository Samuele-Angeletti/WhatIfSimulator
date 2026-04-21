using WhatIfSimulator.Core.Models;

namespace WhatIfSimulator.Core.Simulation
{
    public interface ISimulationSystem
    {
        void Apply(WorldState worldState, SimulationTickContext context);
    }
}

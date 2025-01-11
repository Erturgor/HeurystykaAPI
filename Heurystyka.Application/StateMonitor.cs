using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Application
{
    public class StateMonitor
    {
        public string currentState { get; set; }

        public void UpdateState(string state)
        {
            currentState = state;
        }
    }
}

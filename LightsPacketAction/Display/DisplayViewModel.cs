using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsPacketAction
{
    class DisplayViewModel
    {
        public DisplayViewModel(List<string> buttons)
        {
            Buttons = buttons;
        }

        public List<string> Buttons { get; private set; }
    }
}

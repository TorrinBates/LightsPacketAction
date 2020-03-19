using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class NextViewModel : ViewModelBase
    {
        public NextViewModel(ICommand command)
        {
            NextCommand = command;
        }

        public ICommand NextCommand { get; private set; }
    }
}

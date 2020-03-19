using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class DualButtonViewModel : ViewModelBase
    {
        public DualButtonViewModel(ICommand prevcom, ICommand applycom)
        {
            PreviousCommand = prevcom;
            ApplyCommand = applycom;
        }

        public ICommand PreviousCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
    }
}

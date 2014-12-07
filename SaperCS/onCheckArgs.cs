using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperCS
{
	class OnCheckArgs : EventArgs
	{
		public OnCheckArgs(int state)
		{
			this.state = state;
		}

		public int state { get; private set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SaperCS
{
	class FieldClickArgs : EventArgs
	{
		public FieldClickArgs(Point position, int state)
		{
			this.position = position;
			this.state = state;
		}

		public Point position { get; private set; }
		public int state { get; private set; }
	}
}

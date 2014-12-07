using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaperCS
{
	public partial class Form1 : Form
	{
		private Game game;
		public Form1()
		{
			InitializeComponent();
			game = new Game(this, this.Controls, 20);
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			game.Ticker_Tick();
		}
	}
}

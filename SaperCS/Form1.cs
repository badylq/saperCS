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
			game = new Game(this, this.Controls);
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{

		}
	}
}

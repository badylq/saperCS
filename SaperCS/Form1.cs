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
		Field field1;
		public Form1()
		{
			InitializeComponent();
			field1 = new Field(new Point(0, 0), this.Controls);
		}
	}
}

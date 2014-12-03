using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SaperCS
{
	class Field
	{
		public Field(Point location, Control.ControlCollection controls){
			this.fieldButton = new Button();
			this.fieldButton.Location = location;
			this.fieldButton.Size = new System.Drawing.Size(25, 25);
			this.fieldButton.Visible = true;
			this.fieldButton.Text = "";
			this.controls = controls;
			this.fieldButton.Click += new EventHandler(fieldButton_Click);
			this.controls.Add(this.fieldButton);
			
		}
		public bool isMine { get; private set; }
		public bool wasClicked {get; private set;}
		private Button fieldButton;
		private Control.ControlCollection controls;

		private void fieldButton_Click(object sender, EventArgs e)
		{
			fieldButton.Enabled = false;
		}
	}
}

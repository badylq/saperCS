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
		public Field(Control.ControlCollection controls, Game game, Size areaSize, Point p, Point position){
			this.game = game;
			this.position = position;
			this.fieldButton = new Button();
			this.fieldButton.Size = new System.Drawing.Size((areaSize.Width - 16) / 10, (areaSize.Height - 39) / 10);
			this.fieldButton.Location = new Point((p.X * this.fieldButton.Size.Width), (p.Y * this.fieldButton.Size.Height));
			this.fieldButton.Visible = true;
			this.fieldButton.Text = "";
			this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", (fieldButton.Size.Height/3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.fieldButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.controls = controls;
			this.fieldButton.Click += new EventHandler(fieldButton_Click);
			this.controls.Add(this.fieldButton);
			
		}
		private void fieldButton_Click(object sender, EventArgs e)
		{
			fieldButton.Enabled = false;
			if(isMine)
			{
				fieldButton.Text = "X";
				EventArgs args = null;
				game.onMineExplode(this, args);
			}
			else
			{
				FieldClickArgs fieldClickArgs = new FieldClickArgs(this.position);
				game.onFieldClick(this, fieldClickArgs);
			}
		}


		public void MakeThisMine()
		{
			isMine = true;
		}

		public void UpdateSize(Size areaSize, Point p)
		{
			this.fieldButton.Size = new System.Drawing.Size((areaSize.Width-16) / 10, (areaSize.Height - 39) / 10);
			this.fieldButton.Location = new Point((p.X * this.fieldButton.Size.Width), (p.Y * this.fieldButton.Size.Height));
			this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", (fieldButton.Size.Height / 3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.fieldButton.Update();
		}

		public void UpdateNeighbors(int neighborsCount)
		{
			if (neighborsCount > 0)
			{
				this.fieldButton.Text = neighborsCount.ToString();
				if (neighborsCount > 1)
				{
					if (neighborsCount > 2)
					{
						if (neighborsCount > 3)
						{
							this.fieldButton.BackColor = Color.Red;
						}
						else
						{
							this.fieldButton.BackColor = Color.Orange;
						}
					}
					else
					{
						this.fieldButton.BackColor = Color.DarkGreen;
					}
				}
				else
				{
					this.fieldButton.BackColor = Color.Blue;
				}
			}
		}
		
		#region Variables
		public bool isMine { get; private set; }
		public bool wasClicked { get; private set; }
		private Button fieldButton;
		private Control.ControlCollection controls;
		private Game game;
		private Point position;

		#endregion
	}
}

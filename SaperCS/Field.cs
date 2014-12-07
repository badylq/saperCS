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
			this.fieldButton.Size = new System.Drawing.Size((areaSize.Width - 16) / 10, (areaSize.Height - 89) / 10);
			this.fieldButton.Location = new Point((p.X * this.fieldButton.Size.Width), (p.Y * this.fieldButton.Size.Height + 50));
			this.fieldButton.Visible = true;
			this.fieldButton.Text = "";
			this.fieldButton.BackColor = SystemColors.Control;
			this.fieldButton.Font = new System.Drawing.Font("Wingdings", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.fieldButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.controls = controls;
			this.fieldButton.MouseUp += new MouseEventHandler(fieldButton_MouseClick);
			this.controls.Add(this.fieldButton);
			
		}
		private void fieldButton_MouseClick(object sender, MouseEventArgs e)
		{
			MouseEventArgs mouseEventArgs = e as MouseEventArgs;
			switch (mouseEventArgs.Button)
			{
				case MouseButtons.Left:
					if (!this.wasClicked)
					{
						this.fieldButton.Enabled = false;
						this.wasClicked = true;
						if (isMine)
						{
							fieldButton.Text = "N";
							this.fieldButton.BackColor = Color.DarkRed;
							EventArgs args = null;
							game.onMineExplode(this, args);
						}
						else
						{
							FieldClickArgs fieldClickArgs = new FieldClickArgs(this.position, this.checkState);
							game.onFieldClick(this, fieldClickArgs);
						}
					}
					break;	
				case MouseButtons.Right:
					if (!this.wasClicked)
					{
						if (checkState == 0)
						{
							this.fieldButton.Font = new System.Drawing.Font("Wingdings", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
							fieldButton.Text = "O";
							this.checkState = 1;
							OnCheckArgs onCheckArgs = new OnCheckArgs(1);
							this.onCheck(this, onCheckArgs);
						}
						else if (checkState == 1)
						{
							this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
							fieldButton.Text = "?";
							this.checkState = 2;
							OnCheckArgs onCheckArgs = new OnCheckArgs(2);
							this.onCheck(this, onCheckArgs);
						}
						else
						{
							fieldButton.Text = "";
							this.checkState = 0;
							OnCheckArgs onCheckArgs = new OnCheckArgs(0);
							this.onCheck(this, onCheckArgs);
						}
					}
					break;
				case MouseButtons.Middle:
					if (!this.wasClicked)
					{
						MessageBox.Show("srodek");
					}
					break;
				default:
					break;
				}
		}

		public void Click(int neighborsCount)
		{
			this.fieldButton.Enabled = false;
			this.wasClicked = true;
			UpdateNeighbors(neighborsCount);
		}

		public void ShowMine()
		{
			fieldButton.Text = "M";
			if(checkState == 1)
				this.fieldButton.BackColor = Color.DarkGreen;
			else
			this.fieldButton.BackColor = Color.IndianRed;
			this.fieldButton.Enabled = false;
			this.wasClicked = true;
			this.fieldButton.Font = new System.Drawing.Font("Wingdings", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
		}


		public void MakeThisMine()
		{
			isMine = true;
		}

		public void Disable()
		{
			this.wasClicked = true;
		}

		public void Reset()
		{
			isMine = false;
			this.fieldButton.Enabled = true;
			this.wasClicked = false;
			this.fieldButton.Text = "";
			this.fieldButton.BackColor = SystemColors.Control;
		}

		public void UpdateSize(Size areaSize, Point p)
		{
			this.fieldButton.Size = new System.Drawing.Size((areaSize.Width-16) / 10, (areaSize.Height - 89) / 10);
			this.fieldButton.Location = new Point((p.X * this.fieldButton.Size.Width), (p.Y * this.fieldButton.Size.Height + 50));
			if(!wasClicked || isMine)
				if (checkState == 2)
					this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				else
				this.fieldButton.Font = new System.Drawing.Font("Wingdings", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			else
				this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.fieldButton.Update();
		}

		public void UpdateNeighbors(int neighborsCount)
		{
			if (neighborsCount > 0)
			{
				this.fieldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", ((int)(fieldButton.Size.Height / 2.5)), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				this.fieldButton.Text = neighborsCount.ToString();
				if (neighborsCount > 1)
				{
					if (neighborsCount > 2)
					{
						if (neighborsCount > 3)
						{
							if (neighborsCount > 4)
							{
								if (neighborsCount > 5)
								{
									if (neighborsCount > 6)
									{
										if (neighborsCount > 7)
										{
											this.fieldButton.BackColor = Color.Orange;
										}
										else
										{
											this.fieldButton.BackColor = Color.Yellow;
										}
									}
									else
									{
										this.fieldButton.BackColor = Color.LightYellow;
									}
								}
								else
								{
									this.fieldButton.BackColor = Color.LightSeaGreen;
								}
							}
							else
							{
								this.fieldButton.BackColor = Color.YellowGreen;
							}
						}
						else
						{
							this.fieldButton.BackColor = Color.LightGreen;
						}
					}
					else
					{
						this.fieldButton.BackColor = Color.LightSkyBlue;
					}
				}
				else
				{
					this.fieldButton.BackColor = Color.LightBlue;
				}
			}
		}
		
		#region Variables
		public bool isMine { get; private set; }
		public bool wasClicked { get; private set; }
		private Button fieldButton;
		private Control.ControlCollection controls;
		private Game game;
		public Point position { get; private set; }
		public int checkState { get; private set; }
		public EventHandler onCheck;

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SaperCS
{
	class Game
	{
		public Game (Form window, Control.ControlCollection controls, int mines){
			this.startedPlaying = false;
			onMineExplode += new EventHandler(Mine_Explode);
			onFieldClick += new EventHandler(Field_Click);
			this.controls = controls;
			this.window = window;
			window.SizeChanged += new System.EventHandler(this.window_SizeChanged);
			this.gameRandom = new Random();
			this.mines = mines;
			this.minesLeft = this.mines;
			this.PopulateFields(this.mines);
			this.neighborsToCheck = new List<Field> { };
			this.isPlaying = true;
			this.clock = 0;
			this.fieldsLeftUnclicked = field.Length;

			this.newGameButton = new Button();
			this.newGameButton.Location = new Point((window.Size.Width - 16)/2 - 25, 0);
			this.newGameButton.Name = "newGameButton";
			this.newGameButton.Size = new System.Drawing.Size(50, 50);
			this.newGameButton.TabIndex = 0;
			this.newGameButton.Text = "oo \\__/";
			this.newGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.newGameButton.UseVisualStyleBackColor = true;
			this.newGameButton.Click += new EventHandler(newGameButton_Click);
			this.controls.Add(this.newGameButton);

			this.timePassed = new Label();
			this.timePassed.Location = new Point(0, 0);
			this.timePassed.Name = "timePassedText";
			this.timePassed.Size = new Size(125, 50);
			this.timePassed.Text = "Time: 000";
			this.timePassed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.timePassed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.controls.Add(this.timePassed);

			this.minesLeftText = new Label();
			this.minesLeftText.Location = new Point(window.Size.Width - 16 - 125, 0);
			this.SetMinesLeftText();
			this.minesLeftText.Size = new Size(125, 50);
			this.minesLeftText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.minesLeftText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.controls.Add(this.minesLeftText);
		}

		private void PopulateFields(int numberOfMines)
		{
			Point p;
			field = new Field[10, 10];
			for (int i = 0; i < 10; i++)
			{
				for (int ii = 0; ii < 10; ii++)
				{
					p = new Point(i, ii);
					field[i, ii] = new Field(controls, this, window.Size, p, new Point(i,ii));
					field[i, ii].onCheck += new EventHandler(Field_Checked);
				}
			}
			this.CreateMines(numberOfMines);
		}

		private void CreateMines(int numberOfMines)
		{
			int createdMines = 0;
			int randomX;
			int randomY;
			while (createdMines < numberOfMines)
			{
				randomX = gameRandom.Next(10);
				randomY = gameRandom.Next(10);
				if (!field[randomX, randomY].isMine)
				{
					field[randomX, randomY].MakeThisMine();
					createdMines++;
				}
			}
		}

		private void UpdateFieldsSize()
		{
			Point p;
			for (int i = 0; i < 10; i++)
			{
				for (int ii = 0; ii < 10; ii++)
				{
					p = new Point(i, ii);
					this.field[i, ii].UpdateSize(window.Size, p);
				}
			}
		}

		private void window_SizeChanged(object sender, EventArgs e)
		{
			this.UpdateFieldsSize();
			this.newGameButton.Location = new Point((window.Size.Width - 16) / 2 - 25, 0);
			this.minesLeftText.Location = new Point(window.Size.Width - 16 - 125, 0);
			window.Update();
		}

		private void Mine_Explode(object sender, EventArgs e)
		{
			this.newGameButton.Text = "xx ___";
			this.isPlaying = false;
			if (sender is Field)
			{
				Field tempField = sender as Field;
				for (int i = 0; i < 10; i++)
				{
					for (int ii = 0; ii < 10; ii++)
					{
						this.field[i, ii].Disable();
						if (this.field[i, ii].isMine && tempField != this.field[i, ii])
						{
							this.field[i, ii].ShowMine();
						}
					}
				}
			}
			MessageBox.Show("You lost after playing for " + clock + "s.", "You lost");
		}

		private void Field_Click(object sender, EventArgs e)
		{
			Point position;
			if (this.startedPlaying == false)
				this.startedPlaying = true;
			if (e is FieldClickArgs)
			{
				FieldClickArgs fieldClickArgs = e as FieldClickArgs;
				position = fieldClickArgs.position;
				this.field[fieldClickArgs.position.X, fieldClickArgs.position.Y].UpdateNeighbors(CheckForMines(position, true));
				if (fieldClickArgs.state == 1)
				{
					this.minesLeft += 1;
					this.SetMinesLeftText();
				}
				fieldsLeftUnclicked -= 1;
				if(fieldsLeftUnclicked == this.mines)
				{
					isPlaying = false;
					this.GameWon();
				}
			}
		}

		private int CheckForMines(Point position, bool runCheck)
		{
			int neighborsCounter = 0;
			if (position.X > 0)
			{
				if(position.Y > 0)
				{
					if (field[position.X - 1, position.Y - 1].isMine)
						neighborsCounter += 1;
				}
				if (field[position.X - 1, position.Y].isMine)
					neighborsCounter += 1;
				if(position.Y < 9)
				{
					if (field[position.X - 1, position.Y + 1].isMine)
						neighborsCounter += 1;
				}
			}

			if(position.Y > 0)
			{
				if (field[position.X, position.Y - 1].isMine)
					neighborsCounter += 1;
				if (position.X < 9)
				{
					if(field[position.X + 1, position.Y - 1].isMine)
					neighborsCounter +=1;
				}
			}

			if (position.X < 9)
			{
				if (field[position.X + 1, position.Y].isMine)
					neighborsCounter += 1;
				if (position.Y < 9)
				{
					if (field[position.X + 1, position.Y + 1].isMine)
						neighborsCounter += 1;
				}
			}
			if (position.Y < 9)
			{
				if (field[position.X, position.Y + 1].isMine)
					neighborsCounter += 1;
			}

			if (neighborsCounter == 0)
			{
				this.CheckNeighbors(position, runCheck);
			}

			return neighborsCounter;
		}

		private void CheckNeighbors(Point position, bool runCheck)
		{
			if (position.X > 0)
			{
				if (position.Y > 0)
				{
					if (!neighborsToCheck.Contains(field[position.X - 1, position.Y - 1]) && !field[position.X - 1, position.Y - 1].wasClicked)
						neighborsToCheck.Add(field[position.X - 1, position.Y - 1]);
				}
				if (!neighborsToCheck.Contains(field[position.X - 1, position.Y]) && !field[position.X - 1, position.Y].wasClicked)
					neighborsToCheck.Add(field[position.X - 1, position.Y]);
				if(position.Y < 9)
				{
					if (!neighborsToCheck.Contains(field[position.X - 1, position.Y + 1]) && !field[position.X - 1, position.Y + 1].wasClicked)
						neighborsToCheck.Add(field[position.X - 1, position.Y + 1]);
				}
			}

			if (position.Y > 0)
			{
				if (!neighborsToCheck.Contains(field[position.X, position.Y - 1]) && !field[position.X, position.Y - 1].wasClicked)
					neighborsToCheck.Add(field[position.X, position.Y - 1]);
				if (position.X < 9)
				{
					if (!neighborsToCheck.Contains(field[position.X + 1, position.Y - 1]) && !field[position.X + 1, position.Y - 1].wasClicked)
						neighborsToCheck.Add(field[position.X + 1, position.Y - 1]);
				}
			}

			if (position.X < 9)
			{
				if (!neighborsToCheck.Contains(field[position.X + 1, position.Y]) && !field[position.X + 1, position.Y].wasClicked)
					neighborsToCheck.Add(field[position.X + 1, position.Y]);
				if (position.Y < 9)
				{
					if (!neighborsToCheck.Contains(field[position.X + 1, position.Y + 1]) && !field[position.X + 1, position.Y + 1].wasClicked)
						neighborsToCheck.Add(field[position.X + 1, position.Y + 1]);
				}
			}
			if (position.Y < 9)
			{
				if (!neighborsToCheck.Contains(field[position.X, position.Y + 1]) && !field[position.X, position.Y + 1].wasClicked)
					neighborsToCheck.Add(field[position.X, position.Y + 1]);
			}
			
			while (neighborsToCheck.Count() > 0 && runCheck)
			{
				neighborsToCheck[0].Click(this.CheckForMines(neighborsToCheck[0].position, false));
				fieldsLeftUnclicked -= 1;
				if (fieldsLeftUnclicked ==this.mines)
				{
					isPlaying = false;
					this.GameWon();
				}
				neighborsToCheck.RemoveAt(0);
			}
		}

		private void newGameButton_Click(object sender, EventArgs e)
		{
			this.NewGame();
			this.newGameButton.Text = "oo \\__/";
		}
		public void NewGame()
		{
			for (int i = 0; i < 10; i++)
			{
				for (int ii = 0; ii < 10; ii++)
				{
					this.field[i, ii].Reset();
				}
			}
			this.minesLeft = this.mines;
			this.SetMinesLeftText();
			this.CreateMines(this.mines);
			this.isPlaying = true;
			this.clock = 0;
			this.timePassed.Text = "Time: 000";
			this.fieldsLeftUnclicked = field.Length;
			this.startedPlaying = false;
		}

		public void Ticker_Tick()
		{
			if (isPlaying && startedPlaying)
			{
				clock += 1;
				if (clock > 9)
				{
					if (clock > 99)
					{
						this.timePassed.Text = "Time: " + clock;
					}
					else
						this.timePassed.Text = "Time: 0" + clock;
				}
				else
					this.timePassed.Text = "Time: 00" + clock;
			}
		}
 
		private void SetMinesLeftText()
		{
			if (minesLeft >= 0)
			{
				this.minesLeftText.ForeColor = Color.Black;
				if (minesLeft > 9)
				{
					if (minesLeft > 99)
					{
						this.minesLeftText.Text = "Mines:  " + minesLeft;
					}
					else
						this.minesLeftText.Text = "Mines:  0" + minesLeft;
				}
				else
					this.minesLeftText.Text = "Mines:  00" + minesLeft;
			}
			else
			{
				this.minesLeftText.ForeColor = Color.Red;
				if (minesLeft < -9)
				{
					if (minesLeft < -99)
					{
						this.minesLeftText.Text = "Mines: -" + minesLeft * -1;
					}
					else
						this.minesLeftText.Text = "Mines: -0" + minesLeft * -1;
				}
				else
					this.minesLeftText.Text = "Mines: -00" + minesLeft * -1;
			}
		}
		private void Field_Checked(object sender, EventArgs e)
		{
			OnCheckArgs onChechArgs = e as OnCheckArgs;
			switch (onChechArgs.state)
			{
				case 1:
					minesLeft -= 1;
					this.SetMinesLeftText();
					break;
				case 2:
					minesLeft += 1;
					this.SetMinesLeftText();
					break;
				default:
					break;
			}
		}

		private void GameWon()
		{
			this.minesLeft = 0;
			this.SetMinesLeftText();
			MessageBox.Show("You won in " + clock + "s.", "You won");
		}

		#region Variables
		private Control.ControlCollection controls;
		private Field[,] field;
		private Random gameRandom;
		private Form window;
		public EventHandler onMineExplode;
		public EventHandler onFieldClick;
		private List<Field> neighborsToCheck;
		private Button newGameButton;
		private bool isPlaying;
		private int clock;
		private Label timePassed;
		private Label minesLeftText;
		private int mines;
		private int minesLeft;
		private int fieldsLeftUnclicked;
		private bool startedPlaying;

		#endregion

	}
}

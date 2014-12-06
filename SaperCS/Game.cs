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
		public Game (Form window, Control.ControlCollection controls){
			onMineExplode += new EventHandler(Mine_Explode);
			onFieldClick += new EventHandler(Field_Click);
			this.controls = controls;
			this.window = window;
			window.SizeChanged += new System.EventHandler(this.window_SizeChanged);
			this.gameRandom = new Random();
			this.PopulateFields(10);
			this.neighborsToCheck = new List<Field> { };
		}
		public bool Run()
		{
			
			return true;
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
				}
			}

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
			window.Update();
		}

		private void Mine_Explode(object sender, EventArgs e)
		{
			// Game end code
		}

		private void Field_Click(object sender, EventArgs e)
		{
			Point position;
			if (e is FieldClickArgs)
			{
				FieldClickArgs fieldClickArgs = e as FieldClickArgs;
				position = fieldClickArgs.position;
				this.field[fieldClickArgs.position.X, fieldClickArgs.position.Y].UpdateNeighbors(CheckForMines(position, true));
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
				neighborsToCheck.RemoveAt(0);
			}
		}
 

		#region Variables
		private Control.ControlCollection controls;
		private Field[,] field;
		private Random gameRandom;
		private Form window;
		public EventHandler onMineExplode;
		public EventHandler onFieldClick;
		private List<Field> neighborsToCheck;

		#endregion
	}
}

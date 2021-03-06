using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
	
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			GetMode();
		}

		int x, y; //значения размерности
		bool draw = false;
        bool rightMouseButton = false;
		int[,] mapArray;

		//отрисовка элементов карты
		int sideSize = 10; //размер стороны квадрата
		static int lineWidth = 1; //ширина линии квадрата

		PictureBox globalMapPictureBox = new PictureBox();
		Bitmap globalMap;
		Graphics globalMapGraphics;

		Button saveButton = new Button ();
		Button loadButton = new Button ();
		Button createButton = new Button ();
		Label xTrackBarLabel = new Label ();
		Label yTrackBarLabel = new Label ();
		TrackBar xTrackBar = new TrackBar ();
		TrackBar yTrackBar = new TrackBar ();
		Size buttonSize = new Size (100, 25);

		Pen emptyRectPen = new Pen(Color.Gray, lineWidth); //линия пустой клетки
		SolidBrush takenRectBrush = new SolidBrush(Color.Black); //зарисовка занятой клетки
        SolidBrush emptyRectBrush = new SolidBrush(Color.LightGray);
		//


		void GetMode()
		{
			this.Size = new Size(500, 350); //окно программы

			loadButton.Location = new Point (10, 10);
			loadButton.Size = buttonSize;
			loadButton.Text = "Загрузить";
			loadButton.MouseClick += new MouseEventHandler (LoadButtonClick);
			Controls.Add (loadButton);

			saveButton.Location = new Point (125, 10);
			saveButton.Size = buttonSize;
			saveButton.Text = "Сохранить";
			saveButton.MouseClick += new MouseEventHandler (SaveButtonClick);
			Controls.Add (saveButton);

			createButton.Location = new Point (240, 10);
			createButton.Size = buttonSize;
			createButton.Text = "Создать";
			createButton.MouseClick += new MouseEventHandler (CreateButtonClick);
			Controls.Add (createButton);

			xTrackBarLabel.Location = new Point (10, 50);
			xTrackBarLabel.Size = new Size(40,20);
			Controls.Add (xTrackBarLabel);

			xTrackBar.Location = new Point (50, 50);
			xTrackBar.Minimum = 10;
			xTrackBar.Maximum = 100;
			xTrackBar.TickFrequency = 10;
			xTrackBar.Scroll += TrackBarScroll;
			Controls.Add (xTrackBar);

			yTrackBarLabel.Location = new Point (170, 50);
			yTrackBarLabel.Size = new Size(40,20);
			Controls.Add (yTrackBarLabel);

			yTrackBar.Location = new Point (210, 50);
			yTrackBar.Minimum = 10;
			yTrackBar.Maximum = 100;
			yTrackBar.TickFrequency = 10;
			yTrackBar.Scroll += TrackBarScroll;
			Controls.Add (yTrackBar);
		}

		void TrackBarScroll (object sender, EventArgs e)
		{
			x = xTrackBar.Value;
			y = yTrackBar.Value;
			xTrackBarLabel.Text = String.Format ("X: {0}", xTrackBar.Value);
			yTrackBarLabel.Text = String.Format ("Y: {0}", yTrackBar.Value);
		}

		void LoadButtonClick(object sender, EventArgs e)
		{

		} //нажание на кнопку загрузки

		void SaveButtonClick(object sender, EventArgs e)
		{
            string fileName = System.IO.Path.Combine(@"c:\MARS maps", System.IO.Path.GetRandomFileName());
		} //нажание на кнопку сохранения

		void CreateButtonClick(object sender, EventArgs e)
		{
			mapArray = new int[y, x];
			DrawMap ();
		} //нажатие на кнопку создания карты

		// начало отрисовки интерфейса и карты
		void DrawMap()
		{
			globalMapPictureBox.MouseMove += new MouseEventHandler(GlobalMapMouseMove);
			globalMapPictureBox.MouseUp += new MouseEventHandler(GlobalMapMouseUp);
			globalMapPictureBox.MouseDown += new MouseEventHandler(GlobalMapMouseDown);


			emptyRectPen.Alignment = PenAlignment.Inset; //закрашивание внутри контура

			this.Size = new Size(x * sideSize + 300, y * sideSize + 150); //окно программы

			globalMapPictureBox.Location = new Point (10, 100); //размещение карты в окне
			globalMapPictureBox.Size = new Size(x * sideSize+1, y * sideSize+1); //окно карты
			Controls.Add(globalMapPictureBox);
			globalMap = new Bitmap(x * sideSize+1, y * sideSize+1);
			globalMapGraphics = Graphics.FromImage(globalMap);

			for (int i = 0; i < y; i++) //отрисовка пустой карты
			{
				for (int j = 0; j < x; j++)
				{
                    globalMapGraphics.FillRectangle(takenRectBrush, j * (sideSize), i * (sideSize), sideSize + 1, sideSize + 1);
                    globalMapGraphics.DrawRectangle(emptyRectPen, j * (sideSize), i * (sideSize), sideSize, sideSize);
                    mapArray[i, j] = 1;
				}
			}

			globalMapPictureBox.Image = globalMap;

		} //отрисовка карты
		//конец отрисовки интерфейса и карты

		private void GlobalMapMouseMove(object sender, MouseEventArgs e)
		{
			if (draw)
			{
				int squareX = e.X / sideSize;
				int squareY = e.Y / sideSize;
                if (squareX < x && squareY < y && squareX >= 0 && squareY >= 0)
                {
                    if (!rightMouseButton)
                    {
                        mapArray[squareY, squareX] = 0;
                        globalMapGraphics.FillRectangle(emptyRectBrush, squareX * sideSize, squareY * sideSize, sideSize + 1, sideSize + 1);
                        globalMapGraphics.DrawRectangle(emptyRectPen, squareX * (sideSize), squareY * (sideSize), sideSize, sideSize);
                    }
                    else
                    {
                        mapArray[squareY, squareX] = 1;
                        globalMapGraphics.FillRectangle(takenRectBrush, squareX * sideSize, squareY * sideSize, sideSize + 1, sideSize + 1);
                        globalMapGraphics.DrawRectangle(emptyRectPen, squareX * (sideSize), squareY * (sideSize), sideSize, sideSize);
                    }
                    globalMapPictureBox.Image = globalMap;
                }
			}
		}

		private void GlobalMapMouseUp(object sender, MouseEventArgs e)
		{
			draw = false;
		}

		private void GlobalMapMouseDown(object sender, MouseEventArgs e)
		{
			draw = true;
            if (e.Button == MouseButtons.Right)
            {
                rightMouseButton = true;
            }
            else
            {
                rightMouseButton = false;
            }
		}
	}
}

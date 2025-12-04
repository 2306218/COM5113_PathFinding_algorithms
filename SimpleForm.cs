using System;
using System.Drawing;
using System.Windows.Forms;

namespace PathFindingAssignment
{
    // Simple Windows Forms viewer to display the map and the path graphically.
    public class SimpleForm : Form
    {
        private readonly Map map;           // The map to display
        private PathResult lastResult;      // The last path found (for drawing)

        // UI controls
        private ComboBox algorithmCombo;
        private Button runButton;
        private Panel drawPanel;

        public SimpleForm(Map map)
        {
            this.map = map;

            Text = "Path Finding Viewer";
            Width = 800;
            Height = 800;

            // Drop-down list for selecting an algorithm
            algorithmCombo = new ComboBox();
            algorithmCombo.Items.Add("1 - BFS");
            algorithmCombo.Items.Add("2 - DFS");
            algorithmCombo.Items.Add("3 - Hill Climbing");
            algorithmCombo.Items.Add("4 - Best First");
            algorithmCombo.Items.Add("5 - Dijkstra");
            algorithmCombo.SelectedIndex = 0;
            algorithmCombo.Dock = DockStyle.Top;

            // Button to run the chosen algorithm
            runButton = new Button();
            runButton.Text = "Run Algorithm";
            runButton.Dock = DockStyle.Top;
            runButton.Click += RunButton_Click;

            // Panel where the map and path will be drawn
            drawPanel = new Panel();
            drawPanel.Dock = DockStyle.Fill;
            drawPanel.BackColor = Color.White;
            drawPanel.Paint += DrawPanel_Paint;

            // Add controls to the form
            Controls.Add(drawPanel);
            Controls.Add(runButton);
            Controls.Add(algorithmCombo);
        }

        // Called when the user clicks "Run Algorithm"
        private void RunButton_Click(object sender, EventArgs e)
        {
            if (algorithmCombo.SelectedItem == null) return;

            string selected = (string)algorithmCombo.SelectedItem;

            // The option is the first character (e.g. "1 - BFS" → "1")
            string option = selected.Substring(0, 1);

            IPathFinder finder = PathFinderFactory.Create(option);

            // Run the algorithm and redraw the panel
            lastResult = finder.FindPath(map);
            drawPanel.Invalidate();

            if (!lastResult.Success)
            {
                MessageBox.Show("No path found.");
            }
        }

        // Paints the map and path on the panel
        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int rows = map.Rows;
            int cols = map.Cols;

            // Decide the size of each cell so the map fits the panel
            int cellSize = Math.Min(drawPanel.Width / cols, drawPanel.Height / rows);
            if (cellSize < 5) cellSize = 5;

            // Draw base terrain
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Rectangle rect = new Rectangle(
                        c * cellSize,
                        r * cellSize,
                        cellSize - 1,
                        cellSize - 1);

                    TerrainType t = map.Grid[r, c];
                    Color colour;

                    // Choose a colour based on terrain type
                    switch (t)
                    {
                        case TerrainType.Open:
                            colour = Color.White;
                            break;
                        case TerrainType.Wood:
                            colour = Color.Green;
                            break;
                        case TerrainType.Water:
                            colour = Color.LightBlue;
                            break;
                        case TerrainType.Wall:
                            colour = Color.Black;
                            break;
                        default:
                            colour = Color.Gray;
                            break;
                    }

                    using (Brush b = new SolidBrush(colour))
                    {
                        g.FillRectangle(b, rect);
                    }

                    // Draw cell border
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }

            // If we have a successful result, draw the path in yellow
            if (lastResult != null && lastResult.Success && lastResult.Path != null)
            {
                foreach (GridPosition pos in lastResult.Path)
                {
                    Rectangle rect = new Rectangle(
                        pos.Col * cellSize,
                        pos.Row * cellSize,
                        cellSize - 1,
                        cellSize - 1);

                    using (Brush b = new SolidBrush(Color.Yellow))
                    {
                        g.FillRectangle(b, rect);
                    }

                    g.DrawRectangle(Pens.OrangeRed, rect);
                }
            }

            // Draw start and goal markers on top
            DrawMarker(g, map.Start, cellSize, Color.Blue); // start = blue
            DrawMarker(g, map.Goal, cellSize, Color.Red);   // goal = red
        }

        // Draws a small coloured circle in the centre of a cell (for start/goal)
        private void DrawMarker(Graphics g, GridPosition pos, int cellSize, Color colour)
        {
            Rectangle rect = new Rectangle(
                pos.Col * cellSize + cellSize / 4,
                pos.Row * cellSize + cellSize / 4,
                cellSize / 2,
                cellSize / 2);

            using (Brush b = new SolidBrush(colour))
            {
                g.FillEllipse(b, rect);
            }
        }
    }
}

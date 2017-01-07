using System;

namespace Project1
{
    public struct ViewTable
    {
        public int size;      // Size of the table   
        public int startRow;  // Row from which the table is drawn

        public ViewTable(int Size)
        {
            startRow = 3;
            this.size = Size;
        }

        /// <summary>
        /// Formula сonverts ordinary coordinates in view coordinates
        /// </summary>
        /// <param name="row">target cell coordinate by horizontals</param>
        /// <param name="column">target cell coordinate in the vertical</param>
        private void SetTableCursorPosition(int row, int column)
        {
            Console.SetCursorPosition((Console.WindowWidth / 2 - size * 2) + 1 + 4 * column, startRow + row * 2);
        }

        /// <summary>
        /// Draw a table 
        /// </summary>
        /// <param name="tb">Program table</param>
        public void DrawField(Table tb)
        {
            //Console.ResetColor();
            int currentRow = startRow; // line which is drawn now

            //С помощью метода SetCursorPosition указывается, где должна начаться следующая операция записи в окно консоли.
            //Если заданная позиция курсора находится вне области, которая в данный момент видима в окне консоли, начало координат этого окна автоматически изменяется, чтобы курсор стал видимым.
            Console.SetCursorPosition(Console.WindowWidth / 2 - size * 2, currentRow - 1);

            //Draw the "zero" line by horizontals
            for (int k = 0; k < size; k++)
            {
                Console.Write("+---");
            }
            Console.WriteLine("+");

            //Draw table
            for (int i = 0; i < size; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - size * 2, currentRow);
                currentRow++;

                // Draw the edge of the table in the vertical
                Console.Write("|");
                for (int j = 0; j < size; j++)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (tb.findFoxes[j, i] == 1)
                    {
                        Console.Write(" V ");
                    }
                    if (tb.findFoxes[j, i] == -1)
                    {
                        Console.Write(" X ");
                    }
                    if (tb.findFoxes[j, i] == 0)
                    {
                        Console.Write("   ");
                    }
                    Console.ResetColor();
                    Console.Write("|");

                }

                Console.SetCursorPosition(Console.WindowWidth / 2 - size * 2, currentRow);
                currentRow++;

                //Draw the edge of the table by horizontals
                for (int k = 0; k < size; k++)
                {
                    Console.Write("+---");
                }
                Console.WriteLine("+");

            }
            //Console.BackgroundColor = ConsoleColor.Green;
            //Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// View the peeling of the point
        /// </summary>
        /// <param name="targetRow">target cell coordinate by horizontals</param>
        /// <param name="targetColumn">target cell coordinate in the vertical</param>
        public void Peleng(int targetRow, int targetColumn, Table tb)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < size; i++)
            {
                SetTableCursorPosition(targetRow, i);
                if (tb.findFoxes[i, tb.cursorY] == 1)
                {
                    Console.Write(" V ");
                }
                if (tb.findFoxes[i, tb.cursorY] == -1)
                {
                    Console.Write(" X ");
                }
                if (tb.findFoxes[i, tb.cursorY] == 0)
                {
                    Console.Write("   ");
                }

            }

            for (int i = 0; i < size; i++)
            {
                SetTableCursorPosition(i, targetColumn);
                if (tb.findFoxes[tb.cursorX, i] == 1)
                {
                    Console.Write(" V ");
                }
                if (tb.findFoxes[tb.cursorX, i] == -1)
                {
                    Console.Write(" X ");
                }
                if (tb.findFoxes[tb.cursorX, i] == 0)
                {
                    Console.Write("   ");
                }

            }

            Console.BackgroundColor = ConsoleColor.Red;
            SetTableCursorPosition(targetRow, targetColumn);
            if (tb.findFoxes[tb.cursorX, tb.cursorY] == 1)
            {
                Console.Write(" V ");
            }
            if (tb.findFoxes[tb.cursorX, tb.cursorY] == -1)
            {
                Console.Write(" X ");
            }
            if (tb.findFoxes[tb.cursorX, tb.cursorY] == 0)
            {
                Console.Write("   ");
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Menu items
        /// </summary>
        public void ConsoleMenu()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(5, 8);
            Console.WriteLine(" Начать Игру ");

            Console.SetCursorPosition(5, 10);
            Console.WriteLine("   Справка   ");

            Console.SetCursorPosition(5, 12);
            Console.WriteLine("    Выход    ");
            Console.ResetColor();
        }

        /// <summary>
        /// Select menu items 
        /// </summary>
        /// <param name="mn">a number of menu item that was select</param>
        public void SelectItemMenu(Menu mn)
        {
            ConsoleMenu();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            switch (mn)
            {
                case Menu.Game:
                    Console.SetCursorPosition(5, 8);
                    Console.WriteLine(" Начать Игру ");

                    break;
                case Menu.Reference:
                    Console.SetCursorPosition(5, 10);
                    Console.WriteLine("   Справка   ");
                    break;
                case Menu.Exit:
                    Console.SetCursorPosition(5, 12);
                    Console.WriteLine("    Выход    ");
                    break;
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Console Yes/No
        /// </summary>
        /// <param name="x">Cursor Position by horizontals</param>
        /// <param name="y">Cursor Position in the vertical</param>
        public void ConsoleYesNo(int x, int y)
        {

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(x, y);
            Console.WriteLine(" Да ");  

            Console.SetCursorPosition(x + 16, y);
            Console.WriteLine(" Нет ");

            Console.ResetColor();
        }


        /// <summary>
        /// Select Yes or No
        /// </summary>
        /// <param name="ch">What user choose</param>
        /// <param name="x">Cursor Position by horizontals</param>
        /// <param name="y">Cursor Position in the vertical</param>
        public void SelectYesNo(YesNo ch, int x, int y)
        {
            ConsoleYesNo(x, y);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;

            switch (ch)
            {
                case YesNo.Yes:
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(" Да ");
                    break;
                case YesNo.No:
                    Console.SetCursorPosition(x + 16, y);
                    Console.WriteLine(" Нет ");
                    break;
            }
            Console.ResetColor();
        }



    }
}
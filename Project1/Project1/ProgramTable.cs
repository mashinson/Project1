using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    /// <summary>
    /// Menu Items
    /// </summary>
    public enum Menu
    {
        NoAction = -1,
        Game = 0,
        Reference = 1,
        Exit = 2

    }

    public enum YesNo
    {
        NoAction = -1,
        Yes = 0,
        No = 1
    }



    public struct Table
    {
        public int cursorX;
        public int cursorY;
        public int[,] arrayFoxes;        // array of Foxes
        public int[,] findFoxes;         // Foxes which found \ not found
        public int countFindFoxes;       // sum of caught foxes 
        public int shots;               // sum of shots
        public int countPellingFoxes;    // sum of caught foxes by pelling
        public int leftFoxes;            // sum of the remaining foxes
        public int findNowFoxes;         // sum of caugnt foxes in the cell
        public int cursorMenu;           
        public int cursorYesNo;
        public int cursorYesNoExit;
        public int start;
        public Table(int size, int countOfFoxes)
        {
            start = Environment.TickCount;
            cursorYesNo = 0;
            cursorYesNoExit = 0;
            cursorMenu = 0;
            cursorX = 0;
            cursorY = 0;
            countFindFoxes = 0;
            shots = 0;
            countPellingFoxes = 0;
            leftFoxes = countOfFoxes;
            findNowFoxes = 0;
            arrayFoxes = new int[size, size];
            findFoxes = new int[size, size];

            for (int i = 0; i < arrayFoxes.GetLength(0); i++)
            {
                for (int j = 0; j < arrayFoxes.GetLength(1); j++)
                {
                    arrayFoxes[i, j] = 0;
                    findFoxes[i, i] = 0;
                }
            }
        }


        /// <summary>
        /// Move the cursor on the playing field
        /// </summary>
        /// <param name="dx">shift by horizontals</param>
        /// <param name="dy">shift in the vertical</param>
        /// <param name="tb">User table</param>
        public void MoveGameCursore(int dx, int dy, ViewTable tb)
        {
            if (cursorX + dx >= 0 && cursorX + dx < tb.size && cursorY + dy >= 0 && cursorY + dy < tb.size)
            {
                cursorX = cursorX + dx;
                cursorY = cursorY + dy;
                Console.SetCursorPosition((Console.WindowWidth / 2 - tb.size * 2) + 2 + 4 * cursorX, tb.startRow + cursorY * 2);
            }
            else
            {
                Console.SetCursorPosition((Console.WindowWidth / 2 - tb.size * 2) + 2 + 4 * cursorX, tb.startRow + cursorY * 2);
            }
        }

        /// <summary>
        /// find countFindFoxes, count of shots, pelling
        /// </summary>
        /// <param name="tb">Concole table</param>
        /// <returns>if ch = 0 - don`t find Foxes, ch = 1 - find Foxes, ch = -1 - shot in first cell, ch = 2 - user win </returns>
        public int EnterFoxes(ViewTable tb)
        {
            int result = 0;
            findNowFoxes = 0;
            shots += 1;
            if (leftFoxes > 0)
            {
                if (cursorX == 0 && cursorY == 0)
                {
                    result = -1;
                }
                else
                {
                    if (arrayFoxes[cursorX, cursorY] == 0)
                    {
                        findFoxes[cursorX, cursorY] = -1;
                        result = 0;
                    }

                    else
                    {
                        countFindFoxes += arrayFoxes[cursorX, cursorY];
                        findNowFoxes = arrayFoxes[cursorX, cursorY];
                        leftFoxes -= arrayFoxes[cursorX, cursorY];
                        arrayFoxes[cursorX, cursorY] = 0;

                        findFoxes[cursorX, cursorY] = 1;
                        result = 1;
                    }


                }

                // Peleng
                tb.Peleng(cursorY, cursorX, this);
                countPellingFoxes = 0;
                for (int j = 0; j < arrayFoxes.GetLength(1); j++)
                {
                    countPellingFoxes += arrayFoxes[cursorX, j];
                }
                for (int i = 0; i < arrayFoxes.GetLength(0); i++)
                {
                    countPellingFoxes += arrayFoxes[i, cursorY];
                }


            }

            if (leftFoxes == 0)
            {
                result = 2;
            }
            //cursorX = 0;
            //cursorY = 0;
            return result;
        }

        /// <summary>
        /// choose menu items
        /// </summary>
        /// <param name="tb">User table</param>
        /// <param name="dy">shift in the vertical</param>
        public void MoveMenu(ViewTable tb, int dy)
        {

            cursorMenu += dy;
            if (cursorMenu < 0)
            {
                cursorMenu = 2;
            }
            if (cursorMenu > 2)
            {
                cursorMenu = 0;
            }
            tb.SelectItemMenu((Menu)cursorMenu);
        }

        /// <summary>
        /// choose through Yes or No (If user win)
        /// </summary>
        /// <param name="tb">User table</param>
        /// <param name="dx">shift by horizontals</param>
        public void MoveYesNo(ViewTable tb, int dx)
        {
            cursorYesNo += dx;
            if (cursorYesNo <= 0)
            {
                cursorYesNo = 0;
            }
            if (cursorYesNo >= 1)
            {
                cursorYesNo = 1;
            }
            int x = 20;
            int y = 20;
            tb.SelectYesNo((YesNo)cursorYesNo, x, y);
        }

        /// <summary>
        /// choose through Yes or No (Exit game) 
        /// </summary>
        /// <param name="tb">User table</param>
        /// <param name="dx">shift by horizontals</param>
        public void MoveYesNoExit(ViewTable tb, int dx)
        {
            cursorYesNoExit += dx;
            if (cursorYesNoExit <= 0)
            {
                cursorYesNoExit = 0;
            }
            if (cursorYesNoExit >= 1)
            {
                cursorYesNoExit = 1;
            }
            int x = 20;
            int y = 10;
            tb.SelectYesNo((YesNo)cursorYesNoExit, x, y);
        }



    }
}

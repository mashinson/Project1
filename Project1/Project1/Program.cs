using System;
using System.IO;

namespace Project1
{
    enum ProgramSate
    {
        Menu,
        Game,
        Reference,
        GameSetUp,
        Exit
    }

    class Program
    {
        public static string strMenu = "";
        public static string strWin = "";
        public static string strReference = "";
        public static string strExit = "";
        public static int countOfFoxes; // Count of all Foxes
        public static int sizeOfTable; // Size of table
        public static int go = 0;
        public static bool showPeleng = false;

        static void Main(string[] args)
        {
            ViewTable tb = new ViewTable();
            Table playingField = new Table();

            strMenu = loadResources("MenuFox.txt");
            strWin = loadResources("Win.txt");
            strReference = loadResources("Reference.txt");
            strExit = loadResources("ExitGame.txt");
            ProgramSate ps = ProgramSate.Menu;

            while (ps != ProgramSate.Exit)
            {
                switch (ps)
                {
                    case ProgramSate.Menu:
                        ps = Menu(ref tb, ref playingField);
                        break;
                    case ProgramSate.Game:
                        ps = Game(ref tb, ref playingField);
                        break;
                    case ProgramSate.Reference:
                        ps = Reference(ref tb, ref playingField);
                        break;
                    case ProgramSate.GameSetUp:
                        ps = GameSetUp(ref tb, ref playingField);
                        break;
                    case ProgramSate.Exit:
                        return;
                }
            }

        }

        /// <summary>
        /// User determines the size of the field and the number of foxes on it
        /// </summary>
        /// <returns>Program state</returns>
        public static ProgramSate GameSetUp(ref ViewTable tb, ref Table playingField)
        {
            showPeleng = false;
            go = 0;
            bool ch = true;
            string s = "";
            bool bl;

            while (ch)
            {
                s = "";
                Console.Clear();
                Console.Write("Введите размер таблички (не меньше 2 и не больше 10): ");
                s = Console.ReadLine();
                bl = Int32.TryParse(s, out sizeOfTable);
                if (bl == false || sizeOfTable < 2 || sizeOfTable > 10)
                {
                    Console.Clear();
                    Console.WriteLine("Введите правильный размер табличики!!!");
                    Console.ReadLine();
                }
                else
                {
                    ch = false;
                }
            }

            ch = true;

            while (ch)
            {
                Console.Clear();
                Console.Write("Введите количество лис (не меньше 2): ");
                s = Console.ReadLine();
                bl = Int32.TryParse(s, out countOfFoxes);
                if (bl == false || countOfFoxes < 2)
                {
                    Console.Clear();
                    Console.WriteLine("Введите правильное колиество лис!!!");
                    Console.ReadLine();
                }
                else
                {
                    ch = false;
                }

            }
            Console.Clear();
            tb = new ViewTable(sizeOfTable);
            playingField = new Table(sizeOfTable, countOfFoxes);

            RandomArray(ref playingField);
            tb.DrawField(playingField);


            playingField.MoveGameCursore(0, 0, tb);
            return ProgramSate.Game;
        }

        /// <summary>
        /// Fills an array randomly by numbers 
        /// </summary>
        /// <param name="playingField">ProgramTable</param>
        public static void RandomArray(ref Table playingField)
        {
            int i, j;
            Random x = new Random();

            for (int w = 0; w < countOfFoxes; w++)
            {
                i = x.Next(0, sizeOfTable);
                j = x.Next(0, sizeOfTable);
                if ((i == 0) & (j == 0))
                {
                    j = +1;
                }
                playingField.arrayFoxes[i, j] = playingField.arrayFoxes[i, j] + 1;
            }

        }

        /// <summary>
        /// Load different files
        /// </summary>
        /// <param name="fileName">name of file and it's path</param>
        /// <returns>a string s that contains information from a file</returns>
        public static string loadResources(string fileName)
        {
            string s = "";
            StreamReader reader = new StreamReader(fileName);
            try
            {
                do
                {
                    s += reader.ReadLine() + "\n";
                }
                while (reader.Peek() != -1);
            }

            catch
            {
                s = "Error!";
            }

            finally
            {
                reader.Close();
            }
            return s;
        }



        /// <summary>
        /// Move by Menu
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <returns>Program state</returns>
        public static ProgramSate Menu(ref ViewTable tb, ref Table playingField)
        {
            Console.Clear();
            Console.Write(strMenu); // draw menu
            ProgramSate ps = ProgramSate.Menu;
            Console.CursorVisible = false;

            Action action;
            playingField.MoveMenu(tb, 0);

            action = UserActions.GetUserAction();
            switch (action)
            {
                case Action.Bottom:
                    playingField.MoveMenu(tb, 1);
                    break;
                case Action.Top:
                    playingField.MoveMenu(tb, -1);
                    break;
                case Action.Enter:
                    switch (playingField.cursorMenu)
                    {
                        case 0:
                            ps = ProgramSate.GameSetUp;
                            break;
                        case 1:
                            ps = ProgramSate.Reference;
                            break;
                        case 2:
                            ps = ProgramSate.Exit;
                            break;

                    }
                    break;
            }
            Console.CursorVisible = true;
            return ps;
        }

        /// <summary>
        /// Show Reference
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <returns>Program state</returns>
        private static ProgramSate Reference(ref ViewTable tb, ref Table playingField)
        {
            Action action;
            Console.Clear();
            Console.WriteLine(strReference);
            Console.SetCursorPosition(0, 0);

            action = UserActions.GetUserAction();
            while (action != Action.Exit)
            {
                action = UserActions.GetUserAction();
            }
            return ProgramSate.Menu;

        }


        /// <summary>
        /// Game, move by table, shot ...
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <returns>Program state</returns>
        public static ProgramSate Game(ref ViewTable tb, ref Table playingField)
        {
            ProgramSate ps = ProgramSate.Game;
            Action action = Action.NoAction;

            Console.Clear();
            tb.DrawField(playingField);
            playingField.MoveGameCursore(0, 0, tb);

            Console.CursorVisible = true;

            if (showPeleng)
            {
                Console.Clear();
                tb.DrawField(playingField);
                go = playingField.EnterFoxes(tb);
                if (go == 2)
                {
                    ps = WinGame(ref tb, ref playingField);
                }
                else
                {
                    GameInfo(tb, playingField, go);
                    Console.CursorVisible = false;

                    action = UserActions.GetUserAction();
                }
            }
            else
            {
                action = UserActions.GetUserAction();
            }

            switch (action)
            {
                case Action.Left:
                    showPeleng = false;
                    playingField.MoveGameCursore(-1, 0, tb);
                    break;
                case Action.Right:
                    showPeleng = false;
                    playingField.MoveGameCursore(1, 0, tb);
                    break;
                case Action.Top:
                    showPeleng = false;
                    playingField.MoveGameCursore(0, -1, tb);
                    break;
                case Action.Bottom:
                    showPeleng = false;
                    playingField.MoveGameCursore(0, 1, tb);
                    break;
                case Action.Enter:
                    showPeleng = !showPeleng;
                    break;
                case Action.Exit:
                    ps = GameExit(ref tb, ref playingField);
                    break;
            }
            return ps;
        }

        /// <summary>
        /// Exit from game
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <returns>Program state</returns>
        private static ProgramSate GameExit(ref ViewTable tb, ref Table playingField)
        {
            ProgramSate ps = ProgramSate.Game;
            Action action;
            Console.Clear();
            Console.WriteLine(strExit);
            playingField.MoveYesNoExit(tb, 0);
            bool bl = true;
            while (bl)
            {
                action = UserActions.GetUserAction();
                switch (action)
                {
                    case Action.Left:
                        playingField.MoveYesNoExit(tb, -1);
                        break;
                    case Action.Right:
                        playingField.MoveYesNoExit(tb, 1);
                        break;
                    case Action.Enter:

                        if (playingField.cursorYesNoExit == 0)
                        {
                            bl = false;
                            ps = ProgramSate.Menu;

                        }
                        else
                        {
                            bl = false;
                            ps = ProgramSate.Game;
                        }
                        break;
                }
            }
            return ps;
        }

        /// <summary>
        ///  Show if user find/don't find foxes, countFindFoxes, count of shots, pelling ...
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <param name="go">go = 0 - don`t find Foxes, go = 1 - find Foxes, go = -1 - shot in first cell, go = 2 - user win </param>
        /// <param name="start">start = Environment.TickCount;</param>
        public static void GameInfo(ViewTable tb, Table playingField, int go)
        {
            switch (go)
            {
                case -1:
                    Console.SetCursorPosition(0, 7 + 2 * tb.size);
                    Console.WriteLine("Здесь находитесь ВЫ. Введите другую клеточку.");
                    break;
                case 0:
                    Console.SetCursorPosition(0, 7 + 2 * tb.size);
                    Console.Write("Вы не попали:((((");

                    Console.SetCursorPosition(0, 8 + 2 * tb.size);
                    Console.Write("Количество лис по пеллингу: {0}", playingField.countPellingFoxes);

                    Console.SetCursorPosition(0, 9 + 2 * tb.size);
                    Console.Write("Количество всего найденных лис: {0}", playingField.countFindFoxes);

                    Console.SetCursorPosition(0, 10 + 2 * tb.size);
                    Console.Write("Количество лис, которых нужно найти: {0}", playingField.leftFoxes);
                    break;
                case 1:
                    Console.SetCursorPosition(0, 7 + 2 * tb.size);
                    Console.Write("Вы попали!!!");

                    Console.SetCursorPosition(0, 8 + 2 * tb.size);
                    Console.WriteLine("Количество найденных лис: {0}", playingField.findNowFoxes);

                    Console.SetCursorPosition(0, 9 + 2 * tb.size);
                    Console.Write("Количество лис по пеллингу: {0}", playingField.countPellingFoxes);

                    Console.SetCursorPosition(0, 10 + 2 * tb.size);
                    Console.Write("Количество всего найденных лис: {0}", playingField.countFindFoxes);

                    Console.SetCursorPosition(0, 11 + 2 * tb.size);
                    Console.Write("Количество лис, которых нужно найти: {0}", playingField.leftFoxes);
                    break;

            }
        }

        /// <summary>
        /// If user wins game
        /// </summary>
        /// <param name="tb">User Table</param>
        /// <param name="playingField">Program Table</param>
        /// <returns>Program state</returns>
        private static ProgramSate WinGame(ref ViewTable tb, ref Table playingField)
        {
            ProgramSate ps = ProgramSate.Game;
            Action action;
            Console.Clear();
            Console.Write(strWin);
            playingField.MoveYesNo(tb, 0);
            Console.SetCursorPosition(38, 12);
            Console.WriteLine(playingField.shots);
            Console.SetCursorPosition(35, 15);
            Console.WriteLine((Environment.TickCount - playingField.start) / 1000);

            bool ch = true;
            while (ch)
            {
                action = UserActions.GetUserAction();
                switch (action)
                {
                    case Action.Left:
                        playingField.MoveYesNo(tb, -1);
                        break;
                    case Action.Right:
                        playingField.MoveYesNo(tb, 1);
                        break;
                    case Action.Enter:
                        ch = false;
                        if (playingField.cursorYesNo == 0)
                        {
                            ps = ProgramSate.GameSetUp;
                        }
                        else
                        {
                            ps = ProgramSate.Menu;
                        }
                        break;
                }
            }
            return ps;
        }
    }
}


using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace LESSON_SEVEN
{
    class Program
    {
        public static void Main(string[] args)
        {
            Hangman();
        }

        static int ChooseCategory()
        {
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("         ██╗░░██╗░█████╗░███╗░░██╗░██████╗░███╗░░░███╗░█████╗░███╗░░██╗      ");
            Console.WriteLine("         ██║░░██║██╔══██╗████╗░██║██╔════╝░████╗░████║██╔══██╗████╗░██║      ");
            Console.WriteLine("         ███████║███████║██╔██╗██║██║░░██╗░██╔████╔██║███████║██╔██╗██║      ");
            Console.WriteLine("         ██╔══██║██╔══██║██║╚████║██║░░╚██╗██║╚██╔╝██║██╔══██║██║╚████║      ");
            Console.WriteLine("         ██║░░██║██║░░██║██║░╚███║╚██████╔╝██║░╚═╝░██║██║░░██║██║░╚███║      ");
            Console.WriteLine("         ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝░╚═════╝░╚═╝░░░░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝      ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                             CHOOSE A CATEGORY:                              ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("                                                                             ");
            Console.WriteLine("    1 - MOVIES                 2 - HARD WORDS            3 - COUNTRIES       ");

            Console.WriteLine();
            Start:
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice != 1 && choice != 2 && choice != 3)
            {
                Console.WriteLine("Wrong choice");
                goto Start;
            }
            return choice;
        }

        static void Hangman()
        {
            string[] movies = { "INCEPTION", "GOODFELLAS", "PARASITE", "INTERSTELLAR", "HARAKIRI", "PSYCHO", "GLADIATOR", "WHIPLASH", 
            "CASABLANCA", "ALIEN", "MEMENTO", "JOKER", "OLDBOY", "COCO", "BRAVEHEART", "AMADEUS", "VERTIGO", "M", "SNATCH", "SCARFACE",
            "IKIRU", "METROPOLIS", "UP", "HEAT", "YOJIMBO", "UNFORGIVEN", "CASINO", "CHINATOWN", "STALKER", "FARGO", "PLATOON", "ROCKY"};
            
            string[] hardWords = { "ABRUPTLY", "ABSURD", "ABYSS", "AWKWARD", "BEEKEEPER", "BIKINI", "COBWEB", "GOSSIP", "HAIKU", "JAZZ", "JIGSAW", "KIWIFRUIT", 
            "JUKEBOX", "KEYHOLE", "MATRIX", "OXYGEN", "PAJAMAS", "VAPORWARE", "VODKA", "VOODOO", "WITCHCRAFT", "ZODIAC", "WIZARD", "ZOMBIE"};
            
            string[] countries = { "LITHUANIA", "CYPRUS", "JAPAN", "GEORGIA", "BRAZIL", "TUNISIA", "FIJI", "CHINA", "ROMANIA", "PORTUGAL", "SINGAPORE", 
            "SWEDEN","FINLAND", "SPAIN", "GREENLAND", "ICELAND", "NETHERLANDS", "GERMANY", "BULGARIA", "CANADA", "FRANCE", "LUXEMBOURG", "ESTONIA",
            "AUSTRALIA", "RUSSIA", "GREECE", "CHILE", "URUGUAY", "INDIA", "MALAYSIA", "THAILAND", "VIETNAM", "BHUTAN"};

            bool playGame = true;

            while (playGame)
            {
                Soundtrack player = new Soundtrack();
                player.MainGame();

                List<char> lettersChosen = new List<char>();
                List<string> wordsChosen = new List<string>();

                var random = new Random();
                var selection = ChooseCategory();
                var word = WordSelection(selection, movies, hardWords, countries, random);

                char[] wordCharArray = GetEmptyWordArray(word.Length);

                bool guessesEnded = false;
                int mistakes = 0;

                while (!guessesEnded)
                {
                    Console.Clear();
                    HangmanPictures(mistakes);
                    Console.WriteLine();
                    PrintCharArray(wordCharArray);

                    PrintLettersAndWordsGuessed(lettersChosen, wordsChosen);

                    Console.Write("Guess letter or a whole word: ");
                    var guess = Console.ReadLine().ToUpper();

                    while (!(IsGoodGuess(guess) && DoesItRepeat(guess, lettersChosen, wordsChosen)))
                    {
                        Console.WriteLine("Guess letter or a whole word");
                        guess = Console.ReadLine().ToUpper();
                    }

                    GuessesChosen(guess, lettersChosen, wordsChosen);

                    if (!GuessesAnalysis(guess, word, wordCharArray))
                    {
                        mistakes++;
                    }

                    if (mistakes == 6)
                    {
                        Console.Clear();
                        HangmanPictures(mistakes);
                        PrintCharArray(wordCharArray);
                        PrintLettersAndWordsGuessed(lettersChosen, wordsChosen);
                        Console.WriteLine();
                        Console.WriteLine($"The word was: {word}\n");
                        guessesEnded = true;
                    }
                    else if (DidThePlayerWon(wordCharArray))
                    {
                        Console.Clear();
                        HangmanPictures(mistakes);
                        PrintCharArray(wordCharArray);
                        PrintLettersAndWordsGuessed(lettersChosen, wordsChosen);

                        Soundtrack victorySound = new Soundtrack();
                        victorySound.Victory();

                        Console.WriteLine("You won!");
                        guessesEnded = true;
                    }
                }
                playGame = RestartGame();
                Console.Clear();
            }
        }

        static bool RestartGame()
        {
            Console.WriteLine("Would you like to play again? Y/N");
            string selection = Console.ReadLine().ToLower();

            while (selection != "y" && selection != "n")
            {
                Console.Clear();
                Console.WriteLine("Would you like to play again? Y/N");
                selection = Console.ReadLine().ToLower();
            }

            if (selection == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool DidThePlayerWon(char[] wordCharArray)
        {

            for (int i = 0; i < wordCharArray.Length; i++)
            {
                if (wordCharArray[i] == '_')
                {
                    return false;
                }
            }
            return true;
        }

        static void GuessesChosen(string guess, List<char> lettersChosen, List<string> wordsChosen)
        {
            if (guess.Length == 1)
            {
                lettersChosen.Add(guess[0]);
            }
            else
            {
                wordsChosen.Add(guess);
            }
        }
    
        static void PrintLettersAndWordsGuessed(List<char> lettersChosen, List<string> wordsChosen)
        {
            Console.WriteLine("\n\nLetters guessed: ");

            foreach (var raide in lettersChosen)
            {
                Console.Write(raide + " ");
            }

            Console.WriteLine("\n\nWords guessed: ");

            foreach (var zodis in wordsChosen)
            {
                Console.Write(zodis + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static bool DoesItRepeat(string guess, List<char> lettersChosen, List<string> wordsChosen)
        {
            if (guess.Length == 1)
            {
                for (int i = 0; i < lettersChosen.Count; i++)
                {
                    if (guess[0] == lettersChosen[i])
                    {
                        Console.WriteLine($"Letter {guess[0]} was already used");
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < wordsChosen.Count; i++)
                {
                    if (guess == wordsChosen[i])
                    {
                        Console.WriteLine($"Word {guess} was already used");
                        return false;
                    }
                }
            }
            return true;
        }
        

        static bool IsGoodGuess(string guess)
        {
            if (guess.Length == 1)
            {
                if (guess[0] >= 65 && guess[0] <= 90)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Symbol {guess[0]} is not a letter");
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < guess.Length; i++)
                {
                    if (!(guess[i] >= 65 && guess[i] <= 90))
                    {
                        Console.WriteLine("Guess is not a word");
                        return false;
                    }
                }
            }
            return true;
        }
       
        
        static bool GuessesAnalysis(string guess, string word, char [] wordCharArray)
        {
            bool goodGuess = false;
            if (guess.Length == 1)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == guess[0])
                    {
                        wordCharArray[i] = guess[0];
                        goodGuess = true;
                    }
                }
            }
            else
            {
                if (word == guess)
                {
                    goodGuess = true;

                    for (int i = 0; i < word.Length; i++)
                    {
                        wordCharArray[i] = word[i];
                    }
                }
            }
            return goodGuess;
        }

        static string WordSelection(int selection, string [] movies, string[]hardWords, string[] countries, Random random)
        {
            string word = string.Empty;

            switch (selection)
            {
                case 1:
                    word = movies[random.Next(0, movies.Length)];
                    break;
                case 2:
                    word = hardWords[random.Next(0, hardWords.Length)];
                    break;
                case 3:
                    word = countries[random.Next(0, countries.Length)];
                    break;
                default: 
                    Console.WriteLine("Wrong selection"); 
                    break;
            }
            return word;
        }

        static char[] GetEmptyWordArray(int arraySize)
        {
            char[] wordCharArray = new char[arraySize];

            for (int i = 0; i < wordCharArray.Length; i++)
            {
                wordCharArray[i] = '_';
            }
            return wordCharArray;
        }

        static void PrintCharArray(char [] wordCharArray) 
        {
            string wordGuessed = string.Empty;

            for (int i = 0; i < wordCharArray.Length; i++)
            {
                wordGuessed += $"{ wordCharArray[i]} ";
            }
            Console.WriteLine(wordGuessed);
        }

        static void HangmanPictures(int a)
        {
            switch (a)
            {
                case 0:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||              ");
                    Console.WriteLine(@"| |/         |/              ");
                    Console.WriteLine(@"| |          ||              ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"----------|_        |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 1:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"----------|_        |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 2:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |         .-`--'.          ");
                    Console.WriteLine(@"| |         Y. .Y\           ");
                    Console.WriteLine(@"| |          |   |           ");
                    Console.WriteLine(@"| |          | . |           ");
                    Console.WriteLine(@"| |          |   |           ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"----------|_        |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 3:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |         .-`--'.          ");
                    Console.WriteLine(@"| |        /Y. .Y\           ");
                    Console.WriteLine(@"| |       // |   |           ");
                    Console.WriteLine(@"| |      //  | . |           ");
                    Console.WriteLine(@"| |     ')   |   |           ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"----------|_        |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 4:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |         .-`--'.          ");
                    Console.WriteLine(@"| |        /Y. .Y\           ");
                    Console.WriteLine(@"| |       // |   | \\        ");
                    Console.WriteLine(@"| |      //  | . |  \\       ");
                    Console.WriteLine(@"| |     ')   |   |   (`      ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"| |                          ");
                    Console.WriteLine(@"----------|_        |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 5:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |         .-`--'.          ");
                    Console.WriteLine(@"| |        /Y. .Y\           ");
                    Console.WriteLine(@"| |       // |   | \\        ");
                    Console.WriteLine(@"| |      //  | . |  \\       ");
                    Console.WriteLine(@"| |     ')   |   |   (`      ");
                    Console.WriteLine(@"| |          ||'             ");
                    Console.WriteLine(@"| |          ||              ");
                    Console.WriteLine(@"| |          ||              ");
                    Console.WriteLine(@"| |          ||              ");
                    Console.WriteLine(@"| |         / |              ");
                    Console.WriteLine(@"----------|_`-'     |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");
                    break;

                case 6:
                    Console.WriteLine(@" ___________.._______        ");
                    Console.WriteLine(@"| .__________))______|       ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| | / /      ||              ");
                    Console.WriteLine(@"| |/ /       ||              ");
                    Console.WriteLine(@"| | /        ||.-''.         ");
                    Console.WriteLine(@"| |/         |/  _  \        ");
                    Console.WriteLine(@"| |          ||  `/,|        ");
                    Console.WriteLine(@"| |          (\\`_.'         ");
                    Console.WriteLine(@"| |         .-`--'.          ");
                    Console.WriteLine(@"| |        /Y. .Y\           ");
                    Console.WriteLine(@"| |       // |   | \\        ");
                    Console.WriteLine(@"| |      //  | . |  \\       ");
                    Console.WriteLine(@"| |     ')   |   |   (`      ");
                    Console.WriteLine(@"| |          ||'||           ");
                    Console.WriteLine(@"| |          || ||           ");
                    Console.WriteLine(@"| |          || ||           ");
                    Console.WriteLine(@"| |          || ||           ");
                    Console.WriteLine(@"| |         / | | \          ");
                    Console.WriteLine(@"----------|_`-' `-' |---|    ");
                    Console.WriteLine(@"----------\ \         | |    ");
                    Console.WriteLine(@"| |        \ \        | |    ");
                    Console.WriteLine(@"| |         \ \       | |    ");
                    Console.WriteLine(@"| |          `'       | |    ");
                    Console.WriteLine("\n\n                          ");

                    Soundtrack GameOverSound = new Soundtrack();
                    GameOverSound.Death();
                    
                    break; 
            }
        }
    }
}
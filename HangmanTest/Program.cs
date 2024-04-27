using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Aligator
{

    class HangmanProgram
    {
        public static void Main(string[] args)
        {
            // WS: to nie moze byc sciezka absolute, ten plik musi byc w zrodlach i do niego trzeba dac referencje, zeby mozna bylo kod skompilowac na innym urzadzeniu
            string[] import = File.ReadAllLines("/Users/juliaszczepanek/Desktop/dane_wisielec.csv"); //zapisuje zimportowane słowa do tablicy
            
            String[] wisielec = { "          _____\n          |   |\n          |\n          |\n          |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |\n          |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |   |\n          |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |   |\n          |   |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |   |/\n          |   |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |  \\|/\n          |   |\n          |\n         ---\n", "          _____\n          |   |\n          |   O\n          |  \\|/\n          |   |\n          |  /\n         ---\n", "          _____\n          |   |\n          |   O\n          |  \\|/\n          |   |\n          |  / \\\n         ---\n" }; // inicjacja tablicy z wisielcami

            Random randomObject = new Random(); //deklaracja nowego obiektu random
            int index = randomObject.Next(import.Length); // tworzy nowy obiekt typu random z tablicy i zapisuje go w zmiennej int 
            
            string word = import[index]; // zapisuje wylosowane słowo (import[idex]) do stringa
           
            word = word.ToUpper(); //konwertuje litery w wylosowanym słowie na wielkie

            char[] charArr = word.ToCharArray(); // dodaje słowo do tablicy z charami
            int i = 0; //inicjowanie zmiannej i która
            int wrongValue = 0; //inicjowanie zmiennej która sprawdza ile razy użtkownik wpisał błędną wartość 

            var display = charArr.Select(i => "_").ToArray(); // funkcja która tworzy nową tablice i dodaje "_" tyle razy ile wynosi długość tablicy z literami 
            

            if (word.Contains(" ")) //sprawdza czy słowo wylosowane zawiera spajce
            {
                int iloscSpacji = 0;
                for (int y = 0; y < charArr.Length; y++) //pętla sprawdza indeksy poszczególnych pozycji w slowie i gdy są spacją to zastępuje je spacją 
                {
                    
                    if (word[y].ToString() == " ")
                    {
                        int indeksSpacji = y;
                        display[indeksSpacji] = " ";
                        iloscSpacji++;
                    }
                    
                }
                Console.WriteLine($"The drawn word has {word.Length - iloscSpacji} characters");
            } else
            {
                Console.WriteLine($"The drawn word has {word.Length} characters");
            }

            Array.ForEach(display, Console.Write); //drukuje tablice z podłogami i o ile występują również ze spacjami

            Stopwatch timer = new Stopwatch(); //tworzenie obiektu z timerem
            timer.Start(); //timer startuje

            for (int b = 0; i != 8 || b < word.Length; b++) //pętla programu + zmieniłaś wartość b na 0 
            {
                Console.WriteLine("\nEnter a character or word to guess: ");
                string inputString = Console.ReadLine(); //odczytuje slowo wpisane przez uzytkowanika
                Console.WriteLine("\n");
           
                inputString = inputString.ToUpper(); //konwertuje słowow/litere na z wielkiej litery 

                bool found = false; // inicjowanie wartości found = false 
                foreach (char ch in charArr) //iteruje po całej tablicy
                {

                    if (String.Equals(inputString, word)) //sprawdza czy wprowadzona wartosc przez uzytkowanika jest równa wylosowanemu słowu 
                    {
                        timer.Stop(); //stopuje stopwatch
                        TimeSpan timeTaken = timer.Elapsed; //zapisuje wartośc z timera do zmiennej timeTaken
                        string time = timeTaken.ToString(@"m\:ss\.fff"); //konwertuje czas otrzymany do stringa 

                        Console.WriteLine("\nYou guessed right! The word was: " + word);

                        Console.WriteLine($"Popełniłeś {i} błędów, na {b - 1} prób, z czego {wrongValue} razy wprowadziłeś błędną wartość i zajęło ci to: {time} czasu"); //drukuje w terminalu 
                        using (StreamWriter writetext = new StreamWriter("write.txt")) //tworzy plik tekstowy "write.txt"
                        {
                            writetext.WriteLine($"Popełniłeś {i} błędów, na {b - 1} prób, z czego {wrongValue} razy wprowadziłeś błędną wartość i zajęło ci to: {time} czasu"); //zapisuje do pliku 
                            writetext.Close();//zamyka plik tekstowy
                        }
                        System.Environment.Exit(1); //zamyka program 

                    } else if (inputString.Length != 1 && inputString.Length != word.Length) // zabezpiecza program przed wprowadzeniem błędnych wartości
                    {
                        Console.WriteLine("Invalid input");
                        found = true; //przypisanie wartosci true do zmiennejfound
                        wrongValue++;
                        b--;
                        break;

                    } else if (!String.Equals(inputString, word) & inputString.Length >= word.Length) //sprawdza czy slowow wpisane jest rozne od slowa wylosowanego
                    {
                        break;

                    } else if (inputString.Contains(ch)) //
                    {

                        Console.WriteLine("Great! The word includes: " + inputString);
                        found = true;

                        for (int x = 0; x < charArr.Length; x++) // iteruje po tablicy z literami i sprawdza czy dana litera występuje a jeśli tak to zwraca indeksy i przypisuje wartość pod danym ideksem wpisując ją do tablicy ze spacjami i podłogami 
                        {
                            if (charArr[x] == ch)
                            {
                                int indekslitery = x;
                                display[indekslitery] = inputString;
                            }   
                        }

                        Array.ForEach(display, Console.Write); // drukuje tablice z podłogami, literami które zostały odgadnięte i spacjami 
                        break;
                    }
                }

                var testArray = display.SelectMany(x => x.ToCharArray()); //dodaje wartosci z tablicy z podłogami do nowej tablicy

                bool isEqual = Enumerable.SequenceEqual(charArr,testArray); //zwraca true gdy tablica z literami jest równa tablicy z podłogami
           
                if (isEqual) // jeśli isEqual jest prawdą to drukuje infromacje jeśli nieprawda to nic sie nie dzieje 
                {
                    timer.Stop();
                    TimeSpan timeTaken = timer.Elapsed;
                    string time = timeTaken.ToString(@"m\:ss\.fff");
                 
                    Console.WriteLine("\nYou guessed right! The word was: " + word);
                    Console.WriteLine($"Popełniłeś {i} błędów, na {b - 1} prób, z czego {wrongValue} razy wprowadziłeś błędną wartość i zajęło ci to: {time} czasu");
                    using (StreamWriter writetext = new StreamWriter("write.txt"))
                    {
                        writetext.WriteLine($"Popełniłeś {i} błędów, na {b - 1} prób, z czego {wrongValue} razy wprowadziłeś błędną wartość i zajęło ci to: {time} czasu");
                        writetext.Close();
                    }
                    System.Environment.Exit(1);
                }             

                if (!found) // jeśli found jest false to wtedy wywołuje ifa i drukuje informacje ze wprowadzona wartosc jest nieprawidłowa, found zmienia wartosc na true w momencie gdy wpisana litera jest prawidłowa lub gdy wpisane słowo jest równe wylosowanemu słowu  gdy 
                {
                    Console.WriteLine("\nYou guessed wrong or you entered wrong value!\n" + wisielec[i]);
                    Array.ForEach(display, Console.Write);
                    i++; 
                }
            }
            Console.WriteLine("\n\nYou loose!");
            System.Environment.Exit(1);
        }
    }
}




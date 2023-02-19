﻿using System;
using CsvHelper; // I was able to use the CsvHelper to print out the list of movies but was very difficult to know how to get it to do anything else, Im still new to some of the concepts
using Helper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using BetterConsoleTables;// This one was also hard to understand ontop of looping through the Csvhelper list

namespace MovieLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "movies.csv";
            int maxIndex = 0;
            int i = 0;
            int j = 1000;
            bool done = false;
            bool exit = false;
            int choice = 0;
            List<MovieInfo> list = new List<MovieInfo>();
            List<string> genre = new List<string>();
            Console.WriteLine("Welcome to Blockbuster!");
            do
            {

                Console.WriteLine("1) Add a Movie to the list.");
                Console.WriteLine("2) To see a list of all Movies.");
                Console.WriteLine("3) To exit.");
                choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");  // we created these input methods in intro that come in handy for user management
                do
                {
                    if (choice > 3 || choice < 1)
                    {
                        Console.WriteLine("Please select a menu option");
                        choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");
                    }
                } while (choice > 5 || choice < 1);

                List<UInt64> MovieIds = new List<UInt64>();
                List<string> MovieTitles = new List<string>();
                List<string> MovieGenres = new List<string>();                                    // I wanted to make these into methods but was have a hard time with proper syntax
                StreamReader sr = new StreamReader(file);
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        string[] movieDetails = line.Split(',');
                        MovieIds.Add(UInt64.Parse(movieDetails[0]));
                        MovieTitles.Add(movieDetails[1]);
                        MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                    }
                    else
                    {
                        MovieIds.Add(UInt64.Parse(line.Substring(0, idx - 1)));
                        line = line.Substring(idx + 1);
                        idx = line.IndexOf('"');
                        MovieTitles.Add(line.Substring(0, idx));
                        line = line.Substring(idx + 2);
                        MovieGenres.Add(line.Replace("|", ", "));
                    }
                }
                sr.Close();

                if (choice == 1)
                {
                    string movietitle = Input.GetStringWithPrompt("Enter the movie title: ", "Please try again: "); 
                    List<string> LowerCaseTitle = MovieTitles.ConvertAll(t => t.ToLower());
                    if (LowerCaseTitle.Contains(movietitle.ToLower()))
                    {
                        Console.WriteLine("That movie name already exist");
                    }
                    else
                    {
                        UInt64 movid = MovieIds.Max() + 1;
                        GenreBuilder(genre);
                        string genresString = string.Join("|", genre);
                        movietitle = movietitle.IndexOf(',') != -1 ? $"\"{movietitle}\"" : movietitle;
                        Console.WriteLine($"{movid},{movietitle},{genresString}");
                        StreamWriter sw = new StreamWriter(file, true);
                        sw.WriteLine($"{movid},{movietitle},{genresString}");                                                           // another one I wanted to make these into methods but was have a hard time with proper syntax
                        sw.Close();
                        MovieIds.Add(movid);
                        MovieTitles.Add(movietitle);
                        MovieTitles.Add(genresString);
                        genre.Clear();                                                                                                   // if i didn't clear the genre list it would keep adding them to the next entries
                        

                    }


                }

                else if (choice == 2)

                    using (var streamReader = new StreamReader(file))
                    {
                        using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                        {
                            var records = csvReader.GetRecords<MovieInfo>().ToList();
                            do
                            {
                                done = false;
                                maxIndex = (records.Count);
                                List<MovieInfo> movielist = records.GetRange(i, j);
                                Table table = new Table(TableConfiguration.Unicode());
                                table.From<MovieInfo>(movielist);
                                Console.Write(table.ToString());
                                
                                if ((i + j) < (maxIndex - j))
                                {

                                    i += j;
                                    Console.WriteLine("Press Enter For More Movies: ");                                                   //  and another one I wanted to make these into methods but was have a hard time with proper syntax or maybe in their own class.
                                    Console.ReadLine();                                                                                  // I kind of have a grasp on the concepts of namespace/class/objects but how to navigate and access them is still blurry
                                }                                                                                                       // The movies are listed and put into a table that loops 1000 at a time with some index/range juggling at the end 
                                else if ((i + j) > (maxIndex - j))
                                {
                                    i += j;
                                    j = (maxIndex - i);
                                    if (j == 0)
                                    {
                                        Console.WriteLine("Shows Over!");
                                        i = 0;                                                                                       // had to reset counters or it only ran the list once and never loaded any rows again
                                        j = 1000;
                                        done = true;
                                    }
                                }

                            } while (!done);

                        }
                    }

                else if (choice == 3)
                {
                    exit = true;
                }

            } while (!exit);
            Console.WriteLine("Thank you for visiting Blockbuster, Good Bye!");


            static void GenreBuilder(List<string> genre)
            {
                string userInput;
                string genres;

                userInput = Input.GetStringWithPrompt("Are there any Genres? Y/N: ", "Please try again");
                do
                {
                    
                    if (userInput.ToUpper() == "Y")
                    {
                        genres = Input.GetStringWithPrompt("Please add Genre", "Please try again");
                        genre.Add(genres);
                        userInput = Input.GetStringWithPrompt("Are there any more Genres? Y/N: ", "Please try again");
                    }
                    else if (userInput.ToUpper() == "N" && genre.Count == 0)
                    {
                        genre.Add("(no genres listed)");
                    }
                    else
                    {
                        Console.WriteLine("Please select Y or N ");
                        userInput = "Y";
                    }
                } while (userInput.ToUpper() == "Y");
            }                    
        }
    }  
}






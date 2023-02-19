using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    public class MovieInfo
    {
        [Name("movieId")]
        public int MovieId { get; set; }
        [Name("title")]
        public string Title { get; set; }
        [Name("genres")]
        public string Genres { get; set; }
        
        public MovieInfo()
        {

        }
        public MovieInfo(int movieId, string title, string genres)
        {
            MovieId = movieId;
            Title = title;
            Genres = genres;
        }
        public override string ToString()
        {
            return $"{MovieId},{Title},{Genres}";
        }
    }
    
}

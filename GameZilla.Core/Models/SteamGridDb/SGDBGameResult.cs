using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models.SteamGridDb;
public class SGDBGame
{
    public string[] Types
    {
        get; set;
    }
    public int Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public bool Verified
    {
        get; set;
    }
    public int? ReleaseDate
    {
        get; set;
    }
}

public class SGDBGameResult
{
    public bool Success
    {
        get; set;
    }
    public List<SGDBGame> Data
    {
        get; set;
    }
}
public class ImgResult
{
    public bool Success
    {
        get; set;
    }
    public List<SGDBImg> Data
    {
        get; set;
    }
}

public class SGDBImg
{
    public int Id
    {
        get; set;
    }
    public int Score
    {
        get; set;
    }
    public string Style
    {
        get; set;
    }
    public int Width
    {
        get; set;
    }
    public int Height
    {
        get; set;
    }
    public bool Nsfw
    {
        get; set;
    }
    public bool Humor
    {
        get; set;
    }
    public string Notes
    {
        get; set;
    }
    public string Mime
    {
        get; set;
    }
    public string Language
    {
        get; set;
    }
    public string Url
    {
        get; set;
    }
    public string Thumb
    {
        get; set;
    }
    public bool Lock
    {
        get; set;
    }
    public bool Epilepsy
    {
        get; set;
    }
    public int Upvotes
    {
        get; set;
    }
    public int Downvotes
    {
        get; set;
    }
    public Author Author
    {
        get; set;
    }
}

public class Author
{
    public string Name
    {
        get; set;
    }
    public string Steam64
    {
        get; set;
    }
    public string Avatar
    {
        get; set;
    }
}


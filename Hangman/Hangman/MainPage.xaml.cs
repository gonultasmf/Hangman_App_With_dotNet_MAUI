using System.ComponentModel;
using System.Diagnostics;

namespace Hangman;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	#region UI Properties

	public string Spotlight
	{
		get
		{
			return spotlight;
		}

		set
		{
			spotlight = value;
			OnPropertyChanged();
		}
	}

	public List<char> Letters
	{
		get { return letters; }
		set
		{
			letters = value;
			OnPropertyChanged();
		}
	}

	public string Message
	{
		get { return message; }
		set
		{
			message = value;
			OnPropertyChanged();
		}
	}

	public string GameStatus
	{
		get { return gameStatus; }
		set
		{
			gameStatus = value;
			OnPropertyChanged();
		}
	}

	public string CurrentImage
	{
		get { return currentImage; }
		set
		{
			currentImage = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region Fields

	List<char> guessed = new List<char>();
	List<string> words = new List<string>()
	{
		"intel",
		"amd",
		"hp",
		"monster",
		"dell",
		"asus",
		"macbook",
		"huawei",
		"xioami",
		"lenovo",
		"samsung",
		"msi",
		"acer",
		"casper",
		"honor",
		"gigabyte",
		"python",
		"java",
		"dart",
		"flutter",
		"react",
		"angular",
		"html",
		"css",
		"javascript",
		"cplusplus",
		"ruby",
		"kotlin",
		"swift",
		"golang",
		"typescript",
		"visualbasic",
		"android",
		"ios",
		"macos",
		"windows",
		"linux",
		"tizen",
		"delphi",
		"matlab",
		"shell",
		"maui",
		"php",
		"csharp",
		"mongodb",
		"sql",
		"xaml",
		"json",
		"word",
		"excel",
		"powerpoint",
		"code",
		"hotreload",
		"snippets"
	};

	string answer = "";
	private string spotlight;
	private List<char> letters = new List<char>();
	private string message;
	int mistakes = 0;
	int maxWrong = 6;
	private string gameStatus;
	private string currentImage = "img0.jpg";

	#endregion
	public MainPage()
	{
		InitializeComponent();
		Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
		BindingContext = this;
		PickWord();
		UpdateStatus();
		CalculateWord(answer, guessed);
	}

	#region Game Engine

	private void PickWord()
	{
		answer = words[new Random().Next(0, words.Count)];
		Debug.WriteLine(answer);
	}

	private void CalculateWord(string answer, List<char> guessed)
	{
		var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();
		Spotlight = string.Join(' ', temp);
	}

	private void HandleGuess(char letter)
	{
		if (guessed.IndexOf(letter) == -1)
		{
			guessed.Add(letter);
		}
		if (answer.IndexOf(letter) >= 0)
		{
			CalculateWord(answer, guessed);
			CheckIfGameWon();
		}
		else if (answer.IndexOf(letter) == -1)
		{
			mistakes++;
			UpdateStatus();
			CheckIfGameLost();
			CurrentImage = $"img{mistakes}.jpg";
		}
	}

	private void CheckIfGameLost()
	{
		if (mistakes == maxWrong)
		{
			Message = "You Lost!!!";
			DisableLetters();
		}
	}

	private void DisableLetters()
	{
		foreach (var child in LettersContainer.Children)
		{
			var btn = child as Button;
			if (btn != null)
			{
				btn.IsEnabled = false;
			}
		}
	}
    private void EnableLetters()
    {
        foreach (var child in LettersContainer.Children)
        {
            var btn = child as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }

    private void CheckIfGameWon()
	{
		if (Spotlight.Replace(" ", "") == answer)
		{
			Message = "You Win!";
			DisableLetters();

        }
	}

	private void UpdateStatus()
	{
		GameStatus = $"Errors: {mistakes} of {maxWrong}";
	}

	#endregion

	private void Button_Clicked(object sender, EventArgs e)
	{
		var btn = sender as Button;
		if (btn != null)
		{
			var letter = btn.Text;
			btn.IsEnabled = false;
			HandleGuess(letter[0]);
		}
	}

	private void btnReset_Clicked(object sender, EventArgs e)
	{
		mistakes = 0;
		guessed = new List<char>();
		CurrentImage = "img0.jpg";
		PickWord();
		CalculateWord(answer, guessed);
		Message = "";
		UpdateStatus();
		EnableLetters();
    }
}


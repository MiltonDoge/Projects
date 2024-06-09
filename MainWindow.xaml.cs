using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_Mastermind_Voparil
{
    public partial class MainWindow : Window
    {
        private const int CodeLength = 4; // Délka tajného kódu
        private const int MaxRows = 8; // Maximální počet pokusů
        private readonly Color[] secretCode = new Color[CodeLength]; // Pole pro tajný kód
        private Color selectedColor = Colors.Transparent; // Aktuálně vybraná barva
        private readonly List<List<Ellipse>> guessEllipses = new List<List<Ellipse>>(); // List pro elipsy hráčových pokusů
        private readonly List<List<Ellipse>> leftFeedbackEllipses = new List<List<Ellipse>>(); // List pro elipsy vlevo (správné pozice)
        private readonly List<List<Ellipse>> rightFeedbackEllipses = new List<List<Ellipse>>(); // List pro elipsy vpravo (špatné pozice)
        private int currentRow = 0; // Aktuální řádek pokusu

        public MainWindow()
        {
            InitializeComponent();
            GenerateSecretCode(); // Generování tajného kódu při spuštění
            InitializeGameBoard(); // Inicializace herní desky
        }

        // Generuje náhodný tajný kód z dostupných barev
        private void GenerateSecretCode()
        {
            Random rand = new Random();
            Color[] colors = { Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Purple, Colors.Orange };
            colors = colors.OrderBy(x => rand.Next()).ToArray(); // Náhodné pořadí barev
            for (int i = 0; i < CodeLength; i++)
            {
                secretCode[i] = colors[i]; // Nastavení tajného kódu
            }
        }

        // Inicializuje herní desku
        private void InitializeGameBoard()
        {
            for (int row = 0; row < MaxRows; row++)
            {
                StackPanel rowStack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                List<Ellipse> rowEllipses = new List<Ellipse>();

                // Vytváří levý panel pro zpětnou vazbu
                StackPanel leftFeedbackStack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(10, 0, 10, 0) };
                for (int i = 0; i < CodeLength; i++)
                {
                    Ellipse feedbackEllipse = new Ellipse { Style = (Style)Resources["FeedbackStyle"], Fill = Brushes.Transparent };
                    leftFeedbackStack.Children.Add(feedbackEllipse);
                }
                rowStack.Children.Add(leftFeedbackStack);

                // Vytváří elipsy pro hráčovy pokusy
                for (int col = 0; col < CodeLength; col++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        Stroke = Brushes.Black,
                        Fill = Brushes.White,
                        Tag = new Tuple<int, int>(row, col)
                    };
                    ellipse.MouseDown += Ellipse_MouseDown; // Přidání události kliknutí
                    rowEllipses.Add(ellipse);
                    rowStack.Children.Add(ellipse);
                }

                // Vytváří pravý panel pro zpětnou vazbu
                StackPanel rightFeedbackStack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(10, 0, 0, 0) };
                for (int i = 0; i < CodeLength; i++)
                {
                    Ellipse feedbackEllipse = new Ellipse { Style = (Style)Resources["FeedbackStyle"], Fill = Brushes.Transparent };
                    rightFeedbackStack.Children.Add(feedbackEllipse);
                }
                rowStack.Children.Add(rightFeedbackStack);

                GameStack.Children.Add(rowStack);

                guessEllipses.Add(rowEllipses); // Přidání řady elips do seznamu pokusů
                leftFeedbackEllipses.Add(new List<Ellipse>(leftFeedbackStack.Children.Cast<Ellipse>())); // Přidání elips vlevo do seznamu zpětné vazby
                rightFeedbackEllipses.Add(new List<Ellipse>(rightFeedbackStack.Children.Cast<Ellipse>())); // Přidání elips vpravo do seznamu zpětné vazby
            }
        }

        // Událost při kliknutí na elipsu v herní desce
        private void Ellipse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (selectedColor != Colors.Transparent) // Kontrola, zda je vybrána barva
            {
                Ellipse ellipse = (Ellipse)sender;
                var tag = (Tuple<int, int>)ellipse.Tag;

                if (tag.Item1 == currentRow) // Kontrola, zda se jedná o aktuální řádek
                {
                    ellipse.Fill = new SolidColorBrush(selectedColor); // Nastavení barvy elipsy
                }
            }
        }

        // Událost při kliknutí na elipsu v paletě barev
        private void ColorPalette_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            SolidColorBrush brush = (SolidColorBrush)ellipse.Fill;
            selectedColor = brush.Color; // Nastavení vybrané barvy
        }

        // Událost při kliknutí na tlačítko "Zkontrolovat"
        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            if (currentRow >= MaxRows)
            {
                MessageBox.Show("Hra ukončena. Vyčerpali jste všechny pokusy.");
                return;
            }

            // Získání barev z aktuálního řádku pokusu
            var guessColors = guessEllipses[currentRow].Select(ellipse => ((SolidColorBrush)ellipse.Fill).Color).ToArray();

            if (guessColors.Any(color => color == Colors.White))
            {
                MessageBox.Show("Vyplňte všechny kruhy v aktuální řadě.");
                return;
            }

            bool[] correctPosition = new bool[CodeLength]; // Pole pro správné pozice
            bool[] correctColor = new bool[CodeLength]; // Pole pro správné barvy na špatných pozicích
            var leftFeedbackEllipsesCurrent = leftFeedbackEllipses[currentRow];
            var rightFeedbackEllipsesCurrent = rightFeedbackEllipses[currentRow];

            // Kontrola správných pozic
            for (int i = 0; i < CodeLength; i++)
            {
                if (guessColors[i] == secretCode[i])
                {
                    leftFeedbackEllipsesCurrent[i].Fill = Brushes.Green; // Zelená pro správné pozice
                    correctPosition[i] = true;
                }
            }

            // Kontrola správných barev na špatných pozicích
            for (int i = 0; i < CodeLength; i++)
            {
                if (!correctPosition[i])
                {
                    for (int j = 0; j < CodeLength; j++)
                    {
                        if (!correctPosition[j] && !correctColor[j] && guessColors[i] == secretCode[j])
                        {
                            rightFeedbackEllipsesCurrent[i].Fill = Brushes.Orange; // Oranžová pro správné barvy na špatných pozicích
                            correctColor[j] = true;
                            break;
                        }
                    }
                }
            }

            if (guessColors.SequenceEqual(secretCode)) // Kontrola, zda hráč uhodl kód
            {
                NewGame();
            }
            else
            {
                currentRow++; // Přechod na další řádek pokusu
                if (currentRow >= MaxRows)
                {
                    MessageBox.Show("Hra ukončena. Vyčerpali jste všechny pokusy.");
                }
            }
        }

        // Spuštění nové hry po úspěšném uhodnutí kódu nebo ukončení hry
        private void NewGame()
        {
            MessageBoxResult result = MessageBox.Show("Gratulujeme! Uhodli jste kód! Chcete hrát znovu?", "Konec hry", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                GenerateSecretCode();
                GameStack.Children.Clear();
                guessEllipses.Clear();
                leftFeedbackEllipses.Clear();
                rightFeedbackEllipses.Clear();
                InitializeGameBoard();
                currentRow = 0; // Resetování aktuálního řádku
            }
            else
            {
                Close();
            }
        }
    }
}
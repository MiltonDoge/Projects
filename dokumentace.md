Popis Tříd a Metod
Třída MainWindow
Třída MainWindow reprezentuje hlavní okno aplikace.

Konstanty
CodeLength: Definuje délku tajného kódu (počet barev).
MaxRows: Definuje maximální počet pokusů, které hráč má.
Proměnné
secretCode: Pole pro uložení tajného kódu.
selectedColor: Proměnná pro aktuálně vybranou barvu hráčem.
guessEllipses: List pro uložení elips, které představují pokusy hráče.
leftFeedbackEllipses: List pro uložení elips na levé straně, které poskytují zpětnou vazbu o správných barvách na správných pozicích.
rightFeedbackEllipses: List pro uložení elips na pravé straně, které poskytují zpětnou vazbu o správných barvách na špatných pozicích.
currentRow: Proměnná pro sledování aktuálního řádku pokusu.
Metody
MainWindow(): Konstruktor třídy. Inicializuje komponenty, generuje tajný kód a inicializuje herní desku.
GenerateSecretCode(): Generuje náhodný tajný kód z dostupných barev.
InitializeGameBoard(): Inicializuje herní desku. Vytváří řady elips pro pokusy hráče a zpětnou vazbu.
Ellipse_MouseDown(object sender, MouseButtonEventArgs e): Událost pro kliknutí na elipsu hráče. Nastavuje barvu elipsy na aktuálně vybranou barvu, pokud je elipsa v aktuálním řádku.
ColorPalette_MouseDown(object sender, MouseButtonEventArgs e): Událost pro kliknutí na elipsu v paletě barev. Nastavuje aktuálně vybranou barvu.
CheckGuess_Click(object sender, RoutedEventArgs e): Událost pro kliknutí na tlačítko "Zkontrolovat". Kontroluje aktuální pokus hráče, poskytuje zpětnou vazbu a postupuje na další řádek nebo ukončuje hru, pokud byly vyčerpány všechny pokusy.
NewGame(): Spouští novou hru po úspěšném uhodnutí tajného kódu nebo po ukončení hry. Zobrazuje dialogové okno s možností hrát znovu.

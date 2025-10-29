//Auteur : JMY
//Date   : 25.9.2025
//Lieu   : ETML
//Descr. : Entraînement au test 323
//         Cet outil permet de comparer 2 fichiers (avec le même nombre de lignes) ligne par ligne et indiquer les différences... Il permet aussi de faire du chiffrement

///MENU
Console.WriteLine("+--------------------------------+");
Console.WriteLine("|DIFFIT : A very limited DIFFTOOL|");
Console.WriteLine("+--------------------------------+");

Console.Write("Fichier A: ");
string? pathA = Console.ReadLine();

Console.Write("Fichier B: ");
string? pathB = Console.ReadLine();

// Vérification des entrées utilisateur
var paths = new string?[] { pathA, pathB };
bool filesAreValid = paths.Aggregate(true, (a, b) => a && b != null && File.Exists(b));
if (!filesAreValid)
{
    Console.WriteLine("Erreur: les fichiers doivent être existants et accessibles !");
    Environment.Exit(-2);
}

/// CHARGEMENT DES DONNÉES
// TODO: 01 Charger le contenu texte du fichier A (indice: File.ReadAllLines...)
// resultat : linesA contient toutes les lignes du fichier A
string[] linesA = File.ReadAllLines(pathA!);

// TODO: 02 Charger le contenu texte du fichier B (indice: File.ReadAllLines...)
// resultat : linesB contient toutes les lignes du fichier B
string[] linesB = File.ReadAllLines(pathB!);

// TODO: 03 Vérifier que les fichier ont le même nombre de lignes
// resultat : si !=, message d'erreur et arrêt
if (linesA.Length != linesB.Length)
{
    Console.WriteLine("Erreur: les fichiers n'ont pas le même nombre de ligne");
    Environment.Exit(-2);
}

Console.WriteLine(">Fichiers chargés avec succés");

// TODO: 04 Définir les fonctions de nettoyage
// Une fonction de nettoyage reçoit un texte (une ligne de fichier) et renvoie cette même ligne adaptée
// Il existe la fonction Replace sur les string...
// Le caractère tabulation s’écrit \t
// resultat : fonctions prêtes pour supprimer espaces, tabs et forcer la casse
Func<string, string> cleanSpaces = text => text.Replace(" ", "");
Func<string, string> cleanTabs = text => text.Replace("\t", "");
Func<string, string> enforceCase = text => text.ToLowerInvariant();

/// OPTIONS DE NETTOYAGE
Console.WriteLine("Choisir les options:");

Console.Write("-Ignorer les espaces [o/n]: ");
bool ignoreSpaces = Console.ReadLine() == "o";

Console.Write("-Ignorer les tabulations [o/n]: ");
bool ignoreTabs = Console.ReadLine() == "o";

Console.Write("-Ignorer la casse [o/n]: ");
bool ignoreCase = Console.ReadLine() == "o";

// TODO:  05 Appliquer le nettoyage selon la demande utilisateur
// On conserve une copie des lignes originales pour le chiffrement
var originalLinesA = linesA.ToArray();
var originalLinesB = linesB.ToArray();

// resultat : linesA/linesB transformées selon les options choisies
if (ignoreSpaces)
{
    linesA = linesA.Select(cleanSpaces).ToArray();
    linesB = linesB.Select(cleanSpaces).ToArray();
}

if (ignoreTabs)
{
    linesA = linesA.Select(cleanTabs).ToArray();
    linesB = linesB.Select(cleanTabs).ToArray();
}

if (ignoreCase)
{
    linesA = linesA.Select(enforceCase).ToArray();
    linesB = linesB.Select(enforceCase).ToArray();
}


// TODO: 06 Créer et remplir une liste de LinesComparison à partir de linesA et linesB
// resultat : liste des paires (A,B) par numéro de ligne
List<LinesComparison> comparisons = Enumerable
    .Range(0, linesA.Length)
    .Select(i => new LinesComparison { Number = i, ContentA = linesA[i], ContentB = linesB[i] })
    .ToList();

// TODO: 07 Sélectionner les lignes qui ont des différences
// resultat : seules les lignes différentes sont conservées
var diffLines = comparisons.Where(c => c.ContentA != c.ContentB).ToList();

// TODO: 08 Afficher le nombre de lignes identiques et différentes entre les 2 fichiers
// resultat : ex. "Identiques: 8 | Différentes: 2"
int total = comparisons.Count;
int different = diffLines.Count;
int same = total - different;
Console.WriteLine($"Identiques: {same} | Différentes: {different}");

// TODO: 09 Définir une fonction qui compte les différences (caractères différents) entre deux textes (sera utilisé pour les 2 lignes de A et B...)
// Pour info/rappel, la fonction Zip (comme une fermeture éclair) permet d’associer deux listes.
// Et pour info/rappel, un string est une liste de char...
// Ainsi "12345".Zip("ABCDE", (a, b) => $"{a}{b}").ToList().ForEach(Console.Write);//1A2B3C4D5E
// ATTENTION: zip ne prend que le nombre d’éléments minimum commun entre 2 listes...
// Ceci implique une correction: en plus du nombre de différences, il faut ajouter la différence du nombre de caractères entre les deux...
// resultat : retourne le nombre total de variations sur une paire
Func<LinesComparison, int> countVariations = cmp =>
    cmp.ContentA
        .Zip(cmp.ContentB, (a, b) => a == b ? 0 : 1)
        .Sum() + cmp.LengthVariation;

// TODO: 10 Afficher pour chaque ligne différente, le nombre de variations
// resultat : ex. "Ligne 3: 5 variations"
diffLines.ForEach(c => Console.WriteLine($"Ligne {c.NumberHuman}: {countVariations(c)} variations"));

/// Diff coloré
// TODO: 11 Colorier les différences
// Pour chaque ligne où il y a des différences:
// On affiche ainsi:
// Les lettres similaires sont en vert
// Les lettres différentes sont en rouge (options entre[a/b])
// On n’indique rien sur les caractères en plus ou en moins
// resultat : représentation mixte par ligne avec couleurs
foreach (var cmp in diffLines)
{
    Console.Write($"Ligne {cmp.NumberHuman}: ");
    int minLen = Math.Min(cmp.ContentA.Length, cmp.ContentB.Length);
    for (int i = 0; i < minLen; i++)
    {
        char a = cmp.ContentA[i];
        char b = cmp.ContentB[i];
        if (a == b)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(a);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"[{a}/{b}]");
        }
        Console.ResetColor();
    }
    Console.WriteLine();
}

/// Chiffrement
// TODO: 11 Créer une fonction qui chiffre le 1er fichier en décalant les caratères d’un nombre
//saisi par l’utilisateur (clé)
// Le contenu chiffré est enregistré sur le disque dans le fichier "cipheredA.txt"
// Le pendant de ReadAllLines est WriteAllLines
Console.Write("\n\nSPECIAL FEATURE: Clé de chiffrement [1-25]: ");
byte key = Convert.ToByte(Console.ReadLine());

// resultat : fichier "cipheredA.txt" créé à la racine du programme
Func<char, int, char> shiftLetter = (ch, k) =>
{
    if (ch >= 'a' && ch <= 'z')
    {
        int pos = ch - 'a';
        return (char)('a' + (pos + k) % 26);
    }
    if (ch >= 'A' && ch <= 'Z')
    {
        int pos = ch - 'A';
        return (char)('A' + (pos + k) % 26);
    }
    return ch; // autres caractères inchangés
};

var ciphered = originalLinesA
    .Select(line => new string(line.Select(ch => shiftLetter(ch, key)).ToArray()))
    .ToArray();

File.WriteAllLines("cipheredA.txt", ciphered);


/// <summary>
/// Classe pour porter une information de comparaison
/// </summary>
public class LinesComparison
{
    public int Number { get; set; }
    public string ContentA { get; set; } = "";
    public string ContentB { get; set; } = "";

    /// <summary>
    /// Ajuste le numéro de ligne...
    /// </summary>
    public int NumberHuman
    {
        get => Number + 1;
    }

    public int LengthVariation { get => Math.Abs(ContentA.Length - ContentB.Length); }
}

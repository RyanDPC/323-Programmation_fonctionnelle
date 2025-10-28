# 📚 Guide de Révision : LINQ et Lambda Expressions en C#

## 📑 Table des matières

- [🎯 Concepts Essentiels](#-concepts-essentiels)
  - [Lambda Expressions (Fonctions Fléchées)](#-lambda-expressions-fonctions-fléchées)
  - [LINQ - Language Integrated Query](#-linq---language-integrated-query)
- [🔧 Méthodes LINQ Principales](#-méthodes-linq-principales)
  - [Select (Map/Transformation)](#-select-maptransformation)
  - [Where (Filter)](#-where-filter)
  - [Aggregate (Reduce/Fold)](#-aggregate-reducefold)
  - [Zip (Fermeture Éclair)](#-zip-fermeture-éclair)
  - [Range (Générateur de Nombres)](#-range-générateur-de-nombres)
  - [ToArray / ToList](#-toarray--tolist)
  - [Count](#-count)
  - [ForEach](#-foreach)
- [🎓 Exemples Pratiques Réunis](#-exemples-pratiques-réunis)
  - [Exemple 1 : Traitement de Texte](#exemple-1--traitement-de-texte)
  - [Exemple 2 : Comparaison de Collections](#exemple-2--comparaison-de-collections)
  - [Exemple 3 : Transformation en Chaînage](#exemple-3--transformation-en-chaînages)
  - [Exemple 4 : Chiffrement Caesar](#exemple-4--chiffrement-caesar)
- [🔑 Points Importants à Retenir](#-points-importants-à-retenir)
  - [DOs](#-dos)
  - [DON'Ts](#-donts)
- [🧩 Patterns Fréquents au Test](#-patterns-fréquents-au-test)
  - [Pattern 1 : Créer une liste d'objets à partir de deux listes](#pattern-1--créer-une-liste-dobjets-à-partir-de-deux-listes)
  - [Pattern 2 : Compter les différences entre deux textes](#pattern-2--compter-les-différences-entre-deux-textes)
  - [Pattern 3 : Filtrer et transformer](#pattern-3--filtrer-et-transformer)
  - [Pattern 4 : Appliquer une fonction conditionnelle](#pattern-4--appliquer-une-fonction-conditionnelle)
- [📝 Rappels Techniques](#-rappels-techniques)
  - [Déclaration de Func](#déclaration-de-func)
  - [Utilisation pratique](#utilisation-pratique)
- [🎯 Exercice Type "Test"](#-exercice-type-test)
- [💡 Conseils pour le Test](#-conseils-pour-le-test)

## 🎯 Concepts Essentiels

### 1. **Lambda Expressions (Fonctions Fléchées)**

Une lambda est une fonction anonyme qui peut être assignée à une variable.

```csharp
// Syntaxe : paramètres => expression
Func<int, int> doubleX = x => x * 2;

// Plusieurs paramètres
Func<int, int, int> add = (a, b) => a + b;

// Corps avec accolades pour plusieurs instructions
Func<string, string> upperCase = text => 
{
    return text.ToUpper();
};
```

**Exemple concret :**
```csharp
// Transformer une liste de nombres en leurs carrés
var nombres = new[] { 1, 2, 3, 4, 5 };
var carres = nombres.Select(x => x * x).ToList();
// Résultat : [1, 4, 9, 16, 25]
```

---

### 2. **LINQ - Language Integrated Query**

Permet de manipuler des collections avec des méthodes fluides (méthodes d'extension).

## 🔧 Méthodes LINQ Principales

### **Select** (Map/Transformation)
Transforme chaque élément selon une fonction.

```csharp
var personnes = new[] { "Alice", "Bob", "Charlie" };
var longueurs = personnes.Select(nom => nom.Length).ToList();
// Résultat : [5, 3, 7]

// Avec index
var avecIndex = personnes.Select((nom, i) => $"{i}: {nom}").ToList();
// Résultat : ["0: Alice", "1: Bob", "2: Charlie"]
```

### **Where** (Filter)
Filtre les éléments selon une condition.

```csharp
var nombres = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var pairs = nombres.Where(n => n % 2 == 0).ToList();
// Résultat : [2, 4, 6, 8, 10]
```

### **Aggregate** (Reduce/Fold)
Réduit une collection à une seule valeur.

```csharp
var nombres = new[] { 1, 2, 3, 4, 5 };

// Somme
int somme = nombres.Aggregate(0, (acc, n) => acc + n);
// Résultat : 15

// Produit
int produit = nombres.Aggregate(1, (acc, n) => acc * n);
// Résultat : 120

// Chaîne de caractères
string concat = nombres.Aggregate("", (acc, n) => acc + n.ToString());
// Résultat : "12345"
```

### **Zip** (Fermeture Éclair)
Associe deux listes élément par élément.

```csharp
var suite1 = new[] { 1, 2, 3, 4 };
var suite2 = new[] { "A", "B", "C", "D" };
var zippe = suite1.Zip(suite2, (a, b) => $"{a}{b}").ToList();
// Résultat : ["1A", "2B", "3C", "4D"]

// Attention : ne prend que le minimum d'éléments communs
var court = new[] { 1, 2 };
var long = new[] { "A", "B", "C", "D" };
var mixte = court.Zip(long, (a, b) => $"{a}{b}").ToList();
// Résultat : ["1A", "2B"] seulement !
```

### **Range** (Générateur de Nombres)
Crée une séquence de nombres.

```csharp
var indices = Enumerable.Range(0, 5).ToList();
// Résultat : [0, 1, 2, 3, 4]

// Combine avec Select pour créer des données
var personnes = new[] { "Alice", "Bob", "Charlie" };
var avecIndex = Enumerable
    .Range(0, personnes.Length)
    .Select(i => new { Index = i, Nom = personnes[i] })
    .ToList();
```

### **ToArray / ToList**
Convertit en tableau ou liste.

```csharp
var nombres = new[] { 1, 2, 3 };
var liste = nombres.ToList();
var tableau = nombres.ToArray();
```

### **Count**
Compte les éléments (ou les éléments selon une condition).

```csharp
var nombres = new[] { 1, 2, 3, 4, 5 };
int total = nombres.Count(); // 5
int pairs = nombres.Count(n => n % 2 == 0); // 2
```

### **ForEach**
Applique une action à chaque élément.

```csharp
var nombres = new[] { 1, 2, 3 };
nombres.ToList().ForEach(n => Console.WriteLine($"Valeur: {n}"));
```

---

## 🎓 Exemples Pratiques Réunis

### Exemple 1 : Traitement de Texte
```csharp
string texte = "Bonjour le monde";

// Extraire chaque mot et obtenir sa longueur
var mots = texte.Split(' ').Select(mot => mot.Length).ToList();
// Résultat : [7, 2, 6]

// Compter les mots de plus de 3 caractères
int longsMots = texte.Split(' ').Count(mot => mot.Length > 3);
// Résultat : 2
```

### Exemple 2 : Comparaison de Collections
```csharp
string ligneA = "Hello";
string ligneB = "HellX";

// Compter les caractères différents
int differences = ligneA
    .Zip(ligneB, (a, b) => a == b ? 0 : 1)
    .Sum();
// Résultat : 1 (le dernier caractère diffère)

// Plus la différence de longueur
int longueurDiff = Math.Abs(ligneA.Length - ligneB.Length);
int totalDiff = differences + longueurDiff;
```

### Exemple 3 : Transformation en Chaînage
```csharp
var nombres = new[] { 1, 2, 3, 4, 5, 6 };

var resultat = nombres
    .Where(n => n % 2 == 0)      // [2, 4, 6]
    .Select(n => n * n)          // [4, 16, 36]
    .ToList();                   // List<int>
```

### Exemple 4 : Chiffrement Caesar
```csharp
// Créer une fonction de décalage
Func<char, int, char> shiftLetter = (ch, key) =>
{
    if (ch >= 'a' && ch <= 'z')
    {
        int pos = ch - 'a';
        return (char)('a' + (pos + key) % 26);
    }
    return ch;
};

// Chiffrer un texte ligne par ligne
string[] lignes = File.ReadAllLines("source.txt");
var chiffre = lignes
    .Select(line => new string(
        line.Select(ch => shiftLetter(ch, 3)).ToArray()))
    .ToArray();
File.WriteAllLines("chiffre.txt", chiffre);
```

---

## 🔑 Points Importants à Retenir

### ✅ **DOs**
- Utilisez `Select` pour transformer chaque élément
- Utilisez `Where` pour filtrer
- Utilisez `Aggregate` pour accumuler/réduire
- Chaînez les méthodes LINQ pour des transformations complexes
- N'oubliez pas `.ToArray()` ou `.ToList()` pour matérialiser les résultats

### ❌ **DON'Ts**
- Ne pas oublier que `Zip` s'arrête à la liste la plus courte
- Ne pas appeler plusieurs fois la même requête sans la matérialiser (ToList/ToArray)
- Attention aux indexes : `Select` avec indexe utilise `Select((item, index) => ...)`

---

## 🧩 Patterns Fréquents au Test

### Pattern 1 : Créer une liste d'objets à partir de deux listes
```csharp
// Situation : On a deux listes parallel et on veut les combiner
string[] noms = { "Alice", "Bob" };
int[] ages = { 25, 30 };

// Solution : Enumerable.Range + Select
var personnes = Enumerable
    .Range(0, noms.Length)
    .Select(i => new { Nom = noms[i], Age = ages[i] })
    .ToList();
```

### Pattern 2 : Compter les différences entre deux textes
```csharp
string a = "Hello";
string b = "HellX";

int diff = a
    .Zip(b, (charA, charB) => charA == charB ? 0 : 1)
    .Sum() + Math.Abs(a.Length - b.Length);
```

### Pattern 3 : Filtrer et transformer
```csharp
// Garder seulement les nombres positifs et les doubler
var numbers = new[] { -5, 2, -3, 8, 1 };
var result = numbers
    .Where(n => n > 0)
    .Select(n => n * 2)
    .ToList();
```

### Pattern 4 : Appliquer une fonction conditionnelle
```csharp
string[] lignes = { "bonjour", "BONJOUR", "Salut" };
bool ignoreCase = true;

// Solution : appliquer conditionnellement
var nettoyees = lignes;
if (ignoreCase)
{
    nettoyees = lignes.Select(l => l.ToLower()).ToArray();
}

// Ou en une ligne avec Select conditionnel
var nettoyees2 = lignes.Select(l => ignoreCase ? l.ToLower() : l).ToArray();
```

---

## 📝 Rappels Techniques

### Déclaration de Func
```csharp
// Func<TypeEntrée, TypeSortie>
Func<int, int> double = x => x * 2;

// Func<Type1, Type2, TypeSortie>
Func<int, int, int> multiply = (a, b) => a * b;

// Pour string vers string
Func<string, string> upper = text => text.ToUpper();

// Pour char vers char
Func<char, char> toLower = ch => char.ToLower(ch);
```

### Utilisation pratique
```csharp
// Définir une fonction de nettoyage
Func<string, string> removeSpaces = text => text.Replace(" ", "");

// L'appliquer à une liste
var lignes = new[] { "hello world", "foo bar" };
var nettoyees = lignes.Select(removeSpaces).ToArray();
// Résultat : ["helloworld", "foobar"]
```

---

## 🎯 Exercice Type "Test"

**Consigne :** Vous avez deux fichiers texte avec le même nombre de lignes. Écrivez le code pour :
1. Charger les deux fichiers
2. Comparer ligne par ligne
3. Afficher le nombre de différences

**Solution :**
```csharp
// Charger les fichiers
string[] linesA = File.ReadAllLines("fileA.txt");
string[] linesB = File.ReadAllLines("fileB.txt");

// Vérifier même longueur
if (linesA.Length != linesB.Length)
{
    Console.WriteLine("Erreur: lignes différentes");
    return;
}

// Créer les comparaisons avec index
var comparisons = Enumerable
    .Range(0, linesA.Length)
    .Select(i => new { 
        Numero = i, 
        LigneA = linesA[i], 
        LigneB = linesB[i] 
    })
    .ToList();

// Filtrer les lignes différentes
var differents = comparisons
    .Where(c => c.LigneA != c.LigneB)
    .ToList();

// Afficher
Console.WriteLine($"Différences: {differents.Count}");
differents.ForEach(c => 
    Console.WriteLine($"Ligne {c.Numero + 1}: [{c.LigneA} vs {c.LigneB}]"));
```

---

## 💡 Conseils pour le Test

1. **Lisez attentivement** la consigne - comprenez ce que chaque TODO demande
2. **Identifiez** quelle méthode LINQ utiliser (Select, Where, Aggregate, Zip, etc.)
3. **Testez** mentalement sur un exemple simple
4. **Vérifiez** la syntaxe lambda : `paramètre => expression`
5. **N'oubliez pas** de matérialiser (ToArray/ToList) quand nécessaire
6. **Attention aux index** : utilisez `Range(0, length)` pour créer des indices

Bon courage ! 🚀


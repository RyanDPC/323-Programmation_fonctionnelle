using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recherche
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// A. Filtrage basique //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string[] words = { "bonjour", "hello", "monde", "vert", "rouge", "bleu", "jaune", "xannafshdu" };

            //var filetered = words.Where(w => !w.Contains('x') && w.Length >= 4 && w.Length == (int)words.Average(y => y.Length));
            //filetered.Reverse().ToList().ForEach(w => Console.WriteLine(w));
            //filetered.OrderBy(w => w).ToList().ForEach(w => Console.WriteLine(w));
            //filetered.OrderByDescending(w => w).ToList().ForEach(w => Console.WriteLine(w));

            //foreach (var word in filetered)
            //{
            //    Console.WriteLine(word);
            //}

            //Console.ReadLine();
            /// B. Données parasites 1 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string[] wordsParasite = { "whatThe!!!", "bonjour", "hello", "monde", "vert", "rouge", "bleu", "jaune", "My kingdom for a horse !", "Ooops I did it again" };
            //wordsParasite
            //                 .OrderByDescending(z => z.Length)
            //                 .Skip(3)
            //                 .ToList().ForEach(z => Console.WriteLine(z));

            //Console.ReadLine();
            /// C. Données parasites 2 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string[] wordsParasite2 = { "+++++", "<<<<<", ">>>>>", "bonjour", "hello", "@@@@", "vert", "rouge", "bleu", "jaune", "#####", "%%%%%%%" };

            //char[] specialChars = "+*ç%&/()=@#°§¬|¢><".ToCharArray();

            //wordsParasite2
            //             .Where(w => !w.Any(c => specialChars.Contains(c)))
            //             .ToList().ForEach(w => Console.WriteLine(w));

            //wordsParasite2.Where(x => Regex.IsMatch(x, "^[a-zA-Z]+$")).ToList().ForEach(x => Console.WriteLine(x));

            //Console.ReadLine();

            /// D. Élitisme //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string[] wordsElitisme = { "i am the winner", "hello", "monde", "vert", "rouge", "bleu", "i am the looser" };

            //// Winner
            //Console.WriteLine($"The Winner is: {wordsElitisme.First()}");

            //// Looser
            //Console.WriteLine($"The Looser is: {wordsElitisme.Last()}");

            //Console.ReadLine();

            /// D.^Partie 2: Epsilon /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Dictionary<char, double> frenchLetterFrequencies = new Dictionary<char, double>(){
            {'A', 0.0815}, {'B', 0.0097}, {'C', 0.0326}, {'D', 0.0367}, {'E', 0.1472},
            {'F', 0.0107}, {'G', 0.0087}, {'H', 0.0074}, {'I', 0.0754}, {'J', 0.0061},
            {'K', 0.0005}, {'L', 0.0546}, {'M', 0.0297}, {'N', 0.0709}, {'O', 0.0570},
            {'P', 0.0252}, {'Q', 0.0136}, {'R', 0.0669}, {'S', 0.0795}, {'T', 0.0724},
            {'U', 0.0631}, {'V', 0.0184}, {'W', 0.0004}, {'X', 0.0043}, {'Y', 0.0030},
            {'Z', 0.0012}
        };

                double Epsilon(string word)
                {
                    word = word.ToUpper();
                    return word
                        .Where(c => frenchLetterFrequencies.ContainsKey(c))
                        .GroupBy(c => c)
                        .Sum(g => frenchLetterFrequencies[g.Key] / g.Count());
                }

                Console.WriteLine(Epsilon("epsilon"));
                Console.ReadLine();
        }
        /// D. Partie 3: Dictionnaire /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        List<string> frenchWords = new List<string>() {
    "Merci",
    "Hotdog",
    "Oui",
    "Non",
    "Désolé",
    "Réunion",
    "Manger",
    "Boire",
    "Téléphone",
    "Ordinateur",
    "Internet",
    "Email",
    "Sandwich",
    "Hello",
    "Taxi",
    "Hotel",
    "Gare",
    "Train",
    "Bus",
    "Métro",
    "Tramway",
    "Vélo",
    "Voiture",
    "Piéton",
    "Feu rouge",
    "Cédez",
    "Ralentir",
    "gauche",
    "droite",
    "Continuer",
    "Sandwich",
    "Retourner",
    "Arrêter",
    "Stationnement",
    "Parking",
    "Interdit",
    "Péage",
    "Trafic",
    "Route",
    "Rond-point",
    "Football",
    "Carrefour",
    "Feu",
    "Panneau",
    "Vitesse",
    "Tramway",
    "Aéroport",
    "Héliport",
    "Port",
    "Ferry",
    "Bateau",
    "Canot",
    "Kayak",
    "Paddle",
    "Surf",
    "Plage",
    "Mer",
    "Océan",
    "Rivière",
    "Lac",
    "Étang",
    "Marais",
    "Forêt",
    "Hello",
    "Montagne",
    "Vallée",
    "Plaine",
    "Désert",
    "Jungle",
    "Savane",
    "Volleyball",
    "Tundra",
    "Glacier",
    "Neige",
    "Pluie",
    "Soleil",
    "Nuage",
    "Vent",
    "Tempête",
    "Ouragan",
    "Tornade",
    "Séisme",
    "Tsunami",
    "Volcan",
    "Éruption",
    "Ciel"
};
    }
}

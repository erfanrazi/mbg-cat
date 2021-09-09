using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards_analysis
{
    class Card
    {
        public List<Part> parts;
        public Feature feature;
    }

    class Part
    {
        List<string> nodes;

        public bool Has(Feature feature)
        {

        }
    }

    enum Feature
    {
        junc, midway,
        b,
        T, C, S,
        Tc, Cc, Sc,
        Td, Cd, Sd,
        Tp, Cp, Sp,
        mwb,
        mwT, mwC, mwS,
        mwTc, mwCc, mwSc,
        mwTd, mwCd, mwSd,
        mwTp, mwCp, mwSp,
    }

    class Program
    {
        public static Random randomGenerator;

        static void Main(string[] args)
        {
            randomGenerator = new Random();

            List<Card> cards = new List<Card>();
            try
            {
                cards = LoadCardsFromFile(@"D:\Projects\mahdi boardgame\cards\cards2.txt");
            }
            catch (Exception)
            {
                Console.WriteLine("File not found");
                return;
            }

            // repeat the whole experiment 5 times
            for (int k = 0; k < 5; k++)
            {
                int numCards = cards.Count;
                int numCardsToPick = 3;
                int numOkCards = 0;
                for (int j = 0; j < 1000; j++)
                {
                    List<int> randoms = UniqueRandoms(numCardsToPick, numCards);

                    var _cards = new List<Card>();
                    foreach (int r in randoms)
                        _cards.Add(cards[r]);

                    foreach (Card card in _cards)
                    {
                        // card features
                        bool card_b;
                        bool card_T, card_C, card_S;
                        bool card_Tc, card_Cc, card_Sc;
                        bool card_Td, card_Cd, card_Sd;
                        bool card_Tp, card_Cp, card_Sp;
                        bool card_junc, card_midway;
                        card_b = false;
                        card_T = card_C = card_S = false;
                        card_Tc = card_Cc = card_Sc = false;
                        card_Td = card_Cd = card_Sd = false;
                        card_Tp = card_Cp = card_Sp = false;
                        card_junc = card_midway = false;

                        // extract card features
                        foreach (var part in card.roadParts)
                        {
                            // part features
                            bool part_b;
                            bool part_T, part_C, part_S;
                            bool part_Tc, part_Cc, part_Sc;
                            bool part_Td, part_Cd, part_Sd;
                            bool part_Tp, part_Cp, part_Sp;
                            bool part_junc, part_midway;
                            part_b = false;
                            part_T = part_C = part_S = false;
                            part_Tc = part_Cc = part_Sc = false;
                            part_Td = part_Cd = part_Sd = false;
                            part_Tp = part_Cp = part_Sp = false;
                            part_junc = part_midway = false;

                            foreach (string s in part)
                            {
                                if (s == "b") has_b = true;
                                else if (s == "Tc") has_Tc = has_T = true;
                                else if (s == "Td") has_Td = has_T = true;
                                else if (s == "Tp") has_Tp = has_T = true;
                                else if (s == "Cc") has_Cc = has_C = true;
                                else if (s == "Cd") has_Cd = has_C = true;
                                else if (s == "Cp") has_Cp = has_C = true;
                                else if (s == "Sc") has_Sc = has_S = true;
                                else if (s == "Sd") has_Sd = has_S = true;
                                else if (s == "Sp") has_Sp = has_S = true;
                            }

                            if (part.Length == 3)
                            {
                                if ((part[0] + part[1] + part[2]).All(char.IsDigit))
                                    has_junc = true;
                                else if ((part[0] + part[1] + part[2]).IndexOfAny(new char[] { 'T', 'C', 'S' }) != -1)
                                    has_midway = has_mwt = true;
                            }
                        }
                    }

                    if (has_Tc && has_C && has_S && (has_junc || has_mwtc || has_mwC || has_mwS))
                    {
                        numOkCards++;
                    }
                }

                Console.WriteLine("number of ok cards: " + numOkCards.ToString());
            }
        }

        private static List<Card> LoadCardsFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);

            // read whole file
            List<Card> cards = new List<Card>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] words = line.Split(' ', '[', ']');

                if (words[0].ToLower() == "card")
                {
                    Card card = new Card();
                    card.roadParts = new List<string[]>();
                    for (int i = 3; i < words.Count(); i += 3)
                    {
                        string[] codes = words[i].Split(',');
                        card.roadParts.Add(codes);
                    }
                    cards.Add(card);
                }
            }

            return cards;
        }

        private static List<int> UniqueRandoms(int n, int N)
        {
            List<int> uniqueRandoms = new List<int>();

            // choose n random numbers from range [0, N)
            for (int i = 0; i < n; i++)
            {
                int x;
                do x = randomGenerator.Next(N);
                while (uniqueRandoms.Contains(x));
                uniqueRandoms.Add(x);
            }

            return uniqueRandoms;
        }

    }
}

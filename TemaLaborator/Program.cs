using System;
using System.Collections.Generic;

namespace InchirieriMasini
{
    public class Masina
    {
        public string Marca { get; set; }
        public string Model { get; set; }
        public string NumarInmatriculare { get; set; }
        public bool EsteDisponibila { get; set; }
    }

    public class Client
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
    }

    public class Angajat
    {
        public string Utilizator { get; set; }
        public string Parola { get; set; }
    }

    public class Inchiriere
    {
        public Masina MasinaInchiriata { get; set; }
        public Client Clientul { get; set; }
        public DateTime DataInceput { get; set; }
        public DateTime DataSfarsit { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {
            bool rulare = true;

            List<Masina> listaMasini = new List<Masina>();
            List<Client> listaClienti = new List<Client>();

            while (rulare)
            {
                Console.Clear();
                Console.WriteLine("==== Sistem Inchirieri Masini ====");
                Console.WriteLine("1 - Adaugare masina noua.");
                Console.WriteLine("2 - Afiseaza toate masinile.");
                Console.WriteLine("3 - Cauta masina dupa marca.");
                Console.WriteLine("4 - Client Nou.");
                Console.WriteLine("5 - Iesire.");
                Console.Write("\nSelectati o optiune:");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":

                        Masina masinaNoua = new Masina();

                        Console.Write("Introduceti marca masinii:");
                        masinaNoua.Marca = Console.ReadLine();

                        Console.Write("Introduceti modelul masinii:");
                        masinaNoua.Model = Console.ReadLine();

                        Console.Write("Introduceto numarul de inmatriculare:");
                        masinaNoua.NumarInmatriculare = Console.ReadLine();

                        masinaNoua.EsteDisponibila = true;

                        listaMasini.Add(masinaNoua);

                        Console.WriteLine("\nMasina a fost salvata cu succes!");
                        break;

                    case "2":
                        Console.WriteLine("\n--- Lista Masini ---");
                        if (listaMasini.Count == 0)
                            Console.WriteLine("Nu exista masini.");

                        foreach (var masina in listaMasini)
                        {
                            Console.WriteLine($"- {masina.Marca} {masina.Model} [{masina.NumarInmatriculare}]");
                        }

                        break;

                    case "3":

                        Console.Write("\nIntroduceti marca pentru cautare: ");
                        string cautare = Console.ReadLine();
                        bool gasit = false;

                        foreach (var masina in listaMasini)
                        {
                            if (masina.Marca.Equals(cautare, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"[GASIT] {masina.Marca} {masina.Model} - Nr: {masina.NumarInmatriculare}");
                                gasit = true;
                            }
                        }

                        if (gasit == false) Console.WriteLine("----Nu exista aceasta marca de masina in acest garaj----");
                        break;



                    case "4":
                        Console.WriteLine("\n--- Date Client ---");

                        Client clientNou = new Client();

                        Console.Write("Nume client:");
                        clientNou.Nume = Console.ReadLine();

                        Console.Write("Prenume client:");
                        clientNou.Prenume = Console.ReadLine();

                        Console.Write("CNP client:");
                        clientNou.CNP = Console.ReadLine();

                        listaClienti.Add(clientNou);
                        Console.WriteLine("\nClientul a fost adaugat cu succes!");

                        break;


                    case "5":
                        rulare = false;
                        Console.WriteLine("---Program terminat---");
                        Console.WriteLine("La revedere");
                        break;

                    default:
                        Console.WriteLine("Optiune invalida!" +
                            "Incercati din nou.");
                        break;

                                                                
                }

                if (rulare)
                {
                    Console.WriteLine("\nApasati ENTER pentru meniu:");
                    Console.ReadKey();
                }

            }
               
        }
    }
}
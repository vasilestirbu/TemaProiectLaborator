using System;
using System.Collections.Generic;
using System.IO;

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
        static void SalveazaMasina(Masina m)
        {
            using(StreamWriter sw = new StreamWriter("masini.txt", true))
            {
                sw.WriteLine($"{m.Marca};{m.Model};{m.NumarInmatriculare};{m.EsteDisponibila}");    
            }
        }

        static List<Masina> IncarcaMasini()
        {
            List<Masina> lista = new List<Masina>();
            if (!File.Exists("masini.txt")) return lista;

            string[] linii = File.ReadAllLines("masini.txt");
            foreach(var linie in linii)
            {
                string[] date = linie.Split(';');
                Masina m = new Masina
                {
                    Marca = date[0],
                    Model = date[1],
                    NumarInmatriculare = date[2],
                    EsteDisponibila = bool.Parse(date[3])
                };
                lista.Add(m);
            }
            return lista;
        }

        static void SalveazaClient(Client c)
        {
            using (StreamWriter sw = new StreamWriter("clienti.txt", true))
            {
                sw.WriteLine($"{c.Nume};{c.Prenume};{c.CNP}");
            }
        }

        static List<Client> IncarcaClienti()
        {
            List<Client> lista = new List<Client>();
            if (!File.Exists("clienti.txt")) return lista;

            string[] linii = File.ReadAllLines("clienti.txt");
            foreach (var linie in linii)
            {
                string[] date = linie.Split(';');
                Client c = new Client
                {
                    Nume = date[0],
                    Prenume = date[1],
                    CNP = date[2]
                };
                lista.Add(c);
            }
            return lista;
        }

        static void SuprascrieMasini(List<Masina> masini)
        {
            using (StreamWriter sw = new StreamWriter("masini.txt", false))
            {
                foreach (var m in masini)
                    sw.WriteLine($"{m.Marca};{m.Model};{m.NumarInmatriculare};{m.EsteDisponibila}");
            }
        }

        static void SuprascrieClienti(List<Client> clienti)
        {
            using (StreamWriter sw = new StreamWriter("clienti.txt", false))
            {
                foreach (var c in clienti)
                    sw.WriteLine($"{c.Nume};{c.Prenume};{c.CNP}");
            }
        }

        static void Main(string[] args)
        {
            bool rulare = true;

            List<Masina> listaMasini = IncarcaMasini();
            List<Client> listaClienti = IncarcaClienti();

            while (rulare)
            {
                Console.Clear();
                Console.WriteLine("==== Sistem Inchirieri Masini ====");
                Console.WriteLine("1 - Adaugare masina noua.");
                Console.WriteLine("2 - Afiseaza toate masinile.");
                Console.WriteLine("3 - Cauta masina dupa marca.");
                Console.WriteLine("4 - Client Nou.");
                Console.WriteLine("5 - Iesire.");
                Console.WriteLine("6 - Modifica masina dupa numar");
                Console.WriteLine("7 - Modifica client dupa CNP");
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

                    case "6":
                        Console.Write("Introdu numarul de inmatriculare: ");
                        string nr = Console.ReadLine();
                        bool modificat = false;

                        for (int i = 0; i < listaMasini.Count; i++)
                        {
                            if (listaMasini[i].NumarInmatriculare == nr)
                            {
                                Console.Write("Marca noua: ");
                                listaMasini[i].Marca = Console.ReadLine();
                                Console.Write("Model nou: ");
                                listaMasini[i].Model = Console.ReadLine();
                                modificat = true;
                                break;
                            }
                        }

                        if (modificat)
                        {
                            SuprascrieMasini(listaMasini);
                            Console.WriteLine("Masina modificata!");
                        }
                        else
                        {
                            Console.WriteLine("Masina nu a fost gasita!");
                        }
                        break;

                    case "7":
                        Console.Write("CNP client: ");
                        string cnp = Console.ReadLine();
                        bool gasitClient = false;

                        foreach (var c in listaClienti)
                        {
                            if (c.CNP == cnp)
                            {
                                Console.Write("Nume nou: ");
                                c.Nume = Console.ReadLine();
                                Console.Write("Prenume nou: ");
                                c.Prenume = Console.ReadLine();
                                gasitClient = true;
                                break;
                            }
                        }

                        if (gasitClient)
                        {
                            SuprascrieClienti(listaClienti);
                            Console.WriteLine("Client modificat!");
                        }
                        else
                        {
                            Console.WriteLine("Client negasit!");
                        }
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
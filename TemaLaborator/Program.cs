using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InchirieriMasini
{
    
    public enum Culoare
    {
        Rosu,
        Alb,
        Negru
    }

    [Flags]
    public enum Optiuni
    {
        Nimic = 0,
        AerConditionat = 1,
        Navigatie = 2,
        CutieAutomata = 4
    }

    
    public class Masina
    {
        public string Marca { get; set; }
        public string Model { get; set; }
        public string NumarInmatriculare { get; set; }
        public bool EsteDisponibila { get; set; }

        public Culoare CuloareMasina { get; set; }
        public Optiuni OptiuniMasina { get; set; }
    }

    public class Client
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
    }

    class Program
    {
        
        static void SalveazaMasina(Masina m)
        {
            using (StreamWriter sw = new StreamWriter("masini.txt", true))
            {
                sw.WriteLine($"{m.Marca};{m.Model};{m.NumarInmatriculare};{m.EsteDisponibila};{m.CuloareMasina};{m.OptiuniMasina}");
            }
        }

        
        static List<Masina> IncarcaMasini()
        {
            List<Masina> lista = new List<Masina>();
            if (!File.Exists("masini.txt")) return lista;

            string[] linii = File.ReadAllLines("masini.txt");
            foreach (var linie in linii)
            {
                string[] date = linie.Split(';');

                Masina m = new Masina
                {
                    Marca = date[0],
                    Model = date[1],
                    NumarInmatriculare = date[2],
                    EsteDisponibila = bool.Parse(date[3]),
                    CuloareMasina = (Culoare)Enum.Parse(typeof(Culoare), date[4]),
                    OptiuniMasina = (Optiuni)Enum.Parse(typeof(Optiuni), date[5])
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
                {
                    sw.WriteLine($"{m.Marca};{m.Model};{m.NumarInmatriculare};{m.EsteDisponibila};{m.CuloareMasina};{m.OptiuniMasina}");
                }
            }
        }

        static void SuprascrieClienti(List<Client> clienti)
        {
            using (StreamWriter sw = new StreamWriter("clienti.txt", false))
            {
                foreach (var c in clienti)
                {
                    sw.WriteLine($"{c.Nume};{c.Prenume};{c.CNP}");
                }
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
                Console.Write("\nSelectati o optiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        Masina masinaNoua = new Masina();

                        Console.Write("Marca: ");
                        masinaNoua.Marca = Console.ReadLine();

                        Console.Write("Model: ");
                        masinaNoua.Model = Console.ReadLine();

                        Console.Write("Numar inmatriculare: ");
                        masinaNoua.NumarInmatriculare = Console.ReadLine();

                        masinaNoua.EsteDisponibila = true;

                        Console.WriteLine("Culoare: 0-Rosu, 1-Alb, 2-Negru");
                        masinaNoua.CuloareMasina = (Culoare)int.Parse(Console.ReadLine());

                        Console.WriteLine("Optiuni (suma): 1-AerConditionat, 2-Navigatie, 4-CutieAutomata");
                        masinaNoua.OptiuniMasina = (Optiuni)int.Parse(Console.ReadLine());

                        listaMasini.Add(masinaNoua);
                        SalveazaMasina(masinaNoua);

                        Console.WriteLine("Masina adaugata!");
                        break;

                    case "2":
                        Console.WriteLine("\n--- Lista Masini ---");

                        if (listaMasini.Count == 0)
                            Console.WriteLine("Nu exista masini.");

                        foreach (var masina in listaMasini)
                        {
                            Console.WriteLine($"- {masina.Marca} {masina.Model} [{masina.NumarInmatriculare}] | {masina.CuloareMasina} | {masina.OptiuniMasina}");
                        }
                        break;

                    case "3":
                        Console.Write("Marca cautata: ");
                        string cautare = Console.ReadLine();

                        var rezultate = listaMasini
                            .Where(m => m.Marca.Equals(cautare, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (rezultate.Count > 0)
                        {
                            foreach (var masina in rezultate)
                            {
                                Console.WriteLine($"{masina.Marca} {masina.Model}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu exista masini.");
                        }
                        break;

                    case "4":
                        Client clientNou = new Client();

                        Console.Write("Nume: ");
                        clientNou.Nume = Console.ReadLine();

                        Console.Write("Prenume: ");
                        clientNou.Prenume = Console.ReadLine();

                        Console.Write("CNP: ");
                        clientNou.CNP = Console.ReadLine();

                        listaClienti.Add(clientNou);
                        SalveazaClient(clientNou);

                        Console.WriteLine("Client adaugat!");
                        break;

                    case "5":
                        rulare = false;
                        break;

                    case "6":
                        Console.Write("Numar inmatriculare: ");
                        string nr = Console.ReadLine();

                        bool modificat = false;

                        foreach (var m in listaMasini)
                        {
                            if (m.NumarInmatriculare == nr)
                            {
                                Console.Write("Marca noua: ");
                                m.Marca = Console.ReadLine();

                                Console.Write("Model nou: ");
                                m.Model = Console.ReadLine();

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
                        Console.Write("CNP: ");
                        string cnp = Console.ReadLine();

                        bool gasit = false;

                        foreach (var c in listaClienti)
                        {
                            if (c.CNP == cnp)
                            {
                                Console.Write("Nume nou: ");
                                c.Nume = Console.ReadLine();

                                Console.Write("Prenume nou: ");
                                c.Prenume = Console.ReadLine();

                                gasit = true;
                                break;
                            }
                        }

                        if (gasit)
                        {
                            SuprascrieClienti(listaClienti);
                            Console.WriteLine("Client modificat!");
                        }
                        else
                        {
                            Console.WriteLine("Client negasit!");
                        }
                        break;

                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }

                if (rulare)
                {
                    Console.WriteLine("\nApasa ENTER...");
                    Console.ReadKey();
                }
            }
        }
    }
}
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
}
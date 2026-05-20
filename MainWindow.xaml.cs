using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InchirieriMasiniWPF
{

    public enum Culoare { Rosu, Alb, Negru }

    [Flags]
    public enum Optiuni
    {
        Nimic = 0,
        AerConditionat = 1,
        Navigatie = 2,
        CutieAutomata = 4
    }


    public class ClientViewModel
    {
        public string CNP { get; set; } = string.Empty;
        public string Nume { get; set; } = string.Empty;
        public string Prenume { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string PermisConducere { get; set; } = string.Empty;

        public string NumeComplet => $"{Nume} {Prenume}";
    }


    public class MasinaViewModel
    {
        public string Marca { get; set; }
        public string Model { get; set; }
        public string NumarInmatriculare { get; set; }
        public bool EsteDisponibila { get; set; }
        public Culoare CuloareMasina { get; set; }
        public Optiuni OptiuniMasina { get; set; }

        
        public string DisponibilText => EsteDisponibila ? "✅ Da" : "❌ Nu";
        public string DisponibilCuloare => EsteDisponibila ? "#16A34A" : "#DC2626";
    }


    public partial class MainWindow : Window
    {
        private const string FisierMasini = "masini.txt";
        private const string FisierClienti = "clienti.txt";
        private ObservableCollection<MasinaViewModel> _toateMasinile = new();
        private ObservableCollection<ClientViewModel> _totiClientii = new();

        public MainWindow()
        {
            InitializeComponent();
            IncarcaMasini();
            IncarcaClienti();
            GridMasini.ItemsSource = _toateMasinile;
            GridClienti.ItemsSource = _totiClientii;
        }

        
        private void IncarcaMasini()
        {
            _toateMasinile.Clear();
            if (!File.Exists(FisierMasini)) return;

            foreach (var linie in File.ReadAllLines(FisierMasini))
            {
                var d = linie.Split(';');
                if (d.Length < 6) continue;

                _toateMasinile.Add(new MasinaViewModel
                {
                    Marca = d[0],
                    Model = d[1],
                    NumarInmatriculare = d[2],
                    EsteDisponibila = bool.Parse(d[3]),
                    CuloareMasina = (Culoare)Enum.Parse(typeof(Culoare), d[4]),
                    OptiuniMasina = (Optiuni)Enum.Parse(typeof(Optiuni), d[5])
                });
            }
            ActualizeazaStatus();
        }

        
        private void SalveazaToate()
        {
            using var sw = new StreamWriter(FisierMasini, false);
            foreach (var m in _toateMasinile)
                sw.WriteLine($"{m.Marca};{m.Model};{m.NumarInmatriculare};{m.EsteDisponibila};{m.CuloareMasina};{m.OptiuniMasina}");
        }

        
        private void IncarcaClienti()
        {
            _totiClientii.Clear();
            if (!File.Exists(FisierClienti)) return;

            foreach (var linie in File.ReadAllLines(FisierClienti))
            {
                var d = linie.Split(';');
                if (d.Length < 5) continue;

                _totiClientii.Add(new ClientViewModel
                {
                    CNP = d[0],
                    Nume = d[1],
                    Prenume = d[2],
                    Telefon = d[3],
                    PermisConducere = d[4]
                });
            }
        }

        
        private void SalveazaClienti()
        {
            using var sw = new StreamWriter(FisierClienti, false);
            foreach (var c in _totiClientii)
                sw.WriteLine($"{c.CNP};{c.Nume};{c.Prenume};{c.Telefon};{c.PermisConducere}");
        }

        
        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtMarca.Text) ||
                string.IsNullOrWhiteSpace(TxtModel.Text) ||
                string.IsNullOrWhiteSpace(TxtNr.Text) ||
                CmbCuloare.SelectedIndex == -1)
            {
                TxtStatus.Text = "⚠️ Completați toate câmpurile obligatorii!";
                return;
            }

            
            Optiuni opt = Optiuni.Nimic;
            if (ChkAer.IsChecked == true) opt |= Optiuni.AerConditionat;
            if (ChkNav.IsChecked == true) opt |= Optiuni.Navigatie;
            if (ChkCutie.IsChecked == true) opt |= Optiuni.CutieAutomata;

            var masina = new MasinaViewModel
            {
                Marca = TxtMarca.Text.Trim(),
                Model = TxtModel.Text.Trim(),
                NumarInmatriculare = TxtNr.Text.Trim().ToUpper(),
                EsteDisponibila = true,
                CuloareMasina = (Culoare)CmbCuloare.SelectedIndex,
                OptiuniMasina = opt
            };

            _toateMasinile.Add(masina);
            SalveazaToate();

            
            TxtMarca.Clear(); TxtModel.Clear(); TxtNr.Clear();
            CmbCuloare.SelectedIndex = -1;
            ChkAer.IsChecked = ChkNav.IsChecked = ChkCutie.IsChecked = false;

            TxtStatus.Text = $"✅ Mașina {masina.Marca} {masina.Model} a fost adăugată.";
            ActualizeazaStatus();
        }

        
        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string nr)
            {
                var masina = _toateMasinile.FirstOrDefault(m => m.NumarInmatriculare == nr);
                if (masina != null)
                {
                    _toateMasinile.Remove(masina);
                    SalveazaToate();
                    TxtStatus.Text = $"🗑️ Mașina [{nr}] a fost ștearsă.";
                    ActualizeazaStatus();
                }
            }
        }

        
        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            FiltreazaDupa(TxtCautare.Text);
        }

        
        private void TxtCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltreazaDupa(TxtCautare.Text);
        }

        private void FiltreazaDupa(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                GridMasini.ItemsSource = _toateMasinile;
                TxtStatus.Text = $"Total mașini: {_toateMasinile.Count}";
                return;
            }

            var filtrate = _toateMasinile
                .Where(m => m.Marca.Contains(text, StringComparison.OrdinalIgnoreCase))
                .ToList();

            GridMasini.ItemsSource = filtrate;
            TxtStatus.Text = $"🔍 {filtrate.Count} mașini găsite pentru \"{text}\"";
        }

        private void ActualizeazaStatus()
        {
            TxtStatus.Text = $"Total mașini: {_toateMasinile.Count} | " +
                             $"Disponibile: {_toateMasinile.Count(m => m.EsteDisponibila)} | " +
                             $"Clienți: {_totiClientii.Count}";
        }

        
        private void BtnAdaugaClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCnp.Text) ||
                string.IsNullOrWhiteSpace(TxtNumeClient.Text) ||
                string.IsNullOrWhiteSpace(TxtPrenumeClient.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefon.Text) ||
                string.IsNullOrWhiteSpace(TxtPermis.Text))
            {
                TxtStatus.Text = "⚠️ Completați toate câmpurile obligatorii pentru client!";
                return;
            }

            string cnp = TxtCnp.Text.Trim();
            if (_totiClientii.Any(c => c.CNP == cnp))
            {
                TxtStatus.Text = "⚠️ Un client cu acest CNP există deja!";
                return;
            }

            var client = new ClientViewModel
            {
                CNP = cnp,
                Nume = TxtNumeClient.Text.Trim(),
                Prenume = TxtPrenumeClient.Text.Trim(),
                Telefon = TxtTelefon.Text.Trim(),
                PermisConducere = TxtPermis.Text.Trim().ToUpper()
            };

            _totiClientii.Add(client);
            SalveazaClienti();
            ReseteazaFormularClient();

            TxtStatus.Text = $"✅ Clientul {client.NumeComplet} a fost adăugat.";
            ActualizeazaStatus();
        }

        
        private void BtnModificaClient_Click(object sender, RoutedEventArgs e)
        {
            if (GridClienti.SelectedItem is not ClientViewModel clientSelectat)
            {
                TxtStatus.Text = "⚠️ Selectați un client din tabel pentru a-l modifica!";
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtNumeClient.Text) ||
                string.IsNullOrWhiteSpace(TxtPrenumeClient.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefon.Text) ||
                string.IsNullOrWhiteSpace(TxtPermis.Text))
            {
                TxtStatus.Text = "⚠️ Completați toate câmpurile obligatorii pentru client!";
                return;
            }

            clientSelectat.Nume = TxtNumeClient.Text.Trim();
            clientSelectat.Prenume = TxtPrenumeClient.Text.Trim();
            clientSelectat.Telefon = TxtTelefon.Text.Trim();
            clientSelectat.PermisConducere = TxtPermis.Text.Trim().ToUpper();

            SalveazaClienti();
            GridClienti.Items.Refresh();
            ReseteazaFormularClient();

            TxtStatus.Text = $"✅ Clientul {clientSelectat.NumeComplet} a fost modificat.";
        }

       
        private void BtnStergeClient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string cnp)
            {
                var client = _totiClientii.FirstOrDefault(c => c.CNP == cnp);
                if (client != null)
                {
                    var result = MessageBox.Show($"Sigur doriți să ștergeți clientul {client.NumeComplet}?", "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        _totiClientii.Remove(client);
                        SalveazaClienti();
                        ReseteazaFormularClient();
                        TxtStatus.Text = $"🗑️ Clientul cu CNP [{cnp}] a fost șters.";
                        ActualizeazaStatus();
                    }
                }
            }
        }

        
        private void GridClienti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridClienti.SelectedItem is ClientViewModel client)
            {
                TxtCnp.Text = client.CNP;
                TxtCnp.IsEnabled = false; 
                TxtNumeClient.Text = client.Nume;
                TxtPrenumeClient.Text = client.Prenume;
                TxtTelefon.Text = client.Telefon;
                TxtPermis.Text = client.PermisConducere;

                BtnModificaClient.IsEnabled = true;
                BtnAdaugaClient.IsEnabled = false;
            }
        }

        
        private void BtnReseteazaClient_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaFormularClient();
        }

        private void ReseteazaFormularClient()
        {
            TxtCnp.Clear();
            TxtCnp.IsEnabled = true;
            TxtNumeClient.Clear();
            TxtPrenumeClient.Clear();
            TxtTelefon.Clear();
            TxtPermis.Clear();
            GridClienti.SelectedItem = null;

            BtnModificaClient.IsEnabled = false;
            BtnAdaugaClient.IsEnabled = true;
        }

        
        private void TxtCautareClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltreazaClientiDupa(TxtCautareClient.Text);
        }

        private void FiltreazaClientiDupa(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                GridClienti.ItemsSource = _totiClientii;
                return;
            }

            var filtrate = _totiClientii
                .Where(c => c.Nume.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                            c.Prenume.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                            c.CNP.Contains(text, StringComparison.OrdinalIgnoreCase))
                .ToList();

            GridClienti.ItemsSource = filtrate;
        }

        
        private void BtnInchiriereNoua_Click(object sender, RoutedEventArgs e)
        {
            var rentWin = new RentWindow(_toateMasinile, _totiClientii);
            if (rentWin.ShowDialog() == true)
            {
                SalveazaToate();
                IncarcaMasini();
                ActualizeazaStatus();
                TxtStatus.Text = "🚀 Închirierea a fost realizată cu succes!";
            }
        }
    }
}
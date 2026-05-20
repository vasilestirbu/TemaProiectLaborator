using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace InchirieriMasiniWPF
{
    public partial class RentWindow : Window
    {
        private const string FisierInchirieri = "inchirieri.txt";
        private ObservableCollection<MasinaViewModel> _masini;
        private ObservableCollection<ClientViewModel> _clienti;

        public RentWindow(ObservableCollection<MasinaViewModel> masini, ObservableCollection<ClientViewModel> clienti)
        {
            InitializeComponent();
            _masini = masini;
            _clienti = clienti;

            // Populam combobox-ul de clienti
            CmbClienti.ItemsSource = _clienti;

            // Populam combobox-ul de masini doar cu cele care sunt disponibile
            var masiniDisponibile = _masini.Where(m => m.EsteDisponibila).ToList();
            CmbMasini.ItemsSource = masiniDisponibile;

            if (masiniDisponibile.Count == 0)
            {
                MessageBox.Show("Nu există mașini disponibile în acest moment!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            if (CmbClienti.SelectedItem is not ClientViewModel clientSelectat)
            {
                MessageBox.Show("Vă rugăm să selectați un client!", "Eroare validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbMasini.SelectedItem is not MasinaViewModel masinaSelectata)
            {
                MessageBox.Show("Vă rugăm să selectați o mașină!", "Eroare validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int zile = (int)SliderZile.Value;
            double cost = zile * 100; // 100 RON pe zi

            // 1. Modificam disponibilitatea masinii
            masinaSelectata.EsteDisponibila = false;

            // 2. Salvare contract in fisierul text
            try
            {
                using (var sw = new StreamWriter(FisierInchirieri, true))
                {
                    sw.WriteLine($"{clientSelectat.CNP};{masinaSelectata.NumarInmatriculare};{DateTime.Now:yyyy-MM-dd HH:mm:ss};{zile};{cost}");
                }

                MessageBox.Show($"Închirierea a fost înregistrată cu succes!\n\nClient: {clientSelectat.NumeComplet}\nMașină: {masinaSelectata.Marca} {masinaSelectata.Model} ({masinaSelectata.NumarInmatriculare})\nDurată: {zile} zile\nCost Total: {cost} RON", 
                                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea închirierii: {ex.Message}", "Eroare sistem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

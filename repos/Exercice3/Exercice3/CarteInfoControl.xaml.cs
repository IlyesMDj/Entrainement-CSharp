using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Exercice3
{
    /// <summary>
    /// Logique d'interaction pour CarteInfoControl.xaml
    /// </summary>
    public partial class CarteInfoControl : UserControl
    {
        public CarteInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitreProperty = DependencyProperty.Register("Titre", typeof(string), typeof(CarteInfoControl));

        public string Titre
        {
            get { return (string)GetValue(TitreProperty); }
            set { SetValue(TitreProperty, value); }
        }

        public static readonly DependencyProperty ValeurProperty =
            DependencyProperty.Register("Valeur", typeof(string), typeof(CarteInfoControl));

        public string Valeur
        {
            get { return (string)GetValue(ValeurProperty); }
            set { SetValue(ValeurProperty, value); }
        }
    }
}

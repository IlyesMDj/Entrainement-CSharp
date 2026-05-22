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
    /// Logique d'interaction pour ChampSaisie.xaml
    /// </summary>
    public partial class ChampSaisie : UserControl
    {
        public ChampSaisie()
        {
            InitializeComponent();
        }

        public string Titre
        {
            get { return (string)GetValue(TitreProperty); }
            set { SetValue(TitreProperty, value); }
        }

        public static readonly DependencyProperty TitreProperty =
            DependencyProperty.Register("Titre", typeof(string), typeof(ChampSaisie), new PropertyMetadata(string.Empty));

        public string TexteSaisi
        {
            get { return (string)GetValue(TexteSaisiProperty); }
            set { SetValue(TexteSaisiProperty, value); }
        }
        public static readonly DependencyProperty TexteSaisiProperty =
            DependencyProperty.Register("TexteSaisi", typeof(string), typeof(ChampSaisie), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}

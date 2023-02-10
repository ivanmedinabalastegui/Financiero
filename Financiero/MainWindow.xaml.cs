using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace Financiero
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cbBanco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Hace que al seleccionar los combobox, se introduzca un número dependiendo de qué banco se elija
            ComboBox cb = (ComboBox)sender;
            ComboBoxItem cbi = (ComboBoxItem)cb.SelectedItem;
            string entidad = cbi.Content.ToString();
            if (entidad == "Santander")
            {
                tfCorriente1.Text = "0049";
            }
            else if (entidad == "BBVA")
            {
                tfCorriente1.Text = "0182";
            }
            else if (entidad == "La Caixa")
            {
                tfCorriente1.Text = "2100";
            }
            else if (entidad == "Bankia")
            {
                tfCorriente1.Text = "2038";
            }
            else if (entidad == "Sabadell")
            {
                tfCorriente1.Text = "0081";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Se inicializan valores
            int number = int.Parse(tfCorriente1.Text);
            int number2 = int.Parse(tfCorriente2.Text);
            long number3 = long.Parse(tfCorriente4.Text);
            int primerDC = 0;
            int segundoDC = 0;

            //Para conseguir el primer y el segundo dígito de control
            if(rbVisa.IsChecked == false && rbMastercard.IsChecked == false && rbAmericanExpress.IsChecked == false)
            {
                MessageBox.Show("Debes seleccionar un tipo de tarjeta");

            } else if(Regex.IsMatch(number2.ToString(), @"^\d{4}$"))
            {
                if (number3.ToString().Length < 11)
                {
                    //Se coge cada dígito del número de la entidad y se multiplica por el número que indica
                    int primerDigitoE = (number / 1000) * 4;
                    int segundoDigitoE = ((number % 1000) / 100) * 8;
                    int tercerDigitoE = ((number % 100) / 10) * 5;
                    int cuartoDigitoE = (number % 10) * 10;

                    //Se coge cada dígito del número de la sucursal y se multiplica por el número que indica
                    int primerDigitoS = (number2 / 1000) * 9;
                    int segundoDigitoS = ((number2 % 1000) / 100) * 7;
                    int tercerDigitoS = ((number2 % 100) / 10) * 3;
                    int cuartoDigitoS = (number2 % 10) * 6;

                    //Se suman todos los dígitos
                    int sumaTotal = primerDigitoE + primerDigitoS + segundoDigitoE + segundoDigitoS +
                        tercerDigitoE + tercerDigitoS + cuartoDigitoE + cuartoDigitoS;

                    //Se coge el resto de dividirlo entre 11
                    int resto = sumaTotal % 11;

                    //A 11 se le resta el resto y conseguimos el primer dígito de control
                    primerDC = 11 - resto;

                    //Si el dígito es 10, se cambia por 1, sino se queda con el mismo valor
                    if (primerDC == 10)
                    {
                        primerDC = 1;
                    }


                    string num = "";
                    //Comprueba si tiene 10 dígitos
                    if (Regex.IsMatch(number3.ToString(), @"^\d{10}$"))
                    {
                        num = number3.ToString();
                    }
                    //Si no es así, se rellena con ceros hasta tener 10
                    else
                    {
                        num = number3.ToString().PadLeft(10, '0');
                        tfCorriente4.Text = num;
                    }

                    //Se coge cada dígito del número de cuenta y se multiplica por el número que indica
                    int primerDigitoC = num[0] * 1;
                    int segundoDigitoC = num[1] * 2;
                    int tercerDigitoC = num[2] * 4;
                    int cuartoDigitoC = num[3] * 8;
                    int quintoDigitoC = num[4] * 5;
                    int sextoDigitoC = num[5] * 10;
                    int septimoDigitoC = num[6] * 9;
                    int octavoDigitoC = num[7] * 7;
                    int noveoDigitoC = num[8] * 3;
                    int decimoDigitoC = num[9] * 6;

                    //Se hace una suma total
                    int resultado = primerDigitoC + segundoDigitoC + tercerDigitoC +
                        cuartoDigitoC + quintoDigitoC + sextoDigitoC + septimoDigitoC +
                        octavoDigitoC + noveoDigitoC + decimoDigitoC;

                    //Se coge el resto de dividirlo entre 11
                    int resto2 = resultado % 11;

                    //Se consigue el segundo dígito
                    segundoDC = 11 - resto2;

                    //Si el dígito es 10, se cambia por 1, sino se queda con el mismo valor
                    if (segundoDC == 10)
                    {
                        segundoDC = 1;
                    }

                    //Se juntan los dos dígitos
                    string total1 = primerDC.ToString() + segundoDC.ToString();

                    //Se pone en su celda correspondiente
                    tfCorriente3.Text = total1;

                    //Se suman todos los campos para formar el número de cuenta
                    string cuenta = tfCorriente1.Text.ToString() + tfCorriente2.Text.ToString() +
                        tfCorriente3.Text.ToString() + tfCorriente4.Text.ToString();

                    //Se hacen las cuentas pertinentes
                    string IBANNormal1 = cuenta +"142800";

                    BigInteger restoIBAN = BigInteger.Parse(IBANNormal1) % 97;

                    BigInteger IBANNormal2 = 98 - restoIBAN;

                    string IBANNormal3 = IBANNormal2.ToString();

                    //Si los dos números del ES resulta ser 1, se le añade un 0 a la izquierda
                    if (IBANNormal2.ToString().Length == 1)
                    {
                        IBANNormal3 = 0 + IBANNormal2.ToString();
                    }

                    string IBANFinal = "ES" + IBANNormal3;

                    //Se insertan los datos en el campo de texto
                    tfIBAN1.Text = IBANFinal;
                    tfIBAN2.Text = cuenta;

                    string cuentaTarjeta = "";

                    if (rbMastercard.IsChecked == true)
                    {
                        Random random = new Random();
                        BigInteger randomNumber1 = random.Next(10000, 99999);
                        BigInteger randomNumber2 = random.Next(10000, 99999);
                        BigInteger randomNumber3 = random.Next(1000, 9999);
                        string randomTotal = randomNumber1.ToString() + randomNumber2.ToString() + randomNumber3.ToString();
                        cuentaTarjeta = "51" + randomTotal;
                        tfTarjetaPago.Text = cuentaTarjeta;
                    }
                    else if (rbVisa.IsChecked == true)
                    {
                        Random random = new Random();
                        BigInteger randomNumber1 = random.Next(10000, 99999);
                        BigInteger randomNumber2 = random.Next(10000, 99999);
                        BigInteger randomNumber3 = random.Next(10000, 99999);
                        string randomTotal = randomNumber1.ToString() + randomNumber2.ToString() + randomNumber3.ToString();
                        cuentaTarjeta = "4" + randomTotal;
                        tfTarjetaPago.Text = cuentaTarjeta;
                    }
                    else
                    {
                        Random random = new Random();
                        BigInteger randomNumber1 = random.Next(10000, 99999);
                        BigInteger randomNumber2 = random.Next(10000, 99999);
                        BigInteger randomNumber3 = random.Next(1000, 9999);
                        string randomTotal = randomNumber1.ToString() + randomNumber2.ToString() + randomNumber3.ToString();
                        cuentaTarjeta = "3" + randomTotal;
                        tfTarjetaPago.Text = cuentaTarjeta;
                    }

                }
                else
                {
                    //Muestra un mensaje si el número supera los 10 dígitos o no se ha introducido ningun campo
                    MessageBox.Show("El número de cuenta no puede estar vacío y debe de tener al menos 10 dígitos");
                }
            }
            else
            {
                //Muestra un mensaje si el número supera los 4 dígitos
                MessageBox.Show("El número de sucursal tiene que tener 4 dígitos");
            }
        }



        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Para cambiar la imagen de la tarjeta
            if (rbMastercard.IsChecked == true)
            {
                imgTarjeta1.Visibility = Visibility.Visible;
                imgTarjeta2.Visibility = Visibility.Hidden;
                imgTarjeta3.Visibility = Visibility.Hidden;
            }
            else if (rbVisa.IsChecked == true)
            {
                imgTarjeta1.Visibility = Visibility.Hidden;
                imgTarjeta2.Visibility = Visibility.Visible;
                imgTarjeta3.Visibility = Visibility.Hidden;
            } else
            {
                imgTarjeta1.Visibility = Visibility.Hidden;
                imgTarjeta2.Visibility = Visibility.Hidden;
                imgTarjeta3.Visibility = Visibility.Visible;
            }

        }
    }
}

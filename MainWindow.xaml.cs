using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Collections.ObjectModel;

namespace Administracija_Korisnika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            TextBoxId.Clear();
            TextBoxUserName.Clear();
            TextBoxPassword.Clear();
            ComboBoxAdmin.SelectedIndex = -1;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxUsers.ItemsSource = UserDAL.FetchUsers();
            //ComboBoxAdmin.ItemsSource = UserDAL.FetchUsers();
        }

        private void ListBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxUsers.SelectedIndex > -1)
            {
                User us = ListBoxUsers.SelectedItem as User;
                Clear();
                TextBoxId.Text = us.UserId.ToString();
                TextBoxUserName.Text = us.UserName;
                TextBoxPassword.Text = us.UserPassword;
                ComboBoxAdmin.SelectedValue = us.IsAdmin.ToString();
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                MessageBox.Show("Username is mandatory for new users");
                TextBoxUserName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxPassword.Text))
            {
                MessageBox.Show("Password is mandatory for new users");
                TextBoxPassword.Focus();
                return false;
            }
            if (ComboBoxAdmin.SelectedIndex < 0)
            {
                MessageBox.Show("Admin status is mandatory for new users");
                ComboBoxAdmin.Focus();
                return false;
            }
            return true;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                User u = new User { UserName = TextBoxUserName.Text, UserPassword = TextBoxPassword.Text, IsAdmin = Convert.ToInt32(ComboBoxAdmin.SelectedValue.ToString())};
                int id = UserDAL.AddUser(u);

                if (id > 0)
                {
                    MessageBox.Show("User is added successfully");
                    Clear();
                    ListBoxUsers.ItemsSource = UserDAL.FetchUsers();
                    ListBoxUsers.SelectedIndex = ListBoxUsers.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show("Error occured");
                }
            }
        }
    }
}

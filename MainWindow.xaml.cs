using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

namespace LoginSystemEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Users
        {
            public int ID { get; set; }
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
            [Column(TypeName = "datetime")]
            public DateTime CreationTime { get; set; }

        }
        public class AppDbContext : DbContext
        {
            public DbSet<Users> User { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                options.UseSqlServer(@"Server=(local)\SQLEXPRESS;Database=LoginApp;Integrated Security=True");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            string nameInput = nameInputBox.Text;
            string password = passInputBox.Password;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var database = new AppDbContext())
            {
                var user = database.User.Where(u => u.UserName == nameInputBox.Text && u.Password == passInputBox.Password);

                if (user.Count() > 0)
                {
                    MessageBox.Show("Logged in!");
                }
                else
                {
                    MessageBox.Show("Wrong Username or Password");
                }
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var database = new AppDbContext())
            {
                var checkIfExist = database.User.Where(u => u.UserName.Contains(nameInputBox.Text));
                if (nameInputBox.Text.Count() != 0 && passInputBox.Password.Count() != 0)
                {
                    if (checkIfExist.Any() == false)
                    {
                        Users newUser = new Users()
                        {
                            UserName = nameInputBox.Text,
                            Password = passInputBox.Password,
                            CreationTime = DateTime.Now
                        };
                        database.User.Add(newUser);
                        database.SaveChanges();
                        MessageBox.Show("Created new account");
                    }
                    else
                    {
                        MessageBox.Show("A user with that name already exists!");
                    }
                }
                else
                {
                    MessageBox.Show("Requires both username and password");
                }
            }
        }
    }
}

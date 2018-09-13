using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.IO;


namespace LogIn
{

    public partial class Form1 : Form
    {
        string SQL;
        OdbcConnection cn = new OdbcConnection();
        OdbcCommand cmd = new OdbcCommand();
        OdbcDataReader reader;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LogIn();
        }

        private void LogIn()
        {

            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }
            else
            {
                try
                {
                    string username = txtUsername.Text;
                    string password = txtPassword.Text;
                    SQL = "SELECT salt,saltedpassword FROM mylogin.data where username =? ;";
                    cmd.CommandText = SQL;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@username", username);
                    reader = cmd.ExecuteReader();
                    if (reader.RecordsAffected == 0)
                    {
                        MessageBox.Show("Wrong Username");
                        return;
                    }
                    reader.Read();
                    string salt = reader["salt"].ToString();
                    string dbPass = reader["saltedpassword"].ToString();
                    string HashPassword = MyCrypto.CalcSaltedPass(salt, password);


                    if (HashPassword == dbPass)
                    {
                        MessageBox.Show("You are now logged in");
                        System.Diagnostics.Process p = System.Diagnostics.Process.Start("calc.exe");//You can call other forms , window programs etc.
                        
                    }
                    else
                        MessageBox.Show("Wrong Password");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "oops!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                finally
                {
                    try
                    {
                        reader.Close();
                    }

                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.Message, "oops!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Form f = new frmCreate();
            f.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                cn.ConnectionString = "Driver={MySQL ODBC 5.3 UNICODE Driver};Server=localhost;Database=mylogin;" +
               "User=root;Password=;Option=3;";
                cn.Open();
                cmd.Connection = cn;
            }
            catch (Exception)
            {
                MessageBox.Show("Check if MySQL(xampp) is running");
            }

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }



    }
}












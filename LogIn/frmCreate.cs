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
    public partial class frmCreate : Form
    {
        string SQL;
        OdbcConnection cn = new OdbcConnection();
        OdbcDataAdapter da;
        OdbcCommand cmd = new OdbcCommand();
        DataTable dt = new DataTable();
        
        public frmCreate()
        {
            InitializeComponent();
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {
           
            CreateUser();
        }
        
        private void SetPassword(string user, string userPassword)
        { 
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string salt = MyCrypto.getSalt();
            string HashPassword = MyCrypto.CalcSaltedPass(salt, password);
            SQL = "UPDATE DATA SET saltedpassword=" + HashPassword + "WHERE username = " + username + "; ";
            if (IsValidEmail(email) == true)
            {
                SQL = "INSERT INTO mylogin.data(username,salt,saltedpassword,email) VALUES('" + username + "','" + salt + "','" + HashPassword + "','" + email + "');";
            }
            else
                MessageBox.Show("Invalid email address");
            
        }

        private void CreateUser()
        {
            SetPassword(txtUsername.Text, txtPassword.Text);
            try
            {
                if (txtUsername.Text != "" && txtPassword.Text != "" && txtEmail.Text!="")
                {
                    da = new OdbcDataAdapter();
                    da.SelectCommand = new OdbcCommand(SQL,cn);
                    OdbcCommandBuilder builder = new OdbcCommandBuilder(da);
                    da.Fill(dt);
                    MessageBox.Show("User Created");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or Password or Email field is empty");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "oops!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                this.Close();
            }

        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void frmCreate_Load(object sender, EventArgs e)
        {
            try
            {
                cn.ConnectionString = "Driver={MySQL ODBC 5.3 UNICODE Driver};Server=localhost;Database=mylogin;" +
               "User=root;Password=;Option=3;";
                cn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                this.Close();
            }

        }

    }
}

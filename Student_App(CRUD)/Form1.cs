using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Student_App_CRUD_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label1.Text = "Student Id";
            label2.Text = "Student Name";
            label3.Text = "Student Age";
            label4.Text = "Student Course";


            CreateTable();
            LoadData();
        }

        private void CreateTable()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string query = "CREATE TABLE IF NOT EXISTS Students (Id TEXT, Name TEXT, Age TEXT, Course TEXT)";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string query = "SELECT * FROM Students";

                SQLiteDataAdapter da = new SQLiteDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string query = "SELECT * FROM Students";

                SQLiteDataAdapter da = new SQLiteDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            if (string.IsNullOrWhiteSpace(txtId.Text) ||
               string.IsNullOrWhiteSpace(txtName.Text) ||
               string.IsNullOrWhiteSpace(txtAge.Text) ||
               string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM Students WHERE Id = @Id";
                SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", txtId.Text);

                long count = (long)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Student ID already exists!");
                    return;
                }

                string query = "INSERT INTO Students (Id, Name, Age, Course) VALUES (@Id, @Name, @Age, @Course)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Course", txtCourse.Text);

                cmd.ExecuteNonQuery();
            }

            LoadData();

            txtId.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();

            MessageBox.Show("Data Saved!");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string query = "UPDATE Students SET Name=@Name, Age=@Age, Course=@Course WHERE Id=@Id";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Course", txtCourse.Text);

                cmd.ExecuteNonQuery();
            }

            LoadData();

            txtId.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();

            MessageBox.Show("Data Updated!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Enter Student ID to delete!");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString; 

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM Students WHERE Id=@Id";
                SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", txtId.Text);

                long count = (long)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Invalid Student ID! No record found.");
                    return;
                }

                string query = "DELETE FROM Students WHERE Id=@Id";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);

                cmd.ExecuteNonQuery();
            }

            LoadData();

            txtId.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();

            MessageBox.Show("Data Deleted!");
        }
    }
}

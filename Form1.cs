using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP1_BDAvancee.controller;
using TP1_BDAvancee.db;
using TP1_BDAvancee.model;
using TP1_BDAvancee.view;

namespace TP1_BDAvancee
{
    public partial class Form1 : Form
    {
        private ArticleManager articleManager;
        private DataSet ds;
        private DataTable table;
        int id = 1;
        int countArticles;
        public Form1()
        {
            InitializeComponent();
            DatabaseConnection.Connexion();
            articleManager = new ArticleManager();
            countArticles = articleManager.getAllArticles().Count;
            fillDataTableOnlyValids();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            NewArticleWindow newArticleWindow = new NewArticleWindow();

            if (newArticleWindow.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string code = newArticleWindow.textBoxCode.Text;
                    string name = newArticleWindow.textBoxName.Text;
                    string description = newArticleWindow.textBoxDescription.Text;
                    string brand = newArticleWindow.textBoxBrand.Text;
                    string category = newArticleWindow.textBoxCategory.Text;
                    decimal price = decimal.Parse(newArticleWindow.textBoxPrice.Text);
                    string urlImage = newArticleWindow.textBoxURL.Text;
                    Boolean invalid = true;

                    Article article = new Article(code, name, description, brand, category, price, urlImage, invalid);

                    articleManager.addArticle(article);

                    fillDataTableOnlyValids();
                    getArticlesCount();

                    MessageBox.Show("Article added successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void fillDataTable()
        {
            ds = new DataSet();
            table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Code");
            table.Columns.Add("Name");
            table.Columns.Add("Description");
            table.Columns.Add("Brand");
            table.Columns.Add("Category");
            table.Columns.Add("Price");
            table.Columns.Add("URL Image");
            table.Columns.Add("Valid");
            List<Article> articles = articleManager.getAllArticles();
            foreach (Article article in articles)
            {
                table.Rows.Add(article.id, article.code, article.name, article.description, article.brand, article.category, article.price, article.imageURL, article.valid);
            }
            ds.Tables.Add(table);
            dataGridView1.DataSource = ds.Tables[0];
        }

        public void fillDataTableOnlyValids()
        {
            ds = new DataSet();
            table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Code");
            table.Columns.Add("Name");
            table.Columns.Add("Description");
            table.Columns.Add("Brand");
            table.Columns.Add("Category");
            table.Columns.Add("Price");
            table.Columns.Add("URL Image");
            table.Columns.Add("Valid");
            List<Article> articles = articleManager.getAllValidArticles();
            foreach (Article article in articles)
            {
                table.Rows.Add(article.id, article.code, article.name, article.description, article.brand, article.category, article.price, article.imageURL, article.valid);
            }
            ds.Tables.Add(table);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private async void LoadImageFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                    HttpResponseMessage response = await client.GetAsync(imageUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        if (stream == null || stream.Length == 0)
                        {
                            return;
                        }

                        using (Image image = Image.FromStream(stream))
                        {
                            pictureBox1.Image = new Bitmap(image);
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        private async void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Article article = articleManager.getArticleById(id);

            if (article != null)
            {
                await Task.Run(() => LoadImageFromUrl(article.imageURL));
                labelId.Text = article.id.ToString();
                labelCode.Text = article.code;
                labelName.Text = article.name;
                labelDescription.Text = article.description;
                labelBrand.Text = article.brand;
                labelCategory.Text = article.category;
                labelPrice.Text = article.price.ToString();
            }
        }

        private void nextArticle()
        {
            id++;
            Article article = articleManager.getArticleById(id);
            while (article.id == 0 && id <= countArticles)
            {
                id++;
                article = articleManager.getArticleById(id);
            }
            if (article.id != 0)
            {
                LoadImageFromUrl(article.imageURL);
                labelId.Text = article.id.ToString();
                labelCode.Text = article.code;
                labelName.Text = article.name;
                labelDescription.Text = article.description;
                labelBrand.Text = article.brand;
                labelCategory.Text = article.category;
                labelPrice.Text = article.price.ToString();
            }
            else
            {
                MessageBox.Show("No more articles to show.");
            }
        }

        private void previousArticle()
        {
            id--;
            if (id < 1)
            {
                MessageBox.Show("No more articles to show.");
                id = 1;
                return;
            }
            Article article = articleManager.getArticleById(id);
            while (article.id == 0 && id > 0)
            {
                id--;
                article = articleManager.getArticleById(id);
            }
            if (article.id != 0)
            {
                LoadImageFromUrl(article.imageURL);
                labelId.Text = article.id.ToString();
                labelCode.Text = article.code;
                labelName.Text = article.name;
                labelDescription.Text = article.description;
                labelBrand.Text = article.brand;
                labelCategory.Text = article.category;
                labelPrice.Text = article.price.ToString();
            }
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            nextArticle();
            if (id <= articleManager.getArticlesCount())
            {
                buttonForward.Enabled = true;
            }
            else
            {
                buttonForward.Enabled = false;
            }

            if(id > 1)
            {
                buttonPrevious.Enabled = true;
            }
            else
            {
                buttonPrevious.Enabled = false;
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            previousArticle();
            if (id > 1)
            {
                buttonPrevious.Enabled = true;
            }
            else
            {
                buttonPrevious.Enabled = false;
            }

            if (id <= articleManager.getArticlesCount())
            {
                buttonForward.Enabled = true;
            }
            else
            {
                buttonForward.Enabled = false;
            }
        }

        private void getArticlesCount()
        {
            countArticles = articleManager.getArticlesCount();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBoxId.Text = selectedId.ToString();
            id = selectedId;
            Article article = articleManager.getArticleById(selectedId);

            LoadImageFromUrl(article.imageURL);
            labelId.Text = article.id.ToString();
            labelCode.Text = article.code;
            labelName.Text = article.name;
            labelDescription.Text = article.description;
            labelBrand.Text = article.brand;
            labelCategory.Text = article.category;
            labelPrice.Text = article.price.ToString();
            checkButtonsEnabled();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this article?", "Delete Article", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    articleManager.deleteArticle(int.Parse(textBoxId.Text));
                    MessageBox.Show("Article deleted successfully!");
                    fillDataTableOnlyValids();
                    getArticlesCount();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxId.Text))
            {
                MessageBox.Show("Please select an article to edit.");
                return;
            }

            FormUpdateArticle formUpdateArticle = new FormUpdateArticle(int.Parse(textBoxId.Text), this);
            formUpdateArticle.ShowDialog();
            fillDataTableOnlyValids();
        }

        private void fillDataTableSearch()
        {
            ds = new DataSet();
            table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Code");
            table.Columns.Add("Name");
            table.Columns.Add("Description");
            table.Columns.Add("Brand");
            table.Columns.Add("Category");
            table.Columns.Add("Price");
            table.Columns.Add("URL Image");
            table.Columns.Add("Valid");
            List<Article> articles = articleManager.getArticlesFiltered(textBoxSearch.Text);
            foreach (Article article in articles)
            {
                table.Rows.Add(article.id, article.code, article.name, article.description, article.brand, article.category, article.price, article.imageURL, article.valid);
            }
            ds.Tables.Add(table);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fillDataTableSearch();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            fillDataTableOnlyValids();
        }

        private void checkBox1_ControlAdded(object sender, ControlEventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                fillDataTable();
            }
            else
            {
                fillDataTableOnlyValids();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkButtonsEnabled();
        }

        private void checkButtonsEnabled()
        {
            if (id > 1)
            {
                buttonPrevious.Enabled = true;
            }
            else
            {
                buttonPrevious.Enabled = false;
            }

            if (id < articleManager.getArticlesCount())
            {
                buttonForward.Enabled = true;
            }
            else
            {
                buttonForward.Enabled = false;
            }
        }
    }
}
